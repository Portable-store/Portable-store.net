﻿using Portable_store.WPF.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Portable_store.WPF.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Main_window : Window
    {
        public Main_window()
        {
            InitializeComponent();

            // For watever reason VS won't compile any user control inside xaml
            var window_control = new Controls.Window_Control
            {
                HorizontalAlignment = HorizontalAlignment.Right
            };
            window_control.SetValue(Grid.ColumnProperty, 2);

            TopBar.Children.Add(window_control);

            application_list_page = new();

            refresh_Store();
        }

        Application_list_Page application_list_page;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            ((HwndSource)PresentationSource.FromVisual(this)).AddHook(HookProc);
        }

        public static IntPtr HookProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == Win32.WM_GETMINMAXINFO)
            {
                // We need to tell the system what our size should be when maximized. Otherwise it will cover the whole screen,
                // including the task bar.
                Win32.MINMAXINFO mmi = (Win32.MINMAXINFO)(Marshal.PtrToStructure(lParam, typeof(Win32.MINMAXINFO)) ?? new Win32.MINMAXINFO());

                // Adjust the maximized size and position to fit the work area of the correct monitor
                var monitor = Win32.MonitorFromWindow(hwnd, Win32.MONITOR_DEFAULTTONEAREST);

                if (monitor != IntPtr.Zero)
                {
                    var monitorInfo = new Win32.MONITORINFO
                    {
                        cbSize = Marshal.SizeOf(typeof(Win32.MONITORINFO))
                    };

                    Win32.GetMonitorInfo(monitor, ref monitorInfo);

                    Win32.RECT rcWorkArea = monitorInfo.rcWork;
                    Win32.RECT rcMonitorArea = monitorInfo.rcMonitor;

                    mmi.ptMaxPosition.X = Math.Abs(rcWorkArea.Left - rcMonitorArea.Left);
                    mmi.ptMaxPosition.Y = Math.Abs(rcWorkArea.Top - rcMonitorArea.Top);
                    mmi.ptMaxSize.X = Math.Abs(rcWorkArea.Right - rcWorkArea.Left);
                    mmi.ptMaxSize.Y = Math.Abs(rcWorkArea.Bottom - rcWorkArea.Top);
                }

                Marshal.StructureToPtr(mmi, lParam, true);
            }

            return IntPtr.Zero;
        }

        private async void refresh_Store()
        {
            await Store.Refresh_Async();
            Debug.WriteLine("Refresh done!");
        }

        private async void Search_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            await application_list_page.Search_Async(Search_Box.Text);
            Content_frame.Navigate(application_list_page);
        }
    }
}
