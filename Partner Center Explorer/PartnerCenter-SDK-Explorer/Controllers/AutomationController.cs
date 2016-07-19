// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Samples.Azure.Management;
using Microsoft.Store.PartnerCenter.Samples.Common;
using Microsoft.Store.PartnerCenter.Samples.Common.Context;
using Microsoft.Store.PartnerCenter.Samples.Common.Models;
using Microsoft.Store.PartnerCenter.Samples.Common.Models.Automation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    /// <summary>
    /// Handles the Automation view requests.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [AuthorizationFilter(ClaimType = ClaimTypes.Role, ClaimValue = "PartnerAdmin")]
    public class AutomationController : Controller
    {
        /// <summary>
        /// Handle the Index view request.
        /// </summary>
        /// <returns>An empty view is returned.</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Loads the invoke runbook dialog.
        /// </summary>
        /// <param name="runbookName">Name of the runbook.</param>
        /// <returns>A partial view containing an instance of <see cref="InvokeRunbookModel"/>.</returns>
        [HttpGet]
        public PartialViewResult Invoke(string runbookName)
        {
            InvokeRunbookModel invokeRunbookModel = new InvokeRunbookModel()
            {
                ResourceGroupName = AppConfig.AutomationResourceGroup,
                RunbookName = runbookName
            };

            return PartialView(invokeRunbookModel);
        }

        /// <summary>
        /// Invokes the specified runbook. 
        /// </summary>
        /// <param name="model">An aptly populated instance of <see cref="InvokeRunbookModel"/>.</param>
        /// <returns>A partial view containing a list of runbooks.</returns>
        [HttpPost]
        public async Task<PartialViewResult> Invoke(InvokeRunbookModel model)
        {
            AuthorizationToken token;
            Automation automation;
            IList<RunbookModel> runbooks;

            try
            {
                token = await TokenContext.GetAADTokenAsync($"{AppConfig.Authority}/{AppConfig.AutomationTenantId}/oauth2/token",
                    AppConfig.ManagementUri, AppConfig.AutomationUsername, AppConfig.AutomationPassword);
                automation = new Automation(AppConfig.AutomationSubscriptionId, token.AccessToken);
                await automation.InvokeRunbookAsync(model.ResourceGroupName, AppConfig.AutomationAccount, model.RunbookName);
                runbooks = await GetRunbooks(token.AccessToken);

                return PartialView("List", runbooks);
            }
            finally
            {
                automation = null;
                token = null;
            }
        }

        /// <summary>
        /// Handles the List view request.
        /// </summary>
        /// <returns>A partial view result containing a list of runbooks.</returns>
        public async Task<PartialViewResult> List()
        {
            IList<RunbookModel> runbooks = await GetRunbooks();
            return PartialView(runbooks);
        }

        private static async Task<IList<RunbookModel>> GetRunbooks()
        {
            AuthorizationToken token;
            Automation automation;
            IList<RunbookModel> runbooks;

            try
            {
                token = await TokenContext.GetAADTokenAsync($"{AppConfig.Authority}/{AppConfig.AutomationTenantId}/oauth2/token",
                    AppConfig.ManagementUri, AppConfig.AutomationUsername, AppConfig.AutomationPassword);
                automation = new Automation(AppConfig.AutomationSubscriptionId, token.AccessToken);
                runbooks = await automation.GetRunbooksAsync(AppConfig.AutomationResourceGroup, AppConfig.AutomationAccount);

                return runbooks;
            }
            finally
            {
                automation = null;
                token = null;
            }
        }

        private static async Task<IList<RunbookModel>> GetRunbooks(string token)
        {
            Automation automation;
            IList<RunbookModel> runbooks;

            try
            {
                automation = new Automation(AppConfig.AutomationSubscriptionId, token);
                runbooks = await automation.GetRunbooksAsync(AppConfig.AutomationResourceGroup, AppConfig.AutomationAccount);

                return runbooks;
            }
            finally
            {
                automation = null;
                token = null;
            }
        }
    }
}