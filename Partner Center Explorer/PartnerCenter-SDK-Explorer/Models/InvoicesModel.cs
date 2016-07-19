// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.Invoices;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model for listing all invoices.
    /// </summary>
    public class InvoicesModel
    {
        /// <summary>
        /// Gets or sets the invoices.
        /// </summary>
        /// <value>
        /// All of the invoices that belong to the configured account identifier.
        /// </value>
        public ResourceCollection<Invoice> Invoices
        { get; set; }
    }
}