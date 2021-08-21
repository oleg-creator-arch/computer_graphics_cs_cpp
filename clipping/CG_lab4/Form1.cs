using System;
using System.Drawing;
using System.Windows.Forms;

namespace CG_lab4
{
    public partial class Form1 : Form
    {
        Bitmap bmp; // Битмап для рисования
        Graphics g; // объект для рисования
                    

        Point[] points = new Point[] 
        {
            new Point(100, 100),
            new Point(250, 100),
            new Point(250, 200),
            new Point(100, 200),
        };

        //Сложение векторов (точек)
        public static Point pointAdd(Point p1, Point p2) { return new Point(p1.X + p2.X, p1.Y + p2.Y); }
        //Вычитание векторов (точек)
        public static Point pointSub(Point p1, Point p2) { return new Point(p1.X - p2.X, p1.Y - p2.Y); }
        //Скалярное произведение векторов
        public static float scalar(PointF p1, Point p2) { return (p1.X * (float)p2.X) + (p1.Y * (float)p2.Y); }
        //Умножение вектора на число
        public static Point multiply(Point p1, float number) { return new Point((int)Math.Round(p1.X * number, 0), (int)Math.Round(p1.Y * number, 0)); }

        //Отображение линии в окне
        public void drawLineInWindow(Graphics g, Point p1, Point p2, Point[] points)
        {
            //Рисуем весь отрезок светло-серым цветом (цвет невидимой части отрезка)
            g.DrawLine(new Pen(Color.FromArgb(220, 220, 220)), p1, p2);

            //Вычисляем вектор отрезка (P2 - P1)
            Point vectorP = pointSub(p2, p1);

            //Количество точек в окне
            int n = points.Length;

            //инициализируем переменные для поиска точек пересечения
            float tIn = 0, tOut = 1;

            //Проходим по всем граням многоугольника (окна)
            for (int i = 0; i < n; i++)
            {
                //Вычисляем внутреннюю нормаль грани
                Point a = pointSub(points[((i + 1 < n) ? i + 1 : 0)], points[i]);
                PointF pf = new PointF(-a.Y, a.X);
                double tmp = Math.Sqrt((pf.X * pf.X) + (pf.Y * pf.Y));
                PointF normal = new PointF((float)(pf.X / tmp), (float)(pf.Y / tmp));

                float dwn = scalar(normal, vectorP); //вычисление значения знаменателя
                float up = scalar(normal, pointSub(p1, points[i])); //вычисление значения числителя

                //если знаменатель равен нулю и числитель меньше нуля, то отрезок полностью не видим
                if (dwn == 0.0f)
                    if (up < 0) return;

                    else continue;

                //вичисляем точку пересечения
                float t = -up / dwn;

                //определяем, к чему относится точка пересечения и при необходимости запоминаем её.
                //Если знаменатель меньше нуля, то точка является точкой выхода из окна
                if (dwn < 0)
                    tOut = (t < tOut) ? t : tOut;
                //Иначе точка является точкой входа в окно
                else tIn = (t > tIn) ? t : tIn;
            }
            //Если Точка выхода не меньше, чем точка входа, то отрезок попадает в окно и его необходимо отрисовать видимую его часть.
            if (tOut >= tIn)
                g.DrawLine(new Pen(Color.Red), pointAdd(multiply(vectorP, tIn), p1), pointAdd(multiply(vectorP, tOut), p1));
        }

        //очистка экрана и отображение границ окна
        void init()
        {
            g.Clear(Color.White); //очистка
            g.DrawPolygon(new Pen(Color.Red), points); //рисование границ окна
        }

        //Конструктор
        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;
        }

        //Событие загрузки формы
        private void Form1_Load(object sender, EventArgs e)
        {
            init();
            pictureBox1.Image = bmp;
        }

        //Событие клика мышью
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        //инициализация начальных значений для демонстрационной анимации
        int x = 50, y = 50;
        int dx = 1, dy = 0;

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            //запуск/остановка демонстрационной анимации.
            timer1.Enabled = !timer1.Enabled;
        }

        //таймер демонстрационной анимации
        private void timer1_Tick(object sender, EventArgs e)
        {
            //определение приближения к граничным точкам анимации
            if (x < 50 || x >  300)
            {
                x -= dx;
                dy = dx;
                dx = 0;
            }
            if (y < 50 || y > 300)
            {
                y -= dy;
                dx = -dy;
                dy = 0;
            }

            //очистка экрана и рисование границ окна
            init();

            //рисование отрезков внутри окна
            drawLineInWindow(g, new Point(200, 150), new Point(x, y), points);
            drawLineInWindow(g, new Point(x, 1), new Point(x, bmp.Height - 1), points);
            drawLineInWindow(g, new Point(1, y), new Point(bmp.Width - 1, y), points);

            //вычисление очередной координаты
            x += dx;
            y += dy;

            //Отображение полученного кадра
            pictureBox1.Image = bmp;
        }
    }
}
