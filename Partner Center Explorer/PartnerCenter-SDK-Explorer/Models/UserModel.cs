// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model that represent a user. 
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier that owns the user.
        /// </value>
        public string CustomerId
        { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name for the user.
        /// </value>
        public string DisplayName
        { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The user's first name.
        /// </value>
        public string FirstName
        { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier assigned to the user.
        /// </value>
        public string Id
        { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The user's last name.
        /// </value>
        public string LastName
        { get; set; }

        /// <summary>
        /// Gets or sets the last directory synchronization time.
        /// </summary>
        /// <value>
        /// The last directory synchronization time.
        /// </value>
        /// <remarks>
        /// This value will only be populated if the domain the user is associated with 
        /// is configured directory synchronization. Otherwise, this value will be null.
        /// </remarks>
        public DateTime? LastDirectorySyncTime
        { get; set; }

        /// <summary>
        /// Gets or sets the usage location.
        /// </summary>
        /// <value>
        /// The usage location assigned to the user.
        /// </value>
        /// <remarks>
        /// This sample project will configure this value to match the country code specified 
        /// in the web.confing. The ability to have users in multiple regions is not supported.
        /// </remarks>
        public string UsageLocation
        { get; set; }

        /// <summary>
        /// Gets or sets the user principal name.
        /// </summary>
        /// <value>
        /// The user's user principal name.
        /// </value>
        public string UserPrincipalName
        { get; set; }
    }
}