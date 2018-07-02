using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for ConnectingWindow.xaml
    /// </summary>
    public partial class ConnectingWindow : Window
    {
        private MainWindow mainWindow;
        public ConnectingWindow(MainWindow parent)
        {
            InitializeComponent();
            //this.WindowStartupLocation = WindowStartupLocation.CenterOwner;


            mainWindow = parent;
            Left = parent.Left + parent.Width / 2 - Width / 2;
            Top = parent.Top + parent.Height/ 2 - Height / 2;
            Loaded += ConnectingWindow_Loaded;

        }

        private void ConnectingWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }
        private void Username_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Login();
        }
        private void Login()
        {
            mainWindow.Init(Username.Text);
            Log.Items.Add(mainWindow.ClientSocket.Connected ? "Connection successful!" : "Connection failed, please check your internet connection!");
            Log.Items.Add($"Username: {Username.Text}");
            UpdateLayout();
            mainWindow.Visibility = Visibility.Visible;
            Thread.Yield();
            Thread.Sleep(1000);
            Close();
        }
    }
}
