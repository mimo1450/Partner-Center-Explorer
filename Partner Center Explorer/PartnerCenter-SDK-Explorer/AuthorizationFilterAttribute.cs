// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Claims;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer
{
    /// <summary>
    /// Authorization filter attribute used to verify authenticated user has the speicifed claim and value.
    /// </summary>
    /// <seealso cref="Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.AuthorizeAttribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizationFilterAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Gets or sets the type of the claim.
        /// </summary>
        /// <value>
        /// The type of the claim.
        /// </value>
        public string ClaimType
        { get; set; }

        /// <summary>
        /// Gets or sets the claim value.
        /// </summary>
        /// <value>
        /// The claim value.
        /// </value>
        public string ClaimValue
        { get; set; }

        /// <summary>
        /// Called when a process requests authorization.
        /// </summary>
        /// <param name="filterContext">The filter context, which encapsulates information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />.</param>
        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            ClaimsPrincipal user = filterContext.HttpContext.User as ClaimsPrincipal;

            if (user != null && user.HasClaim(ClaimType, ClaimValue))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}