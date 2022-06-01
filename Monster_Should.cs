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
    /// Класс, тестирующий методы класса монстра
    /// </summary>
    [TestFixture]
    class Monster_Should
    {
        private Monster monster;//монстр
        private Bullet bullet;//пуля

        /// <summary>
        /// Устанавливает основные параметры монстра
        /// </summary>
        [SetUp]
        protected virtual void Setup()
        {
            monster = new Monster();//инициализируем монстра
            monster.Position = new Point(0,0);//задаем начальное положение  монстра
        }

        /// <summary>
        /// Монстр не должен получать ранение, когда пуля далеко от него
        /// </summary>
        [Test]
        public void NotHurt_WhenBulletIsFar()
        {
            //положение пули относительно монстра
            var bulletPosition = new Point(monster.Position.X - 100, monster.Position.Y);
            //инициализируем пулю
            bullet = new Bullet(bulletPosition);
            //максимальный уровень здоровья монстра
            var maxHealth = monster.Health;
            //проверяем столкновение монстра с пулей
            monster.CheckCollisions(bullet);
            //здоровье монстра должно остаться на максимальном уровне
            Assert.AreEqual(maxHealth, monster.Health);
        }

        /// <summary>
        /// Монстр должен получить ранение, когда его касается пуля
        /// </summary>
        [Test]
        public void GetHurt_WhenCollidedWithBullet()
        {
            //координаты пули совпадают с координатами монстра
            var bulletPosition = monster.Position;           
            bullet = new Bullet(bulletPosition);//инициализируем пулю
            //сниженный на единицу уровень здоровья монстра
            var reducedHealth = monster.Health - 1; 
            //проверяем столкновение монстра с пулей
            monster.CheckCollisions(bullet);         
            //здоровье монстра должно уменьшиться на единицу
            Assert.AreEqual(reducedHealth, monster.Health);
        }

        /// <summary>
        /// Монстр должен прекратить движение, когда его убивает охотник
        /// </summary>
        [Test]
        public void StopMoving_WhenIsDead()
        {
            //пусть у монстра останетсяя только одна единица здоровья
            monster.Health = 1;
            //инициализируем пулю
            bullet = new Bullet(monster.Position);
            //проверяем столкновение монстра с пулей
            monster.CheckCollisions(bullet);
            //монстр должен перестать двигаться, т.к он должен умереть
            Assert.AreEqual(false, monster.IsMonsterMoving);
        }
    }
}
