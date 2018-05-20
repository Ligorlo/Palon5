using System;
using System.Windows.Forms;

namespace ToCreate
{
    /// <summary>
    /// Form6 - форма проверки пароля, если устройства нет поблизости 
    /// </summary>
    public partial class NotFoundCheckingPassword :  MetroFramework.Forms.MetroForm
    {
        // поле хранящее информацию ключа
        ToCode forpas;
        // проверка правильности пароля
        bool pasbool = false;
        // свойство проверки правильности
        public bool Pasbool
        {
            get { return pasbool; }
        }
        public NotFoundCheckingPassword(ToCode cod)
        {
            forpas = cod;
            InitializeComponent();
        }
        // кнопка проверки пароля
        private void Ready_Click(object sender, EventArgs e)
        {
            // проверка пароля
            if(textBox1.Text != ""&& forpas.Password == textBox1.Text)
            {
                // верный пароль 
                pasbool = true;
                this.Close();
            }
            else
            {
                // неверный пароль
                label1.Text = "Incorrect password";
            }
        }
    }
    
}
