// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Samples.AzureAD.Graph.API.Models
{
    public class AssignedLicense
    {
        public List<Guid> DisabledPlans
        { get; set; }

        public Guid? SkuId
        { get; set; }
    }
}