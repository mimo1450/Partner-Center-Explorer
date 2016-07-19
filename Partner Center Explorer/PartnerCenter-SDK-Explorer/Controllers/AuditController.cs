// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models.Query;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models;
using System;
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
        private SdkContext _context;

        /// <summary>
        /// Get the audit records for the specified date range using the Partner Center API.
        /// </summary>
        /// <param name="endDate">The end date of the audit record logs.</param>
        /// <param name="startDate">The start date of the audit record logs.</param>
        /// <returns></returns>
        public ActionResult GetRecords(DateTime endDate, DateTime startDate)
        {
            AuditRecordsModel auditRecordsModel = new AuditRecordsModel()
            {
                Records = Context.PartnerOperations.AuditRecords.Query(startDate, endDate, QueryFactory.Instance.BuildSimpleQuery())
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

        private SdkContext Context => _context ?? (_context = new SdkContext());
    }
}