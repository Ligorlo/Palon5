using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Json;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
namespace ToCreate
{
    /// <summary>
    /// Form5 - форма для шифрования после изменения
    /// </summary>
    public partial class EncryptBack5 :  MetroFramework.Forms.MetroForm
    {
        string normpath;
        // метод шифрования
        private static byte[] EncryptAES(string tocode, byte[] Key, byte[] IV)
        {
            // массив в который будет записан зашифрованный массив байтов
            byte[] encrypted;
            using (Aes aes = Aes.Create())
            {
                // ключ
                aes.Key = Key;
                // IV
                aes.IV = IV;
                // encryptor
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(csEncrypt))
                        {
                            sw.Write(tocode);
                        }
                        encrypted = ms.ToArray();
                    }
                }
            }
            // возвращаем массив байтов
            return encrypted;
        }
        // поле пути 
        string path;
        // поле ключа
        byte[] key;
        byte[] IV;
        // номер 
        int num;
        // переменная проверяющая папка ли это
        bool directory;
        // для сериализации
        ToCode cod;
        public EncryptBack5(string path, byte[] key, byte[] IV, ToCode cod, int num, bool b, bool directory, string adr, string pass)
        {

            this.directory = directory;
            // файл
            if (directory)
            {
                // проверка надо ли удалять ключ
                if (b == false)
                {
                    File.Delete($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Pakman{num}.txt");
                    //this.Close();
                }
                else
                {
                    this.IV = IV;
                    this.key = key;
                    path = Path.ChangeExtension(path, cod.Rassh);
                    this.path = path;
                    this.num = num;
                    this.cod = cod;
                    InitializeComponent();
                }
            }
            // папка
            else
            {
                // проверка надо ли удалять ключ
                if (b == false)
                {
                    File.Delete($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/PakmanD{num}.txt");
                    //this.Close();
                }
                else
                {
                    this.key = key;
                    this.IV = IV;
                    this.path = path.Remove(path.Length - 1);
                    this.num = num;
                    this.cod = cod;
                    InitializeComponent();
                }
            }
            // проверка 
            if (Directory.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon"))
            {
                // проверка существует ли журнал
                if (File.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Using.txt"))
                {
                    // добавляем наше устройство 
                    normpath = path;
                    // сериализация данных о файле и о том как мы его должны закодировать 
                    DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(Together));
                    FileStream str = null; 
                    try
                    {
                       str = new FileStream($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Using.txt", FileMode.Open);
                    }
                    catch
                    {
                        MessageBox.Show("Two processes in one time");
                        Application.Exit();
                    }
                    Together a = (Together)json.ReadObject(str);
                    str.Close();
                    File.Delete($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Using.txt");
                    Array.Resize(ref a.mas, a.mas.Length + 1);
                    a.mas[a.mas.Length - 1] = new Processonefile(adr, pass, normpath, directory);
                    str = new FileStream($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Using.txt", FileMode.Create);
                    json.WriteObject(str, a);
                    str.Close();
                }
                else
                {
                    // создаем журнал и добавляем туда первое устройство
                    normpath = path;
                    DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(Together));
                    Processonefile type = new Processonefile(adr, pass, normpath, directory);
                    // класс одного файла
                    Processonefile[] typemas = new Processonefile[1] { type };
                    // класс всех изменяемых файлов из журнала
                    Together a = new Together(typemas);
                    FileStream str = new FileStream($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Using.txt", FileMode.Create);
                    json.WriteObject(str, a);
                    str.Close();
                }
            }
            else
            {
                MessageBox.Show("Sorry, something had happened");
                this.Close();
            }
            Encryptback.Text = $"Encrypt back \n {Path.ChangeExtension(path, cod.Rassh)}";
        }
        // метод кодирования
        public void proove()
        {
            Encryptback.Enabled = false;
            // файл
            if (directory)
            {
                // прочитали
                byte[] bytearr2 = File.ReadAllBytes(path);
                File.Delete(path);
                // перевели в base64
                string Inbaseforencrypt = Convert.ToBase64String(bytearr2);
                // зашифровали
                byte[] tofile = EncryptAES(Inbaseforencrypt, key, IV);
                // засереализовали
                Connectwithakey main = new Connectwithakey(tofile, Encoding.ASCII.GetBytes(num.ToString()));
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(Connectwithakey));
                FileStream file = new FileStream(path, FileMode.OpenOrCreate);
                json.WriteObject(file, main);
                file.Close();
                bool filename = true;
                int number = 2;
                string pathcheck = path;
                // подбираем файл таким образом чтобы не было повтора
                pathcheck = Path.ChangeExtension(pathcheck, ".code3");
                if (File.Exists(pathcheck))
                {
                    while (filename)
                    {
                        pathcheck = Path.ChangeExtension(pathcheck, "");
                        pathcheck = pathcheck.Remove(pathcheck.Length - 1);
                        pathcheck = $"{pathcheck}{number}";
                        pathcheck = Path.ChangeExtension(pathcheck, ".code3");
                        if (!File.Exists(pathcheck))
                        {
                            filename = false;
                        }
                    }
                }
                File.Move(path, pathcheck);
            }
            // папка 
            else
            {
                // архивируем (подбираем свободный путь)
                string direct = Path.ChangeExtension(path, cod.Rassh);
                int number2 = 2;
                bool exist = true;
                while (exist)
                {
                    if (!File.Exists(direct))
                    {
                        ZipFile.CreateFromDirectory(path, direct);
                        exist = false;
                    }
                    else
                    {
                        direct = Path.ChangeExtension(direct, $"");
                        direct.Remove(direct.Length - 1);
                        direct = $"direct{number2}";
                        direct = Path.ChangeExtension(direct, cod.Rassh);
                        number2++;
                    }
                }
                // удаляем первоначальную папку
                Directory.Delete(path, true);
                path = direct;
                // вноь считываем 
                byte[] bytearr2 = File.ReadAllBytes(path);
                // удаляем заархивироанный файл
                File.Delete(path);
                // зашифровываем 
                string Inbaseforencrypt = Convert.ToBase64String(bytearr2);
                byte[] tofile = EncryptAES(Inbaseforencrypt, key, IV);
                Connectwithakey main = new Connectwithakey(tofile, Encoding.ASCII.GetBytes(num.ToString()));
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(Connectwithakey));
                FileStream file = new FileStream(path, FileMode.OpenOrCreate);
                // запиываем обратно 
                json.WriteObject(file, main);
                file.Close();
                // end with main file
                // start with key file
                bool filename = true;
                int number = 2;
                string pathcheck = direct;
                // подбираем файл таким образом чтобы не было повтора
                pathcheck = Path.ChangeExtension(pathcheck, ".code4");
                if (File.Exists(pathcheck))
                {
                    while (filename)
                    {
                        pathcheck = Path.ChangeExtension(pathcheck, "");
                        pathcheck = pathcheck.Remove(pathcheck.Length - 1);
                        pathcheck = $"{pathcheck}{number}";
                        pathcheck = Path.ChangeExtension(pathcheck, ".code4");
                        if (!File.Exists(pathcheck))
                        {
                            filename = false;
                        }
                    }
                }
                // переводим файл в новое расширение
                File.Move(direct, pathcheck);
            }
        }
        // кнопка шифрования
        private void Encryptback_Click(object sender, EventArgs e)
        {
            // файл или папка на месте 
            if (File.Exists(path)||Directory.Exists(path))
            {
                // метод кодирования
                proove();
                this.Close();
            }
            // файл или папка не найдена
            else
            {
                MessageBox.Show("Sorry, but we can't find your file, please put it back");
            }
        }
        // метод выполняемый при закрытии файла
        private void Form5_FormClosing(object sender, FormClosingEventArgs e)
        {
            // проверяем есть ли ещё журнал, если есть то удаляем из него устройство
            if (Directory.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon"))
            {
                
                if (File.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Using.txt"))
                {
                    // десериализуем журнал
                    try
                    { 
                        // ищем наш файл
                    DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(Together));
                    FileStream str = new FileStream($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Using.txt", FileMode.Open);
                    Together a = (Together)json.ReadObject(str);
                    str.Close();
                    File.Delete($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Using.txt");
                        // если всего один изменяемый файл в журнале то удаляем файл
                        if (a.mas.Length == 1)
                        {

                        }
                        else
                        {
                            // есть ещё изменемы файлы - удаляем данный из журнала и обновляем 
                            int count = 0;
                            for (int i = 0; i < a.mas.Length; i++)
                            {
                                if (a.mas[i].path == normpath)
                                {
                                    // сдвигаем все на один влево
                                    count++;
                                    for (int j = i; j < a.mas.Length; j++)
                                    {
                                        if (j != a.mas.Length - 1)
                                            a.mas[j] = a.mas[j + 1];
                                    }
                                }
                            }
                            // создаём новый файл 
                            Array.Resize(ref a.mas, a.mas.Length - count);
                            File.Create($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Using.txt").Close();
                            str = new FileStream($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Using.txt", FileMode.Open);
                            Together together = new Together(a.mas);
                            json.WriteObject(str, together);
                            str.Close();
                        }
                        
                    }
                    // эисклбчение при десериализации
                    catch (Exception ex)
                    {
                        MessageBox.Show("Source files were corrupted");
                        //this.Close();
                    }

                }
            }
        }
    }
    /// <summary>
    /// Processonefile - класс информауии об испоьзуемом файле
    /// </summary>
    [DataContract]
    public class Processonefile
    {
        [DataMember]
        // MAC - адрес 
       public  string adr;
        [DataMember]
       public  string pass;
        [DataMember]
       public  string path;
        [DataMember]
        public bool direct;
       public Processonefile(string adr, string pass,string path, bool direct)
        {
            this.adr = adr;
            this.pass = pass;
            this.path = path;
            this.direct = direct;
        }
    }
    /// <summary>
    /// Класс Together - класс всех используемых файлов
    /// </summary>
    [DataContract]
    public class Together
    {
        [DataMember]
        // массив файлов
        public Processonefile [] mas;
        public Together(Processonefile[] mas)
        {
            this.mas = mas;
        }
    }

}
