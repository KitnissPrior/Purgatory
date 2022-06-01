using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using NUnit.Framework;

namespace Purgatory
{
    /// <summary>
    /// Класс, тестирующий методы класса игрового объекта
    /// </summary>
    [TestFixture]
    class GameItem_Should
    {
        //игровой объект
        private GameItem item;
        //размеры формы
        private Size formSize;

        /// <summary>
        /// Устанавливает основные параметры игрового объекта
        /// </summary>
        [SetUp]
        protected virtual void Setup()
        {
            item = new GameItem();//инициализируем игровой объект
            item.ItemSize = new Size(30, 30);//задаем размеры объекта
            item.Speed = 10;//задаем скорость передвижения объекта
            formSize = new Size(200, 200);//задаем размеры формы
        }

        /// <summary>
        /// Проверяет текущее положение объекта
        /// </summary>
        /// <param name="direction">Направление движения</param>
        /// <param name="expectedPosition">Позиция,
        /// в которой должен оказаться объект после движения</param>
        public void CheckPosition(MovingDirection direction, Point expectedPosition)
        {    
            //если объект будет находиться в пределах формы в результате своего движения
            if (item.InBounds(formSize, direction))
                item.Move();//объект перемещается
            //проверяем, правильно ли переместился объект
            Assert.AreEqual(expectedPosition, item.Position);
        }

        /// <summary>
        /// Объект должен двигаться, когда у него есть пространство для движения
        /// </summary>
        [Test]
        public void Move_WhenInBounds()
        {
            //начальное положение объекта - по центру формы
            item.Position = new Point(formSize.Width/2, formSize.Height/2);

            //проверяем положение объекта для смещения вперед
            var position = new Point(item.Position.X + item.Speed, item.Position.Y);
            CheckPosition(MovingDirection.Forward, position);
            //проверяем положение объекта для смещения назад
            position = new Point(item.Position.X - item.Speed, item.Position.Y);
            CheckPosition(MovingDirection.Back, position);
            //проверяем положение объекта для смещения вниз
            position = new Point(item.Position.X, item.Position.Y + item.Speed/2);
            CheckPosition(MovingDirection.Down, position);
            //проверяем положение объекта для смещения вверх
            position = new Point(item.Position.X, item.Position.Y - item.Speed / 2);
            CheckPosition(MovingDirection.Up, position);
        }

        /// <summary>
        /// Объект должен стоять на месте, когда ему некуда двигаться
        /// </summary>
        [Test]
        public void Stay_WhenNowhereToMove()
        {
            //начальное положение объекта:
            var rightSide = new Point(formSize.Width - item.ItemSize.Width, 100); //у правого края формы
            var leftSide = new Point(0, 100);//у левого края формы
            var downSide = new Point(10, formSize.Height - item.ItemSize.Height);//внизу формы
            var upSide = new Point(10, 0);//вверху формы

            //проверяем, что объект стоит на месте: 
            item.Position = rightSide;//когда объект находится в правом углу формы
            CheckPosition(MovingDirection.Forward, rightSide);
            item.Position = leftSide;//когда объект находится в левом углу формы
            CheckPosition(MovingDirection.Back, leftSide);
            item.Position = downSide;//когда объект находится в нижней части формы
            CheckPosition(MovingDirection.Down, downSide);
            item.Position = upSide;//когда объект находится в верхней части формы
            CheckPosition(MovingDirection.Up, upSide);
        }
    }
}
