// -----------------------------------------------------------------------
// <copyright file="CustomersController.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Explorer.Controllers
{
    using Configuration;
    using Logic.Authentication;
    using Microsoft.Store.PartnerCenter.Models;
    using Microsoft.Store.PartnerCenter.Models.Customers;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    /// <summary>
    /// Provides the ability to manage customers.
    /// </summary>
    [RoutePrefix("api/customers")]
    public class CustomerController : ApiController
    {
        /// <summary>
        /// Retrieves a list of all customers.
        /// </summary>
        /// <returns>
        /// The list of customers utilized for rendering purpose.
        /// </returns>
        /// <remarks>
        /// If the authenticated users is a customer administrator then the only 
        /// customer in the list will be the one they are associated with. This 
        /// is done for security purposes. 
        /// </remarks>
        public async Task<CustomersViewModel> GetCustomers()
        {
            Customer customer;
            CustomerPrincipal principal;
            CustomersViewModel viewModel;
            IAggregatePartner partner;
            SeekBasedResourceCollection<Customer> customers;

            try
            {
                partner = PartnerService.Instance.CreatePartnerOperations(
                    new TokenManagement().GetPartnerCenterAppOnlyCredentials(
                        $"{ApplicationConfiguration.ActiveDirectoryEndpoint}/{ApplicationConfiguration.AccountId}"));

                principal = HttpContext.Current.User as CustomerPrincipal;

                viewModel = new CustomersViewModel()
                {
                    Customers = new List<Customer>()
                };

                if (principal.IsAdmin)
                {
                    customers = await partner.Customers.GetAsync();
                    viewModel.Customers.AddRange(customers.Items);
                }
                else
                {
                    // The authenticated users is not a partner administrator. This means the
                    // user is a global administrator for the customer their account is 
                    // associated with.In this situation only the customer the authenticated 
                    // user belongs to should be returned.
                    customer = await partner.Customers.ById(principal.CustomerId).GetAsync();
                    viewModel.Customers.Add(customer);
                }

                return viewModel;
            }
            finally
            {
                customer = null;
                customers = null;
                partner = null;
                principal = null;
            }
        }
    }
}