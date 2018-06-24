using System.ComponentModel;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    partial class FormConnecting
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
            this.Log = new System.Windows.Forms.ListBox();
            this.LoadingAnimation = new System.Windows.Forms.PictureBox();
            this.UserNameText = new System.Windows.Forms.Label();
            this.LoginButton = new System.Windows.Forms.Button();
            this.Username = new System.Windows.Forms.TextBox();
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
            this.Log.SelectedIndexChanged += new System.EventHandler(this.Log_SelectedIndexChanged);
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
            // UserNameText
            // 
            this.UserNameText.AutoSize = true;
            this.UserNameText.Location = new System.Drawing.Point(8, 102);
            this.UserNameText.Name = "UserNameText";
            this.UserNameText.Size = new System.Drawing.Size(58, 13);
            this.UserNameText.TabIndex = 3;
            this.UserNameText.Text = "Username:";
            this.UserNameText.Click += new System.EventHandler(this.UserNameText_Click);
            // 
            // LoginButton
            // 
            this.LoginButton.Location = new System.Drawing.Point(259, 97);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(56, 22);
            this.LoginButton.TabIndex = 4;
            this.LoginButton.Text = "Log in";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // Username
            // 
            this.Username.Location = new System.Drawing.Point(72, 98);
            this.Username.Name = "Username";
            this.Username.Size = new System.Drawing.Size(181, 20);
            this.Username.TabIndex = 5;
            // 
            // FormConnecting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 127);
            this.Controls.Add(this.Username);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.UserNameText);
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
            this.PerformLayout();

        }

        #endregion
        private ListBox Log;
        private PictureBox LoadingAnimation;
        private Label UserNameText;
        private Button LoginButton;
        private TextBox Username;
    }
}

