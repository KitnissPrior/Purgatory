using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Purgatory
{
    /// <summary>
    /// Класс пули
    /// </summary>
    public class Bullet: GameItem
    {
        /// <summary>
        /// Конструктор класса пули
        /// </summary>
        /// <param name="position">Положение пули</param>
        public Bullet(Point position)
        {
            Speed = 25;//скорость пули
            Position = position;//положение пули
            ItemSize = new Size(12, 12);//размер картинки с изображением пули
            //картинка с изображением пули
            Picture = new Bitmap(Properties.Resources.bullet, ItemSize);
        }
    }
}
