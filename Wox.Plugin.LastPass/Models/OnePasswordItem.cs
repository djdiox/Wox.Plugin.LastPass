using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wox.Plugin.OnePassword.Models
{// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
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
    }

    public class Url
    {
        [JsonProperty("label")]
        public string Label;

        [JsonProperty("primary")]
        public bool Primary;

        [JsonProperty("href")]
        public string Href;
    }

    public class Vault
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("content_version")]
        public string ContentVersion;

        public List<OnePasswordItem> Accounts { get; internal set; } = new List<OnePasswordItem>();

        internal static Vault Open(string text1, string text2, ClientInfo clientInfo, Authenticate.TwoFactorUI twoFactorUI)
        {
            throw new NotImplementedException();
        }
    }
}
