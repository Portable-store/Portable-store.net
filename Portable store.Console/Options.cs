using System.Diagnostics;
using System.Reflection;

namespace Portable_store.Console
{
    internal class Options
    {
        internal static void ShowHelp()
        {
            ConsoleHelper.WriteLine(
                $"{Assembly.GetExecutingAssembly().GetName().Name} [options] command" + Environment.NewLine +
                Environment.NewLine +
                "Commands:" + Environment.NewLine +
                "refresh: Refresh the application metadatas cache" + Environment.NewLine +
                "search [App name]: Search the given application(s)" + Environment.NewLine +
                "download [App name]: Download the given application(s)" + Environment.NewLine +
                "update [App name]: Update the given applciation(s)" + Environment.NewLine +
                Environment.NewLine +
                "list : List downloaded application" + Environment.NewLine +
                "delete [App name]: Delete the given application(s)" + Environment.NewLine +
                "run [App name]: Start an application" + Environment.NewLine +
                Environment.NewLine +
                "create [metadata file name]: Create a new application metadata" + Environment.NewLine +
                "read [App name/metadata file]: Read an application metadata" + Environment.NewLine +
                Environment.NewLine +
                "Options:" + Environment.NewLine +
                "-q, --quiet: Produces output suitable for logging, omitting progress indicators." + Environment.NewLine +
                "-y, --yes: Automatic yes to prompts. Assume \"yes\" as answer to all prompts and run non-interactively." + Environment.NewLine +
                "-V, --verbose: Show full versions logs for upgraded and installed packages." + Environment.NewLine +
                "-C, --clean: Do a clean download, this might delete all user settings or unwanted file, inside the destination application folder." + Environment.NewLine +
                "-h, --help: Show help dialog" + Environment.NewLine +
                "-v, --version: Show the application version." + Environment.NewLine +
                "-c, --config-file: Specify a configuration file to use."
            );
        }

        internal static void ShowVersion()
        {
            ConsoleHelper.WriteLine(
                "Portable store by Tom OLIVIER" + Environment.NewLine +
                "Version " + GetAssemblyFileVersion() + Environment.NewLine +
                "https://github.com/Tom60chat/Portable-store" + Environment.NewLine +
                Environment.NewLine +
                "Copyright (c) 2022, Tom OLIVIER" + Environment.NewLine +
                "All rights reserved.");
        }

        private static string GetAssemblyFileVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileVersion.FileVersion ?? "unknown";
        }
    }
}
