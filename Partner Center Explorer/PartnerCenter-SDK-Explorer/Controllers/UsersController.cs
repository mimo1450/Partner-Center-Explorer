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

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Controllers
{
    [AuthorizationFilter(ClaimType = ClaimTypes.Role, ClaimValue = "PartnerAdmin")]
    public class UsersController : Controller
    {
        private SdkContext _context;

        [HttpGet]
        public ActionResult Create(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }

            NewUserModel newUserModel = new NewUserModel()
            {
                CustomerId = customerId
            };

            return View(newUserModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create(NewUserModel newUserModel)
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
                    Users = await Context.PartnerOperations.Customers.ById(newUserModel.CustomerId).Users.GetAsync()
                };

                return PartialView("ListUsers", usersModel);
            }
            finally
            {
                customerUser = null;
            }
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(string customerId, string userId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }
            else if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
            }

            await Context.PartnerOperations.Customers.ById(customerId).Users.ById(userId).DeleteAsync();

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string customerId, string userId)
        {
            CustomerUser customerUser;
            EditUserModel editUserModel;

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
                customerUser = await Context.PartnerOperations.Customers.ById(customerId).Users.ById(userId).GetAsync();

                editUserModel = new EditUserModel()
                {
                    CustomerId = customerId,
                    DisplayName = customerUser.DisplayName,
                    FirstName = customerUser.FirstName,
                    LastName = customerUser.LastName,
                    Licenses = await GetLicenses(customerId, userId),
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

        [HttpPost]
        public async Task<ActionResult> Edit(EditUserModel editUserModel)
        {
            CustomerUser customerUser;

            try
            {
                customerUser = await Context.PartnerOperations.Customers.ById(editUserModel.CustomerId)
                    .Users.ById(editUserModel.UserId).GetAsync();

                if (customerUser.DisplayName != editUserModel.DisplayName)
                {
                    customerUser.DisplayName = editUserModel.DisplayName;
                }
                else if (customerUser.FirstName != editUserModel.FirstName)
                {
                    customerUser.FirstName = editUserModel.FirstName;
                }
                else if (customerUser.LastName != editUserModel.LastName)
                {
                    customerUser.LastName = editUserModel.LastName;
                }
                else if (customerUser.UserPrincipalName != editUserModel.UserPrincipalName)
                {
                    customerUser.UserPrincipalName = editUserModel.UserPrincipalName;
                }

                await Context.PartnerOperations.Customers.ById(editUserModel.CustomerId)
                    .Users.ById(editUserModel.UserId).PatchAsync(customerUser);

                await ProcessLicenseModifications(editUserModel);

                UsersModel usersModel = new UsersModel()
                {
                    CustomerId = editUserModel.CustomerId,
                    Users = await Context.PartnerOperations.Customers.ById(editUserModel.CustomerId).Users.GetAsync()
                };

                return PartialView("ListUsers", usersModel);
            }
            finally
            {
                customerUser = null;
            }
        }

        public ActionResult ListUsers(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }

            UsersModel usersModel = new UsersModel()
            {
                CustomerId = customerId,
                Users = Context.PartnerOperations.Customers.ById(customerId).Users.Get()
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
                throw new ArgumentNullException("customerId");
            }
            else if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
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