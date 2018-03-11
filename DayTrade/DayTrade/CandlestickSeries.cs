using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayTrade
{
    public static class CandlestickSeries
    {
        public static List<CandlestickEntity> GetListByStartEndDate(DayTradeContext ctx, string symbol, int intervalInMinutes, DateTime startDate, DateTime endDate)
        {
            var startUTC = startDate.ToUniversalTime();
            var endUTC = endDate.ToUniversalTime();
            var dataset = ctx.Candlesticks
                    .Where(c => c.Symbol.Equals(symbol, StringComparison.InvariantCultureIgnoreCase)
                                && c.Timestamp >= startUTC
                                && c.Timestamp <= endUTC)
                    .OrderBy(c => c.Timestamp)
                    .ToList()
                    ;
            return ConvertCandlestickList(dataset, intervalInMinutes);
        }
        public static List<CandlestickEntity> GetListByCount(DayTradeContext ctx, string symbol, int intervalInMinutes, int count)
        {
            var dataset = ctx.Candlesticks
                    .Where(c => c.Symbol.Equals(symbol, StringComparison.InvariantCultureIgnoreCase))
                    .OrderByDescending(c => c.Timestamp)
                    .Take(count * intervalInMinutes)
                    .ToList()
                    ;
            return ConvertCandlestickList(dataset, intervalInMinutes, count);
        }
        private static List<CandlestickEntity> ConvertCandlestickList(List<CandlestickEntity> dataset, int intervalInMinutes, int count=-1)
        {
            if (intervalInMinutes == 1) return dataset;

            var result = count==-1? new List<CandlestickEntity> () : new List<CandlestickEntity>(count);
            CandlestickEntity current = null;
            int index = 0;
            foreach (var candle in dataset)
            {
                if ((index % intervalInMinutes) == 0)
                {
                    if (current != null)
                    {
                        result.Add(current);
                    }
                    current = candle;
                }
                else
                {
                    current += candle;
                }
                index++;
            }
            result.Add(current);
            return result;
        }

    }
}
