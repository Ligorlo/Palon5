using System;
using System.Windows.Forms;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using System.IO;
using System.Text;

namespace ToCreate
{
    public partial class Form2 : Form
    {
        public delegate void meth();
        // поле пути к файлу, который надо закодировать
        string path;
        public Form2(string patharg)
        {
            InitializeComponent();
            path = patharg;
            label1.Visible = false;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        // массив инфорации о найденных девайсах
        BluetoothDeviceInfo[] info;
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
                            // форма запроса пароля и кодирования
                            string pathdev = $"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Devices.txt";
                            string[] before = null;
                            if (File.Exists(pathdev))
                            {
                               before =  File.ReadAllLines(pathdev);
                                File.Delete(pathdev);
                            }
                            byte[] devname = Encoding.ASCII.GetBytes(device.DeviceName);
                            byte[] devadress = Encoding.ASCII.GetBytes(device.DeviceAddress.ToString());
                            int q = 0;
                            for (int i = 0; i < devadress.Length; i++)
                            {
                                if (q == devname.Length)
                                    q = 0;
                                devadress[i] = (byte)(devname[q] ^ devadress[i]);
                                q++;
                            }
                            string together  = device.DeviceName + " bluetooth " + Encoding.ASCII.GetString(devadress);
                            if(before != null)
                            {
                                Array.Resize(ref before, before.Length + 1);
                                before[before.Length - 1] = together;
                            }
                            else
                            {
                                before = new string[1];
                                before[0] = together;
                            }
                            File.WriteAllLines(pathdev, before);
                           Form3 Passwordandshifr = new Form3(path, device.DeviceName, device.DeviceAddress.ToString());
                            Passwordandshifr.ShowDialog();
                            this.Close();
                        }
                    }
                }
            }
        }

        private void Code_Click(object sender, EventArgs e)
        {
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
            if(File.Exists($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Devices.txt"))
            {
                Form7 saved = new Form7(path);
                saved.ShowDialog();
                if(saved.Boo == true)
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
}
   

