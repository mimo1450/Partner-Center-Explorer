// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Store.PartnerCenter.Samples.Common.Models
{
    /// <summary>
    /// Model for Azure Automation runbooks.
    /// </summary>
    public class RunbookModel
    {
        /// <summary>
        /// Gets or sets the creation time of the runbook.
        /// </summary>
        /// <value>
        /// The creation time of the runbook.
        /// </value>
        public DateTimeOffset CreationTime
        { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// Description of the runbook.
        /// </value>
        public string Description
        { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the runbook.
        /// </summary>
        /// <value>
        /// The identifier of the runbook.
        /// </value>
        public string Id
        { get; set; }

        /// <summary>
        /// Gets or sets the job count of the runbook.
        /// </summary>
        /// <value>
        /// The job count of the runbook.
        /// </value>
        public int JobCount
        { get; set; }

        /// <summary>
        /// Gets or sets the last modified by attribute of the runbook.
        /// </summary>
        /// <value>
        /// Who last modified the runbook.
        /// </value>
        public string LastModifiedBy
        { get; set; }

        /// <summary>
        /// Gets or sets the last modified time.
        /// </summary>
        /// <value>
        /// The last modified time of the runbook.
        /// </value>
        public DateTimeOffset LastModifiedTime
        { get; set; }

        /// <summary>
        /// Gets or sets the option to log activity trace of the runbook.
        /// </summary>
        /// <value>
        /// The log activity trace of the runbook.
        /// </value>
        public int LogActivityTrace
        { get; set; }

        /// <summary>
        /// Gets or sets progress log option.
        /// </summary>
        /// <value>
        /// The log process option for the runbook.
        /// </value>
        public bool LogProgress
        { get; set; }

        /// <summary>
        ///  Gets or sets verbose log option.
        /// </summary>
        /// <value>
        ///  Gets or set the verbose log option for the runbook.
        /// </value>
        public bool LogVerbose
        { get; set; }

        /// <summary>
        /// Gets or sets the name of the runbook.
        /// </summary>
        /// <value>
        /// The name of the runbook.
        /// </value>
        public string Name
        { get; set; }

        /// <summary>
        /// Gets or sets the state of the provisioning.
        /// </summary>
        /// <value>
        /// The state of the provisioning of the runbook.
        /// </value>
        public string ProvisioningState
        { get; set; }

        /// <summary>
        /// Gets or sets the name of the resource group.
        /// </summary>
        /// <value>
        /// The name of the resource group.
        /// </value>
        /// <remarks>
        /// This property is used by the view for routing purposes.
        /// </remarks>
        public string ResourceGroupName
        { get; set; }

        /// <summary>
        /// Gets or sets the type of the runbook.
        /// </summary>
        /// <value>
        /// The type of the runbook.
        /// </value>
        public string RunbookType
        { get; set; }

        /// <summary>
        /// Gets or sets the state of the runbook.
        /// </summary>
        /// <value>
        /// State of the runbook.
        /// </value>
        public string State
        { get; set; }
    }
}