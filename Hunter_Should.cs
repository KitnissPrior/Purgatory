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
    /// Класс, тестирующий методы класса охотника
    /// </summary>
    [TestFixture]
    class Hunter_Should
    {
        private Hunter hunter;//охотник
        private Monster monster;//монстр

        /// <summary>
        /// Устанавливает параметры полей класса
        /// </summary>
        [SetUp]
        protected virtual void Setup()
        {
            hunter = new Hunter();//инициализируем охотника
            hunter.Position = new Point(100, 100);//координаты охотника
            monster = new Monster();//инициализируем монстра
            monster.Harm = 2;//урон, который наносит монстр
        }

        /// <summary>
        /// Охотник не должен получать ранение, когда монстр его не касается
        /// </summary>
        [Test]
        public void BeSafe_WhenMonsterIsFar()
        {
            //монстр находится справа от охотника и никак его не касается
            var monsterFarPosition = new Point(
                hunter.Position.X + hunter.ItemSize.Width + 100, 
                hunter.Position.Y);
            monster.Position = monsterFarPosition;
            var maxHeath = hunter.Health;//максимальный уровень здоровья охотника
            //проверяем, столкнулись ли монстр с охотником
            hunter.CheckCollisions(monster);
            //здоровье охотника должно остаться на максимальном уровне
            Assert.AreEqual(maxHeath, hunter.Health);
        }

        /// <summary>
        /// Охотник должен получать ранение при столкновении с монстром
        /// </summary>
        [Test]
        public void GetHurt_WhenCollidedWithMonster()
        {
            //положение монстра, при котором монстр касается охотника
            var monsterClosePosition = new Point(
                hunter.Position.X + hunter.ItemSize.Width - 2,
                hunter.Position.Y);
            monster.Position = monsterClosePosition;
            //при столкновении охотника с монстром монстр наносит ущерб здоровью охотника
            var reducedHeath = hunter.Health - monster.Harm;
            //проверяем, столкнулись ли монстр с охотником
            hunter.CheckCollisions(monster);
            //здоровье охотника должно уменьшиться
            Assert.AreEqual(reducedHeath, hunter.Health);
        }
    }
}
