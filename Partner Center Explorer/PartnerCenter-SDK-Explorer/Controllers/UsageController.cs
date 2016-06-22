// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models.Customers;
using Microsoft.Store.PartnerCenter.Models.Subscriptions;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models;
using System;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    [AuthorizationFilter(ClaimType = ClaimTypes.Role, ClaimValue = "PartnerAdmin")]
    public class UsageController : Controller
    {
        private SdkContext _context;

        public ActionResult ViewUsage(string customerId, string subscriptionId)
        {
            Customer customer;
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

                UsageModel usageModel = new UsageModel()
                {
                    CompanyName = customer.CompanyProfile.CompanyName,
                    CustomerId = customerId,
                    DailyUsage = Context.PartnerOperations.Customers.ById(customerId)
                        .Subscriptions.ById(subscriptionId).UsageRecords.Daily.Get(),
                    MonthlyUsage = Context.PartnerOperations.Customers.ById(customerId)
                        .Subscriptions.ById(subscriptionId).UsageRecords.Resources.Get(),
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
    }
}