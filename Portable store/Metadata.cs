using Portable_store.Enums;
using Portable_store.Extensions;
using Portable_store.Models;
using System;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Portable_store
{
    public static class Metadata
    {
        internal readonly static JsonSerializerOptions JSON_option = new()
        {
            AllowTrailingCommas = true,
            WriteIndented = true,
        };

        internal static async Task<string?> Create_info_Async(Application_info_Model application_info, IProgress<Progress_info_Model> progress, CancellationToken cancellationToken = default)
        {
            string metadata_JSON = application_info.To_JSON();
            var progress_info = new Progress_info_Model(1)
            {
                Details = $"Creating {Path.GetFileNameWithoutExtension(application_info.Name)} information file..."
            };
            progress.Report(progress_info);

            try
            {
                string metadata_folder = Application_path.GetFullPath($"Output", Application_path.Data);
                string metadata_file = Path.Combine(metadata_folder, $"{application_info.Display_name} information {DateTime.Now:yyyy-dd-M--HH-mm-ss}.json");

                Directory.CreateDirectory(metadata_folder);
                await File.WriteAllTextAsync(metadata_file, metadata_JSON, cancellationToken);

                progress.Report(progress_info.Increment());

                return metadata_file;
            }
            catch (Exception ex)
            {
                progress.Report(progress_info.Update($"Failed to create the application info file: {ex.Message}.", 1));
            }

            return null;
        }

        /// <summary>
        /// Asynchronously create a new metadata file.
        /// </summary>
        /// <param name="metadata">The application metadata</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The path to the new metadata file</returns>
        public static async Task<string?> Create_Async(Application_Model metadata, IProgress<Progress_info_Model> progress, CancellationToken cancellationToken = default)
        {
            string metadata_JSON = metadata.To_JSON();
            var progress_info = new Progress_info_Model(1)
            {
                Details = $"Creating {Path.GetFileNameWithoutExtension(metadata.Name)} metadata file..."
            };
            progress.Report(progress_info);

            try
            {
                string metadata_folder = Application_path.GetFullPath($"Output", Application_path.Data);
                string metadata_file = Path.Combine(metadata_folder, $"{metadata.Display_name} {DateTime.Now:yyyy-dd-M--HH-mm-ss}.json");

                Directory.CreateDirectory(metadata_folder);
                await File.WriteAllTextAsync(metadata_file, metadata_JSON, cancellationToken);

                progress.Report(progress_info.Increment());

                return metadata_file;
            }
            catch (Exception ex)
            {
                progress.Report(progress_info.Update($"Failed to create the metadata file: {ex.Message}.", 1));
            }

            return null;
        }

        /// <summary>
        /// Read an metadata file
        /// </summary>
        /// <param name="App_name_OR_File_name"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static async Task<Application_Model?> Read_Async(string file_name, IProgress<Progress_info_Model> progress, CancellationToken cancellationToken = default)
        {
            var progress_info = new Progress_info_Model(1)
            {
                Details = $"Reading: {Path.GetFileNameWithoutExtension(file_name)}."
            };
            progress.Report(progress_info);

            if (File.Exists(file_name))
            {
                try
                {
                    var metadata_JSON = await File.ReadAllTextAsync(file_name, cancellationToken);
                    var metadata = JsonSerializer.Deserialize<Application_Model>(metadata_JSON, JSON_option);

                    if (metadata != null && string.IsNullOrEmpty(metadata.Display_name))
                        metadata.Display_name = Path.GetFileNameWithoutExtension(file_name);

                    progress.Report(progress_info.Increment());

                    return metadata;
                }
                catch (Exception ex)
                {
                    progress.Report(progress_info.Update($"Failed to read the metadata file: {ex.Message}.", 1));
                }
            }
            else
                progress.Report( progress_info.Update("Failed to read {file_name}: The metadata file don't exist.", 1) );


            return null;
        }

        public static IEnumerable<Application_version_Model> Get_compatible_version(in Application_Model metadata) =>
            metadata.Versions.Where(version =>
                Is_supported(version.Operating_system) &&
                Is_supported(version.Architecture) );

        /// <summary>
        /// If the given architecture is supported.
        /// </summary>
        /// <param name="architecture">Architecture</param>
        /// <returns>If supported return true, else false</returns>
        private static bool Is_supported(Architecture? architecture) =>
            // Native support
            architecture == null ||
            architecture == RuntimeInformation.OSArchitecture ||
            // Emulation
            (RuntimeInformation.OSArchitecture == Architecture.X64 && architecture == Architecture.X86) ||
            (RuntimeInformation.OSArchitecture == Architecture.Arm64 && architecture == Architecture.Arm);

        /// <summary>
        /// If the given operating system is supported.
        /// </summary>
        /// <param name="operating_system">Operating system</param>
        /// <returns>If supported return true, else false</returns>
        private static bool Is_supported(Operating_system_Enum? operating_system) =>
            operating_system == null ||
            RuntimeInformation.IsOSPlatform(operating_system.Value.To_struct());
    }
}
