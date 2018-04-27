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
        bool b;
        public Form2(string patharg, bool b)
        {
            InitializeComponent();
            path = patharg;
            // индексатор поиска
            label1.Visible = false;
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
            if (((ListBox)(sender)).SelectedItem != null)
            {
                phone = ((ListBox)(sender)).SelectedItem.ToString();
                // захват названия и адреса теефона
                if (phone != "")
                {
                    foreach (BluetoothDeviceInfo device in info)
                    {
                        if (phone.Contains(device.DeviceName))
                        {
                            // папка для хранения уже использованных девайсов
                            string pathdev = $"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Devices.txt";
                            // массив уже использованных
                            //string[] before = null;
                            //if (File.Exists(pathdev))
                            //{
                            //    before = File.ReadAllLines(pathdev);
                            //    File.Delete(pathdev);
                            //}
                            //// переводим адрес и имя в байты и для того чтобы скрыть адрес ксорим имя с адресом
                            byte[] devname = Encoding.ASCII.GetBytes(device.DeviceName);
                            byte[] devadress = Encoding.ASCII.GetBytes(device.DeviceAddress.ToString());
                            byte [] devad = new byte[devadress.Length];
                            int q = 0;
                            for (int i = 0; i < devadress.Length; i++)
                            {
                              if (q == devname.Length)
                                    q = 0;
                                devad[i] = (byte)(devname[q] ^ devadress[i]);
                                q++;
                            }
                            //// создаем строчку и добавляем в файл телефонов
                            //string together = device.DeviceName + " bluetooth " + Encoding.ASCII.GetString(devadress);
                            //if (before != null)
                            //{
                            //    Array.Resize(ref before, before.Length + 1);
                            //    before[before.Length - 1] = together;
                            //}
                            //else
                            //{
                            //    before = new string[1];
                            //    before[0] = together;
                            //}
                            //File.WriteAllLines(pathdev, before);
                            // вызов формы запроса пароля
                            Devices  dev = new Devices(devname, devad);
                            if (File.Exists(pathdev) && File.ReadAllLines(pathdev)[0] != "")
                            {
                                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Devices[]));
                                    FileStream str = new FileStream(pathdev, FileMode.OpenOrCreate);
                                    Devices[] devices = (Devices[])ser.ReadObject(str);
                                    str.Close();
                                    Array.Resize(ref devices, devices.Length + 1);
                                    devices[devices.Length - 1] = dev;
                                    File.Delete(pathdev);
                                    str = new FileStream(pathdev, FileMode.OpenOrCreate);
                                    ser.WriteObject(str, devices);
                            }
                            else
                            {
                                Devices[] devices = new Devices[1];
                                devices[0] = dev;
                                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Devices[]));
                                FileStream str = new FileStream(pathdev, FileMode.OpenOrCreate);
                                ser.WriteObject(str, devices);
                            }
                            Form3 Passwordandshifr = new Form3(path, device.DeviceName, device.DeviceAddress.ToString(), b);
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

        private void Code_Click(object sender, EventArgs e)
        {
            // проверка - включен ли Bluetooth
            if (BluetoothRadio.IsSupported)
            {
                label1.Visible = true;
                label1.Refresh();
                label1.Update();
                Code.Enabled = false;
                // ищем устройства
                BluetoothClient bc = new BluetoothClient();
                info = null;
                info = bc.DiscoverDevices();
                // переносим информацию в список устройств
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
                // меняем название кнопки
                if (Code.Text == "Start")
                    Code.Text = "Restart";
                Code.Enabled = true;
                label1.Visible = false;
            }
            else
            {
                MessageBox.Show("Please turn on bluetooth on your computer");
            }
        }

        // кнопка выбора из старых
        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Devices.txt"))
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
   

