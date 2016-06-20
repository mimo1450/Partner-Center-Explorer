// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Subscriptions
{
    public interface ISubscriptionDetails
    {
        string FriendlyName
        { get; set; }

        string Status
        { get; set; }
    }
}