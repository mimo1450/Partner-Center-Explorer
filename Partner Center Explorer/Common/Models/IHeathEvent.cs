// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Store.PartnerCenter.Samples.Common.Models
{
    /// <summary>
    /// Interface for health events obtained from Azure Insights and Office 365 Service Communcations API.
    /// </summary>
    public interface IHealthEvent
    {
        /// <summary>
        /// Gets the type of the <see cref="IHealthEvent"/>.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        HealthEventType EventType
        { get; }

        /// <summary>
        /// Gets or sets the status of the <see cref="IHealthEvent"/>.
        /// </summary>
        /// <value>
        /// The status of the <see cref="IHealthEvent"/>.
        /// </value>
        string Status
        { get; set; }
    }
}