// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Samples.Azure.Management;
using Microsoft.Store.PartnerCenter.Models.Customers;
using Microsoft.Store.PartnerCenter.Models.Invoices;
using Microsoft.Store.PartnerCenter.Samples.Common;
using Microsoft.Store.PartnerCenter.Samples.Common.Context;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Subscriptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    /// <summary>
    /// Handles requests for the Manage views.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [AuthorizationFilter(ClaimType = ClaimTypes.Role, ClaimValue = "PartnerAdmin")]
    public class ManageController : Controller
    {
        private SdkContext _context;

        /// <summary>
        /// Gets a list of deployments for the specified resource group.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="resourceGroupName">Name of the resource group.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// customerId
        /// or
        /// subscriptionId
        /// or
        /// resourceGroupName
        /// </exception>
        [HttpGet]
        public async Task<PartialViewResult> Deployments(string customerId, string resourceGroupName, string subscriptionId)
        {
            List<DeploymentModel> deployments;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            if (string.IsNullOrEmpty(resourceGroupName))
            {
                throw new ArgumentNullException(nameof(resourceGroupName));
            }
            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }

            try
            {
                deployments = await GetDeploymentsAsync(customerId, resourceGroupName, subscriptionId);
                return PartialView("Deployments", deployments);
            }
            finally
            {
                deployments = null;
            }
        }

        /// <summary>
        /// Handles the Index view reqeuest.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// customerId
        /// or
        /// subscriptionId
        /// </exception>
        public async Task<ActionResult> Index(string customerId, string subscriptionId)
        {
            Customer customer;
            SubscriptionManageModel manageModel;
            PartnerCenter.Models.Subscriptions.Subscription subscription;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }

            try
            {
                customer = await Context.PartnerOperations.Customers.ById(customerId).GetAsync();
                subscription = await Context.PartnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId).GetAsync();

                manageModel = new SubscriptionManageModel()
                {
                    CompanyName = customer.CompanyProfile.CompanyName,
                    CustomerId = customer.Id,
                    FriendlyName = subscription.FriendlyName,
                    SubscriptionId = subscriptionId
                };

                manageModel.ViewName = (subscription.BillingType == BillingType.License) ? "Office" : "Azure";

                if (manageModel.ViewName.Equals("Azure", StringComparison.CurrentCultureIgnoreCase))
                {
                    manageModel.SubscriptionDetails = new AzureSubscriptionDetails()
                    {
                        FriendlyName = subscription.FriendlyName,
                        Status = subscription.Status.ToString()
                    };
                }
                else
                {
                    manageModel.SubscriptionDetails = new OfficeSubscriptionDetails()
                    {
                        FriendlyName = subscription.FriendlyName,
                        Quantity = subscription.Quantity,
                        Status = subscription.Status.ToString()
                    };
                }

                return View(manageModel.ViewName, manageModel);
            }
            finally
            {
                customer = null;
                subscription = null;
            }
        }

        /// <summary>
        /// Handles the request to create a new deployment.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="resourceGroupName">Name of the resource group.</param>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <returns>A partial view that will be used to configure the required values for the new deployment.</returns>
        [HttpGet]
        public PartialViewResult NewDeployment(string customerId, string resourceGroupName, string subscriptionId)
        {
            NewDeploymentModel newDeploymentModel = new NewDeploymentModel()
            {
                CustomerId = customerId,
                ResourceGroupName = resourceGroupName,
                SubscriptionId = subscriptionId
            };

            return PartialView(newDeploymentModel);
        }

        /// <summary>
        /// Creates a new Azure Resource Manager (ARM) deployment.
        /// </summary>
        /// <param name="model">An instance of <see cref="NewDeploymentModel"/>.</param>
        /// <returns>A collection of deployments</returns>
        [HttpPost]
        public async Task<PartialViewResult> NewDeployment(NewDeploymentModel model)
        {
            AuthenticationResult token;
            List<DeploymentModel> results;

            try
            {
                token = TokenContext.GetAADToken(
                    $"{AppConfig.Authority}/{model.CustomerId}",
                   AppConfig.ManagementUri
                );

                using (ResourceManager manager = new ResourceManager(token.AccessToken))
                {
                    await manager.ApplyTemplateAsync(
                        model.SubscriptionId,
                        model.ResourceGroupName,
                        model.TemplateUri,
                        model.ParametersUri
                    );

                    results = await GetDeploymentsAsync(model.CustomerId, model.ResourceGroupName, model.SubscriptionId);
                    return PartialView("Deployments", results);
                }
            }
            finally
            {
                results = null;
                token = null;
            }
        }

        /// <summary>
        /// Gets resource groups that belong to the specified customer and subscription.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <returns>Returns a collection of resource groups in JSON.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// customerId
        /// or
        /// subscriptionId
        /// </exception>
        [HttpGet]
        public async Task<JsonResult> ResourceGroups(string customerId, string subscriptionId)
        {
            AuthenticationResult token;
            List<ResourceGroup> groups;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }

            try
            {
                token = TokenContext.GetAADToken(
                    $"{AppConfig.Authority}/{customerId}",
                   AppConfig.ManagementUri
               );

                using (ResourceManager manager = new ResourceManager(token.AccessToken))
                {
                    groups = await manager.GetResourceGroupsAsync(subscriptionId);
                    return Json(groups, JsonRequestBehavior.AllowGet);
                }
            }
            finally
            {
                groups = null;
                token = null;
            }
        }

        private SdkContext Context => _context ?? (_context = new SdkContext());

        private async Task<List<DeploymentModel>> GetDeploymentsAsync(string customerId, string resourceGroupName, string subscriptionId)
        {
            AuthenticationResult token;
            List<DeploymentExtended> deployments;
            List<DeploymentModel> model;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            if (string.IsNullOrEmpty(resourceGroupName))
            {
                throw new ArgumentNullException(nameof(resourceGroupName));
            }
            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }

            try
            {
                token = TokenContext.GetAADToken(
                    $"{AppConfig.Authority}/{customerId}",
                   AppConfig.ManagementUri
               );

                using (ResourceManager manager = new ResourceManager(token.AccessToken))
                {
                    deployments = await manager.GetDeploymentsAsync(subscriptionId, resourceGroupName);
                    model = new List<DeploymentModel>();

                    foreach (DeploymentExtended d in deployments)
                    {
                        model.Add(new DeploymentModel()
                        {
                            Id = d.Id,
                            Name = d.Name,
                            ProvisioningState = d.Properties.ProvisioningState,
                            Timestamp = d.Properties.Timestamp.Value
                        });
                    }

                    return model;
                }
            }
            finally
            {
                deployments = null;
                token = null;
            }
        }
    }
}