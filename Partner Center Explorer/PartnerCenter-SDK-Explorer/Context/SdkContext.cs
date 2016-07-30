// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Store.PartnerCenter.Extensions;
using Microsoft.Store.PartnerCenter.Samples.Common;
using Microsoft.Store.PartnerCenter.Samples.Common.Cache;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context
{
    /// <summary>
    /// Context class used to obtain base object to interact with the Partner Center SDK.
    /// </summary>
    public class SdkContext
    {
        private IAggregatePartner _partnerOperations;
        private readonly ICacheManager _cache;
        private readonly MachineKeyDataProtector _protector;

        /// <summary>
        /// Initializes a new instance of the <see cref="SdkContext"/> class.
        /// </summary>
        public SdkContext()
        {
            _cache = CacheManager.Instance;
            _protector = new MachineKeyDataProtector(new[] { typeof(DistributedTokenCache).FullName });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdkContext"/> class.
        /// </summary>
        /// <param name="cache">An instance of <see cref="ICacheManager"/> used for caching tokens.</param>
        public SdkContext(ICacheManager cache)
        {
            _cache = cache;
            _protector = new MachineKeyDataProtector(new[] { typeof(DistributedTokenCache).FullName });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdkContext"/> class.
        /// </summary>
        /// <param name="partnerOperations">The partner operations.</param>
        public SdkContext(IAggregatePartner partnerOperations)
        {
            _partnerOperations = partnerOperations;
        }

        /// <summary>
        /// Gets the an instance of <see cref="IAggregatePartner"/>.
        /// </summary>
        /// <value>
        /// An instnace of <see cref="IAggregatePartner"/> used by the SDK to perform operations.
        /// </value>
        /// <remarks>
        /// This property utilizes App + User authentication. Various operations with the Partner Center
        /// SDK require App + User authorization. More details regarding Partner Center authentication can be
        /// found at https://msdn.microsoft.com/en-us/library/partnercenter/mt634709.aspx
        /// </remarks>
        public async Task<IAggregatePartner> GetPartnerOperationsAysnc()
        {
            IPartnerCredentials credentials;

            try
            {
                if (_partnerOperations != null)
                {
                    return _partnerOperations;
                }

                credentials = await GetPartnerCenterTokenAsync();
                _partnerOperations = PartnerService.Instance.CreatePartnerOperations(credentials);

                return _partnerOperations;
            }
            finally
            {
                credentials = null;
            }
        }

        private async Task<IPartnerCredentials> GetPartnerCenterTokenAsync()
        {
            AuthenticationResult authResult;
            IPartnerCredentials credentials;
            byte[] data;
            string token;

            try
            {
                if (_cache.KeyExists(Key))
                {
                    data = _protector.Unprotect(Convert.FromBase64String(_cache.Read(Key)));
                    token = System.Text.Encoding.Default.GetString(data);
                    credentials = JsonConvert.DeserializeObject<PartnerCenterToken>(token);

                    if (!credentials.IsExpired())
                    {
                        return credentials;
                    }
                }

                authResult = await TokenContext.GetAADTokenAsync(
                    $"{AppConfig.Authority}/{AppConfig.AccountId}/oauth2",
                    AppConfig.PartnerCenterApiUri);

                credentials =
                    await PartnerCredentials.Instance.GenerateByUserCredentialsAsync(AppConfig.ApplicationId,
                        new AuthenticationToken(authResult.AccessToken, authResult.ExpiresOn));

                token = JsonConvert.SerializeObject(credentials);
                data = _protector.Protect(System.Text.Encoding.Default.GetBytes(token));
                token = Convert.ToBase64String(data);

                _cache.Write(Key, token, credentials.ExpiresAt.AddMinutes(-1).TimeOfDay);

                return credentials;
            }
            finally
            {
                authResult = null;
                data = null;
            }
        }

        private static string Key =>
            $"Resource:PartnerCenterAPI::UserId:{ClaimsPrincipal.Current.Identities.First().FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value}";
    }
}