// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Samples.AzureAD.Graph.API.Models
{
    /// <summary>
    /// Represents a collection returned from an Azure AD Graph API call. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T>
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The values returned from the Azure AD Graph API call.
        /// </value>
        public List<T> Value
        { get; set; }
    }
}