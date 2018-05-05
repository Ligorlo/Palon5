﻿using System;
using System.Windows.Forms;
namespace ToCreate
{
    /// <summary>
    /// Начальный класс форм1, где пользоватеь выбирает, что делать с файлом
    /// </summary>
    public partial class Form1 : Form
    {
        // массив пути к файл, если программа была открыта не на прямую
        string[] assosiatedfile;
        public Form1(string[] args)
        {
            this.assosiatedfile = args;
            //args = new string[1];args[0] = @"\\Mac\Home\Desktop\P.code4";
            // args - возможный путь
            //( который появляется при открытии файла code3, и включается раскодирование)
            // если же это пустой массив, то открывается сама программа кодирования
            // запуск основной программы
            if (args.Length == 0)
            {
                // инициализация мнтерфейса
                InitializeComponent();
            }
            // запуск программы разкодирования
            else
            {
                // поиск устройства и декодирование 
                this.Hide();
                // форма  поиска устройства и декодирования
                Form4 Decode = new Form4(args);
                Decode.ShowDialog();
                //this.Close();
                this.Shown += new EventHandler(MyForm_CloseOnStart);
            }
        }
        // метод закрытия формы в конструкторе нельзя пропитсать this.Close();
        private void MyForm_CloseOnStart(object sender, EventArgs e)
        {
            this.Close();
        }
        // кнопка освобождения (Redeem) или декодирования файла без обратного кодирования
        private void Redeembutton_Click(object sender, EventArgs e)
        {
            // путь к выбранному файлу
            string redeempath = "";
            // расширение файла даолжно быть code3
            openFileDialog1.Filter = "(*.code3)|*.code3";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                redeempath = openFileDialog1.FileName;
                openFileDialog1.Dispose();
                this.Hide();
                // массив из двух элементов для того, чтобы далее можно было понять 
                // нужно ли далее создавать возможность обратного кодирования
                assosiatedfile = new string[2];
                assosiatedfile[0] = redeempath;
                assosiatedfile[1] = "";
                // вызов формы декодирования
                Form4 Decode = new Form4(assosiatedfile);
                Decode.ShowDialog();
                this.Close();
            }
        }
        // кнопка захвата (Capture) кодирования файла
        private void Capture1_Click(object sender, EventArgs e)
        {
            // путь выбранный пользователем
            string path = "";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog2.FileName;
                openFileDialog2.Dispose();
                if (path != null)
                {
                    // форма поиска и выбора устройства
                    Form2 getbluetoothpas = new Form2(path, true);
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
        // кнопка кодирования папки
        private void Capture_Folder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog1.SelectedPath;
                // поиск устройства
                Form2 Finddevice = new Form2(path, false);
                Finddevice.ShowDialog();
            }
        }
        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {
        }
        // кнопка декодирования папки
        private void RedeemFolderbutton_Click(object sender, EventArgs e)
        {
            // путь к выбранному файлу
            string redeempath = "";
            // расширение файла даолжно быть code4
            openFileDialog3.Filter = "(*.code4)|*.code4";
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                redeempath = openFileDialog3.FileName;
                openFileDialog3.Dispose();
                this.Hide();
                // массив из двух элементов для того, чтобы далее можно было понять 
                // нужно ли далее создавать возможность обрантного кодирования
                assosiatedfile = new string[2];
                assosiatedfile[0] = redeempath;
                assosiatedfile[1] = "";
                // вызов формы декодирования
                Form4 Decode = new Form4(assosiatedfile);
                Decode.ShowDialog();
                this.Close();
            }
        }
        // кнопка справок
        private void Information_Click(object sender, EventArgs e)
        {

        }
    }
}
