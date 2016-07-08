// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    /// <summary>
    /// Controller for all Home views.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [AuthorizationFilter(ClaimType = ClaimTypes.Role, ClaimValue = "PartnerAdmin")]
    public class HomeController : Controller
    {
        /// <summary>
        /// Handles the request for the index view.
        /// </summary>
        /// <returns>Returns an empty view.</returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}