using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class MainWindow : Form
    {
        public static string VersionString = "v1.0.0";
        public TcpClient clientSocket = new TcpClient();
        NetworkStream serverStream;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (TextInputBox.Text.Contains("\n") && TextInputBox.TextLength >= 1) {

                byte[] outStream = Encoding.Unicode.GetBytes(TextInputBox.Text.TrimEnd('\n'));
                serverStream.Write(outStream, 0, outStream.Length);

                TextInputBox.ResetText();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {


        }
        public void setTitle(string title)
        {
            Text = $"TheArcaneChat -=- Version {VersionString} -=- {title}";
            Title.Text = Text;
            if (false /*hasConsole*/)
            {
                Console.Title = "Debug Output | " + title;
            }
            Console.WriteLine("Set the window title to: " + title);
        }
        private void shutdown()
        {
            var shutdownTask = new Task(() =>
            {
                Notification.Visible = false;
                var outStream = Encoding.Unicode.GetBytes("\0CLIMSG\0exit");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Close(1000);
            });
                shutdownTask.Start();
            AllowTransparency = true;
            for (var i = 1.0f; i > 0.0; i -= 0.00005f * 250/*0.00005f*/)
            {
                Opacity = i;
                //Console.WriteLine(i + "");
                Thread.Sleep(1000 / 120);
            }

            shutdownTask.Wait();
            Application.Exit();
            Close();
            Environment.Exit(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var formConnecting = new FormConnecting(this);
            formConnecting.Show();
            formConnecting.TopLevel = true;
            new Thread(()=> {
                while(true)
                try
                {
                    Thread.Sleep(100);
                    if (serverStream == null) continue;
                    var inStream = new List<byte>();
                    var returndata = "";
                    while (!returndata.Contains("\0MSGEND\0"))
                    {
                        var inBytes = new byte[1];
                        serverStream.Read(inBytes, 0, 1);
                        inStream.AddRange(inBytes);
                        returndata = Encoding.Unicode.GetString(inStream.ToArray());
                    }
                    returndata = returndata.Replace("\0MSGEND\0","");
                    Invoke(new MethodInvoker(() =>
                    {
                        MessageLog.Items.Add(returndata);
                    }));
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
            Console.WriteLine("INIT");
            try
            {
#if DEBUG
                clientSocket.Connect("127.0.0.1", 8888);
#else
            clientSocket.Connect("TheArcaneBrony.ddns.net", 8888);
#endif
                setTitle($"Connected as {username}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                setTitle("(Connection failed!)");
            }


            serverStream = clientSocket.GetStream();
            var outStream = Encoding.Unicode.GetBytes("/nick " + username);
            serverStream.Write(outStream, 0, outStream.Length);

        }
        protected override void OnMouseDown(MouseEventArgs e)

        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                Capture = false;
                var msg = Message.Create(Handle, 0XA1, new IntPtr(2), IntPtr.Zero);
                WndProc(ref msg);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (DebugCheckbox.CheckState == CheckState.Checked)
            {
                Width = 876;
            }
            else if (DebugCheckbox.CheckState == CheckState.Unchecked)
            {
                Width = 552;
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            shutdown();
        }
    }
}
