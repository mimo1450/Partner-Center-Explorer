// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Subscriptions;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    public class SubscriptionManageModel
    {
        public string CompanyName
        { get; set; }

        public string CustomerId
        { get; set; }

        public string FriendlyName
        { get; set; }

        public ISubscriptionDetails SubscriptionDetails
        { get; set; }

        public string SubscriptionId
        { get; set; }

        public string SubscriptionType
        { get; set; }
    }
}