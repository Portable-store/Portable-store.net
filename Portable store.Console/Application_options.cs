namespace Portable_store.Console
{
    internal static class Application_options
    {
        #region Properties
        /// <summary>
        /// -q, --quiet
        /// Produces output suitable for logging, omitting progress indicators.
        /// </summary>
        public static bool Quiet { get; private set; }

        /// <summary>
        /// -y, --yes
        /// Automatic yes to prompts. Assume "yes" as answer to all prompts and run non-interactively.
        /// </summary>
        public static bool Yes { get; private set; }

        /// <summary>
        /// -V, --verbose
        /// Show full versions logs for upgraded and installed packages.
        /// </summary>
        public static bool Verbose { get; private set; }

        /// <summary>
        /// -C, --clean, --clean_download
        /// Do a clean download, this might delete all user settings or unwanted file, inside the destination application folder.
        /// </summary>
        public static bool Clean { get; private set; }

        /// <summary>
        /// -h, --help
        /// Show help dialog
        /// </summary>
        public static bool Help { get; private set; }

        /// <summary>
        /// -v, --version
        /// Show the application version.
        /// </summary>
        public static bool Version { get; private set; }

        /// <summary>
        /// -c, --config-file
        /// Specify a configuration file to use.
        /// </summary>
        public static string Config_file { get; private set; } = "";
        
        #endregion

        #region Methods
        public static void Parse_options(IEnumerable<string> options)
        {
            foreach (var option in options)
            {
                Parse_option(option);
            }

            if (Verbose && Quiet)
            {
                Verbose = Quiet = false;
                ConsoleHelper.WriteLine("-V, --verbose and -q, --quiet don't make sense, both are ignored");
            }
        }

        public static void Parse_option(string argument)
        {
            var split = argument.Split('=', 2);
            var option = split[0];
            var value = split.Length > 1 ? split[1] : string.Empty;

            switch (option)
            {
                case "-q":
                case "--quiet":
                    Quiet = true;
                    break;

                case "-y":
                case "--yes":
                    Yes = true;
                    break;

                case "-V":
                case "--verbose":
                    Verbose = true;
                    break;

                case "-C":
                case "--clean":
                    Clean = true;
                    break;

                case "-h":
                case "--help":
                    Help = true;
                    break;

                case "-v":
                case "--version":
                    Version = true;
                    break;

                case "-c":
                case "--config-file":
                    Config_file = value;
                    break;
            }
        }
        #endregion
    }
}
