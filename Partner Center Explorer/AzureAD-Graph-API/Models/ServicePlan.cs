// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Samples.AzureAD.Graph.API.Models
{
    public class ServicePlan
    {
        public string AppliesTo
        { get; set; }

        public string ServicePlanId
        { get; set; }

        public string ServicePlanName
        { get; set; }

        public string ProvisioningStatus
        { get; set; }
    }
}