using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Purgatory
{
    /// <summary>
    /// Класс начальной формы с краткой инструкцией к игре
    /// </summary>
    public partial class BeginningForm : Form
    {
        private int laterClicksCount = 0;//количество нажатий по кнопке "далее"
        private Button laterButton;
        private Button closingButton;
        private Label aboutGame;
        private Label pageNumber;
        private List<string> instructions;

        /// <summary>
        /// Конструктор формы с инструкцией
        /// </summary>
        public BeginningForm()
        {
            InitializeComponent();
            //строки, описывающие игру
            string purgatory = "Чистилище (англ. purgatory) – это место, куда попадают монстры после смерти.";
            string situation = "В результате неудачно сработавшего заклинания вас забрасывает в самое сердце Чистилища. При вас только верный револьвер и немного отваги.";
            string toDo = "Ваша задача – добраться до портала в мир людей, истребив на своем пути как можно больше чудовищ.";
            string control = "Перемещайте вашего охотника по Чистилищу с помощью клавиш A,W,S,D.\nЧтобы выстрелить, нажмите L";
            string goodLuck = "Желаем удачи!";

            //список инструкций к игре
            instructions = new List<string>();
            instructions.Add(purgatory);
            instructions.Add(situation);
            instructions.Add(toDo);
            instructions.Add(control);
            instructions.Add(goodLuck);

            //размер формы
            Size = new Size(490, 350);
            //начальное положение формы - по центру экрана
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.Black;
            //запрещаем менять размеры окна
            MaximizeBox = false;
            Text = "Об игре Purgatory";//заголовок формы
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
            var skullSize = new Size(70, 70);//размер картинки с черепом
            var skullPosition = new Point(350, 160);//положение картинки с черепом
            //картинка с черепом
            var skull = new Bitmap(Properties.Resources.skull, skullSize);
            var portalSize = new Size(65, 100);//размер портала
            //положение портала
            var portalPosition = new Point(ClientSize.Width / 2, skullPosition.Y - 26);
            var portal = new Bitmap(Properties.Resources.portal, portalSize);
            graphics.DrawImage(skull, skullPosition);//отрисовка черепа
            switch(laterClicksCount)//если на кнопку "далее" нажали:
            {
                case 2://два раза
                    //"закрашиваем" череп черным прямоугольником
                    graphics.FillEllipse(Brushes.Black, skullPosition.X, skullPosition.Y,
                skullSize.Width, skullSize.Height + 5);
                    //отрисовываем портал
                    graphics.DrawImage(portal, portalPosition);
                    break;
                case 3://три раза
                    //"закрашиваем" портал черным прямоугольником
                    graphics.FillEllipse(Brushes.Black, skullPosition.X, skullPosition.Y,
                skullSize.Width, skullSize.Height + 5);
                    break;
            }
        }

        /// <summary>
        /// Инициализирует элементы интерфейса
        /// </summary>
        private void InitializeControls()
        {
            //описание игры
            aboutGame = new Label
            {
                Text = instructions[0],
                Size = new Size(400, 90),
                Location = new Point(ClientSize.Width / 11, ClientSize.Height / 6),
                ForeColor = Color.LightGray,
                Font = new Font("Bradley Hand ITC", 14.0F)
            };

            //кнопка "Далее"
            laterButton = new Button
            {
                Text = "Далее",
                Size = new Size(80, 40),
                Location = new Point(ClientSize.Width / 2 - 80, aboutGame.Location.Y + 200),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                Font = new Font("Bradley Hand ITC", 9.0F)//шрифт
            };
            //кнопка "Пропустить обучение"
            closingButton = new Button
            {
                Text = "Пропустить обучение",
                Size = laterButton.Size,
                Location = new Point(laterButton.Location.X + 100, laterButton.Location.Y),
                BackColor = Color.DarkGray,
                ForeColor = laterButton.ForeColor,
                Font = laterButton.Font
            };

            //номер текущей страницы инструкции
            pageNumber = new Label
            {
                Text = "1/5",
                Location = new Point(430, 10),
                ForeColor = Color.White,
                Font = new Font("Bradley Hand ITC", 11.0F, FontStyle.Bold)//полужирный шрифт

            };
            //добавляем на форму текстовое поле, кнопки и текстовое поле с номером страницы
            Controls.Add(aboutGame);
            Controls.Add(laterButton);
            Controls.Add(closingButton);
            Controls.Add(pageNumber);
            AddButtonsActions();//добавляем обработчики нажатия кнопок
        }

        /// <summary>
        /// Добавляет обработчики нажатия кнопок
        /// </summary>
        private void AddButtonsActions()
        {
            //при нажатии кнопки "Пропустить обучение" форма закрывается
            closingButton.Click += (sender, args) => Close();
            //если была нажата кнопка "далее"
            laterButton.Click += (sender, args) =>
            {
                laterClicksCount++;//увеличиваем кол-во нажатий по кнопке на единицу
                switch (laterClicksCount)
                {
                    case 1: //если нажали в первый раз
                        aboutGame.Text = instructions[1];//описываем ситуацию, в которой находится игрок
                        pageNumber.Text = "2/5";//номер текущей страницы инструкции
                        Invalidate();//перерисовываем форму
                        break;
                    case 2://если нажали во второй раз
                        aboutGame.Text = instructions[2]; //рассказываем о сути игры
                        pageNumber.Text = "3/5";//номер текущей страницы инструкции
                        Invalidate();//перерисовываем форму
                        break;
                    case 3://если нажали в третий раз
                        aboutGame.Text = instructions[3];//рассказываем, как управлять охотником
                        pageNumber.Text = "4/5";//номер текущей страницы инструкции
                        Invalidate();//перерисовываем форму
                        break;
                    case 4://если нажали в четвертый раз
                        aboutGame.Text = instructions[4];//желаем игроку удачи
                        laterButton.Hide();//убираем кнопку "далее"
                        closingButton.Text = "Погнали!";//меняем текст кнопки "пропустить обучение" на "погнали"
                        closingButton.Font = new Font("Bradley Hand ITC", 10.0F);//увеличиваем шрифт кнопки "погнали!"
                        closingButton.BackColor = laterButton.BackColor;//меняем цвет кнопки "погнали" на более яркий
                        pageNumber.Text = "5/5";//номер текущей страницы инструкции
                        break;
                }
            };
        }
    }
}
