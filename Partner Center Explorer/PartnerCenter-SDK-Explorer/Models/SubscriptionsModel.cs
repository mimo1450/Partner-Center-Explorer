// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model used to list subscriptions.
    /// </summary>
    public class SubscriptionsModel
    {
        /// <summary>
        /// Gets or sets the list of subscriptions owned by the customer.
        /// </summary>
        /// <value>
        /// The subscriptions owned by the customer.
        /// </value>
        public List<SubscriptionModel> Subscriptions
        { get; set; }
    }
}