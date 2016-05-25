using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowError(string errorMessage, string signIn)
        {
            ViewBag.SignIn = signIn;
            ViewBag.ErrorMessage = errorMessage;

            return View();
        }
    }
}