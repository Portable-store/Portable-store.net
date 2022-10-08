using Portable_store.Enums;
using Portable_store.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;

namespace Portable_store
{
    public static class Store
    {
        public static readonly Uri Applications_folder_URI =
            new("https://github.com/Tom60chat/Portable-store.git/trunk/Datas/Applications?token=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");

        public static async Task<bool> Download_Async(Application_Model metadata, Application_version_Model version, IProgress<Progress_info_Model> progress)
        {
            var progress_info = new Progress_info_Model(0);

            progress_info.Increment("Downloading: " + metadata.Name);
            progress.Report(progress_info);

            var downloaded_file = await Downloader.Download_Async(metadata, version);

            await Library.Install_Async(metadata, downloaded_file, progress);

            File.Delete(downloaded_file);

            return true;
        }

        public static async Task<IReadOnlyList<Application_Model>> Search_Async(string application_name, IProgress<Progress_info_Model> progress)
        {
            var metadatas = await Cache.Fetch_cached_applications_Async(application_name, progress);

            return metadatas;
        }

        public static Task<bool> Update_Async(string application_name, IProgress<Progress_info_Model> progress)
        {
            //var metadatas = Search_Async(application_name);

            return Task.FromResult(false);
        }

        public static bool Refresh() =>
            Cache.Refresh_metadata_cache(Applications_folder_URI.OriginalString);
    }
}