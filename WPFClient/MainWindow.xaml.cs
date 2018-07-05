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
                {
                    Thread.Sleep(10);
                    try
                    {
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

                        if (returndata.StartsWith("\0SRVMSG\0"))
                        {
                            switch (returndata.Replace("\0SRVMSG\0", ""))
                            {
                                case "ping":
                                   // Send("\0CLIMSG\0ping");
                                    break;
                                case "exit":
                                    break;
                            }

                        }
                        else
                        {
                            Dispatcher.Invoke(new MethodInvoker(() =>
                            {
                                //LbChat.Items.Add(returndata);

                                var test = new Message();
                                test.Username = Username;
                                test.MessageText = returndata;

                                test.ProfilePicURL = "https://cdn.discordapp.com/avatars/84022289024159744/a_f53385b99292bb3c6cd595b197988d7a.gif?size=1024";
                                test.Path = "https://vignette.wikia.nocookie.net/goanimate-v2/images/d/df/Vector_138_pinkie_pie_9_by_dashiesparkle-d8npl5m.png/revision/latest?cb=20150424155451";
                                LbChat.Items.Add(test);
                            }));
                        }





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
                }


            }).Start();
        }
        public void Init(string username)
        {
            Username = username;
            
            try
            {
#if DEBUG
                try
                {
                    ClientSocket.Connect("127.0.0.1", 8888);
                }
                catch
                {
                    ClientSocket.Connect("thearcanebrony.ddns.net", 8888);
                }
#else
                ClientSocket.Connect("thearcanebrony.ddns.net", 8888);
#endif
                SetTitle($"Connected as {username}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                SetTitle("Connection failed!");
            }

            _serverStream = ClientSocket.GetStream();
            var outStream = Encoding.Unicode.GetBytes($"\0cmd\0logon {username}");
            _serverStream.Write(outStream, 0, outStream.Length);
        }
        public void SetTitle(string title)
        {
            Title = $"TheArcaneChat [WPF] -=- Version {VersionString} -=- {title}";
            WindowTitle.Content = Title;
        }
    }
    public class Message
    {
        public string Username { get; set; }
        public string MessageText { get; set; }
        public string ProfilePicURL { get; set; }
        public string Path { get; set; }
    }
}
