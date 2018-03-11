﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Binance;
using Binance.Application;

namespace BinanceConsoleApp.Controllers
{
    internal class GetOrderBook : IHandleCommand
    {
        public async Task<bool> HandleAsync(string command, CancellationToken token = default)
        {
            if (!command.StartsWith("depth ", StringComparison.OrdinalIgnoreCase) &&
                !command.Equals("depth", StringComparison.OrdinalIgnoreCase) &&
                !command.StartsWith("book ", StringComparison.OrdinalIgnoreCase) &&
                !command.Equals("book", StringComparison.OrdinalIgnoreCase))
                return false;

            var args = command.Split(' ');

            string symbol = Symbol.BTC_USDT;
            var limit = 10;

            if (args.Length > 1)
            {
                if (!int.TryParse(args[1], out limit))
                {
                    symbol = args[1];
                    limit = 10;
                }
            }

            if (args.Length > 2)
            {
                int.TryParse(args[2], out limit);
            }

            var orderBook = await Program.Api.GetOrderBookAsync(symbol, limit, token);

            lock (Program.ConsoleSync)
            {
                Console.WriteLine();
                orderBook.Print(Console.Out, limit);
                Console.WriteLine();
            }

            return true;
        }
    }
}
