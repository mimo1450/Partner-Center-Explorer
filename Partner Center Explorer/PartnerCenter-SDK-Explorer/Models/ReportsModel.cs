// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.PowerBI.Api.Beta.Models;
using System.Collections.Generic;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model that is used to display a list of Power BI reports. 
    /// </summary>
    public class ReportsModel
    {
        /// <summary>
        /// Gets or sets the collection of Power BI reports available for the defined workspace.
        /// </summary>
        /// <value>
        /// The collection fo Power BI reports available for the defined workspace.
        /// </value>
        public List<Report> Reports
        { get; set; }
    }
}