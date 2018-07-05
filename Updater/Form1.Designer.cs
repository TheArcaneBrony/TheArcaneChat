namespace Updater
{
    partial class Form1
    {
        private void InitializeComponent()
        {
            Title = new System.Windows.Forms.Label();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            Title.Location = new System.Drawing.Point(-3, -2);
            Title.Size = new System.Drawing.Size(101, 13);
            Title.Text = "ArcaneChat Update";
            progressBar1.Location = new System.Drawing.Point(0, 10);
            progressBar1.Size = new System.Drawing.Size(97, 17);
            ClientSize = new System.Drawing.Size(94, 26);
            Controls.Add(Title);
            Controls.Add(progressBar1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Load += new System.EventHandler(Form1_Load);
        }
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

