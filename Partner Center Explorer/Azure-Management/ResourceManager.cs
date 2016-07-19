// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Rest;
using Microsoft.Rest.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Samples.Azure.Management
{
    /// <summary>
    /// Facilitates interactions with the Azure Resource Manager API.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class ResourceManager : IDisposable
    {
        private ResourceManagementClient _client;
        private readonly string _token;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceManager"/> class.
        /// </summary>
        /// <param name="token">A valid JSON Web Token (JWT).</param>
        public ResourceManager(string token)
        {
            _token = token;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _client?.Dispose();
            }

            _disposed = true;
        }

        /// <summary>
        /// Apply an Azure Resource Manager (ARM) template.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="resourceGroupName">Name of the resource group.</param>
        /// <param name="templateUri">URI for the ARM template.</param>
        /// <param name="parametersUri">URI for the ARM template parameters.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// subscriptionId
        /// or
        /// resourceGroupName
        /// or
        /// templateUri
        /// </exception>
        public async Task<string> ApplyTemplateAsync(string subscriptionId, string resourceGroupName, string templateUri, string parametersUri)
        {
            Deployment deployment;
            DeploymentExtended result;
            string deploymentName;

            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }
            if (string.IsNullOrEmpty(resourceGroupName))
            {
                throw new ArgumentNullException(nameof(resourceGroupName));
            }
            if (string.IsNullOrEmpty(templateUri))
            {
                throw new ArgumentNullException(nameof(templateUri));
            }

            try
            {
                Client.SubscriptionId = subscriptionId;

                deployment = new Deployment()
                {
                    Properties = new DeploymentProperties()
                    {
                        Mode = DeploymentMode.Incremental,
                        TemplateLink = new TemplateLink(templateUri)
                    }
                };

                if (!string.IsNullOrEmpty(parametersUri))
                {
                    deployment.Properties.ParametersLink = new ParametersLink(parametersUri);
                }

                deploymentName = Guid.NewGuid().ToString();

                result = await Client.Deployments.CreateOrUpdateAsync(resourceGroupName, deploymentName, deployment);

                return result.Properties.ProvisioningState;
            }
            finally
            {
                deployment = null;
                result = null;
            }
        }

        /// <summary>
        /// Gets a list of deployments for the specified subscription and resource group.
        /// </summary>
        /// <param name="subscriptionId">The subscrption identifier.</param>
        /// <param name="resourceGroupName">Name of the resource group.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// subscritpionId
        /// or
        /// resourceGroupName
        /// </exception>
        public async Task<List<DeploymentExtended>> GetDeploymentsAsync(string subscriptionId, string resourceGroupName)
        {
            IPage<DeploymentExtended> deployements;

            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }
            if (string.IsNullOrEmpty(resourceGroupName))
            {
                throw new ArgumentNullException(nameof(resourceGroupName));
            }

            try
            {
                Client.SubscriptionId = subscriptionId;
                deployements = await Client.Deployments.ListAsync(resourceGroupName);

                return deployements.ToList();
            }
            finally
            {
                deployements = null;
            }
        }

        /// <summary>
        /// Gets a collection of resource groups for the specified subscription.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <returns>A collection of resource groups.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// subscriptinId
        /// </exception>
        public async Task<List<ResourceGroup>> GetResourceGroupsAsync(string subscriptionId)
        {
            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }

            IPage<ResourceGroup> resourceGroups;

            try
            {
                Client.SubscriptionId = subscriptionId;
                resourceGroups = await Client.ResourceGroups.ListAsync();
                return resourceGroups.ToList();
            }
            finally
            {
                resourceGroups = null;
            }
        }

        private ResourceManagementClient Client => _client ?? (_client = new ResourceManagementClient(new TokenCredentials(_token)));
    }
}