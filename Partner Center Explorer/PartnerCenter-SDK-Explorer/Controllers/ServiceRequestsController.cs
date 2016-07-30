// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.ServiceRequests;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    /// <summary>
    /// Handles all request for Service Requests views.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [AuthorizationFilter(ClaimType = ClaimTypes.Role, ClaimValue = "PartnerAdmin")]
    public class ServiceRequestsController : Controller
    {
        /// <summary>
        /// Handles the Index view request.
        /// </summary>
        /// <returns>
        /// A view containing an instnace of <see cref="ServiceRequestsController"/>
        /// </returns>
        public ActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> List()
        {
            ServiceRequestsModel serviceRequestsModel = new ServiceRequestsModel()
            {
                ServiceRequests = await GetServiceRequestsAsync()
            };

            return PartialView(serviceRequestsModel);
        }

        private static async Task<List<ServiceRequestModel>> GetServiceRequestsAsync()
        {
            IAggregatePartner operations;
            ResourceCollection<ServiceRequest> requests;

            try
            {
                operations = await new SdkContext().GetPartnerOperationsAysnc();
                requests = await operations.ServiceRequests.GetAsync();

                return requests.Items.Select(r => new ServiceRequestModel()
                {
                    CreatedDate = r.CreatedDate,
                    Id = r.Id,
                    Organization = r.Organization?.Name,
                    PrimaryContact = r.PrimaryContact?.Email,
                    ProductName = r.ProductName,
                    Status = r.Status.ToString(),
                    Title = r.Title
                }).ToList();
            }
            finally
            {
                operations = null;
                requests = null;
            }
        }
    }
}