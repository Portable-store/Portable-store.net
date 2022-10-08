using System.Text.Json.Serialization;

namespace Portable_store.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Architecture
    {
        All,
        AMD64,
        ARM,
        ARM64,
        IA64,
        MSIL,
        x86
    }
}
