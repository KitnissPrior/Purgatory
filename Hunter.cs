using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace Purgatory
{
    /// <summary>
    /// Класс охотника
    /// </summary>
    public class Hunter: GameItem
    {
        public int Scores;//баллы охотника
        public int Health;//здоровье охотника
        public bool IsShooting = false;//стреляет ли охотник в данный момент
        private int shootsCount;//кол-во сделанных выстрелов
        public bool IsHurt = false;//наносят ли охотнику урон в данный момент

        /// <summary>
        /// Конструктор класса охотника
        /// </summary>
        public Hunter()
        {
            Speed = 10;//скорость охотника
            Scores = 0;//в начале игры охотник еще баллов не набрал
            Health = 10;//здоровье охотника
            ItemSize = new Size(200, 240);//размер картинки с охотником
            Picture = new Bitmap(Properties.Resources.hunter, ItemSize);//картинка с охотником
        }

        /// <summary>
        /// Выполняет выстрел
        /// </summary>
        public void Shoot()
        {
            shootsCount++;//увеличиваем кол-во выстрелов на 1
            IsShooting = true;//охотник начал стрелять
            //положение пули относительно охотника
            var bulletPosition = new Point(Position.X + ItemSize.Width - 4, Position.Y + Position.Y / 3 - 2);
            Bullet bullet = new Bullet(bulletPosition);//создаем пулю
            //пуля начинает движение
            Program.MyPurgatoryForm.StartBulletMove(bullet,shootsCount);
        }

        /// <summary>
        /// Проверяет, столкнулся ли охотник с монстром
        /// </summary>
        /// <param name="monster">Монстр</param>
        public void CheckCollisions(Monster monster)
        {
            //если монстр задевает охотника
            if(monster.Position.X <= Position.X + ItemSize.Width - 2)
            {
                Health -= monster.Harm;//монстр наносит охотнику урон
                IsHurt = true;//охотник ранен
            }
        }
    }
}
