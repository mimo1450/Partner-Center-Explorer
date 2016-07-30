// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models.Customers;
using Microsoft.Store.PartnerCenter.Models.Subscriptions;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    /// <summary>
    /// Controller for all Usage views.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [AuthorizationFilter(ClaimType = ClaimTypes.Role, ClaimValue = "PartnerAdmin")]
    public class UsageController : Controller
    {
        /// <summary>
        /// Views the usage.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// customerId
        /// or
        /// subscriptionId
        /// </exception>
        public async Task<ActionResult> ViewUsage(string customerId, string subscriptionId)
        {
            Customer customer;
            IAggregatePartner operations;
            Subscription subscription;

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
                operations = await new SdkContext().GetPartnerOperationsAysnc();
                customer = await operations.Customers.ById(customerId).GetAsync();
                subscription = await operations.Customers.ById(customerId).Subscriptions.ById(subscriptionId).GetAsync();

                UsageModel usageModel = new UsageModel()
                {
                    CompanyName = customer.CompanyProfile.CompanyName,
                    CustomerId = customerId,
                    DailyUsage = await operations.Customers.ById(customerId)
                        .Subscriptions.ById(subscriptionId).UsageRecords.Daily.GetAsync(),
                    MonthlyUsage = await operations.Customers.ById(customerId)
                        .Subscriptions.ById(subscriptionId).UsageRecords.Resources.GetAsync(),
                    SubscriptionId = subscriptionId,
                    SubscriptionFriendlyName = subscription.FriendlyName
                };

                return View(usageModel);
            }
            finally
            {
                customer = null;
                subscription = null;
            }
        }
    }
}