using Octokit;
using Portable_store.Enums;
using Portable_store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Portable_store
{
    internal static class Downloader
    {
        internal static async Task<string> Download_Async(Application_metadata_Model metadata, Application_version_Model version)
        {
            var url = metadata.Source_type switch
            {
                Source_type_Enum.GitHub =>
                    await Download_from_GitHub_Async(metadata, version),
                Source_type_Enum.SourceForge =>
                    await Download_from_SourceForge_Async(metadata),
                Source_type_Enum.RedirectLink =>
                    await Download_from_RedirectLink_Async(metadata),
                Source_type_Enum.PortableSetupWizard =>
                    await Download_from_PortableSetupWizard_Async(metadata),
                Source_type_Enum.SetupWizard =>
                    await Download_from_SetupWizard_Async(metadata),
                _ or Source_type_Enum.DirectLink =>
                    await Download_from_DirectLink_Async(metadata),
            };

            using var client = new HttpClient();
            using var stream = await client.GetStreamAsync(url);
            using var file_stream = Cache.Create_new_download_file(Path.GetFileName(url));

            await stream.CopyToAsync(file_stream);

            return file_stream.Name;
        }

        private static async Task<string> Download_from_GitHub_Async(Application_metadata_Model metadata, Application_version_Model version)
        {
            var names = metadata.Name.Split('/', 2);

            if (names.Length != 2)
                throw new ArgumentException("The given metadata don't contain a proper GitHub name");

            var RepositoryOwner = names[0];
            var RepositoryName = names[1];

            try
            {
                var client = new GitHubClient(new ProductHeaderValue(RepositoryName));
                var release = await client.Repository.Release.GetLatest(RepositoryOwner, RepositoryName);


                var rtest = await client.Repository.Get(RepositoryOwner, RepositoryName);

                var assets = release.Assets;

                foreach (var asset in assets)
                {
                    if (Regex.IsMatch(asset.Name, version.URI))
                        return asset.BrowserDownloadUrl;
                }
            }
            catch
            {

            }

            return string.Empty;
        }

        private static async Task<string> Download_from_SourceForge_Async(Application_metadata_Model metadata)
        {


            return string.Empty;
        }

        private static async Task<string> Download_from_RedirectLink_Async(Application_metadata_Model metadata)
        {


            return string.Empty;
        }

        private static async Task<string> Download_from_DirectLink_Async(Application_metadata_Model metadata)
        {


            return string.Empty;
        }

        private static async Task<string> Download_from_PortableSetupWizard_Async(Application_metadata_Model metadata)
        {


            return string.Empty;
        }

        private static async Task<string> Download_from_SetupWizard_Async(Application_metadata_Model metadata)
        {


            return string.Empty;
        }

        /// <summary>
        /// Complete missing information from a local metadata like:
        ///  - Icon
        ///  - Description
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        internal static Task<Application_metadata_Model> Download_metadata_Async(Application_metadata_Model metadata) =>
            metadata.Source_type switch
            {
                Source_type_Enum.GitHub =>
                    Download_metadata_from_GitHub_Async(metadata),
                Source_type_Enum.SourceForge =>
                    Download_metadata_from_SourceForge_Async(metadata),
                Source_type_Enum.RedirectLink =>
                    Download_metadata_from_RedirectLink_Async(metadata),
                Source_type_Enum.PortableSetupWizard =>
                    Download_metadata_from_PortableSetupWizard_Async(metadata),
                Source_type_Enum.SetupWizard =>
                    Download_metadata_from_SetupWizard_Async(metadata),
                _ or Source_type_Enum.DirectLink =>
                    Download_metadata_from_DirectLink_Async(metadata),
            };

        private static async Task<Application_metadata_Model> Download_metadata_from_GitHub_Async(Application_metadata_Model metadata)
        {
            var names = metadata.Name.Split('/', 2);

            if (names.Length != 2)
                throw new ArgumentException("The given metadata don't contain a proper GitHub name");

            var RepositoryOwner = names[0];
            var RepositoryName = names[1];

            try
            {
                var git_client = new GitHubClient(new ProductHeaderValue(RepositoryName));
                var repository = await git_client.Repository.Get(RepositoryOwner, RepositoryName);

                using var client = new HttpClient();
                var readme_url = $"https://raw.githubusercontent.com/{RepositoryOwner}/{RepositoryName}/{repository.DefaultBranch}/README.md";

                // Name
                if (string.IsNullOrEmpty(metadata.Name))
                    metadata.Name = repository.FullName;

                // Icon url
                if (string.IsNullOrEmpty(metadata.Icon_uri))
                    metadata.Icon_uri = repository.Owner.AvatarUrl;

                // Short decription
                if (string.IsNullOrEmpty(metadata.Descriptions.Short_description))
                    metadata.Descriptions.Short_description = repository.Description;

                // Website
                if (string.IsNullOrEmpty(metadata.Descriptions.Website))
                    metadata.Descriptions.Website = repository.Homepage;

                // Keywords
                if (metadata.Descriptions.Keywords.Length == 0)
                    metadata.Descriptions.Keywords = repository.Topics.ToArray();

                // Long description
                if (string.IsNullOrEmpty(metadata.Descriptions.Long_description))
                {
                    using var stream = await client.GetStreamAsync(readme_url);
                    using var reader = new StreamReader(stream);
                    metadata.Descriptions.Long_description = await reader.ReadToEndAsync();
                }
            }
            catch
            {

            }

            return metadata;
        }

        private static async Task<Application_metadata_Model> Download_metadata_from_SourceForge_Async(Application_metadata_Model metadata)
        {


            return metadata;
        }

        private static async Task<Application_metadata_Model> Download_metadata_from_RedirectLink_Async(Application_metadata_Model metadata)
        {


            return metadata;
        }

        private static async Task<Application_metadata_Model> Download_metadata_from_DirectLink_Async(Application_metadata_Model metadata)
        {


            return metadata;
        }

        private static async Task<Application_metadata_Model> Download_metadata_from_PortableSetupWizard_Async(Application_metadata_Model metadata)
        {


            return metadata;
        }

        private static async Task<Application_metadata_Model> Download_metadata_from_SetupWizard_Async(Application_metadata_Model metadata)
        {


            return metadata;
        }
    }
}
