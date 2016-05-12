using Microsoft.Samples.Office365.Management.API;
using Microsoft.Samples.Office365.Management.API.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    public class StatusController : Controller
    {

        /// <summary>
        /// Handles the HTTP get request for the Office view.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns></returns>
        public ActionResult Office(string tenantId)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                return RedirectToAction("Index", "Customers");
            }

            return View();
        }

        /// <summary>
        /// Get the specified messages using the Office 365 Service Communication API.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="messageIds">The message ids.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">tenantId</exception>
        public JsonResult GetMessages(string tenantId, string messageIds)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException("tenantId");
            }

            List<Message> messages;
            ServiceCommunications service;
            string[] ids;

            try
            {
                messages = new List<Message>();
                service = new ServiceCommunications();
                ids = messageIds.Split(',');

                foreach (string id in ids)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        messages.Add(service.GetMessage(tenantId, id));
                    }
                }

                return Json(new { Result = "OK", Records = messages, TotalRecordCount = messages.Count });
            }
            finally
            {
                messages = null;
                service = null;
            }
        }

        /// <summary>
        /// Get Office 365status details for specified tenant using the Office 365 Service Communication API.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">tenantId</exception>
        public JsonResult OfficeStatus(string tenantId)
        {
            List<StatusDetails> records;
            ServiceCommunications service;

            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException("tenantId");
            }

            try
            {
                service = new ServiceCommunications();
                records = service.GetCurrentStatus(tenantId);

                return Json(new { Result = "OK", Records = records, TotalRecordCount = records.Count });
            }
            finally
            {
                service = null;
            }
        }
    }
}