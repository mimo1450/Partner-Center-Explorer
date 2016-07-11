// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Samples.AzureAD.Graph.API.Models
{
    public class Domain
    {
        public bool AdminManaged
        { get; set; }

        public string AuthenticationType
        { get; set; }

        public string AvailabilityStatus
        { get; set; }

        public bool IsDefault
        { get; set; }

        public bool IsInitial
        { get; set; }

        public bool IsRoot
        { get; set; }

        public bool IsVerified
        { get; set; }

        public string Name
        { get; set; }

        public List<string> SupportedServices
        { get; set; }
    }
}