using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FMinecraft_Launcher_v2_Installer.Pages;
using Microsoft.Win32;
using Path = System.IO.Path;

namespace FMinecraft_Launcher_v2_Installer
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool isInstalling = false;
        public bool Finish = false;
        bool closing = false;
        public bool isError = false;
        bool isUninstall = false;

        public MainWindow()
        {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                MessageBox.Show("There is already an installation process in progress.");
                isError = true;
                CloseWindow();
                return;
            }

            if (Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName)
            .Equals("Uninstall.exe", StringComparison.OrdinalIgnoreCase))
            {
                Task.Run(async () =>
                {
                    isUninstall = true;
                    var LauncherFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FMinecraftLauncher", "win-x64");
                    foreach (var p in Process.GetProcesses())
                    {
                        try { if (p.MainModule.FileName.StartsWith(LauncherFolder, StringComparison.OrdinalIgnoreCase)) p.Kill(); } catch { }
                    }
                    await Task.Run(() => Directory.Delete(LauncherFolder, true));
                    string keyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\FMinecraft Launcher v2.exe";
                    Registry.CurrentUser.DeleteSubKeyTree(keyPath, false);

                    var ShortcutLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs", "FMinecraft Launcher");
                    if (Directory.Exists(ShortcutLocation)) Directory.Delete(ShortcutLocation, true);
                });
                MessageBox.Show("Uninstall successfully!");
                Process.Start(new ProcessStartInfo("cmd", $"/c timeout 1 && rmdir \"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\" /s /q")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                }); 
                CloseWindow();
                return;
            }

            InitializeComponent();
        }

        public void CloseWindow(object sender = null, RoutedEventArgs e = null)
        {
            if (Finish || isError || isUninstall) { closing = true; Close();return; }
            if(isInstalling)
            {
                MessageBox.Show("Installation in progress. Please do not close this window.", "FMinecraft Launcher Installer");
            }
            else
            {
                var result = MessageBox.Show("Do you want to cancel the installation process?",
                             "FMinecraft Launcher Installer",
                             MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    closing = true;
                    Close();
                }
            }
        }

        public void DragWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e) { }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (new Ping().Send("8.8.8.8").Status != IPStatus.Success)
            {
                MessageBox.Show("Please confirm the network connection.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                isError = true;
                CloseWindow();
                return;
            }
            Content = new LicenseAgreement(this);
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e) { if (!closing) e.Cancel = true; }
    }
}
