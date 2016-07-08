// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.Users;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    public class UsersModel
    {
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier value assigned to the customer.
        /// </value>
        public string CustomerId
        { get; set; }

        /// <summary>
        /// Gets or sets a collection of users that belong to a specific customer.
        /// </summary>
        /// <value>
        /// The users that belong to a specific customer.
        /// </value>
        public SeekBasedResourceCollection<CustomerUser> Users
        { get; set; }
    }
}