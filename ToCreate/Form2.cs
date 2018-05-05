﻿
using System;
using System.Windows.Forms;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Diagnostics;
namespace ToCreate
{
    /// <summary>
    /// Форма Bluetooth поиска устройства и выбора
    /// </summary>
    public partial class Form2 : Form
    {
        // поле пути к файлу, который надо закодировать
        string path;
        // булеое поле для определения нужно ли удалять файл ключа
        bool b;
        public Form2(string patharg, bool b)
        {
            InitializeComponent();
            path = patharg;
            // индексатор поиска
            Indexer.Visible = false;
            this.b = b;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
        }
        // массив инфорации о найденных девайсах
        BluetoothDeviceInfo[] info;
        // выбранный телефон
        string phone;
        // выбор устройства
        private void FoundDevices_DoubleClick(object sender, EventArgs e)
        {
            // проверка выбронного устройства
            if (((ListBox)(sender)).SelectedItem != null)
            {
                phone = ((ListBox)(sender)).SelectedItem.ToString();
                // захват названия и адреса теефона
                if (phone != "")
                {
                    foreach (BluetoothDeviceInfo device in info)
                    {
                        // проверка - совпадают ли имена
                        if (phone.Contains(device.DeviceName))
                        {
                            string pathdev = "";
                            // папка для хранения уже использованных девайсов
                            if (Directory.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming"))
                            {
                                if (!Directory.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon"))
                                Directory.CreateDirectory($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon");
                                    pathdev = $"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Devices.txt";
                            }
                            else
                            {

                                pathdev = $"../Devices.txt";
                            }
                            // имена устройств
                            byte[] devname = Encoding.ASCII.GetBytes(device.DeviceName);
                            // адреса устройств
                            byte[] devadress = Encoding.ASCII.GetBytes(device.DeviceAddress.ToString());
                            // прячем адрес ксоря с именем
                            byte [] devad = new byte[devadress.Length];
                            int q = 0;
                            for (int i = 0; i < devadress.Length; i++)
                            {
                              if (q == devname.Length)
                                    q = 0;
                                devad[i] = (byte)(devname[q] ^ devadress[i]);
                                q++;
                            }
                            // объект для сериализации с именем и изменнным адресом
                            Devices  dev = new Devices(devname, devad);
                            // записываем в файл 
                            if (File.Exists(pathdev) && File.ReadAllLines(pathdev)[0] != "")
                            {
                                // прочитываем то что уже есть в файле
                                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Devices[]));
                                    FileStream str = new FileStream(pathdev, FileMode.OpenOrCreate);
                                    Devices[] devices = (Devices[])ser.ReadObject(str);
                                    str.Close();
                                bool b = true;
                                // проверяем нет ли уже этого устройства в использованных
                                foreach (Devices item in devices)
                                {
                                    if (Encoding.ASCII.GetString(item.name) == Encoding.ASCII.GetString(devname) && Encoding.ASCII.GetString(item.adress) == Encoding.ASCII.GetString(devad))
                                    {
                                        b = false;
                                    }
                                }
                                // добавляем наше устройство 
                                if (b)
                                {
                                    Array.Resize(ref devices, devices.Length + 1);
                                    devices[devices.Length - 1] = dev;
                                    File.Delete(pathdev);
                                    str = new FileStream(pathdev, FileMode.OpenOrCreate);
                                    ser.WriteObject(str, devices);
                                    str.Close();
                                }
                            }
                            // папки не существует, надо создать новую
                            else
                            {
                                Devices[] devices = new Devices[1];
                                devices[0] = dev;
                                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Devices[]));
                                FileStream str = new FileStream(pathdev, FileMode.OpenOrCreate);
                                ser.WriteObject(str, devices);
                                str.Close();
                            }
                            // форма запроса пароля и шифрования
                            Form3 Passwordandshifr = new Form3(path, device.DeviceName, device.DeviceAddress.ToString(), b);
                            // вызов формы запроса пароля и шифрования
                            Passwordandshifr.ShowDialog();
                            this.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Something got wrong - please try again");
                }
            }
            else
            {
                MessageBox.Show("Please try again");
            }

        }
        // клиент блютус
        BluetoothClient blclient;
        // таймер 
        System.Windows.Forms.Timer timer;
        // кнопка поиска устройств в течение 13 секунд (+- 1)
    private void Code_Click(object sender, EventArgs e)
        {
            // проверка - включен ли Bluetooth
            if (BluetoothRadio.IsSupported)
            {
                blclient = new BluetoothClient();
                // индексатор поиска
                Indexer.Visible = true;
                Indexer.Refresh();
                Indexer.Update();
                // кнопка поиска не доступна во время поиска
                Searchdevice.Enabled = false;
                info = null;
                // кнопка выбора устройств из выбранных неудобна
                Choosefromused.Enabled = false;
                // старт поиска
                blclient.BeginDiscoverDevices(10, true, true, true, false,new AsyncCallback(Power), 1);
                // включение таймера для того чтобы в конце поиска активировать кнопки и записать в listbox наденные устройства
                timer = new System.Windows.Forms.Timer();
                timer.Interval = 1000;
                timer.Tick += Tick;
                timer.Start();
            }
            else
            {
                MessageBox.Show("Please turn on bluetooth on your computer");
            }
        }
       // метод записи устройств в массив после поиска
        public void Power(IAsyncResult s)
        {
            info = blclient.EndDiscoverDevices(s);
        }
        // метод проверки есть ли ещё устройства поблизости и запись их в FoundDevice
        public void Tick(object sender , EventArgs e)
        {
            if (info != null)
            {
                foreach (BluetoothDeviceInfo device in info)
                {
                    // проверка повтора устройств в списке
                    if (!FoundDevices.Items.Contains(device.DeviceName + " - " + device.DeviceAddress))
                    {
                        // добавление
                        FoundDevices.Items.Add(device.DeviceName + " - " + device.DeviceAddress);
                        FoundDevices.Refresh();
                        FoundDevices.Update();
                    }
                }
                // замена название кнопки 
                if (Searchdevice.Text == "Start")
                    Searchdevice.Text = "Restart";
                // активиуем кнопки
                Searchdevice.Enabled = true;
                // выключаем индексатор поиска
                Indexer.Visible = false;
                Choosefromused.Enabled = true;
                // выключаем таймер
                timer.Stop();
            }
        }
        // кнопка выбора из старых
        private void Already_used_Click(object sender, EventArgs e)
        {
            // проверка были ли устройства до 
            if (File.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Devices.txt")|| File.Exists($"../Devices.txt"))
            {
                // вызов формы выбора одного из сохраненных устройств
                Form7 saved = new Form7(path, b);
                saved.ShowDialog();
                // проверка - сделан ли уже выбор
                if (saved.Boo == true)
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Sorry, but there are no saved phones");
            }


        }
    }
    [DataContract]
    public class Devices
    {
        [DataMember]
        public byte[] name;
        [DataMember]
        public byte[] adress;
        public Devices (byte [] name, byte [] adress )
        {
            this.name = name;
            this.adress = adress;
        }
    }
}
   