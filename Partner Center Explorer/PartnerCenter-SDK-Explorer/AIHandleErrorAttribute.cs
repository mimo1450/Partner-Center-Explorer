// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer
{
    public class AIHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.HttpContext != null && filterContext.Exception != null)
            {
                //If customError is Off, then AI HTTPModule will report the exception
                //If it is On, or RemoteOnly (default) - then we need to explicitly track the exception
                if (filterContext.HttpContext.IsCustomErrorEnabled)
                {
                    TelemetryClient client = new TelemetryClient();
                    client.TrackException(filterContext.Exception);
                }
            }
            base.OnException(filterContext);
        }
    }
}