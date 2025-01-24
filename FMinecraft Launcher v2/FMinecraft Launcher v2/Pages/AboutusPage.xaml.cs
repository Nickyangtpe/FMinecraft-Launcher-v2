using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace FMinecraft_Launcher_v2.Pages
{
    /// <summary>
    /// AboutusPage.xaml 的互動邏輯
    /// </summary>
    public partial class AboutusPage : Page
    {
        #region Fields
        private MainWindow _mainWindow;
        #endregion

        #region Constructor
        public AboutusPage(MainWindow mainWindow)
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
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.homepage();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.settingspage();
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
    }
}