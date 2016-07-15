// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.Auditing;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// View model for audit log records from Partner Center.
    /// </summary>
    public class AuditRecordsModel
    {
        /// <summary>
        /// Gets or sets the collection of audit log records.
        /// </summary>
        /// <value>
        /// The collection of audit log records.
        /// </value>
        public SeekBasedResourceCollection<AuditRecord> Records
        { get; set; }
    }
}