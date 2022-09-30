using Portable_store.Enums;

namespace Portable_store.Models
{
    public class Application_version_Model
    {
        public Application_version_Model(string name, Processor_architecture_Enum architecture, Uri uri, string executable_path, Operating_system_Enum operating_system)
        {
            Name = name ?? "";
            Architecture = architecture;
            URI = uri ?? throw new ArgumentNullException(nameof(uri));
            Executable_path = executable_path ?? "";
            Operating_system = operating_system;
        }

        /// <summary>
        /// The application version name.
        /// If it is empty, it will be considered the default version
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// The application version architecture.
        /// </summary>
        public Processor_architecture_Enum Architecture { get; set; }

        /// <summary>
        /// The application download path if weblink.
        /// The application asset name if repository.
        /// </summary>
        public Uri URI { get; set; }
        /// <summary>
        /// The application executable path.
        /// If it is empty, and the downloaded file is executable, it will be chosen.
        /// </summary>
        public string Executable_path { get; set; } = "";
        /// <summary>
        /// The application operating system.
        /// By default is cross-platform.
        /// </summary>
        public Operating_system_Enum Operating_system { get; set; } = Operating_system_Enum.None;
    }
}