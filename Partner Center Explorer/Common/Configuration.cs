using System.Configuration;

namespace Microsoft.Store.PartnerCenter.Samples.Common
{
    /// <summary>
    /// Helper object used to quickly access settings defined in the web.config
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Gets the  service root for the Partner Center API.
        /// </summary>
        /// <value>
        /// The service root for the Parnter Center API.
        /// </value>
        public static string ApiServiceRoot
        {
            get { return ConfigurationManager.AppSettings["ApiServiceRoot"]; }
        }

        /// <summary>
        /// Gets the authority used for obtaining the application credential token.
        /// </summary>
        /// <value>
        /// The authority use for obtaining the application token.
        /// </value>
        public static string Authority
        {
            get { return ConfigurationManager.AppSettings["Authority"]; }
        }

        /// <summary>
        /// Gets the service root for the Azure AD Graph API.
        /// </summary>
        /// <value>
        /// The service root for the Azure AD Graph API.
        /// </value>
        public static string AzureADGraphAPIRoot
        {
            get { return ConfigurationManager.AppSettings["AzureADGraphAPIRoot"]; }
        }

        /// <summary>
        /// Gets the native Azure AD application identifier.
        /// </summary>
        /// <value>
        /// The application identifier used for authentication purposes.
        /// </value>
        public static string NativeApplicationId
        {
            get { return ConfigurationManager.AppSettings["NativeApplicationId"]; }
        }

        /// <summary>
        /// Gets the password used to connect to the Partner Center SDK.
        /// </summary>
        /// <value>
        /// The password used to connect to the Partner Center SDK.
        /// </value>
        public static string Password
        {
            get { return ConfigurationManager.AppSettings["Password"]; }
        }

        /// <summary>
        /// Gets the username used to connect to the Partner Center SDK.
        /// </summary>
        /// <value>
        /// The username use to connect to the Partner Center SDK.
        /// </value>
        public static string Username
        {
            get { return ConfigurationManager.AppSettings["Username"]; }
        }

        /// <summary>
        /// Gets the web Azure AD application identifier.
        /// </summary>
        /// <value>
        /// The web Azure AD application identifier used for authentication.
        /// </value>
        public static string WebApplicationId
        {
            get { return ConfigurationManager.AppSettings["WebApplicationId"]; }
        }

        /// <summary>
        /// Gets the web Azure AD application secret.
        /// </summary>
        /// <value>
        /// The web Azure AD application secret use for authentication.
        /// </value>
        public static string WebApplicationSecret
        {
            get { return ConfigurationManager.AppSettings["WebApplicationSecret"]; }
        }
    }
}