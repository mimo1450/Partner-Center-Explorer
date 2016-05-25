using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Store.PartnerCenter.Samples.Common;
using System;
using System.Linq;
using System.Security.Claims;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context
{
    /// <summary>
    /// Used to perform basic token operations.
    /// </summary>
    public class TokenContext
    {
        /// <summary>
        /// Gets the token for the current autheticated user.
        /// </summary>
        /// <value>
        /// The token used authenticate the current user.
        /// </value>
        /// <remarks>This token is used to construct a user assertion in order to obtain tokens for specific resources.</remarks>
        public static string UserAssertionToken
        {
            get
            {
                System.IdentityModel.Tokens.BootstrapContext bootstrapContext;

                try
                {
                    bootstrapContext = ClaimsPrincipal.Current.Identities.First().BootstrapContext as System.IdentityModel.Tokens.BootstrapContext;

                    return bootstrapContext.Token;
                }
                finally
                {
                    bootstrapContext = null;
                }
            }
        }

        /// <summary>
        /// Obtain an Azure AD token for the specified resource.
        /// </summary>
        /// <param name="authority">Authority used to obtain the token.</param>
        /// <param name="resource">Identifier of the target resource that is the recipient of the requested token.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// authority
        /// or
        /// resource
        /// </exception>
        /// <remarks>This function obtains a token for the specified resource on behalf the current authenticated user.</remarks>
        public static AuthenticationResult GetAADToken(string authority, string resource)
        {
            AuthenticationContext authContext;

            if (string.IsNullOrEmpty(authority))
            {
                throw new ArgumentNullException("authority");
            }
            else if (string.IsNullOrEmpty(resource))
            {
                throw new ArgumentNullException("resource");
            }

            try
            {
                authContext = new AuthenticationContext(authority);

                return authContext.AcquireToken(
                    resource,
                    new ClientCredential(
                        Configuration.ApplicationId,
                        Configuration.ApplicationSecret
                    ),
                    new UserAssertion(UserAssertionToken, "urn:ietf:params:oauth:grant-type:jwt-bearer")
                );
            }
            finally
            {
                authContext = null;
            }
        }
    }
}