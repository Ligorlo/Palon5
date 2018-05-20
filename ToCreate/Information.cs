using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace ToCreate
{
    /// <summary>
    /// Форма справок
    /// </summary>
    public partial class Information :  MetroFramework.Forms.MetroForm
    {
        public Information()
        {
            Bitmap MyImage = null;
            // панель с информцией
            InitializeComponent();
            panel1.AutoScroll = true;
            try
            {
                MyImage = new Bitmap($"{Directory.GetCurrentDirectory()}/inf.png");
                pictureBox1.Image = (Image)MyImage;
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Can't find a picture");
            }

           
        }
        // кнопка возвращения
        private void Back_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
