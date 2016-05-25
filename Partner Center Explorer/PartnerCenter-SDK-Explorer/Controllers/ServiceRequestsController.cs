using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.ServiceRequests;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    /// <summary>
    /// Controller class for the Serivce Requests view. 
    /// </summary>
    /// <seealso cref="Controller" />
    [Authorize]
    public class ServiceRequestsController : Controller
    {
        SdkContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRequestsController"/> class.
        /// </summary>
        public ServiceRequestsController()
        { }

        /// <summary>
        /// Handles the HTTP GET request for the Index view. 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets a list of service requests opened by the partner. 
        /// </summary>
        /// <returns>
        /// A list of service requests opened by the partner.
        /// </returns>
        [HttpPost]
        public JsonResult GetServiceRequests()
        {
            ResourceCollection<ServiceRequest> requests;

            try
            {
                requests = Context.PartnerOperations.ServiceRequests.Get();
                return Json(new { Result = "OK", Records = requests.Items, TotalRecordCount = requests.TotalCount });

            }
            finally
            {
                requests = null;
            }
        }

        private SdkContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new SdkContext();
                }

                return _context;
            }
        }
    }
}