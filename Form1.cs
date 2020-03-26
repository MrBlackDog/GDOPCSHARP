using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Bitmap image;//План помещения
        bool IsClicked = false;//Индикатор зажатия ЛКМ для маяка
        bool IsClicked2 = false;//Индикатор зажатия ЛКМ для угла
        int deltax = 0;//Изменение координаты Х при переносе маяка
        int deltay = 0;//Изменение координаты У при переносе маяка
        Rectangle el;//Трафарет для маяка при переносе
        Rectangle ek;//Трафарет для угла при переносе
        Bitmap bmp;//Настройка области рисования
        Graphics graph;
        Pen pen;//Перо
        List<Point> lp = new List<Point>();//Лист с координатами маяков
        List<Point> Bp = new List<Point>();//Лист с координатами комнаты
        List<Point> Per = new List<Point>();//Лист с массивом видимых координат
        List<Point> lishn = new List<Point>();//Лист маяков видимых мастеру
        SolidBrush Brush;//Параметр заливки маяка
        int f;//Флаг для определения маяка при переносе
        int g;//Флаг для определения угла комнаты при переносе
        int M;//Количество маяков
        double[,] SatPos;//Массив с координатами маяков вида [x1, x2, ... , xn]
                         //                                  [y1, y2, ... , yn]
        double[,] SatClone;
        double[,] Grad;//Градиентная матрица
        double[,] Z;//Матрица со значениеми геометрического фактора в каждой точке помещения
        double[,] Tran;//Транспонированная матрица вида [x1, y1]
                       //[x2, y2]
                       //[......]
                       //[xn, yn]
        double[,] Umn;//Перемноженная матрицад для расчетов
        Label[] labell;//Массив с нумерацией маяков
        Label[] labelbox;//Массив с нумерацией углов
        int mayak = 0;//Подсчет количества маяков
        int x, y;//Координаты курсора при клике
        double[,] clone;//Матрица клон для кдаления маяка
        double[,] BoxPos;//Массив с координатами комнаты вида [x1, x2, ... , xn]
                         //                                   [y1, y2, ... , yn]
        double[,] BoxClone;
        int flag = 0;//Подсчет количества углов комнаты
        int N;//Количество углов комнаты
        int ind = 0;//Индикатор добавления угла
        int v = 0;//Индикатор занятого места
        bool s = false;//Индикатор пересечения
        int press = 0;//Индикатор стерания комнаты
        int kolich;//Количество маяков с учетом видимости
        int qunt;//Количество маяков видимых при RD
        Form2 form = new Form2();//Progress Bar
        bool photo = false;//Индикатор загрузки плана
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
            button22.Enabled = false;
            button23.Enabled = false;
            textBox1.Enabled = false;
            button25.Enabled = false;
            button26.Enabled = false;
            button27.Enabled = false;
            button28.Enabled = false;
            button29.Enabled = false;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;

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
            label13.BackColor = Color.Transparent;
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
             this.Height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
             this.Width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
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
            button5.Visible = true;
            button6.Visible = true;
            button7.Visible = true;
            button8.Visible = true;
            for (int j = 0; j < 1000; j++)
            {
                for (int l = 0; l < 1000; l++)
                {
                    if (Z[j, l] < 1 && Z[j,l]>0)
                    {
                        pen = new Pen(Color.FromArgb(0, 0, 255));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1 && Z[j, l] < 1.1)
                    {
                        pen = new Pen(Color.FromArgb(40, 40, 220));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.1 && Z[j, l] < 1.15)
                    {
                        pen = new Pen(Color.FromArgb(80, 80, 180));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.15 && Z[j, l] < 1.2)
                    {
                        pen = new Pen(Color.FromArgb(40, 140, 40));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.2 && Z[j, l] < 1.35)
                    {
                        pen = new Pen(Color.FromArgb(50, 200, 50));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.35 && Z[j, l] < 1.5)
                    {
                        pen = new Pen(Color.FromArgb(0, 255, 0));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.5 && Z[j, l] < 1.7)
                    {
                        pen = new Pen(Color.FromArgb(130, 255, 0));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.7 && Z[j, l] < 1.9)
                    {
                        pen = new Pen(Color.FromArgb(180, 255, 50));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.9 && Z[j, l] < 2.1)
                    {
                        pen = new Pen(Color.FromArgb(255, 255, 0));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 2.1 && Z[j, l] < 2.3)
                    {
                        pen = new Pen(Color.FromArgb(255, 220, 0));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 2.3 && Z[j, l] < 2.5)
                    {
                        pen = new Pen(Color.FromArgb(255, 180, 0));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 2.5 && Z[j, l] < 2.7)
                    {
                        pen = new Pen(Color.FromArgb(255, 150, 0));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 2.7 && Z[j, l] < 3)
                    {
                        pen = new Pen(Color.FromArgb(255, 130, 0));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 3 && Z[j, l] < 5)
                    {
                        pen = new Pen(Color.FromArgb(255, 120, 0));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 5 && Z[j, l] < 10)
                    {
                        pen = new Pen(Color.FromArgb(255, 80, 0));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 10 && Z[j, l] < 15)
                    {
                        pen = new Pen(Color.FromArgb(255, 40, 0));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j,l] == 0)
                    {
                        pen = new Pen(Color.FromArgb(255, 255, 255));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 15)
                    {
                        pen = new Pen(Color.FromArgb(255, 0, 0));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    form.progressBar1.Value += 1;
                }
            }
            button2.Enabled = true;
            if (form.progressBar1.Value == 2000000)
            {
                form.Hide();
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
        private void multi(int kol)//Перемножение матриц
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Umn[i, j] = 0;
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
            double[,] B = new double[N, N];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    B[i, j] = 0.0;

                    if (i == j)
                        B[i, j] = 1.0;
                }
            for (int k = 0; k < N; k++)
            {
                temp = Umn[k, k];
                for (int j = 0; j < N; j++)
                {
                    Umn[k, j] /= temp;
                    B[k, j] /= temp;
                }
                for (int i = k + 1; i < N; i++)
                {
                    temp = Umn[i, k];
                    for (int j = 0; j < N; j++)
                    {
                        Umn[i, j] -= Umn[k, j] * temp;
                        B[i, j] -= B[k, j] * temp;
                    }
                }
            }
            for (int k = N - 1; k > 0; k--)
            {
                for (int i = k - 1; i >= 0; i--)
                {
                    temp = Umn[i, k];
                    for (int j = 0; j < N; j++)
                    {
                        Umn[i, j] -= Umn[k, j] * temp;
                        B[i, j] -= B[k, j] * temp;
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
            double trac = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (i == j)
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
            SatClone = new double[2, M + 1];
            for (int h = 0; h < M; h++)
            {
                SatClone[0, h] = SatPos[0, h];
                SatClone[1, h] = SatPos[1, h];
            }
            BoxClone = new double[2, N + 2];
            for (int h = 0; h < N; h++)
            {
                BoxClone[0, h] = BoxPos[0, h];
                BoxClone[1, h] = BoxPos[1, h];
            }
            BoxClone[0, N] = BoxPos[0, 0];
            BoxClone[1, N] = BoxPos[1, 0];
            Z = new double[1000, 1000];
            int i = 0;
            for (int x = 0; x < 1000; x++)
            {
                for (int y = 0; y < 1000; y++)
                {
                    vidimost(x, y);
                    kol1 = kolich;
                    if (kol1 < 2)
                        Z[x, y] = 0;
                    else
                    {
                        while (i < kol1)
                        {
                            Grad[i, 0] = (x - SatPos[0, i]) / (Math.Sqrt(Math.Pow((x - SatPos[0, i]), 2) + Math.Pow((y - SatPos[1, i]), 2)));
                            Grad[i, 1] = (y - SatPos[1, i]) / (Math.Sqrt(Math.Pow((x - SatPos[0, i]), 2) + Math.Pow((y - SatPos[1, i]), 2)));
                            i += 1;
                        }
                        Transp(kol1);
                        multi(kol1);
                        inversionMatrix(2);
                        Z[x, y] = trace(2);
                    }
                    i = 0;
                    kol1 = M;
                    SatPos = new double[2, M + 1];
                    for (int h = 0; h < M; h++)
                    {
                        SatPos[0, h] = SatClone[0, h];
                        SatPos[1, h] = SatClone[1, h];
                    }
                    Per.Clear();
                    form.progressBar1.Value += 1;
                }
            }
        }
        private void Sort1(int kol1)//Решение для разностно-дальномерного метода
        {
            SatClone = new double[2, M + 1];
            for (int h = 0; h < M; h++)
            {
                SatClone[0, h] = SatPos[0, h];
                SatClone[1, h] = SatPos[1, h];
            }
            BoxClone = new double[2, N + 2];
            for (int h = 0; h < N; h++)
            {
                BoxClone[0, h] = BoxPos[0, h];
                BoxClone[1, h] = BoxPos[1, h];
            }
            BoxClone[0, N] = BoxPos[0, 0];
            BoxClone[1, N] = BoxPos[1, 0];
            Z = new double[1000, 1000];
            int i = 0;
            for (int x = 0; x < 1000; x++)
            {
                for (int y = 0; y < 1000; y++)
                {
                    lish();
                    vidimost1(x, y);
                    kol1 = kolich+1;
                      if (kol1 < 3)
                      {
                          Z[x, y] = 0;
                      }
                      else
                      {
                          while (i < kol1)
                          {
                              Grad[i, 0] = ((x - SatPos[0, i]) / (Math.Sqrt(Math.Pow((x - SatPos[0, i]), 2) + Math.Pow((y - SatPos[1, i]), 2)))) - ((x - SatPos[0, kol1 - 1]) / (Math.Sqrt(Math.Pow((x - SatPos[0, kol1 - 1]), 2) + Math.Pow((y - SatPos[1, kol1 - 1]), 2))));
                              Grad[i, 1] = ((y - SatPos[1, i]) / (Math.Sqrt(Math.Pow((x - SatPos[0, i]), 2) + Math.Pow((y - SatPos[1, i]), 2)))) - ((y - SatPos[1, kol1 - 1]) / (Math.Sqrt(Math.Pow((x - SatPos[0, kol1 - 1]), 2) + Math.Pow((y - SatPos[1, kol1 - 1]), 2))));
                              i += 1;
                          }

                          Transp(kol1);
                          multi(kol1);
                          inversionMatrix(2);
                          Z[x, y] = trace(2);
                      }                   
                    i = 0;
                    kol1 = M;
                    form.progressBar1.Value += 1;
                    SatPos = new double[2, M + 1];
                    for (int h = 0; h < M; h++)
                    {
                        SatPos[0, h] = SatClone[0, h];
                        SatPos[1, h] = SatClone[1, h];
                    }
                    lishn.Clear();
                    Per.Clear();
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
        private void labalbox()//Нумерация маяков
        {
            for (int b = 0; b < N; b++)
            {
                labelbox[b] = new Label();
                labelbox[b].Location = new Point(Convert.ToInt32(BoxPos[0, b]) - 6, Convert.ToInt32(BoxPos[1, b]) - 20);
                labelbox[b].ForeColor = Color.Black;
                labelbox[b].Text = (b + 1).ToString();
                labelbox[b].Size = new Size(30, 12);
                labelbox[b].BackColor = this.label1.Parent.BackColor;
                labelbox[b].Parent = this.pictureBox1;
                labelbox[b].BackColor = Color.Transparent;
            }
        }
        private void roompaint()//Отрисовка комнаты
        {
            for (int j = 0; j < N; j++)
            {
                graph.DrawRectangle(pen, Convert.ToInt32(BoxPos[0, j]) - 6, Convert.ToInt32(BoxPos[1, j]) - 6, 12, 12);
            }
            for (int i = 0; i < N - 1; i++)
            {
                graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, i]), Convert.ToInt32(BoxPos[1, i]), Convert.ToInt32(BoxPos[0, i + 1]), Convert.ToInt32(BoxPos[1, i + 1]));
            }
            graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, N - 1]), Convert.ToInt32(BoxPos[1, N - 1]), Convert.ToInt32(BoxPos[0, 0]), Convert.ToInt32(BoxPos[1, 0]));
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)//Клики
        {
            string sum = textBox1.Text;
            string ugl = textBox2.Text;
            //Маяки
            if (e.Button == MouseButtons.Left)
            {
                if (mayak < M)
                {
                    x = e.Location.X;
                    y = e.Location.Y;
                    if (mayak > 0)
                    {
                        foreach (Point p in lp)
                        {
                            if (x > p.X - 16 && x < p.X + 16 && y < p.Y + 16 && y > p.Y - 16)
                            {
                                v = 1;
                            }
                        }
                        foreach (Point pp in Bp)
                        {
                            if (x > pp.X - 12 && x < pp.X + 12 && y < pp.Y + 12 && y > pp.Y - 12)
                                v = 1;
                        }
                        if (v == 1)
                        {
                            v = 0;
                            MessageBox.Show("Is this place taken");
                        }
                        else
                        {
                            Drawing();
                            lp.Add(new Point() { X = x, Y = y });//Заполнение листа с координатами маяков
                            foreach (Point pp in lp)
                            {
                                graph.DrawEllipse(pen, pp.X - 8, pp.Y - 8, 16, 16);//Прорисовка маяков
                            }
                            roompaint();
                            listBox1.Items.Clear();//Очистка листа перед новым заполненим 
                            int pole1 = 1;
                            foreach (Point l in lp)
                            {
                                listBox1.Items.Add(pole1 + ")" + "X:" + l.X + "," + "Y:" + (1000 - l.Y));//Вывод координат маяков на экран
                                pole1 += 1;
                            }
                            mayak = mayak + 1;
                        }
                    }
                    if (mayak == 0)
                    {
                        Drawing();
                        lp.Add(new Point() { X = x, Y = y });//Заполнение листа с координатами маяков
                        foreach (Point p in lp)
                        {
                            graph.DrawEllipse(pen, p.X - 8, p.Y - 8, 16, 16);//Прорисовка маяков
                        }
                        roompaint();
                        listBox1.Items.Clear();//Очистка листа перед новым заполненим 
                        int pole = 1;
                        foreach (Point l in lp)
                        {
                            listBox1.Items.Add(pole + ")" + "X:" + l.X + "," + "Y:" + (1000 - l.Y));//Вывод координат маяков на экран
                            pole += 1;
                        }
                        mayak = mayak + 1;
                    }
                    if (mayak == M)
                    {
                        button23.Enabled = true;
                        button1.Enabled = true;
                        button3.Enabled = true;
                        button25.Enabled = true;
                        button27.Enabled = true;
                        button28.Enabled = true;
                        int kol = 0;
                        foreach (Point lol in lp)//Создание массива с координтами маяков
                        {
                            SatPos[0, kol] = lol.X;
                            SatPos[1, kol] = lol.Y;
                            kol += 1;
                        }
                        labal();
                        labalbox();
                        kol = 0;
                        button26.Enabled = true;
                    }
                }
            }
            //Маяки
            if (e.Button == MouseButtons.Right)
            {
                if (IsClicked == false && IsClicked2 == false)
                {
                    if (mayak >= M)
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
            //Комната
            if (e.Button == MouseButtons.Left)
            {
                if (flag < N)
                {
                    if (ind == 0)
                    {
                        x = e.Location.X;
                        y = e.Location.Y;
                        if (flag > 0)
                        {
                            foreach (Point p in Bp)
                            {
                                if (x > p.X - 12 && x < p.X + 12 && y < p.Y + 12 && y > p.Y - 12)
                                {
                                    v = 1;
                                }
                            }
                            if (v == 1)
                            {
                                v = 0;
                                MessageBox.Show("Is this place taken");
                            }
                            else
                            {
                                if (photo == false)
                                {
                                    Drawing();
                                    Bp.Add(new Point() { X = x, Y = y });//Заполнение листа с координатами комнаты
                                    foreach (Point p in Bp)
                                    {
                                        graph.DrawRectangle(pen, p.X - 6, p.Y - 6, 12, 12);//Прорисовка углов
                                    }
                                    BoxPos[0, flag] = x;
                                    BoxPos[1, flag] = y;

                                    for (int i = 0; i < flag; i++)
                                    {
                                        graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, i]), Convert.ToInt32(BoxPos[1, i]), Convert.ToInt32(BoxPos[0, i + 1]), Convert.ToInt32(BoxPos[1, i + 1]));
                                    }
                                    if (press == 1)
                                    {
                                        foreach (Point pp in lp)
                                        {
                                            graph.DrawEllipse(pen, pp.X - 8, pp.Y - 8, 16, 16);//Прорисовка маяков
                                        }
                                    }
                                    if (flag == N - 1)
                                    {
                                        graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, flag]), Convert.ToInt32(BoxPos[1, flag]), Convert.ToInt32(BoxPos[0, 0]), Convert.ToInt32(BoxPos[1, 0]));
                                        if (press == 0)
                                        {
                                            button22.Enabled = true;
                                            textBox1.Enabled = true;
                                            pictureBox1.Enabled = false;
                                            button1.Enabled = true;
                                        }
                                        if (press == 1)
                                        {
                                            button23.Enabled = true;
                                            button25.Enabled = true;
                                            button3.Enabled = true;
                                        }
                                        button27.Enabled = true;
                                        button29.Enabled = true;
                                        labalbox();
                                    }
                                    listBox2.Items.Clear();//Очистка листа перед новым заполненим 
                                    int pole = 1;
                                    foreach (Point l in Bp)
                                    {
                                        listBox2.Items.Add(pole + ")" + "X:" + l.X + "," + "Y:" + (1000 - l.Y));//Вывод координат комнаты на экран
                                        pole += 1;
                                    }
                                    flag += 1;
                                }
                                else
                                {
                                    graph = Graphics.FromImage(image);
                                    pen = new Pen(Color.Black);
                                    pictureBox1.Image = image;
                                    Bp.Add(new Point() { X = x, Y = y });//Заполнение листа с координатами комнаты
                                    foreach (Point p in Bp)
                                    {
                                        graph.DrawRectangle(pen, p.X - 6, p.Y - 6, 12, 12);//Прорисовка углов
                                    }
                                    BoxPos[0, flag] = x;
                                    BoxPos[1, flag] = y;

                                    for (int i = 0; i < flag; i++)
                                    {
                                        graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, i]), Convert.ToInt32(BoxPos[1, i]), Convert.ToInt32(BoxPos[0, i + 1]), Convert.ToInt32(BoxPos[1, i + 1]));
                                    }
                                    if (press == 1)
                                    {
                                        foreach (Point pp in lp)
                                        {
                                            graph.DrawEllipse(pen, pp.X - 8, pp.Y - 8, 16, 16);//Прорисовка маяков
                                        }
                                    }
                                    if (flag == N - 1)
                                    {
                                        Drawing();
                                        foreach (Point p in Bp)
                                        {
                                            graph.DrawRectangle(pen, p.X - 6, p.Y - 6, 12, 12);//Прорисовка углов
                                        }
                                        for (int i = 0; i < flag; i++)
                                        {
                                            graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, i]), Convert.ToInt32(BoxPos[1, i]), Convert.ToInt32(BoxPos[0, i + 1]), Convert.ToInt32(BoxPos[1, i + 1]));
                                        }
                                        graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, flag]), Convert.ToInt32(BoxPos[1, flag]), Convert.ToInt32(BoxPos[0, 0]), Convert.ToInt32(BoxPos[1, 0]));
                                        if (press == 0)
                                        {
                                            button22.Enabled = true;
                                            textBox1.Enabled = true;
                                            pictureBox1.Enabled = false;
                                            button1.Enabled = true;
                                        }
                                        if (press == 1)
                                        {
                                            button23.Enabled = true;
                                            button25.Enabled = true;
                                            button3.Enabled = true;
                                        }
                                        button27.Enabled = true;
                                        button29.Enabled = true;
                                        labalbox();
                                    }
                                    listBox2.Items.Clear();//Очистка листа перед новым заполненим 
                                    int pole = 1;
                                    foreach (Point l in Bp)
                                    {
                                        listBox2.Items.Add(pole + ")" + "X:" + l.X + "," + "Y:" + (1000 - l.Y));//Вывод координат комнаты на экран
                                        pole += 1;
                                    }
                                    flag += 1;
                                }
                            }
                        }
                        if (flag == 0)
                        {
                            if (photo == false)
                            {
                                Drawing();
                                Bp.Add(new Point() { X = x, Y = y });//Заполнение листа с координатами комнаты
                                foreach (Point p in Bp)
                                {
                                    graph.DrawRectangle(pen, p.X - 6, p.Y - 6, 12, 12);//Прорисовка углов
                                }
                                if (press == 1)
                                {
                                    foreach (Point pp in lp)
                                    {
                                        graph.DrawEllipse(pen, pp.X - 8, pp.Y - 8, 16, 16);//Прорисовка маяков
                                    }
                                }
                                BoxPos[0, flag] = x;
                                BoxPos[1, flag] = y;
                                listBox2.Items.Clear();//Очистка листа перед новым заполненим 
                                int pole = 1;
                                foreach (Point l in Bp)
                                {
                                    listBox2.Items.Add(pole + ")" + "X:" + l.X + "," + "Y:" + (1000 - l.Y));//Вывод координат комнаты на экран
                                    pole += 1;
                                }
                                flag += 1;
                            }
                            else
                            {
                                graph = Graphics.FromImage(image);
                                pen = new Pen(Color.Black);
                                pictureBox1.Image = image;
                                Bp.Add(new Point() { X = x, Y = y });//Заполнение листа с координатами комнаты
                                foreach (Point p in Bp)
                                {
                                    graph.DrawRectangle(pen, p.X - 6, p.Y - 6, 12, 12);//Прорисовка углов
                                }
                                if (press == 1)
                                {
                                    foreach (Point pp in lp)
                                    {
                                        graph.DrawEllipse(pen, pp.X - 8, pp.Y - 8, 16, 16);//Прорисовка маяков
                                    }
                                }
                                BoxPos[0, flag] = x;
                                BoxPos[1, flag] = y;
                                listBox2.Items.Clear();//Очистка листа перед новым заполненим 
                                int pole = 1;
                                foreach (Point l in Bp)
                                {
                                    listBox2.Items.Add(pole + ")" + "X:" + l.X + "," + "Y:" + (1000 - l.Y));//Вывод координат комнаты на экран
                                    pole += 1;
                                }
                                flag += 1;
                            }
                        }
                    }
                }
                if (flag < N)
                {
                    if (ind != 0)
                    {
                        x = e.Location.X;
                        y = e.Location.Y;
                        foreach (Point p in Bp)
                        {
                            if (x > p.X - 12 && x < p.X + 12 && y < p.Y + 12 && y > p.Y - 12)
                            {
                                v = 1;
                            }
                        }
                        foreach (Point p in lp)
                        {
                            if (x > p.X - 16 && x < p.X + 16 && y < p.Y + 16 && y > p.Y - 16)
                            {
                                v = 1;
                            }
                        }
                        if (v == 1)
                        {
                            v = 0;
                            MessageBox.Show("Is this place taken");
                        }
                        else
                        {
                            Drawing();
                            button23.Enabled = true;
                            button22.Enabled = false;
                            pictureBox1.Enabled = true;
                            button25.Enabled = true;
                            button3.Enabled = true;
                            button1.Enabled = true;
                            button26.Enabled = true;
                            button27.Enabled = true;
                            button29.Enabled = true;
                            for (int j = 0; j < M; j++)
                            {
                                graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                            }
                            Bp.Add(new Point() { X = x, Y = y });//Заполнение листа с координатами комнаты
                            foreach (Point p in Bp)
                            {
                                graph.DrawRectangle(pen, p.X - 6, p.Y - 6, 12, 12);//Прорисовка углов
                            }
                            listBox2.Items.Clear();//Очистка листа перед новым заполненим 
                            int pole1 = 1;
                            foreach (Point l in Bp)
                            {
                                listBox2.Items.Add(pole1 + ")" + "X:" + l.X + "," + "Y:" + (1000 - l.Y));//Вывод координат комнаты на экран
                                pole1 += 1;
                            }
                            int kol1 = 0;
                            foreach (Point lol in Bp)//Создание массива с координтами маяков
                            {
                                BoxPos[0, kol1] = lol.X;
                                BoxPos[1, kol1] = lol.Y;
                                kol1 += 1;
                            }
                            labal();
                            labalbox();
                            roompaint();
                            flag += 1;
                        }
                    }
                }

            }
            //Комната
            if (e.Button == MouseButtons.Right)
            {
                if (IsClicked == false && IsClicked2 == false)
                {
                    if (flag >= N)
                    {
                        x = e.Location.X;
                        y = e.Location.Y;
                        for (int j = 0; j < N; j++)
                        {
                            if (x > (BoxPos[0, j] - 6) && x < (BoxPos[0, j] + 6) && y < (BoxPos[1, j] + 6) && y > (BoxPos[1, j] - 6))//Проверка тучка в область маяка
                            {
                                if (N > 3)
                                {
                                    g = j;
                                    Drawing();
                                    for (int k = 0; k < N; k++)
                                    {
                                        labelbox[k].Dispose();
                                    }
                                    ek = new Rectangle(1, 1, 1, 1);//Убираем с карты образ маяка, создавшийся при перетаскивании
                                    graph.DrawRectangle(pen, ek);
                                    N -= 1;
                                    textBox2.Text = N.ToString();
                                    clone = new double[2, N + 1];
                                    labelbox = new Label[N + 1];
                                    deleteugl();
                                    break;
                                }
                                else
                                    MessageBox.Show("There must be at least three corner on the map");
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

            for (int i = 0; i <= M; i++)
            {
                if (i < numberbeacon)
                {
                    clone[0, i] = SatPos[0, i];
                    clone[1, i] = SatPos[1, i];
                }
                if (i > numberbeacon)
                {
                    clone[0, i - 1] = SatPos[0, i];
                    clone[1, i - 1] = SatPos[1, i];
                }
            }
            SatPos = new double[2, M + 1];
            for (int i = 0; i < M; i++)
            {
                SatPos[0, i] = clone[0, i];
                SatPos[1, i] = clone[1, i];
            }
            for (int j = 0; j < M; j++)
            {
                graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
            }
            roompaint();
            listBox1.Items.Clear();
            for (int i = 0; i < M; i++)
            {
                listBox1.Items.Add((i + 1) + ")" + "X:" + SatPos[0, i] + "," + "Y:" + (1000 - SatPos[1, i]));
            }
            labal();
            button2.PerformClick();
        }
        private void deleteugl()
        {
            Point del = new Point() { X = Convert.ToInt32(BoxPos[0, g]), Y = Convert.ToInt32(BoxPos[1, g]) };
            Bp.Remove(del);

            for (int i = 0; i <= N; i++)
            {
                if (i < g)
                {
                    clone[0, i] = BoxPos[0, i];
                    clone[1, i] = BoxPos[1, i];
                }
                if (i > g)
                {
                    clone[0, i - 1] = BoxPos[0, i];
                    clone[1, i - 1] = BoxPos[1, i];
                }
            }
            BoxPos = new double[2, N + 1];
            for (int i = 0; i < N; i++)
            {
                BoxPos[0, i] = clone[0, i];
                BoxPos[1, i] = clone[1, i];
            }
            for (int j = 0; j < M; j++)
            {
                graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
            }
            roompaint();
            listBox2.Items.Clear();
            for (int i = 0; i < N; i++)
            {
                listBox2.Items.Add((i + 1) + ")" + "X:" + BoxPos[0, i] + "," + "Y:" + (1000 - BoxPos[1, i]));
            }
            labalbox();
            button2.PerformClick();
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
                form.Show();
                button2.PerformClick();
                form.progressBar1.Value = 0;
                Sort(M);
                Surf();
                pen = new Pen(Color.Black);
                for (int j = 0; j < M; j++)
                {
                    Brush = new SolidBrush(Color.Black);
                    graph.FillEllipse(Brush, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                }
                for (int j = 0; j < N; j++)
                {
                    Brush = new SolidBrush(Color.Black);
                    graph.FillRectangle(Brush, Convert.ToInt32(BoxPos[0, j]) - 6, Convert.ToInt32(BoxPos[1, j]) - 6, 12, 12);
                }
                roompaint();
                pen.Width = 5;
                for (int i = 0; i < N - 1; i++)
                {
                    graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, i]), Convert.ToInt32(BoxPos[1, i]), Convert.ToInt32(BoxPos[0, i + 1]), Convert.ToInt32(BoxPos[1, i + 1]));
                }
                graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, N - 1]), Convert.ToInt32(BoxPos[1, N - 1]), Convert.ToInt32(BoxPos[0, 0]), Convert.ToInt32(BoxPos[1, 0]));
                pen.Width = 1;
            }
            if (checkBox2.Checked == true && checkBox1.Checked == false)//RD
            {
                if (M == 2)
                {
                    MessageBox.Show("Install more beacons");
                }
                else
                {
                    form.Show();
                    button2.PerformClick();
                    form.progressBar1.Value = 0;
                    Sort1(M);
                    Surf();
                    pen = new Pen(Color.Black);
                    for (int j = 0; j < M; j++)
                    {
                        Brush = new SolidBrush(Color.Black);
                        graph.FillEllipse(Brush, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                    }
                    for (int j = 0; j < N; j++)
                    {
                        Brush = new SolidBrush(Color.Black);
                        graph.FillRectangle(Brush, Convert.ToInt32(BoxPos[0, j]) - 6, Convert.ToInt32(BoxPos[1, j]) - 6, 12, 12);
                    }
                    roompaint();
                    pen.Width = 5;
                    for (int i = 0; i < N - 1; i++)
                    {
                        graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, i]), Convert.ToInt32(BoxPos[1, i]), Convert.ToInt32(BoxPos[0, i + 1]), Convert.ToInt32(BoxPos[1, i + 1]));
                    }
                    graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, N - 1]), Convert.ToInt32(BoxPos[1, N - 1]), Convert.ToInt32(BoxPos[0, 0]), Convert.ToInt32(BoxPos[1, 0]));
                    pen.Width = 1;
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
            //Маяки
            if (e.Button == MouseButtons.Left)
            {
                if (mayak >= M && flag >= N)
                {
                    x = e.Location.X;
                    y = e.Location.Y;
                    for (int j = 0; j < M; j++)
                    {
                        if (x > (SatPos[0, j] - 8) && x < (SatPos[0, j] + 8) && y < (SatPos[1, j] + 8) && y > (SatPos[1, j] - 8))//Проверка тычка в область маяка
                        {
                            f = j;
                            el = new Rectangle((Convert.ToInt32(SatPos[0, j]) - 8), (Convert.ToInt32(SatPos[1, j]) - 8), 16, 16);
                            IsClicked = true;
                            for (int k = 0; k < M; k++)
                            {
                                labell[k].Dispose();
                            }

                            for (int k = 0; k < N; k++)
                            {
                                labelbox[k].Dispose();
                            }
                            button5.Visible = false;
                            button6.Visible = false;
                            button7.Visible = false;
                            button8.Visible = false;
                            form.progressBar1.Value = 0;
                            deltax = e.X - el.X;
                            deltay = e.Y - el.Y;
                            break;
                        }
                    }
                }
            }
            //Комната
            if (e.Button == MouseButtons.Left)
            {
                if (flag >= N && mayak >= M)
                {
                    x = e.Location.X;
                    y = e.Location.Y;
                    for (int j = 0; j < N; j++)
                    {
                        if (x > (BoxPos[0, j] - 6) && x < (BoxPos[0, j] + 6) && y < (BoxPos[1, j] + 6) && y > (BoxPos[1, j] - 6))//Проверка тучка в область маяка
                        {
                            g = j;
                            ek = new Rectangle((Convert.ToInt32(BoxPos[0, j]) - 6), (Convert.ToInt32(BoxPos[1, j]) - 6), 12, 12);
                            IsClicked2 = true;
                            for (int k = 0; k < M; k++)
                            {
                                labell[k].Dispose();
                            }
                            for (int k = 0; k < N; k++)
                            {
                                labelbox[k].Dispose();
                            }
                            button5.Visible = false;
                            button6.Visible = false;
                            button7.Visible = false;
                            button8.Visible = false;
                            form.progressBar1.Value = 0;
                            deltax = e.X - ek.X;
                            deltay = e.Y - ek.Y;
                            break;
                        }
                    }
                }
            }
        }
        private void pictureBox1_MouseMove_1(object sender, MouseEventArgs e)//Отслеживание движения мыши
        {
            //Маяки
            if (mayak >= M)
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
                    for (int i = 0; i < N; i++)
                    {
                        if (el.X > (BoxPos[0, i] - 10) && el.X < (BoxPos[0, i] + 10) && el.Y < (BoxPos[1, i] + 10) && el.Y > (BoxPos[1, i] - 10))
                        {
                            if (el.Y < (BoxPos[1, i] + 10))
                            {
                                el.Y = Convert.ToInt32(BoxPos[1, i] + 4);
                            }
                            if (el.X < (BoxPos[0, i] + 10))
                            {
                                el.X = Convert.ToInt32(BoxPos[0, i] + 4);
                            }
                        }
                        if (el.X + 16 > (BoxPos[0, i] - 10) && el.X + 16 < (BoxPos[0, i] + 10) && el.Y < (BoxPos[1, i] + 10) && el.Y > (BoxPos[1, i] - 10))
                        {
                            if (el.Y < (BoxPos[1, i] + 10))
                            {
                                el.Y = Convert.ToInt32(BoxPos[1, i] + 8);
                            }
                            if (el.X + 16 > (BoxPos[0, i] - 10))
                            {
                                el.X = Convert.ToInt32(BoxPos[0, i] - 16);
                            }
                        }
                        if (el.X > (BoxPos[0, i] - 10) && el.X < (BoxPos[0, i] + 10) && el.Y + 16 < (BoxPos[1, i] + 10) && el.Y + 16 > (BoxPos[1, i] - 10))
                        {
                            if (el.Y + 16 > (BoxPos[1, i] - 10))
                            {
                                el.Y = Convert.ToInt32(BoxPos[1, i] - 16);
                            }
                            if (el.X < (BoxPos[0, i] + 10))
                            {
                                el.X = Convert.ToInt32(BoxPos[0, i] + 8);
                            }
                        }
                        if (el.X + 16 > (BoxPos[0, i] - 10) && el.X + 16 < (BoxPos[0, i] + 10) && el.Y + 16 < (BoxPos[1, i] + 10) && el.Y + 16 > (BoxPos[1, i] - 10))
                        {
                            if (el.Y + 16 > (BoxPos[1, i] - 10))
                            {
                                el.Y = Convert.ToInt32(BoxPos[1, i] - 20);
                            }
                            if (el.X + 16 > (BoxPos[0, i] - 10))
                            {
                                el.X = Convert.ToInt32(BoxPos[0, i] - 20);
                            }
                        }
                    }
                    for (int i = 0; i < f; i++)
                    {
                        if (el.X > (SatPos[0, i] - 12) && el.X < (SatPos[0, i] + 12) && el.Y < (SatPos[1, i] + 12) && el.Y > (SatPos[1, i] - 12))
                        {
                            if (el.Y < (SatPos[1, i] + 12))
                            {
                                el.Y = Convert.ToInt32(SatPos[1, i] + 4);
                            }
                            if (el.X < (SatPos[0, i] + 12))
                            {
                                el.X = Convert.ToInt32(SatPos[0, i] + 4);
                            }
                        }
                        if (el.X + 12 > (SatPos[0, i] - 12) && el.X + 12 < (SatPos[0, i] + 12) && el.Y < (SatPos[1, i] + 12) && el.Y > (SatPos[1, i] - 12))
                        {
                            if (el.Y < (SatPos[1, i] + 12))
                            {
                                el.Y = Convert.ToInt32(SatPos[1, i] + 8);
                            }
                            if (el.X + 12 > (SatPos[0, i] - 12))
                            {
                                el.X = Convert.ToInt32(SatPos[0, i] - 16);
                            }
                        }
                        if (el.X > (SatPos[0, i] - 12) && el.X < (SatPos[0, i] + 12) && el.Y + 12 < (SatPos[1, i] + 12) && el.Y + 12 > (SatPos[1, i] - 12))
                        {
                            if (el.Y + 12 > (SatPos[1, i] - 12))
                            {
                                el.Y = Convert.ToInt32(SatPos[1, i] - 16);
                            }
                            if (el.X < (SatPos[0, i] + 12))
                            {
                                el.X = Convert.ToInt32(SatPos[0, i] + 8);
                            }
                        }
                        if (el.X + 12 > (SatPos[0, i] - 12) && el.X + 12 < (SatPos[0, i] + 12) && el.Y + 12 < (SatPos[1, i] + 12) && el.Y + 12 > (SatPos[1, i] - 12))
                        {
                            if (el.Y + 12 > (SatPos[1, i] - 12))
                            {
                                el.Y = Convert.ToInt32(SatPos[1, i] - 20);
                            }
                            if (el.X + 12 > (SatPos[0, i] - 12))
                            {
                                el.X = Convert.ToInt32(SatPos[0, i] - 20);
                            }
                        }
                    }
                    for (int i = f + 1; i < M; i++)
                    {
                        if (el.X > (SatPos[0, i] - 12) && el.X < (SatPos[0, i] + 12) && el.Y < (SatPos[1, i] + 12) && el.Y > (SatPos[1, i] - 12))
                        {
                            if (el.Y < (SatPos[1, i] + 12))
                            {
                                el.Y = Convert.ToInt32(SatPos[1, i] + 4);
                            }
                            if (el.X < (SatPos[0, i] + 12))
                            {
                                el.X = Convert.ToInt32(SatPos[0, i] + 4);
                            }
                        }
                        if (el.X + 12 > (SatPos[0, i] - 12) && el.X + 12 < (SatPos[0, i] + 12) && el.Y < (SatPos[1, i] + 12) && el.Y > (SatPos[1, i] - 12))
                        {
                            if (el.Y < (SatPos[1, i] + 12))
                            {
                                el.Y = Convert.ToInt32(SatPos[1, i] + 8);
                            }
                            if (el.X + 12 > (SatPos[0, i] - 12))
                            {
                                el.X = Convert.ToInt32(SatPos[0, i] - 16);
                            }
                        }
                        if (el.X > (SatPos[0, i] - 12) && el.X < (SatPos[0, i] + 12) && el.Y + 12 < (SatPos[1, i] + 12) && el.Y + 12 > (SatPos[1, i] - 12))
                        {
                            if (el.Y + 12 > (SatPos[1, i] - 12))
                            {
                                el.Y = Convert.ToInt32(SatPos[1, i] - 16);
                            }
                            if (el.X < (SatPos[0, i] + 12))
                            {
                                el.X = Convert.ToInt32(SatPos[0, i] + 8);
                            }
                        }
                        if (el.X + 12 > (SatPos[0, i] - 12) && el.X + 12 < (SatPos[0, i] + 12) && el.Y + 12 < (SatPos[1, i] + 12) && el.Y + 12 > (SatPos[1, i] - 12))
                        {
                            if (el.Y + 12 > (SatPos[1, i] - 12))
                            {
                                el.Y = Convert.ToInt32(SatPos[1, i] - 20);
                            }
                            if (el.X + 12 > (SatPos[0, i] - 12))
                            {
                                el.X = Convert.ToInt32(SatPos[0, i] - 20);
                            }
                        }
                    }
                    pictureBox1.Invalidate();
                    SatPos[0, f] = el.X + 8;
                    SatPos[1, f] = el.Y + 8;
                    Drawing();
                    for (int j = 0; j < M; j++)
                    {
                        graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                    }
                    roompaint();
                }
            }
            //Комната
            if (flag >= N)
            {
                if (IsClicked2)
                {
                    ek.X = e.X - deltax;
                    ek.Y = e.Y - deltay;
                    if (ek.X < 0)
                    {
                        ek.X = 0;
                    }
                    if (ek.X > 988)
                    {
                        ek.X = 988;
                    }
                    if (ek.Y < 0)
                    {
                        ek.Y = 0;
                    }
                    if (ek.Y > 988)
                    {
                        ek.Y = 988;
                    }
                    for (int i = 0; i < M; i++)
                    {
                        if (ek.X > (SatPos[0, i] - 12) && ek.X < (SatPos[0, i] + 12) && ek.Y < (SatPos[1, i] + 12) && ek.Y > (SatPos[1, i] - 12))
                        {
                            if (ek.Y < (SatPos[1, i] + 12))
                            {
                                ek.Y = Convert.ToInt32(SatPos[1, i] + 6);
                            }
                            if (ek.X < (SatPos[0, i] + 12))
                            {
                                ek.X = Convert.ToInt32(SatPos[0, i] + 6);
                            }
                        }
                        if (ek.X + 12 > (SatPos[0, i] - 12) && ek.X + 12 < (SatPos[0, i] + 12) && ek.Y < (SatPos[1, i] + 12) && ek.Y > (SatPos[1, i] - 12))
                        {
                            if (ek.Y < (SatPos[1, i] + 12))
                            {
                                ek.Y = Convert.ToInt32(SatPos[1, i] + 8);
                            }
                            if (ek.X + 12 > (SatPos[0, i] - 12))
                            {
                                ek.X = Convert.ToInt32(SatPos[0, i] - 16);
                            }
                        }
                        if (ek.X > (SatPos[0, i] - 12) && ek.X < (SatPos[0, i] + 12) && ek.Y + 12 < (SatPos[1, i] + 12) && ek.Y + 12 > (SatPos[1, i] - 12))
                        {
                            if (ek.Y + 12 > (SatPos[1, i] - 12))
                            {
                                ek.Y = Convert.ToInt32(SatPos[1, i] - 16);
                            }
                            if (ek.X < (SatPos[0, i] + 12))
                            {
                                ek.X = Convert.ToInt32(SatPos[0, i] + 8);
                            }
                        }
                        if (ek.X + 12 > (SatPos[0, i] - 12) && ek.X + 12 < (SatPos[0, i] + 12) && ek.Y + 12 < (SatPos[1, i] + 12) && ek.Y + 12 > (SatPos[1, i] - 12))
                        {
                            if (ek.Y + 12 > (SatPos[1, i] - 12))
                            {
                                ek.Y = Convert.ToInt32(SatPos[1, i] - 18);
                            }
                            if (ek.X + 12 > (SatPos[0, i] - 12))
                            {
                                ek.X = Convert.ToInt32(SatPos[0, i] - 18);
                            }
                        }
                    }
                    for (int i = 0; i < g; i++)
                    {
                        if (ek.X > (BoxPos[0, i] - 8) && ek.X < (BoxPos[0, i] + 8) && ek.Y < (BoxPos[1, i] + 8) && ek.Y > (BoxPos[1, i] - 8))
                        {
                            if (ek.Y < (BoxPos[1, i] + 8))
                            {
                                ek.Y = Convert.ToInt32(BoxPos[1, i] + 6);
                            }
                            if (ek.X < (BoxPos[0, i] + 8))
                            {
                                ek.X = Convert.ToInt32(BoxPos[0, i] + 6);
                            }
                        }
                        if (ek.X + 12 > (BoxPos[0, i] - 8) && ek.X + 12 < (BoxPos[0, i] + 8) && ek.Y < (BoxPos[1, i] + 8) && ek.Y > (BoxPos[1, i] - 8))
                        {
                            if (ek.Y < (BoxPos[1, i] + 8))
                            {
                                ek.Y = Convert.ToInt32(BoxPos[1, i] + 8);
                            }
                            if (ek.X + 12 > (BoxPos[0, i] - 8))
                            {
                                ek.X = Convert.ToInt32(BoxPos[0, i] - 16);
                            }
                        }
                        if (ek.X > (BoxPos[0, i] - 8) && ek.X < (BoxPos[0, i] + 8) && ek.Y + 12 < (BoxPos[1, i] + 8) && ek.Y + 12 > (BoxPos[1, i] - 8))
                        {
                            if (ek.Y + 12 > (BoxPos[1, i] - 8))
                            {
                                ek.Y = Convert.ToInt32(BoxPos[1, i] - 16);
                            }
                            if (ek.X < (BoxPos[0, i] + 8))
                            {
                                ek.X = Convert.ToInt32(BoxPos[0, i] + 8);
                            }
                        }
                        if (ek.X + 12 > (BoxPos[0, i] - 8) && ek.X + 12 < (BoxPos[0, i] + 8) && ek.Y + 12 < (BoxPos[1, i] + 8) && ek.Y + 12 > (BoxPos[1, i] - 8))
                        {
                            if (ek.Y + 12 > (BoxPos[1, i] - 8))
                            {
                                ek.Y = Convert.ToInt32(BoxPos[1, i] - 18);
                            }
                            if (ek.X + 12 > (BoxPos[0, i] - 8))
                            {
                                ek.X = Convert.ToInt32(BoxPos[0, i] - 18);
                            }
                        }
                    }
                    for (int i = g + 1; i < N; i++)
                    {
                        if (ek.X > (BoxPos[0, i] - 8) && ek.X < (BoxPos[0, i] + 8) && ek.Y < (BoxPos[1, i] + 8) && ek.Y > (BoxPos[1, i] - 8))
                        {
                            if (ek.Y < (BoxPos[1, i] + 8))
                            {
                                ek.Y = Convert.ToInt32(BoxPos[1, i] + 6);
                            }
                            if (ek.X < (BoxPos[0, i] + 8))
                            {
                                ek.X = Convert.ToInt32(BoxPos[0, i] + 6);
                            }
                        }
                        if (ek.X + 12 > (BoxPos[0, i] - 8) && ek.X + 12 < (BoxPos[0, i] + 8) && ek.Y < (BoxPos[1, i] + 8) && ek.Y > (BoxPos[1, i] - 8))
                        {
                            if (ek.Y < (BoxPos[1, i] + 8))
                            {
                                ek.Y = Convert.ToInt32(BoxPos[1, i] + 8);
                            }
                            if (ek.X + 12 > (BoxPos[0, i] - 8))
                            {
                                ek.X = Convert.ToInt32(BoxPos[0, i] - 16);
                            }
                        }
                        if (ek.X > (BoxPos[0, i] - 8) && ek.X < (BoxPos[0, i] + 8) && ek.Y + 12 < (BoxPos[1, i] + 8) && ek.Y + 12 > (BoxPos[1, i] - 8))
                        {
                            if (ek.Y + 12 > (BoxPos[1, i] - 8))
                            {
                                ek.Y = Convert.ToInt32(BoxPos[1, i] - 16);
                            }
                            if (ek.X < (BoxPos[0, i] + 8))
                            {
                                ek.X = Convert.ToInt32(BoxPos[0, i] + 8);
                            }
                        }
                        if (ek.X + 12 > (BoxPos[0, i] - 8) && ek.X + 12 < (BoxPos[0, i] + 8) && ek.Y + 12 < (BoxPos[1, i] + 8) && ek.Y + 12 > (BoxPos[1, i] - 8))
                        {
                            if (ek.Y + 12 > (BoxPos[1, i] - 8))
                            {
                                ek.Y = Convert.ToInt32(BoxPos[1, i] - 18);
                            }
                            if (ek.X + 12 > (BoxPos[0, i] - 8))
                            {
                                ek.X = Convert.ToInt32(BoxPos[0, i] - 18);
                            }
                        }
                    }
                    pictureBox1.Invalidate();
                    BoxPos[0, g] = ek.X + 6;
                    BoxPos[1, g] = ek.Y + 6;
                    Drawing();
                    for (int j = 0; j < M; j++)
                    {
                        graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                    }
                    roompaint();
                }
            }
        }
        private void beacon()//Прорисовка новых маяков
        {
            SatPos[0, f] = el.X + 8;
            SatPos[1, f] = el.Y + 8;
            for (int kol = 0; kol < M; kol++)
            {
                Point test = new Point() { X = Convert.ToInt32(SatPos[0, kol]), Y = Convert.ToInt32(SatPos[1, kol]) };
                lp[kol] = test;
            }
            Drawing();
            for (int j = 0; j < M; j++)
            {
                graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
            }
            roompaint();
            listBox1.Items.Clear();
            for (int i = 0; i < M; i++)
            {
                listBox1.Items.Add((i + 1) + ")" + "X:" + SatPos[0, i] + "," + "Y:" + (1000 - SatPos[1, i]));
            }
            labal();
            labalbox();
            ek = new Rectangle(1, 1, 1, 1);//Убираем с карты образ маяка, создавшийся при перетаскивании
            graph.DrawRectangle(pen, ek);
            button2.PerformClick();
        }
        private void Box()
        {
            BoxPos[0, g] = ek.X + 6;
            BoxPos[1, g] = ek.Y + 6;
            for (int kol = 0; kol < N; kol++)
            {
                Point test = new Point() { X = Convert.ToInt32(BoxPos[0, kol]), Y = Convert.ToInt32(BoxPos[1, kol]) };
                Bp[kol] = test;
            }
            Drawing();
            for (int j = 0; j < M; j++)
            {
                graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
            }
            roompaint();
            listBox2.Items.Clear();
            for (int i = 0; i < N; i++)
            {
                listBox2.Items.Add((i + 1) + ")" + "X:" + BoxPos[0, i] + "," + "Y:" + (1000 - BoxPos[1, i]));
            }
            labal();
            labalbox();
            ek = new Rectangle(1, 1, 1, 1);//Убираем с карты образ маяка, создавшийся при перетаскивании
            graph.DrawRectangle(pen, ek);
            button2.PerformClick();
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)//Отслеживание отпуска ЛКМ
        {
            //Маяки
            if (mayak >= M && IsClicked == true)
            {
                IsClicked = false;
                beacon();
            }
            //Комната
            if (flag >= N && IsClicked2 == true)
            {
                IsClicked2 = false;
                Box();
            }
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)//Отрисока маяка при перетаскивании
        {
            if (IsClicked == true)
            {
                pen = new Pen(Color.Black);
                e.Graphics.DrawEllipse(pen, el);
            }
            if (IsClicked2 == true)
            {
                pen = new Pen(Color.Black);
                e.Graphics.DrawRectangle(pen, ek);
            }
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
                int n;
                if (int.TryParse(textBox1.Text, out n))
                {
                    M = Convert.ToInt32(sum);
                    if (M == 1 || M <= 0)
                    {
                        MessageBox.Show("Install more beacons");
                    }
                    else
                    {
                        for (int k = 0; k < N; k++)
                        {
                            labelbox[k].Dispose();
                        }
                        pictureBox1.Enabled = true;
                        textBox1.Enabled = false;
                        SatPos = new double[2, M + 1];
                        Grad = new double[M + 1, 2];
                        Tran = new double[2, M + 1];
                        Umn = new double[2, 2];
                        labell = new Label[M + 1];
                        button22.Enabled = false;
                        button27.Enabled = false;
                        button1.Enabled = false;
                    }
                }
                else
                    MessageBox.Show("Input correct number");
            }
        }
        private void button23_Click(object sender, EventArgs e)//add1 beacon
        {
            for (int k = 0; k < M; k++)
            {
                labell[k].Dispose();
            }
            for (int k = 0; k < N; k++)
            {
                labelbox[k].Dispose();
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
            button25.Enabled = false;
            button26.Enabled = false;
            button27.Enabled = false;
            button28.Enabled = false;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
        }
        private void button25_Click(object sender, EventArgs e)//add1 room
        {
            for (int k = 0; k < M; k++)
            {
                labell[k].Dispose();
            }
            for (int k = 0; k < N; k++)
            {
                labelbox[k].Dispose();
            }
            textBox2.Text = (N + 1).ToString();
            N += 1;
            flag = N - 1;
            button25.Enabled = false;
            BoxPos = new double[2, N + 1];
            labelbox = new Label[N + 1];
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button23.Enabled = false;
            pictureBox1.Enabled = true;
            button26.Enabled = false;
            button27.Enabled = false;
            button29.Enabled = false;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
            ind = 1;
        }
        private void button24_Click(object sender, EventArgs e)//build
        {
            string ugl = textBox2.Text;
            if (ugl != "")
            {
                int n;
                if (int.TryParse(textBox2.Text, out n))
                {
                    N = Convert.ToInt32(ugl);
                    if (N > 2)
                    {
                        BoxPos = new double[2, N + 1];
                        labelbox = new Label[N + 1];
                        pictureBox1.Enabled = true;
                        button24.Enabled = false;
                        textBox2.Enabled = false;
                    }
                    else
                        MessageBox.Show("Input more");
                }
                else
                    MessageBox.Show("Input correct number");
            }
            else
                MessageBox.Show("Input number");
        }
        private void button2_Click(object sender, EventArgs e)//Очистка GDOP поверхности
        {
            Drawing();
            for (int j = 0; j < M; j++)
            {
                graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
            }
            roompaint();
            double[,] Grad;
            double[,] Z;
            double[,] Tran;
            double[,] Umn;
            button2.Enabled = false;
            form.progressBar1.Value = 0;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
        }
        private void button26_Click(object sender, EventArgs e)//clear beacons
        {
            if (N == 0)
            {
                for (int k = 0; k < M; k++)
                {
                    labell[k].Dispose();
                }
                for (int k = 0; k < N; k++)
                {
                    labelbox[k].Dispose();
                }

                graph.Clear(Color.White);
                mayak = 0;
                flag = 0;
                lp.Clear();
                Bp.Clear();
                listBox1.Items.Clear();
                listBox2.Items.Clear();
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
                double[,] BoxPos;
                f = 0;
                g = 0;
                deltax = 0;
                deltay = 0;
                el = new Rectangle();
                ek = new Rectangle();
                M = 0;
                N = 0;
                ind = 0;
                press = 0;
                textBox1.Enabled = false;
                textBox2.Enabled = true;
                form.progressBar1.Value = 0;
                button3.Enabled = false;
                button2.Enabled = false;
                button22.Enabled = true;
                button1.Enabled = false;
                button23.Enabled = false;
                button24.Enabled = true;
                button22.Enabled = false;
                button25.Enabled = false;
                button26.Enabled = false;
                button27.Enabled = false;
                button28.Enabled = false;
                button5.Visible = false;
                button6.Visible = false;
                button7.Visible = false;
                button8.Visible = false;
            }
            else
            {
                for (int k = 0; k < M; k++)
                {
                    labell[k].Dispose();
                }
                mayak = 0;
                lp.Clear();
                listBox1.Items.Clear();
                textBox1.Text = "";
                Drawing();
                roompaint();
                double[,] SatPos;
                double[,] Grad;
                double[,] Z;
                double[,] Tran;
                double[,] Umn;
                f = 0;
                el = new Rectangle();
                M = 0;

                button23.Enabled = false;
                button22.Enabled = true;
                textBox1.Enabled = true;
                pictureBox1.Enabled = false;
                button25.Enabled = false;
                button3.Enabled = false;
                button1.Enabled = true;
                button26.Enabled = false;
                button2.Enabled = false;
                button28.Enabled = false;
                button5.Visible = false;
                button6.Visible = false;
                button7.Visible = false;
                button8.Visible = false;
            }
        }

        private void button27_Click(object sender, EventArgs e)//clear room
        {
            if (M == 0)
            {
                for (int k = 0; k < M; k++)
                {
                    labell[k].Dispose();
                }
                for (int k = 0; k < N; k++)
                {
                    labelbox[k].Dispose();
                }

                graph.Clear(Color.White);
                mayak = 0;
                flag = 0;
                lp.Clear();
                Bp.Clear();
                listBox1.Items.Clear();
                listBox2.Items.Clear();
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
                double[,] BoxPos;
                f = 0;
                g = 0;
                deltax = 0;
                deltay = 0;
                el = new Rectangle();
                ek = new Rectangle();
                M = 0;
                N = 0;
                ind = 0;
                press = 0;
                textBox1.Enabled = false;
                textBox2.Enabled = true;
                form.progressBar1.Value = 0;
                button3.Enabled = false;
                button2.Enabled = false;
                button22.Enabled = true;
                button1.Enabled = false;
                button23.Enabled = false;
                button24.Enabled = true;
                button22.Enabled = false;
                button25.Enabled = false;
                button26.Enabled = false;
                button27.Enabled = false;
                button29.Enabled = false;
                button5.Visible = false;
                button6.Visible = false;
                button7.Visible = false;
                button8.Visible = false;
            }
            else
            {
                for (int k = 0; k < N; k++)
                {
                    labelbox[k].Dispose();
                }
                flag = 0;
                Bp.Clear();
                listBox2.Items.Clear();
                textBox2.Text = "";
                double[,] BoxPos;
                g = 0;
                N = 0;
                ind = 0;
                press = 1;
                button3.Enabled = false;
                button2.Enabled = false;
                pictureBox1.Enabled = false;
                button1.Enabled = false;
                button22.Enabled = false;
                button23.Enabled = false;
                textBox1.Enabled = false;
                textBox2.Enabled = true;
                button25.Enabled = false;
                button24.Enabled = true;
                button27.Enabled = false;
                button29.Enabled = false;
                button5.Visible = false;
                button6.Visible = false;
                button7.Visible = false;
                button8.Visible = false;
                ek = new Rectangle();
                Drawing();
                foreach (Point p in lp)
                {
                    graph.DrawEllipse(pen, p.X - 8, p.Y - 8, 16, 16);//Прорисовка маяков
                }
            }
        }

        private void button28_Click(object sender, EventArgs e)//Save beacons
        {
            SaveFileDialog savebeacon = new SaveFileDialog();
            savebeacon.DefaultExt = ".txt";
            savebeacon.Filter = ".txt|*.txt";
            if (savebeacon.ShowDialog() == System.Windows.Forms.DialogResult.OK && savebeacon.FileName.Length > 0)
            {
                using (StreamWriter beacon = new StreamWriter(savebeacon.FileName, true))
                {
                    for (int i = 0; i < M; i++)
                        beacon.Write(SatPos[0, i] + " " + (1000 - SatPos[1, i]) + '\n');
                }
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveroom = new SaveFileDialog();
            saveroom.DefaultExt = ".txt";
            saveroom.Filter = "txt.|*.txt";
            if (saveroom.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveroom.FileName.Length > 0)
            {
                using (StreamWriter beacon = new StreamWriter(saveroom.FileName, true))
                {
                    for (int i = 0; i < N; i++)
                        beacon.Write(BoxPos[0, i] + " " + (1000 - BoxPos[1, i]) + '\n');
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)//Полная очитска
        {
            for (int k = 0; k < M; k++)
            {
                labell[k].Dispose();
            }
            for (int k = 0; k < N; k++)
            {
                labelbox[k].Dispose();
            }
            graph.Clear(Color.White);
            mayak = 0;
            flag = 0;
            lp.Clear();
            Bp.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Clear();
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
            double[,] BoxPos;
            f = 0;
            g = 0;
            deltax = 0;
            deltay = 0;
            el = new Rectangle();
            ek = new Rectangle();
            M = 0;
            N = 0;
            ind = 0;
            press = 0;
            textBox1.Enabled = false;
            textBox2.Enabled = true;
            form.progressBar1.Value = 0;
            button3.Enabled = false;
            button2.Enabled = false;
            button22.Enabled = true;
            button1.Enabled = false;
            button23.Enabled = false;
            button24.Enabled = true;
            button22.Enabled = false;
            button25.Enabled = false;
            button26.Enabled = false;
            button27.Enabled = false;
            button28.Enabled = false;
            button29.Enabled = false;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
        }

        private void button30_Click(object sender, EventArgs e)
        {
            photo = true;
          //  Bitmap image; //Bitmap для открываемого изображения
            OpenFileDialog open_dialog = new OpenFileDialog(); //создание диалогового окна для выбора файла
            open_dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*"; //формат загружаемого файла
            if (open_dialog.ShowDialog() == DialogResult.OK) //если в окне была нажата кнопка "ОК"
            {
                try
                {
                    image = new Bitmap(open_dialog.FileName);
                    pictureBox1.Image = image;
                    pictureBox1.Invalidate();
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно открыть выбранный файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void vidimost(int x, int y)
        {
            s = false;
            for (int i = 0; i < M; i++)
            {
                double ax1 = x;
                double ay1 = y;
                double ax2 = SatPos[0, i];
                double ay2 = SatPos[1, i];
                s = false;
                for (int j = 0; j < N; j++)
                {
                    double bx1 = BoxClone[0, j];
                    double by1 = BoxClone[1, j];
                    double bx2 = BoxClone[0, j + 1];
                    double by2 = BoxClone[1, j + 1];
                    double v1 = (bx2 - bx1) * (ay1 - by1) - (by2 - by1) * (ax1 - bx1);
                    double v2 = (bx2 - bx1) * (ay2 - by1) - (by2 - by1) * (ax2 - bx1);
                    double v3 = (ax2 - ax1) * (by1 - ay1) - (ay2 - ay1) * (bx1 - ax1);
                    double v4 = (ax2 - ax1) * (by2 - ay1) - (ay2 - ay1) * (bx2 - ax1);
                    if ((v1 * v2 < 0) && (v3 * v4 < 0))
                    {
                        s = true;
                    }
                }
                if(s==false)
                    Per.Add(new Point() { X = Convert.ToInt32(SatPos[0, i]), Y = Convert.ToInt32(SatPos[1, i]) });
            }
            kolich = 0;
            foreach (Point pp in Per)
            {
                kolich += 1;
            }
            SatPos = new double[2, kolich + 1];
            kolich = 0;
            foreach (Point pp in Per)
            {
                SatPos[0, kolich] = pp.X;
                SatPos[1, kolich] = pp.Y;
                kolich += 1;
            }
            s = false;
        }

        private void lish()
        {
            SatClone = new double[2, M + 1];
            for (int h = 0; h < M; h++)
            {
                SatClone[0, h] = SatPos[0, h];
                SatClone[1, h] = SatPos[1, h];
            }

            for (int i = 0; i < M - 1; i++)
            {

             
                s = false;
                for (int j = 0; j < N; j++)
                {
                    double ax1 = SatPos[0, i];
                    double ay1 = SatPos[1, i];
                    double ax2 = SatPos[0, M - 1];
                    double ay2 = SatPos[1, M - 1];
                    double bx1 = BoxClone[0, j];
                    double by1 = BoxClone[1, j];
                    double bx2 = BoxClone[0, j + 1];
                    double by2 = BoxClone[1, j + 1];

                    double v1 = (bx2 - bx1) * (ay1 - by1) - (by2 - by1) * (ax1 - bx1);
                    double v2 = (bx2 - bx1) * (ay2 - by1) - (by2 - by1) * (ax2 - bx1);
                    double v3 = (ax2 - ax1) * (by1 - ay1) - (ay2 - ay1) * (bx1 - ax1);
                    double v4 = (ax2 - ax1) * (by2 - ay1) - (ay2 - ay1) * (bx2 - ax1);
                    if ((v1 * v2 < 0) && (v3 * v4 < 0))
                    {
                        s = true;
                    }
                }
                if (s==false)
                {
                    lishn.Add(new Point() { X = Convert.ToInt32(SatPos[0, i]), Y = Convert.ToInt32(SatPos[1, i]) });
                }

            }
            qunt = 0;
            foreach (Point pp in lishn)
            {
                qunt += 1;
            }
            SatPos = new double[2, qunt + 1];
            qunt = 0;
            foreach (Point pp in lishn)
            {
                SatPos[0, qunt] = pp.X;
                SatPos[1, qunt] = pp.Y;
                qunt += 1;
            }
            SatPos[0, qunt] = SatClone[0, M - 1];
            SatPos[1, qunt] = SatClone[1, M - 1];
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"GDOP - программа, которая помогает определить значение геометрического фактора. 
                            Инструкция по использованию:
1.Ввести количество углов помещения, в котором вы собираетесь определить значение геометрического фактора;
2.После нажатия на кпоку build установить углы на местности(для удобства имеется координатная ось, а также слева отображаются координаты курсора на поле в данный момент;
3.Ввести количество маяков;
4.После нажатия на кнопку set утановить маяки на местности.
5.После этого вы уже можете расчитать значение геометричесукого фактора по дальномерному методу или разностно - дальномерному методу(главный маяк(мастер) будет последним поставленным на местности), для этого выберите метод и нажмите кнопку GO;
6.Также у вас есть возможность добавлять по 1 маяку на местность и 1 углу комнаты. Для этого необходимо нажать на кнопку add 1 рядом с введенным значением маяков или углов комнаты.
7.Еще одна особенность данный программы заключается в том, что у вас есть возможность перемещать маяки по местности или углы комнаты, при этом все расчеты будут проводится относительно новых параметров.
8.При нажатии правой кнопки мыши на маяк или угол команты данный маяк(угол) удалится с местности.
9.Вы не можете ставить несколько маяков или углов комнаты в 1 точку.
10.Также при переносе маяков или углов комнаты вы не можете перетащить их в другой маяк или угол.
11.Вы можете полностью удалить комнату с местности, нажав кнопку clear room;
12.Вы можете полностью удалить маяки с местности, нажав кнопку clear beacons;
13.Очистить все полностью -кнопка clear.
О значениях градиента геометрического фактора:
1.Синий цвет - хорошая видимость;
2.Зеленый цвет - средняя видимость;
3.Красный цвет - плохая видимость;
4.Белый цвет - видимости нет;
Резкий переход цвета обозначает, то что за чертой перехода цвета в силу вступает(отступает) еще 1 или несколько маяков.

GDOP is a program that helps determine the value of a geometric factor.
Instructions for use:
1. Enter the number of corners of the room in which you are going to determine the value of the geometric factor;
2. After clicking on the build button, set the angles on the ground (for convenience, there is a coordinate axis, and the coordinates of the cursor on the field at the moment are also displayed on the left;
3. Enter the number of beacons;
4. After pressing the set button, set the beacons on the ground.
5. After that, you can already calculate the value of the geometric factor by the rangefinder method or the difference-rangefinder method (the main beacon (master) will be the last set on the ground), to do this, select the method and press the GO button;
6. Also you have the opportunity to add 1 lighthouse to the terrain and 1 corner of the room. To do this, click on the add 1 button next to the entered value of the beacons or corners of the room.
7. Another feature of this program is that you have the ability to move the beacons in the area or the corners of the room, while all the calculations will be carried out with respect to the new parameters.
8. When you right-click on the lighthouse or the corner of the team, this lighthouse (corner) will be removed from the terrain.
9. You cannot set several beacons or corners of a room at 1 point.
10. Also, when moving lighthouses or room corners, you cannot drag them to another lighthouse or corner.
11. You can completely remove the room from the area by clicking the clear room button;
12. You can completely remove the beacons from the area by clicking the clear beacons button;
13. Clear everything completely - clear button.
About the values ​​of the gradient of the geometric factor:
1. Blue color - good visibility;
2. Green color - average visibility;
3. Red color - poor visibility;
4. White color - no visibility;
A sharp color transition means that beyond the line of color transition one more or several lighthouses come into force (recede).

Created by x4SVxx");
        }

        private void vidimost1(int x, int y)
        {
            s = false;
            for (int j = 0; j < N; j++)
            {
                double ax1 = x;
                double ay1 = y;
                double ax2 = SatClone[0, M-1];
                double ay2 = SatClone[1, M-1];

                double bx1 = BoxClone[0, j];
                double by1 = BoxClone[1, j];
                double bx2 = BoxClone[0, j + 1];
                double by2 = BoxClone[1, j + 1];
                double v1 = (bx2 - bx1) * (ay1 - by1) - (by2 - by1) * (ax1 - bx1);
                double v2 = (bx2 - bx1) * (ay2 - by1) - (by2 - by1) * (ax2 - bx1);
                double v3 = (ax2 - ax1) * (by1 - ay1) - (ay2 - ay1) * (bx1 - ax1);
                double v4 = (ax2 - ax1) * (by2 - ay1) - (ay2 - ay1) * (bx2 - ax1);
                if ((v1 * v2 < 0) && (v3 * v4 < 0))
                {
                    s = true;
                }
            }
            if (s == true)
                kolich = 0;
            else
            {
                s = false;
                for (int i = 0; i < qunt; i++)
                {
                    s = false;
                    double ax1 = x;
                    double ay1 = y;
                    double ax2 = SatPos[0, i];
                    double ay2 = SatPos[1, i];
                    s = false;
                    for (int j = 0; j < N; j++)
                    {
                        double bx1 = BoxClone[0, j];
                        double by1 = BoxClone[1, j];
                        double bx2 = BoxClone[0, j + 1];
                        double by2 = BoxClone[1, j + 1];
                        double v1 = (bx2 - bx1) * (ay1 - by1) - (by2 - by1) * (ax1 - bx1);
                        double v2 = (bx2 - bx1) * (ay2 - by1) - (by2 - by1) * (ax2 - bx1);
                        double v3 = (ax2 - ax1) * (by1 - ay1) - (ay2 - ay1) * (bx1 - ax1);
                        double v4 = (ax2 - ax1) * (by2 - ay1) - (ay2 - ay1) * (bx2 - ax1);
                        if ((v1 * v2 < 0) && (v3 * v4 < 0))
                        {
                            s = true;
                        }
                    }
                    if (s == false)
                    {
                        Per.Add(new Point() { X = Convert.ToInt32(SatPos[0, i]), Y = Convert.ToInt32(SatPos[1, i]) });
                    }

                }
                kolich = 0;
                foreach (Point pp in Per)
                {
                    kolich += 1;
                }
                SatPos = new double[2, kolich + 2];
                kolich = 0;
                foreach (Point pp in Per)
                {
                    SatPos[0, kolich] = pp.X;
                    SatPos[1, kolich] = pp.Y;
                    kolich += 1;
                }
                SatPos[0, kolich] = SatClone[0,M-1];
                SatPos[1, kolich] = SatClone[1,M-1];
                s = false;
            }
        }
    }
}
