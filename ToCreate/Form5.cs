using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Security.Principal;
using System.IO.Compression;
namespace ToCreate
{
    // форма для шифрования (для безопасности шифрует если её закрываешь)
    public partial class Form5 : Form
    {
        // поле пути 
        string path;
        // поле ключа
        byte[] key;
        int num;
        bool directory;
        // для сериализации
        ToCode cod;
        public Form5(string path, byte[] key, ToCode cod, int  num, bool b, bool directory )
        {
            this.directory = directory;
            if (directory)
            {
                if (b == false)
                {
                    File.Delete($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/Pakman{num}.txt");
                    //this.Close();
                }
                else
                {
                    //Process s = new Process();
                    //this.FormClosing += proove;
                    this.key = key;
                    // изменяем обратно расширение
                    path = Path.ChangeExtension(path, cod.Rassh);
                    // 
                    this.path = path;
                    this.num = num;
                    this.cod = cod;
                    InitializeComponent();
                }
            }
            else
            {
                if (b == false)
                {
                    File.Delete($"C:/Users/{Environment.UserName}/AppData/Roaming/Palon/PakmanD{num}.txt");
                    //this.Close();
                }
                else
                {
                    //Process s = new Process();
                    //this.FormClosing += proove;
                    this.key = key;
                    // изменяем обратно расширение
                    // path = Path.ChangeExtension(path, "");
                    // 
                   
                    this.path = path.Remove(path.Length - 1);
                    this.num = num;
                    this.cod = cod;
                    InitializeComponent();
                }
            }


        }
        private void proove(/* object sender, FormClosingEventArgs e*/)
        {
            if (directory)
            {
                byte[] bytearr2 = File.ReadAllBytes(path);
                File.Delete(path);
                int q = 0;
                for (int i = 0; i < bytearr2.Length; i++)
                {
                    if (q == key.Length)
                        q = 0;
                    bytearr2[i] = (byte)(bytearr2[i] ^ key[q]);
                    q++;
                }
                Connectwithakey main = new Connectwithakey(bytearr2, Encoding.ASCII.GetBytes(num.ToString()));
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(Connectwithakey));
                FileStream file = new FileStream(path, FileMode.OpenOrCreate);

                json.WriteObject(file, main);
                file.Close();
                // end with main file
                // start with key file
                string pass = cod.Password;
                IntPtr accountToken = WindowsIdentity.GetCurrent().Token;
                WindowsIdentity win = new WindowsIdentity(accountToken);
                string ID = win.User.ToString();
                byte[] onlykey = Encoding.UTF8.GetBytes(ID);
                q = 0;
                for (int i = 0; i < key.Length; i++)
                {
                    if (q == onlykey.Length)
                        q = 0;
                    key[i] = (byte)(key[i] ^ onlykey[q]);
                }
                File.Move(path, Path.ChangeExtension(path, ".code3"));
                button1.Enabled = false;
            }
            else
            {
                ZipFile.CreateFromDirectory(path, Path.ChangeExtension(path, cod.Rassh));
                Directory.Delete(path, true);
                path = Path.ChangeExtension(path, cod.Rassh);
                byte[] bytearr2 = File.ReadAllBytes(path);
                File.Delete(path);
                int q = 0;
                for (int i = 0; i < bytearr2.Length; i++)
                {
                    if (q == key.Length)
                        q = 0;
                    bytearr2[i] = (byte)(bytearr2[i] ^ key[q]);
                    q++;
                }
                Connectwithakey main = new Connectwithakey(bytearr2, Encoding.ASCII.GetBytes(num.ToString()));
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(Connectwithakey));
                FileStream file = new FileStream(path, FileMode.OpenOrCreate);

                json.WriteObject(file, main);
                file.Close();
                // end with main file
                // start with key file
                string pass = cod.Password;
                IntPtr accountToken = WindowsIdentity.GetCurrent().Token;
                WindowsIdentity win = new WindowsIdentity(accountToken);
                string ID = win.User.ToString();
                byte[] onlykey = Encoding.UTF8.GetBytes(ID);
                q = 0;
                for (int i = 0; i < key.Length; i++)
                {
                    if (q == onlykey.Length)
                        q = 0;
                    key[i] = (byte)(key[i] ^ onlykey[q]);
                }
                File.Move(path, Path.ChangeExtension(path, ".code4"));
                button1.Enabled = false;
            }
        }
        // кнопка шифрования
        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists(path)||Directory.Exists(path))
            {
                proove();
                this.Close();
            }
            else
            {
                MessageBox.Show("Sorry, but we can't find your file, please put it back");
            }
        }
    }
}
