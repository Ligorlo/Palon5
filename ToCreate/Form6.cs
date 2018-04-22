using System;
using System.Windows.Forms;

namespace ToCreate
{
    // форма ввода пароля
    public partial class Form6 : Form
    {
        ToCode forpas;
        // проверка правильности пароля
        bool pasbool = false;
        // свойство проверки правильности
        public bool Pasbool
        {
            get { return pasbool; }
        }
        public Form6(ToCode cod)
        {
            forpas = cod;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != ""&& forpas.Password == textBox1.Text)
            {
                pasbool = true;
                this.Close();
            }
            else
            {
                label1.Text = "Incorrect password";
            }
        }
    }
    
}
