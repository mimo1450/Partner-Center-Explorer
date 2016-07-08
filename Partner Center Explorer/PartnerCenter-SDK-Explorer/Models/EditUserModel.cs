// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    public class EditUserModel
    {
        public string CustomerId
        { get; set; }

        [Display(Name = "Display Name")]
        [Required]
        public string DisplayName
        { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName
        { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName
        { get; set; }

        public List<LicenseModel> Licenses
        { get; set; }

        public string Password
        { get; set; }

        public string UsageLocation
        {
            get { return "US"; }
        }

        public string UserId
        { get; set; }

        [Display(Name = "User Principal Name")]
        [Required]
        public string UserPrincipalName
        { get; set; }
    }
}