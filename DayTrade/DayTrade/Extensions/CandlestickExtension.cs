
using System;
using System.Diagnostics;

namespace DayTrade
{
    public partial class CandlestickEntity
    {

        private static decimal Percent(decimal oldValue, decimal newValue)
        {
            return (newValue - oldValue) / oldValue * 100;
        }

        public void CalculatePercents()
        {
            ClosePercent = Percent(Open, Close);
            LowPercent = Percent(Open, Low);
            HighPercent = Percent(Open, High);
        }

        public void InitializeFrom(CandlestickEntity candle)
        {
            CreatedDate = candle.CreatedDate;
            Id = candle.Id;
            Symbol = candle.Symbol;
            Open = candle.Open;
            Interval = candle.Interval;
            Close = candle.Close;
            Low = candle.Low;
            High = candle.High;
            ClosePercent = candle.ClosePercent;
            LowPercent = candle.LowPercent;
            HighPercent = candle.HighPercent;
            Provider = candle.Provider;
            Timestamp = candle.Timestamp;
            Up = candle.Up;
            NumberOfTrades = candle.NumberOfTrades;
            Volume = candle.Volume;
        }

        private void Accumulate( CandlestickEntity newCandle)
        {
            //Debug.Assert(Up == newCandle.Up);
            Interval = DetermineInterval(Interval, newCandle.Interval);
            Close = newCandle.Close;
            Low = Math.Min(Low, newCandle.Low);
            High = Math.Max(High, newCandle.High);
            NumberOfTrades += newCandle.NumberOfTrades;
            Volume += newCandle.Volume;
            CalculatePercents();
        }

        private string DetermineInterval(string oldInterval, string newInterval)
        {
            var oldInt = new Interval(oldInterval);
            var newInt = new Interval(newInterval);

            return (oldInt + newInt).ToString();
        }


        public static CandlestickEntity operator + (CandlestickEntity a, CandlestickEntity b)
        {
            var result = new CandlestickEntity();
            result.InitializeFrom(a);
            result.Accumulate(b);
            return result;
        }

        public bool Contains(CandlestickEntity candle)
        {
            return Symbol == candle.Symbol &&
                Timestamp <= candle.Timestamp &&
                EndTimestamp >= candle.EndTimestamp;
        }

        public DateTime EndTimestamp
        {
            get
            {
                return Timestamp.AddMinutes((new Interval(Interval)).InMinutes());
            }
        }

    }
}
