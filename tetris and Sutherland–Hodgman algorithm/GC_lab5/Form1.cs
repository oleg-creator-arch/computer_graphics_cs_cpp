using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GC_lab5
{
    public partial class Form1 : Form
    {
        int halfHeight = 0; // половина высоты
        int halfWidth = 0; //половина ширины

        Bitmap bmp; //битмап для рисования
        Graphics g; //объект для рисования
        Point[] points = new Point[0]; //точки окна
        Point[] figure = new Point[0]; //точки отображаемого многоугольника

        //Сложение векторов
        public static Point pointAdd(Point p1, Point p2) { return new Point(p1.X + p2.X, p1.Y + p2.Y); }
        //Вычитание векторов
        public static Point pointSub(Point p1, Point p2) { return new Point(p1.X - p2.X, p1.Y - p2.Y); }
        //Скалярное произведение векторов
        public static float scalar(PointF p1, PointF p2) { return p1.X * p2.X + p1.Y * p2.Y; }

        //очистка битмапа и рисование контура окна
        private void bmpInit() {
            g.Clear(Color.White); //очистка

            //рисование координатных осей
            g.DrawLine(new Pen(Color.FromArgb(250, 250, 250)), halfWidth, 0, halfWidth, pictureBox1.Height); //вертикаль
            g.DrawLine(new Pen(Color.FromArgb(250, 250, 250)), 0, halfHeight, pictureBox1.Width, halfHeight); //горизонталь

            //рисование контура окна
            if (points.Length > 2)
                g.DrawPolygon(new Pen(Color.Red), points);
            else if (points.Length == 2)
                g.DrawLine(new Pen(Color.Red), points[0], points[1]);
            else if (points.Length == 1)
                bmp.SetPixel(points[0].X, points[0].Y, Color.Red);
        }

        //конструктор формы
        public Form1()
        {
            InitializeComponent();

            //инициализация битмапа
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);

            //вичисление половинных величин
            halfHeight = pictureBox1.Height / 2;
            halfWidth = pictureBox1.Width / 2;
        }

        //рисование многоугольника внутри окна
        private void drawPoligonInWindow(List<Point> poligon, List<Point> window) {
            if (poligon.Count > 2)
            {
                //рисуем контур цветом невидимой части
                g.DrawPolygon(new Pen(Color.FromArgb(215, 215, 215)), poligon.ToArray());

                //если окно существует
                if (window.Count > 0)
                {
                    //вставляем в конец первую точку для упрощения вычислений
                    window.Add(window[0]);

                    //дублируем все точки отображаемого многоугольника
                    List<Point> newPoints = poligon;

                    //проходим по всем отрезкам окна
                    for (int i = 0; i < window.Count - 1; i++)
                    {
                        //Вычисляем внутреннюю нормаль грани
                        Point a = pointSub(window[i + 1], window[i]);
                        PointF pf = new PointF(-a.Y, a.X);
                        double tmp = Math.Sqrt((pf.X * pf.X) + (pf.Y * pf.Y));
                        PointF normal = new PointF((float)(pf.X / tmp), (float)(pf.Y / tmp));

                        List<Point> tmpPoints = new List<Point>(); //список подходящих точек
                        if (newPoints.Count <= 0) return; //если видимых точек уже нет, то заканчваем алгоритм
                        newPoints.Add(newPoints[0]); //дублируем первую точку в конец списка для упрощения вычислений

                        //предыдущее значение t
                        float tPrew = scalar(normal, pointSub(newPoints[0], window[i]));

                        //проходим по всем точкам отображаемого многоугольника
                        for (int j = 1; j < newPoints.Count; j++)
                        {
                            //вичисляем текущее значение t
                            float t = scalar(normal, pointSub(newPoints[j], window[i]));
                            if (t >= 0) //если точка во внутренней полуплоскости, то
                            {
                                if (tPrew < 0) //если предыдущая точка во внешней полуплоскости, то
                                {
                                    //вычислеем точку входа в окно и вставляем её в список подходящих точек
                                    Point p = pointSub(newPoints[j], newPoints[j - 1]);
                                    float tt = -scalar(normal, pointSub(newPoints[j - 1], window[i])) / scalar(normal, p);
                                    tmpPoints.Add(pointAdd(newPoints[j - 1], new Point((int)Math.Round(p.X * tt, 0), (int)Math.Round(p.Y * tt, 0))));
                                }
                                tmpPoints.Add(newPoints[j]); //вставляем текущую точку в список подходящих точек
                            }
                            else //если точка во внешней полуплоскости, то
                                if (tPrew >= 0) //если предыдущая точка во внутренней полуплоскости, то
                                {
                                    //вычисляем точку выхода из окна и вставляем её в список подходящих точек
                                    Point p = pointSub(newPoints[j], newPoints[j - 1]);
                                    float tt = -scalar(normal, pointSub(newPoints[j - 1], window[i])) / scalar(normal, p);
                                    tmpPoints.Add(pointAdd(newPoints[j - 1], new Point((int)Math.Round(p.X * tt, 0), (int)Math.Round(p.Y * tt, 0))));
                                }
                            //езменяем предыдущее значение t на текущее
                            tPrew = t;
                        }
                        //Запоминаем список подходящих точек
                        newPoints = tmpPoints;
                    }

                    //Если есть хоть одна точка внутри окна, отображаем её
                    if (newPoints.Count > 2)
                        g.DrawPolygon(new Pen(Color.Black), newPoints.ToArray());
                    else if (newPoints.Count == 2)
                        g.DrawLine(new Pen(Color.Black), newPoints[0], newPoints[1]);
                    else if (newPoints.Count == 1)
                        bmp.SetPixel(newPoints[0].X, newPoints[0].Y, Color.Black);
                    else return;
                }
            }
            else if (poligon.Count == 2)
                g.DrawLine(new Pen(Color.FromArgb(215, 215, 215)), poligon[0], poligon[1]);
            else if (poligon.Count == 1)
                bmp.SetPixel(poligon[0].X, poligon[0].Y, Color.FromArgb(215, 215, 215));
        }

        //Рисование текущего кадра
        private void render()
        {
            //преобразуем точки окна из списка координат
            points = new Point[listBox1.Items.Count];
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                String[] cord = listBox1.Items[i].ToString().Split(' ');
                int tmp_x = Int32.Parse(cord[0]) + halfWidth;
                int tmp_y = Int32.Parse(cord[1]) * -1 + halfHeight;
                points[i] = new Point(tmp_x, tmp_y);
            }

            //преобразуем точки многоугольника из списка координат
            figure = new Point[listBox2.Items.Count];
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                String[] cord = listBox2.Items[i].ToString().Split(' ');
                int tmp_x = Int32.Parse(cord[0]) + halfWidth;
                int tmp_y = Int32.Parse(cord[1]) * -1 + halfHeight;
                figure[i] = new Point(tmp_x + x, tmp_y + y);
            }

            bmpInit(); //очищаем экран и рисуем контур окна
            drawPoligonInWindow(new List<Point>(figure), new List<Point>(points)); //рисуем многоугольник внутри окна
            pictureBox1.Image = bmp; //отображаем кадр
        }

        //переменные для анимации
        int x = 0, y = 0, dx = 0, dy = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            //координаты смещения
            x = 0;
            y = 0;
            //вектор смещения
            dx = rand.Next() % 5;
            rand.Next();
            dy = rand.Next() % 5;

            //отображаем кадр
            render();

            // вкл/выкл анимацию
            timer1.Enabled = !timer1.Enabled;

            //Изменяем текст кнопки
            if (timer1.Enabled) button1.Text = "Стоп";
            else button1.Text = "Старт";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            render(); //отображаем первый кадр
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //записываем координаты нажатой точки
            textBox1.Text = (e.X - halfWidth).ToString();
            textBox4.Text = textBox1.Text;

            textBox2.Text = ((e.Y - halfHeight) * -1).ToString();
            textBox3.Text = textBox2.Text;

            render();
            //показываем выбранную точку
            bmp.SetPixel(e.X, e.Y, Color.Blue);
            pictureBox1.Image = bmp;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                String[] cord = listBox1.SelectedItem.ToString().Split(' ');
                textBox1.Text = cord[0];
                textBox2.Text = cord[1];
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                listBox1.Items[listBox1.SelectedIndex] = textBox1.Text + " " + textBox2.Text;
                render();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                listBox1.Items.Remove(listBox1.SelectedItem);
                render();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(textBox1.Text + " " + textBox2.Text);
            render();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                listBox2.Items.Remove(listBox2.SelectedItem);
                render();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                listBox2.Items[listBox2.SelectedIndex] = textBox4.Text + " " + textBox3.Text;
                render();
            }
        }

     

        private void КакПользоватьсяToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Form2 helpForm = new Form2();
            helpForm.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox2.Items.Add(textBox4.Text + " " + textBox3.Text);
            render();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                String[] cord = listBox2.SelectedItem.ToString().Split(' ');
                textBox4.Text = cord[0];
                textBox3.Text = cord[1];
            }
        }

        //анимация
        private void timer1_Tick(object sender, EventArgs e)
        {
            //проходим по точкам фигуры
            foreach (Point point in figure)
            {
                //если какая-то точка приблизилась к границе, отражаем её под тем же углом
                if (point.X + dx > bmp.Width || point.X + dx < 0)
                {
                    dx *= -1;
                    break;
                }
                if (point.Y + dy > bmp.Height || point.Y + dy < 0)
                {
                    dy *= -1;
                    break;
                }
            }

            //отображаем текущий кадр
            render();

            //смещаем координаты для следующего кадра
            x += dx;
            y += dy;
        }
    }
}
