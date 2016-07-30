// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models.Query;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    /// <summary>
    /// Controller for Audit views.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [AuthorizationFilter(ClaimType = ClaimTypes.Role, ClaimValue = "PartnerAdmin")]
    public class AuditController : Controller
    {
        /// <summary>
        /// Get the audit records for the specified date range using the Partner Center API.
        /// </summary>
        /// <param name="endDate">The end date of the audit record logs.</param>
        /// <param name="startDate">The start date of the audit record logs.</param>
        /// <returns>A partial view containing the requested audit record logs.</returns>
        public async Task<PartialViewResult> GetRecords(DateTime endDate, DateTime startDate)
        {
            IAggregatePartner operations = await new SdkContext().GetPartnerOperationsAysnc();

            AuditRecordsModel auditRecordsModel = new AuditRecordsModel()
            {
                Records = operations.AuditRecords.Query(startDate, endDate, QueryFactory.Instance.BuildSimpleQuery())
            };

            return PartialView("Records", auditRecordsModel);
        }

        /// <summary>
        /// Handles the request for the Search view.
        /// </summary>
        /// <returns>Returns an empty view.</returns>
        public ActionResult Search()
        {
            return View();
        }
    }
}