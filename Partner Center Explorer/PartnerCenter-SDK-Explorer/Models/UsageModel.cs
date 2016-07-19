// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.Usage;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model for Azure subscription usage details.
    /// </summary>
    public class UsageModel
    {
        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        public string CompanyName
        { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier that owns the usage data.
        /// </value>
        public string CustomerId
        { get; set; }

        /// <summary>
        /// Gets or sets the daily usage.
        /// </summary>
        /// <value>
        /// The daily subscription usage.
        /// </value>
        public ResourceCollection<SubscriptionDailyUsageRecord> DailyUsage
        { get; set; }

        /// <summary>
        /// Gets or sets the monthly usage.
        /// </summary>
        /// <value>
        /// The monthly subscription usage.
        /// </value>
        public ResourceCollection<AzureResourceMonthlyUsageRecord> MonthlyUsage
        { get; set; }

        /// <summary>
        /// Gets or sets the subscription identifier.
        /// </summary>
        /// <value>
        /// The subscription identifier that owns the usage.
        /// </value>
        public string SubscriptionId
        { get; set; }

        /// <summary>
        /// Gets or sets the subsciption nick name.
        /// </summary>
        /// <value>
        /// The subscription nick name value.
        /// </value>
        public string SubscriptionFriendlyName
        { get; set; }
    }
}