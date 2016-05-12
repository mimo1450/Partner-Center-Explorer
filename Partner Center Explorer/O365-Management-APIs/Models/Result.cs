using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.Samples.Office365.Management.API.Models
{
    internal class Result<T>
    {
        [JsonProperty("@odata.context")]
        public string ODataContext
        { get; set; }

        public List<T> Value
        { get; set; }
    }
}
