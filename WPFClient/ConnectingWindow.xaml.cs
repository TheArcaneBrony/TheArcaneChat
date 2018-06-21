using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for ConnectingWindow.xaml
    /// </summary>
    public partial class ConnectingWindow : Window
    {
        private MainWindow mainWindow = null;
        public ConnectingWindow(MainWindow parent)
        {
            InitializeComponent();
            mainWindow = parent;
            this.Loaded += ConnectingWindow_Loaded;

        }

        private void ConnectingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
              //  mainWindow.init(Username.Text);

                Log.Items.Add(this.mainWindow.clientSocket.Connected ? "Connection successful!" : "Connection failed, please check your internet connection!");
                Log.Items.Add($"Username: {Username.Text}");
                this.UpdateLayout();
                Thread.Yield();
                Thread.Sleep(1000);
                Close();
        }
    }
}
