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
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    [AuthorizationFilter(ClaimType = ClaimTypes.Role, ClaimValue = "PartnerAdmin")]
    public class InvoicesController : Controller
    {
        private SdkContext _context;

        public async Task<ActionResult> AzureDetails(string customerName, string invoiceId)
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
                InvoiceLineItems = await GetInvoiceLineItemsAsync(invoiceId, customerName, "Azure")
            };

            return PartialView(invoiceDetailsModel);
        }

        public async Task<ActionResult> Index()
        {
            InvoicesModel invoicesModel = new InvoicesModel()
            {
                Invoices = await Context.PartnerOperations.Invoices.GetAsync()
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
        public async Task<JsonResult> GetCustomers(string invoiceId)
        {
            List<string> customers;
            List<InvoiceLineItem> lineItems;

            try
            {
                customers = new List<string>();
                lineItems = await GetInvoiceLineItemsAsync(invoiceId);

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
        public async Task<FileContentResult> ExportCustomer(string invoiceId, string customerName, string providerType)
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
                    data = await GetUsageRecordsAsync(invoiceId, customerName);
                }
                else
                {
                    data = await GetLicensedRecordsAsync(invoiceId, customerName);
                }

                return File(data.ToArray(), "text/csv", string.Format("Invoice-{0}-{1}-{2}.csv", invoiceId, customerName, providerType));

            }
            finally
            {
                data = null;
            }
        }

        public async Task<ActionResult> OfficeDetails(string customerName, string invoiceId)
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
                InvoiceLineItems = await GetInvoiceLineItemsAsync(invoiceId, customerName, "Office")
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
        private async Task<List<InvoiceLineItem>> GetInvoiceLineItemsAsync(string invoiceId)
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
                    invoice = await Context.PartnerOperations.Invoices.ById(invoiceId).GetAsync();
                    lineItems = new List<InvoiceLineItem>();

                    foreach (InvoiceDetail detail in invoice.InvoiceDetails)
                    {
                        data = await Context.PartnerOperations.Invoices.ById(invoiceId).By(detail.BillingProvider, detail.InvoiceLineItemType).GetAsync();
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

        private async Task<List<InvoiceLineItem>> GetInvoiceLineItemsAsync(string invoiceId, string customerName, string providerType)
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
                    items = await GetInvoiceLineItemsAsync(invoiceId);

                    return items
                        .Where(x => x.GetType() == typeof(UsageBasedLineItem))
                        .Cast<UsageBasedLineItem>()
                        .Where(x => x.CustomerCompanyName.Equals(customerName, StringComparison.OrdinalIgnoreCase))
                        .ToList<InvoiceLineItem>();
                }
                else if (providerType.Equals("Office", StringComparison.OrdinalIgnoreCase))
                {
                    items = await GetInvoiceLineItemsAsync(invoiceId);

                    return items
                        .Where(x => x.GetType() == typeof(LicenseBasedLineItem))
                        .Cast<LicenseBasedLineItem>()
                        .Where(x => x.CustomerName.Equals(customerName, StringComparison.OrdinalIgnoreCase))
                        .ToList<InvoiceLineItem>();
                }
                else
                {
                    return new List<InvoiceLineItem>();
                }
            }
            finally
            {
                items = null;
            }
        }

        private async Task<byte[]> GetLicensedRecordsAsync(string invoiceId, string customerName)
        {
            List<InvoiceLineItem> data;
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
                data = await GetInvoiceLineItemsAsync(invoiceId, customerName, "Office");
                items = data.Cast<LicenseBasedLineItem>().ToList();

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

        private async Task<byte[]> GetUsageRecordsAsync(string invoiceId, string customerName)
        {
            List<InvoiceLineItem> data;
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
                data = await GetInvoiceLineItemsAsync(invoiceId, customerName, "Azure");
                items = data.Cast<UsageBasedLineItem>().ToList();

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
                data = null;
                items = null;
            }
        }
    }
}