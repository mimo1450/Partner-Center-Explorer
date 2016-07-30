// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Store.PartnerCenter.Samples.Common.Cache
{
    /// <summary>
    /// Used to access the appropriate cache management stragety.
    /// </summary>
    public static class CacheManager
    {
        private static readonly Dictionary<string, Func<ICacheManager>> Managers;
        private static readonly string Type;

        /// <summary>
        /// Initializes the <see cref="CacheManager"/> class.
        /// </summary>
        static CacheManager()
        {
            Managers = new Dictionary<string, Func<ICacheManager>>()
            {
                { "InMemory", () => new InMemoryCacheManager()},
                { "Redis", () => new RedisCacheManager()}
            };

            Type = string.IsNullOrEmpty(AppConfig.RedisConnection) ? "InMemory" : "Redis";
        }

        /// <summary>
        /// Gets an instance of <see cref="ICacheManager"/>.
        /// </summary>
        /// <value>
        /// An appropriate instnace of <see cref="ICacheManager"/>.
        /// </value>
        /// <remarks>
        /// This will return an instance of an object that impelements <see cref="ICacheManager"/>. 
        /// The Managers dictionary object contains a delegate that points to the constructor
        /// of an object that implements the interface.
        /// </remarks>
        public static ICacheManager Instance => Managers[Type].Invoke();
    }
}