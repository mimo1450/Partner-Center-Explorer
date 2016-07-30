// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Store.PartnerCenter.Samples.Common.Cache;
using Microsoft.Store.PartnerCenter.Samples.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.Store.PartnerCenter.Samples.Common
{
    public static class TokenContext
    {
        private static readonly ICacheManager _cache;
        private static readonly MachineKeyDataProtector _protector;

        /// <summary>
        /// Initializes the <see cref="TokenContext"/> class.
        /// </summary>
        static TokenContext()
        {
            _cache = CacheManager.Instance;
            _protector = new MachineKeyDataProtector(new[] { typeof(TokenContext).FullName });
        }

        /// <summary>
        /// Gets the token for the current autheticated user.
        /// </summary>
        /// <value>
        /// The token used authenticate the current user.
        /// </value>
        /// <remarks>This token is used to construct a user assertion in order to obtain tokens for specific resources.</remarks>
        public static string UserAssertionToken
        {
            get
            {
                System.IdentityModel.Tokens.BootstrapContext bootstrapContext;

                try
                {
                    bootstrapContext = ClaimsPrincipal.Current.Identities.First().BootstrapContext as System.IdentityModel.Tokens.BootstrapContext;

                    return bootstrapContext?.Token;
                }
                finally
                {
                    bootstrapContext = null;
                }
            }
        }

        /// <summary>
        /// Asynchronously obtains an Azure AD token for the specified resource.
        /// </summary>
        /// <param name="authority">Authority used to obtain the token.</param>
        /// <param name="resource">Identifier of the target resource that is the recipient of the requested token.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// authority
        /// or
        /// resource
        /// </exception>
        /// <remarks>This function obtains a token for the specified resource on behalf the current authenticated user.</remarks>
        public static async Task<AuthenticationResult> GetAADTokenAsync(string authority, string resource)
        {
            AuthenticationContext authContext;
            DistributedTokenCache tokenCache;

            if (string.IsNullOrEmpty(authority))
            {
                throw new ArgumentNullException(nameof(authority));
            }
            if (string.IsNullOrEmpty(resource))
            {
                throw new ArgumentNullException(nameof(resource));
            }

            try
            {
                if (string.IsNullOrEmpty(AppConfig.RedisConnection))
                {
                    authContext = new AuthenticationContext(authority);
                }
                else
                {
                    tokenCache = new DistributedTokenCache(resource);
                    authContext = new AuthenticationContext(authority, tokenCache);
                }

                return await authContext.AcquireTokenAsync(
                    resource,
                    new ClientCredential(
                        AppConfig.ApplicationId,
                        AppConfig.ApplicationSecret),
                    new UserAssertion(UserAssertionToken, "urn:ietf:params:oauth:grant-type:jwt-bearer"));
            }
            finally
            {
                authContext = null;
                tokenCache = null;
            }
        }

        /// <summary>
        /// Asynchronously obtains an Azure AD token for the specified resource.
        /// </summary>
        /// <param name="authority">Authority used to obtain the token.</param>
        /// <param name="resource">The resource for which the token should be obtained.</param>
        /// <param name="token">Token used to obtain another token.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// authority
        /// or
        /// resource
        /// or
        /// token
        /// </exception>
        public static async Task<AuthorizationToken> GetAADTokenAsync(string authority, string resource, string token)
        {
            AuthorizationToken authToken;
            Communication comm;
            HttpContent content;
            List<KeyValuePair<string, string>> values;


            if (string.IsNullOrEmpty(authority))
            {
                throw new ArgumentNullException(nameof(authority));
            }
            if (string.IsNullOrEmpty(resource))
            {
                throw new ArgumentNullException(nameof(resource));
            }
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            try
            {
                authToken = GetTokenFromCache(resource);

                if (authToken != null)
                {
                    return authToken;
                }

                comm = new Communication();
                values = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("assertion", token),
                    new KeyValuePair<string, string>("client_id", AppConfig.ApplicationId),
                    new KeyValuePair<string, string>("client_secret", AppConfig.ApplicationSecret),
                    new KeyValuePair<string, string>("grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer"),
                    new KeyValuePair<string, string>("requested_token_use", "on_behalf_of"),
                    new KeyValuePair<string, string>("resource", resource),
                    new KeyValuePair<string, string>("scope", "openid")
                };

                content = new FormUrlEncodedContent(values);

                authToken = await comm.PostAsync<AuthorizationToken>(authority, content);
                WriteTokenToCache(resource, authToken);

                return authToken;
            }
            finally
            {
                comm = null;
                content = null;
                values = null;
            }
        }

        /// <summary>
        /// Asynchronously obtains an Azure AD token for the specified resource.
        /// </summary>
        /// <param name="authority">Authority used to obtain the token.</param>
        /// <param name="resource">The resource for which the token should be obtained.</param>
        /// <param name="username">The username to be used to obtain the token.</param>
        /// <param name="password">The password to be used to obtain the token.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// authority
        /// or
        /// resource
        /// or
        /// username
        /// or
        /// password
        /// </exception>
        public static async Task<AuthorizationToken> GetAADTokenAsync(string authority, string resource, string username, string password)
        {
            AuthorizationToken authToken;
            Communication comm;
            HttpContent content;
            List<KeyValuePair<string, string>> values;

            if (string.IsNullOrEmpty(authority))
            {
                throw new ArgumentNullException(nameof(authority));
            }
            if (string.IsNullOrEmpty(resource))
            {
                throw new ArgumentNullException(nameof(resource));
            }
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            try
            {
                authToken = GetTokenFromCache(resource);

                if (authToken != null)
                {
                    return authToken;
                }

                comm = new Communication();
                values = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("resource", resource),
                    new KeyValuePair<string, string>("client_id", AppConfig.ApplicationId),
                    new KeyValuePair<string, string>("client_secret", AppConfig.ApplicationSecret),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("username", username)
                };

                content = new FormUrlEncodedContent(values);

                authToken = await comm.PostAsync<AuthorizationToken>(authority, content);
                WriteTokenToCache(resource, authToken);

                return authToken;
            }
            finally
            {
                content = null;
                values = null;
            }
        }

        private static string GetKey(string resource)
        {
            return $"Resource:{resource}::UserId:{ClaimsPrincipal.Current.Identities.First().FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value}";
        }

        private static AuthorizationToken GetTokenFromCache(string resource)
        {
            AuthorizationToken authToken;
            byte[] data;
            string token;

            try
            {
                if (!_cache.KeyExists(GetKey(resource)))
                {
                    return null;
                }

                data = _protector.Unprotect(Convert.FromBase64String(_cache.Read(GetKey(resource))));
                token = System.Text.Encoding.Default.GetString(data);
                authToken = JsonConvert.DeserializeObject<AuthorizationToken>(token);

                return !authToken.IsNearExpiry() ? authToken : null;

            }
            finally
            {
                data = null;
            }
        }

        private static void WriteTokenToCache(string resource, AuthorizationToken token)
        {
            byte[] data;
            string tokenData;

            try
            {
                tokenData = JsonConvert.SerializeObject(token);
                data = _protector.Protect(System.Text.Encoding.Default.GetBytes(tokenData));
                tokenData = Convert.ToBase64String(data);

                _cache.Write(GetKey(resource), tokenData, token.ExpiresOn.AddMinutes(-1).TimeOfDay);

            }
            finally
            {
                data = null;
            }
        }
    }
}