using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace CG_lab8
{
    public partial class Form1 : Form
    {
        private Bitmap bmp;
        public Form1()
        {
            InitializeComponent();
        }

        Thread thread = null;//указатель на поток обработки
        //окрываем файл
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //обновляем поток
            if (thread != null) thread.Abort();
            //присваивание изображения
            bmp = (Bitmap)Image.FromFile(openFileDialog1.FileName);
            pictureBox1.Image = bmp;

            thread = new Thread(new ParameterizedThreadStart((bmp_in) => {
                Bitmap bmp_res = null; //битмап для копирования исходного изображения
                //запрашиваем копирование битмапа и отображение сообщения о процессе обработки
                panel1.Invoke(new Action(() => { panel1.Visible = true; bmp_res = new Bitmap((Bitmap)bmp_in); }));
                //очищаем гистограммы от предыдущего анализа
                redChart.Invoke(new Action(() => { redChart.Series[0].Points.Clear(); }));
                greenChart.Invoke(new Action(() => { greenChart.Series[0].Points.Clear(); }));
                blueChart.Invoke(new Action(() => { blueChart.Series[0].Points.Clear(); }));

                //массивы частот уровней
                double[] redVals = new double[256];
                double[] greenVals = new double[256];
                double[] blueVals = new double[256];
                //обнуляем частоты
                for (int i = 0; i < 255; i++)
                {
                    redVals[i] = 0;
                    greenVals[i] = 0;
                    blueVals[i] = 0;
                }

                //переменные для поиска максимумов
                double redMax = 0, greenMax = 0, blueMax = 0;
                //переменные для начальных моментов
                double redM3 = 0, greenM3 = 0, blueM3 = 0;
                //проходим по всем пикселям изображения
                for (int i = 0; i < bmp_res.Height; i++)
                {
                    for (int j = 0; j < bmp_res.Width; j++)
                    {
                        Color curC = bmp_res.GetPixel(j, i); // берём очередной пиксель

                        //инкрементируем частоту текущего уровня в каждой из составляющих
                        if (++redVals[curC.R] > redMax) redMax = redVals[curC.R];
                        if (++greenVals[curC.G] > greenMax) greenMax = greenVals[curC.G];
                        if (++blueVals[curC.B] > blueMax) blueMax = blueVals[curC.B];

                        //суммируем текущий уровень для нахождения начального момента
                        redM3 += (double)curC.R * (double)curC.R * (double)curC.R;
                        greenM3 += (double)curC.G * (double)curC.G * (double)curC.G;
                        blueM3 += (double)curC.B * (double)curC.B * (double)curC.B;
                    }
                }

                //находим делитель
                double delim = (double)bmp_res.Height * (double)bmp_res.Width;
                //делим начальные моменты
                redM3 /= delim; greenM3 /= delim; blueM3 /= delim;
                //вычисляем уровни в гистограммах
                for (int i = 0; i < 255; i++)
                {
                    //красный
                    redVals[i] /= delim;
                    redChart.Invoke(new Action(() => { redChart.Series[0].Points.AddXY((double)i, redVals[i] * redMax); }));

                    //зелёный
                    greenVals[i] /= delim;
                    greenChart.Invoke(new Action(() => { greenChart.Series[0].Points.AddXY((double)i, greenVals[i] * greenMax); }));

                    //синий
                    blueVals[i] /= delim;
                    blueChart.Invoke(new Action(() => { blueChart.Series[0].Points.AddXY((double)i, blueVals[i] * blueMax); }));
                }

                //записываем начальные сосенты в метки под гистограммами
                mRedLabel.Invoke(new Action(() => { mRedLabel.Text = "m3 = " + redM3; }));
                mGreenLabel.Invoke(new Action(() => { mGreenLabel.Text = "m3 = " + greenM3; }));
                mBlueLabel.Invoke(new Action(() => { mBlueLabel.Text = "m3 = " + blueM3; }));

                //убираем табличку с текстом
                panel1.Invoke(new Action(() => { panel1.Visible = false; thread = null; }));
            }));

            thread.Start(bmp);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //открываем диалог открытия файла
            openFileDialog1.ShowDialog();
        }
    }
}
