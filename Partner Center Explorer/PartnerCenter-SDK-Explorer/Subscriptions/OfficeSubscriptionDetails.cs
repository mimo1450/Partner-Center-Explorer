// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Subscriptions
{
    public class OfficeSubscriptionDetails : ISubscriptionDetails
    {
        public string FriendlyName
        { get; set; }

        public int Quantity
        { get; set; }

        public string Status
        { get; set; }
    }
}