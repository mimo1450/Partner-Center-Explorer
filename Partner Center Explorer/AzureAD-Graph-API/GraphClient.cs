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
    public class GraphClient : IGraphClient
    {
        private Communication _comm;
        private string _token;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphClient"/> class.
        /// </summary>
        /// <param name="token">A valid JSON Web Token (JWT).</param>
        public GraphClient(string token)
        {
            _comm = new Communication();
            _token = token;
        }

        public List<Domain> GetDomains(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }

            return SynchronousExecute(() => GetDomainsAsync(customerId));
        }

        public async Task<List<Domain>> GetDomainsAsync(string customerId)
        {
            Result<Domain> domains;
            string requestUri;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }
            try
            {
                requestUri = string.Format("{0}/{1}/domains?api-version=beta",
                    AppConfig.GraphUri,
                    customerId
                );

                domains = await _comm.GetAsync<Result<Domain>>(
                    requestUri,
                    new MediaTypeWithQualityHeaderValue("application/json"),
                    _token
                );

                return domains.Value;
            }
            finally
            {
                domains = null;
            }
        }

        public List<ServiceConfigurationRecord> GetServiceConfigurationRecords(string customerId, string domain)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }
            else if (string.IsNullOrEmpty(domain))
            {
                throw new ArgumentNullException("domain");
            }

            return SynchronousExecute(() => GetServiceConfigurationRecordsAsync(customerId, domain));
        }

        public async Task<List<ServiceConfigurationRecord>> GetServiceConfigurationRecordsAsync(string customerId, string domain)
        {
            Result<ServiceConfigurationRecord> records;
            string data;
            string requestUri;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }
            else if (string.IsNullOrEmpty(domain))
            {
                throw new ArgumentNullException("domain");
            }

            try
            {
                requestUri = string.Format("{0}/{1}/domains('{2}')/serviceConfigurationRecords?api-version=beta",
                  AppConfig.GraphUri,
                  customerId,
                  domain
              );

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