// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    /// <summary>
    /// Controller for all Error views.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class ErrorController : Controller
    {
        /// <summary>
        /// Handles the request for the index view.
        /// </summary>
        /// <returns>Returns an empty view.</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Handles the request to show the error.
        /// </summary>
        /// <param name="errorMessage">The error message to be displayed.</param>
        /// <returns>Returns an empty view with an aptly populated ViewBag.</returns>
        public ActionResult ShowError(string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;

            return View();
        }
    }
}