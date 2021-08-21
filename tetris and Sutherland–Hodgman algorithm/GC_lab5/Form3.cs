using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace GC_lab5
{
    public partial class Form3 : Form
    {
        public const int width = 15, height = 25, k = 15; // Размеры поля и размер клетки в пикселях
        public int[,] shape = new int[2, 4]; // Массив для хранения падающей фигурки (для каждого блока 2 координаты [0, i] и [1, i]
        public int[,] field = new int[width, height]; // Массив для хранения поля
        public Bitmap bitfield = new Bitmap(k * (width + 1) + 1, k * (height + 3) + 1);
        public Graphics gr; // Для рисования поля на PictureBox
        SoundPlayer sp = new SoundPlayer("5laba.wav");
        public Form3()
        {
            InitializeComponent();

            sp.Play();
            gr = Graphics.FromImage(bitfield);
            // заполним поле по краям 
            for (int i = 0; i < width; i++)
                field[i, height - 1] = 1;
            for (int i = 0; i < height; i++)
            {
                field[0, i] = 1;
                field[width - 1, i] = 1;
            }
            SetShape();
        }
        public void SetShape()
        {
            //int buf = DateTime.Now.Millisecond;
            Random x = new Random(DateTime.Now.Millisecond);
            switch (x.Next(7))
            { // Рандомно выбираем 1 из 7 возможных фигурок
                case 0: shape = new int[,] { { 2, 3, 4, 5 }, { 8, 8, 8, 8 } }; break;
                case 1: shape = new int[,] { { 2, 3, 2, 3 }, { 8, 8, 9, 9 } }; break;
                case 2: shape = new int[,] { { 2, 3, 4, 4 }, { 8, 8, 8, 9 } }; break;
                case 3: shape = new int[,] { { 2, 3, 4, 4 }, { 8, 8, 8, 7 } }; break;
                case 4: shape = new int[,] { { 3, 3, 4, 4 }, { 7, 8, 8, 9 } }; break;
                case 5: shape = new int[,] { { 3, 3, 4, 4 }, { 9, 8, 8, 7 } }; break;
                case 6: shape = new int[,] { { 3, 4, 4, 4 }, { 8, 7, 8, 9 } }; break;
            }
        }
        //геймплей
        private void Form3_KeyDown(object sender, KeyEventArgs e)
        {
            var shapeT = new int[2, 4];
            Array.Copy(shape, shapeT, shape.Length);// Создадим копию фигурки, чтобы в случае, когда после переворота на поле найдется ошибка, не переворачивать её обратно, а просто восстановить копию
            int maxx = 0, maxy = 0;
            switch (e.KeyCode)
            {
                case Keys.A:
                    for (int i = 0; i < 4; i++)
                        shape[1, i]--;// Сначала сдвигаем координаты всех кусочков фигуры на 1 влево по оси OX 
                    if (FindMistake())// Если после этого нашлась ошибка
                        for (int i = 0; i < 4; i++)
                            shape[1, i]++;// Возвращаем фигурку обратно на 1 вправо
                    break;
                case Keys.D:
                    for (int i = 0; i < 4; i++)
                        shape[1, i]++;
                    if (FindMistake())
                        for (int i = 0; i < 4; i++)
                            shape[1, i]--;
                    break;
                case Keys.Escape:
                    sp.Stop();
                    this.Close();
                    break;
                case Keys.Left:
                    for (int i = 0; i < 4; i++)
                        shape[1, i]--;
                    if (FindMistake())
                        for (int i = 0; i < 4; i++)
                            shape[1, i]++;
                    break;
                case Keys.Right:
                    for (int i = 0; i < 4; i++)
                        shape[1, i]++;
                    if (FindMistake())
                        for (int i = 0; i < 4; i++)
                            shape[1, i]--;
                    break;
                case Keys.J://читы
                    for (int i = 0; i < 4; i++)
                        shape[0, i]++;
                    break;
                case Keys.Down:
                    for (int i = 0; i < 4; i++)
                        shape[0, i]++;
                    if (FindMistake())
                        for (int i = 0; i < 4; i++)
                            shape[0, i]--;
                    break;
                case Keys.S:
                    for (int i = 0; i < 4; i++)
                        shape[0, i]++;
                    if (FindMistake())
                        for (int i = 0; i < 4; i++)
                            shape[0, i]--;
                    break;
                case Keys.Space:

                    for (int i = 0; i < 4; i++)
                    {
                        if (shape[0, i] > maxy)
                            maxy = shape[0, i];
                        if (shape[1, i] > maxx)
                            maxx = shape[1, i];
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        int temp = shape[0, i];
                        shape[0, i] = maxy - (maxx - shape[1, i]) - 1;
                        shape[1, i] = maxx - (3 - (maxy - temp)) + 1;
                    }
                    if (FindMistake())
                        Array.Copy(shapeT, shape, shape.Length);
                    break;
                case Keys.Up:

                    for (int i = 0; i < 4; i++)
                    {
                        if (shape[0, i] > maxy)
                            maxy = shape[0, i];
                        if (shape[1, i] > maxx)
                            maxx = shape[1, i];
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        int temp = shape[0, i];
                        shape[0, i] = maxy - (maxx - shape[1, i]) - 1;
                        shape[1, i] = maxx - (3 - (maxy - temp)) + 1;
                    }
                    if (FindMistake())
                        Array.Copy(shapeT, shape, shape.Length);
                    break;
                case Keys.W:
                    for (int i = 0; i < 4; i++)
                    {
                        if (shape[0, i] > maxy)
                            maxy = shape[0, i];
                        if (shape[1, i] > maxx)
                            maxx = shape[1, i];
                    }// Найдем максимальные координаты значения фигуры по X и по Y
                    for (int i = 0; i < 4; i++)
                    {
                        int temp = shape[0, i];
                        shape[0, i] = maxy - (maxx - shape[1, i]) - 1;
                        shape[1, i] = maxx - (3 - (maxy - temp)) + 1;
                    }// Перевернем фигуру.
                    if (FindMistake())
                        Array.Copy(shapeT, shape, shape.Length);
                    break;
            }
            if (3 < 0)
            {


            }

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {

            if (field[8, 3] == 1)
            {
                sp.Stop();
                timer1.Stop();
                MessageBox.Show("вы проиграли");
                this.Close();// Если клетка поля, на которой появляются фигурки заполнены, завершить программу.
            }
            for (int i = 0; i < 4; i++)
                shape[0, i]++; // Сместить фигурку вниз
            if (FindMistake())
            {
                for (int i = 0; i < 4; i++)
                    field[shape[1, i], --shape[0, i]]++;
                SetShape();
            } // Если нашлась ошибка, перенести фигурку на 1 клетку вверх, сохранить её в массив field и создать новую фигурку
            for (int i = height - 2; i > 2; i--)
            {
                var cross = (from t in Enumerable.Range(0, field.GetLength(0)).Select(j => field[j, i]).ToArray() where t == 1 select t).Count(); // Количество заполненных полей в ряду
                if (cross == width)
                {
                    for (int k = i; k > 1; k--)
                        for (int l = 1; l < width - 1; l++)
                            field[l, k] = field[l, k - 1];
                    timer1.Interval--;
                    if (timer1.Interval == 60)
                    {
                        MessageBox.Show("вы прошли игру твоему компу пизда это твой приз звони 89604613797");
                    }
                }
            } // Проверка на заполненность рядом, если нашлись ряды, в которых все клетки заполнены, сместить все ряды, которые находятся выше убранной линии, на 1 вниз

            FillField(); // Перерисовать поле
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            sp.Play();
        }

        public void FillField()
        {
            gr.Clear(Color.Black); //Очистим поле
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    if (field[i, j] == 1)
                    { // Если клетка поля существует
                        gr.FillRectangle(Brushes.White, i * k, j * k, k, k); // Рисуем в этом месте квадратик
                        gr.DrawRectangle(Pens.Black, i * k, j * k, k, k);
                    }
            for (int i = 0; i < 4; i++)
            { // Рисуем падающую фигуру
                gr.FillRectangle(Brushes.LawnGreen, shape[1, i] * k, shape[0, i] * k, k, k);
                gr.DrawRectangle(Pens.Black, shape[1, i] * k, shape[0, i] * k, k, k);
            }
            pictureBox1.Image = bitfield;
        }
        public bool FindMistake()
        {
            for (int i = 0; i < 4; i++)
                if (shape[1, i] >= width || shape[0, i] >= height ||
                    shape[1, i] <= 0 || shape[0, i] <= 0 ||
                    field[shape[1, i], shape[0, i]] == 1)
                    return true;
            return false;
        }




    }
}
