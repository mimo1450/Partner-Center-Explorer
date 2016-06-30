// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Samples.AzureAD.Graph.API.Models
{
    public class ServiceConfigurationRecord : IServiceConfigurationRecord
    {
        public string DnsRecordId
        { get; set; }

        public bool IsOptional
        { get; set; }

        public string Label
        { get; set; }

        public string RecordType
        { get; set; }

        public string SupportedService
        { get; set; }

        public int Ttl
        { get; set; }
    }
}