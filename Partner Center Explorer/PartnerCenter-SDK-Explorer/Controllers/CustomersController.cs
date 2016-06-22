// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models.Customers;
using Microsoft.Store.PartnerCenter.Samples.Common;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    [AuthorizationFilter(ClaimType = ClaimTypes.Role, ClaimValue = "PartnerAdmin")]
    public class CustomersController : Controller
    {
        private SdkContext _context;

        [HttpDelete]
        public HttpResponseMessage Delete(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }

            // A customer can only be deleted in the integration sandbox environment.   
            // If this request is attempted in a non-sandbox environment it will fail. 
            if (AppConfig.IsSandboxEnvironment)
            {
                Context.PartnerOperations.Customers.ById(customerId).Delete();
            }

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        public ActionResult Index()
        {
            CustomersModel customersModel = new CustomersModel()
            {
                Customers = Context.PartnerOperations.Customers.Get(),
                IsSandboxEnvironment = AppConfig.IsSandboxEnvironment
            };

            return View(customersModel);
        }

        public ActionResult Show(string customerId)
        {
            Customer customer;
            CustomerModel customerModel;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }

            try
            {
                customer = Context.PartnerOperations.Customers.ById(customerId).Get();

                customerModel = new CustomerModel()
                {
                    BillingProfile = customer.BillingProfile,
                    CompanyName = customer.CompanyProfile.CompanyName,
                    CompanyProfile = customer.CompanyProfile,
                    DomainName = customer.CompanyProfile.Domain,
                    TenantId = customer.CompanyProfile.TenantId
                };

                customerModel.Subscriptions = Context.PartnerOperations.Customers.ById(customerId).Subscriptions.Get();

                return View(customerModel);
            }
            finally
            {
                customer = null;
                customerModel = null;
            }
        }

        public ActionResult Subscriptions(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }

            SubscriptionsModel subscriptionsModel = new SubscriptionsModel()
            {
                Subscriptions = Context.PartnerOperations.Customers.ById(customerId).Subscriptions.Get()
            };

            return View(subscriptionsModel);
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