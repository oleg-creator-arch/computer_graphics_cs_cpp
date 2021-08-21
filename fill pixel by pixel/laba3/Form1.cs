using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace laba3
{
    public partial class Form1 : Form
    {
        Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();
        Graphics g;
        Bitmap buf;

        public Form1()
        {
            InitializeComponent();
        }

        // Алгоритм 4-связной гранично-определенной области
        private void button1_Click(object sender, EventArgs e)
        {
            myStopwatch.Restart();
            myStopwatch.Start();
            // Очистка холста
            g.Clear(Color.Black);
            // Очистка ListBox1
            listBox1.Items.Clear();

            // Рисуем область
            buf = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(buf);
            g.DrawRectangle(new Pen(Color.Red, 1), 50, 50, 100, 100);
            buf.SetPixel(100, 100, Color.Red);
            for (int i = 0; i < 30; i++) buf.SetPixel(100 - i, 100 + i, Color.Red);
            for (int i = 0; i < 30; i++) buf.SetPixel(100 + i, 100 + i, Color.Red);
            for (int i = 0; i < 30; i++) buf.SetPixel(100 - i, 100 + i, Color.Red);
            for (int i = 0; i < 10; i++) buf.SetPixel(100 - 30 + i, 100 + 30, Color.Red);
            for (int i = 0; i < 10; i++) buf.SetPixel(100 + 30 - i, 100 + 30, Color.Red);
            for (int i = 0; i < 20; i++) buf.SetPixel(100 - 20 - i, 100 + 30 + i, Color.Red);
            for (int i = 0; i < 20; i++) buf.SetPixel(100 + 20 + i, 100 + 30 + i, Color.Red);
            for (int i = 0; i < 20; i++) buf.SetPixel(100 - 40 + i, 100 + 50, Color.Red);
            for (int i = 0; i < 20; i++) buf.SetPixel(100 + 40 - i, 100 + 50, Color.Red);

            //координаты точки
            int x;
            int y;
            pix x_y;

            Color oldcolor = Color.FromArgb(255, 255, 255, 255); // исходное значение цвета черный
            listBox1.Items.Add("Цвет oldcolor - " + @oldcolor);

            Color newcolor = Color.FromArgb(255, 0, 255, 100); // новое значение цвета зеленый 
            listBox1.Items.Add("Цвет newcolor - " + @newcolor);

            Color framecolor = Color.FromArgb(255, 255, 0, 0);// цвет границы красный 
            listBox1.Items.Add("Цвет framecolor - " + @framecolor);

            Stack<pix> stack = new Stack<pix>(); // создаем стек для пикселей

            // помещаем затравочный пиксель в стек
            stack.Push(new pix(55, 55));
            listBox1.Items.Add("-------------------------------- ");

            // помещаем пиксель в стек
            while (stack.Count != 0) // пока стек не пуст
            {
                // извлекаем пиксель из стека
                x_y = stack.Pop();
                x = x_y.getX();
                y = x_y.getY();

                // рисуем пиксель
                PutPixel(g, newcolor, x, y);

                // для точек, соседних с (x,y): 
                // (x+1,y); (x,y+1); (x-1,y); (x,y-1). Проверяем условие
                // (x+1,y)
                if (buf.GetPixel(x + 1, y) != newcolor && buf.GetPixel(x + 1, y) != framecolor)
                {
                    // рисуем пиксель
                    PutPixel(g, newcolor, x + 1, y);
                    stack.Push(new pix(x + 1, y));
                }
                // (x,y+1)
                if (buf.GetPixel(x, y + 1) != newcolor && buf.GetPixel(x, y + 1) != framecolor)
                {
                    // рисуем пиксель
                    PutPixel(g, newcolor, x, y + 1);
                    stack.Push(new pix(x, y + 1));
                }
                // (x-1, y)
                if (buf.GetPixel(x - 1, y) != newcolor && buf.GetPixel(x - 1, y) != framecolor)
                {
                    // рисуем пиксель
                    PutPixel(g, newcolor, x - 1, y);
                    stack.Push(new pix(x - 1, y));
                }
                //(x, y - 1)
                if (buf.GetPixel(x, y - 1) != newcolor && buf.GetPixel(x, y - 1) != framecolor)
                {
                    // рисуем пиксель
                    PutPixel(g, newcolor, x, y - 1);
                    stack.Push(new pix(x, y - 1));
                }
                pictureBox1.Image = buf;
                pictureBox1.Refresh();
            }
            myStopwatch.Stop();
            listBox1.Items.Add("Конец работы алгоритма 4-связной");
            listBox1.Items.Add("гранично-определенной области");
            listBox1.Items.Add("время работы 4-связного алгоритма:");
            listBox1.Items.Add(myStopwatch.ElapsedMilliseconds + " мс");
        }

        // Интервальный затравочный алгоритм
        private void button2_Click(object sender, EventArgs e)
        {
            myStopwatch.Restart();
            myStopwatch.Start();
            // Очистка холста
            g.Clear(Color.Black);
            // Очистка ListBox1
            listBox1.Items.Clear();

            // Рисуем область
            buf = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(buf);
            g.DrawRectangle(new Pen(Color.Red, 1), 50, 50, 100, 100);
            buf.SetPixel(100, 100, Color.Red);
            for (int i = 0; i < 30; i++) buf.SetPixel(100 - i, 100 + i, Color.Red);
            for (int i = 0; i < 30; i++) buf.SetPixel(100 + i, 100 + i, Color.Red);
            for (int i = 0; i < 30; i++) buf.SetPixel(100 - i, 100 + i, Color.Red);
            for (int i = 0; i < 10; i++) buf.SetPixel(100 - 30 + i, 100 + 30, Color.Red);
            for (int i = 0; i < 10; i++) buf.SetPixel(100 + 30 - i, 100 + 30, Color.Red);
            for (int i = 0; i < 20; i++) buf.SetPixel(100 - 20 - i, 100 + 30 + i, Color.Red);
            for (int i = 0; i < 20; i++) buf.SetPixel(100 + 20 + i, 100 + 30 + i, Color.Red);
            for (int i = 0; i < 20; i++) buf.SetPixel(100 - 40 + i, 100 + 50, Color.Red);
            for (int i = 0; i < 20; i++) buf.SetPixel(100 + 40 - i, 100 + 50, Color.Red);
            

            //координаты точки
            int x;
            int y;
            pix x_y;
            int xl; // крайняя левая точка интервала
            int xr; // крайняя правая точка интервала
            int xt; // буфер x
            int F = 0; // флаг

            Color oldcolor = Color.FromArgb(255, 255, 255, 255); // исходное значение цвета черный
            listBox1.Items.Add("Цвет oldcolor - " + @oldcolor);

            Color newcolor = Color.FromArgb(255, 0, 255, 100); // новое значение цвета зеленый 
            listBox1.Items.Add("Цвет newcolor - " + @newcolor);

            Color framecolor = Color.FromArgb(255, 255, 0, 0);// цвет границы красный 
            listBox1.Items.Add("Цвет framecolor - " + @framecolor);

            Stack<pix> stack = new Stack<pix>(); // создаем стек для пикселей

            // помещаем затравочный пиксель в стек
            stack.Push(new pix(55, 55));
            listBox1.Items.Add("-------------------------------- ");

            // помещаем пиксель в стек
            while (stack.Count != 0) // пока стек не пуст
            {
                // извлекаем пиксель из стека,
                // закрашиваем и заполняем интервалы
                // слева и справа с запоминанием крайних точек
                x_y = stack.Pop();
                x = x_y.getX();
                y = x_y.getY();

                //запоминаем x
                xt = x;

                // рисуем пиксель
                PutPixel(g, newcolor, x, y);

                // Заполняем интервал справа от затравки
                x++;
                while (buf.GetPixel(x, y) != framecolor)
                {
                    PutPixel(g, newcolor, x, y);
                    x++;
                }

                //запоминаем крайний правый пиксел
                xr = x - 1;

                // восстанавливаем x
                x = xt;

                // заполняем интервал слева от затравки
                x--;
                while (buf.GetPixel(x, y) != framecolor)
                {
                    PutPixel(g, newcolor, x, y);
                    x--;
                }

                // запоминаем крайний левый пиксель
                xl = x + 1;

                // обработка строки y = y + 1 
                // с определением затравочной точки

                y++;
                x = xl;
                while (x < xr)
                {
                    F = 0; // флаг
                    while (buf.GetPixel(x, y) != framecolor &&
                        buf.GetPixel(x, y) != newcolor &&
                        x < xr)
                    {
                        if (F == 0)
                        {
                            F = 1;
                        }
                        x++;
                    }
                    if (F == 1)
                    {
                        if (x == xr && buf.GetPixel(x, y) != framecolor &&
                            buf.GetPixel(x, y) != newcolor)
                        {
                            stack.Push(new pix(x, y));
                        }
                        else
                        {
                            stack.Push(new pix(x - 1, y));
                        }
                        F = 0;
                    }
                    xt = x;
                    while (buf.GetPixel(x, y) == framecolor ||
                        buf.GetPixel(x, y) == newcolor && x < xr)
                    {
                        x++;
                    }
                    if (x == xt)
                    {
                        x++;
                    }
                }

                //обработка строки y = y - 1
                y = y - 2;
                x = xl;
                while (x < xr)
                {
                    F = 0; // флаг
                    while (buf.GetPixel(x, y) != framecolor &&
                        buf.GetPixel(x, y) != newcolor &&
                        x < xr)
                    {
                        if (F == 0)
                        {
                            F = 1;
                        }
                        x++;
                    }
                    if (F == 1)
                    {
                        if (x == xr && buf.GetPixel(x, y) != framecolor &&
                            buf.GetPixel(x, y) != newcolor)
                        {
                            stack.Push(new pix(x, y));
                        }
                        else
                        {
                            stack.Push(new pix(x - 1, y));
                        }
                        F = 0;
                    }
                    xt = x;
                    while (buf.GetPixel(x, y) == framecolor ||
                        buf.GetPixel(x, y) == newcolor && x < xr)
                    {
                        x++;
                    }
                    if (x == xt)
                    {
                        x++;
                    }
                }
                pictureBox1.Image = buf;
                pictureBox1.Refresh();
            }
            myStopwatch.Stop();
            listBox1.Items.Add("Конец работы интервального");
            listBox1.Items.Add("затравочного алгоритма");
            listBox1.Items.Add("время работы");
            listBox1.Items.Add("интервального затравочного алгоритма:");
            listBox1.Items.Add(myStopwatch.ElapsedMilliseconds + " мс");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            g = this.pictureBox1.CreateGraphics(); // создаем объект - холст
            g.Clear(Color.Black);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Метод, устанавливающий пиксел на форме с заданными цветом
        private static void PutPixel(Graphics g, Color col, int x, int y)
        {
            g.FillRectangle(new SolidBrush(col), x, y, 1, 1);
        }

        public class pix : Form1 // класс пиксель
        {
            // координаты пикселя
            int x;
            int y;
            public pix(int x1, int y1)
            {
                x = x1;
                y = y1;
            }
            public int getX()
            {
                return x;
            }
            public int getY()
            {
                return y;
            }
        }
    }
}
