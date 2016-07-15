// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models.Orders;
using System.Collections.Generic;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model for creating new orders for Partner Center.
    /// </summary>
    public class NewSubscriptionModel
    {
        /// <summary>
        /// Gets or sets the collection avaialable offers.
        /// </summary>
        /// <value>
        /// Collection of avaialable offers from Partner Center.
        /// </value>
        public List<OfferModel> AvaialableOffers
        { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier for which the order should be created.
        /// </value>
        public string CustomerId
        { get; set; }

        /// <summary>
        /// Gets or sets the order line items.
        /// </summary>
        /// <value>
        /// Line item cotained within the order.
        /// </value>
        public List<OrderLineItem> LineItems
        { get; set; }
    }
}