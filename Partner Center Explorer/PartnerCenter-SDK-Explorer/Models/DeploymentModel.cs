// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model for new deployments in Azure using Azure Resource Manager (ARM) templates.
    /// </summary>
    public class DeploymentModel
    {
        /// <summary>
        /// Gets or sets the identifier for the deployment.
        /// </summary>
        /// <value>
        /// The identifier for the deployment.
        /// </value>
        public string Id
        { get; set; }

        /// <summary>
        /// Gets or sets the name for the deployment
        /// </summary>
        /// <value>
        /// The name of the deployment.
        /// </value>
        public string Name
        { get; set; }

        /// <summary>
        /// Gets or sets the state of the provisioning.
        /// </summary>
        /// <value>
        /// The state of the provisioning.
        /// </value>
        public string ProvisioningState
        { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>
        /// The timestamp for the deployment.
        /// </value>
        public DateTime Timestamp
        { get; set; }
    }
}