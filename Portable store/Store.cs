using Portable_store.Models;

namespace Portable_store
{
    public static class Store
    {
        public static readonly Uri Applications_folder_URI =
            new("https://github.com/Tom60chat/Portable-store.git/trunk/Datas/Applications");

        public static async Task<bool> Download_Async(Application_metadata_Model metadata, Application_version_Model version, IProgress<Progress_info_Model>? progress = null)
        {
            var progress_info = new Progress_info_Model(0);

            progress_info.Increment("Downloading: " + metadata.Name);
            progress?.Report(progress_info);

            var downloaded_file = await Downloader.Download_Async(metadata, version);

            await Library.Install_Async(metadata, downloaded_file, progress);

            File.Delete(downloaded_file);

            return true;
        }

        public static async Task<IReadOnlyList<Application_metadata_Model>> Search_Async(string application_name, IProgress<Progress_info_Model>? progress = null, CancellationToken cancellation_token = default)
        {
            var metadatas = await Cache.Fetch_cached_applications_Async(application_name, progress, cancellation_token);

            return metadatas;
        }

        public static Task<bool> Update_Async(Application_info_Model application_info, IProgress<Progress_info_Model> progress)
        {
            //var metadatas = Search_Async(application_name);

            return Task.FromResult(false);
        }

        /// <summary>
        /// Basic refresh
        /// </summary>
        /// <returns></returns>
        public static bool Refresh() =>
            Cache.Refresh_metadata_cache(Applications_folder_URI.OriginalString);

        /// <summary>
        /// Refresh with much more info inside metadatas
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> Refresh_Async()
        {
            var result = Cache.Refresh_metadata_cache(Applications_folder_URI.OriginalString);

            if (result)
                await Cache.Enhance_metadata_cache();

            return result;
        }
    }
}