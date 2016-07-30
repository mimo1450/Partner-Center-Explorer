// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model for service requests from Partner Center.
    /// </summary>
    public class ServiceRequestsModel
    {
        /// <summary>
        /// Gets or sets the service requests.
        /// </summary>
        /// <value>
        /// The service requests found in Partner Center.
        /// </value>
        public List<ServiceRequestModel> ServiceRequests
        { get; set; }
    }
}