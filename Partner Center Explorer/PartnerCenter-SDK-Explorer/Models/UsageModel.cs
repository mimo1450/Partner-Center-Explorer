// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.Usage;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    public class UsageModel
    {
        public string CompanyName
        { get; set; }

        public string CustomerId
        { get; set; }

        public ResourceCollection<SubscriptionDailyUsageRecord> DailyUsage
        { get; set; }

        public ResourceCollection<AzureResourceMonthlyUsageRecord> MonthlyUsage
        { get; set; }

        public string SubscriptionFriendlyName
        { get; set; }
    }
}