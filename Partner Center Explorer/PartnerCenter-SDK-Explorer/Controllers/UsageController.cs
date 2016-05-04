using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.Usage;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    /// <summary>
    /// Controller for the Usage views. 
    /// </summary>
    /// <seealso cref="Controller" />
    public class UsageController : Controller
    {
        IAggregatePartner _operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsageController"/> class.
        /// </summary>
        public UsageController()
        {
            _operations = SdkContext.UserPartnerOperations;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsageController"/> class.
        /// </summary>
        /// <param name="operations">An instance of <see cref="IAggregatePartner"/>.</param>
        public UsageController(IAggregatePartner operations)
        {
            _operations = operations;
        }

        /// <summary>
        /// Handles the HTTP GET request for the Index view.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// subscriptionId
        /// or
        /// tenantId
        /// </exception>
        public ActionResult Index(string subscriptionId, string tenantId)
        {
            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException("subscriptionId");
            }
            else if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException("tenantId");
            }

            ViewBag.SubscriptionId = subscriptionId;
            ViewBag.TenantId = tenantId;

            return View();
        }

        /// <summary>
        /// Gets a collection of resources that contains a list of services within the specified subscription and the 
        /// associated rated usage information.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns>
        /// An instance of <see cref="JsonResult"/> containing a list of services within the specified subscription and 
        /// the associated rated usage information.
        /// </returns>
        /// <remarks>
        /// This function uses the Partner Center SDK to obtain the rated usage information. Addtional information can
        /// be found at https://msdn.microsoft.com/en-us/library/partnercenter/mt651646.aspx
        /// </remarks>
        [HttpPost]
        public JsonResult GetUsage(string subscriptionId, string tenantId)
        {
            ResourceCollection<AzureResourceMonthlyUsageRecord> records;

            if (string.IsNullOrEmpty(subscriptionId))
            {
                return Json(new { Result = "ERROR", Message = "Argument subscriptionId cannot be null." });
            }
            else if (string.IsNullOrEmpty(tenantId))
            {
                return Json(new { Result = "ERROR", Message = "Argument tenantId cannot be null." });
            }

            try
            {
                records = _operations.Customers.ById(tenantId).Subscriptions.ById(subscriptionId).UsageRecords.Resources.Get();

                // This is done to work around a circular reference error that occurs when attempting to 
                // serialize the CurrencyLocale property. 
                var dataSet = records.Items.Select(x => new
                {
                    Category = x.Category, 
                    LastModifiedDate = x.LastModifiedDate,
                    QuantityUsed = x.QuantityUsed, 
                    ResourceId = x.ResourceId,
                    ResourceName = x.ResourceName,
                    Subcategory = x.Subcategory,
                    TotalCost = x.TotalCost, 
                    Unit = x.Unit
                });


                return Json(new { Result = "OK", Records = dataSet, TotalRecordCount = records.TotalCount });
            }
            finally
            {
                records = null;
            }
        }
    }
}