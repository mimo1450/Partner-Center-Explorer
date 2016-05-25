using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Configuration;
using System.Threading.Tasks;
using System.IdentityModel.Tokens;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer
{
    public partial class Startup
    {
        private string ClientId = ConfigurationManager.AppSettings["ApplicationId"];
        private string Authority = ConfigurationManager.AppSettings["Authority"] + "/common";

        /// <summary>
        /// Configures the authentication.
        /// </summary>
        /// <param name="app">The application.</param>
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
                        SaveSigninToken = true,
                        // instead of using the default validation (validating against a single issuer value, as we do in line of business apps), 
                        // we inject our own multitenant validation logic
                        ValidateIssuer = false,
                        // If the app needs access to the entire organization, then add the logic
                        // of validating the Issuer here.
                        // IssuerValidator
                        // 
                        RoleClaimType = "roles"
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
                            context.OwinContext.Response.Redirect("/Home/Error");
                            context.HandleResponse(); // Suppress the exception
                            return Task.FromResult(0);
                        }
                    }
                });
        }
    }
}