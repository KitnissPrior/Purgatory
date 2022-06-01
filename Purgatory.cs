using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace Purgatory
{
    /// <summary>
    /// Класс Чистилища
    /// </summary>
    public static class Purgatory
    {
        public static Hunter PlayerHunter;//главный герой - охотник
        public readonly static int ZombieCount;//общее количество зомби в игре
        public static int ZombieAppeared;//счетчик появившихся зомби
        public static List<bool> LevelsCompleted;//количество пройденных уровней
        public static readonly int LevelsCount;//количество уровней
        public static Bitmap Portal;//портал - выход из Чистилища
        public static Point PortalPosition;//положение портала на форме

        /// <summary>
        /// Конструктор класса Чистилища
        /// </summary>
        static Purgatory()
        {
            //в начале игры создается охотник
            PlayerHunter = new Hunter();
            ZombieCount = 15;//количество зомби, которые появятся в игре

            LevelsCompleted = new List<bool>();//инициализируем список уровней
            LevelsCompleted.Add(false);//добавляем первый уровень (он пока не пройден)
            LevelsCount = 2;//задаем количество уровней
        }

        /// <summary>
        /// Добавляет зомби в игру
        /// </summary>
        public static void AddZombie()
        {
            //создаем таймер
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 2000;//изначально зомби появляются с интервалом 2 сек
            ZombieAppeared = 0;//изначально ни одного зомби не появилось
            timer.Tick += (sender, args) =>
            {
                //по тику таймера создается зомби с соответствующим ему порядковым номером
                Program.MyPurgatoryForm.CreateMonster(new Zombie(), ZombieAppeared);
                ZombieAppeared++;//увеличиваем кол-во зомби на 1
                timer.Interval -= 50;//постепенно увеличиваем частоту появления зомби
                //если появился последний зомби или охотника съели
                if (ZombieAppeared == ZombieCount || PlayerHunter.Health <= 0)
                    timer.Stop();// останавливаем таймер            
            };
            timer.Start();//запускаем таймер
        }

        /// <summary>
        /// Добавляет левиафана в игру
        /// </summary>
        public static void AddLeviathan()
        {
            //создаем левиафана
            Program.MyPurgatoryForm.CreateMonster(new Leviathan(), -1);
        }

        /// <summary>
        /// Добавляет портал
        /// </summary>
        /// <param name="position">Координаты портала</param>
        public static void AddPortal(Point position)
        {
            //размер портала
            var size = new Size(150, 300);
            //картинка с изображением портала
            Portal = new Bitmap(Properties.Resources.portal, size);
            //устанавливаем координаты портала
            PortalPosition = new Point(position.X - size.Width,position.Y);
        }

        /// <summary>
        /// Проверяет, вошел ли охотник в портал
        /// </summary>
        public static void CheckEnteringPortal()
        {
            //текущее положение охотника на форме с учетом его размеров
            var hunterPosition = PlayerHunter.Position.X + PlayerHunter.ItemSize.Width/3*2;
            //если охотник вошел в портал
            if (hunterPosition >= PortalPosition.X)
                Program.MyPurgatoryForm.CompleteLevel(true);//уровень пройден
        }
    }
}
