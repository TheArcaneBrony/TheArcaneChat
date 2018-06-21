using System;
using System.Drawing;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class FormConnecting : Form
    {
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream;
        public FormConnecting()
        {
            InitializeComponent();
        }
        private MainWindow mainForm = null;
        public FormConnecting(Form callingForm)
        {
            mainForm = callingForm as MainWindow;
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.SetDesktopLocation(mainForm.Left+mainForm.Width/2-Width/2, mainForm.Top+mainForm.Height/2-Height/2);
            mainForm.Visible = true;
            //mainForm.init();
            mainForm.Refresh();
            Visible = true;
            //this.mainForm.debugListBox.Items.Add("Client connection status\nIP: "+ clientSocket.GetStream.);

           // ActiveForm.TopMost = true;
            TopMost = true;

            //Log.Items.Add(this.mainForm.clientSocket.Connected ? "Connection successful!" : "Connection failed, please check your internet connection!");
            //Log.Items.Add($"Username: {Environment.UserName}");
            Refresh();

            //Thread.Yield();
            //Thread.Sleep(1000);
            //Close();
        }

        private void Log_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void UserNameText_Click(object sender, EventArgs e)
        {

        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            mainForm.init(Username.Text);
            Log.Items.Add(this.mainForm.clientSocket.Connected ? "Connection successful!" : "Connection failed, please check your internet connection!");
            Log.Items.Add($"Username: {Username.Text}");
            Refresh();
            Thread.Yield();
            Thread.Sleep(1000);
            Close();
        }
    }
}
