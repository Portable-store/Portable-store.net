using Portable_store.Converters;
using Portable_store.Enums;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Portable_store.Models
{
    [Serializable]
    public class Application_version_Model
    {
        #region Constructors
        public Application_version_Model(string name, Architecture? architecture, string uri, string executable_path, Operating_system_Enum? operating_system)
        {
            Name = name ?? "";
            Architecture = architecture;
            URI = uri ?? throw new ArgumentNullException(nameof(uri));
            Executable_path = executable_path ?? "";
            Operating_system = operating_system;
        }
        public Application_version_Model(Architecture? architecture, string uri, Operating_system_Enum? operating_system) :
            this("", architecture, uri, "", operating_system) { }

        public Application_version_Model() : this("", null, "", "", null) { }
        #endregion

        #region Properties
        /// <summary>
        /// The application version name.
        /// If it is empty, it will be considered the default version
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// The application version architecture.
        /// </summary>
        [JsonConverter(typeof(JsonStringNullableEnumConverter<Architecture?>))]
        public Architecture? Architecture { get; set; }

        /// <summary>
        /// The application download path if weblink.
        /// The application asset name if repository.
        /// </summary>
        public string URI { get; set; }

        /// <summary>
        /// The application executable path.
        /// If it is empty, and the downloaded file is executable, it will be chosen.
        /// </summary>
        [JsonPropertyName("Executable path")]
        public string Executable_path { get; set; } = "";

        /// <summary>
        /// The application operating system.
        /// By default is cross-platform.
        /// </summary>
        [JsonPropertyName("Operating system")]
        [JsonConverter(typeof(JsonStringNullableEnumConverter<Operating_system_Enum?>))]
        public Operating_system_Enum? Operating_system { get; set; } = null;
        #endregion

        #region Methods
        public override string ToString() => string.IsNullOrEmpty(Name) ? "Default" : Name;

        public string To_long_string() =>
            "Name: " + Name + Environment.NewLine +
            "Architecture: " + Architecture.ToString() + Environment.NewLine +
            "URI: " + URI + Environment.NewLine +
            "Executable path: " + Executable_path + Environment.NewLine +
            "Operating system: " + Operating_system;
        #endregion
    }
}