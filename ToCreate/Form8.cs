using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace ToCreate
{
    /// <summary>
    /// Форма справок
    /// </summary>
    public partial class Back : Form
    {
        public Back()
        {
            Bitmap MyImage;
            // панель с информцией
            InitializeComponent();
            panel1.AutoScroll = true;
             MyImage = new Bitmap($"{Directory.GetCurrentDirectory()}/inf.png");
           pictureBox1.Image = (Image)MyImage;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
        }
        // кнопка возвращения
        private void Back_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
