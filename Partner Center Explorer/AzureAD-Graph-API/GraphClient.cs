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

        public void AssignUserLicense(string customerId, string userId, LicenseAssignment assignment)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }
            else if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
            }
            else if (assignment == null)
            {
                throw new ArgumentNullException("assignment");
            }

            SynchronousExecute(() => AssignUserLicenseAsync(customerId, userId, assignment));
        }

        public async Task<LicenseAssignment> AssignUserLicenseAsync(string customerId, string userId, LicenseAssignment assignment)
        {
            string requestUri;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }
            else if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
            }
            else if (assignment == null)
            {
                throw new ArgumentNullException("assignment");
            }

            requestUri = string.Format("{0}/{1}/users/{2}/assignLicense?api-version=1.6",
                AppConfig.GraphUri,
                customerId,
                userId
            );

            await _comm.PostAsJsonAsync(
                requestUri,
                new MediaTypeWithQualityHeaderValue("application/json"),
                assignment,
                _token
            );

            return null;
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

                data =  await _comm.GetStringAsync<string>(
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

        public List<SubscribedSku> GetSubscribedSkus(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }

            return SynchronousExecute(() => GetSubscribedSkusAsync(customerId));
        }

        public async Task<List<SubscribedSku>> GetSubscribedSkusAsync(string customerId)
        {
            Result<SubscribedSku> result;
            string requestUri;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }

            try
            {
                requestUri = string.Format("{0}/{1}/subscribedSkus?api-version=1.6",
                    AppConfig.GraphUri,
                    customerId
                );

                result = await _comm.GetAsync<Result<SubscribedSku>>(
                    requestUri,
                    new MediaTypeWithQualityHeaderValue("application/json"),
                    _token
                );

                return result.Value;
            }
            finally
            {
                result = null;
            }
        }

        public User GetUser(string customerId, string userId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }
            else if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
            }

            return SynchronousExecute(() => GetUserAsync(customerId, userId));
        }

        public async Task<User> GetUserAsync(string customerId, string userId)
        {
            User user;
            string requestUri;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }
            else if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
            }

            requestUri = string.Format("{0}/{1}/users/{2}?api-version=1.6",
                AppConfig.GraphUri,
                customerId,
                userId
            );

            user = await _comm.GetAsync<User>(
                requestUri,
                new MediaTypeWithQualityHeaderValue("application/json"),
                _token
             );

            user.AvailableSkus = await GetSubscribedSkusAsync(customerId);

            return user;
        }

        public List<User> GetUsers(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }

            return SynchronousExecute(() => GetUsersAsync(customerId));
        }

        public async Task<List<User>> GetUsersAsync(string customerId)
        {
            Result<User> results;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }

            try
            {
                results = await _comm.GetAsync<Result<User>>(
                    string.Format("{0}/{1}/users?api-version=1.6", AppConfig.GraphUri, customerId),
                    new MediaTypeWithQualityHeaderValue("application/json"),
                    _token
                );

                return results.Value;
            }
            finally
            {
                results = null;
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