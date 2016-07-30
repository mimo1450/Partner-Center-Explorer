// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Azure;
using Microsoft.Azure.Management.Automation;
using Microsoft.Azure.Management.Automation.Models;
using Microsoft.Store.PartnerCenter.Samples.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Samples.Azure.Management
{
    /// <summary>
    /// Facilitates interactions with the Azure Management API to expose Azure Automation operations.
    /// </summary>
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
        /// Asynchrounously gets a collection of runbooks.
        /// </summary>
        /// <param name="resourceGroupName">Name of the resource group.</param>
        /// <param name="automationAccount">The automation account.</param>
        /// <returns>The response model for the list runbook operation.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// resourceGroupName
        /// or
        /// automationAccount
        /// </exception>
        public async Task<List<RunbookModel>> GetRunbooksAsync(string resourceGroupName, string automationAccount)
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
                return GetRunbookModelsCollection(response.Runbooks, resourceGroupName);
            }
            finally
            {
                response = null;
            }
        }

        /// <summary>
        /// Invokes the specified runbook.
        /// </summary>
        /// <param name="resourceGroupName">Name of the resource group.</param>
        /// <param name="automationAccount">The automation account.</param>
        /// <param name="runbookName">Name of the runbook.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// automationAccount
        /// or
        /// resourceGroupName
        /// or
        /// runbookName
        /// </exception>
        public async Task InvokeRunbookAsync(string resourceGroupName, string automationAccount, string runbookName)
        {
            JobCreateParameters jcp;
            RunbookAssociationProperty rap;

            if (string.IsNullOrEmpty(automationAccount))
            {
                throw new ArgumentNullException(nameof(automationAccount));
            }
            if (string.IsNullOrEmpty(runbookName))
            {
                throw new ArgumentNullException(nameof(runbookName));
            }
            if (string.IsNullOrEmpty(resourceGroupName))
            {
                throw new ArgumentNullException(nameof(resourceGroupName));
            }

            try
            {
                rap = new RunbookAssociationProperty()
                {
                    Name = runbookName
                };

                jcp = new JobCreateParameters(new JobCreateProperties(rap)
                {
                    Parameters = null
                });

                await Client.Jobs.CreateAsync(resourceGroupName, automationAccount, jcp).ConfigureAwait(false);
            }
            finally
            {
                rap = null;
                jcp = null;
            }
        }

        private AutomationManagementClient Client => _client ?? (_client = new AutomationManagementClient(_token));

        private List<RunbookModel> GetRunbookModelsCollection(IList<Runbook> runbooks, string resourceGroupName)
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
                    ResourceGroupName = resourceGroupName,
                    RunbookType = rb.Properties.RunbookType,
                    State = rb.Properties.State
                });
            }

            return models;
        }
    }
}