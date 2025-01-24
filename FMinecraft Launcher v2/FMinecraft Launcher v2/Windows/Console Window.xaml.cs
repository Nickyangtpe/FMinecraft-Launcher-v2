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
    /// Console_Window.xaml 的互動邏輯
    /// </summary>
    public partial class Console_Window : Window
    {
        MainWindow Mainwindow;
        public Console_Window(MainWindow mainwindow)
        {
            Mainwindow = mainwindow;
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Mainwindow.Closed) return;
            e.Cancel = true;
            Hide();
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(outputTextBox.Text);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            outputTextBox.Text = "";
        }
    }
}
