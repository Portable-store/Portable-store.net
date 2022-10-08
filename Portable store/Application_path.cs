namespace Portable_store
{
    public static class Application_path
    {
        /// <summary>
        /// Get the application root folder
        /// </summary>
        public static string Root =>
            AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// Get the application data folder
        /// </summary>
        public static string Data =>
            AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "AppData";


        #region Methods
        /// <summary>
        /// Get the full path of a file inside the app directory
        /// </summary>
        /// <param name="fileName">Relative path</param>
        /// <returns>The full path</returns>
        /// <exception cref="ArgumentException" />
        /// <exception cref="System.Security.SecurityException" />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="NotSupportedException" />
        /// <exception cref="PathTooLongException" />
        public static string GetFullPath(string fileName, string? baseDirectory = null)
        {
            baseDirectory ??= Root;

            fileName = fileName.Replace('\\', '/');
            fileName = fileName.TrimStart('/');

            string relativePath = Path.Combine(baseDirectory, fileName);
            string path = Path.GetFullPath(relativePath);

            return path;
        }
        #endregion
    }
}
