﻿using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Binance.Api
{
    /// <summary>
    /// Binance API user <see cref="IBinanceApiUser"/> implementation.
    /// </summary>
    public sealed class BinanceApiUser : IBinanceApiUser
    {
        #region Public Properties

        public string ApiKey { get; }

        public IApiRateLimiter RateLimiter { get; set; }

        #endregion Public Properties

        #region Private Fields

        private readonly HMAC _hmac;

        private readonly object _sync = new object();

        private readonly BinanceApiOptions _options;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Construct an <see cref="IBinanceApiUser"/> instance providing an API
        /// key and optional API secret. The API secret is not required for 
        /// the user stream methods, but is required for other account methods.
        /// </summary>
        /// <param name="apiKey">The user's API key.</param>
        /// <param name="apiSecret">The user's API secret (optional, but required for signing).</param>
        /// <param name="rateLimiter">The rate limiter (auto-configured).</param>
        /// <param name="options">The JSON API options.</param>
        public BinanceApiUser(string apiKey, string apiSecret = null, IApiRateLimiter rateLimiter = null, IOptions<BinanceApiOptions> options = null)
        {
            Throw.IfNullOrWhiteSpace(apiKey, nameof(apiKey));

            ApiKey = apiKey;

            if (!string.IsNullOrWhiteSpace(apiSecret))
            {
                _hmac = new HMACSHA256(Encoding.UTF8.GetBytes(apiSecret));
            }

            RateLimiter = rateLimiter;
            _options = options?.Value ?? new BinanceApiOptions();

            // Configure order rate limiter.
            RateLimiter?.Configure(TimeSpan.FromDays(_options.OrderRateLimit.DurationDays), _options.OrderRateLimit.Count);
            // Configure order burst rate limiter.
            RateLimiter?.Configure(TimeSpan.FromSeconds(_options.OrderRateLimit.BurstDurationSeconds), _options.OrderRateLimit.BurstCount);
        }

        #endregion Constructors

        #region Public Methods

        public string Sign(string totalParams)
        {
            Throw.IfNullOrWhiteSpace(totalParams, nameof(totalParams));

            if (_hmac == null)
                throw new InvalidOperationException($"{nameof(BinanceApiUser)}.{nameof(Sign)} requires the user's API secret.");

            byte[] hash;
            lock (_sync)
            {
                hash = _hmac.ComputeHash(Encoding.UTF8.GetBytes(totalParams));
            }

            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }

        public override string ToString()
        {
            return ApiKey;
        }

        #endregion Public Methods

        #region IDisposable

        private bool _disposed;

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _hmac?.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}