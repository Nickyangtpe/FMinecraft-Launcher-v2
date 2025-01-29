using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace FMinecraft_Launcher_v2.Windows
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        // Define a new base path to the FMinecraft Launcher folder in AppData Roaming
        private static readonly string BasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".FMinecraftLauncher");

        public LoadingWindow(MainWindow _mainWindow)
        {
            InitializeComponent();

            Initialize(_mainWindow);
        }

        private async void Initialize(MainWindow _mainWindow)
        {
            var CoverDirectory = Path.Combine(BasePath, ".launcher", "Covers");
            if (!Directory.Exists(CoverDirectory))
            {
                var tempZipPath = Path.Combine(Path.GetTempPath(), "BannerArea.zip");
                var url = "https://github.com/Nickyangtpe/FMinecraft-Launcher-v2/raw/refs/heads/main/Resources/Banner%20Area.zip";

                using (var client = new WebClient())
                {
                    client.DownloadProgressChanged += (sender, e) =>
                    {
                        double DownloadedKB = Math.Round(e.BytesReceived / 1024.0, 1);
                        double TotalKB = Math.Round(e.TotalBytesToReceive / 1024.0, 1);
                        LoadLabel.Text = $"{DownloadedKB}KB / {TotalKB}KB";
                    };
                    await client.DownloadFileTaskAsync(new Uri(url), tempZipPath);
                }


                await Task.Run(() => ZipFile.ExtractToDirectory(tempZipPath, CoverDirectory, true));

                File.Delete(tempZipPath);

            }

            LoadLabel.Text = "Loading...";

            await Task.Run(async () =>
            {
                while (!Finish)
                {
                    await Task.Delay(200);
                }
            });

            _mainWindow.Show();
            Close();
        }

        public bool Finish = false;


        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!Finish) e.Cancel = true;
        }
    }
}