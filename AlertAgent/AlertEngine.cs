using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DayTrade;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace AlertAgent
{
    class AlertEngine
    {
        List<AlertSubscriptionEntity> Subscriptions;
        List<CandlestickEntity> candleSent = new List<CandlestickEntity>();

        private void RefreshSubscriptions()
        {
            Subscriptions = new EntityStore().GetAllActiveSubscriptions();
        }
        public void Run(DateTime startDateTime)
        {
            RefreshSubscriptions();

            var candlesToSend = new List<CandlestickEntity>();
            foreach (var symbol in Subscriptions.Select(s => s.Symbol).Distinct())
            {
                foreach (var subscription in Subscriptions.Where(s => s.Symbol == symbol))
                {
                    Console.WriteLine($"{symbol}: {subscription.Interval}");
                    var intervalInMinutes = new Interval(subscription.Interval).InMinutes();
                    var count = Math.Min(30, Convert.ToInt32((DateTime.Now - startDateTime).TotalMinutes)+1);
                    VerifySymbol(symbol, candlesToSend, intervalInMinutes, count, c=> Math.Abs(c.ClosePercent)>subscription.PercentChanged);
                    candlesToSend.RemoveAll(c =>
                                    candleSent.Exists(
                                        c1 => c1.Contains(c)
                                        )
                                    );
                }
            }
            SendCandles(candlesToSend);
            candleSent.AddRange(candlesToSend);

            Thread.Sleep(60000);
        }

        private static void SendCandles(List<CandlestickEntity> candlesToSend)
        {
            var msg = new StringBuilder();
            var header = true;
            foreach (var candle in candlesToSend)
            {
                if (header)
                {
                    header = false;
                    msg.AppendLine($"Sent on {DateTime.Now}");
                }
                msg.AppendLine($"{candle.Timestamp.ToLocalTime().ToShortTimeString()} Symbol={candle.Symbol}: Open={candle.Open:0.00000000}, Close={candle.Close:0.00000000} {candle.ClosePercent:00.0}% {candle.Interval}");
            }
            SendMsg(msg.ToString());
        }

        private static void VerifySymbol(string symbol, List<CandlestickEntity> candlesToSend, int intervalInMinutes, int count, Func<CandlestickEntity, bool> alertCondition)
        {
            var candles = new EntityStore().GetLastCandlestick(symbol, intervalInMinutes, count);
            var lastCandle = new DayTrade.CandlestickEntity();
            lastCandle = candles.Last();
            bool positiveSequence = lastCandle.Up;
            bool negativeSequence = !lastCandle.Up;
            for (int index = candles.Count - 1; index > 0; index--)
            {
                if (alertCondition(lastCandle)) candlesToSend.Add(lastCandle);
                if (candles[index].Up)
                {
                    positiveSequence = true;
                    if (negativeSequence)
                    {
                        negativeSequence = false;
                        if (alertCondition(lastCandle)) candlesToSend.Add(lastCandle);
                        lastCandle = candles[index];
                    }
                    else
                    {
                        lastCandle += candles[index];
                    }
                }
                else
                {
                    negativeSequence = true;
                    if (positiveSequence)
                    {
                        positiveSequence = false;
                        if (alertCondition(lastCandle)) candlesToSend.Add(lastCandle);
                        lastCandle = candles[index];
                    }
                    {
                        lastCandle += candles[index];
                    }
                }
            }
        }
        private static void SendMsg(string msg)
        {
            if (string.IsNullOrEmpty(msg)) return;

            TwilioClient.Init("AC1e23541e068caf41ddf02e36fd03c7ab", "110076dcf936a676a1ca6e9e6ebbd9fa");

            var sms = MessageResource.Create(new PhoneNumber("+14256154882"),
                                   from: new PhoneNumber("+14255980278"),
                                   body: msg
                                   );
            Console.WriteLine($"Msg sent on {DateTime.Now.ToLocalTime()} \n{sms.DateSent} : {msg}");
        }
    }
}
