// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Store.PartnerCenter.Samples.Common.Cache
{
    public class InMemoryCacheManager : ICacheManager
    {
        private static readonly Lazy<Dictionary<string, string>> _cache =
           new Lazy<Dictionary<string, string>>(() => new Dictionary<string, string>());

        public bool KeyExists(string key)
        {
            return Cache.ContainsKey(key);
        }

        public string Read(string key)
        {
            return Cache[key];
        }

        public void Delete(string key)
        {
            Cache.Remove(key);
        }

        public void Write(string key, string value)
        {
            Cache[key] = value;
        }

        public void Write(string key, string value, TimeSpan expires)
        {
            Cache[key] = value;
        }

        private static Dictionary<string, string> Cache => _cache.Value;
    }
}