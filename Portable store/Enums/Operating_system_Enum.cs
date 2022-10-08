using System.Text.Json.Serialization;

namespace Portable_store.Enums
{
    /// <summary>
    /// List of supported operating system by the store.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Operating_system_Enum
    {
        /// <summary>
        /// The operating system is Windows NT or later.
        /// </summary>
        Windows,
        /// <summary>
        /// The operating system is Linux.
        /// </summary>
        Linux,
        /// <summary>
        /// The operating system is FreeBSD.
        /// </summary>
        FreeBSD,
        /// <summary>
        /// The operating system is Macintosh.
        /// </summary>
        macOS
    }
}
