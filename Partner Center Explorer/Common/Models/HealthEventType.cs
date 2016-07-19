// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Store.PartnerCenter.Samples.Common.Models
{
    /// <summary>
    /// Enumeration description possible health event types.
    /// </summary>
    public enum HealthEventType
    {
        /// <summary>
        /// The health event is from Azure Insights.
        /// </summary>
        Azure,

        /// <summary>
        /// The health event is from Office 365 Service Communications API.
        /// </summary>
        Office
    }
}