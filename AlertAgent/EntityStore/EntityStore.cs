using DayTrade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AlertAgent
{
    class EntityStore
    {
        const string connectionString = "Data Source=daytrade.database.windows.net;Initial Catalog=DayTrade;Integrated Security=False;User ID=Guess;Password=RippleRules2018;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Application Name=BinanceConsoleApp";

        internal List<AlertSubscriptionEntity> GetAllActiveSubscriptions()
        {
            using (var ctx = CreateContext())
            {
                return ctx.AlertSubscriptions
                    .Include("AlertSubscriber")
                    .Where(a => a.Active)
                    .ToList()
                    ;
            }
        }

        public EntityStore()
        {
        }

        private DayTradeContext CreateContext()
        { 
            var context = new DayTradeContext(connectionString);
            context.Configuration.AutoDetectChangesEnabled = false;
            return context;
        }

        public List<CandlestickEntity> GetLastCandlestick(string symbol, int intervalInMinutes, int count)
        {
            using (var ctx = CreateContext())
            {
                return CandlestickSeries.GetListByCount(ctx, symbol, intervalInMinutes, count);
            }
        }
    }
}
