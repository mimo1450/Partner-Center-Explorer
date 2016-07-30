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
        /// <summary>
        /// Handles the partial view request for Azure details.
        /// </summary>
        /// <param name="customerName">Name of the customer.</param>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>A partial view containing the InvoiceDetailsModel model.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public async Task<PartialViewResult> AzureDetails(string customerName, string invoiceId)
        {
            if (string.IsNullOrEmpty(customerName))
            {
                throw new ArgumentNullException(nameof(customerName));
            }
            if (string.IsNullOrEmpty(invoiceId))
            {
                throw new ArgumentNullException(nameof(invoiceId));
            }

            InvoiceDetailsModel invoiceDetailsModel = new InvoiceDetailsModel()
            {
                InvoiceLineItems = await GetInvoiceLineItemsAsync(invoiceId, customerName, "Azure")
            };

            return PartialView(invoiceDetailsModel);
        }

        /// <summary>
        /// Handles the index view request.
        /// </summary>
        /// <returns>A view containing the InvoicesModel model.</returns>
        public async Task<ActionResult> Index()
        {
            IAggregatePartner operations = await new SdkContext().GetPartnerOperationsAysnc();

            InvoicesModel invoicesModel = new InvoicesModel()
            {
                Invoices = await operations.Invoices.GetAsync()
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

            if (string.IsNullOrEmpty(invoiceId))
            {
                throw new ArgumentNullException(nameof(invoiceId));
            }

            try
            {
                customers = new List<string>();
                lineItems = await GetInvoiceLineItemsAsync(invoiceId);

                customers.AddRange(
                    lineItems
                        .Where(x => x is DailyUsageLineItem)
                        .Cast<DailyUsageLineItem>()
                        .Select(x => x.CustomerCompanyName));

                customers.AddRange(
                    lineItems
                        .Where(x => x is LicenseBasedLineItem)
                        .Cast<LicenseBasedLineItem>()
                        .Select(x => x.CustomerName));

                customers.AddRange(
                    lineItems
                        .Where(x => x is UsageBasedLineItem)
                        .Cast<UsageBasedLineItem>()
                        .Select(x => x.CustomerCompanyName));

                return Json(customers.Distinct(), JsonRequestBehavior.AllowGet);
            }
            finally
            {
                customers = null;
                lineItems = null;
            }
        }

        /// <summary>
        /// Handles the Details view request.
        /// </summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>Return a vew with the InvoiceDetailsModel model.</returns>
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
                throw new ArgumentNullException(nameof(invoiceId));
            }
            if (string.IsNullOrEmpty(customerName))
            {
                throw new ArgumentNullException(nameof(customerName));
            }
            if (string.IsNullOrEmpty(providerType))
            {
                throw new ArgumentNullException(nameof(providerType));
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

                return File(data.ToArray(), "text/csv", $"Invoice-{invoiceId}-{customerName}-{providerType}.csv");
            }
            finally
            {
                data = null;
            }
        }

        /// <summary>
        /// Handles the request for the OfficeDetails partial view.
        /// </summary>
        /// <param name="customerName">Name of the customer.</param>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>A partial view containing the InvoiceDetailsModel model.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public async Task<PartialViewResult> OfficeDetails(string customerName, string invoiceId)
        {
            if (string.IsNullOrEmpty(customerName))
            {
                throw new ArgumentNullException(nameof(customerName));
            }
            if (string.IsNullOrEmpty(invoiceId))
            {
                throw new ArgumentNullException(nameof(invoiceId));
            }

            InvoiceDetailsModel invoiceDetailsModel = new InvoiceDetailsModel()
            {
                InvoiceLineItems = await GetInvoiceLineItemsAsync(invoiceId, customerName, "Office")
            };

            return PartialView(invoiceDetailsModel);
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
            IAggregatePartner operations;
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
                cacheName = $"{invoiceId}_LineItems";
                lineItems = MemoryCache.Default[cacheName] as List<InvoiceLineItem>;
                operations = await new SdkContext().GetPartnerOperationsAysnc();

                if (lineItems != null)
                {
                    return lineItems;
                }

                invoice = await operations.Invoices.ById(invoiceId).GetAsync();
                lineItems = new List<InvoiceLineItem>();

                foreach (InvoiceDetail detail in invoice.InvoiceDetails)
                {
                    data = await operations.Invoices.ById(invoiceId).By(detail.BillingProvider, detail.InvoiceLineItemType).GetAsync();
                    lineItems.AddRange(data.Items);
                }

                MemoryCache.Default[cacheName] = lineItems;

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
                throw new ArgumentNullException(nameof(invoiceId));
            }
            if (string.IsNullOrEmpty(customerName))
            {
                throw new ArgumentNullException(nameof(customerName));
            }
            if (string.IsNullOrEmpty(providerType))
            {
                throw new ArgumentNullException(nameof(providerType));
            }

            try
            {
                if (providerType.Equals("Azure", StringComparison.OrdinalIgnoreCase))
                {
                    items = await GetInvoiceLineItemsAsync(invoiceId);

                    return items
                        .Where(x => x is UsageBasedLineItem)
                        .Cast<UsageBasedLineItem>()
                        .Where(x => x.CustomerCompanyName.Equals(customerName, StringComparison.OrdinalIgnoreCase))
                        .ToList<InvoiceLineItem>();
                }
                else if (providerType.Equals("Office", StringComparison.OrdinalIgnoreCase))
                {
                    items = await GetInvoiceLineItemsAsync(invoiceId);

                    return items
                        .Where(x => x is LicenseBasedLineItem)
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
                throw new ArgumentNullException(nameof(invoiceId));
            }
            if (string.IsNullOrEmpty(customerName))
            {
                throw new ArgumentNullException(nameof(customerName));
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
                throw new ArgumentNullException(nameof(invoiceId));
            }
            if (string.IsNullOrEmpty(customerName))
            {
                throw new ArgumentNullException(nameof(customerName));
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