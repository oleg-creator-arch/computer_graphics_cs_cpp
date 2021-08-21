using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Diagnostics;

namespace GC_lab5
{
    public partial class Form2 : Form
    {
        //string disk= "G";
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "тетрис")
            {
                Form3 tetris = new Form3();
                tetris.ShowDialog();
               
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
