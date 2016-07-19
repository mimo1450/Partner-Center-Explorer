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

namespace Microsoft.Store.PartnerCenter.Samples.Common.Context
{
    public class TokenContext
    {
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
        /// Obtain an Azure AD token for the specified resource.
        /// </summary>
        /// <param name="authority">Authority used to obtain the token.</param>
        /// <param name="resource">Resource for which the token should be obtained.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// authority
        /// or
        /// resource
        /// </exception>
        /// <remarks>This function obtains a token for the specified resource on behalf the current authenticated user.</remarks>
        public static AuthenticationResult GetAADToken(string authority, string resource)
        {
            if (string.IsNullOrEmpty(authority))
            {
                throw new ArgumentNullException(nameof(authority));
            }
            if (string.IsNullOrEmpty(resource))
            {
                throw new ArgumentNullException(nameof(resource));
            }

            return SynchronousExecute(() => GetAADTokenAsync(authority, resource));
        }

        /// <summary>
        /// Obtains an Azure AD token for the specified resource.
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
        public static AuthorizationToken GetAADToken(string authority, string resource, string token)
        {
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

            return SynchronousExecute(() => GetAADTokenAsync(authority, resource, token));
        }

        /// <summary>
        /// Asynchronously obtains an Azure AD token for the specified resource.
        /// </summary>
        /// <param name="authority">Authority used to obtain the token.</param>
        /// <param name="resource">Resource for which the token should be obtained.</param>
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
        public static AuthorizationToken GetAADToken(string authority, string resource, string username, string password)
        {
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

            return SynchronousExecute(() => GetAADTokenAsync(authority, resource, username, password));
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

                return await comm.PostAsync<AuthorizationToken>(authority, content);
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
            Communication comm;
            HttpContent content;
            List<KeyValuePair<string, string>> values;
            string requestUri;

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

                requestUri = authority;

                return await comm.PostAsync<AuthorizationToken>(requestUri, content);
            }
            finally
            {
                content = null;
                values = null;
            }
        }

        /// <summary>
        /// Synchronously executes an asynchronous function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
        private static T SynchronousExecute<T>(Func<Task<T>> operation)
        {
            try
            {
                return Task.Run(async () => await operation()).Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}