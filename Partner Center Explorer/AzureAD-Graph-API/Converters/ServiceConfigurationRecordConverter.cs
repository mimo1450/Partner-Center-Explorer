// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Samples.AzureAD.Graph.API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Microsoft.Samples.AzureAD.Graph.API.Converters
{
    /// <summary>
    /// <see cref="JsonConverter"/> for service configuration records returned from Azure AD Graph API.
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.JsonConverter" />
    public class ServiceConfigurationRecordConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(ServiceConfigurationRecord).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject json;
            ServiceConfigurationRecord record = null;
            string recordType;

            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            try
            {
                json = JObject.Load(reader);

                if (json.HasValues)
                {
                    recordType = json["recordType"].Value<string>();

                    if (recordType.Equals("cname", StringComparison.CurrentCultureIgnoreCase))
                    {
                        record = new DomainDnsCnameRecord()
                        {
                            CanonicalName = json["canonicalName"].Value<string>()
                        };
                    }
                    else if (recordType.Equals("mx", StringComparison.CurrentCultureIgnoreCase))
                    {
                        record = new DomainDnsMxRecord()
                        {
                            MailExchange = json["mailExchange"].Value<string>(),
                            Preference = json["preference"].Value<int>()
                        };
                    }
                    else if (recordType.Equals("srv", StringComparison.CurrentCultureIgnoreCase))
                    {
                        record = new DomainDnsSrvRecord()
                        {
                            NameTarget = json["nameTarget"].Value<string>(),
                            Port = json["port"].Value<int>(),
                            Priority = json["priority"].Value<int>(),
                            Protocol = json["protocol"].Value<string>(),
                            Service = json["service"].Value<string>(),
                            Weight = json["weight"].Value<int>()
                        };
                    }
                    else if (recordType.Equals("txt", StringComparison.CurrentCultureIgnoreCase))
                    {
                        record = new DomainDnsTxtRecord()
                        {
                            Text = json["text"].Value<string>()
                        };
                    }
                    else
                    {
                        record = new ServiceConfigurationRecord();
                    }

                    record.DnsRecordId = json["dnsRecordId"].Value<string>();
                    record.IsOptional = json["isOptional"].Value<bool>();
                    record.Label = json["label"].Value<string>();
                    record.RecordType = json["recordType"].Value<string>();
                    record.SupportedService = json["supportedService"].Value<string>();
                    record.Ttl = json["ttl"].Value<int>();
                }

                return record;
            }
            finally
            {
                json = null;
            }
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}