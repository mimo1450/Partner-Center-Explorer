// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Samples.Azure.Management;
using Microsoft.Samples.Office365.Management.API;
using Microsoft.Store.PartnerCenter.Models.Customers;
using Microsoft.Store.PartnerCenter.Models.Invoices;
using Microsoft.Store.PartnerCenter.Models.Subscriptions;
using Microsoft.Store.PartnerCenter.Samples.Common;
using Microsoft.Store.PartnerCenter.Samples.Common.Models;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    /// <summary>
    /// Handles request for the Health views.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [AuthorizationFilter(ClaimType = ClaimTypes.Role, ClaimValue = "PartnerAdmin")]
    public class HealthController : Controller
    {
        private SdkContext _context;

        /// <summary>
        /// Handles the index view request.
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
            SubscriptionHealthModel healthModel;
            Subscription subscription;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }
            else if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException("subscriptionId");
            }

            try
            {
                customer = Context.PartnerOperations.Customers.ById(customerId).Get();
                subscription = Context.PartnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId).Get();

                healthModel = new SubscriptionHealthModel()
                {
                    CompanyName = customer.CompanyProfile.CompanyName,
                    CustomerId = customerId,
                    FriendlyName = subscription.FriendlyName,
                    SubscriptionId = subscriptionId
                };

                healthModel.ViewModel = (subscription.BillingType == BillingType.License) ? "Office" : "Azure";

                if (subscription.BillingType == BillingType.Usage)
                {
                    healthModel.HealthEvents = await GetAzureSubscriptionHealthAsync(customerId, subscriptionId);
                }
                else
                {
                    healthModel.HealthEvents = await GetOfficeSubscriptionHealthAsync(customerId);
                }

                return View(healthModel.ViewModel, healthModel);
            }
            finally
            {
                customer = null;
                subscription = null;
            }
        }

        private SdkContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new SdkContext();
                }

                return _context;
            }
        }

        private async Task<List<IHealthEvent>> GetAzureSubscriptionHealthAsync(string customerId, string subscriptionId)
        {
            AuthenticationResult token;

            try
            {
                token = TokenContext.GetAADToken(
                    string.Format(
                        "{0}/{1}",
                        AppConfig.Authority,
                        customerId
                    ),
                    AppConfig.ManagementUri
                );

                using (Insights insights = new Insights(
                    subscriptionId,
                    token.AccessToken
                ))
                {
                    return await insights.GetHealthEventsAsync();
                }
            }
            finally
            {
                token = null;
            }
        }

        private async Task<List<IHealthEvent>> GetOfficeSubscriptionHealthAsync(string customerId)
        {
            ServiceCommunications comm;

            try
            {
                comm = new ServiceCommunications(TokenContext.UserAssertionToken);
                return await comm.GetCurrentStatusAsync(customerId);
            }
            finally
            {
                comm = null;
            }
        }
    }
}