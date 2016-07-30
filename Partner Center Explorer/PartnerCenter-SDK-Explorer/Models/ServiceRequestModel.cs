// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    public class ServiceRequestModel
    {
        public DateTime CreatedDate
        { get; set; }

        public string Id
        { get; set; }

        public string Organization
        { get; set; }

        public string PrimaryContact
        { get; set; }

        public string ProductName
        { get; set; }

        public string Status
        { get; set; }

        public string Title
        { get; set; }
    }
}