// -----------------------------------------------------------------------
// <copyright file="TokenManagement.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Explorer.Logic.Authentication
{
    using Cache;
    using Configuration;
    using IdentityModel.Clients.ActiveDirectory;
    using Models;
    using Newtonsoft.Json;
    using PartnerCenter.Extensions;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides functionality to obtain access tokens.
    /// </summary>
    public class TokenManagement : ITokenManagement
    {
        /// <summary>
        /// Gets an access token from the authority using app only authentication.
        /// </summary>
        /// <param name="authority">Address of the authority to issue the token.</param>
        /// <param name="resource">Identifier of the target resource that is the recipent of the requested token.</param>
        /// <returns>An instnace of <see cref="AuthenticationResult"/> that represented the access token.</returns>
        /// <exception cref="ArgumentException">
        /// authority
        /// or
        /// resource
        /// </exception>
        public AuthenticationResult GetAppOnlyToken(string authority, string resource)
        {
            authority.AssertNotEmpty(nameof(authority));
            resource.AssertNotEmpty(nameof(resource));

            return SynchronousExecute(() => GetAppOnlyTokenAsync(authority, resource));
        }

        /// <summary>
        /// Gets an access token from the authority using app only authentication.
        /// </summary>
        /// <param name="authority">Address of the authority to issue the token.</param>
        /// <param name="resource">Identifier of the target resource that is the recipent of the requested token.</param>
        /// <returns>An instnace of <see cref="AuthenticationResult"/> that represented the access token.</returns>
        /// <exception cref="ArgumentException">
        /// authority
        /// or
        /// resource
        /// </exception>
        public async Task<AuthenticationResult> GetAppOnlyTokenAsync(string authority, string resource)
        {
            AuthenticationContext authContext;
            DistributedTokenCache tokenCache;

            authority.AssertNotEmpty(nameof(authority));
            resource.AssertNotEmpty(nameof(resource));

            try
            {
                // If the Redis Cache connection string is not populated then utilize the constructor
                // that only requires the authority. That constructor will utilize a in-memory caching
                // feature that is built-in into ADAL.
                if (string.IsNullOrEmpty(ApplicationConfiguration.RedisConnection))
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
                        ApplicationConfiguration.ApplicationId,
                        ApplicationConfiguration.ApplicationSecret));
            }
            finally
            {
                authContext = null;
                tokenCache = null;
            }
        }

        /// <summary>
        /// Gets an access token from the authority using app + user authentication.
        /// </summary>
        /// <param name="authority">Address of the authority to issue the token.</param>
        /// <param name="resource">Identifier of the target resource that is the recipent of the requested token.</param>
        /// <param name="token">Assertion token representing the user.</param>
        /// <returns>An instnace of <see cref="AuthenticationResult"/> that represented the access token.</returns>
        /// <exception cref="ArgumentException">
        /// authority
        /// or
        /// resource
        /// or
        /// token
        /// </exception>
        public AuthenticationResult GetAppPlusUserToken(string authority, string resource, string token)
        {
            authority.AssertNotEmpty(nameof(authority));
            resource.AssertNotEmpty(nameof(resource));
            token.AssertNotEmpty(nameof(token));

            return SynchronousExecute(() => GetAppPlusUserTokenAsync(authority, resource, token));
        }

        /// <summary>
        /// Gets an access token from the authority using app + user authentication.
        /// </summary>
        /// <param name="authority">Address of the authority to issue the token.</param>
        /// <param name="resource">Identifier of the target resource that is the recipent of the requested token.</param>
        /// <param name="token">Assertion token represting the user.</param>
        /// <returns>An instnace of <see cref="AuthenticationResult"/> that represented the access token.</returns>
        /// <exception cref="ArgumentException">
        /// authority
        /// or
        /// resource
        /// or 
        /// token
        /// </exception>
        public async Task<AuthenticationResult> GetAppPlusUserTokenAsync(string authority, string resource, string token)
        {
            AuthenticationContext authContext;
            DistributedTokenCache tokenCache;

            authority.AssertNotEmpty(nameof(authority));
            resource.AssertNotEmpty(nameof(resource));
            token.AssertNotEmpty(nameof(token));

            try
            {
                if (string.IsNullOrEmpty(ApplicationConfiguration.RedisConnection))
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
                        ApplicationConfiguration.ApplicationId,
                        ApplicationConfiguration.ApplicationSecret),
                    new UserAssertion(token, "urn:ietf:params:oauth:grant-type:jwt-bearer"));
            }
            finally
            {
                authContext = null;
                tokenCache = null;
            }
        }

        /// <summary>
        /// Gets an instance of <see cref="IPartnerCredentials"/> used to access the Partner Center Managed API.
        /// </summary>
        /// <param name="authority">Address of the authority to issue the token.</param>
        /// <returns>
        /// An instance of <see cref="IPartnerCredentials" /> that represents the access token.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// authority
        /// </exception>
        /// <remarks>This function will use app only authentication to obtain the credentials.</remarks>
        public IPartnerCredentials GetPartnerCenterAppOnlyCredentials(string authority)
        {
            authority.AssertNotEmpty(nameof(authority));

            return SynchronousExecute(() => GetPartnerCenterAppOnlyCredentialsAsync(authority));
        }

        /// <summary>
        /// Gets an instance of <see cref="IPartnerCredentials"/> used to access the Partner Center Managed API.
        /// </summary>
        /// <param name="authority">Address of the authority to issue the token.</param>
        /// <returns>
        /// An instance of <see cref="IPartnerCredentials" /> that represents the access token.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// authority
        /// </exception>
        /// <remarks>This function will use app only authentication to obtain the credentials.</remarks>
        public async Task<IPartnerCredentials> GetPartnerCenterAppOnlyCredentialsAsync(string authority)
        {
            AuthenticationResult authResult;
            IPartnerCredentials credentials;
            string key;

            authority.AssertNotEmpty(nameof(authority));

            try
            {
                key = "Resource::AppOnly::PartnerCenter";

                if (CacheManager.Instance.Exists(key))
                {
                    credentials = JsonConvert.DeserializeObject<PartnerCenterTokenModel>(
                        CacheManager.Instance.Read(key));

                    if (!credentials.IsExpired())
                    {
                        return credentials;
                    }
                }

                authResult = await GetAppOnlyTokenAsync(
                    authority,
                    ApplicationConfiguration.PartnerCenterEndpoint);

                credentials = await PartnerCredentials.Instance.GenerateByApplicationCredentialsAsync(
                    ApplicationConfiguration.PartnerCenterApplicationId,
                    ApplicationConfiguration.PartnerCenterApplicationSecret,
                    ApplicationConfiguration.AccountId);

                CacheManager.Instance.Write(key, JsonConvert.SerializeObject(credentials));

                return credentials;
            }
            finally
            {
                authResult = null;
            }
        }

        /// <summary>
        /// Gets an instance of <see cref="IPartnerCredentials"/> used to access the Partner Center Managed API.
        /// </summary>
        /// <param name="authority">Address of the authority to issue the token.</param>
        /// <param name="token">Assertion token that represents the user.</param>
        /// <returns>
        /// An instance of <see cref="IPartnerCredentials" /> that represents the access token.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// authority
        /// or
        /// token
        /// </exception>
        /// <remarks>This function will use app only authentication to obtain the credentials.</remarks>
        public IPartnerCredentials GetPartnerCenterAppPlusUserCredentials(string authority, string token)
        {
            authority.AssertNotEmpty(nameof(authority));
            token.AssertNotEmpty(nameof(token));

            return SynchronousExecute(() => GetPartnerCenterAppPlusUserCredentialsAsync(authority, token));
        }

        /// <summary>
        /// Gets an instance of <see cref="IPartnerCredentials"/> used to access the Partner Center Managed API.
        /// </summary>
        /// <param name="authority">Address of the authority to issue the token.</param>
        /// <param name="token">Assertion token that represents the user.</param>
        /// <returns>
        /// An instance of <see cref="IPartnerCredentials" /> that represents the access token.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// authority
        /// or
        /// token
        /// </exception>
        /// <remarks>This function will use app only authentication to obtain the credentials.</remarks>
        public async Task<IPartnerCredentials> GetPartnerCenterAppPlusUserCredentialsAsync(string authority, string token)
        {
            AuthenticationResult authResult;
            IPartnerCredentials credentials;
            string key;

            authority.AssertNotEmpty(nameof(authority));

            try
            {
                // TODO - Construct this value!
                key = string.Empty;

                if (CacheManager.Instance.Exists(key))
                {
                    credentials = JsonConvert.DeserializeObject<PartnerCenterTokenModel>(
                        CacheManager.Instance.Read(key));

                    if (!credentials.IsExpired())
                    {
                        return credentials;
                    }
                }

                authResult = await GetAppPlusUserTokenAsync(
                    authority,
                    ApplicationConfiguration.PartnerCenterEndpoint,
                    token);

                credentials = await PartnerCredentials.Instance.GenerateByUserCredentialsAsync(
                    ApplicationConfiguration.PartnerCenterApplicationId,
                    new AuthenticationToken(
                        authResult.AccessToken,
                        authResult.ExpiresOn));

                CacheManager.Instance.Write(key, JsonConvert.SerializeObject(credentials));

                return credentials;
            }
            finally
            {
                authResult = null;
            }
        }

        /// <summary>
        /// Gets an access token utilizing an authorization code. 
        /// </summary>
        /// <param name="authority">Address of the authority to issue the token.</param>
        /// <param name="code">Authorization code received from the service authorization endpoint.</param>
        /// <param name="resource">Identifier of the target resource that is the recipent of the requested token.</param>
        /// <param name="redirectUri">Redirect URI used for obtain the authorization code.</param>
        /// <returns>An instnace of <see cref="AuthenticationResult"/> that represented the access token.</returns>
        /// <exception cref="ArgumentException">
        /// authority
        /// or
        /// code
        /// or
        /// resource
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// redirectUri
        /// </exception>
        public AuthenticationResult GetTokenByAuthorizationCode(string authority, string code, string resource, Uri redirectUri)
        {
            authority.AssertNotEmpty(nameof(authority));
            code.AssertNotEmpty(nameof(code));
            redirectUri.AssertNotNull(nameof(redirectUri));
            resource.AssertNotEmpty(nameof(resource));

            return SynchronousExecute(() => GetTokenByAuthorizationCodeAsync(authority, code, resource, redirectUri));

        }

        /// <summary>
        /// Gets an access token utilizing an authorization code. 
        /// </summary>
        /// <param name="authority">Address of the authority to issue the token.</param>
        /// <param name="code">Authorization code received from the service authorization endpoint.</param>
        /// <param name="resource">Identifier of the target resource that is the recipent of the requested token.</param>
        /// <param name="redirectUri">Redirect URI used for obtain the authorization code.</param>
        /// <returns>An instnace of <see cref="AuthenticationResult"/> that represented the access token.</returns>
        /// <exception cref="ArgumentException">
        /// authority
        /// or
        /// code
        /// or
        /// resource
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// redirectUri
        /// </exception>
        public async Task<AuthenticationResult> GetTokenByAuthorizationCodeAsync(string authority, string code, string resource, Uri redirectUri)
        {
            AuthenticationContext authContext;
            DistributedTokenCache tokenCache;

            authority.AssertNotEmpty(nameof(authority));
            code.AssertNotEmpty(nameof(code));
            redirectUri.AssertNotNull(nameof(redirectUri));
            resource.AssertNotEmpty(nameof(resource));

            try
            {
                // If the Redis Cache connection string is not populated then utilize the constructor
                // that only requires the authority. That constructor will utilize a in-memory caching
                // feature that is built-in into ADAL.
                if (string.IsNullOrEmpty(ApplicationConfiguration.RedisConnection))
                {
                    authContext = new AuthenticationContext(authority);
                }
                else
                {
                    tokenCache = new DistributedTokenCache(resource);
                    authContext = new AuthenticationContext(authority, tokenCache);
                }

                return await authContext.AcquireTokenByAuthorizationCodeAsync(code,
                    redirectUri,
                    new ClientCredential(
                        ApplicationConfiguration.ApplicationId,
                        ApplicationConfiguration.ApplicationSecret),
                    resource);
            }
            finally
            {
                authContext = null;
                tokenCache = null;
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