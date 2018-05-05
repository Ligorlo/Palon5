using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Security.Principal;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using System.IO.Compression;
using System.Diagnostics;
using System.Security.Cryptography;
namespace ToCreate
{
  

    // форма раскодирования
    public partial class Form4 : Form
    {
        static string DecryptAES(byte[] tocode, byte[] Key
, byte[] IV)
        {


            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key
, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(tocode))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt
, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(
csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting  stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }
        bool directory  = true;
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
            string k = Path.GetExtension(path);
            if (k == ".code3")
            {
                // расшифровываем сам ключ
                byte[] key = cod.file;
                byte[] IV = cod.IV;
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
                File.Delete(path);
                byte [] bytefile = Convert.FromBase64String(todecr);
                // создаем файл со старым расширением
                File.WriteAllBytes(path, bytefile);
                File.Move(path, Path.ChangeExtension(path, cod.Rassh));
                bool b = true;
                if (args.Length == 2)
                    b = false;
                // форма для шифрования при закрытии
                Form5 closing = new Form5(path, key, IV, cod, num, b, directory);
                this.Hide();
                if (b == true)
                {
                    closing.ShowDialog();
                }
                else
                {
                    File.Delete($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Pakman{num}.txt");
                }
                this.Close();
            }
            else
            {
                directory = false;
                // расшифровываем сам ключ
                byte[] key = cod.file;
                byte[] IV = cod.IV;
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
                File.Delete(path);
                byte[] bytefile = Convert.FromBase64String(todecr);
                // создаем файл со старым расширением
                //File.WriteAllBytes(path, bytefile);
                // создаем файл со старым расширением
                File.WriteAllBytes(path, bytefile);
                File.Move(path, Path.ChangeExtension(path, cod.Rassh));

                File.Delete(path);
                ZipFile.ExtractToDirectory(Path.ChangeExtension(path, cod.Rassh), Path.ChangeExtension(path, ""));
                File.Delete(Path.ChangeExtension(path, cod.Rassh));
                path = Path.ChangeExtension(path, "");
                bool b = true;
                if (args.Length == 2)
                    b = false;

                // форма для шифрования при закрытии
                Form5 closing = new Form5(path, key,IV,  cod, num, b, directory);
                this.Hide();
                if (b == true)
                {
                    closing.ShowDialog();
                }
                else
                {
                    File.Delete($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/PakmanD{num}.txt");
                }
                this.Close();
            }
        }
        string devicename;
        string deviceadress;
        int compare = 0;
        public void BluetoothDescovery(object sender, DiscoverDevicesEventArgs e)
        {
            if(compare == 0)
            if(e.Devices!= null)
            {
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
        public void BluetoothEndDescovery(object sender, DiscoverDevicesEventArgs e)
        {
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
        private void button1_Click(object sender, EventArgs e)
        {
            if (BluetoothRadio.IsSupported)
            {
                string k = Path.GetExtension(path);
                if (k == ".code3")
                {
                    button1.Enabled = false;
                    DataContractJsonSerializer json2 = new DataContractJsonSerializer(typeof(Connectwithakey));
                    DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(ToCode));
                    // ещё один поиск устройств
                   // BluetoothClient bc = new BluetoothClient();
                   // BluetoothDeviceInfo[] info = null;
                    //info = bc.DiscoverDevices();
                    // чтение основного файла
                    FileStream st = new FileStream(path, FileMode.Open);
                    Connectwithakey con = (Connectwithakey)json2.ReadObject(st);
                    FindkeyNumber = con.file;
                    // номер ключа
                    this.num = int.Parse(Encoding.ASCII.GetString(con.keynumber));
                    st.Close();
                    // извлечение ключа из файла
                    byte[] serkey = File.ReadAllBytes($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Pakman{num}.txt");
                    //безопастность (учетная запись)
                    IntPtr accountToken = WindowsIdentity.GetCurrent().Token;
                    WindowsIdentity win = new WindowsIdentity(accountToken);
                    // раскодируем
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
                    string help = $"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Pakman{num}Help.txt";
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
                    BluetoothComponent component = new BluetoothComponent();
                    
                    component.DiscoverDevicesProgress += BluetoothDescovery;
                    component.DiscoverDevicesComplete += BluetoothEndDescovery;
                    component.DiscoverDevicesAsync(10, true, false, true,true,0);
                   // while (compare == 0)
                    //{

                    //}
                    //foreach (BluetoothDeviceInfo item in info)
                    //{
                    //    if (item.DeviceName == cod.DeviceName && item.DeviceAddress.ToString() == cod.DeviceAdress)
                    //    {
                    //        b = true;
                    //    }
                    //}
                    
                }
                else
                {
                    DataContractJsonSerializer json2 = new DataContractJsonSerializer(typeof(Connectwithakey));
                    DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(ToCode));
                    // ещё один поиск устройств
                   // BluetoothClient bc = new BluetoothClient();
                   // BluetoothDeviceInfo[] info = null;
                   // info = bc.DiscoverDevices();
                    // чтение основного файла
                    FileStream st = new FileStream(path, FileMode.Open);
                    Connectwithakey con = (Connectwithakey)json2.ReadObject(st);
                    FindkeyNumber = con.file;
                    // номер ключа
                    this.num = int.Parse(Encoding.ASCII.GetString(con.keynumber));
                    st.Close();
                    // извлечение ключа из файла
                    byte[] serkey = File.ReadAllBytes($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/PakmanD{num}.txt");
                    //безопастность (учетная запись)
                    IntPtr accountToken = WindowsIdentity.GetCurrent().Token;
                    WindowsIdentity win = new WindowsIdentity(accountToken);
                    // раскодируем
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
                    string help = $"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/PakmanD{num}Help.txt";
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
                    BluetoothComponent component = new BluetoothComponent();

                    component.DiscoverDevicesProgress += BluetoothDescovery;
                    component.DiscoverDevicesComplete += BluetoothEndDescovery;
                    component.DiscoverDevicesAsync(10, true, false, true, true, 0);
                }
                
            }
            else
            {
                string k = Path.GetExtension(path);
                if (k == ".code3")
                {
                    button1.Enabled = false;
                    DataContractJsonSerializer json2 = new DataContractJsonSerializer(typeof(Connectwithakey));
                    DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(ToCode));
                    // ещё один поиск устройств
                    // BluetoothClient bc = new BluetoothClient();
                    // BluetoothDeviceInfo[] info = null;
                    //info = bc.DiscoverDevices();
                    // чтение основного файла
                    FileStream st = new FileStream(path, FileMode.Open);
                    Connectwithakey con = (Connectwithakey)json2.ReadObject(st);
                    FindkeyNumber = con.file;
                    // номер ключа
                    this.num = int.Parse(Encoding.ASCII.GetString(con.keynumber));
                    st.Close();
                    // извлечение ключа из файла
                    byte[] serkey = File.ReadAllBytes($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Pakman{num}.txt");
                    //безопастность (учетная запись)
                    IntPtr accountToken = WindowsIdentity.GetCurrent().Token;
                    WindowsIdentity win = new WindowsIdentity(accountToken);
                    // раскодируем
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
                    string help = $"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Pakman{num}Help.txt";
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
                    // while (compare == 0)
                    //{

                    //}
                    //foreach (BluetoothDeviceInfo item in info)
                    //{
                    //    if (item.DeviceName == cod.DeviceName && item.DeviceAddress.ToString() == cod.DeviceAdress)
                    //    {
                    //        b = true;
                    //    }
                    //}

                }
                else
                {
                    DataContractJsonSerializer json2 = new DataContractJsonSerializer(typeof(Connectwithakey));
                    DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(ToCode));
                    // ещё один поиск устройств
                    // BluetoothClient bc = new BluetoothClient();
                    // BluetoothDeviceInfo[] info = null;
                    // info = bc.DiscoverDevices();
                    // чтение основного файла
                    FileStream st = new FileStream(path, FileMode.Open);
                    Connectwithakey con = (Connectwithakey)json2.ReadObject(st);
                    FindkeyNumber = con.file;
                    // номер ключа
                    this.num = int.Parse(Encoding.ASCII.GetString(con.keynumber));
                    st.Close();
                    // извлечение ключа из файла
                    byte[] serkey = File.ReadAllBytes($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/PakmanD{num}.txt");
                    //безопастность (учетная запись)
                    IntPtr accountToken = WindowsIdentity.GetCurrent().Token;
                    WindowsIdentity win = new WindowsIdentity(accountToken);
                    // раскодируем
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
                    string help = $"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/PakmanD{num}Help.txt";
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
        }
    }
}
