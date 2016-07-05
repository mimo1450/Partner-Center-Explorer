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
    public partial class Startup
    {
        private string ClientId = ConfigurationManager.AppSettings["ApplicationId"];
        private string Authority = ConfigurationManager.AppSettings["Authority"] + "/common";

        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions { });

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = ClientId,
                    Authority = Authority,
                    TokenValidationParameters = new TokenValidationParameters
                    {
                        RoleClaimType = "roles",
                        SaveSigninToken = true,
                        ValidateIssuer = false,
                    },
                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {
                        SecurityTokenValidated = (context) =>
                        {
                            // If your authentication logic is based on users then add your logic here
                            return Task.FromResult(0);
                        },
                        AuthenticationFailed = (context) =>
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