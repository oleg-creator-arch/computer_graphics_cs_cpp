using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KGxasanovLB2
{
    public partial class Form1 : Form
    {
        //Функция растровой развертки отрезка
        void line(Bitmap oblastdraw, int x1, int y1, int x2, int y2)
        {
            //Задаём смещения (dx и dy)
            int dx = 1;
            double dy = (double)(y2 - y1) / (double)(x2 - x1);

            //Начальные значения пера
            int x = x1;
            double y = y1;

            //Цикл рисования
            while (x >= x2)
            {
                //Ставим точку в текущем положении
                oblastdraw.SetPixel(x, (int)(y), Color.Black);

                //Переходим в следующее положение по смещению
                x -= dx;
                y -= dy;
            }
        }
        //Функция растровой развертки дуги 
        void duga(Bitmap oblastdraw, int x0, int y0, int r)
        {
            int rr = r * r, x = r, y = 0;
            while(x>0)
            {
                oblastdraw.SetPixel(x0 + x, y0 + y, Color.Black);
                x--;
                y= -(int)Math.Sqrt((double)(rr - (x * x))); 
            }
        }
        Bitmap bmp1;
        Bitmap bmp2;
        public Form1()
        {
            InitializeComponent();
            //Создаём битмап и вставляем его в пикчербокс, чтобы было видно что мы рисуем
           bmp1 = new Bitmap(this.Width, this.Height);
           pictureBox1.Image = bmp1;
           bmp2 = new Bitmap(this.Width, this.Height);
           pictureBox2.Image = bmp2;

        }
       
       

        private void Form1_Load(object sender, EventArgs e)
        {
            //реализуем отрезок в 8 октанте координат 
            line(bmp1, 100,40, 10, 20);
            duga(bmp2, 50, 100, 100);
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
