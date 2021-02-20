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
            this.ConnectionCount = new System.Windows.Forms.Label();
            this.CPBTest = new CircularProgressBar.CircularProgressBar();
            this.LogBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            //
            // ConnectionCount
            //
            this.ConnectionCount.AutoSize = true;
            this.ConnectionCount.Location = new System.Drawing.Point(12, 9);
            this.ConnectionCount.Name = "ConnectionCount";
            this.ConnectionCount.Size = new System.Drawing.Size(104, 13);
            this.ConnectionCount.TabIndex = 0;
            this.ConnectionCount.Text = "Connected clients: 0";
            this.ConnectionCount.Click += new System.EventHandler(this.Label1_Click);
            //
            // CPBTest
            //
            this.CPBTest.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.Liner;
            this.CPBTest.AnimationSpeed = 500;
            this.CPBTest.BackColor = System.Drawing.Color.Transparent;
            this.CPBTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CPBTest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.CPBTest.InnerColor = System.Drawing.SystemColors.Control;
            this.CPBTest.InnerMargin = 2;
            this.CPBTest.InnerWidth = -1;
            this.CPBTest.Location = new System.Drawing.Point(460, -1);
            this.CPBTest.MarqueeAnimationSpeed = 2000;
            this.CPBTest.Name = "CPBTest";
            this.CPBTest.OuterColor = System.Drawing.SystemColors.Control;
            this.CPBTest.OuterMargin = -25;
            this.CPBTest.OuterWidth = 26;
            this.CPBTest.ProgressColor = System.Drawing.SystemColors.MenuHighlight;
            this.CPBTest.ProgressWidth = 30;
            this.CPBTest.SecondaryFont = new System.Drawing.Font("Microsoft Sans Serif", 36F);
            this.CPBTest.Size = new System.Drawing.Size(32, 32);
            this.CPBTest.StartAngle = -90;
            this.CPBTest.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.CPBTest.SubscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.CPBTest.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
            this.CPBTest.SubscriptText = ".23";
            this.CPBTest.SuperscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.CPBTest.SuperscriptMargin = new System.Windows.Forms.Padding(10, 35, 0, 0);
            this.CPBTest.SuperscriptText = "°C";
            this.CPBTest.TabIndex = 0;
            this.CPBTest.TextMargin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.CPBTest.Value = 1;
            //
            // LogBox
            //
            this.LogBox.FormattingEnabled = true;
            this.LogBox.Location = new System.Drawing.Point(12, 37);
            this.LogBox.Name = "LogBox";
            this.LogBox.Size = new System.Drawing.Size(467, 199);
            this.LogBox.TabIndex = 1;
            //
            // GarbageWindow
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 245);
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.CPBTest);
            this.Controls.Add(this.ConnectionCount);
            this.Name = "GarbageWindow";
            this.Text = "Server";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GarbageWindow_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private CircularProgressBar.CircularProgressBar CPBTest;
        private Label ConnectionCount;
        private ListBox LogBox;
    }
}
