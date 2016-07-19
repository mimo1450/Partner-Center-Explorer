// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model that represents the newly created customer.
    /// </summary>
    public class CreatedCustomerModel
    {
        /// <summary>
        /// Gets or sets the domain which the intial user belongs.
        /// </summary>
        /// <value>
        /// The domain which the initial user belongs.
        /// </value>
        public string Domain
        { get; set; }

        /// <summary>
        /// Gets or sets the password configured the initial user.
        /// </summary>
        /// <value>
        /// The password for the initial user for the newly created customer.
        /// </value>
        public string Password
        { get; set; }

        /// <summary>
        /// Gets or sets the username configured for the initial user.
        /// </summary>
        /// <value>
        /// The username for the initial user for the newly created customer.
        /// </value>
        public string Username
        { get; set; }
    }
}