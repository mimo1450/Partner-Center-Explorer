// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Microsoft.Store.PartnerCenter.Samples.Common
{
    public class Communication
    {
        /// <summary>
        /// Sends a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="requestUri">The Uri where the request should be sent.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="token">The access token value used to authorize the request.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// requestUri
        /// or
        /// mediaType
        /// or
        /// token
        /// </exception>
        /// <exception cref="CommunicationException"></exception>
        public async Task<T> GetAsync<T>(string requestUri, MediaTypeWithQualityHeaderValue mediaType, string token)
        {
            HttpResponseMessage response;

            if (string.IsNullOrEmpty(requestUri))
            {
                throw new ArgumentNullException(nameof(requestUri));
            }
            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Add the required headers for the request.
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Accept.Add(mediaType);

                    response = await client.GetAsync(requestUri);

                    if (!response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        throw new CommunicationException(result, response.StatusCode);
                    }

                    return await response.Content.ReadAsAsync<T>();
                }
            }
            finally
            {
                response = null;
            }
        }

        /// <summary>
        /// Sends a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="requestUri">The Uri where the request should be sent.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="token">The access token value used to authorize the request.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// requestUri
        /// or
        /// mediaType
        /// or
        /// token
        /// </exception>
        /// <exception cref="CommunicationException"></exception>
        public async Task<string> GetStringAsync<T>(string requestUri, MediaTypeWithQualityHeaderValue mediaType, string token)
        {
            HttpResponseMessage response;

            if (string.IsNullOrEmpty(requestUri))
            {
                throw new ArgumentNullException(nameof(requestUri));
            }
            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Add the required headers for the request.
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Accept.Add(mediaType);

                    response = await client.GetAsync(requestUri);

                    if (!response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        throw new CommunicationException(result, response.StatusCode);
                    }

                    return await response.Content.ReadAsStringAsync();
                }
            }
            finally
            {
                response = null;
            }
        }

        /// <summary>
        /// Sends a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="requestUri">The Uri where the request should be sent.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// requestUri
        /// or
        /// content
        /// </exception>
        /// <exception cref="CommunicationException"></exception>
        public async Task<T> PostAsync<T>(string requestUri, HttpContent content)
        {
            HttpResponseMessage response;

            if (string.IsNullOrEmpty(requestUri))
            {
                throw new ArgumentNullException(nameof(requestUri));
            }
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    response = await client.PostAsync(requestUri, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        throw new CommunicationException(result, response.StatusCode);
                    }

                    return await response.Content.ReadAsAsync<T>();
                }
            }
            finally
            {
                response = null;
            }
        }

        /// <summary>
        /// Sends a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="requestUri">The Uri where the request should be sent.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="token">The access token value used to authorize the request.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// requestUri
        /// or
        /// mediaType
        /// or
        /// content
        /// or
        /// token
        /// </exception>
        /// <exception cref="CommunicationException"></exception>
        public async Task<T> PostAsync<T>(string requestUri, MediaTypeWithQualityHeaderValue mediaType, HttpContent content, string token)
        {
            HttpResponseMessage response;

            if (string.IsNullOrEmpty(requestUri))
            {
                throw new ArgumentNullException(nameof(requestUri));
            }
            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Accept.Add(mediaType);

                    response = await client.PostAsync(requestUri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsAsync<T>();
                    }

                    string result = await response.Content.ReadAsStringAsync();
                    throw new CommunicationException(result, response.StatusCode);
                }
            }
            finally
            {
                response = null;
            }
        }

        /// <summary>
        /// Asynchronously posts the content as JSON.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="content">The content.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// requestUri
        /// or
        /// mediaType
        /// or
        /// content
        /// or
        /// token
        /// </exception>
        /// <exception cref="CommunicationException"></exception>
        public async Task<T> PostAsJsonAsync<T>(string requestUri, MediaTypeWithQualityHeaderValue mediaType, T content, string token)
        {
            HttpResponseMessage response;

            if (string.IsNullOrEmpty(requestUri))
            {
                throw new ArgumentNullException(nameof(requestUri));
            }
            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Accept.Add(mediaType);

                    response = await client.PostAsJsonAsync(requestUri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsAsync<T>();
                    }

                    string result = await response.Content.ReadAsStringAsync();
                    throw new CommunicationException(result, response.StatusCode);
                }
            }
            finally
            {
                response = null;
            }
        }
    }
}