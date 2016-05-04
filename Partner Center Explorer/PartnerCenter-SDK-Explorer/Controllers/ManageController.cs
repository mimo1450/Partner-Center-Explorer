using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Samples.AzureAD.Graph.API;
using Microsoft.Samples.AzureAD.Graph.API.Models;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    /// <summary>
    /// Controller class for the Manage views.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class ManageController : Controller
    {
        private ResourceContext _resource;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageController"/> class.
        /// </summary>
        public ManageController()
        {
            _resource = new ResourceContext();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageController"/> class.
        /// </summary>
        /// <param name="resourceContext">The resource context.</param>
        public ManageController(ResourceContext resourceContext)
        {
            _resource = resourceContext;
        }

        /// <summary>
        /// Handles the HTTP GET request for the Azure view.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Azure(string subscriptionId, string tenantId)
        {
            if (string.IsNullOrEmpty(subscriptionId) || string.IsNullOrEmpty(tenantId))
            {
                return RedirectToAction("Index", "Customers");
            }

            return View();
        }


        /// <summary>
        /// Handles the HTTP GET request for the Office view.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Office(string tenantId)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                return RedirectToAction("Index", "Customers");
            }

            return View();
        }

        /// <summary>
        /// Gets a list of Azure resource belonging to the speicifed subscription.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns>
        /// A list of Azure resources that belong to the specified subscription.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// subscriptionId
        /// or
        /// tenantId
        /// </exception>
        /// <remarks>
        /// The data for this function is obtained using the Azure Resource Manager API.
        /// Additional information regarding this API can be found at 
        /// https://msdn.microsoft.com/en-us/library/azure/dn790568.aspx
        /// </remarks>
        [HttpPost]
        public JsonResult GetResources(string subscriptionId, string tenantId)
        {
            List<GenericResource> resources;

            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException("subscriptionId");
            }
            else if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException("tenantId");
            }

            try
            {
                resources = _resource.GetResources(subscriptionId, tenantId);
                return Json(new { Result = "OK", Records = resources, TotalRecordCount = resources.Count });
            }
            finally
            {
                resources = null;
            }

        }

        /// <summary>
        /// Gets a list of Azure resource groups for the speicifed subscription.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// subscriptionId
        /// or
        /// tenantId
        /// </exception>
        /// <remarks>
        /// The data for this function is obtained using the Azure Resource Manager API.
        /// Additional information regarding this API can be found at 
        /// https://msdn.microsoft.com/en-us/library/azure/dn790568.aspx
        /// </remarks>
        [HttpPost]
        public JsonResult GetResourceGroups(string subscriptionId, string tenantId)
        {
            List<ResourceGroup> groups;

            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException("subscriptionId");
            }
            else if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException("tenantId");
            }

            try
            {
                groups = _resource.GetResourceGroups(subscriptionId, tenantId);
                return Json(new { Result = "OK", Records = groups, TotalRecordCount = groups.Count });
            }
            finally
            {
                groups = null;
            }
        }

        /// <summary>
        /// Gets a list of users for the specified tenant.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns>
        /// A list of users for the specified tenant.
        /// </returns>
        /// <exception cref="ArgumentNullException">tenantId</exception>
        /// <remarks>
        /// The data for this function is obtained using the Azure AD Graph API.
        /// Additional information regarding Azure AD Graph API users call can be found at
        /// https://msdn.microsoft.com/en-us/library/azure/ad/graph/api/users-operations
        /// </remarks>
        [HttpPost]
        public JsonResult GetUsers(string tenantId)
        {
            GraphClient client;
            List<User> users;

            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException("tenantId");
            }

            try
            {
                client = new GraphClient();
                users = client.GetUsers(tenantId);

                return Json(new { Result = "OK", Records = users, TotalRecordCount = users.Count });
            }
            finally
            {
                client = null;
                users = null;
            }
        }
    }
}