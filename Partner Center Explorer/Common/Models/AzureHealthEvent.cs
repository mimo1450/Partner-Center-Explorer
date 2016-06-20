// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Store.PartnerCenter.Samples.Common.Models
{
    public class AzureHealthEvent : IHealthEvent
    {
        public string Description
        { get; set; }

        public DateTime EventTimestamp
        { get; set; }

        public HealthEventType EventType
        {
            get { return HealthEventType.Azure; }
        }

        public string ResourceId
        { get; set; }

        public string ResourceGroupName
        { get; set; }

        public string ResourceProviderName
        { get; set; }

        public string ResourceType
        { get; set; }

        public string Status
        { get; set; }
    }
}