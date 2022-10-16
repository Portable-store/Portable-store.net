using Portable_store.Enums;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Portable_store.Models
{
    [Serializable]
    public class Application_metadata_Model
    {
        #region Constructors
        public Application_metadata_Model(string name, string display_name, string icon_uri, Application_description descriptions, Source_type_Enum sourceType, Application_version_Model[] versions)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Display_name = display_name ?? name.Split('/').Last();
            Descriptions = descriptions ?? new Application_description();
            Icon_uri = icon_uri ?? "";
            Source_type = sourceType;
            Versions = versions ?? throw new ArgumentNullException(nameof(versions));
        }

        public Application_metadata_Model(string name, string display_name, Source_type_Enum sourceType, Application_version_Model[] versions) :
            this(display_name, name, "", new(), sourceType, versions) { }

        public Application_metadata_Model() : this("", "", "", new(), Source_type_Enum.DirectLink, Array.Empty<Application_version_Model>()) { }
        #endregion

        #region Properties
        /// <summary>
        /// The application name or repossitory name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The application display name
        /// </summary>
        [JsonPropertyName("Display name")]
        public string Display_name { get; set; }

        /// <summary>
        /// The application icon uri
        /// </summary>
        [JsonPropertyName("Icon uri")]
        public string Icon_uri { get; set; }

        /// <summary>
        /// The application description.
        /// If empty and the source is a repossitory the repossitory description can be use.
        /// </summary>
        public Application_description Descriptions { get; set; }

        /// <summary>
        /// The application source
        /// </summary>
        [JsonPropertyName("Source type")]
        public Source_type_Enum Source_type { get; set; }

        /// <summary>
        /// The applications versions
        /// </summary>
        public Application_version_Model[] Versions { get; set; }
        #endregion

        #region Methods
        public string To_JSON() =>
            JsonSerializer.Serialize(this, Metadata.JSON_option);

        public override string ToString() => Name;

        public string To_long_string()
        {
            var versions_string = new StringBuilder();

            foreach (var version in Versions)
                versions_string.AppendLine("\t" + version
                    .To_long_string()
                    .Replace(Environment.NewLine, Environment.NewLine + "\t") +
                    Environment.NewLine);

            // Remove the last new line
            versions_string.Remove(versions_string.Length -2, 2);

            return "Name: " + Name + Environment.NewLine +
                   "Display name: " + Display_name + Environment.NewLine +
                   "Icon uri: " + Icon_uri + Environment.NewLine +
                   "Descriptions: " + Descriptions.ToString() + Environment.NewLine +
                   "Source type: " + Source_type + Environment.NewLine +
                   "Versions : " + Environment.NewLine +
                       versions_string.ToString();
        }
        #endregion
    }
}
