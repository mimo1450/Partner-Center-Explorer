// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer
{
    /// <summary>
    /// Startup class for the application.
    /// </summary>
    public partial class Startup
    {
        private readonly string _authority = ConfigurationManager.AppSettings["Authority"] + "/common";
        private readonly string _clientId = ConfigurationManager.AppSettings["ApplicationId"];

        /// <summary>
        /// Configures the authentication for the application.
        /// </summary>
        /// <param name="app">The application to configure.</param>
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = _clientId,
                    Authority = _authority,
                    TokenValidationParameters = new TokenValidationParameters
                    {
                        RoleClaimType = "roles",
                        SaveSigninToken = true,
                        ValidateIssuer = false
                    },
                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        SecurityTokenValidated = context => Task.FromResult(0),
                        AuthenticationFailed = context =>
                        {
                            // Pass in the context back to the app
                            context.OwinContext.Response.Redirect("/Error/ShowError");
                            context.HandleResponse(); // Suppress the exception
                            return Task.FromResult(0);
                        }
                    }
                });
        }
    }
}