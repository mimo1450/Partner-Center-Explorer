using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    /// <summary>
    /// Controller for the Home views.
    /// </summary>
    /// <seealso cref="Controller" />
    public class HomeController : Controller
    {
        /// <summary>
        /// Handles the HTTP GET request for the Index view.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}