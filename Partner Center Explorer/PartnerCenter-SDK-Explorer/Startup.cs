// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Owin;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer
{
    /// <summary>
    /// Manage the application startup.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}