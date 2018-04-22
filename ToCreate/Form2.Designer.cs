namespace ToCreate
{
    partial class Form2
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
            this.label1 = new System.Windows.Forms.Label();
            this.Code = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FoundDevices
            // 
            this.FoundDevices.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.FoundDevices.FormattingEnabled = true;
            this.FoundDevices.Location = new System.Drawing.Point(12, 12);
            this.FoundDevices.Name = "FoundDevices";
            this.FoundDevices.Size = new System.Drawing.Size(315, 225);
            this.FoundDevices.TabIndex = 0;
            this.FoundDevices.DoubleClick += new System.EventHandler(this.FoundDevices_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(21, 250);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Searching";
            // 
            // Code
            // 
            this.Code.Location = new System.Drawing.Point(371, 12);
            this.Code.Name = "Code";
            this.Code.Size = new System.Drawing.Size(137, 41);
            this.Code.TabIndex = 2;
            this.Code.Text = "Start";
            this.Code.UseVisualStyleBackColor = true;
            this.Code.Click += new System.EventHandler(this.Code_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(371, 82);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(137, 41);
            this.button2.TabIndex = 3;
            this.button2.Text = "Already used";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.ClientSize = new System.Drawing.Size(579, 373);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Code);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FoundDevices);
            this.Name = "Form2";
            this.Text = "Bluetooth";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ListBox FoundDevices;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Code;
        private System.Windows.Forms.Button button2;
    }
}