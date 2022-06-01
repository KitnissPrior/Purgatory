using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Purgatory
{
    /// <summary>
    /// Класс игрового объекта
    /// </summary>
   public class GameItem
    {
        public int Speed;//скорость передвижения
        public Point Position;//положение объекта на игровой сцене
        public Bitmap Picture;//картинка с изображением объекта
        public Size ItemSize;//размер картинки с изображением объекта
        private Point futurePosition;//положение, в которое собирается переместиться объект
        public bool PositionChanged = false;//изменилось ли положение объекта на игровой сцене
        public List<Bitmap> DeathAnimation;//список спрайтов для анимации умирания объекта
        public bool ShouldShowDead = false;//нужно ли показывать умирание объекта

        /// <summary>
        /// Выполняет перемещение объекта
        /// </summary>
        /// <param name="newPosition">Новые координаты объекта</param>
        public void Move()
        {
            //если перемещаться можно - изменяем координаты элемента
            if(PositionChanged) Position = futurePosition;
            PositionChanged = false;//больше положение элемента не меняется
        }

        /// <summary>
        /// Проверяет, находится ли игровой объект в пределах формы
        /// </summary>
        /// <param name="formSize">Размеры формы</param>
        /// <param name="item">Игровой объект</param>
        /// <returns>Истина или ложь в зависимости от того, 
        /// находится ли игровой объект в пределах формы</returns>
        public bool InBounds(Size formSize, MovingDirection direction)
        {
            switch (direction)//если направление движения
            {
                case MovingDirection.Forward://вперед - перемещаемся вперед
                    futurePosition = new Point(Position.X + Speed, Position.Y);
                    break;
                case MovingDirection.Back://назад - перемещаемся назад 
                    futurePosition = new Point(Position.X - Speed, Position.Y);
                    break;
                case MovingDirection.Up://вверх - перемещаемся вверх 
                    futurePosition = new Point(Position.X, Position.Y - Speed / 2);
                    break;
                case MovingDirection.Down://вниз - перемещаемся вниз
                    futurePosition = new Point(Position.X, Position.Y + Speed/2);
                    break;
            }
            //если объект, сделав один ход, останется в пределах формы, его позиция изменится
            PositionChanged = (futurePosition.X + ItemSize.Width * 1.03 < formSize.Width &&
                futurePosition.X + formSize.Width / 50 > 0 &&
                futurePosition.Y + ItemSize.Height < formSize.Height - formSize.Height/13 &&
                futurePosition.Y > formSize.Height - (formSize.Height / 11) * 7);
            //возвращаем истину или ложь в зависимости от того, могут ли измениться координаты объекта
            return PositionChanged;
        }
    }
}
