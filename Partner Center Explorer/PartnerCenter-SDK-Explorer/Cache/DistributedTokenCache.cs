// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Store.PartnerCenter.Samples.Common;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Cache
{
    public class DistributedTokenCache : TokenCache
    {
        private IDatabase _cache;
        private DpapiDataProtector _protector;
        private string _resource;

        private static Lazy<ConnectionMultiplexer> _connection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(AppConfig.RedisConnection);
        });

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributedTokenCache"/> class.
        /// </summary>
        /// <param name="resource">The resource being accessed.</param>
        public DistributedTokenCache(string resource)
        {
            _cache = Connection.GetDatabase();
            _protector = new DpapiDataProtector(typeof(DistributedTokenCache).FullName, "TokenCache");
            _resource = resource;

            AfterAccess = AfterAccessNotification;
            LoadFromCache();
        }

        /// <summary>
        /// Afters the access notification.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void AfterAccessNotification(TokenCacheNotificationArgs args)
        {
            if (HasStateChanged)
            {
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
        }

        private static ConnectionMultiplexer Connection
        {
            get
            {
                return _connection.Value;
            }
        }

        private string Key
        {
            get
            {
                return string.Format(
                    "Resource:{0}::UserId:{1}",
                    _resource,
                    ClaimsPrincipal.Current.Identities.First().FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value
                );
            }
        }


        private void LoadFromCache()
        {
            byte[] data;

            try
            {
                if (_cache.KeyExists(Key))
                {
                    data = Convert.FromBase64String(_cache.StringGet(Key));

                    if (data != null)
                    {
                        Deserialize(_protector.Unprotect(data));
                    }
                }
            }
            finally
            {
                data = null;
            }
        }
    }
}