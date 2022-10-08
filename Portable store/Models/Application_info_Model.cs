using System.Text.Json;
using System.Text.Json.Serialization;

namespace Portable_store.Models
{
    public class Application_info_Model
    {
        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="executable_path"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Application_info_Model(string name, string display_name, string executable_path, string application_folder)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Display_name = display_name ?? throw new ArgumentNullException(nameof(display_name));
            Executable_path = executable_path ?? throw new ArgumentNullException(nameof(executable_path));
            Application_folder = executable_path ?? throw new ArgumentNullException(nameof(application_folder));
        }
        #endregion

        #region Properties
        /// <summary>
        /// The name of the application
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The display name of the application
        /// </summary>
        [JsonPropertyName("Display name")]
        public string Display_name { get; set; }

        /// <summary>
        /// The executable path of the application
        /// </summary>
        [JsonPropertyName("Executable path")]
        public string Executable_path { get; set; }

        /// <summary>
        /// The folder of the application
        /// </summary>
        [JsonIgnore]
        public string Application_folder { get; set; }
        #endregion

        #region Methods
        public string To_JSON() =>
            JsonSerializer.Serialize(this, Metadata.JSON_option);

        public override string ToString() => Name;
        #endregion
    }
}
