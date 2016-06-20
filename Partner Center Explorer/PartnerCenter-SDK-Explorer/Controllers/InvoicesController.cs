// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CsvHelper;
using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.Invoices;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    [AuthorizationFilter(ClaimType = ClaimTypes.Role, ClaimValue = "PartnerAdmin")]
    public class InvoicesController : Controller
    {
        private SdkContext _context;

        public ActionResult AzureDetails(string customerName, string invoiceId)
        {
            if (string.IsNullOrEmpty(customerName))
            {
                throw new ArgumentNullException("cusotmerName");
            }
            else if (string.IsNullOrEmpty(invoiceId))
            {
                throw new ArgumentNullException("invoiceId");
            }

            InvoiceDetailsModel invoiceDetailsModel = new InvoiceDetailsModel()
            {
                InvoiceLineItems = GetInvoiceLineItems(invoiceId, customerName, "Azure")
            };

            return PartialView(invoiceDetailsModel);
        }

        public ActionResult Index()
        {
            InvoicesModel invoicesModel = new InvoicesModel()
            {
                Invoices = Context.PartnerOperations.Invoices.Get()
            };

            return View(invoicesModel);
        }

        /// <summary>
        /// Gets a list of customers included in the specified invoice. 
        /// </summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>
        /// A distinict list of customers included in the specified invoice.
        /// </returns>
        /// <remarks>
        /// This information is extracted from the invoice line items to  
        /// ensure that the only customers on the invoice will be shown. This 
        /// data is used to populate the ddlCustomers drop down on the Index view. 
        /// </remarks>
        [HttpGet]
        public JsonResult GetCustomers(string invoiceId)
        {
            List<string> customers;
            List<InvoiceLineItem> lineItems;

            try
            {
                customers = new List<string>();
                lineItems = GetInvoiceLineItems(invoiceId);

                customers.AddRange(
                    lineItems
                        .Where(x => x.GetType() == typeof(DailyUsageLineItem))
                        .Cast<DailyUsageLineItem>()
                        .Select(x => x.CustomerCompanyName)
                );

                customers.AddRange(
                    lineItems
                        .Where(x => x.GetType() == typeof(LicenseBasedLineItem))
                        .Cast<LicenseBasedLineItem>()
                        .Select(x => x.CustomerName)
                );

                customers.AddRange(
                    lineItems
                        .Where(x => x.GetType() == typeof(UsageBasedLineItem))
                        .Cast<UsageBasedLineItem>()
                        .Select(x => x.CustomerCompanyName)
                );

                return Json(customers.Distinct(), JsonRequestBehavior.AllowGet);
            }
            finally
            {
                customers = null;
                lineItems = null;
            }
        }

        public ActionResult Details(string invoiceId)
        {
            if (string.IsNullOrEmpty(invoiceId))
            {
                return RedirectToAction("Index", "Invoices");
            }

            InvoiceDetailsModel invoiceDetailsModel = new InvoiceDetailsModel()
            {
                InvoiceId = invoiceId,
            };

            return View(invoiceDetailsModel);
        }

        /// <summary>
        /// Exports specific line items from a given invoice.
        /// </summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <param name="customerName">Name of the customer.</param>
        /// <param name="providerType">Type of the provider.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// invoiceId
        /// or
        /// customerName
        /// or
        /// providerType
        /// </exception>
        public FileContentResult ExportCustomer(string invoiceId, string customerName, string providerType)
        {
            byte[] data;

            if (string.IsNullOrEmpty(invoiceId))
            {
                throw new ArgumentNullException("invoiceId");
            }
            else if (string.IsNullOrEmpty(customerName))
            {
                throw new ArgumentNullException("customerName");
            }
            else if (string.IsNullOrEmpty(providerType))
            {
                throw new ArgumentNullException("providerType");
            }

            try
            {
                if (providerType.Equals("Azure", StringComparison.OrdinalIgnoreCase))
                {
                    data = GetUsageRecords(invoiceId, customerName);
                }
                else
                {
                    data = GetLicensedRecords(invoiceId, customerName);
                }

                return File(data.ToArray(), "text/csv", string.Format("Invoice-{0}-{1}-{2}.csv", invoiceId, customerName, providerType));

            }
            finally
            {
                data = null;
            }
        }

        public ActionResult OfficeDetails(string customerName, string invoiceId)
        {
            if (string.IsNullOrEmpty(customerName))
            {
                throw new ArgumentNullException("cusotmerName");
            }
            else if (string.IsNullOrEmpty(invoiceId))
            {
                throw new ArgumentNullException("invoiceId");
            }

            InvoiceDetailsModel invoiceDetailsModel = new InvoiceDetailsModel()
            {
                InvoiceLineItems = GetInvoiceLineItems(invoiceId, customerName, "Office")
            };

            return PartialView(invoiceDetailsModel);
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

        /// <summary>
        /// Gets a collection of resources representing line items in the specified invoice. 
        /// </summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>A collection of line items for the specified invoice.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <remarks>
        /// This function utilizes the Partner Center SDK to obtain a collection of invoice line items 
        /// for the specified invoice. Additional details regarding the SDK calls used in this function can 
        /// be found at https://msdn.microsoft.com/en-us/library/partnercenter/mt712733.aspx
        /// </remarks>
        private List<InvoiceLineItem> GetInvoiceLineItems(string invoiceId)
        {
            Invoice invoice;
            List<InvoiceLineItem> lineItems;
            ResourceCollection<InvoiceLineItem> data;
            string cacheName;

            if (string.IsNullOrEmpty(invoiceId))
            {
                throw new ArgumentNullException(invoiceId);
            }

            try
            {
                cacheName = string.Format("{0}_LineItems", invoiceId);

                lineItems = MemoryCache.Default[cacheName] as List<InvoiceLineItem>;

                if (lineItems == null)
                {
                    invoice = Context.PartnerOperations.Invoices.ById(invoiceId).Get();
                    lineItems = new List<InvoiceLineItem>();

                    foreach (InvoiceDetail detail in invoice.InvoiceDetails)
                    {
                        data = Context.PartnerOperations.Invoices.ById(invoiceId).By(detail.BillingProvider, detail.InvoiceLineItemType).Get();
                        lineItems.AddRange(data.Items);
                    }

                    MemoryCache.Default[cacheName] = lineItems;
                }

                return lineItems;
            }
            finally
            {
                lineItems = null;
            }
        }

        private List<InvoiceLineItem> GetInvoiceLineItems(string invoiceId, string customerName, string providerType)
        {
            List<InvoiceLineItem> items;

            if (string.IsNullOrEmpty(invoiceId))
            {
                throw new ArgumentNullException("invoiceId");
            }
            else if (string.IsNullOrEmpty(customerName))
            {
                throw new ArgumentNullException("customerName");
            }
            else if (string.IsNullOrEmpty(providerType))
            {
                throw new ArgumentNullException("providerType");
            }

            try
            {
                if (providerType.Equals("Azure", StringComparison.OrdinalIgnoreCase))
                {
                    items = GetInvoiceLineItems(invoiceId)
                        .Where(x => x.GetType() == typeof(UsageBasedLineItem))
                        .Cast<UsageBasedLineItem>()
                        .Where(x => x.CustomerCompanyName.Equals(customerName, StringComparison.OrdinalIgnoreCase))
                        .ToList<InvoiceLineItem>();
                }
                else if (providerType.Equals("Office", StringComparison.OrdinalIgnoreCase))
                {
                    items = GetInvoiceLineItems(invoiceId)
                        .Where(x => x.GetType() == typeof(LicenseBasedLineItem))
                        .Cast<LicenseBasedLineItem>()
                        .Where(x => x.CustomerName.Equals(customerName, StringComparison.OrdinalIgnoreCase))
                        .ToList<InvoiceLineItem>();
                }
                else
                {
                    items = new List<InvoiceLineItem>();
                }

                return items;
            }
            finally
            {
                items = null;
            }
        }

        private byte[] GetLicensedRecords(string invoiceId, string customerName)
        {
            List<LicenseBasedLineItem> items;

            if (string.IsNullOrEmpty(invoiceId))
            {
                throw new ArgumentNullException("invoiceId");
            }
            else if (string.IsNullOrEmpty(customerName))
            {
                throw new ArgumentNullException("customerName");
            }

            try
            {
                items = GetInvoiceLineItems(invoiceId, customerName, "Office").Cast<LicenseBasedLineItem>().ToList();

                using (MemoryStream stream = new MemoryStream())
                {
                    using (TextWriter textWriter = new StreamWriter(stream))
                    {
                        using (CsvWriter writer = new CsvWriter(textWriter))
                        {
                            writer.WriteRecords(items);
                            writer.NextRecord();
                            textWriter.Flush();
                            return stream.ToArray();
                        }
                    }
                }
            }
            finally
            {
                items = null;
            }
        }

        private byte[] GetUsageRecords(string invoiceId, string customerName)
        {
            List<UsageBasedLineItem> items;

            if (string.IsNullOrEmpty(invoiceId))
            {
                throw new ArgumentNullException("invoiceId");
            }
            else if (string.IsNullOrEmpty(customerName))
            {
                throw new ArgumentNullException("customerName");
            }

            try
            {
                items = GetInvoiceLineItems(invoiceId, customerName, "Azure").Cast<UsageBasedLineItem>().ToList();

                using (MemoryStream stream = new MemoryStream())
                {
                    using (TextWriter textWriter = new StreamWriter(stream))
                    {
                        using (CsvWriter writer = new CsvWriter(textWriter))
                        {
                            writer.WriteRecords(items);
                            writer.NextRecord();
                            textWriter.Flush();
                            return stream.ToArray();
                        }
                    }
                }
            }
            finally
            {
                items = null;
            }
        }
    }
}