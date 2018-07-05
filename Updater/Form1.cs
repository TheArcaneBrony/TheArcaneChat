using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Updater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Invoke(new MethodInvoker(delegate
            {
                if (!Directory.Exists(@"C:\Arcane\TheArcaneChat\bin"))
                {
                    Directory.CreateDirectory(@"C:\Arcane");
                    Directory.CreateDirectory(@"C:\Arcane\TheArcaneChat");
                    Directory.CreateDirectory(@"C:\Arcane\TheArcaneChat\bin");
                }

                try
                {
                    var files = Directory.GetFiles(@"C:\TheArcaneChat\bin");
                    foreach (var file in files)
                    {
                        File.Delete(file);
                        progressBar1.Value += 25 / files.Length;
                    }
                }
                catch
                {
                }

                var webClient = new WebClient();
                webClient.DownloadFile("http://arcanebrony.ddns.net/TheArcaneChatSFX.exe",
                    @"C:\Arcane\TheArcaneChat\UpdateInstaller.exe");
                progressBar1.Value = 50;
                var psi = new ProcessStartInfo(@"C:\Arcane\TheArcaneChat\UpdateInstaller.exe");
                psi.Arguments = "-y";
                psi.UseShellExecute = true;
                psi.WorkingDirectory = @"C:\Arcane\TheArcaneChat\bin\";
                Process.Start(psi).WaitForExit();
                progressBar1.Value = 100;
                Process.Start(@"C:\Arcane\TheArcaneChat\bin\WPFClient.exe");
                progressBar1.Visible = true;
                Application.Exit();
            }));
        }
    }
}