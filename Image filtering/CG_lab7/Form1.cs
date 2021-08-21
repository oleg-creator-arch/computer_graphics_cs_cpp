using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace CG_lab7
{
    public partial class Form1 : Form
    {
        //матрица фильтра
        private static float[][] I = new float[][]
        {
            new float[] { 1, 2, 3 },
            new float[] { 4, 5, 6 },
            new float[] { 7, 8, 9 }
        };

        private float d = (
             Math.Abs(I[0][0] - I[1][1]) + Math.Abs(I[1][0] - I[1][1]) + Math.Abs(I[2][0] - I[1][1])
             + Math.Abs(I[0][1] - I[1][1]) + /*Math.Abs(I[1][1] - I[1][1])*/+Math.Abs(I[2][1] - I[1][1])
             + Math.Abs(I[0][2] - I[1][1]) + Math.Abs(I[1][2] - I[1][1]) + Math.Abs(I[2][2] - I[1][1])
             ) / 8;
        float[,] Ir = new float[3, 3];
        float[,] Ig = new float[3, 3];
        float[,] Ib = new float[3, 3];
        private float dr;
        private float dg;
        private float db;
        public Form1()
        {
            InitializeComponent();

        }

        //Применение фильтра
        private Bitmap filterApply(Bitmap bmp)
        {
            Bitmap tmp = new Bitmap(bmp.Width, bmp.Height); //битмап, в котором формируется обработанное изображение

            //Заполняем массив рикселей изображения (для увеличения скорости обработки)
            //Это работает птому что GetPixel работает очень медленно
            Color[][] img = new Color[bmp.Height][];
            for (int y = 0; y < bmp.Height; y++)
            {
                img[y] = new Color[bmp.Width];
                for (int x = 0; x < bmp.Width; x++)
                    img[y][x] = bmp.GetPixel(x, y);
            }

            //Проходим по всем пикселам и вычисляем пиксели результирующего изображения
            for (int sroki = 1; sroki < tmp.Height - 1; sroki++)
                for (int col = 1; col < tmp.Width - 1; col++)
                {
                    //переменные составляющих цветов
                    float resR = 0, //красный
                        resG = 0,   //зелёный
                        resB = 0;   //синий

                    if (radioButton1.Checked)
                    {
                        //проходим по матрице фильтра и вычисляем цвет текущего пиксела
                        for (int i = 0; i < 3; i++)
                            for (int j = 0; j < 3; j++)
                                //если мы не у границы, то применяем фильтр
                                if (col - j - 1 >= 0 && col - j - 1 < bmp.Width && sroki - i - 1 >= 0 && sroki - i - 1 < bmp.Height)
                                {
                                    resR += (float)img[sroki - 1][col - 1].R + d; //красная составляющая
                                    resG += (float)img[sroki - 1][col - 1].G + d; //зелёная составляющая
                                    resB += (float)img[sroki - 1][col - 1].B + d; //синяя составляющая
                                }
                    }
                    else
                    {
                        //проходим по матрице фильтра и вычисляем цвет текущего пиксела
                        for (int i = 0; i < 3; i++)
                            for (int j = 0; j < 3; j++)
                                //если мы не у границы, то применяем фильтр
                                if (col - j - 1 >= 0 && col - j - 1 < bmp.Width && sroki - i - 1 >= 0 && sroki - i - 1 < bmp.Height)
                                {
                                    resR = (float)img[sroki - 1][col - 1].R;
                                    Ir[i, j] = resR;
                                    resG = (float)img[sroki - 1][col - 1].G;
                                    Ig[i, j] = resG;
                                    resB = (float)img[sroki - 1][col - 1].B;
                                    Ib[i, j] = resB;
                                }
                        //высчитываем среднее значение и прибовляем для увелечения яркости
                        dr = (
                            Math.Abs(Ir[0, 0] - Ir[1, 1]) + Math.Abs(Ir[1, 0] - Ir[1, 1]) + Math.Abs(Ir[2, 0] - Ir[1, 1])
                            + Math.Abs(Ir[0, 1] - Ir[1, 1]) + /*Math.Abs(I[1][1] - I[1][1])*/+Math.Abs(Ir[2, 1] - Ir[1, 1])
                            + Math.Abs(Ir[0, 2] - Ir[1, 1]) + Math.Abs(Ir[1, 2] - Ir[1, 1]) + Math.Abs(Ir[2, 2] - Ir[1, 1])
                            ) / 8;
                        dg = (
                            Math.Abs(Ig[0, 0] - Ig[1, 1]) + Math.Abs(Ig[1, 0] - Ig[1, 1]) + Math.Abs(Ig[2, 0] - Ig[1, 1])
                            + Math.Abs(Ig[0, 1] - Ig[1, 1]) + /*Math.Abs(I[1][1] - I[1][1])*/+Math.Abs(Ig[2, 1] - Ig[1, 1])
                            + Math.Abs(Ig[0, 2] - Ig[1, 1]) + Math.Abs(Ig[1, 2] - Ig[1, 1]) + Math.Abs(Ig[2, 2] - Ig[1, 1])
                            ) / 8;
                        db = (
                            Math.Abs(Ib[0, 0] - Ib[1, 1]) + Math.Abs(Ib[1, 0] - Ib[1, 1]) + Math.Abs(Ib[2, 0] - Ib[1, 1])
                            + Math.Abs(Ib[0, 1] - Ib[1, 1]) + /*Math.Abs(I[1][1] - I[1][1])*/+Math.Abs(Ib[2, 1] - Ib[1, 1])
                            + Math.Abs(Ib[0, 2] - Ib[1, 1]) + Math.Abs(Ib[1, 2] - Ib[1, 1]) + Math.Abs(Ib[2, 2] - Ib[1, 1])
                            ) / 8;
                        resR += (float)img[sroki - 1][col - 1].R + dr; //красная составляющая
                        resG += (float)img[sroki - 1][col - 1].G + dg; //зелёная составляющая
                        resB += (float)img[sroki - 1][col - 1].B + db; //синяя составляющая
                    }
                    //ограничивам диапазон значений [0-255]
                    if (resR < 0) resR = 0; if (resR > 255) resR = 255; //красная составляющая
                    if (resG < 0) resG = 0; if (resG > 255) resG = 255; //зелёная составляющая
                    if (resB < 0) resB = 0; if (resB > 255) resB = 255; //синяя составляющая
                    //устанавливаем вычисленный пиксел в результирующем изображении
                    tmp.SetPixel(col, sroki, Color.FromArgb((int)Math.Round(resR), (int)Math.Round(resG), (int)Math.Round(resB)));
                }

            //возвращаем сформированное изображение
            return tmp;
        }

        Thread task = null; //указатель на поток обработки
                            //Кнопка загрузки файла
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //открываем диалог выбора файла
            openFileDialog1.ShowDialog();

            //если файл не выбран, то выходим.
            if (openFileDialog1.FileName == null || openFileDialog1.FileName == "") return;

            //если обработка идёт, останавливаем её
            if (task != null) task.Abort();

            //загружаем выбранный файл
            Bitmap bmp = (Bitmap)Image.FromFile(openFileDialog1.FileName);

            //сбрасываем выбранный файл из диалогового окна
            openFileDialog1.FileName = "";

            //исходное изображение устанавливаем в левую часть окна
            pictureBox1.Image = bmp;

            //запускаем поток обработки зображения
            task = new Thread(new ParameterizedThreadStart((bmp_in) =>
            {
                Bitmap bmp_res = null; //битмап для копирования исходного изображения
                                       //запрашиваем копирование битмапа и отображение сообщеня о процессе обработки
                panel1.Invoke(new Action(() => { panel1.Visible = true; bmp_res = new Bitmap((Bitmap)bmp_in); }));
                Bitmap tmp = filterApply(bmp_res); //применяем фильтр
                                                   //запрашиваем установку обработанного изображения в правую часть окна
                pictureBox2.Invoke(new Action(() => { pictureBox2.Image = tmp; }));
                //запрашиваем скрытие сообщения о процессе обработки
                panel1.Invoke(new Action(() => { panel1.Visible = false; }));
                //удаляем указатель на поток обработки
                task = null;
            }));
            task.Start(bmp);
        }

        //Закрытие формы
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //если идёт обработка, останавливаем её
            if (task != null && task.IsAlive) task.Abort();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
