namespace ToCreate
{
    partial class Startfrom1
    { 
       
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Startfrom1));
            this.Redeembutton = new System.Windows.Forms.Button();
            this.Capturing = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.Capture_folder = new System.Windows.Forms.Button();
            this.RedeemFolder = new System.Windows.Forms.Button();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog3 = new System.Windows.Forms.OpenFileDialog();
            this.Information = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // Redeembutton
            // 
            this.Redeembutton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.Redeembutton.Location = new System.Drawing.Point(12, 130);
            this.Redeembutton.Name = "Redeembutton";
            this.Redeembutton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Redeembutton.Size = new System.Drawing.Size(149, 41);
            this.Redeembutton.TabIndex = 3;
            this.Redeembutton.Text = "Redeem File";
            this.Redeembutton.UseVisualStyleBackColor = false;
            this.Redeembutton.Click += new System.EventHandler(this.Redeembutton_Click);
            // 
            // Capturing
            // 
            this.Capturing.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Capturing.Location = new System.Drawing.Point(12, 63);
            this.Capturing.Name = "Capturing";
            this.Capturing.Size = new System.Drawing.Size(149, 41);
            this.Capturing.TabIndex = 2;
            this.Capturing.Text = "Capture File";
            this.Capturing.UseVisualStyleBackColor = false;
            this.Capturing.Click += new System.EventHandler(this.Capture1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Capture_folder
            // 
            this.Capture_folder.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Capture_folder.Location = new System.Drawing.Point(193, 63);
            this.Capture_folder.Name = "Capture_folder";
            this.Capture_folder.Size = new System.Drawing.Size(149, 41);
            this.Capture_folder.TabIndex = 4;
            this.Capture_folder.Text = "Capture Folder";
            this.Capture_folder.UseVisualStyleBackColor = false;
            this.Capture_folder.Click += new System.EventHandler(this.Capture_Folder_Click);
            // 
            // RedeemFolder
            // 
            this.RedeemFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.RedeemFolder.Location = new System.Drawing.Point(193, 130);
            this.RedeemFolder.Name = "RedeemFolder";
            this.RedeemFolder.Size = new System.Drawing.Size(149, 41);
            this.RedeemFolder.TabIndex = 5;
            this.RedeemFolder.Text = "Redeem Folder";
            this.RedeemFolder.UseVisualStyleBackColor = false;
            this.RedeemFolder.Click += new System.EventHandler(this.RedeemFolderbutton_Click);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // openFileDialog3
            // 
            this.openFileDialog3.FileName = "openFileDialog3";
            // 
            // Information
            // 
            this.Information.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Information.BackgroundImage")));
            this.Information.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Information.Location = new System.Drawing.Point(290, 204);
            this.Information.Name = "Information";
            this.Information.Size = new System.Drawing.Size(27, 26);
            this.Information.TabIndex = 6;
            this.Information.UseVisualStyleBackColor = true;
            this.Information.Click += new System.EventHandler(this.Information_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 265);
            this.Controls.Add(this.Information);
            this.Controls.Add(this.RedeemFolder);
            this.Controls.Add(this.Capture_folder);
            this.Controls.Add(this.Redeembutton);
            this.Controls.Add(this.Capturing);
            this.MaximumSize = new System.Drawing.Size(370, 265);
            this.MinimumSize = new System.Drawing.Size(300, 265);
            this.Name = "Form1";
            this.Text = "Palon";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Redeembutton;
        private System.Windows.Forms.Button Capturing;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button Capture_folder;
        private System.Windows.Forms.Button RedeemFolder;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.OpenFileDialog openFileDialog3;
        private System.Windows.Forms.Button Information;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

