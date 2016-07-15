// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Samples.AzureAD.Graph.API.Models
{
    /// <summary>
    ///  Represents a SRV DNS service configuration record.
    /// </summary>
    /// <seealso cref="Microsoft.Samples.AzureAD.Graph.API.Models.ServiceConfigurationRecord" />
    public class DomainDnsSrvRecord : ServiceConfigurationRecord
    {
        /// <summary>
        /// Gets or sets the name target.
        /// </summary>
        /// <value>
        /// The name target value for the record.
        /// </value>
        public string NameTarget
        { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port value for the record.
        /// </value>
        public int Port
        { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority value for the record.
        /// </value>
        public int Priority
        { get; set; }

        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        /// <value>
        /// The protocol value for the record.
        /// </value>
        public string Protocol
        { get; set; }

        /// <summary>
        /// Gets or sets the service.
        /// </summary>
        /// <value>
        /// The service value for the record.
        /// </value>
        public string Service
        { get; set; }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        /// <value>
        /// The weight value for the record.
        /// </value>
        public int Weight
        { get; set; }
    }
}