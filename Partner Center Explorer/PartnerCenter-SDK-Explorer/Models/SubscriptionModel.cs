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

        public BillingType BillingType
        { get; set; }

        public DateTime CommitmentEndDate
        { get; set; }

        public string CompanyName
        { get; set; }

        public DateTime CreationDate
        { get; set; }

        public string CustomerId
        { get; set; }

        public DateTime EffectiveStartDate
        { get; set; }

        [Display(Name = "Subscription Nickname")]
        [Required]
        public string FriendlyName
        { get; set; }

        public string Id
        { get; set; }

        public string OfferId
        { get; set; }

        public string OfferName
        { get; set; }

        public string ParentSubscriptionId
        { get; set; }

        public string PartnerId
        { get; set; }

        public int Quantity
        { get; set; }

        [Required]
        public SubscriptionStatus Status
        { get; set; }

        public IEnumerable<string> SuspensionReasons
        { get; set; }

        public string UnitType
        { get; set; }

        public string ViewModel
        { get; set; }
    }
}