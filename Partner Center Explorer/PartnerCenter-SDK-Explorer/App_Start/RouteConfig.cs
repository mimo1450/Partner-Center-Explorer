// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Web.Mvc;
using System.Web.Routing;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Customers",
                url: "Customers/{customerId}",
                defaults: new { controller = "Customers", action = "Show" }
            );

            routes.MapRoute(
                name: "Invoices",
                url: "Invoices/{invoiceId}/{action}",
                defaults: new { controller = "Invoices", action = "Details" }
            );

            routes.MapRoute(
                name: "InvoiceDetails",
                url: "Invoices/{invoiceId}/{customerName}/{action}",
                defaults: new { controller = "Invoices" }
            );

            routes.MapRoute(
                name: "SubscriptionHealth",
                url: "Customers/{customerId}/Subscription/{subscriptionId}/Health",
                defaults: new { controller = "Subscriptions", action = "Health" }
            );

            routes.MapRoute(
                name: "SubscriptionManage",
                url: "Customers/{customerId}/Subscription/{subscriptionId}/Manage",
                defaults: new { controller = "Subscriptions", action = "Manage" }
            );

            routes.MapRoute(
                name: "SubscriptionManageResource",
                url: "Customers/{customerId}/Subscription/{subscriptionId}/Manage/{action}",
                defaults: new { controller = "Subscriptions" }
            );

            routes.MapRoute(
                name: "Usage",
                url: "Customers/{customerId}/Subscription/{subscriptionId}/Usage",
                defaults: new { controller = "Usage", action = "ViewUsage" }
            );

            routes.MapRoute(
                name: "Users",
                url: "Customers/{customerId}/users/{userId}/{action}",
                defaults: new { controller = "Users" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}