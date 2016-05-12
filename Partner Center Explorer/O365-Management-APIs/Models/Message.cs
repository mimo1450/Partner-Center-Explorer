using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Microsoft.Samples.Office365.Management.API.Models
{
    /// <summary>
    /// Represents a message from the status API.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end data for the message.
        /// </value>
        public DateTime EndDate
        { get; set; }

        /// <summary>
        /// Gets or sets the feature.
        /// </summary>
        /// <value>
        /// The feature this message applies too.
        /// </value>
        public string Feature
        { get; set; }

        /// <summary>
        /// Gets or sets the display name of the feature.
        /// </summary>
        /// <value>
        /// The display name of the feature that this message applies too.
        /// </value>
        public string FeatureDisplayName
        { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier assigned to the message.
        /// </value>
        public string Id
        { get; set; }

        /// <summary>
        /// Gets or sets the last updated time.
        /// </summary>
        /// <value>
        /// The last time the message was updated.
        /// </value>
        public DateTime LastUpdatedTime
        { get; set; }

        /// <summary>
        /// Gets or sets the message details.
        /// </summary>
        /// <value>
        /// The message details associated with this message.
        /// </value>
        [JsonProperty("messages")]
        public List<MessageDetails> Messages
        { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name of this message.
        /// </value>
        public string Name
        { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date for the message.
        /// </value>
        public DateTime StartDate
        { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status of the message.
        /// </value>
        public string Status
        { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title of the message.
        /// </value>
        public string Title
        { get; set; }

        /// <summary>
        /// Gets or sets the workload.
        /// </summary>
        /// <value>
        /// The workload that this message applies too.
        /// </value>
        public string Workload
        { get; set; }

        /// <summary>
        /// Gets or sets the display name of the workload.
        /// </summary>
        /// <value>
        /// The display name of the workload that this message applies too.
        /// </value>
        public string WorkloadDisplayName
        { get; set; }
    }
}