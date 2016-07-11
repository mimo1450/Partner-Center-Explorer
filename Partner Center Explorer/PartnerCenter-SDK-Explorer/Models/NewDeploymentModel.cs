// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel.DataAnnotations;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model for new Azure Resource Manager (ARM) template deployments.
    /// </summary>
    public class NewDeploymentModel
    {
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier value.
        /// </value>
        public string CustomerId
        { get; set; }

        /// <summary>
        /// Gets or sets the URI where the parameters JSON is located.
        /// </summary>
        /// <value>
        /// The parameters URI.
        /// </value>
        [Display(Name = "Parameters Link")]
        [Required]
        public string ParametersUri
        { get; set; }

        /// <summary>
        /// Gets or sets the name of the resource group.
        /// </summary>
        /// <value>
        /// The name of the resource group.
        /// </value>
        public string ResourceGroupName
        { get; set; }

        /// <summary>
        /// Gets or sets the subscription identifier.
        /// </summary>
        /// <value>
        /// The subscription identifier.
        /// </value>
        public string SubscriptionId
        { get; set; }

        /// <summary>
        /// Gets or sets the URI where the template JSON is located.
        /// </summary>
        /// <value>
        /// The template URI.
        /// </value>
        [Display(Name = "Template Link")]
        [Required]
        public string TemplateUri
        { get; set; }
    }
}