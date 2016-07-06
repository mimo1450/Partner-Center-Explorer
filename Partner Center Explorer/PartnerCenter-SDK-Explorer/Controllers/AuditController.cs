// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models.Query;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models;
using System;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    [AuthorizationFilter(ClaimType = ClaimTypes.Role, ClaimValue = "PartnerAdmin")]
    public class AuditController : Controller
    {
        private SdkContext _context;

        public ActionResult GetRecords(DateTime endDate, DateTime startDate)
        {
            AuditRecordsModel auditRecordsModel = new AuditRecordsModel()
            {
                Records = Context.PartnerOperations.AuditRecords.Query(startDate, endDate, QueryFactory.Instance.BuildSimpleQuery())
            };

            return PartialView("Records", auditRecordsModel);
        }

        public ActionResult Search()
        {
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