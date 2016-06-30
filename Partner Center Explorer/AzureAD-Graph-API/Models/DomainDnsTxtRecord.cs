// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace Microsoft.Samples.AzureAD.Graph.API.Models
{
    public class DomainDnsTxtRecord : ServiceConfigurationRecord, IServiceConfigurationRecord
    {
        public string Text
        { get; set; }
    }
}
