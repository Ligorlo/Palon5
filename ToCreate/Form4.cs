using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Security.Principal;
using InTheHand.Net.Bluetooth;
using System.IO.Compression;
using System.Security.Cryptography;
namespace ToCreate
{
    /// <summary>
    /// Form4 - форма раскодирования
    /// </summary>
    // форма раскодирования
    public partial class Form4 : Form
    {
        // метод раскодирования по алгоритму AES
        static string DecryptAES(byte[] tocode, byte[] Key, byte[] IV)
        {
            // переменная в котрорую запишем раскодированную строку
            string plaintext = null;
            // создаем AES объект 
            using (Aes aesAlg = Aes.Create())
            {
                // ключ
                aesAlg.Key = Key;
                // IV
                aesAlg.IV = IV;
                // создаём декриптор
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(tocode))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            // возвращаем строку
            return plaintext;
        }
        // переменная чтобы обозначить папку от файла
        bool directory  = true;
        // путь выбранного файла
        string[] args;
        // для десериализации
        ToCode cod;
        // поле пути к файлу
        string path;
        // массив байтов в файле
        byte[] FindkeyNumber;
        // айди безопасности учетной записи
        string ID;
        // номер ключа
        int num = -1;
        public Form4(string [] args)
        {
            InitializeComponent();
            // путь к файлу через который программа открыта
           this.path = args[0];
            this.args = args;
        }
        // метод расшифроваия
        public void Redeem()
        {
            // расширение
            string k = Path.GetExtension(path);
            // провекра папка или файл
            if (k == ".code3")
            {
                // файл
                // расшифровываем сам ключ
                byte[] key = cod.file;
                byte[] IV = cod.IV;
                // ксорим с id безопасности ключ в кодировке UTF8
                byte[] onlykey = Encoding.UTF8.GetBytes(ID);
                int q = 0;
                for (int i = 0; i < key.Length; i++)
                {
                    if (q == onlykey.Length)
                        q = 0;
                    key[i] = (byte)(key[i] ^ onlykey[q]);
                }
                // расшировываем файл
                string todecr = DecryptAES(FindkeyNumber, key, IV);
                // удаляем старый файл
                File.Delete(path);
                // конвертим строку обратно в байты 
                byte [] bytefile = Convert.FromBase64String(todecr);
                // создаем файл со старым расширением
                File.WriteAllBytes(path, bytefile);
                int number = 2;
                bool exist = true;
                string pathcheck = Path.ChangeExtension(path, cod.Rassh);
                if (File.Exists(pathcheck))
                {
                    while (exist)
                    {
                        pathcheck = Path.ChangeExtension(path, "");
                        pathcheck = pathcheck.Remove(pathcheck.Length - 1);
                        pathcheck = $"{pathcheck}{number}";
                        number++;
                        pathcheck = Path.ChangeExtension(pathcheck, cod.Rassh);
                        if (!File.Exists(pathcheck))
                        {
                            exist = false;
                        }
                    }
                }
                File.Move(path, pathcheck);
                // определяем нужно ли удалять ключ
                bool b = true;
                if (args.Length == 2)
                    b = false;
                // форма для шифрования при закрытии
                label1.Visible = false;
                Form5 closing = new Form5(path, key, IV, cod, num, b, directory);
                this.Hide();
                if (b == true)
                {
                    closing.ShowDialog();
                }
                else
                {
                    // проверка существую ли ключи
                    if (File.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Pakman{num}.txt"))
                    {
                        File.Delete($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Pakman{num}.txt");
                    }
                    else
                    {
                        if (File.Exists($"../Palon/Pakman{num}.txt"))
                        {
                            File.Delete($"../Palon/Pakman{num}.txt");
                        }
                    }
                }
                this.Close();
            }
            else
            {
                // папка 
                directory = false;
                // расшифровываем сам ключ
                byte[] key = cod.file;
                byte[] IV = cod.IV;
                // ксорим с id UTF8
                byte[] onlykey = Encoding.UTF8.GetBytes(ID);
                int q = 0;
                for (int i = 0; i < key.Length; i++)
                {
                    if (q == onlykey.Length)
                        q = 0;
                    key[i] = (byte)(key[i] ^ onlykey[q]);
                }
                // расшировываем файл
                string todecr = DecryptAES(FindkeyNumber, key, IV);
                // удаляем старую папку
                File.Delete(path);
                // переводим обратно в байты
                byte[] bytefile = Convert.FromBase64String(todecr);
                // записываем обратно
                File.WriteAllBytes(path, bytefile);
                // подбираем файл если такие же существуют
                int number = 2;
                bool exist = true;
                string pathcheck = Path.ChangeExtension(path, cod.Rassh);
                if (File.Exists(pathcheck))
                {
                    while (exist)
                    {
                        pathcheck = Path.ChangeExtension(path, "");
                        pathcheck = pathcheck.Remove(pathcheck.Length - 1);
                        pathcheck = $"{pathcheck}{number}";
                        number++;
                        pathcheck = Path.ChangeExtension(pathcheck, cod.Rassh);
                        if (!File.Exists(pathcheck))
                        {
                            exist = false;
                        }
                    }
                }
                File.Move(path, pathcheck);
                int number2 = 2;
                bool exist2 = true;
                string pathcheck2 = Path.ChangeExtension(pathcheck, "");
                if (Directory.Exists(pathcheck2))
                {
                    while (exist2)
                    {
                        pathcheck2 = Path.ChangeExtension(path, "");
                        pathcheck2 = pathcheck2.Remove(pathcheck2.Length - 1);
                        pathcheck2 = $"{pathcheck2}{number2}";
                        number2++;
                        pathcheck2 = Path.ChangeExtension(pathcheck2, "");
                        if (!File.Exists(pathcheck2))
                        {
                            exist2 = false;
                        }
                    }
                }
                // разархивируем
                ZipFile.ExtractToDirectory(pathcheck, pathcheck2);
                // удаляем старыйфайл
                File.Delete(pathcheck);
                path = pathcheck2;
                // проверка надо ли удалять 
                bool b = true;
                if (args.Length == 2)
                    b = false;
                // форма для шифрования при закрытии
                label1.Visible = false;
                Form5 closing = new Form5(path, key,IV,  cod, num, b, directory);
                this.Hide();
                if (b == true)
                {
                    closing.ShowDialog();
                }
                else
                {
                    // проверка существую ли ключи
                    if (File.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Pakman{num}.txt"))
                    {
                        File.Delete($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Pakman{num}.txt");
                    }
                    else
                    {
                        if (File.Exists($"../Palon/Pakman{num}.txt"))
                        {
                            File.Delete($"../Palon/Pakman{num}.txt");
                        }
                    }
                    this.Close();
                }
            }
        }
        // имя устройства
        string devicename;
        // адрес устройства
        string deviceadress;
        // параметр определяющий найдено ли устройтсво
        int compare = 0;
        public void BluetoothDescovery(object sender, DiscoverDevicesEventArgs e)
        {
            // проверка найден ли уже
            if(compare == 0)
            if(e.Devices!= null)
            {
                    // сравниваем совпадает ли адрес того, что поблизости с указаным в клчевом файле
                foreach (var item in e.Devices)
                {
                    if(item.DeviceAddress.ToString() == deviceadress)
                    {
                        compare = 1;
                            Redeem();
                    }
                }
            }
        }
        // метод в конце поиска
        public void BluetoothEndDescovery(object sender, DiscoverDevicesEventArgs e)
        {
            // если не найдено щапускаем форму сравнения паролей
            if (compare == 0)
            {
                // если устройства нет рядом просим ввести пароль
                Form6 password = new Form6(cod);
                password.ShowDialog();
                // проверка верности пароля
                if (password.Pasbool)
                {
                    Redeem();
                }
            }
        }
        // кнопка для открытия (чтобы не прописывать в конструкторе)
        private void Open_Click(object sender, EventArgs e)
        {
            Open.Enabled = false;
            label1.Visible = true;
            // проверка работает ли bluetooth
            if (BluetoothRadio.IsSupported)
            {
                // проверка расширения
                string k = Path.GetExtension(path);
                if (k == ".code3")
                {
                    // закрыываем кнопку
                    Open.Enabled = false;
                    // дезериализуем ключ и файл
                    DataContractJsonSerializer json2 = new DataContractJsonSerializer(typeof(Connectwithakey));
                    DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(ToCode));
                    FileStream st = new FileStream(path, FileMode.Open);
                    Connectwithakey con = (Connectwithakey)json2.ReadObject(st);
                    FindkeyNumber = con.file;
                    // номер ключа
                    this.num = int.Parse(Encoding.ASCII.GetString(con.keynumber));
                    st.Close();
                    byte[] serkey = null;
                    // извлечение ключа из файла
                    if (File.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Pakman{num}.txt"))
                    {
                      serkey = File.ReadAllBytes($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Pakman{num}.txt");
                    }
                    else
                    {
                        if (File.Exists($"../Palon/Pakman{num}.txt"))
                        {
                            serkey = File.ReadAllBytes($"../Palon/Pakman{num}.txt");
                        }
                        else
                        {
                            MessageBox.Show("Something has happened with keys");
                            this.Close();
                        }
                    }
                    //безопастность (учетная запись)
                    IntPtr accountToken = WindowsIdentity.GetCurrent().Token;
                    WindowsIdentity win = new WindowsIdentity(accountToken);
                    // раскодируем
                    ID = win.User.ToString();
                        // ксорим с id в ASCII
                    byte[] tocodekey = Encoding.ASCII.GetBytes(ID);
                    int q = 0;
                    for (int i = 0; i < serkey.Length; i++)
                    {
                        if (q == tocodekey.Length)
                            q = 0;
                        serkey[i] = (byte)(serkey[i] ^ tocodekey[q]);
                    }
                    // создём вспомогательный файл для десериализации и извдечение полей класса
                    string help;
                    if (Directory.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming"))
                    {
                        help = $"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Pakman{num}Help.txt";
                    }
                    else
                    {
                        help = $"../Pakman{num}Help.txt";
                    }
                    // файл нужен для десериализации (так быстрее чем с memorystream так как есть writeallbytes)
                    File.Create(help).Close();
                    File.WriteAllBytes(help, serkey);
                    FileStream l = new FileStream(help, FileMode.Open);
                    cod = (ToCode)json.ReadObject(l);
                    l.Close();
                    // удаляем вспомогательный файл
                    File.Delete(help);
                    // проверяем совпадение устройста 
                    devicename = cod.DeviceName;
                    deviceadress = cod.DeviceAdress;
                    // компонента bluetooth
                    BluetoothComponent component = new BluetoothComponent();
                    // добавляем метод который будет сделать если найдено новое устройство
                    component.DiscoverDevicesProgress += BluetoothDescovery;
                    // метод в конце работы
                    component.DiscoverDevicesComplete += BluetoothEndDescovery;
                    // поиск устройства
                    component.DiscoverDevicesAsync(10, true, false, true,true,0);
                }
                else
                {
                    DataContractJsonSerializer json2 = new DataContractJsonSerializer(typeof(Connectwithakey));
                    DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(ToCode));
                    // десереализуем файл
                    FileStream st = new FileStream(path, FileMode.Open);
                    Connectwithakey con = (Connectwithakey)json2.ReadObject(st);
                    FindkeyNumber = con.file;
                    // номер ключа
                    this.num = int.Parse(Encoding.ASCII.GetString(con.keynumber));
                    st.Close();
                    // извлечение ключа из файла
                    byte[] serkey = null;
                    // извлечение ключа из файла
                    if (File.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/PakmanD{num}.txt"))
                    {
                        serkey = File.ReadAllBytes($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/PakmanD{num}.txt");
                    }
                    else
                    {
                        if (File.Exists($"../Palon/PakmanD{num}.txt"))
                        {
                            serkey = File.ReadAllBytes($"../Palon/PakmanD{num}.txt");
                        }
                        else
                        {
                            MessageBox.Show("Something has happened with keys");
                            this.Close();
                        }
                    }
                    //безопастность (учетная запись)
                    IntPtr accountToken = WindowsIdentity.GetCurrent().Token;
                    WindowsIdentity win = new WindowsIdentity(accountToken);
                    // раскодируем ксоря c ID в ASCII
                    ID = win.User.ToString();
                    byte[] tocodekey = Encoding.ASCII.GetBytes(ID);
                    int q = 0;
                    for (int i = 0; i < serkey.Length; i++)
                    {
                        if (q == tocodekey.Length)
                            q = 0;
                        serkey[i] = (byte)(serkey[i] ^ tocodekey[q]);
                    }
                    // создём вспомогательный файл для десериализации и извдечение полей класса
                    string help;
                    if (Directory.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming"))
                    {
                        help = $"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/PakmanD{num}Help.txt";
                    }
                    else
                    {
                        help = $"../PakmanD{num}Help.txt";
                    }
                    // перезаписываем файл для десериализации так удобней чем при memorystream так как writeallbytes
                    File.Create(help).Close();
                    File.WriteAllBytes(help, serkey);
                    FileStream l = new FileStream(help, FileMode.Open);
                    cod = (ToCode)json.ReadObject(l);
                    l.Close();
                    // удаляем вспомогательный файл
                    File.Delete(help);
                    // проверяем совпадение устройста 
                    devicename = cod.DeviceName;
                    deviceadress = cod.DeviceAdress;
                    // создаём компоненту поиска
                    BluetoothComponent component = new BluetoothComponent();
                    // добавляем метод при каждом нахождении нового устройствва
                    component.DiscoverDevicesProgress += BluetoothDescovery;
                    // метод в конце поиска
                    component.DiscoverDevicesComplete += BluetoothEndDescovery;
                    // начало писка
                    component.DiscoverDevicesAsync(10, true, false, true, true, 0);
                }
            }
            // раскодируем по паролю 
            else
            {
                // аналогично смотрим расширение
                string k = Path.GetExtension(path);
                if (k == ".code3")
                {
                    // откючаем кнопку
                    Open.Enabled = false;
                    DataContractJsonSerializer json2 = new DataContractJsonSerializer(typeof(Connectwithakey));
                    DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(ToCode));
                    // десериализуем основной файл
                    FileStream st = new FileStream(path, FileMode.Open);
                    Connectwithakey con = (Connectwithakey)json2.ReadObject(st);
                    FindkeyNumber = con.file;
                    // номер ключа
                    this.num = int.Parse(Encoding.ASCII.GetString(con.keynumber));
                    st.Close();
                    // извлечение ключа из файла
                    byte[] serkey = null;
                    // извлечение ключа из файла
                    if (File.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Pakman{num}.txt"))
                    {
                        serkey = File.ReadAllBytes($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Pakman{num}.txt");
                    }
                    else
                    {
                        if (File.Exists($"../Palon/Pakman{num}.txt"))
                        {
                            serkey = File.ReadAllBytes($"../Palon/Pakman{num}.txt");
                        }
                        else
                        {
                            MessageBox.Show("Something has happened with keys");
                            this.Close();
                        }
                    }
                    //безопастность (учетная запись)
                    IntPtr accountToken = WindowsIdentity.GetCurrent().Token;
                    WindowsIdentity win = new WindowsIdentity(accountToken);
                    // раскодируем ксоря с id ASCII
                    ID = win.User.ToString();
                    byte[] tocodekey = Encoding.ASCII.GetBytes(ID);
                    int q = 0;
                    for (int i = 0; i < serkey.Length; i++)
                    {
                        if (q == tocodekey.Length)
                            q = 0;
                        serkey[i] = (byte)(serkey[i] ^ tocodekey[q]);
                    }
                    // создём вспомогательный файл для десериализации и извдечение полей класса
                    string help;
                    if (Directory.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming"))
                    {
                        help = $"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Pakman{num}Help.txt";
                    }
                    else
                    {
                        help = $"../Pakman{num}Help.txt";
                    }
                    File.Create(help).Close();
                    File.WriteAllBytes(help, serkey);
                    FileStream l = new FileStream(help, FileMode.Open);
                    cod = (ToCode)json.ReadObject(l);
                    l.Close();
                    // удаляем вспомогательный файл
                    File.Delete(help);
                    // проверяем совпадение устройста 
                    devicename = cod.DeviceName;
                    deviceadress = cod.DeviceAdress;
                    // если устройства нет рядом просим ввести пароль
                    Form6 password = new Form6(cod);
                    password.ShowDialog();
                    // проверка верности пароля
                    if (password.Pasbool)
                    {
                        Redeem();
                    }
                }
                else
                {
                    // десериализуем основной файл
                    DataContractJsonSerializer json2 = new DataContractJsonSerializer(typeof(Connectwithakey));
                    DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(ToCode));
                    FileStream st = new FileStream(path, FileMode.Open);
                    Connectwithakey con = (Connectwithakey)json2.ReadObject(st);
                    FindkeyNumber = con.file;
                    // номер ключа
                    this.num = int.Parse(Encoding.ASCII.GetString(con.keynumber));
                    st.Close();
                    // извлечение ключа из файла
                    byte[] serkey = null;
                    // извлечение ключа из файла
                    if (File.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/PakmanD{num}.txt"))
                    {
                        serkey = File.ReadAllBytes($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/PakmanD{num}.txt");
                    }
                    else
                    {
                        if (File.Exists($"../Palon/PakmanD{num}.txt"))
                        {
                            serkey = File.ReadAllBytes($"../Palon/PakmanD{num}.txt");
                        }
                        else
                        {
                            MessageBox.Show("Something has happened with keys");
                            this.Close();
                        }
                    }//безопастность (учетная запись)
                    IntPtr accountToken = WindowsIdentity.GetCurrent().Token;
                    WindowsIdentity win = new WindowsIdentity(accountToken);
                    // раскодируем ксоря с ID в ASCII
                    ID = win.User.ToString();
                    byte[] tocodekey = Encoding.ASCII.GetBytes(ID);
                    int q = 0;
                    for (int i = 0; i < serkey.Length; i++)
                    {
                        if (q == tocodekey.Length)
                            q = 0;
                        serkey[i] = (byte)(serkey[i] ^ tocodekey[q]);
                    }
                    // создём вспомогательный файл для десериализации и извдечение полей класса
                    string help;
                    if (Directory.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming"))
                    {
                        help = $"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/PakmanD{num}Help.txt";
                    }
                    else
                    {
                        help = $"../PakmanD{num}Help.txt";
                    }
                    // так быстрее так как в memorystream нет writeallbytes
                    File.Create(help).Close();
                    File.WriteAllBytes(help, serkey);
                    FileStream l = new FileStream(help, FileMode.Open);
                    cod = (ToCode)json.ReadObject(l);
                    l.Close();
                    // удаляем вспомогательный файл
                    File.Delete(help);
                    // проверяем совпадение устройста 
                    devicename = cod.DeviceName;
                    deviceadress = cod.DeviceAdress;
                    // если устройства нет рядом просим ввести пароль
                    Form6 password = new Form6(cod);
                    password.ShowDialog();
                    // проверка верности пароля
                    if (password.Pasbool)
                    {
                        Redeem();
                    }
                }
            }
            Open.Enabled = true;
        }
    }
}
