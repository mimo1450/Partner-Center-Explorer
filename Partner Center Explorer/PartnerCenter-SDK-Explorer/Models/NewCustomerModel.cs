// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model for creating a new customer.
    /// </summary>
    public class NewCustomerModel
    {
        /// <summary>
        /// Gets or sets the first address line.
        /// </summary>
        /// <value>
        /// The first address line for customer being created.
        /// </value>
        [Display(Name = "Address Line 1")]
        [Required]
        public string AddressLine1
        { get; set; }

        /// <summary>
        /// Gets or sets the second address line.
        /// </summary>
        /// <value>
        /// The second address line for customer being created.
        /// </value>
        [Display(Name = "Address Line 2")]
        public string AddressLine2
        { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city for customer being created.
        /// </value>
        [Required]
        public string City
        { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address for the technical for the customer being created.
        /// </value>
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid email address specified.")]
        [Required]
        public string EmailAddress
        { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name for the technical contact for the customer being created.
        /// </value>
        [Required]
        [Display(Name = "First Name")]
        public string FirstName
        { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name of the technical contact for the customer being created.
        /// </value>
        [Display(Name = "Last Name")]
        [Required]
        public string LastName
        { get; set; }

        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        /// <value>
        /// The name of the customer being created.
        /// </value>
        [Display(Name = "Customer Name")]
        [Required]
        public string Name
        { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number for the technical contact for the customer being created.
        /// </value>
        [Display(Name = "Phone Number")]
        [RegularExpression("^(1[\\-\\/\\.]?)?(\\((\\d{3})\\)|(\\d{3}))[\\-\\/\\.]?(\\d{3})[\\-\\/\\.]?(\\d{4})$", ErrorMessage = "Please specify a valid phone number.")]
        [Required]
        public string PhoneNumber
        { get; set; }

        /// <summary>
        /// Gets or sets the domain prefix.
        /// </summary>
        /// <value>
        /// The domain prefix for the customer being created.
        /// </value>
        /// <remarks>
        /// This value is the string that proceeds the onmicrosoft.com domain suffix.
        /// </remarks>
        [Display(Name = "Domain Prefix")]
        [Remote("IsDomainAvailable", "Domains", ErrorMessage = "Domain prefix already exists. Please try another prefix.")]
        [Required]
        public string PrimaryDomain
        { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state where the customer being created is located.
        /// </value>
        [Required]
        public string State
        { get; set; }

        /// <summary>
        /// Gets or sets the supported states.
        /// </summary>
        /// <value>
        /// A list of states supported.
        /// </value>
        public IEnumerable<string> SupportedStates
        { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>
        /// The zip code for the customer being created.
        /// </value>
        [Display(Name = "Zip Code")]
        [RegularExpression("^\\d{5}(-\\d{4})?$", ErrorMessage = "Please specify a valid zip code.")]
        [Required]
        public string ZipCode
        { get; set; }
    }
}