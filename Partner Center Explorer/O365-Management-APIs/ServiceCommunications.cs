// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Samples.Office365.Management.API.Models;
using Microsoft.Store.PartnerCenter.Samples.Common;
using Microsoft.Store.PartnerCenter.Samples.Common.Context;
using Microsoft.Store.PartnerCenter.Samples.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Microsoft.Samples.Office365.Management.API
{
    /// <summary>
    /// Facilities interactions with the Office 365 Service Communications API.
    /// </summary>
    public class ServiceCommunications
    {
        private readonly Communication _comm;
        private readonly string _token;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCommunications" /> class.
        /// </summary>
        /// <param name="token">The user assertion token.</param>
        /// <exception cref="ArgumentNullException">userAssertionToken</exception>
        public ServiceCommunications(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            _comm = new Communication();
            _token = token;
        }

        /// <summary>
        /// Gets the current status for the specified tenant.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns>A list of status details associated with the specified tenant identifier.</returns>
        /// <exception cref="ArgumentNullException">tenantId</exception>
        public List<IHealthEvent> GetCurrentStatus(string tenantId)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            return SynchronousExecute(() => GetCurrentStatusAsync(tenantId));
        }

        /// <summary>
        /// Gets the current status for the specified tenant asynchronously.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns>A list of health events associated with the specified tenant identifier.</returns>
        /// <exception cref="ArgumentNullException">tenantId</exception>
        public async Task<List<IHealthEvent>> GetCurrentStatusAsync(string tenantId)
        {
            Result<OfficeHealthEvent> records;
            string authority;
            string requestUri;

            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            try
            {
                authority = $"{AppConfig.Authority}/{tenantId}/oauth2/token";
                requestUri = $"{OfficeConfig.ApiUri}/api/v1.0/{tenantId}/ServiceComms/CurrentStatus";

                records = await _comm.GetAsync<Result<OfficeHealthEvent>>(
                    requestUri,
                    new MediaTypeWithQualityHeaderValue("application/json"),
                    TokenContext.GetAADToken(authority, OfficeConfig.ApiUri, _token).AccessToken
                );

                return records.Value.ToList<IHealthEvent>();
            }
            finally
            {
                records = null;
            }
        }

        /// <summary>
        /// Gets the specified message.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// tenantId
        /// or
        /// messageId
        /// </exception>
        public Message GetMessage(string tenantId, string messageId)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }
            else if (string.IsNullOrEmpty(messageId))
            {
                throw new ArgumentNullException(nameof(messageId));
            }

            return SynchronousExecute(() => GetMessageAsync(tenantId, messageId));
        }

        /// <summary>
        /// Gets the specified message asynchronously.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// tenantId
        /// or
        /// messageId
        /// </exception>
        public async Task<Message> GetMessageAsync(string tenantId, string messageId)
        {
            Result<Message> results;
            string authority;
            string requestUri;

            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }
            else if (string.IsNullOrEmpty(messageId))
            {
                throw new ArgumentNullException(nameof(messageId));
            }

            try
            {
                authority = $"{AppConfig.Authority}/{tenantId}/oauth2/token";
                requestUri =
                    $"{OfficeConfig.ApiUri}/api/v1.0/{tenantId}/ServiceComms/Messages?$filter=Id eq '{messageId}'";

                results = await _comm.GetAsync<Result<Message>>(
                    requestUri,
                    new MediaTypeWithQualityHeaderValue("application/json"),
                    TokenContext.GetAADToken(authority, OfficeConfig.ApiUri, _token).AccessToken
                );

                return results.Value.SingleOrDefault();
            }
            finally
            {
                results = null;
            }
        }

        /// <summary>
        /// Gets a list of messages for the specified tenant.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">tenantId</exception>
        public List<Message> GetMessages(string tenantId)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            return SynchronousExecute(() => GetMessagesAsync(tenantId));
        }

        /// <summary>
        /// Gets a list of messages for the specified tenant asynchronously.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">tenantId</exception>
        public async Task<List<Message>> GetMessagesAsync(string tenantId)
        {
            Result<Message> messages;
            string authority;
            string requestUri;

            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            try
            {
                authority = $"{AppConfig.Authority}/{tenantId}/oauth2/token";
                requestUri = $"{OfficeConfig.ApiUri}/api/v1.0/{tenantId}/ServiceComms/Messages";

                messages = await _comm.GetAsync<Result<Message>>(
                    requestUri,
                    new MediaTypeWithQualityHeaderValue("application/json"),
                    TokenContext.GetAADToken(authority, OfficeConfig.ApiUri, _token).AccessToken
                );

                return messages.Value;
            }
            finally
            {
                messages = null;
            }
        }

        /// <summary>
        /// Gets the services for the specified tenant.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">tenantId</exception>
        public List<Service> GetServices(string tenantId)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            return SynchronousExecute(() => GetServicesAsync(tenantId));
        }

        /// <summary>
        /// Gets the services for the specified tenant asynchronously.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">tenantId</exception>
        public async Task<List<Service>> GetServicesAsync(string tenantId)
        {
            Result<Service> services;
            string authority;
            string requestUri;

            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(tenantId);
            }

            try
            {
                authority = $"{AppConfig.Authority}/{tenantId}/oauth2/token";
                requestUri = $"{OfficeConfig.ApiUri}/api/v1.0/{tenantId}/ServiceComms/Services";

                services = await _comm.GetAsync<Result<Service>>(
                    requestUri,
                    new MediaTypeWithQualityHeaderValue("application/json"),
                    TokenContext.GetAADToken(authority, OfficeConfig.ApiUri, _token).AccessToken
                );

                return services.Value;
            }
            finally
            {
                services = null;
            }
        }

        /// <summary>
        /// Synchronously executes an asynchronous function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
        private static T SynchronousExecute<T>(Func<Task<T>> operation)
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