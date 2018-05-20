namespace ToCreate
{
    partial class EncryptBack5
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
            this.Encryptback = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Encryptback
            // 
            this.Encryptback.Location = new System.Drawing.Point(20, 54);
            this.Encryptback.Name = "Encryptback";
            this.Encryptback.Size = new System.Drawing.Size(171, 55);
            this.Encryptback.TabIndex = 0;
            this.Encryptback.Text = "Encrypt again";
            this.Encryptback.UseVisualStyleBackColor = true;
            this.Encryptback.Click += new System.EventHandler(this.Encryptback_Click);
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(214, 120);
            this.Controls.Add(this.Encryptback);
            this.MaximumSize = new System.Drawing.Size(214, 120);
            this.MinimumSize = new System.Drawing.Size(214, 120);
            this.Name = "Form5";
            this.Text = "Shifr";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form5_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Encryptback;
    }
}