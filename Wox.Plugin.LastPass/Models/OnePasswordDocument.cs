using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wox.Plugin.OnePassword.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class OnePasswordDocument
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("title")]
        public string Title;

        [JsonProperty("version")]
        public int Version;

        [JsonProperty("vault")]
        public Vault Vault;

        [JsonProperty("overview.ainfo")]
        public string OverviewAinfo;

        [JsonProperty("last_edited_by")]
        public string LastEditedBy;

        [JsonProperty("created_at")]
        public DateTime CreatedAt;

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt;
    }
}
