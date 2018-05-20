using System;
using System.Windows.Forms;

namespace ToCreate
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        ///  
       static int count = 0;
        [STAThread]
       
        static void Main(string[] args)
        {
            count++;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
          //try
         // {
         
                Application.Run(new Startfrom1(args));
         // }
         // catch (System.ObjectDisposedException)
         // {

         // }
            Application.Exit();
        }
    }
}
