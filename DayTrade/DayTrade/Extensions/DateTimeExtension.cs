using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayTrade.Extensions
{
    public static class DateTimeExtension
    {
        static DateTime begin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public static long ToCandlestickTimestamp(this DateTime dateTime)
        {
            return Convert.ToInt64((dateTime - begin).TotalMilliseconds);
        }
        public static DateTime ToCandlestickOpenDateTime(this DateTime dateTime)
        {
            return dateTime.AddMilliseconds(-1*dateTime.Millisecond).AddSeconds(-1*dateTime.Second);
        }
    }
}
