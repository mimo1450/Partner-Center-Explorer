// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Configuration;

namespace Microsoft.Store.PartnerCenter.Samples.Common
{
    public static class AppConfig
    {
        public static string AccountId
        {
            get { return ConfigurationManager.AppSettings["AccountId"]; }
        }

        public static string ApplicationId
        {
            get { return ConfigurationManager.AppSettings["ApplicationId"]; }
        }

        public static string ApplicationSecret
        {
            get { return ConfigurationManager.AppSettings["ApplicationSecret"]; }
        }

        public static string Authority
        {
            get { return ConfigurationManager.AppSettings["Authority"]; }
        }

        public static string GraphUri
        {
            get { return ConfigurationManager.AppSettings["Azure:ADGraphUri"]; }
        }

        public static bool IsSandboxEnvironment
        {
            get { return Convert.ToBoolean(ConfigurationManager.AppSettings["IsSandboxEnvironment"]); }
        }

        public static string ManagementUri
        {
            get { return ConfigurationManager.AppSettings["Azure:ManagementUri"]; }
        }

        public static string PartnerCenterApiUri
        {
            get { return ConfigurationManager.AppSettings["PartnerCenterApiUri"]; }
        }

        public static string RedisConnection
        {
            get { return ConfigurationManager.AppSettings["Redis:Connnection"]; }
        }
    }
}