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
using System.Windows.Shapes;

namespace FMinecraft_Launcher_v2.Windows
{
    /// <summary>
    /// ErrorWindow.xaml 的互動邏輯
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public ErrorWindow(string Error)
        {
            InitializeComponent();
            ErrorText.Text = Error;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
