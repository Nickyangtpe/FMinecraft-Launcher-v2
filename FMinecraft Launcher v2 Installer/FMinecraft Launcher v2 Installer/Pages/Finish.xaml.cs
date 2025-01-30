using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using Path = System.IO.Path;

namespace FMinecraft_Launcher_v2_Installer.Pages
{
    /// <summary>
    /// Finish.xaml 的互動邏輯
    /// </summary>
    public partial class Finish : Page
    {
        MainWindow _mainWindow = null;
        public Finish(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
        }
        private void CloseWindow(object sender, RoutedEventArgs e) => _mainWindow.CloseWindow(sender, e);

        private void DragWindow(object sender, MouseButtonEventArgs e) => _mainWindow.DragWindow(sender, e);

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FMinecraftLauncher"), "win-x64", "FMinecraft Launcher v2.exe"));
            _mainWindow.CloseWindow();
        }
    }
}
