using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream;
        public MainWindow()
        {
            InitializeComponent();
            Connection();
        }

        private void LbChat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) ;

        }

        private void Connection()
        {
            ConnectingWindow connectingWindow = new ConnectingWindow(this);
            connectingWindow.Show();
            connectingWindow.Topmost = true;
            new Thread(() =>
            {
                while (true)
                    try
                    {
                        Thread.Sleep(100);
                        if (serverStream == null) continue;
                        List<byte> inStream = new List<byte>();
                        string returndata = "";
                        while (!returndata.Contains("\0MSGEND\0"))
                        {
                            byte[] inBytes = new byte[1];
                            serverStream.Read(inBytes, 0, 1);
                            inStream.AddRange(inBytes);
                            returndata = System.Text.Encoding.Unicode.GetString(inStream.ToArray());
                        }

                        returndata = returndata.Replace("\0MSGEND\0", "");
                        Dispatcher.Invoke(new MethodInvoker(() => { LbChat.Items.Add(returndata); }));
                        Console.WriteLine(returndata);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
            }).Start();
        }
    }
}
