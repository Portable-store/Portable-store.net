using Portable_store.Models;
using SharpSvn;
using System.Threading;

namespace Portable_store
{
    public class Cache
    {
        private readonly static string Metadatas_path = Application_path.GetFullPath("Caches/Metadatas", Application_path.Data);
        private readonly static string Downloads_path = Application_path.GetFullPath("Caches/Downloads", Application_path.Data);

        internal static bool Refresh_metadata_cache(string source)
        {
            Folders_integrety_check();

            var svnclient = new SvnClient();
            var args = new SvnExportArgs()
            {
                Overwrite = true
            };

            try
            {
                return svnclient.Export(
                    new SvnUriTarget(source),
                    Metadatas_path,
                    args);
            }
            catch
            {
                return false;
            }
        }

        internal static async Task Enhance_metadata_cache(CancellationToken cancellation_token = default)
        {
            var metadatas_folder = new DirectoryInfo(Metadatas_path);

            foreach(var metadata_file in metadatas_folder.GetFiles())
            {
                var metadata = await Metadata.Read_Async(metadata_file.FullName, null, cancellation_token);

                if (metadata != null)
                {
                    var enhanced_metadata = await Downloader.Download_metadata_Async(metadata);
                    var enhanced_metadata_JSON = enhanced_metadata.To_JSON();

                    await File.WriteAllTextAsync(metadata_file.FullName, enhanced_metadata_JSON, cancellation_token);
                }
            }
        }

        internal static Task<IReadOnlyList<Application_metadata_Model>> Fetch_cached_applications_Async(IProgress<Progress_info_Model>? progress)
            => Fetch_cached_applications_Async("*", progress);

        internal static async Task<IReadOnlyList<Application_metadata_Model>> Fetch_cached_applications_Async(string search_pattern, IProgress<Progress_info_Model>? progress, CancellationToken cancellation_token = default)
        {
            Folders_integrety_check();

            var metadatas_path = Directory.GetFiles(Metadatas_path, search_pattern + ".json");
            var metadatas_JSON = new List<Application_metadata_Model>(metadatas_path.Length);

            var progress_info = new Progress_info_Model(metadatas_path.Length);
            var subprogress = new Progress<Progress_info_Model>(p =>
            {
                progress_info.Update(p.Details, metadatas_JSON.Count * 100 / metadatas_path.Length);
                progress?.Report(progress_info);
            });

            foreach (var metadata_path in metadatas_path)
            {
                var metadata = await Metadata.Read_Async(metadata_path, subprogress, cancellation_token);

                if (metadata != null)
                    metadatas_JSON.Add(metadata);
            }

            return metadatas_JSON.AsReadOnly();
        }

        internal static FileStream Create_new_download_file(string name)
        {
            Folders_integrety_check();

            var destination = Path.Combine(Downloads_path, Path.GetRandomFileName());
            var fullname = Path.Combine(destination, name);

            Directory.CreateDirectory(destination);

            return new FileStream(fullname, FileMode.CreateNew);
        }

        private static void Folders_integrety_check()
        {
            Directory.CreateDirectory(Metadatas_path);
            Directory.CreateDirectory(Downloads_path);
        }
    }
}
