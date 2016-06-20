// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Configuration;

namespace Microsoft.Store.PartnerCenter.Samples.Common
{
    public static class PowerBIConfig
    {
        public static string AccessKey
        {
            get { return ConfigurationManager.AppSettings["PowerBI:AccessKey"]; }
        }

        public static Uri BaseUri
        {
            get { return new Uri(ConfigurationManager.AppSettings["PowerBI:BaseUri"]); }

        }

        public static string WorkspaceCollectionName
        {
            get { return ConfigurationManager.AppSettings["PowerBI:WorkspaceCollection"]; }
        }

        public static string WorkspaceId
        {
            get { return ConfigurationManager.AppSettings["PowerBI:WorkspaceId"]; }
        }
    }
}