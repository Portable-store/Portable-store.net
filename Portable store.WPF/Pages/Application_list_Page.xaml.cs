using Portable_store.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Portable_store.WPF.Pages
{
    /// <summary>
    /// Logique d'interaction pour Application_list_Page.xaml
    /// </summary>
    public partial class Application_list_Page : Page
    {
        public Application_list_Page()
        {
            Applications = new();
            InitializeComponent();

            DataContext = this;
        }

        public ObservableCollection<Application> Applications { get; set; }

        private CancellationTokenSource? search_cancellation_token;
        private CancellationTokenSource? search_queue_cancellation_token;

        private async void Install_Uninstall_Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Application application)
            {
                if (application.IsDownloaded)
                    application.Start();
                else
                {
                    var version = Metadata.Get_compatible_versions(application).First();
                    await application.Download_Async(version);
                }
            }
        }

        public async Task<bool> Search_Async(string keywords)
        {
            // A search is already working
            if (search_cancellation_token != null)
            {
                // Cancel the current serch
                search_cancellation_token.Cancel();

                // Cancel the last search
                if (search_queue_cancellation_token != null)
                    search_queue_cancellation_token.Cancel();
                search_queue_cancellation_token = new();

                // Get token instance
                var token = search_queue_cancellation_token.Token;

                // Wait
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(500);
                }

                if (token.IsCancellationRequested)
                    return false;
            }

            //var progress = new Progress<Progress_info_Model>();

            search_cancellation_token = new();
            var applications = await Application.Gets($"*{keywords}*"); //await Store.Search_Async($"*{keywords}*", progress, search_cancellation_token.Token);

            Applications.Clear();
            foreach(var application in applications)
                Applications.Add(application);

            search_cancellation_token = null;

            return true;
        }
    }
}
