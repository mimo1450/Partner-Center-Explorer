// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Samples.Common.Models;
using System.Collections.Generic;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model for the subscription health view.
    /// </summary>
    public class SubscriptionHealthModel
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
        /// The customer identifier who owns the subscription.
        /// </value>
        public string CustomerId
        { get; set; }

        /// <summary>
        /// Gets or sets the subscription nick name.
        /// </summary>
        /// <value>
        /// The nick name assigned to the subscription.
        /// </value>
        public string FriendlyName
        { get; set; }

        /// <summary>
        /// Gets or sets the health events.
        /// </summary>
        /// <value>
        /// The health events associated with the subscription.
        /// </value>
        public List<IHealthEvent> HealthEvents
        { get; set; }

        /// <summary>
        /// Gets or sets the subscription identifier.
        /// </summary>
        /// <value>
        /// The subscription identifier that health events were returned.
        /// </value>
        public string SubscriptionId
        { get; set; }

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model which should be rendered.
        /// </value>
        /// <remarks>
        /// Azure and Office are the only valid values for this property.
        /// </remarks>
        public string ViewModel
        { get; set; }
    }
}