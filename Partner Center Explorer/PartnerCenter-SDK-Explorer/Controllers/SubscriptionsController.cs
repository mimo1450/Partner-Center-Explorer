// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.Customers;
using Microsoft.Store.PartnerCenter.Models.Invoices;
using Microsoft.Store.PartnerCenter.Models.Offers;
using Microsoft.Store.PartnerCenter.Models.Orders;
using Microsoft.Store.PartnerCenter.Models.Subscriptions;
using Microsoft.Store.PartnerCenter.Samples.Common;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Mvc;

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

        /// <summary>
        /// Handles the HTTP GET request for the Create partial view.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>An aptly populate instnace of <see cref="NewSubscriptionModel"/> in a partial view.</returns>
        [HttpGet]
        public PartialViewResult Create(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            NewSubscriptionModel newSubscriptionModel = new NewSubscriptionModel()
            {
                CustomerId = customerId
            };

            return PartialView(newSubscriptionModel);
        }

        /// <summary>
        /// Creates the order specified in the instance of <see cref="NewSubscriptionModel"/>.
        /// </summary>
        /// <param name="model">An aptly populated instance of <see cref="NewSubscriptionModel"/>.</param>
        /// <returns>A collection of subscriptions that belong to the customer.</returns>
        [HttpPost]
        public async Task<PartialViewResult> Create(NewSubscriptionModel model)
        {
            Order newOrder;
            SubscriptionsModel subscriptionsModel;

            try
            {
                newOrder = new Order()
                {
                    LineItems = model.LineItems,
                    ReferenceCustomerId = model.CustomerId
                };

                newOrder = await Context.PartnerOperations.Customers.ById(model.CustomerId).Orders.CreateAsync(newOrder);
                subscriptionsModel = await GetSubscriptionsAsync(model.CustomerId);

                return PartialView("List", subscriptionsModel);
            }
            finally
            {
                newOrder = null;
            }
        }

        /// <summary>
        /// Gets the description for the specified offer.
        /// </summary>
        /// <param name="offerId">The offer identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        [HttpGet]
        public async Task<JsonResult> GetOffer(string offerId)
        {
            if (string.IsNullOrEmpty(offerId))
            {
                throw new ArgumentNullException(nameof(offerId));
            }

            // The Partner Center API can be used to obtain the offer. This application obtains a resource collection
            // of available offers prior to this function being invoked. Since the available offers are obtained prior
            // to the execution of this function we are able to cache that resource collection and use that resource
            // collection instead of the following request.

            // Offer offer = await Context.PartnerOperations.Offers.ByCountry(AppConfig.CountryCode)
            //    .ById(offerId).GetAsync();

            ResourceCollection<Offer> offers;

            try
            {
                offers = await GetAvailableOffersAsync();

                return Json(offers.Items.Single(
                    x => x.Id.Equals(offerId, StringComparison.CurrentCultureIgnoreCase)),
                    JsonRequestBehavior.AllowGet);
            }
            finally
            {
                offers = null;
            }
        }

        /// <summary>
        /// Edits the subscription represented by the instance of <see cref="SubscriptionModel"/>.
        /// </summary>
        /// <param name="model">An aptly populated instance of <see cref="SubscriptionModel"/>.</param>
        /// <returns>A HTTP status code of OK if the edit was successful.</returns>
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
        /// Lists all of the subscriptions owned by the specified customer identifier.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">customerId</exception>
        public async Task<PartialViewResult> List(string customerId)
        {
            SubscriptionsModel subscriptionsModel;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            try
            {
                subscriptionsModel = await GetSubscriptionsAsync(customerId);
                return PartialView(subscriptionsModel);
            }
            finally
            {
                subscriptionsModel = null;
            }
        }

        /// <summary>
        /// Handles the Offers partial view request.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>A partial view containing the OffersModel model.</returns>
        [HttpGet]
        public async Task<PartialViewResult> Offers(string customerId)
        {
            OffersModel offersModel = new OffersModel()
            {
                AvailableOffers = await GetOfferModelsAsync(),
                CustomerId = customerId
            };

            return PartialView(offersModel);
        }

        /// <summary>
        /// Handles the Show view request.
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
        public async Task<ActionResult> Show(string customerId, string subscriptionId)
        {
            Customer customer;
            Subscription subscription;
            SubscriptionModel model;

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

        private async Task<ResourceCollection<Offer>> GetAvailableOffersAsync()
        {
            ResourceCollection<Offer> offers = MemoryCache.Default["AvailableOffers"] as ResourceCollection<Offer>;

            if (offers == null)
            {
                offers = await Context.PartnerOperations.Offers.ByCountry(AppConfig.CountryCode).GetAsync();
                MemoryCache.Default["AvailableOffers"] = offers;
            }

            return offers;
        }

        private async Task<List<OfferModel>> GetOfferModelsAsync()
        {
            List<OfferModel> models;
            ResourceCollection<Offer> availableOffers;

            try
            {
                availableOffers = await GetAvailableOffersAsync();
                models = new List<OfferModel>();

                foreach (Offer offer in availableOffers.Items)
                {
                    if (offer.IsAvailableForPurchase)
                    {
                        models.Add(new OfferModel()
                        {
                            Billing = offer.Billing,
                            Description = offer.Description,
                            Id = offer.Id,
                            IsAddOn = offer.IsAddOn,
                            IsAvailableForPurchase = offer.IsAvailableForPurchase,
                            MaxiumQuantity = offer.MaximumQuantity,
                            MinimumQuantity = offer.MinimumQuantity,
                            Name = offer.Name,
                            PrerequisiteOffers = offer.PrerequisiteOffers
                        });
                    }
                }

                return models;
            }
            finally
            {
                availableOffers = null;
            }
        }

        private async Task<SubscriptionsModel> GetSubscriptionsAsync(string customerId)
        {
            ResourceCollection<Subscription> subscriptions;
            SubscriptionsModel subscriptionsModel;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            try
            {
                subscriptions = await Context.PartnerOperations.Customers.ById(customerId).Subscriptions.GetAsync();
                subscriptionsModel = new SubscriptionsModel()
                {
                    Subscriptions = new List<SubscriptionModel>()
                };

                foreach (Subscription s in subscriptions.Items)
                {
                    subscriptionsModel.Subscriptions.Add(new SubscriptionModel()
                    {
                        AutoRenewEnabled = s.AutoRenewEnabled,
                        BillingType = s.BillingType,
                        CommitmentEndDate = s.CommitmentEndDate,
                        CreationDate = s.CreationDate,
                        CustomerId = customerId,
                        EffectiveStartDate = s.EffectiveStartDate,
                        FriendlyName = s.FriendlyName,
                        Id = s.Id,
                        OfferId = s.OfferId,
                        OfferName = s.OfferName,
                        ParentSubscriptionId = s.ParentSubscriptionId,
                        PartnerId = s.PartnerId,
                        Quantity = s.Quantity,
                        Status = s.Status,
                        SuspensionReasons = s.SuspensionReasons,
                        UnitType = s.UnitType
                    });
                }

                return subscriptionsModel;
            }
            finally
            {
                subscriptions = null;
            }
        }
    }
}