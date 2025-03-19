using Newtonsoft.Json;

namespace PRN231.Models.DTOs.Response
{
    public class ODataResponse<T>
    {
        [JsonProperty("@odata.context")]
        public string Metadata { get; set; }
        public T Value { get; set; }
    }
}
