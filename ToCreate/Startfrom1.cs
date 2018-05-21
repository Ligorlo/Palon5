using System;
using System.Windows.Forms;
using MetroFramework.Forms;
using MetroFramework.Components;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
namespace ToCreate
{
    /// <summary>
    /// Начальный класс Startfrom1, где пользоватеь выбирает, что делать с файлом
    /// </summary>
    public partial class Startfrom1 : MetroFramework.Forms.MetroForm
    {
        // массив пути к файлу, если программа была открыта не на прямую
       string[] assosiatedfile;
        public Startfrom1(string[] args)
        {
            // проверка есть ли журнал
           if(File.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Using.txt"))
            {
                try
                {
                    FileStream str = new FileStream($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Using.txt", FileMode.Open);
                    DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(Together));
                    Together Tojournal = (Together)json.ReadObject(str);
                    // массив запущенных процессов
                    Process[] pr = Process.GetProcesses();
                    str.Close();
                    // проверка числа запущенных программ PALON
                    int myprogramm = 0;
                    foreach (var item in pr)
                    {
                        if (item.ProcessName == "ToCreate")
                        {
                            myprogramm++;
                        }
                    }
                    // проверка числа запущенных программ PALON
                    if (myprogramm <= Tojournal.mas.Length)
                    {
                        for (int i = 0; i < Tojournal.mas.Length; i++)
                        {
                            // проверка существует ли нужный нам файл
                            if (File.Exists(Tojournal.mas[i].path))
                            {
                                // кодируем файл
                                Passwordandencrypt3 extra = new Passwordandencrypt3(Tojournal.mas[i].path, "", Tojournal.mas[i].adr, Tojournal.mas[i].direct);
                            }
                        }
                        // удаляем
                        File.Delete($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Using.txt");
                    }
                }
                // исключение при десериализации
                catch (Exception ex)
                {
                    MessageBox.Show("Source File was changed");
                    this.Close();
                }
            }
            this.assosiatedfile = args;
            //args = new string[1];args[0] = @"\\Mac\Home\Desktop\ваваав.code4";
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
                CheckAndDecrypt Decode = new CheckAndDecrypt(args);
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
                // путь к файлу, с которым будем работать 
                redeempath = openFileDialog1.FileName;
                openFileDialog1.Dispose();
                this.Hide();
                // массив из двух элементов для того, чтобы далее можно было понять 
                // нужно ли далее создавать возможность обратного кодирования
                assosiatedfile = new string[2];
                assosiatedfile[0] = redeempath;
                assosiatedfile[1] = "";
                // вызов формы декодирования
                CheckAndDecrypt Decode = new CheckAndDecrypt(assosiatedfile);
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
                    DiscoverBLEDevices2 getbluetoothpas = new DiscoverBLEDevices2(path, true);
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
                DiscoverBLEDevices2 Finddevice = new DiscoverBLEDevices2(path, false);
                Finddevice.ShowDialog();
            }
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
                CheckAndDecrypt Decode = new CheckAndDecrypt(assosiatedfile);
                Decode.ShowDialog();
                this.Close();
            }
        }
        // кнопка справок
        private void Information_Click(object sender, EventArgs e)
        {
            Information information = new Information();
            information.ShowDialog();
        }
    }
}
