// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Samples.AzureAD.Graph.API.Models
{
    /// <summary>
    /// Represents a Domain obtained from Azure AD Graph API.
    /// </summary>
    public class Domain
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Domain"/> is admin managed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this <see cref="Domain"/> is admin managed; otherwise, <c>false</c>.
        /// </value>
        public bool AdminManaged
        { get; set; }

        /// <summary>
        /// Gets or sets the type of the authentication.
        /// </summary>
        /// <value>
        /// The type of the authentication.
        /// </value>
        public string AuthenticationType
        { get; set; }

        /// <summary>
        /// Gets or sets the availability status.
        /// </summary>
        /// <value>
        /// The availability status.
        /// </value>
        public string AvailabilityStatus
        { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Domain"/> is default.
        /// </summary>
        /// <value>
        /// <c>true</c> if this <see cref="Domain"/> is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsDefault
        { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Domain"/> is initial.
        /// </summary>
        /// <value>
        /// <c>true</c> if this <see cref="Domain"/> is initial; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitial
        { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Domain"/> is the root domain.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this <see cref="Domain"/> is the root domain; otherwise, <c>false</c>.
        /// </value>
        public bool IsRoot
        { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Domain"/> has been verified.
        /// </summary>
        /// <value>
        /// <c>true</c> if this <see cref="Domain"/> has been verified; otherwise, <c>false</c>.
        /// </value>
        public bool IsVerified
        { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name of the domain.
        /// </value>
        public string Name
        { get; set; }

        /// <summary>
        /// Gets or sets the supported services.
        /// </summary>
        /// <value>
        /// Services supported by this domain.
        /// </value>
        /// <remarks>This is a list of Office 365 service supported by the domain.</remarks>
        public List<string> SupportedServices
        { get; set; }
    }
}