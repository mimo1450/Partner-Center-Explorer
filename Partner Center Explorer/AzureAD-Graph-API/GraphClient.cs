// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Samples.AzureAD.Graph.API.Converters;
using Microsoft.Samples.AzureAD.Graph.API.Models;
using Microsoft.Store.PartnerCenter.Samples.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Microsoft.Samples.AzureAD.Graph.API
{
    /// <summary>
    /// Used to perform Azure AD Graph API operations.
    /// </summary>
    /// <seealso cref="Microsoft.Samples.AzureAD.Graph.API.IGraphClient" />
    public class GraphClient : IGraphClient
    {
        private Communication _comm;
        private readonly string _token;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphClient"/> class.
        /// </summary>
        /// <param name="token">A valid JSON Web Token (JWT).</param>
        public GraphClient(string token)
        {
            _comm = new Communication();
            _token = token;
        }

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
        public List<Domain> GetDomains(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            return SynchronousExecute(() => GetDomainsAsync(customerId));
        }

        /// <summary>
        /// Asynchronously gets a list of domains that belong to the specified customer identifier.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>
        /// A list of domains that belong to the specified customer identifier.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// customerId
        /// </exception>
        public async Task<List<Domain>> GetDomainsAsync(string customerId)
        {
            Result<Domain> domains;
            string requestUri;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            try
            {
                requestUri = $"{AppConfig.GraphUri}/{customerId}/domains?api-version=beta";

                domains = await _comm.GetAsync<Result<Domain>>(
                    requestUri,
                    new MediaTypeWithQualityHeaderValue("application/json"),
                    _token);

                return domains.Value;
            }
            finally
            {
                domains = null;
            }
        }

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
        public List<ServiceConfigurationRecord> GetServiceConfigurationRecords(string customerId, string domain)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            if (string.IsNullOrEmpty(domain))
            {
                throw new ArgumentNullException(nameof(domain));
            }

            return SynchronousExecute(() => GetServiceConfigurationRecordsAsync(customerId, domain));
        }

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
        public async Task<List<ServiceConfigurationRecord>> GetServiceConfigurationRecordsAsync(string customerId, string domain)
        {
            Result<ServiceConfigurationRecord> records;
            string data;
            string requestUri;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            if (string.IsNullOrEmpty(domain))
            {
                throw new ArgumentNullException(nameof(domain));
            }

            try
            {
                requestUri =
                    $"{AppConfig.GraphUri}/{customerId}/domains('{domain}')/serviceConfigurationRecords?api-version=beta";

                data = await _comm.GetStringAsync<string>(
                    requestUri,
                    new MediaTypeWithQualityHeaderValue("application/json"),
                    _token
                );

                records = JsonConvert.DeserializeObject<Result<ServiceConfigurationRecord>>(
                    data,
                    new ServiceConfigurationRecordConverter()
                );

                return records.Value;
            }
            finally
            {
                records = null;
            }
        }

        /// <summary>
        /// Synchronously executes an asynchronous function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
        private T SynchronousExecute<T>(Func<Task<T>> operation)
        {
            try
            {
                return Task.Run(async () => await operation()).Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}