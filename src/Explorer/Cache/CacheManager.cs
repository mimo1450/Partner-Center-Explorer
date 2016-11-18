// -----------------------------------------------------------------------
// <copyright file="CacheManager.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Explorer.Cache
{
    using Configuration;
    using System;
    using System.Collections.Generic;

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

            Type = string.IsNullOrEmpty(ApplicationConfiguration.RedisConnection) ? "InMemory" : "Redis";
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