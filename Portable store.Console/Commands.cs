using Portable_store.Models;
using System.Xml.Linq;

namespace Portable_store.Console
{
    internal static class Commands
    {
        /// <summary>
        /// Download the given application(s)
        /// </summary>
        /// <param name="names"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        internal static async Task Download(IEnumerable<string> names, IProgress<Progress_info_Model> progress)
        {
            if (Application_options.Verbose)
                ConsoleHelper.WriteLine("Downloading: " + string.Join(',', names));

            int success = 0;

            foreach (var name in names)
            {
                var applications_metadata = await Store.Search_Async(name, progress);

                if (applications_metadata.Count == 0)
                    ConsoleHelper.WriteLine(name + " was not found.");
                else
                    foreach (var application_metadata in applications_metadata)
                    {
                        var version = Metadata.Get_compatible_versions(application_metadata).First();

                        success += await Store.Download_Async(application_metadata, version, progress) ? 1 : 0;
                    }
            }

            if (success > 0)
                ConsoleHelper.WriteLine($"Download{(success > 1 ? 's' : string.Empty)} finished!");
        }

        /// <summary>
        /// Delete the given application(s)
        /// </summary>
        /// <param name="names"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        internal static async Task Delete(IEnumerable<string> names, IProgress<Progress_info_Model> progress)
        {
            if (Application_options.Verbose)
                ConsoleHelper.WriteLine("Deleting: " + string.Join(',', names));

            int success = 0;

            foreach (var name in names)
            {
                var applications_info = await Library.List_Async(name, progress);

                if (applications_info.Count == 0)
                    ConsoleHelper.WriteLine(name + " was not found");
                else
                    foreach (var application_info in applications_info)
                        success += await Library.Delete_Async(application_info, progress) ? 1 : 0;
            }


            if (success > 0)
                ConsoleHelper.WriteLine($"Deletion{(success > 1 ? 's' : string.Empty)} finished!");
        }

        /// <summary>
        /// Search the given application(s)
        /// </summary>
        /// <param name="names"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        internal static async Task Search(IEnumerable<string> names, IProgress<Progress_info_Model> progress)
        {
            if (Application_options.Verbose)
                ConsoleHelper.WriteLine("Searching: " + string.Join(',', names));

            foreach (var name in names)
            {
                var results = await Store.Search_Async(name, progress);

                if (results.Count == 0)
                    ConsoleHelper.WriteLine(name + " was not found");
                else
                    foreach (var result in results)
                    {
                        ConsoleHelper.WriteLine(result);
                    }
            }
        }

        /// <summary>
        /// List downloaded application
        /// </summary>
        /// <param name="names"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        internal static async Task List(IEnumerable<string> names, IProgress<Progress_info_Model> progress)
        {
            if (Application_options.Verbose)
                ConsoleHelper.WriteLine("Listing: " + string.Join(',', names));

            foreach (var name in names)
            {
                var results = await Library.List_Async(name, progress);

                if (results.Count == 0)
                    ConsoleHelper.WriteLine(name + " was not found");
                else
                    foreach (var result in results)
                    {
                        ConsoleHelper.WriteLine(result);
                    }
            }
        }

        /// <summary>
        /// Update the given applciation(s)
        /// </summary>
        /// <param name="names"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        internal static async Task Update(IEnumerable<string> names, IProgress<Progress_info_Model> progress)
        {
            if (Application_options.Verbose)
                ConsoleHelper.WriteLine("Updating: " + string.Join(',', names));

            int success = 0;

            // TODO
            foreach (var name in names)
                success += await Store.Update_Async(name, progress) ? 1 : 0;

            if (success > 0)
                ConsoleHelper.WriteLine($"Updating{(success > 1 ? 's' : string.Empty)} finished!");
        }

        /// <summary>
        /// Refresh the application metadatas cache
        /// </summary>
        /// <param name="names"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        internal static async Task Refresh(IEnumerable<string> names, IProgress<Progress_info_Model> progress)
        {
            ConsoleHelper.WriteLine("Refreshing cache...");

            ConsoleHelper.WriteLine(await Store.Refresh_Async() ?
                "Cache refreshed!" :
                "An error as ocurred!");
        }

        /// <summary>
        /// Start an application
        /// </summary>
        /// <param name="names"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        internal static async Task Run(IEnumerable<string> names, IProgress<Progress_info_Model> progress)
        {
            if (Application_options.Verbose)
                ConsoleHelper.WriteLine("Starting: " + string.Join(',', names));

            int success = 0;

            foreach (var name in names)
            {
                var applications_info = await Library.List_Async(name, progress);

                if (applications_info.Count == 0)
                    ConsoleHelper.WriteLine(name + " was not found");
                else
                    foreach (var application_info in applications_info)
                        success += Library.Run(application_info, progress) ? 1 : 0;
            }


            if (success > 0)
                ConsoleHelper.WriteLine($"Application{(success > 1 ? 's' : string.Empty)} started.");
        }

        /// <summary>
        /// Create a new application metadata
        /// </summary>
        /// <param name="names"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        internal static async Task Create(IEnumerable<string> names, IProgress<Progress_info_Model> progress)
        {
            if (Application_options.Verbose)
                ConsoleHelper.WriteLine("Starting the creation of: " + string.Join(',', names));

            foreach (var name in names)
            {
                var new_application_metadata = new Application_metadata_Model
                {
                    Name = ConsoleHelper.Ask("Name"),
                    Display_name = name,
                    Icon_uri = ConsoleHelper.OptionalAsk("Icon uri") ?? string.Empty,
                    Description = ConsoleHelper.OptionalAsk("Description") ?? string.Empty
                };
                //application_metadata.Source_type = ConsoleHelper.Ask("Name");

                var number_of_version_to_add = ConsoleHelper.Ask_number("Number of version to add");
                var versions = new List<Application_version_Model>(number_of_version_to_add);

                for (int i = 0; i < number_of_version_to_add; i++)
                {
                    versions[i] = new(
                        ConsoleHelper.OptionalAsk("Name") ?? string.Empty,
                        System.Runtime.InteropServices.Architecture.X86,
                        ConsoleHelper.Ask("URI"),
                        ConsoleHelper.OptionalAsk("URI") ?? string.Empty,
                        Enums.Operating_system_Enum.FreeBSD
                    );
                }

                var path = await Metadata.Create_Async(new_application_metadata, progress);

                ConsoleHelper.WriteLine(path != null ?
                    new_application_metadata.Name + " metadata created at: " + path :
                    $"Fail to create {new_application_metadata.Name} metadata file.");
            }
        }

        /// <summary>
        /// Read an application metadata
        /// </summary>
        /// <param name="names"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        internal static async Task Read(IEnumerable<string> files, IProgress<Progress_info_Model> progress)
        {
            if (Application_options.Verbose)
                ConsoleHelper.WriteLine("Readings: " + string.Join(',', files));

            foreach (var file in files)
            {
                if (!File.Exists(file))
                    ConsoleHelper.WriteLine(file + " doesn't exist.");
                else
                {
                    var metadata = await Metadata.Read_Async(file, progress);

                    if (metadata == null)
                        ConsoleHelper.WriteLine("An error as occured when reading " + file);
                    else
                        ConsoleHelper.WriteLine(metadata.To_long_string());
                }
            }
        }
    }
}
