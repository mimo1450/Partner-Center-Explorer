// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using StackExchange.Redis;
using System;

namespace Microsoft.Store.PartnerCenter.Samples.Common.Cache
{
    public class RedisCacheManager : ICacheManager
    {
        private readonly IDatabase _cache;

        private static readonly Lazy<ConnectionMultiplexer> _connection =
            new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(AppConfig.RedisConnection));

        public RedisCacheManager()
        {
            _cache = Connection.GetDatabase();
        }

        public bool KeyExists(string key)
        {
            return _cache.KeyExists(key);
        }

        public string Read(string key)
        {
            return _cache.StringGet(key);
        }

        public void Delete(string key)
        {
            _cache.KeyDelete(key);
        }

        public void Write(string key, string value)
        {
            _cache.StringSet(key, value);
        }

        public void Write(string key, string value, TimeSpan expires)
        {
            _cache.StringSet(key, value, expires);
        }

        private static ConnectionMultiplexer Connection => _connection.Value;
    }
}