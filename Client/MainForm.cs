using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class MainWindow : Form
    {
        public static string VersionString = "v1.0.0";
        public System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (TextInputBox.Text.Contains("\n") && TextInputBox.TextLength >= 1) {

                byte[] outStream = System.Text.Encoding.Unicode.GetBytes(TextInputBox.Text.TrimEnd('\n'));
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
            Task shutdownTask = new Task(() =>
            {
                Notification.Visible = false;
                byte[] outStream = System.Text.Encoding.Unicode.GetBytes("\0CLIMSG\0exit");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Close(1000);
            });
                shutdownTask.Start();
            AllowTransparency = true;
            for (float i = 1.0f; i > 0.0; i -= 0.00005f * 250/*0.00005f*/)
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
            FormConnecting formConnecting = new FormConnecting(this);
            formConnecting.Show();
            formConnecting.TopLevel = true;
            Thread thread = new Thread(()=> {
                while(true)
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
                        string returndata = System.Text.Encoding.Unicode.GetString(inStream.ToArray()).Replace("\0MSGEND\0","");
                    Invoke(new MethodInvoker(() =>
                    {
                        if (returndata.Contains("\n")) MessageLog.Items.Add(returndata.Replace("\0MSGEND\0", ""));
                        else MessageLog.Items.AddRange(returndata.Replace("\0MSGEND\0", "").Split("\n".ToCharArray()));

                    }));
                    //MessageLog.Refresh();
                    MessageLog.MultiColumn = true;
                    Console.WriteLine(returndata);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
            thread.Start();
        }
        public void init(string username)
        {
            System.Console.WriteLine("INIT");
            try
            {
#if DEBUG
                // small delay to wait on server when debugging.
                //Thread.Sleep(100);
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


            this.serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.Unicode.GetBytes("/nick " + username);
            serverStream.Write(outStream, 0, outStream.Length);

        }
        protected override void OnMouseDown(MouseEventArgs e)

        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                this.Capture = false;
                Message msg = Message.Create(this.Handle, 0XA1, new IntPtr(2), IntPtr.Zero);
                this.WndProc(ref msg);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            switch (DebugCheckbox.CheckState)
            {
                case CheckState.Checked:
                    MainWindow.ActiveForm.Width = 876;
                    break;
                case CheckState.Unchecked:
                    MainWindow.ActiveForm.Width = 552;
                    break;
                default:
                    break;
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            shutdown();
        }
    }
}
