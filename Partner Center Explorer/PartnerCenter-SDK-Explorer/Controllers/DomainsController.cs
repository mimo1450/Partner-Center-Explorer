// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Samples.AzureAD.Graph.API;
using Microsoft.Samples.AzureAD.Graph.API.Models;
using Microsoft.Store.PartnerCenter.Samples.Common;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    /// <summary>
    /// Controller for all Domain views.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [AuthorizationFilter(ClaimType = ClaimTypes.Role, ClaimValue = "PartnerAdmin")]
    public class DomainsController : Controller
    {
        /// <summary>
        /// Handles the request to view Office 365 service configuration records.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="domain">The domain that service configuration records should be returned.</param>
        /// <returns>A partial view containing the configuration records for the specified domain.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// customerId
        /// or
        /// domain
        /// </exception>
        [HttpGet]
        public async Task<PartialViewResult> ConfigurationRecords(string customerId, string domain)
        {
            AuthenticationResult token;
            ConfigurationRecordsModel domainDetailsModel;
            GraphClient client;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            if (string.IsNullOrEmpty(domain))
            {
                throw new ArgumentNullException(nameof(domain));
            }

            try
            {
                token = await TokenContext.GetAADTokenAsync(
                    $"{AppConfig.Authority}/{customerId}",
                    AppConfig.GraphUri
                );

                client = new GraphClient(token.AccessToken);

                domainDetailsModel = new ConfigurationRecordsModel()
                {
                    ServiceConfigurationRecords = await client.GetServiceConfigurationRecordsAsync(customerId, domain)
                };

                return PartialView(domainDetailsModel);
            }
            finally
            {
                client = null;
                token = null;
            }
        }

        /// <summary>
        /// Determines whether the specified domain is available or not.
        /// </summary>
        /// <param name="primaryDomain">The domain prefix to be checked.</param>
        /// <returns><c>true</c> if the domain available; otherwise <c>false</c> is returned.</returns>
        /// <remarks>
        /// This checks if the specified domain is available using the Partner Center API. A domain is
        /// considered to be available if the domain is not already in used by another Azure AD tenant.
        /// </remarks>
        public async Task<JsonResult> IsDomainAvailable(string primaryDomain)
        {
            IAggregatePartner operations = await new SdkContext().GetPartnerOperationsAysnc();

            if (string.IsNullOrEmpty(primaryDomain))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            bool exists = await operations.Domains.ByDomain(primaryDomain + ".onmicrosoft.com").ExistsAsync();

            return Json(!exists, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lists the domains for the specified customer identifier.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>A collection of domains that belong to the specified customer identifier.</returns>
        /// <exception cref="System.ArgumentNullException">customerId</exception>
        [HttpGet]
        public async Task<JsonResult> List(string customerId)
        {
            AuthenticationResult token;
            GraphClient client;
            List<Domain> domains;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            try
            {
                token = await TokenContext.GetAADTokenAsync(
                    $"{AppConfig.Authority}/{customerId}",
                    AppConfig.GraphUri
                );

                client = new GraphClient(token.AccessToken);
                domains = await client.GetDomainsAsync(customerId);

                return Json(domains, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                client = null;
                domains = null;
                token = null;
            }
        }
    }
}