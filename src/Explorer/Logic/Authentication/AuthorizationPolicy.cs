// -----------------------------------------------------------------------
// <copyright file="AuthorizationPolicy.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Explorer.Logic.Authentication
{
    public class AuthorizationPolicy
    {
        public bool IsAuthorized(CustomerPrincipal principal, UserRole requiredUserRole)
        {
            principal.AssertNotNull(nameof(principal));

            bool isAuthorized;

            switch (requiredUserRole)
            {
                case UserRole.Customer:
                    isAuthorized = principal.IsCustomer && !principal.IsAdmin;
                    break;
                case UserRole.Partner:
                    isAuthorized = !principal.IsCustomer && principal.IsAdmin;
                    break;
                case UserRole.Any:
                    isAuthorized = principal.IsCustomer || principal.IsAdmin;
                    break;
                case UserRole.None:
                    isAuthorized = !principal.IsCustomer && !principal.IsAdmin;
                    break;
                default:
                    isAuthorized = false;
                    break;
            }

            return isAuthorized;
        }
    }
}