// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models.Invoices;
using System.Collections.Generic;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model for displaying invoice details.
    /// </summary>
    public class InvoiceDetailsModel
    {
        /// <summary>
        /// Gets or sets the customers.
        /// </summary>
        /// <value>
        /// The customers contained within the invoice line items.
        /// </value>
        public List<string> Customers
        { get; set; }

        /// <summary>
        /// Gets or sets the invoice identifier.
        /// </summary>
        /// <value>
        /// The invoice identifier value.
        /// </value>
        public string InvoiceId
        { get; set; }

        /// <summary>
        /// Gets or sets the invoice line items.
        /// </summary>
        /// <value>
        /// The invoice line items associated with the specified invoice identifier.
        /// </value>
        public List<InvoiceLineItem> InvoiceLineItems
        { get; set; }
    }
}