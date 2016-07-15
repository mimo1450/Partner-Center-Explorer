// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Samples.AzureAD.Graph.API.Models
{
    /// <summary>
    /// Represents a CNAME DNS service configuration record.
    /// </summary>
    /// <seealso cref="Microsoft.Samples.AzureAD.Graph.API.Models.ServiceConfigurationRecord" />
    public class DomainDnsCnameRecord : ServiceConfigurationRecord
    {
        /// <summary>
        /// Gets or sets the name of the canonical.
        /// </summary>
        /// <value>
        /// The canonical name value.
        /// </value>
        public string CanonicalName
        { get; set; }
    }
}