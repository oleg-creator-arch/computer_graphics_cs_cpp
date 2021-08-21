using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OlegxasanovKG1
{
    public partial class Form1 : Form
    {
        //Бонус
        int i; //Переменная используемая в цикле, обозначает кол-во проходов
        double l, a; //Переменные используемые для построения каждого квадрата
        const
        int d0 = 115, x0 = 105, y0 = 90; //Константа, влияющая на расстояние от
                                         //центра квадрата до углов(Не диагональ),
                                         //подобрана экспериментальным путем*/
                                         //Координаты центра квадрата

        /*  Recx0 = 10,
          Recy0 = 15,
          Recx00 = 75,
          Recy00 = 75,
          Linx0 = 10, 
          Liny0 = 15,
          Linx00 = 85, 
          Liny00 = 90;*/
        int Recx0 = 10,
        Recy0 = 15,
        Recx00 = 75,
        Recy00 = 75,
        Linx0 = 10,
        Liny0 = 15,
        Linx00 = 85,
        Liny00 = 90;

        private void Button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "старт")
            {
                button2.Text = "стоп";
                timer1.Enabled = true;
            }
            else
            {
                button2.Text = "старт";
                timer1.Enabled = false;
            }





        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            drawfigyra();

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        //смещение по углу 
        int vecX = -(int)(Math.Cos((165 / 180D) * Math.PI) * 10);
        int vecY = (int)(Math.Sin((165 / 180D) * Math.PI) * 10);

        void perehod()
        {
            //переход квадрата и линий
            Recx0 += vecX;
            Linx0 += vecX;
            Linx00 += vecX;
            if (Recx0 + vecX + Recx00 >= panel1.Width || Recx0 + vecX <= 0)
            {
                vecX *= -1;
            }

            Recy0 += vecY;
            Liny0 += vecY;
            Liny00 += vecY;
            if (Recy0 + vecY + Recy00 >= panel1.Height || Recy0 + vecY <= 0)
            {
                vecY *= -1;
            }

        }



        void drawfigyra()
        {
            //рандомный цвет
            Random BcBet = new Random();
            Pen rychka = new Pen(Color.Red, 3);
            SolidBrush zalivka = new SolidBrush(Color.Black);
            if (BcBet.Next(3) == 1)
            {
                rychka.Color = Color.Green;

            }
            else
            if (BcBet.Next(3) == 2)
            {
                rychka.Color = Color.Red;

            }
            else
            if (BcBet.Next(3) == 2)
            {
                rychka.Color = Color.Blue;

            }

            Graphics oblast = panel1.CreateGraphics();
            oblast.DrawRectangle(rychka, Recx0, Recy0, Recx00, Recy00);
            oblast.DrawLine(rychka, Linx0, Liny0, Linx00, Liny00);
            oblast.DrawLine(rychka, Linx00, Liny0, Linx0, Liny00);
            System.Threading.Thread.Sleep(300);

            //стирание 
            //Очистка экрана
            if (radioButton1.Checked == true)
            {
                
                panel1.Invalidate();
                panel1.Update();

            }
            //Вывод спрайта изображения
            if (radioButton2.Checked == true)
            {
                //координаты скоректированы из-за толщины пера
                oblast.FillRectangle(zalivka, Recx0 - 5, Recy0 - 5, Recx00 + 10, Recy00 + 10);
            }
            //Стирание изображения цветом фона.
            if (radioButton3.Checked == true)
            {
                oblast.Clear(Color.Black);


            }
            perehod();







        }

        public Form1()
        {
            InitializeComponent();
        }






        void DrawSquare(double c, double b) //функция построения и отрисовки квадрата
        {
            int x1, x2, x3, x4, y1, y2, y3, y4; //Координаты углов каждого квадрата
            Pen myPen = new Pen(Color.Green, 1); //Выбираем перо "myPen" черного цвета green толщиной в 1 пиксель
            Graphics g = pictureBox1.CreateGraphics();//Объявляем объект "g" класса Graphics и 
                                                      //предоставляем ему возможность рисования на pictureBox1:
            x1 = (int)(x0 + c * Math.Cos(b + 1 * Math.PI / 4));//Координаты правой нижней
            y1 = (int)(y0 + c * Math.Sin(b + 1 * Math.PI / 4));//точки
            x2 = (int)(x0 + c * Math.Cos(b + 3 * Math.PI / 4)); //Координаты левой нижней 
            y2 = (int)(y0 + c * Math.Sin(b + 3 * Math.PI / 4)); //точки
            x3 = (int)(x0 + c * Math.Cos(b + 5 * Math.PI / 4)); //Координаты левой верхней
            y3 = (int)(y0 + c * Math.Sin(b + 5 * Math.PI / 4)); //точки 
            x4 = (int)(x0 + c * Math.Cos(b + 7 * Math.PI / 4));//Координаты правой верхней
            y4 = (int)(y0 + c * Math.Sin(b + 7 * Math.PI / 4)); //точки
                                                                //Метод строящий фигуру по 4 точкам
            g.DrawPolygon(myPen, new Point[] {
                new Point(x1,y1),new Point(x2,y2),
                new Point(x3,y3),new Point(x4,y4),
            });


        }
        private void Button1_Click(object sender, EventArgs e)
        {
            l = d0;
            a = 0;
            for (i = 1; i <= 30; i++)//Цикл, рисующий 30 квадратов
            {
                DrawSquare(l, a); //Вызов функции построения и отрисовки квадрата
                a = a + Math.PI / 19;//изменяет угол поворота следующих квадратов
                l = l * Math.Sin(Math.PI / 3);//изменяет размер следующих квадратов

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}
