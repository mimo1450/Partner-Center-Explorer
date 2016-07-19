// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models.Invoices;
using Microsoft.Store.PartnerCenter.Models.Subscriptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model for subscriptions managed though Partner Center.
    /// </summary>
    public class SubscriptionModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether automatic renewal is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if automatic renewal is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool AutoRenewEnabled
        { get; set; }

        /// <summary>
        /// Gets or sets the type of the billing.
        /// </summary>
        /// <value>
        /// The type of the billing.
        /// </value>
        public BillingType BillingType
        { get; set; }

        /// <summary>
        /// Gets or sets the commitment end date.
        /// </summary>
        /// <value>
        /// The commitment end date.
        /// </value>
        public DateTime CommitmentEndDate
        { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        public string CompanyName
        { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>
        /// The creation date for the subscription.
        /// </value>
        public DateTime CreationDate
        { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier that owns the subscription.
        /// </value>
        public string CustomerId
        { get; set; }

        /// <summary>
        /// Gets or sets the effective start date.
        /// </summary>
        /// <value>
        /// The effective start date.
        /// </value>
        public DateTime EffectiveStartDate
        { get; set; }

        /// <summary>
        /// Gets or sets the name of the nick name of the subscription.
        /// </summary>
        /// <value>
        /// The nick name of the subscription.
        /// </value>
        [Display(Name = "Subscription Nickname")]
        [Required]
        public string FriendlyName
        { get; set; }

        /// <summary>
        /// Gets or sets the subscription identifier.
        /// </summary>
        /// <value>
        /// The subscription identifier value.
        /// </value>
        public string Id
        { get; set; }

        /// <summary>
        /// Gets or sets the offer identifier.
        /// </summary>
        /// <value>
        /// The offer identifier used to generate the subscription.
        /// </value>
        public string OfferId
        { get; set; }

        /// <summary>
        /// Gets or sets the name of the offer.
        /// </summary>
        /// <value>
        /// The name of the offer used to generate the subscription.
        /// </value>
        public string OfferName
        { get; set; }

        /// <summary>
        /// Gets or sets the parent subscription identifier.
        /// </summary>
        /// <value>
        /// The parent subscription identifier.
        /// </value>
        public string ParentSubscriptionId
        { get; set; }

        /// <summary>
        /// Gets or sets the partner identifier.
        /// </summary>
        /// <value>
        /// The partner identifier who owns the subscription.
        /// </value>
        public string PartnerId
        { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity of units for the subscriptions.
        /// </value>
        /// <remarks>
        /// This value is typically used for Office subscriptions. 
        /// </remarks>
        public int Quantity
        { get; set; }

        /// <summary>
        /// Gets or sets the subscription status.
        /// </summary>
        /// <value>
        /// The status of the subscription.
        /// </value>
        [Required]
        public SubscriptionStatus Status
        { get; set; }

        /// <summary>
        /// Gets or sets the suspension reasons.
        /// </summary>
        /// <value>
        /// The suspension reasons for the subscription.
        /// </value>
        /// <remarks>
        /// If the subscription is not suspended then this will be null.
        /// </remarks>
        public IEnumerable<string> SuspensionReasons
        { get; set; }

        /// <summary>
        /// Gets or sets the type of the units used by the subscription.
        /// </summary>
        /// <value>
        /// The type of the units used by the subscription.
        /// </value>
        public string UnitType
        { get; set; }

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model to be rendered.
        /// </value>
        /// <remarks>
        /// Azure or Office are the only valid values for this property.
        /// </remarks>
        public string ViewModel
        { get; set; }
    }
}