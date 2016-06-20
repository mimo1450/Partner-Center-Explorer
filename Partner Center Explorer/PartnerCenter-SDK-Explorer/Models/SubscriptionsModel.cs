// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.Subscriptions;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    public class SubscriptionsModel
    {
        public ResourceCollection<Subscription> Subscriptions
        { get; set; }
    }
}