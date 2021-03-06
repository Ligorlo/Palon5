﻿namespace ToCreate
{
    partial class DiscoverBLEDevices2
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
            this.FoundDevices = new System.Windows.Forms.ListBox();
            this.Indexer = new System.Windows.Forms.Label();
            this.Searchdevice = new System.Windows.Forms.Button();
            this.Choosefromused = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // FoundDevices
            // 
            this.FoundDevices.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.FoundDevices.FormattingEnabled = true;
            this.FoundDevices.Location = new System.Drawing.Point(23, 63);
            this.FoundDevices.Name = "FoundDevices";
            this.FoundDevices.Size = new System.Drawing.Size(269, 173);
            this.FoundDevices.TabIndex = 0;
            this.FoundDevices.DoubleClick += new System.EventHandler(this.FoundDevices_DoubleClick);
            // 
            // Indexer
            // 
            this.Indexer.AutoSize = true;
            this.Indexer.Enabled = false;
            this.Indexer.Location = new System.Drawing.Point(21, 250);
            this.Indexer.Name = "Indexer";
            this.Indexer.Size = new System.Drawing.Size(55, 13);
            this.Indexer.TabIndex = 1;
            this.Indexer.Text = "Searching";
            // 
            // Searchdevice
            // 
            this.Searchdevice.Location = new System.Drawing.Point(347, 63);
            this.Searchdevice.Name = "Searchdevice";
            this.Searchdevice.Size = new System.Drawing.Size(137, 41);
            this.Searchdevice.TabIndex = 2;
            this.Searchdevice.Text = "Start";
            this.Searchdevice.UseVisualStyleBackColor = true;
            this.Searchdevice.Click += new System.EventHandler(this.Code_Click);
            // 
            // Choosefromused
            // 
            this.Choosefromused.Location = new System.Drawing.Point(347, 135);
            this.Choosefromused.Name = "Choosefromused";
            this.Choosefromused.Size = new System.Drawing.Size(137, 41);
            this.Choosefromused.TabIndex = 3;
            this.Choosefromused.Text = "Already used";
            this.Choosefromused.UseVisualStyleBackColor = true;
            this.Choosefromused.Click += new System.EventHandler(this.Already_used_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(24, 266);
            this.progressBar1.Maximum = 13;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 23);
            this.progressBar1.TabIndex = 4;
            this.progressBar1.Visible = false;
            // 
            // DiscoverBLEDevices2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 335);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Choosefromused);
            this.Controls.Add(this.Searchdevice);
            this.Controls.Add(this.Indexer);
            this.Controls.Add(this.FoundDevices);
            this.Name = "DiscoverBLEDevices2";
            this.Text = "Bluetooth";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ListBox FoundDevices;
        public System.Windows.Forms.Label Indexer;
        private System.Windows.Forms.Button Searchdevice;
        private System.Windows.Forms.Button Choosefromused;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}