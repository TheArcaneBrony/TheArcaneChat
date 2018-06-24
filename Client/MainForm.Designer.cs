using System.ComponentModel;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.Notification = new System.Windows.Forms.NotifyIcon(this.components);
            this.MessageLog = new System.Windows.Forms.ListBox();
            this.DebugCheckbox = new System.Windows.Forms.CheckBox();
            this.TextInputBox = new System.Windows.Forms.TextBox();
            this.DebugLog = new System.Windows.Forms.ListBox();
            this.Title = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Notification
            // 
            this.Notification.Icon = ((System.Drawing.Icon)(resources.GetObject("Notification.Icon")));
            this.Notification.Text = "TheArcaneChat";
            this.Notification.Visible = true;
            this.Notification.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // MessageLog
            // 
            this.MessageLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MessageLog.FormattingEnabled = true;
            this.MessageLog.HorizontalScrollbar = true;
            this.MessageLog.IntegralHeight = false;
            this.MessageLog.Location = new System.Drawing.Point(9, 81);
            this.MessageLog.Name = "MessageLog";
            this.MessageLog.Size = new System.Drawing.Size(535, 186);
            this.MessageLog.TabIndex = 1;
            // 
            // DebugCheckbox
            // 
            this.DebugCheckbox.AutoSize = true;
            this.DebugCheckbox.Location = new System.Drawing.Point(486, 58);
            this.DebugCheckbox.Name = "DebugCheckbox";
            this.DebugCheckbox.Size = new System.Drawing.Size(58, 17);
            this.DebugCheckbox.TabIndex = 2;
            this.DebugCheckbox.Text = "Debug";
            this.DebugCheckbox.UseVisualStyleBackColor = true;
            this.DebugCheckbox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // TextInputBox
            // 
            this.TextInputBox.Location = new System.Drawing.Point(8, 274);
            this.TextInputBox.Multiline = true;
            this.TextInputBox.Name = "TextInputBox";
            this.TextInputBox.Size = new System.Drawing.Size(536, 26);
            this.TextInputBox.TabIndex = 0;
            this.TextInputBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // DebugLog
            // 
            this.DebugLog.FormattingEnabled = true;
            this.DebugLog.Location = new System.Drawing.Point(555, 81);
            this.DebugLog.Name = "DebugLog";
            this.DebugLog.Size = new System.Drawing.Size(297, 186);
            this.DebugLog.TabIndex = 3;
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Location = new System.Drawing.Point(5, 9);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(228, 13);
            this.Title.TabIndex = 4;
            this.Title.Text = "TheArcaneChat -=- Version 1.0.0 -=- LOADING";
            // 
            // CloseButton
            // 
            this.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseButton.Location = new System.Drawing.Point(524, 5);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(20, 20);
            this.CloseButton.TabIndex = 5;
            this.CloseButton.Text = "X";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 308);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.DebugLog);
            this.Controls.Add(this.DebugCheckbox);
            this.Controls.Add(this.MessageLog);
            this.Controls.Add(this.TextInputBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainWindow";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "The Arcane Chat";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private NotifyIcon Notification;
        private ListBox MessageLog;
        private CheckBox DebugCheckbox;
        private TextBox TextInputBox;
        private ListBox DebugLog;
        private Label Title;
        private Button CloseButton;
    }
}

