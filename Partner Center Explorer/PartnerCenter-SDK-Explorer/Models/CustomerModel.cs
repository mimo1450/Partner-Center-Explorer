// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models.Customers;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model that represents a customer from Partner Center.
    /// </summary>
    public class CustomerModel
    {
        /// <summary>
        /// Gets or sets the billing profile.
        /// </summary>
        /// <value>
        /// An instnace of <see cref="CustomerBillingProfile"/>.
        /// </value>
        public CustomerBillingProfile BillingProfile
        { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        public string CompanyName
        { get; set; }

        /// <summary>
        /// Gets or sets the company profile.
        /// </summary>
        /// <value>
        /// An instance of <see cref="CustomerCompanyProfile"/>.
        /// </value>
        public CustomerCompanyProfile CompanyProfile
        { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier value.
        /// </value>
        public string CustomerId
        { get; set; }

        /// <summary>
        /// Gets or sets the name of the domain.
        /// </summary>
        /// <value>
        /// The name of the domain.
        /// </value>
        /// <remarks>This is the onmicrosoft.com domain assigned to the customer.</remarks>
        public string DomainName
        { get; set; }

        /// <summary>
        /// Gets or sets the relationship to partner.
        /// </summary>
        /// <value>
        /// The relationship to partner value.
        /// </value>
        public string RelationshipToPartner
        { get; set; }
    }
}