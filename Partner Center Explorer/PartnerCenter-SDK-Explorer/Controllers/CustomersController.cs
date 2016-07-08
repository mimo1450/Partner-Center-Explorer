// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.Customers;
using Microsoft.Store.PartnerCenter.Samples.Common;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    /// <summary>
    /// Controller for all Customers views.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [AuthorizationFilter(ClaimType = ClaimTypes.Role, ClaimValue = "PartnerAdmin")]
    public class CustomersController : Controller
    {
        private SdkContext _context;

        /// <summary>
        /// Deletes the specified customer.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>Returns the NoContent HTTP status code.</returns>
        /// <exception cref="System.ArgumentNullException">customerId</exception>
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }

            // A customer can only be deleted in the integration sandbox environment.   
            // If this request is attempted in a non-sandbox environment it will fail. 
            if (AppConfig.IsSandboxEnvironment)
            {
                await Context.PartnerOperations.Customers.ById(customerId).DeleteAsync();
            }

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Handles the request to load the new customer partial view. 
        /// </summary>
        /// <returns>Returns a partial view for creating new customers.</returns>
        [HttpGet]
        public PartialViewResult Create()
        {
            // TODO - Do not get the supported states list each time. This data should be cached so the forms will load more rapidly. 
            NewCustomerModel newCustomerModel = new NewCustomerModel()
            {
                SupportedStates = Context.PartnerOperations.CountryValidationRules.ByCountry(AppConfig.CountryCode).Get().SupportedStatesList
            };

            return PartialView(newCustomerModel);
        }

        /// <summary>
        /// Create the customer represented by the instance of <see cref="NewCustomerModel"/>.
        /// </summary>
        /// <param name="newCustomerModel">The new customer model.</param>
        /// <returns>Returns a partial view containing result of the customer creation.</returns>
        [HttpPost]
        public async Task<ActionResult> Create(NewCustomerModel newCustomerModel)
        {
            Customer entity;
            CreatedCustomerModel createdCustomerModel;

            try
            {
                entity = new Customer()
                {
                    BillingProfile = new CustomerBillingProfile()
                    {
                        CompanyName = newCustomerModel.Name,
                        Culture = "en-US",
                        DefaultAddress = new Address()
                        {
                            AddressLine1 = newCustomerModel.AddressLine1,
                            AddressLine2 = newCustomerModel.AddressLine2,
                            City = newCustomerModel.City,
                            Country = "US",
                            FirstName = newCustomerModel.FirstName,
                            LastName = newCustomerModel.LastName,
                            PhoneNumber = newCustomerModel.PhoneNumber,
                            PostalCode = newCustomerModel.ZipCode,
                            State = newCustomerModel.State
                        },
                        Email = newCustomerModel.EmailAddress,
                        FirstName = newCustomerModel.FirstName,
                        Language = "en",
                        LastName = newCustomerModel.LastName
                    },
                    CompanyProfile = new CustomerCompanyProfile()
                    {
                        CompanyName = newCustomerModel.Name,
                        Domain = string.Format("{0}.onmicrosoft.com", newCustomerModel.PrimaryDomain)
                    }
                };

                entity = await Context.PartnerOperations.Customers.CreateAsync(entity);

                createdCustomerModel = new CreatedCustomerModel()
                {
                    Domain = string.Format("{0}.onmicrosoft.com", newCustomerModel.PrimaryDomain),
                    Password = entity.UserCredentials.Password,
                    Username = entity.UserCredentials.UserName
                };

                return PartialView("CreatedSuccessfully", createdCustomerModel);
            }
            finally
            {
                entity = null;
            }
        }

        /// <summary>
        /// Handles the index page load event.
        /// </summary>
        /// <returns>Returns a list of customers the belong to the specified partner.</returns>
        public async Task<ActionResult> Index()
        {
            CustomersModel customersModel = new CustomersModel()
            {
                Customers = await Context.PartnerOperations.Customers.GetAsync(),
                IsSandboxEnvironment = AppConfig.IsSandboxEnvironment
            };

            return View(customersModel);
        }

        /// <summary>
        /// Handles the request to view the customer associated with the specified customer identifier.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>Returns view to display the customer details.</returns>
        /// <exception cref="System.ArgumentNullException">customerId</exception>
        public async Task<ActionResult> Show(string customerId)
        {
            Customer customer;
            CustomerModel customerModel;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }

            try
            {
                customer = await Context.PartnerOperations.Customers.ById(customerId).GetAsync();

                customerModel = new CustomerModel()
                {
                    BillingProfile = customer.BillingProfile,
                    CompanyName = customer.CompanyProfile.CompanyName,
                    CompanyProfile = customer.CompanyProfile,
                    DomainName = customer.CompanyProfile.Domain,
                    TenantId = customer.CompanyProfile.TenantId
                };

                customerModel.Subscriptions = await Context.PartnerOperations.Customers.ById(customerId).Subscriptions.GetAsync();

                return View(customerModel);
            }
            finally
            {
                customer = null;
                customerModel = null;
            }
        }

        /// <summary>
        /// Handles the request to view subscription belonging to the customer associated with the specified customer identifier.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>Returns a veiw containing all of the subscriptions owned by the specificed customer.</returns>
        /// <exception cref="System.ArgumentNullException">customerId</exception>
        public async Task<ActionResult> Subscriptions(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }

            SubscriptionsModel subscriptionsModel = new SubscriptionsModel()
            {
                Subscriptions = await Context.PartnerOperations.Customers.ById(customerId).Subscriptions.GetAsync()
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