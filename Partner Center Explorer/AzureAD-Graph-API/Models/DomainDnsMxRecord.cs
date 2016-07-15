// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Samples.AzureAD.Graph.API.Models
{
    /// <summary>
    /// Represents a MX DNS service configuration record.
    /// </summary>
    /// <seealso cref="Microsoft.Samples.AzureAD.Graph.API.Models.ServiceConfigurationRecord" />
    public class DomainDnsMxRecord : ServiceConfigurationRecord
    {
        /// <summary>
        /// Gets or sets the mail exchange.
        /// </summary>
        /// <value>
        /// The mail exchange value.
        /// </value>
        public string MailExchange
        { get; set; }

        /// <summary>
        /// Gets or sets the preference value for the record.
        /// </summary>
        /// <value>
        /// The preference value for the record.
        /// </value>
        public int Preference
        { get; set; }
    }
}