// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Store.PartnerCenter.Samples.Common;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            if (!string.IsNullOrEmpty(AppConfig.InstrumentationKey))
            {
                TelemetryConfiguration.Active.InstrumentationKey = AppConfig.InstrumentationKey;
            }
        }
    }
}