// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace Microsoft.Samples.AzureAD.Graph.API.Models
{
    public interface IServiceConfigurationRecord
    {
        string DnsRecordId
        { get; set; }

        bool IsOptional
        { get; set; }

        string Label
        { get; set; }

        string RecordType
        { get; set; }

        string SupportedService
        { get; set; }

        int Ttl
        { get; set; }
    }
}