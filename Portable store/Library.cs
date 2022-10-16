using Octokit;
using Portable_store.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Portable_store
{
    public static class Library
    {
        public static string Applications_folder = Application_path.GetFullPath("Applications", Application_path.Data);
        public static string ApplicationFiles_folder =
            Application_path.GetFullPath($"Applications{Path.DirectorySeparatorChar}Application files", Application_path.Data);

        internal static async Task<bool> Install_Async(Application_metadata_Model metadata, string download_file, IProgress<Progress_info_Model>? progress = null, CancellationToken cancellationToken = default)
        {
            Folders_integrety_check();

            var progress_info = new Progress_info_Model(1, 0, $"Installing {metadata.Name}...");
            progress?.Report(progress_info);

            if ((await List_Async(metadata.Name, progress)).Count() > 0)
            {
                progress_info.Update($"{metadata.Name} is already installed, update or delete it.", 0);
                progress?.Report(progress_info);
                return false;
            }

            var application_folder = Path.Combine(ApplicationFiles_folder, metadata.Name);

            Directory.CreateDirectory(application_folder); // Given app

            switch (Path.GetExtension(download_file))
            {
                case ".zip":
                    break;

                case ".exe":
                    var executable = Path.Combine(application_folder, Path.GetFileName(download_file));

                    // Create application information file
                    var application_info = await Metadata.Create_info_Async(
                        new(metadata.Name, metadata.Display_name, executable, application_folder),
                        progress, cancellationToken);

                    if (application_info != null)
                        File.Copy(application_info, Path.Combine(application_folder, "Application.json"));
                    else
                    {
                        progress_info.Update($"Can't create {metadata.Name} information file!", 0);
                        progress?.Report(progress_info);
                        return false;
                    }

                    // Copy application files
                    File.Copy(download_file, executable);

                    // Create application shortcut
                    Create_application_shortcut(metadata, executable);
                    break;
            }

            progress_info.Increment();
            progress?.Report(progress_info);
            return true;
        }

        public static async Task<bool> Delete_Async(Application_info_Model application_info, IProgress<Progress_info_Model> progress)
        {
            Folders_integrety_check();

            var progress_info = new Progress_info_Model(1, 0, $"Deleting {application_info.Name}...");
            var application_shortcut = Path.Combine(Applications_folder, application_info.Display_name + ".lnk");

            progress.Report(progress_info);

            try
            {
                // Dunno if that help somethings
                await Task.Run(() =>
                {
                    // Delete application shortcut
                    if (File.Exists(application_shortcut))
                        File.Delete(application_shortcut);

                    // Delete application files
                    var application_folder = new DirectoryInfo(application_info.Application_folder);

                    if (application_folder.Exists)
                    {
                        application_folder.Delete(true);
                        progress.Report(progress_info.Increment($"{application_info.Name} has been deleted!"));
                    }
                    else
                        progress.Report(progress_info.Increment($"{application_info.Name} has already been deleted!"));

                    // Delete parent folder if not empty
                    application_folder.Parent?.Delete(false);
                });
            }
            catch (Exception ex)
            {
                progress.Report(progress_info.Update($"Failed to delete {application_info.Name}: {ex.Message}.", 1));
            }

            return true;
        }

        public static async Task<IEnumerable<Application_info_Model>> List_Async(string application_name, IProgress<Progress_info_Model>? progress = null, CancellationToken cancellation_token = default)
        {
            Folders_integrety_check();

            var applications_folder = Directory.GetDirectories(ApplicationFiles_folder);
            var applications_info = new List<Application_info_Model>(applications_folder.Length);
            var progress_info = new Progress_info_Model(applications_folder.Length);

            foreach (var author_folder in applications_folder)
            {
                var sub_applications_folder = Directory.GetDirectories(author_folder);

                foreach (var application_folder in sub_applications_folder)
                {
                    string application_info_file = Path.Combine(application_folder, "Application.json");

                    if (File.Exists(application_info_file))
                    {
                        try
                        {
                            var application_info_JSON = await File.ReadAllTextAsync(application_info_file, cancellation_token);
                            var application_info = JsonSerializer.Deserialize<Application_info_Model>(application_info_JSON, Metadata.JSON_option);

                            if (application_info != null)
                            {
                                application_info.Application_folder = application_folder;
                                if (application_info.Name.ToLower().Contains(application_name.ToLower()))
                                {
                                    applications_info.Add(application_info);

                                    progress_info.Increment(application_info.Name + " found.");
                                }
                                else
                                    progress_info.Increment();
                                progress?.Report(progress_info);
                            }
                        }
                        catch (Exception ex)
                        {
                            progress_info.Increment($"Failed to read the application info of {Path.GetFileName(application_folder)}: {ex.Message}.");
                            progress?.Report(progress_info);
                        }
                    }
                    else
                    {
                        progress_info.Increment($"The application info of {Path.GetFileName(application_folder)} could not be found, reinstall it");
                        progress?.Report(progress_info);
                    }
                }
            }

            return applications_info;
        }

        internal static string Get_install_info_path(string application_name)
        {
            var application_folder = Path.Combine(ApplicationFiles_folder, application_name);
            string application_info_file = Path.Combine(application_folder, "Application.json");

            return File.Exists(application_info_file) ? application_info_file : string.Empty;
        }

        public static bool Run(Application_info_Model application_info, IProgress<Progress_info_Model>? progress = null)
        {
            var progress_info = new Progress_info_Model(0,0, $"Starting {application_info.Display_name}...");
            progress?.Report(progress_info);

            try
            {
                var process_info = new ProcessStartInfo()
                {
                    FileName = application_info.Executable_path,
                    //WorkingDirectory = application_info.Application_folder,
                    UseShellExecute = true,
                };

                Process.Start(process_info);

                progress?.Report(progress_info.Increment());

                return true;
            }
            catch (Exception ex)
            {
                progress?.Report(progress_info.Update($"Failed start {application_info.Name}: {ex.Message}.", 0));
            }

            return false;
        }

        public static void Create_application_shortcut(in Application_metadata_Model metadata, string executable_path, string? shortcut_path = null)
        {
            Folders_integrety_check();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                shortcut_path ??= Path.Combine(Applications_folder, metadata.Display_name + ".lnk");
                var shell = new IWshRuntimeLibrary.WshShell();
                var shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcut_path);

                shortcut.Description = metadata.Descriptions.ToString();
                shortcut.TargetPath = executable_path;
                shortcut.IconLocation = shortcut.TargetPath;
                shortcut.Save();
            }
            else
                throw new NotImplementedException("Shortcut not implemented yet on this platform");

        }

        private static void Folders_integrety_check()
        {
            Directory.CreateDirectory(Applications_folder);
            Directory.CreateDirectory(ApplicationFiles_folder);
        }
    }
}
