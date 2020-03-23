using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form1 : Form
    {
        int x, y;
        double[,] SatPos = new double[2, 5];
        Bitmap bmp;//Настройка области рисования
        Graphics graph;
        Pen pen;

        public Form1()
        {
            InitializeComponent();
  
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        int f = 0;
        double k1;
        double k2;
        double b1;
        double b2;
        double xp;
        double yp;
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graph = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;
            pen = new Pen(Color.Black);
            x = e.Location.X;
            y = e.Location.Y;
            
            SatPos[0, f] = x;
            SatPos[1, f] = y;
            f += 1;
            for(int i =0;i<f;i++)
            graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0,i]), Convert.ToInt32(SatPos[1,i]), 5, 5);

            if (f==4)
            {
                double x1 = SatPos[0, 0];
                double y1 = SatPos[1, 0];
                double x2 = SatPos[0, 1];
                double y2 = SatPos[1, 1];
                double x3 = SatPos[0, 2];
                double y3 = SatPos[1, 2];
                double x4 = SatPos[0, 3];
                double y4 = SatPos[1, 3];

                if (y2 == y1 || x1 == x2)
                       k1 = 0;
                else
                    k1 = (y2 - y1) / (x2 - x1);
                
                if (y3 == y4 || x3 == x4)
                        k2 = 0;
                else
                    k2 = (y4 - y3) / (x4 - x3);
                
                  if (k1 == k2)
                     textBox1.Text= ("PAR");
                
                     b1 = (x2 * y1 - x1 * y2) / (x2 - x1);
                     b2 = (x4 * y3 - x3 * y4) / (x4 - x3);
                     xp = (b2 - b1) / (k1 - k2);
                     yp = k1 * (b2 - b1) / (k1 - k2) + b1;
                double x1k = x1;
                double x2k = x2;
                double x3k = x3;
                double x4k = x4;
                double y1k = y1;
                double y2k = y2;
                double y3k = y3;
                double y4k = y4;
                if (x2 < x1)
                {
                    x2 = x1k;
                    x1 = x2k;
                }

                if (x4 < x3)
                {
                    x3 = x4k;
                    x4 = x3k;
                }
                if (y2 < y1)
                {
                    y2 = y1k;
                    y1 = y2k;
                }

                if (y4 < y3)
                {
                    y3 = y4k;
                    y4 = y3k;
                }

                if (k1 != k2 && b1 != b2)
                {
                    if (x1 <= xp && xp <= x2 && x3 <= xp && xp <= x4 && y1 <= yp && yp <= y2 && y3 <= yp && yp <= y4)
                        textBox1.Text = ("PER");
                    else
                        textBox1.Text = ("NEPER");
                }

            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
