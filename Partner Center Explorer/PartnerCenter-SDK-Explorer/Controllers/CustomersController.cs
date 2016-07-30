// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.Customers;
using Microsoft.Store.PartnerCenter.Samples.Common;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <summary>
        /// Deletes the specified customer.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>Returns the NoContent HTTP status code.</returns>
        /// <exception cref="System.ArgumentNullException">customerId</exception>
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(string customerId)
        {
            IAggregatePartner operations = await new SdkContext().GetPartnerOperationsAysnc();

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            // A customer can only be deleted in the integration sandbox environment.
            // If this request is attempted in a non-sandbox environment it will fail.
            if (AppConfig.IsSandboxEnvironment)
            {
                await operations.Customers.ById(customerId).DeleteAsync();
            }

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Handles the request to load the new customer partial view.
        /// </summary>
        /// <returns>A partial view for creating new customers.</returns>
        [HttpGet]
        public async Task<PartialViewResult> Create()
        {
            IAggregatePartner operations = await new SdkContext().GetPartnerOperationsAysnc();

            PartnerCenter.Models.CountryValidationRules.CountryValidationRules rules
                = await operations.CountryValidationRules.ByCountry(AppConfig.CountryCode).GetAsync();

            // TODO - Do not get the supported states list each time. This data should be cached so the forms will load more rapidly.
            NewCustomerModel newCustomerModel = new NewCustomerModel()
            {
                SupportedStates = rules.SupportedStatesList
            };

            return PartialView(newCustomerModel);
        }

        /// <summary>
        /// Create the customer represented by the instance of <see cref="NewCustomerModel"/>.
        /// </summary>
        /// <param name="newCustomerModel">The new customer model.</param>
        /// <returns>A partial view containing result of the customer creation.</returns>
        [HttpPost]
        public async Task<PartialViewResult> Create(NewCustomerModel newCustomerModel)
        {
            Customer entity;
            CreatedCustomerModel createdCustomerModel;
            IAggregatePartner operations;

            try
            {
                operations = await new SdkContext().GetPartnerOperationsAysnc();
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
                            Country = AppConfig.CountryCode,
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
                        Domain = $"{newCustomerModel.PrimaryDomain}.onmicrosoft.com"
                    }
                };

                entity = await operations.Customers.CreateAsync(entity);

                createdCustomerModel = new CreatedCustomerModel()
                {
                    Domain = $"{newCustomerModel.PrimaryDomain}.onmicrosoft.com",
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

        public async Task<ActionResult> List()
        {
            CustomersModel customersModel = new CustomersModel()
            {
                Customers = await GetCustomerModelsAsync(),
                IsSandboxEnvironment = AppConfig.IsSandboxEnvironment
            };

            return PartialView(customersModel);
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Handles the request to view the customer associated with the specified customer identifier.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>A view to display the customer details.</returns>
        /// <exception cref="System.ArgumentNullException">customerId</exception>
        public async Task<ActionResult> Show(string customerId)
        {
            Customer customer;
            CustomerModel customerModel;
            IAggregatePartner operations;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            try
            {
                operations = await new SdkContext().GetPartnerOperationsAysnc();
                customer = await operations.Customers.ById(customerId).GetAsync();

                customerModel = new CustomerModel()
                {
                    BillingProfile = customer.BillingProfile,
                    CompanyName = customer.CompanyProfile.CompanyName,
                    CompanyProfile = customer.CompanyProfile,
                    DomainName = customer.CompanyProfile.Domain,
                    CustomerId = customer.CompanyProfile.TenantId
                };

                return View(customerModel);
            }
            finally
            {
                customer = null;
                customerModel = null;
                operations = null;
            }
        }

        private async Task<List<CustomerModel>> GetCustomerModelsAsync()
        {
            IAggregatePartner operations;
            ResourceCollection<Customer> customers;

            try
            {
                operations = await new SdkContext().GetPartnerOperationsAysnc();
                customers = await operations.Customers.GetAsync();

                return customers.Items.Select(item => new CustomerModel()
                {
                    BillingProfile = new CustomerBillingProfile()
                    {
                        CompanyName = item.CompanyProfile.CompanyName,
                        DefaultAddress = new Address()
                        {
                            // AddressLine1 = item.BillingProfile.DefaultAddress.AddressLine1,
                            // AddressLine2 = item.BillingProfile.DefaultAddress.AddressLine2,
                            // City = item.BillingProfile.DefaultAddress.City,
                            //Country = item.BillingProfile.DefaultAddress.Country,
                            //FirstName = item.BillingProfile.FirstName,
                            //LastName = item.BillingProfile.LastName,
                            // PhoneNumber = item.BillingProfile.DefaultAddress.PhoneNumber,
                            // PostalCode = item.BillingProfile.DefaultAddress.PostalCode,
                            // Region = item.BillingProfile.DefaultAddress.Region,
                            // State = item.BillingProfile.DefaultAddress.State
                        }
                    },
                    CompanyName = item.CompanyProfile.CompanyName,
                    CustomerId = item.Id,
                    CompanyProfile = new CustomerCompanyProfile()
                    {
                        CompanyName = item.CompanyProfile.CompanyName,
                        Domain = item.CompanyProfile.Domain,
                        TenantId = item.CompanyProfile.TenantId
                    },
                    DomainName = item.CompanyProfile.Domain,
                    RelationshipToPartner = item.RelationshipToPartner.ToString()
                }).ToList();
            }
            finally
            {
                customers = null;
                operations = null;
            }
        }
    }
}