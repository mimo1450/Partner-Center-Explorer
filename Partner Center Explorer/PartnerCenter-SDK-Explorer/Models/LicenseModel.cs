// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model for licenses that can be assigned to users.
    /// </summary>
    public class LicenseModel
    {
        /// <summary>
        /// Gets or sets the consumed units.
        /// </summary>
        /// <value>
        /// The consumed units for the license.
        /// </value>
        public int ConsumedUnits
        { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier for the license.
        /// </value>
        public string Id
        { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this license is assigned.
        /// </summary>
        /// <value>
        /// <c>true</c> if this license is assigned; otherwise, <c>false</c>.
        /// </value>
        public bool IsAssigned
        { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name for the license.
        /// </value>
        /// <remarks>This value is the internal name for the product SKU (e.g. ENTERPRISE_PACK).</remarks>
        public string Name
        { get; set; }

        /// <summary>
        /// Gets or sets the SKU part number.
        /// </summary>
        /// <value>
        /// The SKU part number for the license.
        /// </value>
        public string SkuPartNumber
        { get; set; }

        /// <summary>
        /// Gets or sets the type of the target.
        /// </summary>
        /// <value>
        /// The type of the target for the license.
        /// </value>
        public string TargetType
        { get; set; }

        /// <summary>
        /// Gets or sets the total units that can be allocated.
        /// </summary>
        /// <value>
        /// The total units number of units that can be allocated.
        /// </value>
        public int TotalUnits
        { get; set; }
    }
}