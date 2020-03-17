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
        SolidBrush Brush;//Параметр заливки маяка
        int f;//Флаг для определения маяка при переносе
        int M;//Количество маяков
        double[,] SatPos;//Массив с координатами маяков вида [x1, x2, ... , xn]
                         //                                  [y1, y2, ... , yn]
        double[,] Grad;//Градиентная матрица
        double[,] Z;//Матрица со значениеми геометрического фактора в каждой точке помещения
        double[,] Tran;//Транспонированная матрица
        double[,] Umn;//Перемноженная матрицад для расчетов
        Label[] labell;//Массив с нумерацией маяков
        int mayak = 0;//Подсчет количества маяков
        int x, y;//Координаты курсора при клике
        double[,] clone;//Матрица клон для кдаления маяка
        public Form1()
        {
            InitializeComponent();
            pictureBox1.MouseClick += pictureBox1_MouseClick;//Клики по picture box
            pictureBox1.MouseMove += pictureBox1_MouseMove;//Координаты курсора
            Drawing();//Оси
            // Делаем обычный стиль.
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            // Убираем кнопки свернуть, развернуть, закрыть.
            this.ControlBox = false;
            // Убираем заголовок.
            this.Text = "";
            button3.Enabled = false;
            button2.Enabled = false;
            pictureBox1.Enabled = false;
            button1.Enabled = false;
            button23.Enabled = false;
            checkBox1.BackColor = Color.Transparent;//Прозрачный фон
            checkBox2.BackColor = Color.Transparent;
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            label4.BackColor = Color.Transparent;
            label5.BackColor = Color.Transparent;
            label6.BackColor = Color.Transparent;
            label7.BackColor = Color.Transparent;
            label8.BackColor = Color.Transparent;
            label9.BackColor = Color.Transparent;
            label10.BackColor = Color.Transparent;
            label11.BackColor = Color.Transparent;
            label12.BackColor = Color.Transparent;
            label14.BackColor = Color.Transparent;
            label15.BackColor = Color.Transparent;
            label16.BackColor = Color.Transparent;
            label17.BackColor = Color.Transparent;
            label18.BackColor = Color.Transparent;
            label19.BackColor = Color.Transparent;
            label20.BackColor = Color.Transparent;
            label21.BackColor = Color.Transparent;
            label22.BackColor = Color.Transparent;
            label23.BackColor = Color.Transparent;
            label24.BackColor = Color.Transparent;
            label25.BackColor = Color.Transparent;
            label26.BackColor = Color.Transparent;
            label27.BackColor = Color.Transparent;
            label28.BackColor = Color.Transparent;
            label29.BackColor = Color.Transparent;
            label30.BackColor = Color.Transparent;
            label31.BackColor = Color.Transparent;
            label32.BackColor = Color.Transparent;
            label33.BackColor = Color.Transparent;
            label34.BackColor = Color.Transparent;
            label35.BackColor = Color.Transparent;
            label36.BackColor = Color.Transparent;
            label37.BackColor = Color.Transparent;
            label38.BackColor = Color.Transparent;
            label39.BackColor = Color.Transparent;
            label40.BackColor = Color.Transparent;
            label41.BackColor = Color.Transparent;
            label42.BackColor = Color.Transparent;
            label43.BackColor = Color.Transparent;
            label44.BackColor = Color.Transparent;
            label45.BackColor = Color.Transparent;
            label46.BackColor = Color.Transparent;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
           // this.Width = 1500;//Размеры окна по умолчанию
          //  this.Height = 1000;
            //this.Height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
            //this.Width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
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
            graph.DrawLine(pen, 0, 750, 5, 750);
            graph.DrawLine(pen, 0, 800, 5, 800);
            graph.DrawLine(pen, 0, 850, 5, 850);
            graph.DrawLine(pen, 0, 900, 5, 900);
            graph.DrawLine(pen, 0, 950, 5, 950);
            graph.DrawLine(pen, 0, 1000, 5, 1000);

            graph.DrawLine(pen, 100, 1000, 100, 995);
            graph.DrawLine(pen, 200, 1000, 200, 995);
            graph.DrawLine(pen, 300, 1000, 300, 995);
            graph.DrawLine(pen, 400, 1000, 400, 995);
            graph.DrawLine(pen, 500, 1000, 500, 995);
            graph.DrawLine(pen, 1, 1000, 1, 995);
            graph.DrawLine(pen, 50, 1000, 50, 995);
            graph.DrawLine(pen, 150, 1000, 150, 995);
            graph.DrawLine(pen, 250, 1000, 250, 995);
            graph.DrawLine(pen, 350, 1000, 350, 995);
            graph.DrawLine(pen, 450, 1000, 450, 995);
            graph.DrawLine(pen, 550, 1000, 550, 995);
            graph.DrawLine(pen, 600, 1000, 600, 995);
            graph.DrawLine(pen, 650, 1000, 650, 995);
            graph.DrawLine(pen, 700, 1000, 700, 995);
            graph.DrawLine(pen, 750, 1000, 750, 995);
            graph.DrawLine(pen, 800, 1000, 800, 995);
            graph.DrawLine(pen, 850, 1000, 850, 995);
            graph.DrawLine(pen, 900, 1000, 900, 995);
            graph.DrawLine(pen, 950, 1000, 950, 995);
            graph.DrawLine(pen, 1000, 1000, 1000, 995);
        }
        private void Surf()//Построение поверхности
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graph = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;
            for (int j = 0; j < 1000; j++)
            {
                for (int l = 0; l < 1000; l++)
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
                        pen = new Pen(Color.RoyalBlue);;
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.15 && Z[j, l] < 1.2)
                    {
                        pen = new Pen(Color.Blue);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.2 && Z[j, l] < 1.35)
                    {
                        pen = new Pen(Color.Navy);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.35 && Z[j, l] < 1.5)
                    {
                        pen = new Pen(Color.BlueViolet);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.5 && Z[j, l] < 1.7)
                    {
                        pen = new Pen(Color.Yellow);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.7 && Z[j, l] < 1.9)
                    {
                        pen = new Pen(Color.Orange);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.9 && Z[j, l] < 2.1)
                    {
                        pen = new Pen(Color.Pink);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 2.1 && Z[j, l] < 2.3)
                    {
                        pen = new Pen(Color.HotPink);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 2.3 && Z[j, l] < 2.5)
                    {
                        pen = new Pen(Color.Crimson);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 2.5 && Z[j, l] < 2.7)
                    {
                        pen = new Pen(Color.Red);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 2.7 && Z[j, l] < 3)
                    {
                        pen = new Pen(Color.DarkRed);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 3 && Z[j, l] < 5)
                    {
                        pen = new Pen(Color.Brown);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 5 && Z[j, l] < 10)
                    {
                        pen = new Pen(Color.Maroon);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 10 && Z[j, l] < 15)
                    {
                        pen = new Pen(Color.Gray);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 15)
                    {
                        pen = new Pen(Color.White);
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    progressBar1.Value += 1;
                }
            }
            button2.Enabled = true;
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
            Z = new double[1000, 1000];
            int i = 0;
            for (int x=0;x<1000;x++)
            {
                for(int y=0;y<1000;y++)
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
                    progressBar1.Value += 1;
                }                
            }
        }
        private void Sort1(int kol1)//Решение для разностно-дальномерного метода
        {
            Z = new double[1000, 1000];
            int i = 0;
            for (int x = 0; x < 1000; x++)
            {
                for (int y = 0; y < 1000; y++)
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
                    progressBar1.Value += 1;
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
                labell[b].Size = new Size(30, 12);
                labell[b].BackColor = this.label1.Parent.BackColor;
                labell[b].Parent = this.pictureBox1;
                labell[b].BackColor = Color.Transparent;
            }
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                string sum = textBox1.Text;
                M = Convert.ToInt32(sum);
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
                        listBox1.Items.Add(pole + ")" + "X:" + l.X + "," + "Y:" + (1000 - l.Y));//Вывод координат маяков на экран
                        pole += 1;
                    }
                }
                if (mayak == (M - 1))
                {
                    button23.Enabled = true;
                    button1.Enabled = true;
                    button3.Enabled = true;
                    int kol = 0;
                    foreach (Point lol in lp)//Создание массива с координтами маяков
                    {
                        SatPos[0, kol] = lol.X;
                        SatPos[1, kol] = lol.Y;
                        kol += 1;
                    }
                    listBox1.Items.Clear();
                    for (int i = 0; i < M; i++)//Вывод координат всех маяков на экран
                    {
                        listBox1.Items.Add((i + 1) + ")" + "X:" + SatPos[0, i] + "," + "Y:" + (1000 - SatPos[1, i]));
                    }
                    labal();
                    kol = 0;
                }
                mayak = mayak + 1;
            }
            if (e.Button == MouseButtons.Right)
            {
                string sum = textBox1.Text;
                if (sum != "")
                {
                    if (mayak > (M - 1))
                    {
                        x = e.Location.X;
                        y = e.Location.Y;
                        for (int j = 0; j < M; j++)
                        {
                            if (x > (SatPos[0, j] - 8) && x < (SatPos[0, j] + 8) && y < (SatPos[1, j] + 8) && y > (SatPos[1, j] - 8))//Проверка тучка в область маяка
                            {
                                if (M > 2)
                                {
                                    f = j;
                                    for (int k = 0; k < M; k++)
                                    {
                                        labell[k].Dispose();
                                    }
                                    Drawing();
                                    el = new Rectangle(1, 1, 1, 1);//Убираем с карты образ маяка, создавшийся при перетаскивании
                                    graph.DrawRectangle(pen, el);
                                    M -= 1;
                                    textBox1.Text = M.ToString();
                                    clone = new double[2, M + 1];
                                    Grad = new double[M + 1, 2];
                                    Tran = new double[2, M + 1];
                                    Umn = new double[2, 2];
                                    labell = new Label[M + 1];
                                    deletebeacons(f);
                                    break;
                                }
                                else
                                    MessageBox.Show("There must be at least two beacons on the map");
                            }
                        }
                    }
                }
            }

        }
        private void deletebeacons(int numberbeacon)
        {
            Point del = new Point() { X = Convert.ToInt32(SatPos[0, numberbeacon]), Y = Convert.ToInt32(SatPos[1, numberbeacon]) };
            lp.Remove(del);
            
            for (int i = 0; i <=M; i++)
            {
                 if (i < numberbeacon)
                 {
                clone[0, i] = SatPos[0, i];
                clone[1, i] = SatPos[1, i];
                 }
                  if (i > numberbeacon)
                  {
                   clone[0, i-1] = SatPos[0, i];
                   clone[1, i-1] = SatPos[1, i];
                  }
            }
            SatPos = new double[2, M + 1];
            for(int i=0; i<M;i++)
            {
                SatPos[0, i] = clone[0, i];
                SatPos[1, i] = clone[1, i];
            }
            for (int j = 0; j < M; j++)
            {
                graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
            }
            listBox1.Items.Clear();
            for (int i = 0; i < M; i++)
            {
                listBox1.Items.Add((i + 1) + ")" + "X:" + SatPos[0, i] + "," + "Y:" + (1000 - SatPos[1, i]));
            }
            labal();
            button2.PerformClick();
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

            double[,] SatPos;
            double[,] Grad;
            double[,] Z;
            double[,] Tran;
            double[,] Umn;
            f = 0;
            deltax = 0;
            deltay = 0;
            el = new Rectangle();
            M = 0;
            textBox1.Enabled = true;
            progressBar1.Value = 0;
            button3.Enabled = false;
            button2.Enabled = false;
            button22.Enabled = true;
            button1.Enabled = false;
            button23.Enabled = false;
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)//Вывод координат курсора на экран
        {
            textBox3.Text = e.Location.X.ToString();
            textBox4.Text = (1000 - e.Location.Y).ToString();
        }
        private void button3_Click(object sender, EventArgs e)//Построение(GO)
        {
            if (checkBox1.Checked == true && checkBox2.Checked == false)//D
            {
                button2.PerformClick();
                progressBar1.Value = 0;
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
                    MessageBox.Show("Install more beacons");
                }
                else
                {
                    button2.PerformClick();
                    progressBar1.Value = 0;
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
                MessageBox.Show("Select 1 method");
            }

            if (checkBox2.Checked == false && checkBox1.Checked == false)//Проверка на заполнения обоих полей
            {
                MessageBox.Show("Select 1 method");
            }

        }
        private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)//Отслеживание нажатия ЛКМ
        {
            if (e.Button == MouseButtons.Left)
            {
                string sum = textBox1.Text;
                if (sum != "")
                {
                    if (mayak > (M - 1))
                    {
                        x = e.Location.X;
                        y = e.Location.Y;
                        for (int j = 0; j < M; j++)
                        {
                            if (x > (SatPos[0, j] - 8) && x < (SatPos[0, j] + 8) && y < (SatPos[1, j] + 8) && y > (SatPos[1, j] - 8))//Проверка тучка в область маяка
                            {
                                f = j;
                                el = new Rectangle((Convert.ToInt32(SatPos[0, j]) - 8), (Convert.ToInt32(SatPos[1, j]) - 8), 16, 16);
                                IsClicked = true;
                                labell[f].Dispose();
                                progressBar1.Value = 0;
                                deltax = e.X - el.X;
                                deltay = e.Y - el.Y;
                                break;
                            }
                        }
                    }
                }
            }
        }
        private void pictureBox1_MouseMove_1(object sender, MouseEventArgs e)//Отслеживание движения мыши
        {
            string sum = textBox1.Text;
            if (sum != "")
            {
                if (mayak > (M - 1))
                {
                    if (IsClicked)
                    {
                        el.X = e.X - deltax;
                        el.Y = e.Y - deltay;
                        if (el.X < 0) 
                        {
                            el.X = 0;
                        }
                        if (el.X > 984)
                        {
                            el.X = 984;
                        }
                        if (el.Y < 0)
                        {
                            el.Y = 0;
                        }
                        if (el.Y > 984)
                        {
                            el.Y = 984;
                        }                        
                        pictureBox1.Invalidate();
                        SatPos[0, f] = el.X + 8;
                        SatPos[1, f] = el.Y + 8;
                        Drawing();
                        for (int j = 0; j < M; j++)
                        {
                            graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                        }                       
                    }
                }
            }
        }
        private void beacon()//Прорисовка новых маяков
        {
            SatPos[0, f] = el.X + 8;
            SatPos[1, f] = el.Y + 8;
           for(int kol = 0; kol<M;kol++)
            {
                Point test = new Point() { X = Convert.ToInt32(SatPos[0, kol]), Y = Convert.ToInt32(SatPos[1, kol]) };
                lp[kol] = test;
            }
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
                listBox1.Items.Add((i + 1) + ")" + "X:" + SatPos[0, i] + "," + "Y:" + (1000 - SatPos[1, i]));
            }
            labal();
            button2.PerformClick();
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
        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox2.Checked = false;
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox1.Checked = false;
            }
        }
        private void button22_Click(object sender, EventArgs e)//set
        {
            string sum = textBox1.Text;
            if (sum == "")//Проверка на пустоту
            {
                MessageBox.Show("Input number");
            }
            else
            {
                M = Convert.ToInt32(sum);
                if (M == 1 || M <= 0)
                {
                    MessageBox.Show("Install more beacons");
                }
                else
                {
                    pictureBox1.Enabled = true;
                    textBox1.Enabled = false;
                    SatPos = new double[2, M+1];
                    Grad = new double[M+1, 2];
                    Tran = new double[2, M+1];
                    Umn = new double[2, 2];
                    labell = new Label[M+1];
                    button22.Enabled = false;
                }
            }
        }
        private void button23_Click(object sender, EventArgs e)//add1
        {
            for (int k = 0; k < M; k++)
            {
                labell[k].Dispose();
            }
            textBox1.Text = (M + 1).ToString();
            M += 1;
            mayak = M - 1;
            button23.Enabled = false;
            SatPos = new double[2, M + 1];
            Grad = new double[M + 1, 2];
            Tran = new double[2, M + 1];
            Umn = new double[2, 2];
            labell = new Label[M + 1];
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
        }
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
        }
        private void button2_Click(object sender, EventArgs e)//Очистка GDOP поверхности
        {
            Drawing();
            for (int j = 0; j < M; j++)
            {
                graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
            }
            double[,] Grad;
            double[,] Z;
            double[,] Tran;
            double[,] Umn;
            button2.Enabled = false;
            progressBar1.Value = 0;
        }
    }
}
