// -----------------------------------------------------------------------
// <copyright file="ActiveDirectory.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Explorer.Logic.Azure
{
    using Authentication;
    using Configuration;
    using Microsoft.Azure.ActiveDirectory.GraphClient;
    using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ActiveDirectory : IActiveDirectory
    {
        IActiveDirectoryClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveDirectory"/> class.
        /// </summary>
        /// <param name="tenantId">The tenant identifer for the customer.</param>
        /// <exception cref="ArgumentException">
        /// tenantId
        /// </exception>
        public ActiveDirectory(string tenantId)
        {
            ITokenManagement tokenMgmt;
            string token;

            tenantId.AssertNotEmpty(nameof(tenantId));

            try
            {
                tokenMgmt = new TokenManagement();
                token = tokenMgmt.GetAppOnlyToken(
                    $"{ApplicationConfiguration.ActiveDirectoryEndpoint}/{tenantId}",
                    ApplicationConfiguration.ActiveDirectoryGraphEndpoint).AccessToken;

                _client = new ActiveDirectoryClient(
                    new Uri(new Uri(ApplicationConfiguration.ActiveDirectoryGraphEndpoint), tenantId),
                    async () => await Task.FromResult(token));
            }
            finally
            {
                tokenMgmt = null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveDirectory"/> class.
        /// </summary>
        /// <param name="tenantId">The tenant identifer for the customer.</param>
        /// <param name="assertionToken">A valid JSON Web Token (JWT).</param>
        /// <exception cref="ArgumentException">
        /// tenantId
        /// or
        /// assertionToken
        /// </exception>
        public ActiveDirectory(string tenantId, string assertionToken)
        {
            ITokenManagement tokenMgmt;
            string token;

            tenantId.AssertNotEmpty(nameof(tenantId));
            assertionToken.AssertNotEmpty(nameof(assertionToken));

            try
            {
                tokenMgmt = new TokenManagement();
                token = tokenMgmt.GetAppPlusUserToken(
                    $"{ApplicationConfiguration.ActiveDirectoryEndpoint}/{tenantId}",
                    ApplicationConfiguration.ActiveDirectoryGraphEndpoint,
                    assertionToken).AccessToken;

                _client = new ActiveDirectoryClient(
                    new Uri(new Uri(ApplicationConfiguration.ActiveDirectoryGraphEndpoint), tenantId),
                    async () => await Task.FromResult(token));
            }
            finally
            {
                tokenMgmt = null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveDirectory"/> class.
        /// </summary>
        /// <param name="tenantId">The tenant identifer for the customer.</param>
        /// <param name="code">Authorization code received from the service authorization endpoint.</param>
        /// <param name="redirectUri">Redirect URI used for obtain the authorization code.</param>
        /// <exception cref="ArgumentException">
        /// tenantId
        /// or
        /// code
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// redirectUri
        /// </exception>
        public ActiveDirectory(string tenantId, string code, Uri redirectUri)
        {
            ITokenManagement tokenMgmt;
            string token;

            tenantId.AssertNotEmpty(nameof(tenantId));
            code.AssertNotEmpty(nameof(code));
            redirectUri.AssertNotNull(nameof(redirectUri));

            try
            {
                tokenMgmt = new TokenManagement();
                token = tokenMgmt.GetTokenByAuthorizationCode(
                    $"{ApplicationConfiguration.ActiveDirectoryEndpoint}/{tenantId}",
                    code,
                    ApplicationConfiguration.ActiveDirectoryGraphEndpoint,
                    redirectUri).AccessToken;

                _client = new ActiveDirectoryClient(
                    new Uri(new Uri(ApplicationConfiguration.ActiveDirectoryGraphEndpoint), tenantId),
                    async () => await Task.FromResult(token));
            }
            finally
            {
                tokenMgmt = null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveDirectory"/> class.
        /// </summary>
        /// <param name="client">An instance of <see cref="IActiveDirectoryClient"/>.</param>
        public ActiveDirectory(IActiveDirectoryClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Obtains a list of directory roles that the specified directory is associated with.
        /// </summary>
        /// <param name="objectId">Object identifier for the object to be checked.</param>
        /// <returns>A list of directory that the specified object identifier is associated with.</returns>
        /// <exception cref="ArgumentException">
        /// objectId
        /// </exception>
        public async Task<List<IDirectoryRole>> GetDirectoryRolesAsync(string objectId)
        {
            DirectoryRole role;
            IPagedCollection<IDirectoryObject> roles;
            List<IDirectoryRole> values;

            objectId.AssertNotEmpty(objectId);

            try
            {
                roles = await _client.Users.GetByObjectId(objectId).MemberOf.ExecuteAsync();
                values = new List<IDirectoryRole>();

                do
                {
                    foreach (IDirectoryObject membership in roles.CurrentPage)
                    {
                        role = membership as DirectoryRole;

                        if (role != null)
                        {
                            values.Add(role);
                        }
                    }

                    roles = await roles.GetNextPageAsync();
                }
                while (roles != null && roles.MorePagesAvailable);

                return values;
            }
            finally
            {
                role = null;
                roles = null;
            }
        }

        /// <summary>
        /// Obtains a list service configuration records for all domains that are associated with the tenant.
        /// </summary>
        /// <returns>A list of service configuration records for all domains associated with the tenant.</returns>
        public async Task<List<IDomainDnsRecord>> GetDomainDnsRecordsAsync()
        {
            IPagedCollection<IDomainDnsRecord> records;
            List<IDomainDnsRecord> models;

            try
            {
                records = await _client.DomainDnsRecords.ExecuteAsync();
                models = new List<IDomainDnsRecord>();

                do
                {
                    foreach (IDomainDnsRecord record in records.CurrentPage)
                    {
                        models.Add(new DomainDnsRecord()
                        {
                            Label = record.Label,
                            RecordType = record.RecordType,
                            Ttl = record.Ttl
                        });
                    }

                    records = await records.GetNextPageAsync();
                }
                while (records != null && records.MorePagesAvailable);

                return models;
            }
            finally
            {
                records = null;
            }
        }
    }
}