using Portable_store.Enums;

namespace Portable_store.Models
{
    public class Application_Model
    {
        /// <summary>
        /// The application name or repossitory name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The application description.
        /// If empty and the source is a repossitory the repossitory description can be use.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The application source
        /// </summary>
        public Source_type_Enum SourceType { get; set; }

        /// <summary>
        /// The applications versions
        /// </summary>
        public Application_version_Model[] Versions;
    }
}
