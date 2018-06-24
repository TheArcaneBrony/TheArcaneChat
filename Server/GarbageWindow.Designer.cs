using System.ComponentModel;
using System.Windows.Forms;

namespace Server
{
    partial class GarbageWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ConnectionCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ConnectionCount
            // 
            ConnectionCount.AutoSize = true;
            ConnectionCount.Location = new System.Drawing.Point(12, 9);
            ConnectionCount.Name = "ConnectionCount";
            ConnectionCount.Size = new System.Drawing.Size(104, 13);
            ConnectionCount.TabIndex = 0;
            ConnectionCount.Text = "Connected clients: 0";
            ConnectionCount.Click += new System.EventHandler(this.Label1_Click);
            // 
            // GarbageWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 245);
            this.Controls.Add(ConnectionCount);
            this.Name = "GarbageWindow";
            this.Text = "Server";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private static Label ConnectionCount;
    }
}

