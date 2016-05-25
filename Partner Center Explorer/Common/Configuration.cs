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
        /// Gets the web Azure AD application identifier.
        /// </summary>
        /// <value>
        /// The web Azure AD application identifier used for authentication.
        /// </value>
        public static string ApplicationId
        {
            get { return ConfigurationManager.AppSettings["ApplicationId"]; }
        }

        /// <summary>
        /// Gets the web Azure AD application secret.
        /// </summary>
        /// <value>
        /// The web Azure AD application secret use for authentication.
        /// </value>
        public static string ApplicationSecret
        {
            get { return ConfigurationManager.AppSettings["ApplicationSecret"]; }
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
        /// Gets the URI root for the Azure Management API. 
        /// </summary>
        /// <value>
        /// The URI root for the Azure Management API.
        /// </value>
        public static string AzureManagementRoot
        {
            get { return ConfigurationManager.AppSettings["AzureManagementRoot"]; }
        }

        /// <summary>
        /// Gets the Office 365 management API root.
        /// </summary>
        /// <value>
        /// The Office 365 management API root.
        /// </value>
        public static string O365ManageAPIRoot
        {
            get { return ConfigurationManager.AppSettings["O365ManageAPIRoot"]; }
        }
    }
}