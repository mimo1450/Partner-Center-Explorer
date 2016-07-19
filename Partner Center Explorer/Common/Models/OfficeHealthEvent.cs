// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Store.PartnerCenter.Samples.Common.Models
{
    /// <summary>
    /// Health event returned from Office 365 Service Communications API.
    /// </summary>
    /// <seealso cref="Microsoft.Store.PartnerCenter.Samples.Common.Models.IHealthEvent" />
    public class OfficeHealthEvent : IHealthEvent
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier of the <see cref="OfficeHealthEvent"/>.
        /// </value>
        public string Id
        { get; set; }

        /// <summary>
        /// Gets the type of the <see cref="IHealthEvent" />.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        public HealthEventType EventType => HealthEventType.Office;

        /// <summary>
        /// Gets or sets an array of incident identifierss.
        /// </summary>
        /// <value>
        /// An array of incident identifiers associated with the <see cref="OfficeHealthEvent"/>.
        /// </value>
        public string[] IncidentIds
        { get; set; }

        /// <summary>
        /// Gets or sets the status of the <see cref="IHealthEvent" />.
        /// </summary>
        /// <value>
        /// The status of the <see cref="IHealthEvent" />.
        /// </value>
        public string Status
        { get; set; }

        /// <summary>
        /// Gets or sets the status time.
        /// </summary>
        /// <value>
        /// The time the status was posted.
        /// </value>
        public DateTime StatusTime
        { get; set; }

        /// <summary>
        /// Gets or sets the workload.
        /// </summary>
        /// <value>
        /// The workload the status event impacts.
        /// </value>
        public string Workload
        { get; set; }

        /// <summary>
        /// Gets or sets the display name of the workload.
        /// </summary>
        /// <value>
        /// The display name of the workload.
        /// </value>
        public string WorkloadDisplayName
        { get; set; }
    }
}