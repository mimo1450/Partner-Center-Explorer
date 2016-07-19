// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Samples.Common;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model for creating a new user.
    /// </summary>
    public class NewUserModel
    {
        public string CustomerId
        { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name for the new user being created.
        /// </value>
        [Display(Name = "Display Name")]
        [Required]
        public string DisplayName
        { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name for the user being created.
        /// </value>
        [Display(Name = "First Name")]
        [Required]
        public string FirstName
        { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name for the user being created.
        /// </value>
        [Display(Name = "Last Name")]
        [Required]
        public string LastName
        { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password for the user being created.
        /// </value>
        [Required]
        public string Password
        { get; set; }

        /// <summary>
        /// Gets the usage location.
        /// </summary>
        /// <value>
        /// The usage location for the user being created.
        /// </value>
        /// <remarks>
        /// It is possible to have a usage location that does not match the configured country code. This sample
        /// project does not implement that logical and will configure all usage location values to the configure
        /// country code.
        /// </remarks>
        public string UsageLocation => AppConfig.CountryCode;

        /// <summary>
        /// Gets or sets the name of the user principal.
        /// </summary>
        /// <value>
        /// The user principal name for the user being created.
        /// </value>
        [Display(Name = "User Principal Name")]
        [Required]
        public string UserPrincipalName
        { get; set; }
    }
}