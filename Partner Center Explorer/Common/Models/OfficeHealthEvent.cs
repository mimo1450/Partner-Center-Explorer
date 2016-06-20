// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Store.PartnerCenter.Samples.Common.Models
{
    public class OfficeHealthEvent : IHealthEvent
    {
        public string Id
        { get; set; }

        public HealthEventType EventType
        {
            get { return HealthEventType.Office; }
        }

        public string[] IncidentIds
        { get; set; }

        public string Status
        { get; set; }

        public DateTime StatusTime
        { get; set; }

        public string Workload
        { get; set; }

        public string WorkloadDisplayName
        { get; set; }
    }
}