// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model used when invoking Azure Automation runbooks.
    /// </summary>
    public class InvokeRunbookModel
    {
        /// <summary>
        /// Gets or sets the name of the resource group.
        /// </summary>
        /// <value>
        /// The name of the resource group.
        /// </value>
        public string ResourceGroupName
        { get; set; }

        /// <summary>
        /// Gets or sets the name of the runbook.
        /// </summary>
        /// <value>
        /// The name of the runbook to be invoked.
        /// </value>
        public string RunbookName
        { get; set; }
    }
}