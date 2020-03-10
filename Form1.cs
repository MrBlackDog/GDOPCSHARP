using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        bool IsClicked = false;//Индикатор зажатия ЛКМ
        int deltax = 0;//Изменение координаты Х при переносе маяка
        int deltay = 0;//Изменение координаты У при переносе маяка
        Rectangle el;//Трафарет для маяка при переносе
        Bitmap bmp;//Настройка области рисования
        Graphics graph;
        Pen pen;
        List<Point> lp = new List<Point>();//Лист с координатами маяков
        int f;//Флаг для определения маяка при переносе
        int M;//Количество маяков
        double[,] SatPos = new double[2, 100];//Массив с координатами маяков вида [x1, x2, ... , xn]
                                              //                                  [y1, y2, ... , yn]
        double[,] Grad = new double[100, 2];//Градиентная матрица
        double[,] Z = new double[1000, 700];//Матрица со значениеми геометрического фактора в каждой точке помещения
        double[,] Tran = new double[2, 100];//Транспонированная матрица
        double[,] Umn = new double[100, 100];//Перемноженная матрицад для расчетов

        SolidBrush Brush;//Параметр заливки маяка

        public Form1()
        {
            InitializeComponent();
            pictureBox1.MouseClick += pictureBox1_MouseClick;//Клики по picture box
            pictureBox1.MouseMove += pictureBox1_MouseMove;//Координаты курсора
            Drawing();//Оси
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Width = 1500;//Размеры окна по умолчанию
            this.Height = 900;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Drawing()//Оси
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graph = Graphics.FromImage(bmp);
            pen = new Pen(Color.Black);
            pictureBox1.Image = bmp;

            graph.DrawLine(pen, 0, 100, 5, 100);
            graph.DrawLine(pen, 0, 200, 5, 200);
            graph.DrawLine(pen, 0, 300, 5, 300);
            graph.DrawLine(pen, 0, 400, 5, 400);
            graph.DrawLine(pen, 0, 500, 5, 500);
            graph.DrawLine(pen, 0, 1, 5, 1);
            graph.DrawLine(pen, 0, 50, 5, 50);
            graph.DrawLine(pen, 0, 150, 5, 150);
            graph.DrawLine(pen, 0, 250, 5, 250);
            graph.DrawLine(pen, 0, 350, 5, 350);
            graph.DrawLine(pen, 0, 450, 5, 450);
            graph.DrawLine(pen, 0, 550, 5, 550);
            graph.DrawLine(pen, 0, 600, 5, 600);
            graph.DrawLine(pen, 0, 650, 5, 650);
            graph.DrawLine(pen, 0, 700, 5, 700);

            graph.DrawLine(pen, 100, 0, 100, 5);
            graph.DrawLine(pen, 200, 0, 200, 5);
            graph.DrawLine(pen, 300, 0, 300, 5);
            graph.DrawLine(pen, 400, 0, 400, 5);
            graph.DrawLine(pen, 500, 0, 500, 5);
            graph.DrawLine(pen, 1, 0, 1, 5);
            graph.DrawLine(pen, 50, 0, 50, 5);
            graph.DrawLine(pen, 150, 0, 150, 5);
            graph.DrawLine(pen, 250, 0, 250, 5);
            graph.DrawLine(pen, 350, 0, 350, 5);
            graph.DrawLine(pen, 450, 0, 450, 5);
            graph.DrawLine(pen, 550, 0, 550, 5);
            graph.DrawLine(pen, 600, 0, 600, 5);
            graph.DrawLine(pen, 650, 0, 650, 5);
            graph.DrawLine(pen, 700, 0, 700, 5);
            graph.DrawLine(pen, 750, 0, 750, 5);
            graph.DrawLine(pen, 800, 0, 800, 5);
            graph.DrawLine(pen, 850, 0, 850, 5);
            graph.DrawLine(pen, 900, 0, 900, 5);
            graph.DrawLine(pen, 950, 0, 950, 5);
            graph.DrawLine(pen, 1000, 0, 1000, 5);
        }


        private void Surf()//Построение поверхности
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graph = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;

            for (int j = 0; j < 1000; j++)
            {
                for (int l = 0; l < 700; l++)
                {
                    if (Z[j, l] < 1)
                    {
                        pen = new Pen(Color.LimeGreen);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }

                    if (Z[j, l] > 1 && Z[j, l] < 1.1)
                    {
                        pen = new Pen(Color.Green);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.1 && Z[j, l] < 1.15)
                    {
                        pen = new Pen(Color.Blue);;
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.15 && Z[j, l] < 1.2)
                    {
                        pen = new Pen(Color.LightBlue);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.2 && Z[j, l] < 1.5)
                    {
                        pen = new Pen(Color.Yellow);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.5 && Z[j, l] < 1.7)
                    {
                        pen = new Pen(Color.Orange);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.7 && Z[j, l] < 2)
                    {
                        pen = new Pen(Color.Pink);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 2 && Z[j, l] < 2.5)
                    {
                        pen = new Pen(Color.DarkViolet);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 2.5 && Z[j, l] < 3)
                    {
                        pen = new Pen(Color.Purple);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 3 && Z[j, l] < 5)
                    {
                        pen = new Pen(Color.Red);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 5 && Z[j, l] < 10)
                    {
                        pen = new Pen(Color.DarkRed);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 10)
                    {
                        pen = new Pen(Color.Black);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }

                }
            }
        }

        private void Transp(int N)//Транспонированная матрица
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Tran[j, i] = Grad[i, j];
                }
            }
        }

        private void cop(int kol)//Перемножение матриц
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                     Umn[i,j] = 0;
                    for (int k = 0; k < kol; k++)
                    {
                        Umn[i, j] += Tran[i, k] * Grad[k, j];
                    }
                }
            }
        }

        private void inversionMatrix(int N)//Обратная матрица
        {
            double temp;
            double[,] B = new double[N,N];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    B[i,j] = 0.0;

                    if (i == j)
                        B[i,j] = 1.0;
                }

            for (int k = 0; k < N; k++)
            {
                temp = Umn[k,k];

                for (int j = 0; j < N; j++)
                {
                    Umn[k,j] /= temp;
                    B[k,j] /= temp;
                }

                for (int i = k + 1; i < N; i++)
                {
                    temp = Umn[i,k];

                    for (int j = 0; j < N; j++)
                    {
                        Umn[i,j] -= Umn[k,j] * temp;
                        B[i,j] -= B[k,j] * temp;
                    }
                }
            }

            for (int k = N - 1; k > 0; k--)
            {
                for (int i = k - 1; i >= 0; i--)
                {
                    temp = Umn[i,k];
                    
                    for (int j = 0; j < N; j++)
                    {
                        Umn[i,j] -= Umn[k,j] * temp;
                        B[i,j] -= B[k,j] * temp;
                    }
                }
            }

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                   Umn[i, j] = B[i, j];
                }
            }
        }

        private double trace(int N)//Расчет GDOP(Матрица Z)
        {
            double trac=0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (i==j)
                    {
                        trac += Umn[i, j];
                    }
                }
            }
            trac = Math.Sqrt(trac);
            return trac;
        }
        private void Sort(int kol1)//Решение для дальномерного метода
        {
            int i = 0;
            for (int x=0;x<1000;x++)
            {
                for(int y=0;y<700;y++)
                {
                    while (i < kol1)
                    {
                        Grad[i, 0] = (x - SatPos[0, i]) / (Math.Sqrt(Math.Pow((x - SatPos[0, i]), 2) + Math.Pow((y - SatPos[1, i]), 2)));
                        Grad[i, 1] = (y - SatPos[1, i]) / (Math.Sqrt(Math.Pow((x - SatPos[0, i]), 2) + Math.Pow((y - SatPos[1, i]), 2)));
                       
                        i += 1;
                    }
                    Transp(kol1);
                    cop(kol1);
                    inversionMatrix(2);
                    Z[x, y] = trace(2);
                    i = 0;
                }
            }
        }

        private void Sort1(int kol1)//Рещение для разностно-дальномерного метода
        {
            int i = 0;
            for (int x = 0; x < 1000; x++)
            {
                for (int y = 0; y < 700; y++)
                {
                    while (i < kol1)
                    {
                        Grad[i, 0] = ((x - SatPos[0, i]) / (Math.Sqrt(Math.Pow((x - SatPos[0, i]), 2) + Math.Pow((y - SatPos[1, i]), 2)))) - ((x - SatPos[0, kol1 - 1]) / (Math.Sqrt(Math.Pow((x - SatPos[0, kol1 - 1]), 2) + Math.Pow((y - SatPos[1, kol1 - 1]), 2))));
                        Grad[i, 1] = ((y - SatPos[1, i]) / (Math.Sqrt(Math.Pow((x - SatPos[0, i]), 2) + Math.Pow((y - SatPos[1, i]), 2)))) - ((y - SatPos[1, kol1 - 1]) / (Math.Sqrt(Math.Pow((x - SatPos[0, kol1 - 1]), 2) + Math.Pow((y - SatPos[1, kol1 - 1]), 2))));
                        i += 1;
                    }
                    Transp(kol1);
                    cop(kol1);
                    inversionMatrix(2);
                    Z[x, y] = trace(2);
                    i = 0;
                }
            }
        }

        private void labal()//Нумерация маяков
        {
            for (int b = 0; b < M; b++)
            {
                labell[b] = new Label();
                labell[b].Location = new Point(Convert.ToInt32(SatPos[0, b]) - 6, Convert.ToInt32(SatPos[1, b]) - 20);
                labell[b].ForeColor = Color.Black;
                labell[b].Text = (b + 1).ToString();
                labell[b].Size = new Size(10, 12);
                labell[b].BackColor = this.label1.Parent.BackColor;
                labell[b].Parent = this.pictureBox1;
                labell[b].BackColor = Color.Transparent;
            }
        }


    Label[] labell = new Label[100];//Массив с нумерацией маяков
    int mayak = 0;//Подсчет количества маяков
    int x, y;//Координаты курсора при клике
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            string sum = textBox1.Text;
            if (sum == "")//Проверка на пустоту
            {
                textBox2.Text = "vvedite chislo";
            }
            else
            {
                M = Convert.ToInt32(sum);
                if (M == 1 || M < 0)
                {
                    textBox2.Text = "Error input";
                }
                else
                {
                   // textBox2.Text = mayak.ToString();
                    if (mayak < M)
                    {
                        Drawing();
                        x = e.Location.X;
                        y = e.Location.Y;

                        lp.Add(new Point() { X = x, Y = y });//Заполнение листа с координатами маяков
                        foreach (Point p in lp)
                        {
                            graph.DrawEllipse(pen, p.X - 8, p.Y - 8, 16, 16);//Прорисовка маяков
                        }

                        listBox1.Items.Clear();//Очистка листа перед новым заполненим 
                        int pole = 1;
                        foreach (Point l in lp)
                        {
                            listBox1.Items.Add(pole+")"+"X:" + l.X + "," + "Y:" + l.Y);//Вывод координат маяков на экран
                            pole += 1;
                        }
                    }

                    if (mayak == (M-1))
                    {
                        int kol = 0;
                        foreach (Point lol in lp)//Создание массива с координтами маяков
                        {                          
                            SatPos[0, kol] = lol.X;
                            SatPos[1, kol] = lol.Y;
                            kol += 1;
                        }

                        listBox1.Items.Clear();
                        for (int i=0;i<M;i++)//Вывод координат всех маяков на экран
                        {
                            listBox1.Items.Add((i + 1) + ")" + "X:" + SatPos[0, i] + "," + "Y:" + SatPos[1, i]);
                        }

                        labal();

                        kol = 0;
                    }

                    mayak = mayak + 1;
                }
            }

        }
    private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)//Полная очитска
        {
            string sum = textBox1.Text;

            if (sum != "")
            {
                for (int k = 0; k < M; k++)
                {
                    labell[k].Dispose();
                }
            }
            graph.Clear(Color.White);
            mayak = 0;
            lp.Clear();
            listBox1.Items.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            Drawing();

            checkBox2.Checked = false;
            checkBox1.Checked = false;

            double[,] SatPos = new double[2, 100];
            double[,] Grad = new double[100, 2];
            double[,] Z = new double[1000, 700];
            double[,] Tran = new double[2, 100];
            double[,] Umn = new double[100, 100];
            f = 0;
            deltax = 0;
            deltay = 0;
            el = new Rectangle();
            M = 0;

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)//Вывод координат курсора на экран
        {
            textBox3.Text = e.Location.X.ToString();
            textBox4.Text = e.Location.Y.ToString();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)//Построение(GO)
        {
            if (checkBox1.Checked == true && checkBox2.Checked == false)//D
            {
                Sort(M);
                Surf();
              
                for (int j = 0;j < M;j++)
                {
                    Brush = new SolidBrush(Color.Black);
                    graph.FillEllipse(Brush, Convert.ToInt32(SatPos[0,j]) - 8, Convert.ToInt32(SatPos[1,j]) - 8, 16, 16);
                }

            }

            if (checkBox2.Checked == true && checkBox1.Checked == false)//RD
            {
                if (M == 2)
                {
                    textBox2.Text = "Error input";
                }
                else
                {
                    Sort1(M);
                    Surf();

                    for (int j = 0; j < M; j++)
                    {
                        Brush = new SolidBrush(Color.Black);
                        graph.FillEllipse(Brush, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                    }

                    int xM = Convert.ToInt32(SatPos[0, M - 1]);
                    int yM = Convert.ToInt32(SatPos[1, M - 1]);
                    Brush = new SolidBrush(Color.White);
                    graph.FillEllipse(Brush, xM - 6, yM - 6, 12, 12);
                }
            }
            
            if (checkBox2.Checked == true && checkBox1.Checked == true)//Проверка на незаполнение
            {
                textBox2.Text = "Error input";
            }

            if (checkBox2.Checked == false && checkBox1.Checked == false)//Проверка на заполнения обоих полей
            {
                textBox2.Text = "Error input";
            }

        }

        private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)//Отслеживание нажатия ЛКМ
        {
            string sum = textBox1.Text;
            if (sum != "")
            {
                if (mayak > (M -1))
                {
                    x = e.Location.X;
                    y = e.Location.Y;
                    for (int j = 0; j < M; j++)
                    {
                        if (x > (SatPos[0, j] - 8) && x < (SatPos[0, j] + 8) && y < (SatPos[1, j] + 8) && y > (SatPos[1, j] - 8))//Проверка тучка в область маяка
                        {
                            textBox2.Text = "yes" + j;
                            f = j;
                            el = new Rectangle((Convert.ToInt32(SatPos[0, j]) - 8), (Convert.ToInt32(SatPos[1, j]) - 8), 16, 16);
                            IsClicked = true;
                            deltax = e.X - el.X;
                            deltay = e.Y - el.Y;
                            break;
                        }
                    }
                }
            }
        }

        private void pictureBox1_MouseMove_1(object sender, MouseEventArgs e)//Отслеживание движения мыши
        {
            textBox2.Text = IsClicked.ToString();
            string sum = textBox1.Text;
            if (sum != "")
            {
                if (mayak > (M - 1))
                {

                    if (IsClicked)
                    {
                        el.X = e.X - deltax;
                        el.Y = e.Y - deltay;
                        pictureBox1.Invalidate();
                    }
                }
            }
        }
        private void beacon()//Прорисовка новых маяков
        {
            SatPos[0, f] = el.X + 8;
            SatPos[1, f] = el.Y + 8;
            Drawing();

            for (int k = 0; k < M; k++)
            {
                labell[k].Dispose();
            }
  
            for (int j = 0; j < M; j++)
            {
                graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
            }

            listBox1.Items.Clear();
            for (int i = 0; i < M; i++)
            {
                listBox1.Items.Add((i + 1) + ")" + "X:" + SatPos[0, i] + "," + "Y:" + SatPos[1, i]);
            }

            labal();
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)//Отслеживание отпуска ЛКМ
        {
            string sum = textBox1.Text;
            if (sum != "")
            {
                if (mayak > M && IsClicked==true)
                {
                    IsClicked = false;
                    beacon();
                }
            }
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)//Отрисока маяка при перетаскивании
        {
            pen = new Pen(Color.Black);
            e.Graphics.DrawEllipse(pen, el);
        }

        private void button2_Click(object sender, EventArgs e)//Очистка GDOP поверхности
        {
            graph.Clear(Color.White);
            Drawing();

            for (int j = 0; j < M; j++)
            {
                graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
            }

            checkBox2.Checked = false;
            checkBox1.Checked = false;
            double[,] Grad = new double[100, 2];
            double[,] Z = new double[1000, 700];
            double[,] Tran = new double[2, 100];
            double[,] Umn = new double[100, 100];

        }
    }
}
