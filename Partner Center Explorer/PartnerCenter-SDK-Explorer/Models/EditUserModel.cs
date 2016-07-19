// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model for editing a user.
    /// </summary>
    public class EditUserModel
    {
        public string CustomerId
        { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name for the user being edited.
        /// </value>
        [Display(Name = "Display Name")]
        [Required]
        public string DisplayName
        { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name of the user being edited.
        /// </value>
        [Display(Name = "First Name")]
        [Required]
        public string FirstName
        { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name of the user being edited.
        /// </value>
        [Display(Name = "Last Name")]
        [Required]
        public string LastName
        { get; set; }

        public List<LicenseModel> Licenses
        { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password assinged to the user being edited.
        /// </value>
        /// <remarks>
        /// The existing password for the user will not be returned. This property should only be
        /// populated if the user's password needs to be changed.
        /// </remarks>
        public string Password
        { get; set; }

        /// <summary>
        /// Gets or sets the usage location.
        /// </summary>
        /// <value>
        /// The usage location assigned to the user being edited.
        /// </value>
        public string UsageLocation
        { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier for the user being edited.
        /// </value>
        public string UserId
        { get; set; }

        /// <summary>
        /// Gets or sets the name of the user principal.
        /// </summary>
        /// <value>
        /// The user principal name for the user being edited.
        /// </value>
        [Display(Name = "User Principal Name")]
        [Required]
        public string UserPrincipalName
        { get; set; }
    }
}