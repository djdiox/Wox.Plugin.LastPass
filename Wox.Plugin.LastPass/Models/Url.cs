using Newtonsoft.Json;

namespace Wox.Plugin.OnePassword.Models
{// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Url
    {
        [JsonProperty("label")]
        public string Label;

        [JsonProperty("primary")]
        public bool Primary;

        [JsonProperty("href")]
        public string Href;
    }
}
