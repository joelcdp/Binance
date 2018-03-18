﻿using System;

// ReSharper disable once CheckNamespace
namespace Binance
{
    public static class ClientOrderExtensions
    {
        /// <summary>
        /// Determine if client order is valid using cached symbol information.
        /// </summary>
        /// <param name="clientOrder"></param>
        /// <returns></returns>
        public static bool IsValid(this ClientOrder clientOrder)
        {
            Throw.IfNull(clientOrder, nameof(clientOrder));

            Symbol symbol = clientOrder.Symbol; // use implicit conversion.

            return symbol != null && symbol.IsValid(clientOrder);
        }

        /// <summary>
        /// Determine if client order is valid using cached symbol information.
        /// </summary>
        /// <param name="clientOrder"></param>
        /// <returns></returns>
        public static void Validate(this ClientOrder clientOrder)
        {
            Throw.IfNull(clientOrder, nameof(clientOrder));

            Symbol symbol = clientOrder.Symbol; // use implicit conversion.

            if (symbol == null)
                throw new ArgumentException($"The symbol ({clientOrder.Symbol}) is not recognized.", nameof(clientOrder.Symbol));

            symbol.Validate(clientOrder);
        }
    }
}
