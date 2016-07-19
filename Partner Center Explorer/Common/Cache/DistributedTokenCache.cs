// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Security.Claims;

namespace Microsoft.Store.PartnerCenter.Samples.Common.Cache
{
    /// <summary>
    /// Token cache for Azure AD tokens obtained using ADAL.
    /// </summary>
    /// <seealso cref="Microsoft.IdentityModel.Clients.ActiveDirectory.TokenCache" />
    public class DistributedTokenCache : TokenCache
    {
        private static readonly Lazy<ConnectionMultiplexer> _connection =
            new Lazy<ConnectionMultiplexer>(() => { return ConnectionMultiplexer.Connect(AppConfig.RedisConnection); });

        private readonly IDatabase _cache;
        private readonly MachineKeyDataProtector _protector;
        private readonly string _resource;

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributedTokenCache"/> class.
        /// </summary>
        /// <param name="resource">The resource being accessed.</param>
        public DistributedTokenCache(string resource)
        {
            _cache = Connection.GetDatabase();
            _protector = new MachineKeyDataProtector(new[] { typeof(DistributedTokenCache).FullName });
            _resource = resource;

            AfterAccess = AfterAccessNotification;
            BeforeAccess = BeforeAccessNotification;
        }

        private static ConnectionMultiplexer Connection => _connection.Value;

        private string Key =>
            $"Resource:{_resource}::UserId:{ClaimsPrincipal.Current.Identities.First().FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value}";

        /// <summary>
        /// Notification method called after any library method accesses the cache.
        /// </summary>
        /// <param name="args">Contains parameters used by the ADAL call accessing the cache.</param>
        public void AfterAccessNotification(TokenCacheNotificationArgs args)
        {
            if (!HasStateChanged)
            {
                return;
            }

            if (Count > 0)
            {
                _cache.StringSet(Key, Convert.ToBase64String(_protector.Protect(Serialize())));
            }
            else
            {
                _cache.KeyDelete(Key);
            }

            HasStateChanged = false;
        }

        /// <summary>
        /// Notification method called before any library method accesses the cache.
        /// </summary>
        /// <param name="args">Contains parameters used by the ADAL call accessing the cache.</param>
        public void BeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            byte[] data;

            try
            {
                if (!_cache.KeyExists(Key))
                {
                    return;
                }

                data = Convert.FromBase64String(_cache.StringGet(Key));

                if (data != null)
                {
                    Deserialize(_protector.Unprotect(data));
                }
            }
            finally
            {
                data = null;
            }
        }

        /// <summary>
        /// Clears the cache by deleting all the items. Note that if the cache is the default shared cache, clearing it would
        /// impact all the instances of <see cref="T:Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext" /> which share that cache.
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            _cache.KeyDelete(Key);
        }

        /// <summary>
        /// Deletes an item from the cache.
        /// </summary>
        /// <param name="item">The item to delete from the cache</param>
        public override void DeleteItem(TokenCacheItem item)
        {
            base.DeleteItem(item);
            _cache.KeyDelete(Key);
        }
    }
}