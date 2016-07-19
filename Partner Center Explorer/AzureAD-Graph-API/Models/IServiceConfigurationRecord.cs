// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Samples.AzureAD.Graph.API.Models
{
    /// <summary>
    /// Interface used by <see cref="ServiceConfigurationRecord"/>s.
    /// </summary>
    public interface IServiceConfigurationRecord
    {
        /// <summary>
        /// Gets or sets the DNS record identifier.
        /// </summary>
        /// <value>
        /// The DNS record identifier.
        /// </value>
        string DnsRecordId
        { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="ServiceConfigurationRecord"/> is optional.
        /// </summary>
        /// <value>
        /// <c>true</c> if this <see cref="ServiceConfigurationRecord"/> is optional; otherwise, <c>false</c>.
        /// </value>
        bool IsOptional
        { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label value for the <see cref="ServiceConfigurationRecord"/>.
        /// </value>
        string Label
        { get; set; }

        /// <summary>
        /// Gets or sets the type of the record.
        /// </summary>
        /// <value>
        /// The type of the <see cref="ServiceConfigurationRecord"/>.
        /// </value>
        string RecordType
        { get; set; }

        /// <summary>
        /// Gets or sets the supported service.
        /// </summary>
        /// <value>
        /// Service supported by the <see cref="ServiceConfigurationRecord"/>.
        /// </value>
        string SupportedService
        { get; set; }

        /// <summary>
        /// Gets or sets the time to live value.
        /// </summary>
        /// <value>
        /// The time to live value for the <see cref="ServiceConfigurationRecord"/>.
        /// </value>
        int Ttl
        { get; set; }
    }
}