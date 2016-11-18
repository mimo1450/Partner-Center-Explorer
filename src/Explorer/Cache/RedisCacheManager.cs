// -----------------------------------------------------------------------
// <copyright file="RedisCacheManager.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Explorer.Cache
{
    using Configuration;
    using Logic;
    using StackExchange.Redis;
    using System;
    using System.Text;

    /// <summary>
    /// Represent the redis cache management strategy.
    /// </summary>
    /// <remarks>This stragtey utilizes the Azure Redis Cache service.</remarks>
    public class RedisCacheManager : ICacheManager
    {
        private readonly IDatabase _cache;
        private readonly MachineKeyDataProtector _protector;

        private static readonly Lazy<ConnectionMultiplexer> _connection =
            new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(ApplicationConfiguration.RedisConnection));

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheManager"/> class.
        /// </summary>
        public RedisCacheManager()
        {
            _cache = Connection.GetDatabase();
            _protector = new MachineKeyDataProtector(new[] { typeof(RedisCacheManager).FullName });
        }

        /// <summary>
        /// Determines whether the specified key exists or not.
        /// </summary>
        /// <param name="key">The key value to be checked.</param>
        /// <returns>
        /// <c>true</c> if the key exists; otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// key
        /// </exception>
        public bool Exists(string key)
        {
            key.AssertNotEmpty(nameof(key));

            return _cache.KeyExists(key);
        }

        /// <summary>
        /// Reads the value for the specified key.
        /// </summary>
        /// <param name="key">The key of the value.</param>
        /// <returns>The value associated with the specified key.</returns>
        /// <exception cref="ArgumentException">
        /// key
        /// </exception>
        public string Read(string key)
        {
            key.AssertNotEmpty(nameof(key));

            byte[] data = _protector.Unprotect(Convert.FromBase64String(_cache.StringGet(key)));
            return Encoding.ASCII.GetString(data);
        }

        /// <summary>
        /// Clears the cache by deleting all the items.
        /// </summary>
        public void Clear()
        {
            // TODO - Write the implementation for this!
        }

        /// <summary>
        /// Deletes the value with specified key.
        /// </summary>
        /// <param name="key">The key of the element to be deleted.</param>
        /// <exception cref="ArgumentException">
        /// key
        /// </exception>
        public void Delete(string key)
        {
            key.AssertNotEmpty(nameof(key));

            _cache.KeyDelete(key);
        }

        /// <summary>
        /// Writes the specified key value pair.
        /// </summary>
        /// <param name="key">The key value to be written.</param>
        /// <param name="value">The value to be associated with the specified key.</param>
        /// <exception cref="ArgumentException">
        /// key
        /// or
        /// value
        /// </exception>
        public void Write(string key, string value)
        {
            key.AssertNotEmpty(nameof(key));
            value.AssertNotEmpty(nameof(value));

            string data = Convert.ToBase64String(
                _protector.Protect(Encoding.ASCII.GetBytes(value)));
            _cache.StringSet(key, data);
        }

        /// <summary>
        /// Writes the specified key value pair.
        /// </summary>
        /// <param name="key">The key value to be written.</param>
        /// <param name="value">The value to be associated with the specified key.</param>
        /// <param name="expires">When the specified key expires.</param>
        /// <exception cref="ArgumentException">
        /// key
        /// or
        /// value
        /// </exception>
        public void Write(string key, string value, TimeSpan expires)
        {
            key.AssertNotEmpty(nameof(key));
            value.AssertNotEmpty(nameof(value));

            string data = Convert.ToBase64String(
                _protector.Protect(Encoding.ASCII.GetBytes(value)));
            _cache.StringSet(key, data, expires);
        }

        private static ConnectionMultiplexer Connection => _connection.Value;
    }
}