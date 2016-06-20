// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace Microsoft.Samples.AzureAD.Graph.API.Models
{
    public class SubscribedSku
    {
        public string CapabilityStatus
        { get; set; }

        public int ConsumedUnits
        { get; set; }

        public string SkuId
        { get; set; }

        public string SkuPartNumber
        { get; set; }
    }
}
