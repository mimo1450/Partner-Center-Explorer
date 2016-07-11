// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Subscriptions;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// View model for managing subscriptions.
    /// </summary>
    public class SubscriptionManageModel
    {
        /// <summary>
        /// Gets or sets the name of the company who owns the subscription.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        public string CompanyName
        { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier assigned to the customer who owns the subscription.
        /// </summary>
        /// <value>
        /// The customer identifier value.
        /// </value>
        public string CustomerId
        { get; set; }

        /// <summary>
        /// Gets or sets the name of the friendly of the subscription.
        /// </summary>
        /// <value>
        /// The friendly name of the subscription.
        /// </value>
        public string FriendlyName
        { get; set; }

        /// <summary>
        /// Gets or sets the subscription details.
        /// </summary>
        /// <value>
        /// The subscription details.
        /// </value>
        public ISubscriptionDetails SubscriptionDetails
        { get; set; }

        /// <summary>
        /// Gets or sets the subscription identifier.
        /// </summary>
        /// <value>
        /// The subscription identifier value.
        /// </value>
        public string SubscriptionId
        { get; set; }

        /// <summary>
        /// Gets or sets the type of the subscription.
        /// </summary>
        /// <value>
        /// The type of the subscription.
        /// </value>
        /// <remarks>The value will either be Azure or Office.</remarks>
        public string ViewName
        { get; set; }
    }
}