// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Samples.Common;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            if (string.IsNullOrEmpty(AppConfig.InstrumentationKey))
            {
                filters.Add(new HandleErrorAttribute());
            }
            else
            {
                filters.Add(new AIHandleErrorAttribute());
            }
        }
    }
}