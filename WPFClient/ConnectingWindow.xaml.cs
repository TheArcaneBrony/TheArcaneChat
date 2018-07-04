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
            mainWindow = parent;
            Left = parent.Left + parent.Width / 2 - Width / 2;
            Top = parent.Top + parent.Height/ 2 - Height / 2;
            Log.Items.Add(new WebBrowser());
        }
        private void Username_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Login();
        }
        private void Login()
        {
            mainWindow.Init(Username.Text);
            Log.Items.Add(mainWindow.ClientSocket.Connected ? "Connection successful!" : "Connection failed!");
            Log.Items.Add($"Username: {Username.Text}");
            UpdateLayout();
            mainWindow.Visibility = Visibility.Visible;
            Log.Items.Clear();
            Username.Text = null;
            UsernameLabel.Content = null;
            Username.Visibility = Visibility.Hidden;
            Log.Visibility = Visibility.Hidden;
            UsernameLabel.Visibility = Visibility.Hidden;

            Close();

        }
    }
}
