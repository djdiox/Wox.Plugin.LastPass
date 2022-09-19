using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wox.Plugin.OnePassword.Models
{
    // OnePasswordItem myDeserializedClass = JsonConvert.DeserializeObject<OnePasswordItem>(data);
    public class OnePasswordItem
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("title")]
        public string Title;

        [JsonProperty("tags")]
        public List<string> Tags;

        [JsonProperty("version")]
        public int Version;

        [JsonProperty("vault")]
        public Vault Vault;

        [JsonProperty("category")]
        public string Category;

        [JsonProperty("last_edited_by")]
        public string LastEditedBy;

        [JsonProperty("created_at")]
        public DateTime CreatedAt;

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt;

        [JsonProperty("additional_information")]
        public string AdditionalInformation;

        [JsonProperty("urls")]
        public List<Url> Urls;

        [JsonProperty("fields")]
        public List<Field> Fields = new List<Field>();
        [JsonProperty("sections")]
        public List<Section> Sections { get; set; }

        public string GetPassword()
        {
            Field res = Fields.Find(e => e.Label == "password");
            var val = res != null ? res.Value : "";
            return val;
        }
    }
}
