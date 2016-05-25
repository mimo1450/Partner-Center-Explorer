using Microsoft.Samples.Office365.Management.API.Models;
using Microsoft.Store.PartnerCenter.Samples.Common;
using Microsoft.Store.PartnerCenter.Samples.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Microsoft.Samples.Office365.Management.API
{
    public class ServiceCommunications
    {
        private AuthorizationToken _token;
        private Communication _comm;
        private string _userAssertionToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCommunications" /> class.
        /// </summary>
        /// <param name="userAssertionToken">The user assertion token.</param>
        /// <exception cref="ArgumentNullException">userAssertionToken</exception>
        public ServiceCommunications(string userAssertionToken)
        {
            if (string.IsNullOrEmpty(userAssertionToken))
            {
                throw new ArgumentNullException("userAssertionToken");
            }

            _comm = new Communication();
            _userAssertionToken = userAssertionToken;
        }

        /// <summary>
        /// Gets the current status for the specified tenant.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns>A list of status details associated with the specified tenant identifier.</returns>
        /// <exception cref="ArgumentNullException">tenantId</exception>
        public List<StatusDetails> GetCurrentStatus(string tenantId)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException("tenantId");
            }

            return SynchronousExecute(() => GetCurrentStatusAsync(tenantId));
        }

        /// <summary>
        /// Gets the current status for the specified tenant asynchronously.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns>A list of status details associated with the specified tenant identifier.</returns>
        /// <exception cref="ArgumentNullException">tenantId</exception>
        public async Task<List<StatusDetails>> GetCurrentStatusAsync(string tenantId)
        {
            Result<StatusDetails> records;
            string requestUri;

            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException("tenantId");
            }

            try
            {
                requestUri = string.Format(
                    "{0}/api/v1.0/{1}/ServiceComms/CurrentStatus",
                    Configuration.O365ManageAPIRoot,
                    tenantId
                );

                records = await _comm.GetAsync<Result<StatusDetails>>(
                    requestUri,
                    new MediaTypeWithQualityHeaderValue("application/json"),
                    GetToken(tenantId).AccessToken
                );

                return records.Value;
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
                throw new ArgumentNullException("tenantId");
            }
            else if (string.IsNullOrEmpty(messageId))
            {
                throw new ArgumentNullException("messageId");
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
            string requestUri;

            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException("tenantId");
            }
            else if (string.IsNullOrEmpty(messageId))
            {
                throw new ArgumentNullException("messageId");
            }

            try
            {
                requestUri = string.Format(
                    "{0}/api/v1.0/{1}/ServiceComms/Messages?$filter=Id eq '{2}'",
                    Configuration.O365ManageAPIRoot,
                    tenantId,
                    messageId
                );

                results = await _comm.GetAsync<Result<Message>>(
                    requestUri,
                    new MediaTypeWithQualityHeaderValue("application/json"),
                    GetToken(tenantId).AccessToken
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
                throw new ArgumentNullException("tenantId");
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
            string requestUri;

            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException("tenantId");
            }

            try
            {
                requestUri = string.Format(
                    "{0}/api/v1.0/{1}/ServiceComms/Messages",
                    Configuration.O365ManageAPIRoot,
                    tenantId
                );

                messages = await _comm.GetAsync<Result<Message>>(
                    requestUri,
                    new MediaTypeWithQualityHeaderValue("application/json"),
                    GetToken(tenantId).AccessToken
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
                throw new ArgumentNullException("tenantId");
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
            string requestUri;

            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(tenantId);
            }

            try
            {
                requestUri = string.Format(
                    "{0}/api/v1.0/{1}/ServiceComms/Services",
                    Configuration.O365ManageAPIRoot,
                    tenantId
                );

                services = await _comm.GetAsync<Result<Service>>(
                    requestUri,
                    new MediaTypeWithQualityHeaderValue("application/json"),
                    GetToken(tenantId).AccessToken
                );

                return services.Value;
            }
            finally
            {
                services = null;
            }
        }

        /// <summary>
        /// Gets a token.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">tenantId</exception>
        private AuthorizationToken GetToken(string tenantId)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException("tenantId");
            }

            if (_token == null)
            {
                _token = SynchronousExecute(() => GetTokenAsync(tenantId));
            }

            return _token;
        }

        /// <summary>
        /// Gets a token asynchronously.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">tenantId</exception>
        private async Task<AuthorizationToken> GetTokenAsync(string tenantId)
        {
            HttpContent content;
            List<KeyValuePair<string, string>> values;
            string requestUri;

            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException("tenantId");
            }

            try
            {
                values = new List<KeyValuePair<string, string>>();

                values.Add(new KeyValuePair<string, string>("assertion", _userAssertionToken));
                values.Add(new KeyValuePair<string, string>("client_id", Configuration.ApplicationId));
                values.Add(new KeyValuePair<string, string>("client_secret", Configuration.ApplicationSecret));
                values.Add(new KeyValuePair<string, string>("grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer"));
                values.Add(new KeyValuePair<string, string>("requested_token_use", "on_behalf_of"));
                values.Add(new KeyValuePair<string, string>("resource", Configuration.O365ManageAPIRoot));
                values.Add(new KeyValuePair<string, string>("scope", "openid"));

                content = new FormUrlEncodedContent(values);

                requestUri = string.Format(
                    "{0}/{1}/oauth2/token",
                    Configuration.Authority,
                    tenantId
                );

                return await _comm.PostAsync<AuthorizationToken>(requestUri, content);
            }
            finally
            {
                content = null;
                values = null;
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