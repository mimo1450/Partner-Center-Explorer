// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Azure;
using Microsoft.Azure.Management.Automation;
using Microsoft.Azure.Management.Automation.Models;
using Microsoft.Store.PartnerCenter.Samples.Common.Models.Automation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Samples.Azure.Management
{
    public class Automation
    {
        private AutomationManagementClient _client;
        private readonly TokenCloudCredentials _token;

        /// <summary>
        /// Initializes a new instance of the <see cref="Automation"/> class.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="token">A valid JSON Web Token (JWT).</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public Automation(string subscriptionId, string token)
        {
            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            _token = new TokenCloudCredentials(subscriptionId, token);
        }

        /// <summary>
        /// Gets a collection of runbooks.
        /// </summary>
        /// <param name="resourceGroupName">Name of the resource group.</param>
        /// <param name="automationAccount">The automation account.</param>
        /// <returns>A collection of runbooks.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public async Task<IList<RunbookModel>> GetRunbooks(string resourceGroupName, string automationAccount)
        {
            RunbookListResponse response;

            if (string.IsNullOrEmpty(automationAccount))
            {
                throw new ArgumentNullException(nameof(automationAccount));
            }
            if (string.IsNullOrEmpty(resourceGroupName))
            {
                throw new ArgumentNullException(nameof(resourceGroupName));
            }

            try
            {
                response = await Client.Runbooks.ListAsync(resourceGroupName, automationAccount);
                return GetRunbookModelsCollection(response.Runbooks);
            }
            finally
            {
                response = null;
            }
        }

        private AutomationManagementClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new AutomationManagementClient(_token);
                }

                return _client;
            }
        }

        private List<RunbookModel> GetRunbookModelsCollection(IList<Runbook> runbooks)
        {
            List<RunbookModel> models = new List<RunbookModel>();

            foreach (Runbook rb in runbooks)
            {
                models.Add(new RunbookModel()
                {
                    CreationTime = rb.Properties.CreationTime,
                    Description = rb.Properties.Description,
                    Id = rb.Id,
                    JobCount = rb.Properties.JobCount,
                    LastModifiedBy = rb.Properties.LastModifiedBy,
                    LastModifiedTime = rb.Properties.LastModifiedTime,
                    LogActivityTrace = rb.Properties.LogActivityTrace,
                    LogProgress = rb.Properties.LogProgress,
                    LogVerbose = rb.Properties.LogVerbose,
                    Name = rb.Name,
                    ProvisioningState = rb.Properties.ProvisioningState.ToString(),
                    RunbookType = rb.Properties.RunbookType,
                    State = rb.Properties.State
                });
            }

            return models;
        }
    }
}