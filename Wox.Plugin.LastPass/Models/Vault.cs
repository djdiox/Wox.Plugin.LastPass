using Newtonsoft.Json;
using System.Collections.Generic;

namespace Wox.Plugin.OnePassword.Models
{// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Vault
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("content_version")]
        public string ContentVersion;

        public List<OnePasswordItem> Accounts { get; set; } = new List<OnePasswordItem>();
    }
}
