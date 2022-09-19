using Newtonsoft.Json;

namespace Wox.Plugin.OnePassword.Models
{// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Field
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("purpose")]
        public string Purpose { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("password_details")]
        public PasswordDetails PasswordDetails { get; set; }
    }
}
