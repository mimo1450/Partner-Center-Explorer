// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.Licenses;
using Microsoft.Store.PartnerCenter.Models.Users;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context;
using Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Store.PartnerCenter.Samples.Common;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    /// <summary>
    /// Controller for Users views.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [AuthorizationFilter(ClaimType = ClaimTypes.Role, ClaimValue = "PartnerAdmin")]
    public class UsersController : Controller
    {
        private SdkContext _context;

        /// <summary>
        /// Handles the Create view request.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>A partial view containing the NewUserModel model.</returns>
        /// <exception cref="System.ArgumentNullException">customerId</exception>
        [HttpGet]
        public PartialViewResult Create(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            NewUserModel newUserModel = new NewUserModel()
            {
                CustomerId = customerId
            };

            return PartialView(newUserModel);
        }

        /// <summary>
        /// Handles the Create user HTTP post.
        /// </summary>
        /// <param name="newUserModel">The new user model.</param>
        /// <returns>A partial view containing a list of users.</returns>
        [HttpPost]
        public async Task<PartialViewResult> Create(NewUserModel newUserModel)
        {
            CustomerUser customerUser;

            try
            {
                customerUser = new CustomerUser()
                {
                    DisplayName = newUserModel.DisplayName,
                    FirstName = newUserModel.FirstName,
                    LastName = newUserModel.LastName,
                    PasswordProfile = new PasswordProfile()
                    {
                        ForceChangePassword = false,
                        Password = newUserModel.Password
                    },
                    UsageLocation = newUserModel.UsageLocation,
                    UserPrincipalName = newUserModel.UserPrincipalName
                };

                await Context.PartnerOperations.Customers.ById(newUserModel.CustomerId).Users.CreateAsync(customerUser);

                UsersModel usersModel = new UsersModel()
                {
                    CustomerId = newUserModel.CustomerId,
                    Users = await GetUsersAsync(newUserModel.CustomerId)
                };

                return PartialView("List", usersModel);
            }
            finally
            {
                customerUser = null;
            }
        }

        /// <summary>
        /// Deletes the specified user.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A HTTP OK if the delete is successful.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// customerId
        /// or
        /// userId
        /// </exception>
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(string customerId, string userId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            await Context.PartnerOperations.Customers.ById(customerId).Users.ById(userId).DeleteAsync();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Edits the specified user.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A partial view containing the EditUserModel model.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        [HttpGet]
        public async Task<PartialViewResult> Edit(string customerId, string userId)
        {
            CustomerUser customerUser;
            EditUserModel editUserModel;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            try
            {
                customerUser = await Context.PartnerOperations.Customers.ById(customerId).Users.ById(userId).GetAsync();

                editUserModel = new EditUserModel()
                {
                    CustomerId = customerId,
                    DisplayName = customerUser.DisplayName,
                    FirstName = customerUser.FirstName,
                    LastName = customerUser.LastName,
                    Licenses = await GetLicenses(customerId, userId),
                    UsageLocation = AppConfig.CountryCode,
                    UserId = userId,
                    UserPrincipalName = customerUser.UserPrincipalName
                };

                return PartialView(editUserModel);
            }
            finally
            {
                customerUser = null;
            }
        }

        /// <summary>
        /// Edits the specified user.
        /// </summary>
        /// <param name="editUserModel">The edit user model.</param>
        /// <returns>A list of users.</returns>
        [HttpPost]
        public async Task<PartialViewResult> Edit(EditUserModel editUserModel)
        {
            CustomerUser customerUser;

            try
            {
                customerUser = await Context.PartnerOperations.Customers.ById(editUserModel.CustomerId)
                    .Users.ById(editUserModel.UserId).GetAsync();

                customerUser.DisplayName = editUserModel.DisplayName;
                customerUser.FirstName = editUserModel.FirstName;
                customerUser.LastName = editUserModel.LastName;
                customerUser.UserPrincipalName = editUserModel.UserPrincipalName;
                customerUser.UsageLocation = editUserModel.UsageLocation;

                await Context.PartnerOperations.Customers.ById(editUserModel.CustomerId)
                    .Users.ById(editUserModel.UserId).PatchAsync(customerUser);

                await ProcessLicenseModifications(editUserModel);

                UsersModel usersModel = new UsersModel()
                {
                    CustomerId = editUserModel.CustomerId,
                    Users = await GetUsersAsync(editUserModel.CustomerId)
                };

                return PartialView("List", usersModel);
            }
            finally
            {
                customerUser = null;
            }
        }

        /// <summary>
        /// Lists the users that belong to the specified customer identifier.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>A list of users that belong to the specified customer identifier.</returns>
        /// <exception cref="System.ArgumentNullException">customerId</exception>
        public async Task<PartialViewResult> List(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            UsersModel usersModel = new UsersModel()
            {
                CustomerId = customerId,
                Users = await GetUsersAsync(customerId)
            };

            return PartialView(usersModel);
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

        private async Task<List<LicenseModel>> GetLicenses(string customerId, string userId)
        {
            LicenseModel licenseModel;
            List<LicenseModel> values;
            ResourceCollection<License> licenses;
            ResourceCollection<SubscribedSku> subscribedSkus;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            else if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            try
            {
                values = new List<LicenseModel>();
                licenses = await Context.PartnerOperations.Customers.ById(customerId).Users.ById(userId).Licenses.GetAsync();
                subscribedSkus = await Context.PartnerOperations.Customers.ById(customerId).SubscribedSkus.GetAsync();

                foreach (SubscribedSku sku in subscribedSkus.Items)
                {
                    licenseModel = new LicenseModel()
                    {
                        ConsumedUnits = sku.ConsumedUnits,
                        Id = sku.ProductSku.Id,
                        IsAssigned = licenses.Items
                            .SingleOrDefault(x => x.ProductSku.Name.Equals(sku.ProductSku.Name)) != null ? true : false,
                        Name = sku.ProductSku.Name,
                        SkuPartNumber = sku.ProductSku.SkuPartNumber,
                        TargetType = sku.ProductSku.TargetType,
                        TotalUnits = sku.TotalUnits
                    };

                    values.Add(licenseModel);
                }

                return values;
            }
            finally
            {
                licenseModel = null;
                licenses = null;
                subscribedSkus = null;
            }
        }

        private async Task<List<UserModel>> GetUsersAsync(string customerId)
        {
            List<UserModel> results;
            SeekBasedResourceCollection<CustomerUser> users;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            try
            {
                results = new List<UserModel>();
                users = await Context.PartnerOperations.Customers.ById(customerId).Users.GetAsync();

                foreach (CustomerUser u in users.Items)
                {
                    results.Add(new UserModel()
                    {
                        CustomerId = customerId,
                        DisplayName = u.DisplayName,
                        FirstName = u.FirstName,
                        Id = u.Id,
                        LastDirectorySyncTime = u.LastDirectorySyncTime,
                        LastName = u.LastName,
                        UsageLocation = u.UsageLocation,
                        UserPrincipalName = u.UserPrincipalName
                    });
                }

                return results;
            }
            finally
            {
                results = null;
            }
        }

        private async Task ProcessLicenseModifications(EditUserModel model)
        {
            LicenseUpdate licenseUpdate;
            LicenseModel license;
            List<LicenseModel> current;
            List<LicenseAssignment> assignments;
            List<string> removals;

            try
            {
                assignments = new List<LicenseAssignment>();
                current = await GetLicenses(model.CustomerId, model.UserId);
                licenseUpdate = new LicenseUpdate();
                removals = new List<string>();

                foreach (LicenseModel item in current)
                {
                    license = model.Licenses.SingleOrDefault(x => x.Id.Equals(item.Id, StringComparison.CurrentCultureIgnoreCase));

                    if (license != null)
                    {
                        if (!item.IsAssigned && license.IsAssigned)
                        {
                            assignments.Add(new LicenseAssignment() { ExcludedPlans = null, SkuId = license.Id });
                        }
                        else if (item.IsAssigned && !license.IsAssigned)
                        {
                            removals.Add(license.Id);
                        }
                    }
                }

                if (assignments.Count > 0)
                {
                    licenseUpdate.LicensesToAssign = assignments;
                }

                if (removals.Count > 0)
                {
                    licenseUpdate.LicensesToRemove = removals;
                }

                if (assignments.Count > 0 || removals.Count > 0)
                {
                    await Context.PartnerOperations.Customers.ById(model.CustomerId)
                        .Users.ById(model.UserId).LicenseUpdates.CreateAsync(licenseUpdate);
                }
            }
            finally
            {
                assignments = null;
                current = null;
                license = null;
                licenseUpdate = null;
                removals = null;
            }
        }
    }
}