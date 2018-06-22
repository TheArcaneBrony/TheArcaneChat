﻿using System;
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
            //this.WindowStartupLocation = WindowStartupLocation.CenterOwner;


            mainWindow = parent;
            this.Left = parent.Left + parent.Width / 2 - Width / 2;
            this.Top = parent.Top + parent.Height/ 2 - Height / 2;
            this.Loaded += ConnectingWindow_Loaded;

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
        private void Username_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Login();
        }
        private void Login()
        {
            mainWindow.init(Username.Text);
            Log.Items.Add(this.mainWindow.clientSocket.Connected ? "Connection successful!" : "Connection failed, please check your internet connection!");
            Log.Items.Add($"Username: {Username.Text}");
            this.UpdateLayout();
            mainWindow.Visibility = Visibility.Visible;
            Thread.Yield();
            Thread.Sleep(1000);
            Close();
        }
    }
}
