// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Samples.Azure.Management;
using Microsoft.Samples.AzureAD.Graph.API;
using Microsoft.Samples.AzureAD.Graph.API.Models;
using Microsoft.Samples.Office365.Management.API;
using Microsoft.Store.PartnerCenter.Models.Customers;
using Microsoft.Store.PartnerCenter.Models.Invoices;
using Microsoft.Store.PartnerCenter.Models.Subscriptions;
using Microsoft.Store.PartnerCenter.Samples.Common;
using Microsoft.Store.PartnerCenter.Samples.Common.Models;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Subscriptions;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    [AuthorizationFilter(ClaimType = ClaimTypes.Role, ClaimValue = "PartnerAdmin")]
    public class SubscriptionsController : Controller
    {
        private SdkContext _context;

        public ActionResult Index()
        {
            return View();
        }

        #region Azure Subscriptions 

        [HttpPost]
        public ActionResult ApplyTemplate(string customerId, string subscriptionId, string resourceGroupName, string templateUri, string parametersUri)
        {
            AuthenticationResult token;

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
                token = TokenContext.GetAADToken(
                    string.Format(
                        "{0}/{1}",
                        AppConfig.Authority,
                        customerId
                    ),
                    AppConfig.ManagementUri
                );

                using (ResourceManager manager = new ResourceManager(token.AccessToken))
                {

                    manager.ApplyTemplate(
                        customerId,
                        subscriptionId,
                        resourceGroupName,
                        templateUri,
                        parametersUri
                    );
                }

                return View();
            }
            finally
            {
                token = null;
            }
        }

        [HttpGet]
        public JsonResult ResourceGroups(string customerId, string subscriptionId)
        {
            AuthenticationResult token;
            ResourceManager manager;

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
                token = TokenContext.GetAADToken(
                   string.Format(
                       "{0}/{1}",
                       AppConfig.Authority,
                       customerId
                   ),
                   AppConfig.ManagementUri
               );

                manager = new ResourceManager(token.AccessToken);
                return Json(manager.GetResourceGroups(customerId, subscriptionId), JsonRequestBehavior.AllowGet);
            }
            finally
            {
                manager = null;
            }
        }

        private List<IHealthEvent> GetAzureSubscriptionHealth(string customerId, string subscriptionId)
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

                    return insights.GetHealthEvents();
                }
            }
            finally
            {
                token = null;
            }
        }

        #endregion

        #region Domain Operations

        [HttpGet]
        public JsonResult GetDomains(string customerId)
        {
            AuthenticationResult token;
            GraphClient client;
            List<Domain> domains;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");

            }

            try
            {
                token = TokenContext.GetAADToken(
                    string.Format(
                        "{0}/{1}",
                        AppConfig.Authority,
                        customerId
                    ),
                    AppConfig.GraphUri
                );

                client = new GraphClient(token.AccessToken);
                domains = client.GetDomains(customerId);

                return Json(domains, JsonRequestBehavior.AllowGet);

            }
            finally
            {
                client = null;
                domains = null;
                token = null;
            }
        }

        #endregion  

        #region Office Subscriptions

        private List<IHealthEvent> GetOfficeSubscriptionHealth(string customerId)
        {
            ServiceCommunications comm;

            try
            {
                comm = new ServiceCommunications(TokenContext.UserAssertionToken);
                return comm.GetCurrentStatus(customerId);
            }
            finally
            {
                comm = null;
            }
        }

        public ActionResult ManageUsers(string customerId)
        {
            AuthenticationResult token;
            GraphClient client;
            SubscriptionManageUsersModel manageUsersModel;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }

            try
            {
                token = TokenContext.GetAADToken(
                    string.Format(
                        "{0}/{1}",
                        AppConfig.Authority,
                        customerId
                    ),
                    AppConfig.GraphUri
                );

                client = new GraphClient(token.AccessToken);

                manageUsersModel = new SubscriptionManageUsersModel()
                {
                    CustomerId = customerId,
                    Users = client.GetUsers(customerId)
                };

                return View(manageUsersModel);
            }
            finally
            {
                client = null;
                token = null;
            }
        }

        #endregion

        public ActionResult Health(string customerId, string subscriptionId)
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
                    FriendlyName = subscription.FriendlyName
                };

                healthModel.SubscriptionType = (subscription.BillingType == BillingType.License) ? "Office" : "Azure";

                if (healthModel.SubscriptionType.Equals("Azure", StringComparison.CurrentCultureIgnoreCase))
                {
                    healthModel.HealthEvents = GetAzureSubscriptionHealth(customerId, subscriptionId);
                }
                else
                {
                    healthModel.HealthEvents = GetOfficeSubscriptionHealth(customerId);
                }

                return View(healthModel);
            }
            finally
            {
                customer = null;
                subscription = null;
            }
        }

        public ActionResult Manage(string customerId, string subscriptionId)
        {
            Customer customer;
            SubscriptionManageModel manageModel;
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

                manageModel = new SubscriptionManageModel()
                {
                    CompanyName = customer.CompanyProfile.CompanyName,
                    CustomerId = customer.Id,
                    FriendlyName = subscription.FriendlyName,
                    SubscriptionId = subscriptionId
                };

                manageModel.SubscriptionType = (subscription.BillingType == BillingType.License) ? "Office" : "Azure";

                if (manageModel.SubscriptionType.Equals("Azure", StringComparison.CurrentCultureIgnoreCase))
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

                return View(manageModel);
            }
            finally
            {
                customer = null;
                subscription = null;
            }
        }

        [HttpPost]
        public ActionResult Update(SubscriptionManageModel model)
        {
            string name = model.SubscriptionDetails.FriendlyName;
            int quantity = ((OfficeSubscriptionDetails)model.SubscriptionDetails).Quantity;

            return View();
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