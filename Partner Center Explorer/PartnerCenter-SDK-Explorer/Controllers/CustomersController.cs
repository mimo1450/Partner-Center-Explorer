using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.Customers;
using Microsoft.Store.PartnerCenter.Models.Subscriptions;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    /// <summary>
    /// Controller for the Customers views. 
    /// </summary>
    /// <seealso cref="Controller" />
    public class CustomersController : Controller
    {
        IAggregatePartner _operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersController"/> class.
        /// </summary>
        public CustomersController()
        {
            _operations = SdkContext.UserPartnerOperations;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersController"/> class.
        /// </summary>
        /// <param name="operations">An instance of <see cref="IAggregatePartner"/>.</param>
        public CustomersController(IAggregatePartner operations)
        {
            _operations = operations;
        }

        /// <summary>
        /// Handles the HTTP GET for the Index view.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets a collection of resources representing all the partner's customers.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="JsonResult"/> containing a collect of resource representing the partner's customers.
        /// </returns>
        /// <remarks>
        /// This function uses the Partner Center SDK to retrieve a collection of resources representing the partner's customers.
        /// Additional information can be found at https://msdn.microsoft.com/en-us/library/partnercenter/mt634685.aspx
        /// </remarks>
        [HttpPost]
        public JsonResult GetCustomers()
        {
            ResourceCollection<Customer> customers;

            try
            {
                customers = _operations.Customers.Get();
                return Json(new { Result = "OK", Records = customers.Items, TotalRecordCount = customers.TotalCount });
            }
            finally
            {
                customers = null;
            }
        }

        /// <summary>
        /// Gets a collection of resources that represent the subscriptions for the specified tenant.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns>
        /// An instance of <see cref="JsonResult"/> contain an array of resources representing the subscriptions for the specified tenant. 
        /// </returns>
        /// <remarks>
        /// This function uses the Partner Center SDK to retrieve a collection of resources representing the subscriptions for 
        /// tenant with the specified identifier. Additional information can be found at 
        /// https://msdn.microsoft.com/en-us/library/partnercenter/mt634673.aspx
        /// </remarks>
        [HttpPost]
        public JsonResult GetSubscriptions(string tenantId)
        {
            ResourceCollection<Subscription> subscriptions;

            if (string.IsNullOrEmpty(tenantId))
            {
                return Json(new { Result = "ERROR", Message = "Argument tenantId cannot be null." });
            }

            try
            {
                subscriptions = _operations.Customers.ById(tenantId).Subscriptions.Get();
                return Json(new { Result = "OK", Records = subscriptions.Items, TotalRecordCount = subscriptions.TotalCount });
            }
            finally
            {
                subscriptions = null;
            }
        }
    }
}