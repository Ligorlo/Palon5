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
    public partial class Form3 : Form
    {
        // путь к файлу
        string path;
        // имя устойства 
        string Name2;
        // адрес устройства
        string adress;
        bool bo;
        public Form3(string path, string ID, string adress, bool bo)
        {
            InitializeComponent();
            this.path = path;
            Name2 = ID;
            this.adress = adress;
            this.bo = bo;
        }
        private void Form3_Load(object sender, EventArgs e)
        {

        }
        // кнопка подтверждения пароля
        private static byte [] EncryptAES(string tocode, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
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
            return encrypted;
        }
        private void Ready_Click_1(object sender, EventArgs e)
        {
            // проверка количества символов
            if (textBox1.Text.Length >= 8)
            {
                if (textBox1.Text == textBox2.Text)
                {
                    if (bo == true)
                    {
                        // папка для ключей
                        string pathtodirectory = $"C:/Users/{Environment.UserName}/AppData/Roaming/Palon";
                        bool b = true;
                        // подбор свободного номера для файла
                        int n = -1;
                        while (b)
                        {
                            n++;
                            if (!File.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Pakman{n}.txt"))
                            {
                                //создаём папку для ключей
                                Directory.CreateDirectory(pathtodirectory);
                                // файл ключа
                                File.Create($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Pakman{n}.txt").Close();
                                byte[] bytearr2 = File.ReadAllBytes(path);
                                File.Delete(path);
                                Random gen = new Random();
                                // генерация ключа
                                byte[] key = new byte[32];
                                for (int i = 0; i < key.Length; i++)
                                {
                                    key[i] = (byte)gen.Next(0, 257);
                                }
                                byte[] IV = new byte[16];
                                for (int i = 0; i < IV.Length; i++)
                                {
                                    IV[i] = (byte)gen.Next(0, 255);
                                }
                                // переводим ключ в байтовый формат (чтобы можно было хранить более 256 ключей)
                                byte[] keynumber = Encoding.ASCII.GetBytes(n.ToString());
                                // Алгоритм кодировки AES 
                                string Inbaseforencrypt = Convert.ToBase64String(bytearr2);
                                byte [] tofile = EncryptAES(Inbaseforencrypt, key, IV);
                                // объединяем номер файла с основной информцией и сериализуем
                                Connectwithakey connect = new Connectwithakey(tofile, keynumber);
                                FileStream str = new FileStream(path, FileMode.OpenOrCreate);
                                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Connectwithakey));
                                js.WriteObject(str, connect);
                                str.Close();
                                // можно ещё раз здесь XOR
                                // end with main file
                                // start with key file
                                // пароль 
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
                               // MemoryStream ms = new MemoryStream();
                               // serializer.WriteObject(ms, serfile);


                                string pathforkey = $"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Pakman{n}.txt";
                                FileStream fileStream = new FileStream(pathforkey, FileMode.Open);
                                serializer.WriteObject(fileStream, serfile);
                                fileStream.Close();
                                byte[] serkey = File.ReadAllBytes(pathforkey); //new byte[ms.Length];
                                                                               // ms.Read(serkey, 0, (int)ms.Length);
                                                                               //new byte[ms.Capacity];
                                                                               //int count = 0;
                                                                               //while (ms.CanRead)
                                                                               //{
                                                                               //    serkey[count] = (byte)ms.ReadByte();
                                                                               //    count++;
                                                                               //}
                                                                               // ещё раз ксорим с win но уже в другой кодировке
                                byte[] tocodekey = Encoding.ASCII.GetBytes(ID);
                                q = 0;
                                for (int i = 0; i < serkey.Length; i++)
                                {
                                    if (q == tocodekey.Length)
                                        q = 0;
                                    serkey[i] = (byte)(serkey[i] ^ tocodekey[q]);
                                }
                                File.WriteAllBytes(pathforkey, serkey);
                                b = false;
                                // меняем расширение файла для сериализации
                                File.Move(path, Path.ChangeExtension(path, ".code3"));
                               
                                this.Close();
                            }
                        }
                    }
                    else
                    {
                        string pathtodirectory = $"C:/Users/{Environment.UserName}/AppData/Roaming/Palon";
                        bool b = true;
                        // подбор свободного номера для файла
                        int n = -1;
                        while (b)
                        {
                            n++;
                            if (!File.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/PakmanD{n}.txt"))
                            {
                                string direct =  Path.ChangeExtension(path, "txt");
                                if (!File.Exists(Path.ChangeExtension(path, "txt")))
                                    ZipFile.CreateFromDirectory(path, Path.ChangeExtension(path, "txt"));
                                Directory.Delete(path, true);
                                //создаём папку для ключей
                                Directory.CreateDirectory(pathtodirectory);
                                // файл ключа
                                File.Create($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/PakmanD{n}.txt").Close();
                                byte[] bytearr2 = File.ReadAllBytes(direct);
                                File.Delete(direct);
                                Random gen = new Random();
                                // генерация ключа
                                byte[] key = new byte[32];
                                for (int i = 0; i < key.Length; i++)
                                {
                                    key[i] = (byte)gen.Next(0, 257);
                                }
                                byte[] IV = new byte[16];
                                for (int i = 0; i < IV.Length; i++)
                                {
                                    IV[i] = (byte)gen.Next(0, 255);
                                }
                                // переводим ключ в байтовый формат (чтобы можно было хранить более 256 ключей)
                                byte[] keynumber = Encoding.ASCII.GetBytes(n.ToString());
                                // Алгоритм кодировки AES 
                                string Inbaseforencrypt = Convert.ToBase64String(bytearr2);
                                byte[] tofile = EncryptAES(Inbaseforencrypt, key, IV);
                                // объединяем номер файла с основной информцией и сериализуем
                                Connectwithakey connect = new Connectwithakey(tofile, keynumber);
                                FileStream str = new FileStream(direct, FileMode.OpenOrCreate);
                                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Connectwithakey));
                                js.WriteObject(str, connect);
                                str.Close();
                                // можно ещё раз здесь XOR
                                // end with main file
                                // start with key file
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
                                MemoryStream ms = new MemoryStream();
                                serializer.WriteObject(ms, serfile);


                                string pathforkey = $"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/PakmanD{n}.txt";
                                FileStream fileStream = new FileStream(pathforkey, FileMode.Open);
                                serializer.WriteObject(fileStream, serfile);
                                fileStream.Close();
                                byte[] serkey = File.ReadAllBytes(pathforkey); //new byte[ms.Length];
                                                                               // ms.Read(serkey, 0, (int)ms.Length);
                                                                               //new byte[ms.Capacity];
                                                                               //int count = 0;
                                                                               //while (ms.CanRead)
                                                                               //{
                                                                               //    serkey[count] = (byte)ms.ReadByte();
                                                                               //    count++;
                                                                               //}
                                                                               // ещё раз ксорим с win но уже в другой кодировке
                                byte[] tocodekey = Encoding.ASCII.GetBytes(ID);
                                q = 0;
                                for (int i = 0; i < serkey.Length; i++)
                                {
                                    if (q == tocodekey.Length)
                                        q = 0;
                                    serkey[i] = (byte)(serkey[i] ^ tocodekey[q]);
                                }
                                File.WriteAllBytes(pathforkey, serkey);
                                b = false;
                                // меняем расширение файла для сериализации
                                File.Move(direct, Path.ChangeExtension(direct, ".code4"));
                                
                                b = false;

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
        [DataMember]
        public byte[] file;
        [DataMember]
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
            [DataMember]
            public byte[] file;
        [DataMember]
        public byte[] IV;
        [DataMember]
            public string DeviceName;
            [DataMember]
            public string DeviceAdress;
            [DataMember]
            public string Rassh;
            [DataMember]
            public string Password;
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
