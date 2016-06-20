// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.PowerBI.Api;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// View model used when displaying a Power BI report.
    /// </summary>
    public class ReportModel
    {
        /// <summary>
        /// Gets or sets the access token used to access the Power BI report.
        /// </summary>
        /// <value>
        /// The access token used to access the Power BI report.
        /// </value>
        public string AccessToken
        { get; set; }

        /// <summary>
        /// Gets or sets an instance of <see cref="IReport"/>.
        /// </summary>
        /// <value>
        /// The Power BI report to be displayed.
        /// </value>
        public IReport Report
        { get; set; }
    }
}