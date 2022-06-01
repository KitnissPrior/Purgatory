using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Purgatory
{
    /// <summary>
    /// Класс левиафана (главного босса)
    /// </summary>
    public class Leviathan: Monster
    {
        /// <summary>
        /// Конструктор класса левиафана
        /// </summary>
        public Leviathan()
        {
            Speed = 10;//скорость левиафана
            Health = 20;//здоровье левиафана
            Position = new Point(0, 0);//начальное положение левиафана
            Harm = 5;//степень нанесения ущерба
            ItemSize = new Size(200, 100);//размер картинки с левиафаном
            Picture = new Bitmap(Properties.Resources.leviathan);//картинка с изображением левиафана
        }
    }
}
