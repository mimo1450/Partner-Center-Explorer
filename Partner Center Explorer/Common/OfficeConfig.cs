// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Configuration;

namespace Microsoft.Store.PartnerCenter.Samples.Common
{
    public static class OfficeConfig
    {
        public static string ApiUri
        {
            get { return ConfigurationManager.AppSettings["O365:ApiUri"]; }
        }
    }
}