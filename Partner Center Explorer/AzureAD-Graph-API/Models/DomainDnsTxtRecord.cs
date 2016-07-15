// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Samples.AzureAD.Graph.API.Models
{
    /// <summary>
    /// Represents a TXT DNS service configuration record.
    /// </summary>
    /// <seealso cref="Microsoft.Samples.AzureAD.Graph.API.Models.ServiceConfigurationRecord" />
    public class DomainDnsTxtRecord : ServiceConfigurationRecord
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text value for the record.
        /// </value>
        public string Text
        { get; set; }
    }
}