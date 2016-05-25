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
        /// Applies the specified ARM template.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="resourceGroup">The resource group.</param>
        /// <param name="templateUri">Uri of the ARM template.</param>
        /// <param name="parametersUri">Uri of the parameters for the ARM template.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// subscriptionId
        /// or
        /// tenantId
        /// or
        /// resourceGroup
        /// or
        /// templateUri
        /// </exception>
        public string ApplyARMTemplate(string tenantId, string subscriptionId, string resourceGroup, string templateUri, string parametersUri)
        {
            Deployment deployment;
            ResourceManagementClient client;
            DeploymentExtended result;
            string deploymentName;

            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException("subscriptionId");
            }
            else if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException("tenantId");
            }
            else if (string.IsNullOrEmpty(resourceGroup))
            {
                throw new ArgumentNullException("resourceGroup");
            }
            else if (string.IsNullOrEmpty(templateUri))
            {
                throw new ArgumentNullException("templateUri");
            }

            try
            {
                client = GetClient(tenantId);
                client.SubscriptionId = subscriptionId;

                deployment = new Deployment();
                deployment.Properties = new DeploymentProperties()
                {
                    Mode = DeploymentMode.Incremental,
                    TemplateLink = new TemplateLink(templateUri)
                };

                if (!string.IsNullOrEmpty(parametersUri))
                {
                    deployment.Properties.ParametersLink = new ParametersLink(parametersUri);
                }

                deploymentName = Guid.NewGuid().ToString();

                result = client.Deployments.CreateOrUpdate(resourceGroup, deploymentName, deployment);

                return result.Properties.ProvisioningState;
            }
            finally
            {
                client = null;
                deployment = null;
            }
        }

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
            AuthenticationResult token;

            try
            {
                token = TokenContext.GetAADToken(
                    string.Format("{0}/{1}", Configuration.Authority, tenantId),
                    Configuration.AzureManagementRoot
                );

                return new ResourceManagementClient(new TokenCredentials(token.AccessToken));
            }
            finally
            {
                token = null;
            }
        }
    }
}