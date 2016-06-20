// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Samples.AzureAD.Graph.API.Models
{
    public class AssignedPlan
    {
        public DateTime? AssignedTimestamp
        { get; set; }

        public string CapabilityStatus
        { get; set; }

        public string Service
        { get; set; }

        public Guid? ServicePlanId
        { get; set; }
    }
}