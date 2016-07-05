// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    public class NewCustomerModel
    {
        [Required]
        public string AddressLine1
        { get; set; }

        public string AddressLine2
        { get; set; }

        [Required]
        public string City
        { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address specified.")]
        [Required]
        public string EmailAddress
        { get; set; }

        [Required]
        public string FirstName
        { get; set; }

        [Required]
        public string LastName
        { get; set; }

        [Display(Name = "Customer Name")]
        [Required]
        public string Name
        { get; set; }

        [Required]
        [RegularExpression("^(1[\\-\\/\\.]?)?(\\((\\d{3})\\)|(\\d{3}))[\\-\\/\\.]?(\\d{3})[\\-\\/\\.]?(\\d{4})$", ErrorMessage = "Please specify a valid phone number.")]
        public string PhoneNumber
        { get; set; }

        [Display(Name = "Domain Prefix")]
        [Required]
        [Remote("IsDomainAvailable", "Domains", ErrorMessage = "Domain prefix already exists. Please try another prefix.")]
        public string PrimaryDomain
        { get; set; }

        [Required]
        public string State
        { get; set; }

        public IEnumerable<string> SupportedStates
        { get; set; }

        [Required]
        [RegularExpression("^\\d{5}(-\\d{4})?$", ErrorMessage = "Please specify a valid zip code.")]
        public string ZipCode
        { get; set; }
    }
}