// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Samples.AzureAD.Graph.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Samples.AzureAD.Graph.API
{
    public interface IGraphClient
    {
        List<Domain> GetDomains(string customerId);

        Task<List<Domain>> GetDomainsAsync(string customerId);

        List<ServiceConfigurationRecord> GetServiceConfigurationRecords(string customerId, string domain);

        Task<List<ServiceConfigurationRecord>> GetServiceConfigurationRecordsAsync(string customerId, string domain);

    }
}