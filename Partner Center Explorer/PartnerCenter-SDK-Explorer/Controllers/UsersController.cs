// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Samples.AzureAD.Graph.API;
using Microsoft.Samples.AzureAD.Graph.API.Models;
using Microsoft.Store.PartnerCenter.Samples.Common;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models;
using System;
using System.Web.Mvc;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    [AuthorizationFilter(ClaimType = ClaimTypes.Role, ClaimValue = "PartnerAdmin")]
    public class UsersController : Controller
    {
        [HttpPost]
        public ActionResult DeleteUser(string customerId, string userId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult Edit(string customerId, string userId)
        {
            AuthenticationResult token;
            GraphClient client;
            User user;
            UserEditModel userEditModel;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }
            else if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
            }

            try
            {
                token = TokenContext.GetAADToken(
                    string.Format("{0}/{1}/oauth", AppConfig.Authority, customerId),
                    AppConfig.GraphUri
                );

                client = new GraphClient(token.AccessToken);
                user = client.GetUser(customerId, userId);

                userEditModel = new UserEditModel()
                {
                    AssignedLicenses = user.AssignedLicenses,
                    AssignedPlans = user.AssignedPlans,
                    Email = user.Mail,
                    ObjectId = user.ObjectId,
                    SubscribedSkus = user.AvailableSkus,
                    Username = user.UserPrincipalName
                };

                return PartialView(userEditModel);

            }
            finally
            {
                client = null;
                token = null;
                user = null;
            }
        }

        [HttpPost]
        public ActionResult UpdateUser(string customerId, User user)
        {
            throw new NotImplementedException();
        }
    }
}