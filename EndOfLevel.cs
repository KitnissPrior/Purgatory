using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Purgatory
{
    /// <summary>
    /// Класс, описывающий окончание уровня
    /// </summary>
    public partial class EndOfLevel : Form
    {
        private readonly bool isSuccessful;//был ли уровень завершен успешно
        private readonly int levelNumber;//номер текущего уровня
        private Label comments;//комментарии к уровню
        private Button closingButton;//кнопка, закрывающая окно
        private string firstSuccessful;//комментарий к пройденному 1 уровню
        private string secondSuccessful;//комментарий к пройденному 2 уровню
        public bool ShouldExit = false;//нужно ли выходить из игры

        /// <summary>
        /// Конструктор класса, описывающего окончание уровня
        /// </summary>
        /// <param name="isLevelSuccessful">Был ли уровень успешно пройден</param>
        /// <param name="number">Номер уровня</param>
        public EndOfLevel(bool isLevelSuccessful, int number)
        {
            isSuccessful = isLevelSuccessful;
            levelNumber = number;
            InitializeComponent();
            DoubleBuffered = true;//включаем двойную буферизацию, чтобы картинки при перерисовке не мерцали
            firstSuccessful = "Вы справились с зомби!\nТеперь остался последний шаг: схватка с левиафаном - самым страшным монстром в Чистилище.";
            secondSuccessful = "Вы одолели всех монстров и вернулись домой!";
            Size = new Size(400, 300);//размер формы
            //начальное положение формы - по центру экрана
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.Black;//цвет фона - черный
            MaximizeBox = false; //запрещаем менять размеры окна
            //заголовок формы зависит от того, был ли пройден уровень
            Text = isLevelSuccessful ? "Ура!" : "Конец";
            ShowIcon = false;//прячем иконку
            InitializeControls();//инициализируем элементы интерфейса
        }

        /// <summary>
        /// Выполняет отрисовку элементов формы 
        /// </summary>
        /// <param name="e">Событие</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            if (!isSuccessful)//если уровень был не пройден
            {
                var size = new Size(110, 110);//размер картинки
                //положение картинки
                var position = new Point(closingButton.Location.X, ClientSize.Height / 2 - 60);
                //картинка с черепом
                var skull = new Bitmap(Properties.Resources.skull, size);
                //рисуем череп, символизирующий смерть охотника
                graphics.DrawImage(skull, position);
            }
            //если пройден 2 уровень - охотник вернулся из Чистилища домой
            else if (levelNumber == 2)
            {
                var size = new Size(140, 140);//размер картинки с домиком
                //положение картинки с домиком
                var position = new Point(ClientSize.Width / 2 - size.Height / 2, ClientSize.Height / 2 - 60);
                var home = new Bitmap(Properties.Resources.home,size);//картинка с домиком
                graphics.DrawImage(home, position);//рисуем домик
            }
        }

        /// <summary>
        /// Инициализируем элементы интерфейса
        /// </summary>
        private void InitializeControls()
        {
            //комментарии к уровню
            comments = new Label
            {
                Size = new Size(350, 80),
                Location = new Point(ClientSize.Width / 12, ClientSize.Height / 8),
                ForeColor = Color.White,
                Font = new Font("Bradley Hand ITC", 12.0F)
            };
            //кнопка, закрывающая окно
            closingButton = new Button
            {
                Text = "Вперёд!",
                Size = new Size(95, 30),
                Location = new Point(ClientSize.Width / 3 + 20, ClientSize.Height/2 + 90),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                Font = new Font("Bradley Hand ITC", 9.2F)
            };
            //если нажали кнопку, закрывающую окно
            closingButton.Click += (sender, args) =>
            {
                //если уровень не был пройден или был пройден последний уровень
                if (!isSuccessful || isSuccessful && levelNumber == Purgatory.LevelsCount) 
                    ShouldExit = true;//нужно выйти из игры
                Close();//закрываем окно
            };
            SetControlsValues(); //устанавливаем значение добавленной кнопки и текстового поля
            //добавляем кнопку и комметарии на форму
            Controls.Add(closingButton);
            Controls.Add(comments);
        }

        /// <summary>
        /// Устанавливает значения элементов интерфейса
        /// </summary>
        private void SetControlsValues()
        {
            if (isSuccessful)//если уровень был успешно пройден
            {
                if (levelNumber == 1)//если был пройден 1 уровень
                {
                    comments.Text = firstSuccessful;
                    comments.Location = new Point(ClientSize.Width / 12, ClientSize.Height / 4);
                }    
                    
                else
                {   //если был пройден 2 уровень
                    closingButton.Text = "Домой!";
                    comments.Size = new Size(350, 30);
                    comments.Text = secondSuccessful;
                }
                closingButton.BackColor = Color.LightGray;
            }
            else //иначе охотник погиб
            {
                comments.Size = new Size(350, 30);
                comments.Text = "Вас съели";
                closingButton.Text = "Выйти из игры";
            }
        }
    }
}
