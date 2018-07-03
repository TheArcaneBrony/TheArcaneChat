using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Media;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Application = System.Windows.Forms.Application;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace WPFClient
{
    public partial class MainWindow : Window
    {
        public string VersionString = "v1.0.0";

        public TcpClient ClientSocket = new TcpClient();
        NetworkStream _serverStream;
        public string Username = "DebugUser";

        public MainWindow()
        {
            InitializeComponent();

            Show();

            var cw = new ConnectingWindow(this);
#if DEBUG
            if(File.Exists(@"C:\TheArcaneBrony.txt"))
            Init("TheArcaneBrony");
            else
            Init(Username);
#else
            cw.Show();
#endif
            Activated += (sender, args) =>
            {
                if (cw.IsVisible) cw.Activate();
            };
            Connection();
            Closing += MainWindow_Closing;
            MouseDown += (sender, args) =>
            {
                CloseButton.Tag = (args.OriginalSource == CloseButton) ? "hit" : "";
                WindowStateButton.Tag = (args.OriginalSource == WindowStateButton) ? "hit" : "";
                MinimiseButton.Tag = (args.OriginalSource == MinimiseButton) ? "hit" : "";

            };

            CloseButton.MouseUp += (sender, args) =>
            {
                if (CloseButton.Tag == "hit") Shutdown();
            };

            WindowStateButton.MouseUp += (sender, args) =>
            {
                if (WindowStateButton.Tag == "hit")
                    if (WindowState == WindowState.Normal)
                        WindowState = WindowState.Maximized;
                    else
                        WindowState = WindowState.Normal;
            };

            MinimiseButton.MouseUp += (sender, args) =>
            {
                if (MinimiseButton.Tag == "hit") Main.WindowState = WindowState.Minimized;
            };

            Titlebar.MouseDown += TitlebarOnMouseDown;
        }

        private void TitlebarOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.MouseDevice.DirectlyOver != CloseButton && e.MouseDevice.DirectlyOver != WindowStateButton && e.MouseDevice.DirectlyOver != MinimiseButton && e.LeftButton == MouseButtonState.Pressed) { DragMove(); }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            Shutdown();
        }

        public void Shutdown()
        {
            var outStream = Encoding.Unicode.GetBytes("\0CLIMSG\0exit");
            _serverStream.Write(outStream, 0, outStream.Length);
            _serverStream.Close(1000);
            Application.Exit();
            Environment.Exit(0);
        }
        private void TxbInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (TxbInput.Text.Length < 0) return;
                else Send(TxbInput.Text);

        }

        private void Send(string text)
        {
            try
            {
                var sendBytes = Encoding.Unicode.GetBytes(TxbInput.Text);
                TxbInput.Text = "";
                _serverStream.Write(sendBytes, 0, sendBytes.Length);
            }
            catch (Exception e)
            {
                LbChat.Items.Add("Connection failure, reconnecting~!");
                Init(Username);
            }
        }

        private void Connection()
        {
            new Thread(() =>
            {
                while (true)
                    try
                    {
                        Thread.Sleep(10);
                        if (_serverStream == null) continue;
                        var inStream = new List<byte>();
                        var returndata = "";
                        while (!returndata.Contains("\0MSGEND\0"))
                        {
                            var inBytes = new byte[1];
                            _serverStream.Read(inBytes, 0, 1);
                            inStream.AddRange(inBytes);
                            returndata = Encoding.Unicode.GetString(inStream.ToArray());
                        }

                        returndata = returndata.Replace("\0MSGEND\0", "");
                        Dispatcher.Invoke(new MethodInvoker(() => { LbChat.Items.Add(returndata); }));
                        new Task(() =>
                        {
                            new SoundPlayer(@"C:\Windows\Media\Windows Default.wav").Play();
                        }).Start();
                        Console.WriteLine(returndata);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
            }).Start();
        }
        public void Init(string username)
        {
            try
            {
#if DEBUG
                ClientSocket.Connect("127.0.0.1", 8888);
#else
                clientSocket.Connect("thearcanebrony.ddns.net", 8888);
#endif
                SetTitle($"Connected as {username}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                SetTitle("Connection failed!");
            }

            _serverStream = ClientSocket.GetStream();
            var outStream = Encoding.Unicode.GetBytes($"/nick {username}");
            _serverStream.Write(outStream, 0, outStream.Length);
        }
        public void SetTitle(string title)
        {
            Title = $"TheArcaneChat [WPF] -=- Version {VersionString} -=- {title}";
            WindowTitle.Content = Title;
        }
    }
}
