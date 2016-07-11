// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    public class UserModel
    {
        public string CustomerId
        { get; set; }

        public string DisplayName
        { get; set; }

        public string FirstName
        { get; set; }

        public string Id
        { get; set; }

        public string LastName
        { get; set; }

        public DateTime? LastDirectorySyncTime
        { get; set; }

        public string UsageLocation
        { get; set; }

        public string UserPrincipalName
        { get; set; }
    }
}