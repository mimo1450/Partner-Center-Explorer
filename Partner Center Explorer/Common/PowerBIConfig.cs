// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Configuration;

namespace Microsoft.Store.PartnerCenter.Samples.Common
{
    /// <summary>
    /// Helper object for access Power BI specific configurations.
    /// </summary>
    public static class PowerBIConfig
    {
        /// <summary>
        /// Gets the access key.
        /// </summary>
        /// <value>
        /// The access key for the workspace collection.
        /// </value>
        /// <remarks>
        /// This value is obtained from the Azure portal.
        /// </remarks>

        public static string AccessKey => ConfigurationManager.AppSettings["PowerBI:AccessKey"];

        /// <summary>
        /// Gets the endpoint for the Power BI API.
        /// </summary>
        /// <value>
        /// Enpoint for the Power BI API.
        /// </value>
        public static Uri BaseUri => new Uri(ConfigurationManager.AppSettings["PowerBI:BaseUri"]);

        /// <summary>
        /// Gets the name of the workspace collection.
        /// </summary>
        /// <value>
        /// The name of the workspace collection.
        /// </value>
        /// <remarks>
        /// This value is obtained from the Azure portal. It is the name of the worksapace collection that was provisioned.
        /// </remarks>
        public static string WorkspaceCollectionName => ConfigurationManager.AppSettings["PowerBI:WorkspaceCollection"];

        /// <summary>
        /// Gets the workspace identifier.
        /// </summary>
        /// <value>
        /// The workspace identifier value.
        /// </value>
        /// <remarks>
        /// This value is identifier of the workspace provisiong using the ProvisiongSample from the Power BI team.
        /// </remarks>
        public static string WorkspaceId => ConfigurationManager.AppSettings["PowerBI:WorkspaceId"];
    }
}