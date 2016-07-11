// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using System.Threading.Tasks;
using System;
using System.Web.Mvc;
using Microsoft.Store.PartnerCenter.Models.Customers;
using Microsoft.Store.PartnerCenter.Models.Subscriptions;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models;
using Microsoft.Store.PartnerCenter.Models.Invoices;
using System.Net.Http;
using System.Net;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    /// <summary>
    /// Handles request for the Subscriptions views.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [AuthorizationFilter(ClaimType = ClaimTypes.Role, ClaimValue = "PartnerAdmin")]
    public class SubscriptionsController : Controller
    {
        private SdkContext _context;

        [HttpPost]
        public async Task<HttpResponseMessage> Edit(SubscriptionModel model)
        {
            Subscription subscription;

            try
            {
                subscription = await Context.PartnerOperations.Customers
                    .ById(model.CustomerId).Subscriptions.ById(model.Id).GetAsync();

                subscription.FriendlyName = model.FriendlyName;
                subscription.Status = model.Status;
                subscription.Quantity = model.Quantity;

                await Context.PartnerOperations.Customers.ById(model.CustomerId)
                    .Subscriptions.ById(model.Id).PatchAsync(subscription);

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            finally
            {
                subscription = null; 
            }
        }

        /// <summary>
        /// Handles the index view request.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <returns>Returns either the Azure or Office subscription view.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// customerId
        /// or
        /// subscriptionId
        /// </exception>
        /// <remarks>
        /// If the subscription's billing type is usage then the Office view is returned. 
        /// Otherwise, the Azure view is returned.
        /// </remarks>
        public async Task<ActionResult> Index(string customerId, string subscriptionId)
        {
            Customer customer;
            Subscription subscription;
            SubscriptionModel model;

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
                customer = await Context.PartnerOperations.Customers.ById(customerId).GetAsync();
                subscription = await Context.PartnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId).GetAsync();

                model = new SubscriptionModel()
                {
                    AutoRenewEnabled = subscription.AutoRenewEnabled,
                    BillingType = subscription.BillingType,
                    CommitmentEndDate = subscription.CommitmentEndDate,
                    CompanyName = customer.CompanyProfile.CompanyName,
                    CreationDate = subscription.CreationDate,
                    CustomerId = customerId,
                    EffectiveStartDate = subscription.EffectiveStartDate,
                    FriendlyName = subscription.FriendlyName,
                    Id = subscription.Id,
                    OfferId = subscription.OfferId,
                    OfferName = subscription.OfferName,
                    ParentSubscriptionId = subscription.ParentSubscriptionId,
                    PartnerId = subscription.PartnerId,
                    Quantity = subscription.Quantity,
                    Status = subscription.Status,
                    SuspensionReasons = subscription.SuspensionReasons,
                    UnitType = subscription.UnitType,
                    ViewModel = (subscription.BillingType == BillingType.License) ? "Office" : "Azure"
                };

                return View(model.ViewModel, model);
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