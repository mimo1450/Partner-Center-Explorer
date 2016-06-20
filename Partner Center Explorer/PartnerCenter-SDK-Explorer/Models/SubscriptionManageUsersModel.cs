// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Samples.AzureAD.Graph.API.Models;
using System.Collections.Generic;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    public class SubscriptionManageUsersModel
    {
        public string CustomerId
        { get; set; }

        public List<User> Users
        { get; set; }
    }
}