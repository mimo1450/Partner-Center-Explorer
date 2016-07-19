// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Configuration;
using Microsoft.Store.PartnerCenter.Samples.Common.Cache;

namespace Microsoft.Store.PartnerCenter.Samples.Common
{
    /// <summary>
    /// Helper object for access application specific configurations.
    /// </summary>
    public static class AppConfig
    {
        /// <summary>
        /// Gets the account identitifer value.
        /// </summary>
        /// <value>
        /// The account identifier value.
        /// </value>
        /// <remarks>This is the AccountId value found in Partner Center for the reseller tenant.</remarks>
        public static string AccountId => ConfigurationManager.AppSettings["AccountId"];

        /// <summary>
        /// Gets the application identifier value.
        /// </summary>
        /// <value>
        /// The application identifier value.
        /// </value>
        /// <remarks>This is the ClientId of the application being used for authentication purposes.</remarks>
        public static string ApplicationId => ConfigurationManager.AppSettings["ApplicationId"];

        /// <summary>
        /// Gets the application secret value.
        /// </summary>
        /// <value>
        /// The application secret value.
        /// </value>
        /// <remarks>This is the secret key for the application being used for authentication purposes.</remarks>
        public static string ApplicationSecret => ConfigurationManager.AppSettings["ApplicationSecret"];

        /// <summary>
        /// Gets the authentication authority.
        /// </summary>
        /// <value>
        /// The authentication authority value.
        /// </value>
        /// <remarks>
        /// This value should be https://login.microsoftonline.com or https://login.windows.net,
        /// and is used when obtaining a token.
        /// </remarks>
        public static string Authority => ConfigurationManager.AppSettings["Authority"];

        /// <summary>
        /// Gets the name of the Azure Automation account.
        /// </summary>
        /// <value>
        /// The name of the Azure Automation account.
        /// </value>
        public static string AutomationAccount => ConfigurationManager.AppSettings["Azure:AutomationAccount"];

        /// <summary>
        /// Gets password to be used when accessing the Azure Automation account.
        /// </summary>
        /// <value>
        /// The password to be used to accessing the Azure Automation account.
        /// </value>
        public static string AutomationPassword => ConfigurationManager.AppSettings["Azure:AutomationPassword"];

        /// <summary>
        /// Gets the resource group name that owns the Azure Automation account.
        /// </summary>
        /// <value>
        /// The resource group name that owns the Azure Automation account.
        /// </value>
        public static string AutomationResourceGroup
            => ConfigurationManager.AppSettings["Azure:AutomationResourceGroupName"];

        /// <summary>
        /// Gets the subscription identifier that owns the Azure Automation account.
        /// </summary>
        /// <value>
        /// The subscription identifier that owns the Azure Automation account.
        /// </value>
        public static string AutomationSubscriptionId
            => ConfigurationManager.AppSettings["Azure:AutomationSubscriptionId"];

        /// <summary>
        /// Gets the tenant identifier that owns the Azure Automation account.
        /// </summary>
        /// <value>
        /// The tenant identifier that owns the Azure Automation account.
        /// </value>
        public static string AutomationTenantId => ConfigurationManager.AppSettings["Azure:AutomationTenantId"];

        /// <summary>
        /// Gets the username to be used when accessing the Azure Automation account.
        /// </summary>
        /// <value>
        /// The user name to be used when accessing the Azure Automation account.
        /// </value>
        public static string AutomationUsername => ConfigurationManager.AppSettings["Azure:AutomationUsername"];

        /// <summary>
        /// Gets the country code value.
        /// </summary>
        /// <value>
        /// The country code value.
        /// </value>
        /// <remarks>
        /// The ISO-2 code value that represents the appropriate country. This value is used when creating customers
        /// and new offers. Currently this sample project does not support multiple countries. Additional information
        /// regarding this value can be found at https://msdn.microsoft.com/en-us/library/partnercenter/mt667939.aspx.
        /// </remarks>
        public static string CountryCode => ConfigurationManager.AppSettings["CountryCode"];

        /// <summary>
        /// Gets the Azure AD Graph API endpoint.
        /// </summary>
        /// <value>
        /// The Azure AD Graph API endpoint.
        /// </value>
        public static string GraphUri => ConfigurationManager.AppSettings["Azure:ADGraphUri"];

        /// <summary>
        /// Gets the AppInsights instrumentation key value.
        /// </summary>
        /// <value>
        /// The AppInsights instrumentation key value.
        /// </value>
        /// <remarks>
        /// This value is the instrumentation key used by AppInsights for tracking purposes. Additional information
        /// can be found at https://azure.microsoft.com/en-us/documentation/articles/app-insights-overview/.
        /// </remarks>
        public static string InstrumentationKey => ConfigurationManager.AppSettings["AppInsights:InstrumentationKey"];

        /// <summary>
        /// Gets a value indicating whether the account identifier is an integration sandbox environment.
        /// </summary>
        /// <value>
        /// <c>true</c> if the account identifier configured is an integration sandbox environment; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Deleting a customer is only supported for an integration sandbox environment. If this value is set to true then
        /// the ability to delete customers will be enabled; otherwise this ability will be disabled.
        /// </remarks>
        public static bool IsSandboxEnvironment => Convert.ToBoolean(ConfigurationManager.AppSettings["IsSandboxEnvironment"]);

        /// <summary>
        /// Gets the Azure Resource Manager API endpoint.
        /// </summary>
        /// <value>
        /// The Azure Resource Manager API endpoint.
        /// </value>
        /// <remarks>This value is used when connecting to the Azure Resource Manager API.</remarks>
        public static string ManagementUri => ConfigurationManager.AppSettings["Azure:ManagementUri"];

        /// <summary>
        /// Gets the Partner Center API endpoint.
        /// </summary>
        /// <value>
        /// The Partner Center API endpoint.
        /// </value>
        /// <remarks>THis value is used by all operations for he Parnter Center API.</remarks>
        public static string PartnerCenterApiUri => ConfigurationManager.AppSettings["PartnerCenterApiUri"];

        /// <summary>
        /// Gets the Redis Cache connection string.
        /// </summary>
        /// <value>
        /// The Redis Cache connection string value.
        /// </value>
        /// <remarks>
        /// This is an optional value. It is used by <see cref="DistributedTokenCache"/> in order to cache tokens.
        /// Obtaining tokens is an expensive operations. Using a presistent caching mechanism allows us to cache tokens
        /// and not worry about a system reboot impacting the performance of the application. Additional information
        /// can be found at https://azure.microsoft.com/en-us/documentation/articles/guidance-multitenant-identity-token-cache/.
        /// </remarks>
        public static string RedisConnection => ConfigurationManager.AppSettings["Redis:Connnection"];
    }
}