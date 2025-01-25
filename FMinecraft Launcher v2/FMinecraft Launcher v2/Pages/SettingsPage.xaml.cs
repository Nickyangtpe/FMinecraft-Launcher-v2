using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static FMinecraft_Launcher_v2.MainWindow;

namespace FMinecraft_Launcher_v2.Pages
{
    /// <summary>
    /// SettingsPage.xaml 的互動邏輯
    /// </summary>
    public partial class SettingsPage : Page
    {
        #region Fields
        private MainWindow _mainWindow;
        #endregion

        #region Constructor
        public SettingsPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            InitializePage();
        }
        #endregion

        #region Initialization
        private void InitializePage()
        {

        }
        #endregion

        #region Event Handlers
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.homepage();
        }

        private void AboutUsButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.aboutpage();
        }

        private void GithubButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/Nickyangtpe/FMinecraft-Launcher/",
                UseShellExecute = true,
            });
        }

        private void ConsoleButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.console_Window.Show();
        }

        private void SettingsChanged(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        private void UserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(UserName.Text) || UserName.Text == "User") 
            {
                UserName.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF959595");
                UserName.Text = "User";
            }
            else UserName.Foreground = (Brush)new BrushConverter().ConvertFrom("#FFFFFFFF");

            SaveSettings();
        }

        private async void SaveSettings()
        {
            if (_mainWindow != null)
            {
                var settings = new Settings
                {
                    HideLauncher = HideLauncher.IsChecked.GetValueOrDefault(),
                    ShowConsole = ShowConsole.IsChecked.GetValueOrDefault(),
                    CloseLauncher = CloseLauncher.IsChecked.GetValueOrDefault(),
                    ShowReleases = releases.IsChecked.GetValueOrDefault(),
                    ShowSnapshot = snapshot.IsChecked.GetValueOrDefault(),
                    ShowAlpha = Alpha.IsChecked.GetValueOrDefault(),
                    TopMost = TopMostLauncher.IsChecked.GetValueOrDefault(),
                    AccessToken = "none",
                    UserName = UserName.Text,
                };
                string SettingsFilePath = Path.Combine(".launcher", "settings.json");
                string SettingsJson = JsonConvert.SerializeObject(settings, Formatting.Indented);
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsFilePath)!);
                await File.WriteAllTextAsync(SettingsFilePath, SettingsJson, Encoding.UTF8);
                _mainWindow.ApplySettingsToUI(settings);
                _mainWindow.Launcher_Console(ConsoleType.Launcher, ConsoleMessageType.Info, $"Settings saved to '{SettingsFilePath}'.");
            }
        }
        private void versionChage(object sender, RoutedEventArgs e)
        {
            if (_mainWindow != null && _mainWindow.isInitialized) _mainWindow.LoadVersionManifestAsync();
        }

        private void CoverComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_mainWindow != null && _mainWindow.isInitialized) _mainWindow.LoadLauncherCover();
        }

        private void VersioButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer", Path.GetFullPath(Path.Combine(".minecraft", "versions")));
        }
        #endregion

    }
}