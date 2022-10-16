using System.Text.Json.Serialization;

namespace Portable_store.Models
{
    [Serializable]
    public class Application_description
    {
        #region Constructors
        public Application_description() : this(null) { }

        public Application_description(
            string? short_description = null, string? long_description = null, string? url = null, string[]? keywords = null)
        {
            Short_description = short_description ?? string.Empty;
            Long_description = long_description ?? string.Empty;
            Website = url ?? string.Empty;
            Keywords = keywords ?? Array.Empty<string>();
        }
        #endregion

        #region Properties
        [JsonPropertyName("Short description")]
        public string Short_description { get; set; }
        [JsonPropertyName("Long description")]
        public string Long_description { get; set; }
        public string Website { get; set; }
        public string[] Keywords { get; set; }
        #endregion

        #region Methods
        public override string ToString() => string.IsNullOrEmpty(Short_description) ?
            Long_description.Split('.').FirstOrDefault(string.Empty) :
            Short_description;
        #endregion
    }
}
