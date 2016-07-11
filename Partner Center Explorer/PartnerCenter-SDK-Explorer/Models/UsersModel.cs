// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

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
        public List<UserModel> Users
        { get; set; }
    }
}