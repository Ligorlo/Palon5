using System;
using System.Windows.Forms;
namespace ToCreate
{
    // Начальный класс
    public partial class Form1 : Form
    {
        string[] args;
        public Form1(string[] args)
        {
            this.args = args;
          //args = new string[1];args[0] = @"\\Mac\Home\Desktop\р.code3";
            // args - возможный путь
            //( который появляется при открытии файла code3, и вклчается раскодирование)
            // если же это пустой массив, то открывается сама программа кодирования
            // запуск основной программы
            if (args.Length == 0)
            {
                InitializeComponent();
            }
            // запуск программы разкодирования
            else
            {
                this.Hide();
                Form4 Decode = new Form4(args);
                Decode.ShowDialog();
                this.Close();
            }
        }
       
        private void button2_Click(object sender, EventArgs e)
        {
            string redeempath = "";
            openFileDialog1.Filter = "(*.code3)|*.code3";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                redeempath = openFileDialog1.FileName;

                openFileDialog1.Dispose();
                this.Hide();
                args = new string[2];
                args[0] = redeempath;
                args[1] = "";
                Form4 Decode = new Form4(args);
                Decode.ShowDialog();
                this.Close();
            }
        }

        private void Capture1_Click(object sender, EventArgs e)
        {
            // путь выбранный пользователем
            string path = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog1.FileName;

                openFileDialog1.Dispose();
                if (path != null)
                {
                    // форма поиска и выбора устройства
                    Form2 getbluetoothpas = new Form2(path);
                    getbluetoothpas.ShowDialog();
                    //this.Close();
                }
                else
                {
                    MessageBox.Show("Невозможно открыть данный файл");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
