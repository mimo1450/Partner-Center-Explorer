// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models.Invoices;
using System.Collections.Generic;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model for offers avaible for the specific country from Partner Center.
    /// </summary>
    public class OfferModel
    {
        /// <summary>
        /// Gets or sets the billing type.
        /// </summary>
        /// <value>
        /// The billing type value.
        /// </value>
        public BillingType Billing
        { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description of the line item.
        /// </value>
        public string Description
        { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier for the offer.
        /// </value>
        public string Id
        { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this offer is add on.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this offer is add on; otherwise, <c>false</c>.
        /// </value>
        public bool IsAddOn
        { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this offer is available for purchase.
        /// </summary>
        /// <value>
        /// <c>true</c> if this offer is available for purchase; otherwise, <c>false</c>.
        /// </value>
        public bool IsAvailableForPurchase
        { get; set; }

        /// <summary>
        /// Gets or sets the maxium quantity.
        /// </summary>
        /// <value>
        /// The maxium quantity that can be ordered.
        /// </value>
        public int MaxiumQuantity
        { get; set; }

        /// <summary>
        /// Gets or sets the minimum quantity.
        /// </summary>
        /// <value>
        /// The minimum quantity that can be ordered.
        /// </value>
        public int MinimumQuantity
        { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name of the offer.
        /// </value>
        public string Name
        { get; set; }

        /// <summary>
        /// Gets or sets the prerequisite offers for this offer.
        /// </summary>
        /// <value>
        /// The prerequisite offers for this offer.
        /// </value>
        /// <remarks>
        /// All prerequisites must be fulfilled prior to adding this offer.
        /// </remarks>
        public IEnumerable<string> PrerequisiteOffers
        { get; set; }
    }
}