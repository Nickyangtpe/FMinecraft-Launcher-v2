using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IWshRuntimeLibrary;
using Microsoft.Win32;
using File = System.IO.File;
using Path = System.IO.Path;

namespace FMinecraft_Launcher_v2_Installer.Pages
{
    /// <summary>
    /// Installing.xaml 的互動邏輯
    /// </summary>
    public partial class Installing : Page
    {
        MainWindow _mainWindow = null;

        public Installing(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
            _mainWindow.isInstalling = true;

            InstallFML();
        }

        private async void InstallFML()
        {
            try
            {
                var Url = await new HttpClient().GetStringAsync("https://github.com/Nickyangtpe/FMinecraft-Launcher-v2/raw/refs/heads/main/Resources/latest");

                var TempFilePath = Path.Combine(Path.GetTempPath(), "FMinecraftLaumcher.latest.TEMP");

                var client = new HttpClient();
                var response = await client.GetAsync(Url, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                var totalBytes = response.Content.Headers.ContentLength.GetValueOrDefault();
                var totalMB = totalBytes / (1024.0 * 1024);
                var buffer = new byte[8192];
                var stream = await response.Content.ReadAsStreamAsync();
                using (var fs = new FileStream(TempFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var totalRead = 0L;
                    int bytesRead;
                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fs.WriteAsync(buffer, 0, bytesRead);
                        totalRead += bytesRead;

                        var currentMB = totalRead / (1024.0 * 1024);
                        ProgressLabel.Content = $"Downloading {currentMB:F2}MB / {totalMB:F2}MB";
                        ProgressBar.Value = totalRead * 100.0 / totalBytes;
                    }
                } 

                ProgressLabel.Content = "Unzipping...";
                
                var LauncherFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FMinecraftLauncher");
                if (Directory.Exists(LauncherFolder)) await Task.Run(() => Directory.Delete(LauncherFolder, true));
                Directory.CreateDirectory(LauncherFolder);
                await Task.Run(() => ZipFile.ExtractToDirectory(TempFilePath, LauncherFolder));
                File.Delete(TempFilePath);

                ProgressLabel.Content = "Creating shortcuts...";
                var ShortcutLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs", "FMinecraft Launcher", "FMinecraft Launcher.lnk");
                var TargetPath = Path.Combine(LauncherFolder, "win-x64", "FMinecraft Launcher v2.exe");
                Directory.CreateDirectory(Path.GetDirectoryName(ShortcutLocation));
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(ShortcutLocation);
                shortcut.TargetPath = TargetPath;
                shortcut.IconLocation = TargetPath;
                shortcut.Save();

                ProgressLabel.Content = "Installing...";
                var Info = FileVersionInfo.GetVersionInfo(TargetPath);
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + Path.GetFileName(TargetPath)))
                {
                    if (key != null)
                    {
                        key.SetValue("DisplayName", Path.GetFileName(TargetPath).Replace(".exe", ""));
                        key.SetValue("DisplayIcon", TargetPath);
                        key.SetValue("InstallLocation", TargetPath);
                        key.SetValue("UninstallString", Path.Combine(LauncherFolder, "Uninstall.exe"));
                        key.SetValue("Publisher", Info.CompanyName);
                        key.SetValue("DisplayVersion", Info.FileVersion);
                    }
                }

                File.Copy(Assembly.GetExecutingAssembly().Location, Path.Combine(LauncherFolder, "Uninstall.exe"));

                ProgressLabel.Content = "Done";

                _mainWindow.isInstalling = false;
                _mainWindow.Finish = true;

                NextButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _mainWindow.isError = true;
                CloseWindow(null, null);
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e) => _mainWindow.CloseWindow(sender, e);

        private void DragWindow(object sender, MouseButtonEventArgs e) => _mainWindow.DragWindow(sender, e);

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Content = new Finish(_mainWindow);
        }
    }
}