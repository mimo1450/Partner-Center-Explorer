// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models.Invoices;
using System.Collections.Generic;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    public class InvoiceDetailsModel
    {
        public List<string> Customers
        { get; set; }

        public string InvoiceId
        { get; set; }

        public List<InvoiceLineItem> InvoiceLineItems
        { get; set; }
    }
}