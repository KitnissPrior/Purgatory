using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Purgatory
{
    /// <summary>
    /// Класс формы с игровой сценой Чистилища
    /// </summary>
    public partial class PurgatoryForm : Form
    {
        public readonly Point HunterStartPosition;//начальная позиция охотника
        public Dictionary<int, Monster> MonstersData;//словарь, содержащий живых монстров и их номера
        public Dictionary<int, Bullet> BulletsData;//словарь, содержащий все активные пули и их номера
        public ProgressBar HealthPanel;//панель здоровья охотника
        public BeginningForm InstructionsForm;

        /// <summary>
        /// Конструктор класса формы Чистилища
        /// </summary>
        public PurgatoryForm()
        {
            InitializeComponent();
            MonstersData = new Dictionary<int, Monster>();//инициализируем словарь зомби
            BulletsData = new Dictionary<int, Bullet>();//инициализируем словарь с пулями
            DoubleBuffered = true;//включаем двойную буферизацию, чтобы картинки при перерисовке не мерцали
            //начальное положение формы - по центру экрана
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1000, 560);//начальные размеры формы
            Text = "Purgatory";//название формы
            MaximizeBox = false;//запрещаем менять размеры окна

            //задаем начальные координаты охотника
            HunterStartPosition = new Point(ClientSize.Width / 6, ClientSize.Height - (ClientSize.Height / 11) * 6);
            Purgatory.PlayerHunter.Position = HunterStartPosition;
            HealthPanel = new ProgressBar()//панель здоровья охотника
            {
                //панель располагается над головой охотника
                Location = new Point(HunterStartPosition.X + 30, HunterStartPosition.Y - 30),
                Size = new Size(100, 20),
                Maximum = Purgatory.PlayerHunter.Health,
                Minimum = 0,
                Value = Purgatory.PlayerHunter.Health//изначально здоровье охотника максимальное
            };
            Controls.Add(HealthPanel);//добавляем панель здоровья на форму
            ShowHealthLevel();//отображаем уровень здоровья охотника

            InstructionsForm = new BeginningForm();//окно с инструкцией к игре
            InstructionsForm.ShowDialog();//запускаем окно с инструкцией к игре
        }

        /// <summary>
        /// Выполняет отрисовку элементов формы 
        /// </summary>
        /// <param name="e">Событие</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            //картинка с фоном размером на всё окно
            var background = new Bitmap(Properties.Resources.forest, 
                new Size(ClientSize.Width, ClientSize.Height));
            graphics.DrawImage(background, new Point(0,0));//рисуем фон

            var hunterPosition = Purgatory.PlayerHunter.Position;//положение охотника на форме
            //положение панели здоровья охотника
            HealthPanel.Location = new Point(hunterPosition.X + 30, hunterPosition.Y - 30);          
            var hunterImage = Purgatory.PlayerHunter.Picture;//картинка с изображением охотника

            if (Purgatory.PlayerHunter.Health > 0)//если охотник живой
            {
                if (Purgatory.Portal != null)//если портал уже создан - отрисовываем портал
                    graphics.DrawImage(Purgatory.Portal, Purgatory.PortalPosition);
                graphics.DrawImage(hunterImage, hunterPosition);//отрисовываем охотника
                if (Purgatory.PlayerHunter.IsShooting)//если охотник стреляет - отрисовываем пулю
                    foreach (var bullet in BulletsData.Values)
                        graphics.DrawImage(bullet.Picture, bullet.Position);
            }
            foreach (var monster in MonstersData)//проходим по коллекции монстров
                //отрисовываем только тех монстров, которые двигаются
                if (monster.Value.IsMonsterMoving) 
                    graphics.DrawImage(monster.Value.Picture, monster.Value.Position);                   
        }

        /// <summary>
        /// Обрабатывает нажатия клавиш
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Событие - нажатая клавиша</param>
        private void PurgatoryForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)//если нажата клавиша 
            {
                case Keys.D://"D" (вперед)
                    //если охотнику не грозит выйти за пределы окна 
                    if (Purgatory.PlayerHunter.InBounds(Size, MovingDirection.Forward))
                    {
                        Purgatory.PlayerHunter.Move();//охотник делает шаг вперед
                        if (Purgatory.Portal != null) //если портал уже создан
                            Purgatory.CheckEnteringPortal();//проверяем, вошел ли охотник в портал
                        Invalidate();//выполняем перерисовку формы
                    }
                    break;
                case Keys.A://"A" (назад)
                    //если охотнику не грозит выйти за пределы окна
                    if (Purgatory.PlayerHunter.InBounds(Size, MovingDirection.Back))
                    {
                        Purgatory.PlayerHunter.Move();//охотник делает шаг назад
                        Invalidate();//выполняем перерисовку формы
                    }
                    break;
                case Keys.W://"W" (вверх)
                    //если охотнику не грозит выйти за пределы окна
                    if (Purgatory.PlayerHunter.InBounds(Size, MovingDirection.Up))
                    {
                        Purgatory.PlayerHunter.Move();//охотник делает шаг вверх
                        Invalidate();//выполняем перерисовку формы
                    }
                    break;
                case Keys.S://"S" (вниз)
                    //если охотнику не грозит выйти за пределы окна
                    if (Purgatory.PlayerHunter.InBounds(Size, MovingDirection.Down))
                    {
                        Purgatory.PlayerHunter.Move();//охотник делает шаг вниз
                        Invalidate();//выполняем перерисовку формы
                    }
                    break;
                case Keys.L://"выстрелить"
                    Purgatory.PlayerHunter.Shoot();//охотник стреляет
                    Invalidate();//выполняем перерисовку формы
                    break;
            }
        }

        /// <summary>
        /// Создает монстра
        /// </summary>
        /// <param name="newMonster">Новый монстр</param>
        /// <param name="number">Соответствующий монстру порядковый номер</param>
        public void CreateMonster(Monster newMonster, int number)
        {
            var rand = new Random();//задаем смещение монстра по Оу случайным образом
            var dy = rand.Next(-20, 20);
            //задаем начальные координаты монстра
            newMonster.Position = new Point(ClientSize.Width - newMonster.ItemSize.Width, HunterStartPosition.Y + dy);
            newMonster.IsMonsterMoving = true;//монстр еще живой, поэтому он двигается
            MonstersData[number] = newMonster;//добавляем в коллекцию монстра и его номер
            StartMonsterMove(number);//монстр под данным номером начал двигаться
        }

        /// <summary>
        /// Запускает таймер на перемещение пули
        /// </summary>
        /// <param name="bullet">Пуля</param>
        /// <param name="number">Порядковый номер пули</param>
        public void StartBulletMove(Bullet bullet, int number)
        {
            BulletsData[number] = bullet;//добавляем пулю в словарь
            var timer = new System.Windows.Forms.Timer();//создаем таймер для анимации перемещения
            timer.Interval = bullet.Speed;//интервал таймера равен скорости пули
            timer.Tick += (sender, args) => //по каждому тику таймера:
            {
                //если пуля находится в пределах формы
                if (bullet.InBounds(ClientSize, MovingDirection.Forward))
                    bullet.Move();//перемещаем пулю вперед
                else
                {
                    Purgatory.PlayerHunter.IsShooting = false;//охотник закончил стрелять
                    BulletsData.Remove(number);//удаляем из словаря пулю под данным номером
                    timer.Stop();//останавливаем таймер
                }
                Invalidate();//выполняем перерисовку пули
            };
            timer.Start();//запускаем таймер
        }

        /// <summary>
        /// Монстр начинает двигаться
        /// </summary>
        /// <param name="number">Номер монстра</param>
        public void StartMonsterMove(int number)
        {
            var monster = MonstersData[number];//берем из словаря монстра под данным номером
            var timer = new System.Windows.Forms.Timer();//создаем таймер для перемещения монстра
            //в зависимости от типа монстра  задаем скорость его перемещения (интервал таймера)
            timer.Interval = monster is Zombie ? monster.Speed * 7 : monster.Speed * 20;
            timer.Tick += (sender, args) =>//по каждому тику таймера
            {
                //если монстр находится в пределах формы
                if (monster.InBounds(ClientSize, MovingDirection.Back))
                {
                    monster.Move();//перемещаем монстра назад (в направлении к охотнику)
                    
                    //для каждой пули, выпущенной охотником
                    foreach (var bullet in BulletsData)
                    {   //проверяем, не столкнулся ли монстр с этой пулей
                        monster.CheckCollisions(bullet.Value);
                        if (!monster.IsMonsterMoving)//если монстр был убит
                        {
                            MonstersData.Remove(number);//удаляем монстра под текущим номером из словаря
                            BulletsData.Remove(bullet.Key);//удаляем из словаря пулю, убившую монстра
                            timer.Stop();//останавливаем таймер
                            break;//выходим из цикла
                        }
                    }
                    Purgatory.PlayerHunter.CheckCollisions(monster);//проверяем столкновение монстра с охотником
                }
                else
                {
                    //иначе монстр прекращает движение
                    monster.IsMonsterMoving = false;                   
                    MonstersData.Remove(number);//удаляем монстра под текущим номером из словаря
                    timer.Stop();//останавливаем таймер
                }
                //если охотника съели
                if (Purgatory.PlayerHunter.Health == 0)
                {
                    MonstersData.Clear();//удаляем всех монстров из коллекции
                    CompleteLevel(false);//уровень не пройден
                    timer.Stop();//останавливаем таймер
                }
                //если появились все зомби, и все монстры были убиты
                if (Purgatory.ZombieAppeared == Purgatory.ZombieCount && MonstersData.Count == 0)
                {
                    timer.Stop();//останавливаем таймер
                    if (Purgatory.LevelsCompleted.Count == 1)//если текущий уровень - первый
                        CompleteLevel(true);//информируем, что уровень завершен успешно
                    else //иначе добавляем на форму портал
                        Purgatory.AddPortal(new Point(ClientSize.Width, ClientSize.Height / 3));
                }               
                Invalidate();//выполняем перерисовку формы
            };
            timer.Start();//запускаем таймер
        }

        /// <summary>
        /// Отображает уровень здоровья охотника
        /// </summary>
        private void ShowHealthLevel()
        {
            var timer = new System.Windows.Forms.Timer();//создаем таймер
            //проверяем уровень здоровья охотника с частотой 100 мс
            timer.Interval = 100;
            timer.Tick += (sender, args) =>//по каждому тику таймера
            {
                //если охотник ранен, но еще жив
                if (Purgatory.PlayerHunter.IsHurt && Purgatory.PlayerHunter.Health > 0)
                {
                    //уменьшаем количество здоровья охотника в зависимости от степени урона,
                    //который наносит монстр
                    HealthPanel.Value = Purgatory.PlayerHunter.Health;
                    //охотника закончили калечить
                    Purgatory.PlayerHunter.IsHurt = false;
                }
                if (Purgatory.PlayerHunter.Health == 0)//если охотника убили
                {
                    timer.Stop();//останавливаем таймер
                    HealthPanel.Value = 0;//здоровье охотника равно нулю
                    HealthPanel.Hide();//прячем панель здоровья
                }
            };
            timer.Start();//запускаем таймер
        }

        /// <summary>
        /// Выводит окошко, информирующее о завершении уровня
        /// </summary>
        /// <param name="isSuccessfull">Был ли успешно завершен уровень</param>
        public void CompleteLevel(bool isSuccessfull)
        {
            var levelsList = Purgatory.LevelsCompleted;//список завершенных уровней
            //окно с информацией о завершении уровня
            var endOfLevel = new EndOfLevel(isSuccessfull, Purgatory.LevelsCompleted.Count);
            //очередной уровень завершен
            Purgatory.LevelsCompleted[levelsList.Count - 1] = true;
            if (!isSuccessfull)//если уровень не пройден
            {
                Controls.Remove(HealthPanel);//удаляем панель здоровья
                MonstersData.Clear();//удаляем всех монстров из словаря
            }
            endOfLevel.ShowDialog();//выводим окно, информирующее о завершении уровня
            //если пользователь нажал "выйти" в диалоговом окне - выходим из игры
            if (endOfLevel.ShouldExit) Close();
            if (isSuccessfull)//если уровень пройден
            {
                if (levelsList.Count == 1)//если пройден только первый уровень
                {
                    //добавляем в список уровней следующий уровень (который пока не пройден)
                    Purgatory.LevelsCompleted.Add(false);
                    BulletsData.Clear();//удаляем все пули из словаря
                    Thread.Sleep(1000);//ждем секунду
                    Purgatory.AddLeviathan();//запускаем в игру левиафана - главного босса
                }                      
            }          
        }
    }
 }
