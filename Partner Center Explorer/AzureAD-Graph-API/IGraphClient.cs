// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Samples.AzureAD.Graph.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Samples.AzureAD.Graph.API
{
    public interface IGraphClient
    {
        /// <summary>
        /// Gets a list of domains that belong to the specified customer identifier.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>
        /// A list of domains that belong to the specified customer identifier.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// customerId
        /// </exception>
        List<Domain> GetDomains(string customerId);

        /// <summary>
        /// Asynchronously get a list of domains that belong to the specified customer identifier.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>
        /// A list of domains that belong to the specified customer identifier.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// customerId
        /// </exception>
        Task<List<Domain>> GetDomainsAsync(string customerId);

        /// <summary>
        /// Gets the service configuration records for the specified domain.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="domain">The domain of interest.</param>
        /// <returns>A list of service configurations records for the specified domain.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// customerId
        /// or
        /// domain
        /// </exception>
        /// <remarks>
        /// Service configuration records are DNS records that should be configured for the specified
        /// domain in order for Office 365 services to function as expected.
        /// </remarks>
        List<ServiceConfigurationRecord> GetServiceConfigurationRecords(string customerId, string domain);

        /// <summary>
        /// Asynchronously gets the service configuration records for the specified domain.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="domain">The domain of interest.</param>
        /// <returns>A list of service configurations records for the specified domain.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// customerId
        /// or
        /// domain
        /// </exception>
        /// <remarks>
        /// Service configuration records are DNS records that should be configured for the specified
        /// domain in order for Office 365 services to function as expected.
        /// </remarks>
        Task<List<ServiceConfigurationRecord>> GetServiceConfigurationRecordsAsync(string customerId, string domain);
    }
}