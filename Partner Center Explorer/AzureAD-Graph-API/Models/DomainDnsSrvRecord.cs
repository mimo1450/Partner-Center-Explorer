// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace Microsoft.Samples.AzureAD.Graph.API.Models
{
    public class DomainDnsSrvRecord : ServiceConfigurationRecord, IServiceConfigurationRecord
    {
        public string NameTarget
        { get; set; }

        public int Port
        { get; set; }

        public int Priority
        { get; set; }

        public string Protocol
        { get; set; }

        public string Service
        { get; set; }

        public int Weight
        { get; set; }
    }
}