using Portable_store.Models;
using System.Runtime.CompilerServices;

namespace Portable_store
{
    public class Application : Application_metadata_Model
    {
        #region Constructors
        private Application(Application_metadata_Model application_metadata) :
            base (application_metadata.Name,
                application_metadata.Display_name,
                application_metadata.Icon_uri,
                application_metadata.Descriptions,
                application_metadata.Source_type,
                application_metadata.Versions)
        { }
        #endregion

        #region Variables
        Application_info_Model? application_info;
        #endregion

        #region Properties
        public bool IsDownloaded => !string.IsNullOrEmpty(Library.Get_install_info_path(Name));
        #endregion

        #region Methods
        public static async Task<Application> From(Application_metadata_Model application_metadata)
        {
            var application = new Application(application_metadata)
            {
                application_info = (await Library.List_Async(application_metadata.Name)).FirstOrDefault()
            };
            return application;
        }

        public static async Task<Application?> Get(string application_name)
        {
            var application_metadata = (await Store.Search_Async(application_name)).FirstOrDefault();

            if (application_metadata != null)
                return await From(application_metadata);
            else
                return null;
        }

        public static async Task<IReadOnlyList<Application>> Gets(string application_name)
        {
            var applications_metadata = await Store.Search_Async(application_name);
            var applications = new List<Application>(applications_metadata.Count);

            foreach (var application_metadata in applications_metadata)
                if (application_metadata != null)
                    applications.Add(await From(application_metadata));

            return applications.AsReadOnly();
        }

        public async Task<bool> Download_Async(Application_version_Model version, IProgress<Progress_info_Model>? progress = null)
        {
            var result = await Store.Download_Async(this, version, progress);

            if (result)
                application_info = (await Library.List_Async(Name)).FirstOrDefault();

            return result;
        }

        public Task<bool> Update_Async(Application_version_Model version, IProgress<Progress_info_Model> progress)
        {
            if (application_info == null)
                return Task.FromResult(false);

            return Store.Update_Async(application_info, progress);
        }

        public Task<bool> Delete_Async(IProgress<Progress_info_Model> progress)
        {
            if (application_info == null)
                return Task.FromResult(false);

            return Library.Delete_Async(application_info, progress);
        }

        public void Create_shortcut(string? shortcut_path = null)
        {
            if (application_info == null)
                return;

            Library.Create_application_shortcut(this, application_info.Executable_path, shortcut_path);
        }

        public bool Start()
        {
            if (application_info == null || !IsDownloaded)
                return false;

            return Library.Run(application_info);
        }

        public override string ToString() => Display_name;
        #endregion
    }
}
