namespace WindowsFormsApp2
{
    partial class FormConnecting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.Log = new System.Windows.Forms.ListBox();
            this.LoadingAnimation = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingAnimation)).BeginInit();
            this.SuspendLayout();
            //
            // Log
            //
            this.Log.FormattingEnabled = true;
            this.Log.Location = new System.Drawing.Point(94, 8);
            this.Log.Name = "Log";
            this.Log.Size = new System.Drawing.Size(221, 82);
            this.Log.TabIndex = 1;
            //
            // LoadingAnimation
            //
            this.LoadingAnimation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.LoadingAnimation.Image = global::Client.Properties.Resources.Loading;
            this.LoadingAnimation.Location = new System.Drawing.Point(8, 8);
            this.LoadingAnimation.Name = "LoadingAnimation";
            this.LoadingAnimation.Size = new System.Drawing.Size(80, 82);
            this.LoadingAnimation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.LoadingAnimation.TabIndex = 2;
            this.LoadingAnimation.TabStop = false;
            //
            // FormConnecting
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 96);
            this.Controls.Add(this.LoadingAnimation);
            this.Controls.Add(this.Log);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConnecting";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connecting";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LoadingAnimation)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox Log;
        private System.Windows.Forms.PictureBox LoadingAnimation;
    }
}

