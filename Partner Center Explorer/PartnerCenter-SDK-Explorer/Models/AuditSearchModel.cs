// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    public class AuditSearchModel
    {
        [DataType(DataType.DateTime)]
        public DateTime EndDate
        { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime StartDate
        { get; set; }
    }
}