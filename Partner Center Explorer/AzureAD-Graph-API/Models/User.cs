// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Samples.AzureAD.Graph.API.Models
{
    public class User
    {
        public List<AssignedLicense> AssignedLicenses
        { get; set; }

        public List<AssignedPlan> AssignedPlans
        { get; set; }

        public List<SubscribedSku> AvailableSkus
        { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city where the user is located.
        /// </value>
        public string City
        { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country where the user is located.
        /// </value>
        public string Country
        { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if this users is directory synchronized. .
        /// </summary>
        /// <value>
        /// <c>True</c> if the user is synchronized with a local directory; otherwise <c>false</c>.
        /// </value>
        public bool? DirSyncEnabled
        { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name for the user.
        /// </value>
        public string DisplayName
        { get; set; }

        /// <summary>
        /// Gets or sets the facsimile telephone number.
        /// </summary>
        /// <value>
        /// The facsimile telephone number for the user.
        /// </value>
        public string FacsimileTelephoneNumber
        { get; set; }

        /// <summary>
        /// Gets or sets the name of the given.
        /// </summary>
        /// <value>
        /// The given name of the user.
        /// </value>
        public string GivenName
        { get; set; }

        /// <summary>
        /// Gets or sets the immutable identifier.
        /// </summary>
        /// <value>
        /// The immutable identifier assigned to the user.
        /// </value>
        public string ImmutableId
        { get; set; }

        /// <summary>
        /// Gets or sets the job title.
        /// </summary>
        /// <value>
        /// The job title.
        /// </value>
        public string JobTitle
        { get; set; }

        /// <summary>
        /// Gets or sets the last directory synchronization time.
        /// </summary>
        /// <value>
        /// The last time the user was synchronized with a local directory.
        /// </value>
        /// <remarks>
        /// This value will be null if the user is not synchronized with a local directory.
        /// </remarks>
        public DateTime? LastDirSyncTime
        { get; set; }

        /// <summary>
        /// Gets or sets the primary SMTP address.
        /// </summary>
        /// <value>
        /// The primary SMTP address assigned to the user.
        /// </value>
        public string Mail
        { get; set; }

        /// <summary>
        /// Gets or sets the email address alias.
        /// </summary>
        /// <value>
        /// The email address alias assigned to the user.
        /// </value>
        public string MailNickName
        { get; set; }

        /// <summary>
        /// Gets or sets the mobile telephone number.
        /// </summary>
        /// <value>
        /// The mobile telephone number assigned to the user.
        /// </value>
        public string Mobile
        { get; set; }

        public Guid ObjectId
        { get; set; }

        /// <summary>
        /// Gets or sets a collection of email addresses.
        /// </summary>
        /// <value>
        /// A collection of email addressed assigned to the user.
        /// </value>
        public string[] OtherMails
        { get; set; }

        /// <summary>
        /// Gets or sets the name of the physical delivery office.
        /// </summary>
        /// <value>
        /// The name of the physical delivery office assigned to the user.
        /// </value>
        public string PhysicalDeliveryOfficeName
        { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>
        /// The postal code where the user is located.
        /// </value>
        public string PostalCode
        { get; set; }

        /// <summary>
        /// Gets or sets the preferred language.
        /// </summary>
        /// <value>
        /// The preferred language for the user.
        /// </value>
        public string PreferredLanguage
        { get; set; }

        /// <summary>
        /// Gets or sets the SIP proxy address.
        /// </summary>
        /// <value>
        /// The SIP proxy address assigned to the user.
        /// </value>
        public string SipProxyAddress
        { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state where the user is located.
        /// </value>
        public string State
        { get; set; }

        /// <summary>
        /// Gets or sets the street address.
        /// </summary>
        /// <value>
        /// The street address where the user is located.
        /// </value>
        public string StreetAddress
        { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname for the user.
        /// </value>
        public string Surname
        { get; set; }

        /// <summary>
        /// Gets or sets the telephone number.
        /// </summary>
        /// <value>
        /// The telephone number assigned to the user.
        /// </value>
        public string TelephoneNumber
        { get; set; }

        /// <summary>
        /// Gets or sets the usage location.
        /// </summary>
        /// <value>
        /// The usage location for the user.
        /// </value>
        public string UsageLocation
        { get; set; }

        /// <summary>
        /// Gets or sets the name of the user principal.
        /// </summary>
        /// <value>
        /// The name of the user principal assigned to the user.
        /// </value>
        public string UserPrincipalName
        { get; set; }
    }
}