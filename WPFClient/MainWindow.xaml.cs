using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Application = System.Windows.Forms.Application;
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
            ConnectingWindow cw = new ConnectingWindow(this);
            cw.Show();
            Visibility = Visibility.Hidden;
            Connection();
            Closing += MainWindow_Closing;
            CloseButton.MouseUp += (sender, args) => { Shutdown(); };
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
         Shutdown();
        }

        public void Shutdown()
        {/*
            var animation = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(2),
                FillBehavior = FillBehavior.Stop,
            };

            animation.Completed += (s, a) => Opacity = 0;

            Titlebar.BeginAnimation(OpacityProperty, animation);*/
            DoubleAnimation animation = new DoubleAnimation();
            animation.To = 0;
            //animation.From = 1;
            animation.Duration = TimeSpan.FromMilliseconds(3000);
            animation.EasingFunction = new QuadraticEase();

            Storyboard sb = new Storyboard();
            sb.Children.Add(animation);

            Titlebar.Opacity = 1;
            Titlebar.Visibility = Visibility.Visible;

            Storyboard.SetTarget(sb, Titlebar);
            Storyboard.SetTargetProperty(sb, new PropertyPath(System.Windows.Controls.Control.OpacityProperty));

            sb.Begin();

            sb.Completed += delegate (object sender, EventArgs e)
            {
                Titlebar.Visibility = Visibility.Collapsed;
            };
            Task shutdownTask = new Task(() =>
            {
                byte[] outStream = System.Text.Encoding.Unicode.GetBytes("\0CLIMSG\0exit");
                serverStream.WriteAsync(outStream, 0, outStream.Length).GetAwaiter().GetResult();
                serverStream.Close(1000);
               /* Dispatcher.Invoke(() => {
                    for (float i = 1.0f; i > 0.0; i -= 0.00005f * 250/*0.00005f*///)
                   /* {
                        Opacity = i;
                        InvalidateVisual();
                        UpdateLayout();
                        //Console.WriteLine(i + "");
                        Thread.Sleep(1000 / 120);
                    }
                });*/
            });
            shutdownTask.Start();

            
        
            //Thread.Sleep(3000);
            shutdownTask.Wait(5000);
            Application.Exit();
            //Close();
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
        public async void Init(string username)
        {
            System.Console.WriteLine("INIT");
            try
            {
#if DEBUG
                clientSocket.Connect("127.0.0.1", 8888);
#else
            clientSocket.Connect("TheArcaneBrony.ddns.net", 8888);
#endif
                SetTitle($"Connected as {username}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                SetTitle("Connection failed!");
            }

            serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.Unicode.GetBytes("/nick " + username);
            await serverStream.WriteAsync(outStream, 0, outStream.Length);
        }
        public void SetTitle(string title)
        {
          //  Title = $"TheArcaneChat -=- Version {VersionString} -=- {title}";
            //title.Text = Title;
            if (false /*hasConsole*/)
            {
                Console.Title = "Debug Output | " + title;
            }
            Console.WriteLine("Set the window title to: " + title);
        }

        // Microsoft.Win32.UserPreferenceCategory



        //WIN API CODE


       // Microsoft.Win32.SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;

        private void SystemEvents_UserPreferenceChanged(object sender, Microsoft.Win32.UserPreferenceChangedEventArgs e)
        {
            if (e.Category == Microsoft.Win32.UserPreferenceCategory.General)
            {
                // your code here, compare saved theme color with current one
            }
        }

        private const int WM_DWMCOMPOSITIONCHANGED = 0x31E;
        private const int WM_THEMECHANGED = 0x031A;
        private IntPtr hwnd;
        private HwndSource hsource;

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            if ((hwnd = new WindowInteropHelper(this).Handle) == IntPtr.Zero)
            {
                throw new InvalidOperationException("Could not get window handle.");
            }

            hsource = HwndSource.FromHwnd(hwnd);
            hsource.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_DWMCOMPOSITIONCHANGED: // Define this as 0x31A
                case WM_THEMECHANGED:          // Define this as 0x31E

                    // Respond to DWM being enabled/disabled or system theme being changed


                    return IntPtr.Zero;

                default:
                    return IntPtr.Zero;
            }
        }
    }
    internal static class NativeMethods
    {
        [DllImport("dwmapi.dll", EntryPoint = "#127")]
        internal static extern void DwmGetColorizationParameters(ref DWMCOLORIZATIONPARAMS fuck);
    }

    public struct DWMCOLORIZATIONPARAMS
    {
        public uint ColorizationColor,
            ColorizationAfterglow,
            ColorizationColorBalance,
            ColorizationAfterglowBalance,
            ColorizationBlurBalance,
            ColorizationGlassReflectionIntensity,
            ColorizationOpaqueBlend;
    }
}
