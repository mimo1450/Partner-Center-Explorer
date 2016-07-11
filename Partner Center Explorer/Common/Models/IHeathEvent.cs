// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Store.PartnerCenter.Samples.Common.Models
{
    public interface IHealthEvent
    {
        HealthEventType EventType
        { get; }

        string Status
        { get; set; }
    }
}