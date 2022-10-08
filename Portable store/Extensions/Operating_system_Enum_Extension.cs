using Portable_store.Enums;
using System.Runtime.InteropServices;

namespace Portable_store.Extensions
{
    public static class Operating_system_Enum_Extension
    {
        /// <summary>
        /// Convert the Operating_system_Enum to OSPlatform
        /// </summary>
        /// <param name="os">operating system enum</param>
        /// <returns>Platform</returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static OSPlatform To_struct(this Operating_system_Enum os) =>
            os switch
            {
                Operating_system_Enum.Windows => OSPlatform.Windows,
                Operating_system_Enum.Linux => OSPlatform.Linux,
                Operating_system_Enum.FreeBSD => OSPlatform.FreeBSD,
                Operating_system_Enum.macOS => OSPlatform.OSX,
                _ => throw new PlatformNotSupportedException()
            };

        /// <summary>
        /// Convert the OSPlatform to Operating_system_Enum
        /// </summary>
        /// <param name="os">Platform</param>
        /// <returns>operating system enum</returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static Operating_system_Enum To_struct(this OSPlatform platform)
        {
            if (platform.Equals(OSPlatform.Windows))
                return Operating_system_Enum.Windows;
            else if (platform.Equals(OSPlatform.Linux))
                return Operating_system_Enum.Linux;
            else if (platform.Equals(OSPlatform.FreeBSD))
                return Operating_system_Enum.FreeBSD;
            else if (platform.Equals(OSPlatform.OSX))
                return Operating_system_Enum.macOS;
            else
                throw new PlatformNotSupportedException();
        }

    }
}
