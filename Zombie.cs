using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Purgatory
{
    /// <summary>
    /// Класс зомби
    /// </summary>
    public class Zombie : Monster
    {
        /// <summary>
        /// Конструктор класса зомби
        /// </summary>
        public Zombie()
        {
            Speed = 40;//задаем скорость передвижения зомби
            Harm = 2;//степень нанесения ущерба охотнику
            Health = 1;//уровень здоровья
            ItemSize = new Size(160, 220);//размер  картинки с изображением зомби
            Picture = new Bitmap(Properties.Resources.zombie, ItemSize);//изображение зомби
        }
    }
}
