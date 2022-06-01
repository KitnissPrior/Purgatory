using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Purgatory
{
    static class Program
    {
        //основное окно игры
        public static PurgatoryForm MyPurgatoryForm;

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MyPurgatoryForm = new PurgatoryForm();//инициализируем основное окно игры
            Purgatory.AddZombie();//запускаем зомби в игру
            Application.Run(MyPurgatoryForm);//запускаем основное окно игры
        }
    }
}
