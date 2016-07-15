// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Samples.AzureAD.Graph.API.Models;
using System.Collections.Generic;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model for domain configuration records obtain from Azure AD Graph API.
    /// </summary>
    public class ConfigurationRecordsModel
    {
        /// <summary>
        /// Gets or sets the service configuration records.
        /// </summary>
        /// <value>
        /// The service configuration records for a specific domain.
        /// </value>
        public List<ServiceConfigurationRecord> ServiceConfigurationRecords
        { get; set; }
    }
}