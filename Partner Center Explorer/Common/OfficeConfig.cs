// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Configuration;

namespace Microsoft.Store.PartnerCenter.Samples.Common
{
    /// <summary>
    /// Helper object for access Office 365 Management API specific configurations.
    /// </summary>
    public static class OfficeConfig
    {
        /// <summary>
        /// Gets the endpoint for the Office 365 Managment API.
        /// </summary>
        /// <value>
        /// The endpoint for the Office 365 Managment API.
        /// </value>
        public static string ApiUri => ConfigurationManager.AppSettings["O365:ApiUri"];
    }
}