using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
namespace ToCreate
{
    public partial class Form7 : Form
    {
        // переменная для понимания причины возвращения, если false значит возврат сделан для повторного поиска
        bool boo = true;
        public bool Boo
        {
            get
            {
                return boo;
            }
        }
        // путь к файлу
        string path;
        // массив 
        string[,] toconsole;
        // папка или файл
        bool boool;
        public Form7(string path, bool boool)
        {

            this.boool = boool;
            this.path = path;
            InitializeComponent();
            // проверка на месте ли ещё файл с девайсами
            if (File.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Devices.txt"))
            {
                // считываем информацию
                FileStream s = new FileStream($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Devices.txt", FileMode.Open);
                // десериализуем
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Devices[]));
                // записываем вседевайсы в listbox
                Devices[] device = (Devices[])ser.ReadObject(s);
                string[,] forlistbox = new string[device.Length, 2];
                for (int i = 0; i < forlistbox.GetLength(0); i++)
                {
                    forlistbox[i, 0] = Encoding.ASCII.GetString(device[i].name);
                    forlistbox[i, 1] = Encoding.ASCII.GetString(device[i].adress);
                    listBox1.Items.Add(forlistbox[i, 0]);
                }
                toconsole = forlistbox;
                s.Close();
            }
            else
            {
                // нет appdata на компе значит сохраняем в другом месте 
                if (File.Exists($"../Palon/Devices.txt"))
                {
                    // берем названия девайсов 
                    FileStream s = new FileStream($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Devices.txt", FileMode.Open);
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Devices[]));
                    Devices[] device = (Devices[])ser.ReadObject(s);
                    // записываем названия в listbox
                    string[,] forlistbox = new string[device.Length, 2];
                    for (int i = 0; i < forlistbox.GetLength(0); i++)
                    {
                        forlistbox[i, 0] = Encoding.ASCII.GetString(device[i].name);
                        forlistbox[i, 1] = Encoding.ASCII.GetString(device[i].adress);
                        listBox1.Items.Add(forlistbox[i, 0]);
                    }
                    toconsole = forlistbox;
                    s.Close();
                }
                else
                {
                    MessageBox.Show("Sorry, but something happened with files");
                }
            }
        }
        // нажатое утройство 
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (((ListBox)(sender)).SelectedItem != null)
            {
              string  phone = ((ListBox)(sender)).SelectedItem.ToString();
                // ищем в массиве выбраное устройство 
                for (int i = 0; i < toconsole.GetLength(0); i++)
                {
                    if(toconsole[i, 0] == phone)
                    {
                        byte[] devname = Encoding.ASCII.GetBytes(toconsole[i,0]);
                        byte[] devadress = Encoding.ASCII.GetBytes(toconsole[i, 1]);
                        int q = 0;
                        // раскодируем адрес
                        for (int j = 0; j < devadress.Length; j++)
                        {
                            if (q == devname.Length)
                                q = 0;
                            devadress[j] = (byte)(devname[q] ^ devadress[j]);
                            q++;
                        }
                        toconsole[i, 1] = Encoding.ASCII.GetString(devadress);
                        // форма ввода пароля и кодирования
                        Form3 pass = new Form3(path, toconsole[i, 0], toconsole[i, 1], boool );
                        pass.ShowDialog();
                        this.Close();
                        break;
                    }
                }
            }
        }
        // кнопка возращения к поиску
            private void Back_Click(object sender, EventArgs e)
        {
            boo = false;
            this.Close();
        }
    }
}
