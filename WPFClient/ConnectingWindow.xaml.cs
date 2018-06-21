using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
