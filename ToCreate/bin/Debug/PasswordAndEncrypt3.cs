using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Security.Principal;
using System.IO.Compression;
using System.Security.Cryptography;
namespace ToCreate
{
    /// <summary>
    /// Form3 - класс форма ввода парол и кодирования
    /// </summary>
    public partial class Passwordandencrypt3 :  MetroFramework.Forms.MetroForm
    {
        // путь к файлу
        string path;
        // имя устойства 
        string Name2;
        // адрес устройства
        string adress;
        // параметр 
        bool bo;
        //пароль
        string pass;

        // присвоение свойствам значений

        public Passwordandencrypt3(string path, string ID, string adress, bool bo)
        {
            InitializeComponent();
            this.path = path;
            Name2 = ID;
            this.adress = adress;
            this.bo = bo;
        }
        public Passwordandencrypt3(string path, string ID, string adress, bool bo, string pass)
        {;
            this.path = path;
            Name2 = ID;
            this.adress = adress;
            this.bo = bo;
            this.pass = pass;
            InitializeComponent();
            this.Hide();
            textBox1.Text = pass;
            textBox2.Text = pass;
            Ready_Click_1(0, new EventArgs());
        }
        // кодирование файла алгоритмом AES
        private static byte [] EncryptAES(string tocode, byte[] Key, byte[] IV)
        {
            // возвращаемые байты
            byte[] encrypted;
            // aes encryptor
            using (Aes aes = Aes.Create())
            {
                //ключ
                aes.Key = Key;
                // IV
                aes.IV = IV;
                // создаём encryptor
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(csEncrypt))
                        {
                            // записываем
                            sw.Write(tocode);
                        }
                        encrypted = ms.ToArray();
                    }
                }
            }
            // возвращаем массив байтов (зашифрованных)
            return encrypted;
        }
        // кнопка готовности ввода
        private void Ready_Click_1(object sender, EventArgs e)
        {
            // проверка количества символов
            if (textBox1.Text.Length >= 8)
            {
                // проверка совпадения паролей
                if (textBox1.Text == textBox2.Text)
                {
                    // провекра файл или папка
                    if (bo == true)
                    {
                        string pathtodirectory = "";
                        // папка для ключей
                        if (Directory.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming"))
                        {
                            pathtodirectory = $"C:/Users/{Environment.UserName}/AppData/Roaming/Palon";
                        }
                        else
                        {
                            pathtodirectory = $"../Palon";
                        }
                        // переменная для остановки подбора
                        bool b = true;
                        // подбор свободного номера для файла
                        int n = -1;
                        while (b)
                        {
                            n++;
                            if (!File.Exists($"{pathtodirectory}/Pakman{n}.txt"))
                            {
                                //создаём папку для ключей
                                Directory.CreateDirectory(pathtodirectory);
                                // файл ключа
                                File.Create($"{pathtodirectory}/Pakman{n}.txt").Close();
                                // содержимое начального файла
                                byte[] bytearr2 = File.ReadAllBytes(path);
                                File.Delete(path);
                                // генератор рандома
                                Random gen = new Random();
                                // генерация ключа
                                byte[] key = new byte[32];
                                for (int i = 0; i < key.Length; i++)
                                {
                                    key[i] = (byte)gen.Next(0, 257);
                                }
                                // генерация IV
                                byte[] IV = new byte[16];
                                for (int i = 0; i < IV.Length; i++)
                                {
                                    IV[i] = (byte)gen.Next(0, 255);
                                }
                                // переводим ключ в байтовый формат (чтобы можно было сериализовать и хранить более 256 ключей)
                                byte[] keynumber = Encoding.ASCII.GetBytes(n.ToString());
                                // Алгоритм кодировки AES 
                                // переводим в base64 
                                string Inbaseforencrypt = Convert.ToBase64String(bytearr2);
                                // закодрованный массив байтов
                                byte [] tofile = EncryptAES(Inbaseforencrypt, key, IV);
                                // объединяем номер файла с основной информцией и сериализуем
                                Connectwithakey connect = new Connectwithakey(tofile, keynumber);
                                // записываем в старый путь 
                                FileStream str = new FileStream(path, FileMode.OpenOrCreate);
                                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Connectwithakey));
                                js.WriteObject(str, connect);
                                str.Close();
                                // берём пароль
                                string pass = textBox1.Text;
                                // для кодирования берется айди безопасности учетно записи, чтобы нельзя было открыть с другого компа
                                IntPtr accountToken = WindowsIdentity.GetCurrent().Token;
                                WindowsIdentity win = new WindowsIdentity(accountToken);
                                string ID = win.User.ToString();
                                // ключ ксорится с win 
                                byte[] onlykey = Encoding.UTF8.GetBytes(ID);
                               int  q = 0;
                                for (int i = 0; i < key.Length; i++)
                                {
                                    if (q == onlykey.Length)
                                        q = 0;
                                    key[i] = (byte)(key[i] ^ onlykey[q]);
                                }
                                // сериализация всех данных безопасности
                                ToCode serfile = new ToCode(key, IV, Name2, adress, pass, Path.GetExtension(path));
                                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ToCode));
                                // путь к ключу
                                string pathforkey = $"{pathtodirectory}/Pakman{n}.txt";
                                FileStream fileStream = new FileStream(pathforkey, FileMode.Open);
                                // сериализуем и записываем ключ
                                serializer.WriteObject(fileStream, serfile);
                                fileStream.Close();
                                // вновь читаем чтобы всё вместе закодировать
                                byte[] serkey = File.ReadAllBytes(pathforkey); 
                                // ксорим всё вместе с ID в кодировке ASCII
                                byte[] tocodekey = Encoding.ASCII.GetBytes(ID);
                                q = 0;
                                for (int i = 0; i < serkey.Length; i++)
                                {
                                    if (q == tocodekey.Length)
                                        q = 0;
                                    serkey[i] = (byte)(serkey[i] ^ tocodekey[q]);
                                }
                                // записываем обратно  файл
                                File.WriteAllBytes(pathforkey, serkey);
                                // останавливаем подбор файло для ключа
                                b = false;
                                // меняем расширение файла для сериализации
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
                                        if (!File.Exists (pathcheck))
                                        {
                                            filename = false;
                                        }
                                    }
                                }
                                File.Move(path, pathcheck);
                                this.Close();
                            }
                        }
                    }
                    else
                    {
                        string pathtodirectory = "";
                        // папка для ключей
                        if (Directory.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming"))
                        {
                            pathtodirectory = $"C:/Users/{Environment.UserName}/AppData/Roaming/Palon";
                        }
                        else
                        {
                            pathtodirectory = $"../Palon";
                        }
                        // переменная для подбора свободного ключа
                        bool b = true;
                        // подбор свободного номера для файла
                        int n = -1;
                        while (b)
                        {
                            n++;
                            // проверка существования ключа
                            if (!File.Exists($"{pathtodirectory}/PakmanD{n}.txt"))
                            {
                                // меняем путь
                                string direct = Path.ChangeExtension(path, "txt");
                                int num = 2;
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
                                        direct = Path.ChangeExtension(path, $"txt{num}");
                                        num++;
                                    }
                                }
                                        Directory.Delete(path, true);
                                //создаём папку для ключей
                                Directory.CreateDirectory(pathtodirectory);
                                // файл ключа
                                File.Create($"{pathtodirectory}/PakmanD{n}.txt").Close();
                                // читаем данные архивированной папки
                                byte[] bytearr2 = File.ReadAllBytes(direct);
                                // удаляем архивированную папку
                                File.Delete(direct);
                                // генератор рандома
                                Random gen = new Random();
                                // генерация ключа
                                byte[] key = new byte[32];
                                for (int i = 0; i < key.Length; i++)
                                {
                                    key[i] = (byte)gen.Next(0, 257);
                                }
                                // генераия IV
                                byte[] IV = new byte[16];
                                for (int i = 0; i < IV.Length; i++)
                                {
                                    IV[i] = (byte)gen.Next(0, 255);
                                }
                                // переводим ключ в байтовый формат (чтобы можно было хранить более 256 ключей)
                                byte[] keynumber = Encoding.ASCII.GetBytes(n.ToString());
                                // Алгоритм кодировки AES 
                                // переводим в base64
                                string Inbaseforencrypt = Convert.ToBase64String(bytearr2);
                                // энкриптим
                                byte[] tofile = EncryptAES(Inbaseforencrypt, key, IV);
                                // объединяем номер файла с основной информцией и сериализуем
                                Connectwithakey connect = new Connectwithakey(tofile, keynumber);
                                // записывем получившееся в новый файл
                                FileStream str = new FileStream(direct, FileMode.OpenOrCreate);
                                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Connectwithakey));
                                js.WriteObject(str, connect);
                                str.Close();
                                // пароль 
                                string pass = textBox1.Text;
                                // для кодирования берется айди безопасности учетно записи, чтобы нельзя было открыть с другого компа
                                IntPtr accountToken = WindowsIdentity.GetCurrent().Token;
                                WindowsIdentity win = new WindowsIdentity(accountToken);
                                string ID = win.User.ToString();
                                // ключ ксорится с win 
                                byte[] onlykey = Encoding.UTF8.GetBytes(ID);
                               int q = 0;
                                for (int i = 0; i < key.Length; i++)
                                {
                                    if (q == onlykey.Length)
                                        q = 0;
                                    key[i] = (byte)(key[i] ^ onlykey[q]);
                                }
                                // сериализация всех данных безопасности
                                ToCode serfile = new ToCode(key,IV,  Name2, adress, pass, Path.GetExtension(direct));
                                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ToCode));
                                string pathforkey = $"{pathtodirectory}/PakmanD{n}.txt";
                                FileStream fileStream = new FileStream(pathforkey, FileMode.Open);
                                // записывем в фвйл
                                serializer.WriteObject(fileStream, serfile);
                                fileStream.Close();
                                // вновь читаем для того чтобы всё ксорить с ID в кодировке ASCII
                                byte[] serkey = File.ReadAllBytes(pathforkey); 
                                byte[] tocodekey = Encoding.ASCII.GetBytes(ID);
                                q = 0;
                                // ксорим
                                for (int i = 0; i < serkey.Length; i++)
                                {
                                    if (q == tocodekey.Length)
                                        q = 0;
                                    serkey[i] = (byte)(serkey[i] ^ tocodekey[q]);
                                }
                                // записываем
                                File.WriteAllBytes(pathforkey, serkey);
                                // избегаем повтора
                                b = false;
                                // меняем расширение файла для сериализации
                                // проверяем чтобы не было исключений
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
                                this.Close();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Passwords do not match");
                }
            }
            else
            {
                MessageBox.Show("The password must be at least eight characters");
            }
        }
    }
    /// <summary>
    /// Класс для сериализации содержимого файла с номером ключа
    /// </summary>
    [DataContract]
    public class Connectwithakey
    {
        // данные файла
        [DataMember]
        public byte[] file;
        [DataMember]
        // номер ключа
        public byte[] keynumber;
        public Connectwithakey(byte[]file, byte [] keynumber)
        {
            this.file = file;
            this.keynumber = keynumber;
        }
    }
    /// <summary>
    /// Класс для сериализации пароля ключа имени и адреса девайса
    /// </summary>
    [DataContract]
        public class ToCode
        {
        // ключ
            [DataMember]
            public byte [] file;
        [DataMember]
        // IV
        public byte[] IV;
        [DataMember]
        // имя устройства
            public string DeviceName;
            [DataMember]
            // адрес устройства
            public string DeviceAdress;
            [DataMember]
            // расширение файла
            public string Rassh;
            [DataMember]
            // пароль
            public string Password;
        // присвоение значений полям
            public ToCode(byte[] file, byte [] IV, string DeviceName, string DeviceAdress, string password, string Rassh)
            {
            this.IV = IV;
                this.file = file;
                this.DeviceName = DeviceName;
                this.DeviceAdress = DeviceAdress;
                Password = password;
                this.Rassh = Rassh;
            }
        }
    }
