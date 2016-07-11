// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Azure;
using Microsoft.Azure.Common.OData;
using Microsoft.Azure.Insights;
using Microsoft.Azure.Insights.Models;
using Microsoft.Store.PartnerCenter.Samples.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Samples.Azure.Management
{
    public class Insights : IDisposable
    {
        private InsightsClient _client;
        private TokenCloudCredentials _token;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Insights"/> class.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="token">Valid JSON Web Token (JWT).</param>
        /// <exception cref="System.ArgumentNullException">
        /// subscriptionId
        /// or
        /// token
        /// </exception>
        public Insights(string subscriptionId, string token)
        {
            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException("subscriptionId");
            }
            else if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException("token");
            }

            _token = new TokenCloudCredentials(subscriptionId, token);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (_client != null)
                {
                    _client.Dispose();
                }
            }

            _disposed = true;
        }

        public async Task<List<IHealthEvent>> GetHealthEventsAsync()
        {
            DateTime end;
            DateTime start;
            EventDataListResponse response;
            AzureHealthEvent healthEvent;
            List<AzureHealthEvent> healthEvents;
            string filter;

            try
            {
                end = DateTime.Now;
                start = DateTime.Now.AddDays(-7);

                healthEvents = new List<AzureHealthEvent>();

                filter = FilterString.Generate<ListEventsForResourceProviderParameters>(
                    eventData => (eventData.EventTimestamp >= start)
                        && (eventData.EventTimestamp <= end)
                        && (eventData.ResourceProvider == "Azure.Health")
                );

                response = await Client.EventOperations.ListEventsAsync(filter, null);

                foreach (EventData data in response.EventDataCollection.Value)
                {
                    healthEvent = new AzureHealthEvent()
                    {
                        Description = data.Description,
                        EventTimestamp = data.EventTimestamp,
                        ResourceGroupName = data.ResourceGroupName,
                        ResourceId = data.ResourceId,
                        ResourceProviderName = data.ResourceProviderName.Value,
                        ResourceType = data.ResourceType.Value,
                        Status = data.Level.ToString(),
                    };

                    healthEvents.Add(healthEvent);
                }

                return healthEvents.ToList<IHealthEvent>();
            }
            finally
            {
                healthEvent = null;
                response = null;
            }
        }

        private InsightsClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new InsightsClient(_token);
                }

                return _client;
            }
        }
    }
}