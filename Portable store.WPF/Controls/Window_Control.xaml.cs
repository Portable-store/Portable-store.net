//https://engy.us/blog/2020/01/01/implementing-a-custom-window-title-bar-in-wpf/

using System;
using System.Windows;
using System.Windows.Controls;

namespace Portable_store.WPF.Controls
{
    public partial class Window_Control : UserControl
    {
        #region Constructors
        public Window_Control()
        {
            InitializeComponent();

            Window.StateChanged += Window_State_changed;
            Refresh_maximize_restore_Button();
        }
        #endregion

        #region Properties
        private static Window Window => System.Windows.Application.Current.MainWindow;
        #endregion

        #region Methods
        private void On_minimize_Button_click(object sender, RoutedEventArgs e)
        {
            Window.WindowState = WindowState.Minimized;
        }

        private void On_maximize_restore_Button_click(object sender, RoutedEventArgs e)
        {
            Window.WindowState = Window.WindowState == WindowState.Maximized ?
                WindowState.Normal : WindowState.Maximized;
        }

        private void On_close_Button_click(object sender, RoutedEventArgs e)
        {
            Window.Close();
        }

        private void Refresh_maximize_restore_Button()
        {
            Maximize_Button.Visibility = Window.WindowState == WindowState.Maximized ?
                Visibility.Collapsed : Visibility.Visible;
            Restore_Button.Visibility = Window.WindowState == WindowState.Maximized ?
                Visibility.Visible : Visibility.Collapsed;
        }

        private void Window_State_changed(object? sender, EventArgs e)
        {
            Refresh_maximize_restore_Button();
        }
        #endregion
    }
}
