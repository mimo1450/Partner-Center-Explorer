using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using Microsoft.Store.PartnerCenter.Samples.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context
{
    /// <summary>
    /// Represents an object used to interact with the Azure Management API.
    /// </summary>
    public class ResourceContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceContext"/> class.
        /// </summary>
        public ResourceContext()
        { }

        /// <summary>
        /// Gets a collection of resource groups belonging to the speicifed subscription and tenant.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns>A collection of resource groups.</returns>
        /// <exception cref="ArgumentNullException">
        /// subscriptionId
        /// or
        /// tenantId
        /// </exception>
        public List<ResourceGroup> GetResourceGroups(string subscriptionId, string tenantId)
        {
            ResourceManagementClient client;

            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException("subscriptionId");
            }
            else if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException("tenantId");
            }

            try
            {
                client = GetClient(tenantId);
                client.SubscriptionId = subscriptionId;

                return client.ResourceGroups.List().ToList();
            }
            finally
            {
                client = null;
            }
        }

        /// <summary>
        /// Gets a collection of resources that belong to the specified subscription and tenant.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns>A collection of resources.</returns>
        /// <exception cref="ArgumentNullException">
        /// subscriptionId
        /// or
        /// tenantId
        /// </exception>
        public List<GenericResource> GetResources(string subscriptionId, string tenantId)
        {
            ResourceManagementClient client;

            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException("subscriptionId");
            }
            else if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException("tenantId");
            }

            try
            {
                client = GetClient(tenantId);
                client.SubscriptionId = subscriptionId;

                return client.Resources.List().ToList();
            }
            finally
            {
                client = null;
            }
        }

        private ResourceManagementClient GetClient(string tenantId)
        {
            AuthenticationContext authContext;
            AuthenticationResult authResult;
            TokenCredentials tokenCredentials;

            try
            {
                authContext = new AuthenticationContext(
                    string.Format("{0}/{1}",
                        Configuration.Authority,
                        tenantId
                    )
                );

                authResult = authContext.AcquireToken("https://management.azure.com/",
                    Configuration.NativeApplicationId,
                    new UserCredential(Configuration.Username, Configuration.Password)
                );

                tokenCredentials = new TokenCredentials(authResult.AccessToken);

                return new ResourceManagementClient(tokenCredentials);
            }
            finally
            {
                authContext = null;
                authResult = null;
                tokenCredentials = null;
            }
        }
    }
}