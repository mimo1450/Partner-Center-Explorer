// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.Customers;
using Microsoft.Store.PartnerCenter.Models.Subscriptions;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    public class CustomerModel
    {
        public CustomerBillingProfile BillingProfile
        { get; set; }

        public string CompanyName
        { get; set; }

        public CustomerCompanyProfile CompanyProfile
        { get; set; }

        public string DomainName
        { get; set; }

        public ResourceCollection<Subscription> Subscriptions
        { get; set; }

        public string TenantId
        { get; set; }
    }
}