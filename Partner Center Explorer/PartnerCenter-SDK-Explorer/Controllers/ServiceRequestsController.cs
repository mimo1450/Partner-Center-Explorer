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
    public class ServiceRequestsController : Controller
    {
        IAggregatePartner _operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRequestsController"/> class.
        /// </summary>
        public ServiceRequestsController()
        {
            _operations = SdkContext.UserPartnerOperations;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRequestsController"/> class.
        /// </summary>
        /// <param name="operations">An instance of <see cref="IAggregatePartner"/>.</param>
        public ServiceRequestsController(IAggregatePartner operations)
        {
            _operations = operations;
        }

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
                requests = _operations.ServiceRequests.Get();
                return Json(new { Result = "OK", Records = requests.Items, TotalRecordCount = requests.TotalCount });

            }
            finally
            {
                requests = null;
            }
        }
    }
}