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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            Thread thread = new Thread(() => {
                while (true)
                    try
                    {
                        Thread.Sleep(50);
                        if (serverStream == null) continue;
                        //byte[] inStream = new byte[clientSocket.ReceiveBufferSize];
                        List<byte> inStream = new List<byte>();

                        //serverStream.Read(inStream, 0, inStream.Length);
                        while (!System.Text.Encoding.Unicode.GetString(inStream.ToArray()).Contains("\0MSGEND\0"))
                        {
                            byte[] inBytes = new byte[1];
                            serverStream.Read(inBytes, 0, 1);
                            inStream.AddRange(inBytes);
                        }
                        string returndata = System.Text.Encoding.Unicode.GetString(inStream.ToArray()).Replace("\0MSGEND\0", "");
                        Invoke(new MethodInvoker(() =>
                        {
                            if (returndata.Contains("\n")) MessageLog.Items.Add(returndata.Replace("\0MSGEND\0", ""));
                            else MessageLog.Items.AddRange(returndata.Replace("\0MSGEND\0", "").Split("\n".ToCharArray()));

                        }));
                        Console.WriteLine(returndata);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
            });
            thread.Start();
        }
    }
}
