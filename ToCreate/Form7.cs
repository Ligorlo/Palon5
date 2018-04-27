using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
namespace ToCreate
{
    public partial class Form7 : Form
    {
        bool boo = true;
        public bool Boo
        {
            get
            {
                return boo;
            }
        }
        string path;
        string[,] toconsole;
        bool boool;
        public Form7(string path, bool boool)
        {
            this.boool = boool;
            this.path = path;
            InitializeComponent();
            FileStream s = new FileStream($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Devices.txt", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Devices[]));
            Devices [] device = (Devices [])ser.ReadObject(s);
            string[,] forlistbox = new string[device.Length, 2];
            for (int i = 0; i < forlistbox.GetLength(0); i++)
            {
                forlistbox[i, 0] = Encoding.ASCII.GetString(device[i].name);
                    forlistbox[i, 1] = Encoding.ASCII.GetString(device[i].adress);
                listBox1.Items.Add(forlistbox[i, 0]);
            }
            //string [] savednames = File.ReadAllLines($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Devices.txt");
            //string[,] forlistbox = new string[savednames.Length, 2];
            //for (int i = 0; i < savednames.Length; i++)
            //{
            //    string [] x = savednames[i].Split(new string[] { " bluetooth " }, StringSplitOptions.RemoveEmptyEntries);
            //    forlistbox[i, 0] = x[0];
            //    forlistbox[i, 1] = x[1];
            //    listBox1.Items.Add(forlistbox[i, 0]);
            //}
            toconsole = forlistbox;
            s.Close();


        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (((ListBox)(sender)).SelectedItem != null)
            {
              string  phone = ((ListBox)(sender)).SelectedItem.ToString();
                for (int i = 0; i < toconsole.GetLength(0); i++)
                {
                    if(toconsole[i, 0] == phone)
                    {
                        byte[] devname = Encoding.ASCII.GetBytes(toconsole[i,0]);
                        byte[] devadress = Encoding.ASCII.GetBytes(toconsole[i, 1]);
                        int q = 0;
                        for (int j = 0; j < devadress.Length; j++)
                        {
                            if (q == devname.Length)
                                q = 0;
                            devadress[j] = (byte)(devname[q] ^ devadress[j]);
                            q++;
                        }
                        toconsole[i, 1] = Encoding.ASCII.GetString(devadress);
                        Form3 pass = new Form3(path, toconsole[i, 0], toconsole[i, 1], boool );
                        pass.ShowDialog();
                        this.Close();
                        break;
                    }
                }
            }
        }
            private void button1_Click(object sender, EventArgs e)
        {
            boo = false;
            this.Close();
        }
    }
}
