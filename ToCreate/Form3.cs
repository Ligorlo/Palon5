using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Security.Principal;
using System.Diagnostics;
using System.IO.Compression;
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
        private void Ready_Click_1(object sender, EventArgs e)
        {
            
            Stopwatch s = new Stopwatch();
            s.Start();
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
                                byte[] key = new byte[gen.Next(50, 100)];
                                for (int i = 0; i < key.Length; i++)
                                {
                                    key[i] = (byte)gen.Next(0, 257);
                                }
                                // переводим ключ в байтовый формат (чтобы можно было хранить более 256 ключей)
                                byte[] keynumber = Encoding.ASCII.GetBytes(n.ToString());
                                // Алгоритм кодировки RC4 (просто ксорим с ключом)
                                int q = 0;
                                for (int i = 0; i < bytearr2.Length; i++)
                                {
                                    if (q == key.Length)
                                        q = 0;
                                    bytearr2[i] = (byte)(bytearr2[i] ^ key[q]);
                                    q++;
                                }
                                // объединяем номер файла с основной информцией и сериализуем
                                Connectwithakey connect = new Connectwithakey(bytearr2, keynumber);
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
                                q = 0;
                                for (int i = 0; i < key.Length; i++)
                                {
                                    if (q == onlykey.Length)
                                        q = 0;
                                    key[i] = (byte)(key[i] ^ onlykey[q]);
                                }
                                // сериализация всех данных безопасности
                                ToCode serfile = new ToCode(key, Name2, adress, pass, Path.GetExtension(path));
                                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ToCode));
                                MemoryStream ms = new MemoryStream();
                                serializer.WriteObject(ms, serfile);


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
                                MessageBox.Show(s.Elapsed.ToString());
                                s.Stop();
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
                                byte[] key = new byte[gen.Next(50, 100)];
                                for (int i = 0; i < key.Length; i++)
                                {
                                    key[i] = (byte)gen.Next(0, 257);
                                }
                                // переводим ключ в байтовый формат (чтобы можно было хранить более 256 ключей)
                                byte[] keynumber = Encoding.ASCII.GetBytes(n.ToString());
                                // Алгоритм кодировки RC4 (просто ксорим с ключом)
                                int q = 0;
                                for (int i = 0; i < bytearr2.Length; i++)
                                {
                                    if (q == key.Length)
                                        q = 0;
                                    bytearr2[i] = (byte)(bytearr2[i] ^ key[q]);
                                    q++;
                                }
                                // объединяем номер файла с основной информцией и сериализуем
                                Connectwithakey connect = new Connectwithakey(bytearr2, keynumber);
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
                                q = 0;
                                for (int i = 0; i < key.Length; i++)
                                {
                                    if (q == onlykey.Length)
                                        q = 0;
                                    key[i] = (byte)(key[i] ^ onlykey[q]);
                                }
                                // сериализация всех данных безопасности
                                ToCode serfile = new ToCode(key, Name2, adress, pass, Path.GetExtension(direct));
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
                                MessageBox.Show(s.Elapsed.ToString());
                                s.Stop();
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
            public string DeviceName;
            [DataMember]
            public string DeviceAdress;
            [DataMember]
            public string Rassh;
            [DataMember]
            public string Password;
            public ToCode(byte[] file, string DeviceName, string DeviceAdress, string password, string Rassh)
            {
                this.file = file;
                this.DeviceName = DeviceName;
                this.DeviceAdress = DeviceAdress;
                Password = password;
                this.Rassh = Rassh;
            }
        }
    }
