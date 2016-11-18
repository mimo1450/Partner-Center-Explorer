// -----------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Explorer
{
    using Configuration;
    using Configuration.Bundling;
    using Configuration.Manager;
    using System.IO;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            FilterConfig.RegisterWebApiFilters(GlobalConfiguration.Configuration.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            string path = Path.Combine(HttpRuntime.AppDomainAppPath, @"Configuration\WebPortalConfiguration.json");

            IWebPortalConfigurationFactory webPortalConfigFactory = new WebPortalConfigurationFactory();
            ApplicationConfiguration.WebPortalConfigurationManager =
                webPortalConfigFactory.Create(path);

            // setup the application assets bundles
            ApplicationConfiguration.WebPortalConfigurationManager.UpdateBundles(Bundler.Instance).Wait();
        }
    }
}