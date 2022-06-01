using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Purgatory
{
    /// <summary>
    /// Класс монстра
    /// </summary>
    public class Monster : GameItem
    {
        public int Health;//здоровье монстра
        public int Harm;//размер ущерба, который наносит монстр охотнику
        public bool IsMonsterMoving = false;//двигается ли монстр (изначально не двигается)

        /// <summary>
        /// Проверяет столкновение монстра с пулей
        /// </summary>
        /// <param name="bullet">Пуля</param>
        public void CheckCollisions(Bullet bullet)
        {
            //если пуля находится внутри рамки, ограничивающей  картинку с монстром
            if (bullet.Position.X + bullet.ItemSize.Width >= Position.X)
                Health--;//у монстра уменьшается здоровье на одну единицу
            if (Health == 0)//если у монстра закончилось здоровье 
                IsMonsterMoving = false; //монстр больше не двигается,т.к его убили
        }
    }
}
