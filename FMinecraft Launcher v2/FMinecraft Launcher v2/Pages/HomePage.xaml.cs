using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace FMinecraft_Launcher_v2.Pages
{
    /// <summary>
    /// HomePage.xaml 的互動邏輯
    /// </summary>
    public partial class HomePage : Page
    {
        #region Fields
        private MainWindow _mainWindow;
        #endregion

        #region Constructor
        public HomePage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            InitializePage();
        }
        #endregion

        #region Initialization
        private void InitializePage()
        {
            // 在這裡加入任何需要在頁面載入時執行的邏輯
        }
        #endregion

        #region Event Handlers
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.settingspage();
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
        #endregion

        private void LaunchMinecraft(object sender, RoutedEventArgs e)
        {
            if(VerisonComboBox.SelectedItem == null)
            {
                _mainWindow.ShowError("Please select a version.");
                return;
            }

            _mainWindow.Launch_Minecraft(VerisonComboBox.SelectedItem.ToString(), VerisonComboBox, LaunchButton, DownloadProgressBar);
        }
    }
}