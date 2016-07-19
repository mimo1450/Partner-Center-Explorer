// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.Customers;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model for listing existing customers.
    /// </summary>
    public class CustomersModel
    {
        /// <summary>
        /// Gets or sets the customers.
        /// </summary>
        /// <value>
        /// The customers that belong to the configured reseller.
        /// </value>
        public ResourceCollection<Customer> Customers
        { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the account identifier is associated with the integration sandbox.
        /// </summary>
        /// <value>
        /// <c>true</c> if the account identifier is associated with the intergration sandbox; otherwise, <c>false</c>.
        /// </value>
        public bool IsSandboxEnvironment
        { get; set; }
    }
}