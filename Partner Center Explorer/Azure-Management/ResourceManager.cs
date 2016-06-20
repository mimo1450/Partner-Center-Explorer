// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Samples.Azure.Management
{
    public class ResourceManager : IDisposable
    {
        private ResourceManagementClient _client;
        private string _token;
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
                if (_client != null)
                {
                    _client.Dispose();
                }
            }

            _disposed = true;
        }

        public string ApplyTemplate(string customerId, string subscriptionId, string resourceGroupName, string templateUri, string parametersUri)
        {
            Deployment deployment;
            DeploymentExtended result;
            string deploymentName;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }
            else if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException("subscriptionId");
            }
            else if (string.IsNullOrEmpty(resourceGroupName))
            {
                throw new ArgumentNullException("resourceGroupName");
            }
            else if (string.IsNullOrEmpty(templateUri))
            {
                throw new ArgumentNullException("templateUri");
            }

            try
            {
                Client.SubscriptionId = subscriptionId;

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

                result = Client.Deployments.CreateOrUpdate(resourceGroupName, deploymentName, deployment);

                return result.Properties.ProvisioningState;
            }
            finally
            {
                deployment = null;
                result = null;
            }
        }

        public List<ResourceGroup> GetResourceGroups(string customerId, string subscriptionId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }
            else if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException("subscriptinId");
            }

            Client.SubscriptionId = subscriptionId;

            return Client.ResourceGroups.List().ToList();
        }

        private ResourceManagementClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new ResourceManagementClient(new TokenCredentials(_token));
                }

                return _client;
            }
        }
    }
}
