// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Samples.Common.Models;
using System.Collections.Generic;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    public class SubscriptionHealthModel
    {
        public string CompanyName
        { get; set; }

        public string CustomerId
        { get; set; }

        public string FriendlyName
        { get; set; }

        public List<IHealthEvent> HealthEvents
        { get; set; }

        public string SubscriptionType
        { get; set; }
    }
}