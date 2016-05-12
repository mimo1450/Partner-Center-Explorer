using System;
using System.Collections.Generic;

namespace Microsoft.Samples.Office365.Management.API.Models
{
    /// <summary>
    /// Represents status details obtained from the Office 365 Service Communication 
    /// </summary>
    public class StatusDetails
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier for the status detail.
        /// </value>
        public string Id
        { get; set; }

        /// <summary>
        /// Gets or sets the incident identifiers associated with this status.
        /// </summary>
        /// <value>
        /// The incident identifiers associated with this status.
        /// </value>
        public List<string> IncidentIds
        { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status assigned to this details.
        /// </value>
        public string Status
        { get; set; }

        /// <summary>
        /// Gets or sets the status time.
        /// </summary>
        /// <value>
        /// The time when this status was generated.
        /// </value>
        public DateTime StatusTime
        { get; set; }

        /// <summary>
        /// Gets or sets the workload.
        /// </summary>
        /// <value>
        /// The workload associated with this status detail.
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