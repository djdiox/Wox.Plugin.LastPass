using Newtonsoft.Json;
using System.Collections.Generic;

namespace Wox.Plugin.OnePassword.Models
{// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class PasswordDetails
    {
        [JsonProperty("strength")]
        public string Strength { get; set; }

        [JsonProperty("history")]
        public List<string> History { get; set; }
    }
}
