// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Samples.AzureAD.Graph.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Samples.AzureAD.Graph.API
{
    public interface IGraphClient
    {
        void AssignUserLicense(string customerId, string userId, LicenseAssignment assignment);

        Task<LicenseAssignment> AssignUserLicenseAsync(string customerId, string userId, LicenseAssignment assignment);

        List<SubscribedSku> GetSubscribedSkus(string customerId);

        Task<List<SubscribedSku>> GetSubscribedSkusAsync(string customerId);

        User GetUser(string customerId, string userId);

        Task<User> GetUserAsync(string customerId, string userId);

        List<User> GetUsers(string customerId);

        Task<List<User>> GetUsersAsync(string customerId);
    }
}