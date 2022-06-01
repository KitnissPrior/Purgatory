using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using NUnit.Framework;
using System.Threading;

namespace Purgatory
{
    /// <summary>
    /// Класс, тестирующий методы класса формы
    /// </summary>
    [TestFixture]
    class PurgatoryForm_Should
    {
        /// <summary>
        /// Устанавливает основные параметры окна Чистилища (инициализирует окно)
        /// </summary>
        [SetUp]
        protected virtual void Setup()
        {
            Program.MyPurgatoryForm = new PurgatoryForm();
        }

        /// <summary>
        /// Монстр должен добавляться в коллекцию монстров
        /// </summary>
        [Test]
        public void AddMonster()
        {
            //текущее количество монстров в коллекции
            var monstersCount = Program.MyPurgatoryForm.MonstersData.Count;
            //создаем монстра (пусть это будет зомби, роли не играет)
            Program.MyPurgatoryForm.CreateMonster(new Zombie(), 0);
            //количество монстров в коллекции должно увеличиться на единицу
            Assert.AreEqual(monstersCount + 1, Program.MyPurgatoryForm.MonstersData.Count);
        }
        
        /// <summary>
        /// Пуля должна добавляться в коллекцию пуль
        /// </summary>
        [Test]
        public void AddBullet()
        {
            //после выстрела количество пуль в коллекции пуль должно увеличиться на единицу
            var expectedCount = Program.MyPurgatoryForm.BulletsData.Count + 1;
            //пуля начинает двигаться
            Program.MyPurgatoryForm.StartBulletMove(new Bullet(new Point(0, 0)), 0);
            //количество пуль в коллекции после выстрела
            var actualCount = Program.MyPurgatoryForm.BulletsData.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        /// <summary>
        /// После прохождения первого уровня должен создаваться следующий
        /// </summary>
        [Test]
        public void CreteSecondLevel_IfFirstIsSuccessfull()
        {
            //очищаем список пройденных уровней
            Purgatory.LevelsCompleted.Clear();
            //добавляем первый уровень, который пока не пройден
            Purgatory.LevelsCompleted.Add(false);
            //теперь первый уровень пройден
            Program.MyPurgatoryForm.CompleteLevel(true);
            //в список уровней должен добавиться второй уровень
            Assert.AreEqual(2, Purgatory.LevelsCompleted.Count);
            //первый уровень в списке должен быть пройден
            Assert.IsTrue(Purgatory.LevelsCompleted.First());
        }


        /// <summary>
        /// Второй уровень не должен создаваться, если первый не был пройден
        /// </summary>
        [Test]
        public void NotCreateSecondLevel_IfFirstIsFailed()
        {
            //очищаем список пройденных уровней
            Purgatory.LevelsCompleted.Clear();
            //добавляем первый уровень, который пока не пройден
            Purgatory.LevelsCompleted.Add(false);
            //игрок не проходит первый уровень
            Program.MyPurgatoryForm.CompleteLevel(false);
            //количество уровней в списке не должно измениться
            Assert.AreEqual(1, Purgatory.LevelsCompleted.Count);
            //все монстры должны удалиться из коллекции монстров
            Assert.AreEqual(0, Program.MyPurgatoryForm.MonstersData.Count);
        }

        /// <summary>
        /// Левиафан (главный босс) должен появляться на втором уровне
        /// </summary>
        [Test]
        public void AddLeviathan_AtSecondLevel()
        {
            //очищаем список пройденных уровней
            Purgatory.LevelsCompleted.Clear();
            //добавляем первый уровень, который пока не пройден
            Purgatory.LevelsCompleted.Add(false);
            //теперь первый уровень пройден
            Program.MyPurgatoryForm.CompleteLevel(true);
            //первый монстр в коллекции монстров (на втором уровне он всего один)
            var monster = Program.MyPurgatoryForm.MonstersData.First().Value;
            //единственный монстр в игре должен быть левиафаном
            Assert.IsTrue(monster is Leviathan);
        }

        /// <summary>
        /// Игра должна завершаться, когда охотник достигает портала в мир людей
        /// </summary>
        [Test]
        public void CompleteGame_IfPortalIsReached()
        {
            //очищаем список пройденных уровней
            Purgatory.LevelsCompleted.Clear();
            //добавляем первый уровень, который уже пройден
            Purgatory.LevelsCompleted.Add(true);
            //добавляем второй уровнь, который пока не пройден
            Purgatory.LevelsCompleted.Add(false);
            //удаляем всех монстров из коллекции(как будто охотник их уже победил)
            Program.MyPurgatoryForm.MonstersData.Clear();

            var portalPosition = new Point(100, 100);//координаты портала
            Purgatory.AddPortal(portalPosition);//добавляем портал на форму
            //задаем координаты охотника так, как будто он уже вошел в портал
            Purgatory.PlayerHunter.Position = portalPosition;
            //проверям, вошел ли охотник в портал
            Purgatory.CheckEnteringPortal();
            //последний (второй) уровень должен быть пройден
            Assert.IsTrue(Purgatory.LevelsCompleted.Last());
        }
    }
}
