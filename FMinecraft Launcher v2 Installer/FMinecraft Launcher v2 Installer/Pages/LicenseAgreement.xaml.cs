using System;
using System.Collections.Generic;
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

namespace FMinecraft_Launcher_v2_Installer.Pages
{
    /// <summary>
    /// LicenseAgreement.xaml 的互動邏輯
    /// </summary>
    public partial class LicenseAgreement : Page
    {
        MainWindow _mainWindow = null;
        public LicenseAgreement(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
        }
        private void CloseWindow(object sender, RoutedEventArgs e) => _mainWindow.CloseWindow(sender, e);

        private void DragWindow(object sender, MouseButtonEventArgs e) => _mainWindow.DragWindow(sender, e);

        private void CancelButton_Click(object sender, RoutedEventArgs e) => _mainWindow.CloseWindow(sender, e);

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Content = new Installing(_mainWindow);
        }
    }
}
