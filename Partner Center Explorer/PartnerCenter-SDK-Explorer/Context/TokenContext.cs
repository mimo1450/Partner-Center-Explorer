// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Store.PartnerCenter.Samples.Common;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Cache;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context
{
    /// <summary>
    /// Used to perform basic token operations.
    /// </summary>
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

                    return bootstrapContext.Token;
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
        /// <param name="resource">Identifier of the target resource that is the recipient of the requested token.</param>
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
                throw new ArgumentNullException("authority");
            }
            else if (string.IsNullOrEmpty(resource))
            {
                throw new ArgumentNullException("resource");
            }

            return SynchronousExecute(() => GetAADTokenAsync(authority, resource));
        }

        public static async Task<AuthenticationResult> GetAADTokenAsync(string authority, string resource)
        {
            AuthenticationContext authContext;
            DistributedTokenCache tokenCache;

            if (string.IsNullOrEmpty(authority))
            {
                throw new ArgumentNullException("authority");
            }
            else if (string.IsNullOrEmpty(resource))
            {
                throw new ArgumentNullException("resource");
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
                        AppConfig.ApplicationSecret
                    ),
                    new UserAssertion(UserAssertionToken, "urn:ietf:params:oauth:grant-type:jwt-bearer")
                );
            }
            finally
            {
                authContext = null;
                tokenCache = null;
            }
        }

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