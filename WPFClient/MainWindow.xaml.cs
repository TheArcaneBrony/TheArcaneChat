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
            init("Luxuride");
            Connection();
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Task shutdownTask = new Task(() =>
            {
                byte[] outStream = System.Text.Encoding.Unicode.GetBytes("\0CLIMSG\0exit");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Close(1000);
            });
            shutdownTask.Start();
            shutdownTask.Wait();
            Environment.Exit(0);
        }

        private async void TxbInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                await Send();
        }
        private async void BtnSend_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await Send();
        }
        private async Task Send()
        {
            if (TxbInput.Text.Length >= 0)
            {
                byte[] sendBite = System.Text.Encoding.Unicode.GetBytes(TxbInput.Text);
                TxbInput.Text = null;
                await serverStream.WriteAsync(sendBite, 0, sendBite.Length);
            }
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
        public async void init(string username)
        {
            System.Console.WriteLine("INIT");
            try
            {
#if DEBUG
                clientSocket.Connect("127.0.0.1", 8888);
#else
            clientSocket.Connect("TheArcaneBrony.ddns.net", 8888);
#endif
                UserName.Text = $"Connected as {username}";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                UserName.Text = "Connection failed!";
            }


            serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.Unicode.GetBytes("/nick " + username);
            await serverStream.WriteAsync(outStream, 0, outStream.Length);
        }
    }
}
