using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        double Xmax;//Максимальное значение по оси X м
        double Ymax;//Максимальное значение по оси Y м

        double pxX;//Пикселей в метре по оси X
        double pxY;//Пикселей в метре по оси Y

        Bitmap image;//План помещения

        Graphics r;
        Bitmap n;
        Graphics rr;
        Bitmap nn;

        bool IsClicked = false;//Индикатор зажатия ЛКМ для маяка
        bool IsClicked2 = false;//Индикатор зажатия ЛКМ для угла
        bool IsClicked3 = false;//Индикатор зажатия ЛКМ на стене

        int deltax = 0;//Изменение координаты Х при переносе маяка
        int deltay = 0;//Изменение координаты У при переносе маяка

        Rectangle beacontraf;//Трафарет для маяка при переносе
        Rectangle boxtraf;//Трафарет для угла при переносе

        Bitmap bmp;//Настройка области рисования
        Graphics graph;
        Pen pen;//Перо

        Bitmap bmp2;
        Graphics gr;

        Image photokard;

        Bitmap newImage;

        List<Point> beaconlist = new List<Point>();//Лист с координатами маяков
        List<Point> boxlist = new List<Point>();//Лист с координатами комнаты
        List<Point> rightpoint = new List<Point>();//Лист с массивом видимых координат
        List<Point> needpoint = new List<Point>();//Лист маяков видимых мастеру
        List<Point> radiuscoordbeacons = new List<Point>();//Лист с координатами маяков в радиусе
        List<int> wallreplace = new List<int>();
        List<int> wallin = new List<int>();
        List<int> radiusbeacon = new List<int>();//Лист видимых в радиусе маяков

        SolidBrush Brush;//Параметр заливки маяка

        int beaconflag;//Флаг для определения маяка при переносе
        int boxflag;//Флаг для определения угла комнаты при переносе
        int wallflag;//Флаг для определения стороны стены при переносе
        int beaconqunt;//Количество маяков
        int minway;
        int nextway;
        int quntbeaconsradius;

        double[,] SatPos;//Массив с координатами маяков вида [x1, x2, ... , xn]
                         //                                  [y1, y2, ... , yn]
        double[,] SatClone;
        double[,] Grad;//Градиентная матрица
        double[,] Z;//Матрица со значениеми геометрического фактора в каждой точке помещения
        double[,] DeltaX;
        double[,] DeltaY;
        double[,] Tran;//Транспонированная матрица вида [x1, y1]
                       //[x2, y2]
                       //[......]
                       //[xn, yn]
        double[,] Multi;//Перемноженная матрицад для расчетов

        Label[] labelbeacon;//Массив с нумерацией маяков
        Label[] labelbox;//Массив с нумерацией углов

        int beaconnumbers = 0;//Подсчет количества маяков
        int x, y;//Координаты курсора при клике

        double[,] clone;//Матрица клон для кдаления маяка
        double[,] BoxPos;//Массив с координатами комнаты вида [x1, x2, ... , xn]
                         //                                   [y1, y2, ... , yn]
        double[,] BoxClone;

        int uglnumbers = 0;//Подсчет количества углов комнаты
        int uglqunt;//Количество углов комнаты
        int addugl = 0;//Индикатор добавления угла
        int indtakenplace = 0;//Индикатор занятого места
        bool indintersection = false;//Индикатор пересечения
        int indclearroom = 0;//Индикатор стерания комнаты
        int kolich;//Количество маяков с учетом видимости
        int qunt;//Количество маяков видимых при D

        Form2 form = new Form2();//Progress Bar

        bool indphoto = false;//Индикатор загрузки плана
        int minnum = 0;//Номер ближайшего угла
        int nextnum = 0;//Номер 2 угла

        double StartInfoX = 0;//Линейка
        double StartInfoY = 0;

        Label labelInfo;
        bool IsInfo = false;

        double[,] WallPos = new double[0, 0];//Wall
        int walluqnt;//Количество углов стен при удалении и переносе
        int wallnumbers = 0;
        bool wallquntin = false;//Индикатор входа в лист 
        int[] wallqunt = new int[0];
        int schet=0;//Подсчет количества углов стен при переносе и удалении
        double[,] BlockClone = new double[0, 0];
        bool inlist = false;
        public Form1()
        {
            InitializeComponent();
            pictureBox1.MouseClick += pictureBox1_MouseClick;//Клики по picture box
            pictureBox1.MouseMove += pictureBox1_MouseMove;//Координаты курсора
            // Делаем обычный стиль.
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            // Убираем кнопки свернуть, развернуть, закрыть.
            this.ControlBox = false;
            // Убираем заголовок.
            this.Text = "";

            timer2.Start();

            button3.Enabled = false;
            button2.Enabled = false;
            pictureBox1.Enabled = false;
            button1.Enabled = false;
            button22.Enabled = false;
            button23.Enabled = false;
            textBox1.Enabled = false;
            textBox7.Enabled = false;
            textBox9.Enabled = false;
            textBox10.Enabled = false;
            button25.Enabled = false;
            button26.Enabled = false;
            button27.Enabled = false;
            button28.Enabled = false;
            button29.Enabled = false;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
            textBox2.Enabled = false;
            button24.Enabled = false;
            button12.Enabled = false;
            textBox7.Visible = false;
            textBox9.Visible = false;
            textBox8.Visible = false;
            textBox10.Visible = false;
            button14.Enabled = false;
            label7.Visible = false;
            label23.Visible = false;
            label25.Visible = false;
            label26.Visible = false;
            label30.Visible = false;
            label28.Visible = false;
            button16.Enabled = false;
            trackBar1.Enabled = false;
            button18.Enabled = false;

            checkBox1.BackColor = Color.Transparent;//Прозрачный фон
            checkBox2.BackColor = Color.Transparent;
            checkBox3.BackColor = Color.Transparent;
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
            label35.BackColor = Color.Transparent;
            label36.BackColor = Color.Transparent;
            label37.BackColor = Color.Transparent;
            label38.BackColor = Color.Transparent;
            label39.BackColor = Color.Transparent;
            label42.BackColor = Color.Transparent;
            label44.BackColor = Color.Transparent;
            label46.BackColor = Color.Transparent;  
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            groupBox3.Location = new Point(Convert.ToInt32(pictureBox1.Location.X+pictureBox1.Width+5), Convert.ToInt32(pictureBox1.Location.Y));

            Size resolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
            this.Height = resolution.Height-100;
            this.Width = resolution.Width-100;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.CenterToScreen();

            pictureBox1.Width = (resolution.Width-100)/100*50;
            pictureBox1.Height = (resolution.Height - 100) / 100 * 90;

            pictureBox1.Location = new Point(groupBox4.Width + 70, (resolution.Height - 100- pictureBox1.Height)/2);

            groupBox4.Width = (resolution.Width-100) / 100 * 7;
            
            textBox3.Width = groupBox4.Width / 2;
            textBox3.Height = groupBox4.Height / 100 * 4;
            textBox3.Location = new Point(groupBox4.Width / 3, groupBox4.Height / 100 * 40);

            textBox4.Width = groupBox4.Width / 2;
            textBox4.Height = groupBox4.Height / 100 * 4;
            textBox4.Location = new Point(groupBox4.Width / 3, textBox3.Location.Y+textBox3.Height+5);

            textBox7.Width = groupBox4.Width / 2;
            textBox7.Height = groupBox4.Height / 100 * 4;
            textBox7.Location = new Point((groupBox4.Width-textBox7.Width)/2, groupBox4.Height/2);

            label7.Location = new Point((groupBox4.Width-label7.Width)/2, textBox7.Location.Y - textBox7.Height);

            label38.Location = new Point(textBox3.Location.X-label38.Width-1, groupBox4.Height / 100 * 40);
            label39.Location = new Point(textBox4.Location.X - label38.Width - 1, textBox3.Location.Y + textBox3.Height + 5);

            label17.Location = new Point((groupBox4.Width-label17.Width)/2, textBox3.Location.Y-textBox3.Height);

            button5.Width= groupBox4.Width - 6;
            button5.Height = groupBox4.Height / 100 * 5; 
            button5.Location = new Point(3, 10);

            button6.Width = groupBox4.Width - 6;
            button6.Height = groupBox4.Height / 100 * 5;
            button6.Location = new Point(3, button5.Location.Y+5+button5.Height);

            button7.Width = groupBox4.Width - 6;
            button7.Height = groupBox4.Height / 100 * 5;
            button7.Location = new Point(3, button6.Location.Y + 5 + button6.Height);

            button8.Width = groupBox4.Width - 6;
            button8.Height = groupBox4.Height / 100 * 5;
            button8.Location = new Point(3, button7.Location.Y + 5 + button7.Height);

            groupBox3.Width = (resolution.Width - 100) / 100 * 35;
            groupBox3.Height = (resolution.Height - 100);

            groupBox3.Location = new Point(pictureBox1.Location.X + pictureBox1.Width + 10, 0);

            textBox8.Width = groupBox4.Width / 2;
            textBox8.Height = groupBox4.Height / 100 * 4;
            textBox8.Location = new Point((groupBox4.Width-textBox8.Width)/2, groupBox4.Height / 3*2);

            label23.Location = new Point((groupBox4.Width - label23.Width) / 2, textBox8.Location.Y - textBox8.Height);

            label28.Location = new Point((groupBox4.Width-label28.Width)/2, textBox8.Location.Y + textBox8.Height);

            label25.Location = new Point((groupBox4.Width - label25.Width) / 2, label28.Height+label28.Location.Y+10);

            textBox9.Width = groupBox4.Width / 2;
            textBox9.Height = groupBox4.Height / 100 * 4;
            textBox9.Location = new Point(groupBox4.Width / 3, label25.Location.Y+label25.Height+3);

            textBox10.Width = groupBox4.Width / 2;
            textBox10.Height = groupBox4.Height / 100 * 4;
            textBox10.Location = new Point(groupBox4.Width / 3, textBox9.Location.Y+textBox9.Height+3);

            label30.Location = new Point(textBox9.Location.X - label30.Width - 1, textBox9.Location.Y);
            label26.Location = new Point(textBox10.Location.X - label26.Width - 1, textBox10.Location.Y);



            label46.Location = new Point(pictureBox1.Location.X - label46.Width, pictureBox1.Location.Y);
            label44.Location = new Point(pictureBox1.Location.X - label44.Width, pictureBox1.Location.Y+pictureBox1.Height/10);
            label42.Location = new Point(pictureBox1.Location.X - label42.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 2);
            label35.Location = new Point(pictureBox1.Location.X - label35.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 3);
            label33.Location = new Point(pictureBox1.Location.X - label33.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 4);
            label31.Location = new Point(pictureBox1.Location.X - label31.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 5);
            label29.Location = new Point(pictureBox1.Location.X - label29.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 6);
            label27.Location = new Point(pictureBox1.Location.X - label27.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 7);
            label36.Location = new Point(pictureBox1.Location.X - label36.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 8);
            label24.Location = new Point(pictureBox1.Location.X - label24.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 9);
            label2.Location = new Point(pictureBox1.Location.X - label2.Width, pictureBox1.Location.Y + pictureBox1.Height);

            label4.Location = new Point(pictureBox1.Location.X + pictureBox1.Width/10-label4.Width/2, pictureBox1.Location.Y + pictureBox1.Height);
            label6.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10*2 - label6.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
            label8.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10*3 - label8.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
            label10.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10*4 - label10.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
            label12.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10*5 - label12.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
            label14.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10*6 - label14.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
            label16.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10*7 - label16.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
            label18.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10*8 - label18.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
            label20.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10*9 - label20.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
            label22.Location = new Point(pictureBox1.Location.X + pictureBox1.Width - label22.Width, pictureBox1.Location.Y + pictureBox1.Height);



            button4.Location = new Point(groupBox3.Width-button4.Width-1, 1);
            button15.Location = new Point(groupBox3.Width - button4.Width - 3 - button15.Width, 1);
            button9.Location = new Point(groupBox3.Width - button4.Width - 3 - button15.Width - button9.Width-3, 1);
            button17.Location = new Point(button9.Location.X - 3 - button17.Width, 1);

            textBox5.Width = groupBox3.Width / 100 *15;
            textBox5.Height = groupBox3.Height / 100 * 2;
            textBox5.Location = new Point((groupBox3.Width-textBox5.Width)/2, 1+button9.Height*2);

            textBox6.Width = groupBox3.Width / 100 * 15;
            textBox6.Height = groupBox3.Height / 100 * 2;
            textBox6.Location = new Point((groupBox3.Width - textBox5.Width) / 2, textBox5.Location.Y+textBox5.Height+3);

            label3.Location = new Point(textBox5.Location.X-label3.Width-1, 1 + button9.Height * 2);
            label5.Location = new Point(textBox6.Location.X - label5.Width - 1, textBox5.Location.Y + textBox5.Height + 3);
            label9.Location = new Point(textBox6.Location.X+textBox6.Width/2-label9.Width/2, textBox6.Location.Y+textBox6.Height+1);

            button10.Location = new Point(textBox5.Location.X + textBox5.Width + 3, (((textBox6.Location.Y + textBox6.Height - textBox5.Location.Y) - button10.Height) / 2)+textBox5.Location.Y);

            groupBox1.Location = new Point((groupBox3.Width-groupBox1.Width)/2, label9.Location.Y+label9.Height+10);

            textBox1.Width = groupBox3.Width / 100 * 8;
            textBox2.Width = groupBox3.Width / 100 * 8;
            textBox1.Location = new Point ((groupBox2.Width-textBox1.Width)/2,label15.Location.Y+label15.Height+1);
            textBox2.Location = new Point((groupBox2.Width - textBox2.Width) / 2, label11.Location.Y + label11.Height + 1);

            button24.Location = new Point(textBox2.Location.X+textBox2.Width+3,textBox2.Location.Y-2);
            button22.Location = new Point(textBox1.Location.X + textBox1.Width + 3, textBox1.Location.Y-2);

            label11.Location = new Point(7, 15);
            label15.Location = new Point(7, 15);

            groupBox1.Size = new Size(button24.Location.X + button24.Width - label11.Location.X + 10, button14.Location.Y + button14.Height - label11.Location.Y + 20);

            button27.Location = new Point((groupBox1.Width-(button27.Width+5+button25.Width+5+button14.Width))/2,button24.Location.Y+button24.Height+3);
            button25.Location = new Point(button27.Location.X+button27.Width+5, button24.Location.Y + button24.Height + 3);
            button14.Location = new Point(button25.Location.X + button25.Width + 5, button24.Location.Y + button24.Height + 3);

            groupBox1.Size = new Size(button24.Location.X + button24.Width - label11.Location.X + 10, button14.Location.Y + button14.Height - label11.Location.Y + 20);

            groupBox2.Location = new Point((groupBox3.Width - groupBox2.Width) / 2, groupBox1.Location.Y + groupBox1.Height + 10);

            groupBox2.Size = new Size(button22.Location.X + button22.Width - label15.Location.X+10, button23.Location.Y + button23.Height - label15.Location.Y + 20);

            button23.Location = new Point(button22.Location.X+button22.Width-button23.Width, button22.Location.Y + button22.Height + 3);
            button26.Location = new Point(button23.Location.X - 5 - button26.Width, button22.Location.Y + button22.Height + 3);

            groupBox2.Size = new Size(button22.Location.X + button22.Width - label15.Location.X + 10, button23.Location.Y + button23.Height - label15.Location.Y + 20);

            checkBox1.Location = new Point((groupBox3.Width-checkBox1.Width)/2, groupBox2.Location.Y + groupBox2.Height+10);
            checkBox2.Location = new Point((groupBox3.Width - checkBox1.Width) / 2, checkBox1.Height+checkBox1.Location.Y+3);

            button1.Location = new Point((groupBox3.Width-(button1.Width+5+button3.Width+5+button2.Width))/2, checkBox2.Location.Y+checkBox2.Height+10);
            button3.Location = new Point(button1.Location.X+button1.Width+5, checkBox2.Location.Y + checkBox2.Height + 10);
            button2.Location = new Point(button3.Location.X + button3.Width + 5, checkBox2.Location.Y + checkBox2.Height + 10);

            trackBar1.Size = new Size(groupBox3.Width/100*30,groupBox3.Height/100*3);
            trackBar1.Location = new Point((groupBox3.Width-trackBar1.Width)/2, button2.Location.Y+button2.Height+10);

            button16.Location = new Point(trackBar1.Location.X+trackBar1.Width+3,trackBar1.Location.Y);

            label19.Location = new Point(trackBar1.Location.X+(trackBar1.Width/2)-label19.Width/2,trackBar1.Location.Y+trackBar1.Height+3);
            label21.Location = new Point(label19.Location.X+label19.Width+3,label19.Location.Y);

            /* label32.Location = new Point((groupBox3.Width-label32.Width)/2,label21.Location.Y+label21.Height+50);

             listBox1.Location = new Point((groupBox3.Width-(listBox1.Width+20+listBox2.Width))/2, label32.Location.Y+label32.Height*3);
             listBox2.Location = new Point(listBox1.Location.X+listBox1.Width+20, listBox1.Location.Y);

             label1.Location = new Point(listBox1.Location.X+(listBox1.Width/2)-label1.Width/2,label32.Location.Y+label32.Height+3);
             label13.Location = new Point(listBox2.Location.X + (listBox2.Width / 2) - label13.Width / 2, label32.Location.Y + label32.Height+3);

             button29.Location = new Point(listBox2.Location.X,listBox2.Location.Y+listBox2.Height+3);
             button11.Location = new Point(button29.Location.X+button29.Width+5,button29.Location.Y);

             button12.Location = new Point(listBox1.Location.X+listBox1.Width-button12.Width, listBox1.Location.Y + listBox1.Height + 3);
             button28.Location = new Point(button12.Location.X-5-button28.Width, listBox1.Location.Y + listBox1.Height + 3);

             button30.Location = new Point((groupBox3.Width-(button30.Width+5+button13.Width))/2,button28.Location.Y+button28.Height+10);
             button13.Location = new Point(button30.Location.X+button30.Width+5,button30.Location.Y);
 */
            button30.Location = new Point((groupBox3.Width - (button30.Width + 5 + button13.Width)) / 2, groupBox3.Height-1-button30.Height);
            button13.Location = new Point(button30.Location.X + button30.Width + 5, groupBox3.Height - 1 - button13.Height);

            button18.Location = new Point(button30.Location.X - button18.Width - 5, button30.Location.Y);

            listBox1.Size = new Size(groupBox3.Width / 100 * 30, button30.Location.Y - label19.Location.Y - label19.Height - label1.Height - label32.Height - button28.Height-15);
            listBox2.Size = new Size(groupBox3.Width / 100 * 30, button30.Location.Y - label19.Location.Y - label19.Height - label1.Height - label32.Height - button28.Height - 15);


            if (listBox1.Height > 300)
                listBox1.Height = 300;
            if (listBox2.Height > 300)
                listBox2.Height = 300;


            listBox1.Location = new Point((groupBox3.Width - (listBox1.Width + 20 + listBox2.Width)) / 2, button30.Location.Y-button30.Height-6-listBox1.Height);
            listBox2.Location = new Point(listBox1.Location.X + listBox1.Width + 20, button30.Location.Y - button30.Height- 6 - listBox2.Height);

            button29.Location = new Point(listBox2.Location.X, listBox2.Location.Y + listBox2.Height + 3);
            button11.Location = new Point(listBox2.Location.X+listBox2.Width-button11.Width, button29.Location.Y);

            button12.Location = new Point(listBox1.Location.X + listBox1.Width - button12.Width, listBox1.Location.Y + listBox1.Height + 3);
            button28.Location = new Point(listBox1.Location.X, listBox1.Location.Y + listBox1.Height + 3);

            label1.Location = new Point(listBox1.Location.X + (listBox1.Width / 2) - label1.Width / 2, listBox1.Location.Y-label1.Height-1);
            label13.Location = new Point(listBox2.Location.X + (listBox2.Width / 2) - label13.Width / 2, listBox2.Location.Y - label13.Height - 1);

            label32.Location = new Point((groupBox3.Width - label32.Width) / 2, label1.Location.Y-label32.Height-1);

            form.progressBar1.Maximum = (pictureBox1.Width*pictureBox1.Height) * 2;

            checkBox3.Location = new Point(textBox2.Location.X - checkBox3.Width - 3, textBox2.Location.Y);

            textBox11.Width = groupBox3.Width / 100 * 8; ;
            textBox11.Location = new Point(label15.Location.X, button26.Location.Y-textBox11.Height-3);

            label37.Location = new Point(textBox11.Location.X,textBox11.Location.Y+textBox11.Height+1);

            label34.Location = new Point(textBox11.Location.X + textBox11.Width+1,textBox11.Location.Y+3);

            Drawing();//Оси
            startroom();
        }
        private void startroom()
        {
            uglqunt = 6;
            pictureBox1.Enabled = false;

            BoxPos = new double[2, 6];

            BoxPos[0, 0] = pictureBox1.Width*0.4;
            BoxPos[0, 1] = pictureBox1.Width / 10;
            BoxPos[0, 2] = pictureBox1.Width / 10;
            BoxPos[0, 3] = pictureBox1.Width * 0.9;
            BoxPos[1, 0] = pictureBox1.Height / 10;
            BoxPos[1, 1] = pictureBox1.Height / 10;
            BoxPos[1, 2] = pictureBox1.Height * 0.9;
            BoxPos[1, 3] = pictureBox1.Height * 0.9;
            BoxPos[0, 4] = pictureBox1.Width * 0.9;
            BoxPos[1, 4] = pictureBox1.Height / 10;
            BoxPos[0, 5] = pictureBox1.Width * 0.6;
            BoxPos[1, 5] = pictureBox1.Height / 10;

            Xmax = 1000;
            Ymax = 1000;
            pxX = (double)(pictureBox1.Width/Xmax);
            pxY = (double)(pictureBox1.Height/ Ymax);

            label22.Text = Convert.ToString(Xmax);
            label20.Text = Convert.ToString(Xmax * 0.9);
            label18.Text = Convert.ToString(Xmax * 0.8);
            label16.Text = Convert.ToString(Xmax * 0.7);
            label14.Text = Convert.ToString(Xmax * 0.6);
            label12.Text = Convert.ToString(Xmax * 0.5);
            label10.Text = Convert.ToString(Xmax * 0.4);
            label8.Text = Convert.ToString(Xmax * 0.3);
            label6.Text = Convert.ToString(Xmax * 0.2);
            label4.Text = Convert.ToString(Xmax * 0.1);
            label46.Text = Convert.ToString(Ymax);
            label44.Text = Convert.ToString(Ymax * 0.9);
            label42.Text = Convert.ToString(Ymax * 0.8);
            label35.Text = Convert.ToString(Ymax * 0.7);
            label33.Text = Convert.ToString(Ymax * 0.6);
            label31.Text = Convert.ToString(Ymax * 0.5);
            label29.Text = Convert.ToString(Ymax * 0.4);
            label27.Text = Convert.ToString(Ymax * 0.3);
            label36.Text = Convert.ToString(Ymax * 0.2);
            label24.Text = Convert.ToString(Ymax * 0.1);

            Size resolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
            pictureBox1.Location = new Point(groupBox4.Width +  label46.Width+10, (resolution.Height - 100 - pictureBox1.Height) / 2);

            label46.Location = new Point(pictureBox1.Location.X - label46.Width, pictureBox1.Location.Y);
            label44.Location = new Point(pictureBox1.Location.X - label44.Width, pictureBox1.Location.Y + pictureBox1.Height / 10);
            label42.Location = new Point(pictureBox1.Location.X - label42.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 2);
            label35.Location = new Point(pictureBox1.Location.X - label35.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 3);
            label33.Location = new Point(pictureBox1.Location.X - label33.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 4);
            label31.Location = new Point(pictureBox1.Location.X - label31.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 5);
            label29.Location = new Point(pictureBox1.Location.X - label29.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 6);
            label27.Location = new Point(pictureBox1.Location.X - label27.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 7);
            label36.Location = new Point(pictureBox1.Location.X - label36.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 8);
            label24.Location = new Point(pictureBox1.Location.X - label24.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 9);
            label2.Location = new Point(pictureBox1.Location.X - label2.Width, pictureBox1.Location.Y + pictureBox1.Height);

            label4.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 - label4.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
            label6.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 2 - label6.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
            label8.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 3 - label8.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
            label10.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 4 - label10.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
            label12.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 5 - label12.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
            label14.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 6 - label14.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
            label16.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 7 - label16.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
            label18.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 8 - label18.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
            label20.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 9 - label20.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
            label22.Location = new Point(pictureBox1.Location.X + pictureBox1.Width - label22.Width, pictureBox1.Location.Y + pictureBox1.Height);

            for (int j = 0; j < uglqunt; j++)
            {
                listBox2.Items.Add((j + 1) + ")" + "X:" + Math.Round((Convert.ToDouble(BoxPos[0, j]) / Convert.ToDouble(pxX)),2) + "," + "Y:" + Math.Round(((Convert.ToDouble(pictureBox1.Height - BoxPos[1, j]) / Convert.ToDouble(pxY))),2));
                boxlist.Add(new Point() { X = Convert.ToInt32(BoxPos[0, j]), Y = Convert.ToInt32(BoxPos[1, j]) });
            }

            labelbox = new Label[uglqunt];
            roompaint();
            labalbox();
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox5.Text = 1000.ToString();
            textBox6.Text = 1000.ToString();
            textBox2.Text = uglqunt.ToString();
            button10.Enabled = false;
            textBox2.Enabled = false;
            button24.Enabled = false;
            button11.Enabled = false;
            textBox1.Enabled = true;
            button22.Enabled = true;
            button12.Enabled = true;
            button29.Enabled = true;
            button1.Enabled = true;
            button27.Enabled = true;
            uglnumbers = 6;
        }
        private void Drawing()//Оси
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graph = Graphics.FromImage(bmp);
            pen = new Pen(Color.Black);
            pictureBox1.Image = bmp;

            graph.DrawLine(pen, 0, pictureBox1.Height/10, 5, pictureBox1.Height / 10);
            graph.DrawLine(pen, 0, pictureBox1.Height / 5, 5, pictureBox1.Height / 5);
            graph.DrawLine(pen, 0, Convert.ToSingle(pictureBox1.Height * 0.3), 5, Convert.ToSingle(pictureBox1.Height * 0.3));
            graph.DrawLine(pen, 0, Convert.ToSingle(pictureBox1.Height * 0.4), 5, Convert.ToSingle(pictureBox1.Height * 0.4));
            graph.DrawLine(pen, 0, Convert.ToSingle(pictureBox1.Height * 0.5), 5, Convert.ToSingle(pictureBox1.Height * 0.5));
            graph.DrawLine(pen, 0, 1, 5, 1);
            graph.DrawLine(pen, 0, pictureBox1.Height / 20, 5, pictureBox1.Height / 20);
            graph.DrawLine(pen, 0, Convert.ToSingle(pictureBox1.Height * 0.15), 5, Convert.ToSingle(pictureBox1.Height * 0.15));
            graph.DrawLine(pen, 0, Convert.ToSingle(pictureBox1.Height * 0.25), 5, Convert.ToSingle(pictureBox1.Height * 0.25));
            graph.DrawLine(pen, 0, Convert.ToSingle(pictureBox1.Height * 0.35), 5, Convert.ToSingle(pictureBox1.Height * 0.35));
            graph.DrawLine(pen, 0, Convert.ToSingle(pictureBox1.Height * 0.45), 5, Convert.ToSingle(pictureBox1.Height * 0.45));
            graph.DrawLine(pen, 0, Convert.ToSingle(pictureBox1.Height * 0.55), 5, Convert.ToSingle(pictureBox1.Height * 0.55));
            graph.DrawLine(pen, 0, Convert.ToSingle(pictureBox1.Height * 0.6), 5, Convert.ToSingle(pictureBox1.Height * 0.6));
            graph.DrawLine(pen, 0, Convert.ToSingle(pictureBox1.Height * 0.65), 5, Convert.ToSingle(pictureBox1.Height * 0.65));
            graph.DrawLine(pen, 0, Convert.ToSingle(pictureBox1.Height * 0.7), 5, Convert.ToSingle(pictureBox1.Height * 0.7));
            graph.DrawLine(pen, 0, Convert.ToSingle(pictureBox1.Height * 0.75), 5, Convert.ToSingle(pictureBox1.Height * 0.75));
            graph.DrawLine(pen, 0, Convert.ToSingle(pictureBox1.Height * 0.8), 5, Convert.ToSingle(pictureBox1.Height * 0.8));
            graph.DrawLine(pen, 0, Convert.ToSingle(pictureBox1.Height * 0.85), 5, Convert.ToSingle(pictureBox1.Height * 0.85));
            graph.DrawLine(pen, 0, Convert.ToSingle(pictureBox1.Height * 0.9), 5, Convert.ToSingle(pictureBox1.Height * 0.9));
            graph.DrawLine(pen, 0, Convert.ToSingle(pictureBox1.Height * 0.95), 5, Convert.ToSingle(pictureBox1.Height * 0.95));
            graph.DrawLine(pen, 0, pictureBox1.Height-1, 5, pictureBox1.Height-1);

            graph.DrawLine(pen, pictureBox1.Width/10, pictureBox1.Height, pictureBox1.Width / 10, pictureBox1.Height-6);
            graph.DrawLine(pen, pictureBox1.Width / 5, pictureBox1.Height, pictureBox1.Width / 5, pictureBox1.Height - 6);
            graph.DrawLine(pen, Convert.ToSingle(pictureBox1.Width * 0.3), pictureBox1.Height, Convert.ToSingle(pictureBox1.Width * 0.3), pictureBox1.Height - 6);
            graph.DrawLine(pen, Convert.ToSingle(pictureBox1.Width * 0.4), pictureBox1.Height, Convert.ToSingle(pictureBox1.Width * 0.4), pictureBox1.Height - 6);
            graph.DrawLine(pen, Convert.ToSingle(pictureBox1.Width * 0.5), pictureBox1.Height, Convert.ToSingle(pictureBox1.Width * 0.5), pictureBox1.Height - 6);
            graph.DrawLine(pen, 1, pictureBox1.Height, 1, pictureBox1.Height - 6);
            graph.DrawLine(pen, pictureBox1.Width/20, pictureBox1.Height, pictureBox1.Width/20, pictureBox1.Height - 6);
            graph.DrawLine(pen, Convert.ToSingle(pictureBox1.Width * 0.15), pictureBox1.Height, Convert.ToSingle(pictureBox1.Width * 0.15), pictureBox1.Height - 6);
            graph.DrawLine(pen, Convert.ToSingle(pictureBox1.Width * 0.25), pictureBox1.Height, Convert.ToSingle(pictureBox1.Width * 0.25), pictureBox1.Height - 6);
            graph.DrawLine(pen, Convert.ToSingle(pictureBox1.Width * 0.35), pictureBox1.Height, Convert.ToSingle(pictureBox1.Width * 0.35), pictureBox1.Height - 6);
            graph.DrawLine(pen, Convert.ToSingle(pictureBox1.Width * 0.45), pictureBox1.Height, Convert.ToSingle(pictureBox1.Width * 0.45), pictureBox1.Height - 6);
            graph.DrawLine(pen, Convert.ToSingle(pictureBox1.Width * 0.55), pictureBox1.Height, Convert.ToSingle(pictureBox1.Width * 0.55), pictureBox1.Height - 6);
            graph.DrawLine(pen, Convert.ToSingle(pictureBox1.Width * 0.6), pictureBox1.Height, Convert.ToSingle(pictureBox1.Width * 0.6), pictureBox1.Height - 6);
            graph.DrawLine(pen, Convert.ToSingle(pictureBox1.Width * 0.65), pictureBox1.Height, Convert.ToSingle(pictureBox1.Width * 0.65), pictureBox1.Height - 6);
            graph.DrawLine(pen, Convert.ToSingle(pictureBox1.Width * 0.7), pictureBox1.Height, Convert.ToSingle(pictureBox1.Width * 0.7), pictureBox1.Height - 6);
            graph.DrawLine(pen, Convert.ToSingle(pictureBox1.Width * 0.75), pictureBox1.Height, Convert.ToSingle(pictureBox1.Width * 0.75), pictureBox1.Height - 6);
            graph.DrawLine(pen, Convert.ToSingle(pictureBox1.Width * 0.8), pictureBox1.Height, Convert.ToSingle(pictureBox1.Width * 0.8), pictureBox1.Height - 6);
            graph.DrawLine(pen, Convert.ToSingle(pictureBox1.Width * 0.85), pictureBox1.Height, Convert.ToSingle(pictureBox1.Width * 0.85), pictureBox1.Height - 6);
            graph.DrawLine(pen, Convert.ToSingle(pictureBox1.Width * 0.9), pictureBox1.Height, Convert.ToSingle(pictureBox1.Width * 0.9), pictureBox1.Height - 6);
            graph.DrawLine(pen, Convert.ToSingle(pictureBox1.Width * 0.95), pictureBox1.Height, Convert.ToSingle(pictureBox1.Width * 0.95), pictureBox1.Height - 6);
            graph.DrawLine(pen, pictureBox1.Width-1, pictureBox1.Height-1, pictureBox1.Width-1, pictureBox1.Height - 6);
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

            for (int j = 0; j < pictureBox1.Width; j++)
            {
                for (int l = 0; l < pictureBox1.Height; l++)
                {
                    if (Z[j, l] <= 0.7 && Z[j, l] > 0)
                    {
                        pen = new Pen(Color.FromArgb(60, 150, 235));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 0.7 && Z[j, l] <= 0.85)
                    {
                        pen = new Pen(Color.FromArgb(30, 144, 255));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 0.85 && Z[j, l] <= 1)
                    {
                        pen = new Pen(Color.FromArgb(0, 0, 255));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1 && Z[j, l] <= 1.1)
                    {
                        pen = new Pen(Color.FromArgb(40, 40, 220));
                         graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.1 && Z[j, l] <= 1.15)
                    {
                        pen = new Pen(Color.FromArgb(80, 80, 180));
                          graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.15 && Z[j, l] <= 1.2)
                    {
                        pen = new Pen(Color.FromArgb(40, 140, 40));
                          graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.2 && Z[j, l] <= 1.35)
                    {
                        pen = new Pen(Color.FromArgb(50, 200, 50));
                           graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.35 && Z[j, l] <= 1.5)
                    {
                        pen = new Pen(Color.FromArgb(0, 255, 0));
                          graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.5 && Z[j, l] <= 1.65)
                    {
                        pen = new Pen(Color.FromArgb(130, 255, 0));
                         graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.65 && Z[j, l] <= 1.8)
                    {
                        pen = new Pen(Color.FromArgb(180, 255, 50));
                         graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.8 && Z[j, l] <= 1.95)
                    {
                        pen = new Pen(Color.FromArgb(210, 255, 25));
                         graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1.95 && Z[j, l] <= 2.1)
                    {
                        pen = new Pen(Color.FromArgb(255, 255, 0));
                           graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 2.1 && Z[j, l] <= 2.25)
                    {
                        pen = new Pen(Color.FromArgb(255, 220, 0));
                         graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 2.25 && Z[j, l] <= 2.4)
                    {
                        pen = new Pen(Color.FromArgb(255, 200, 0));
                         graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 2.4 && Z[j, l] <= 2.55)
                    {
                        pen = new Pen(Color.FromArgb(255, 180, 0));
                          graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 2.55 && Z[j, l] <= 2.7)
                    {
                        pen = new Pen(Color.FromArgb(255, 150, 0));
                          graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 2.7 && Z[j, l] <= 2.85)
                    {
                        pen = new Pen(Color.FromArgb(255, 130, 0));
                           graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 2.85 && Z[j, l] <= 3)
                    {
                        pen = new Pen(Color.FromArgb(255, 125, 0));
                          graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 3 && Z[j, l] <= 5)
                    {
                        pen = new Pen(Color.FromArgb(255, 120, 0));
                           graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 5 && Z[j, l] <= 10)
                    {
                        pen = new Pen(Color.FromArgb(255, 80, 0));
                         graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 10 && Z[j, l] <= 15)
                    {
                        pen = new Pen(Color.FromArgb(255, 40, 0));
                           graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] == 0)
                    {
                        pen = new Pen(Color.FromArgb(255, 255, 255));
                           graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 15 && Z[j, l] <= 20)
                    {
                        pen = new Pen(Color.FromArgb(255, 0, 0));
                          graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 20 && Z[j, l] <= 30)
                    {
                        pen = new Pen(Color.FromArgb(225, 40, 25));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 30 && Z[j, l] <= 50)
                    {
                        pen = new Pen(Color.FromArgb(178, 34, 34));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 50 && Z[j, l] < 1000000000)
                    {
                        pen = new Pen(Color.FromArgb(139, 0, 0));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1000000000)
                    {
                        if (Z[j + 1, l] == 0 && Z[j - 1, l] == 0 && Z[j, l + 1] == 0 && Z[j, l - 1] == 0)
                        {
                            pen = new Pen(Color.FromArgb(255, 255, 255));
                            graph.DrawEllipse(pen, j, l, 1, 1);
                        }
                    }
                    form.progressBar1.Value += 1;
                }
            }
                    button2.Enabled = true;
            if (form.progressBar1.Value == form.progressBar1.Maximum)
            {
                textBox7.Visible = true;
                label7.Visible = true;
                form.Hide();
            }
        }
        private void Transp(int N, int n)//Транспонированная матрица
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Tran[j, i] = Grad[i, j];
                }
            }
        }
        private void multi(int kol,int n)//Перемножение матриц
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Multi[i, j] = 0;
                    for (int k = 0; k < kol; k++)
                    {
                        Multi[i, j] += Tran[i, k] * Grad[k, j];
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
                temp = Multi[k, k];
                for (int j = 0; j < N; j++)
                {
                    Multi[k, j] /= temp;
                    B[k, j] /= temp;
                }
                for (int i = k + 1; i < N; i++)
                {
                    temp = Multi[i, k];
                    for (int j = 0; j < N; j++)
                    {
                        Multi[i, j] -= Multi[k, j] * temp;
                        B[i, j] -= B[k, j] * temp;
                    }
                }
            }
            for (int k = N - 1; k > 0; k--)
            {
                for (int i = k - 1; i >= 0; i--)
                {
                    temp = Multi[i, k];
                    for (int j = 0; j < N; j++)
                    {
                        Multi[i, j] -= Multi[k, j] * temp;
                        B[i, j] -= B[k, j] * temp;
                    }
                }
            }
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Multi[i, j] = B[i, j];
                }
            }
        }
        private double pogreshnostX()
        {
            return Multi[0,0];
        }

        private double pogreshnostY()
        {
            return Multi[1, 1];
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
                        trac += Multi[i, j];
                    }
                }
            }
            trac = Math.Sqrt(trac);
            return trac;
        }
        private void D(int kol1)//Решение для дальномерного метода
        {
            SatClone = new double[2, beaconqunt + 1];
            for (int h = 0; h < beaconqunt; h++)
            {
                SatClone[0, h] = SatPos[0, h];
                SatClone[1, h] = SatPos[1, h];
            }
            BoxClone = new double[2, uglqunt + 1];
            for (int h = 0; h < uglqunt; h++)
            {
                BoxClone[0, h] = BoxPos[0, h];
                BoxClone[1, h] = BoxPos[1, h];
            }

            Z = new double[pictureBox1.Width, pictureBox1.Height];
            DeltaX = new double[pictureBox1.Width, pictureBox1.Height];
            DeltaY = new double[pictureBox1.Width, pictureBox1.Height];

            int i = 0;
            for (int x = 0; x < pictureBox1.Width; x++)
            {
                for (int y = 0; y < pictureBox1.Height; y++)
                {
                    if (checkBox3.Checked == false)
                    {
                        if (walluqnt == 0)
                            vidimost(x, y);
                        else
                            vidimostwall(x, y);
                        kol1 = kolich;
                    }
                    if (textBox11.Text != "")
                    {
                        radius(kol1, x, y);
                        kol1 = quntbeaconsradius;
                    }
                    if (kol1 < 2)
                        Z[x, y] = 0;
                    else
                    {
                        Grad = new double[kol1 + 1, 2];
                        Tran = new double[2, kol1 + 1];
                        Multi = new double[2, 2];
                        while (i < kol1)
                        {
                            Grad[i, 0] = (x - SatPos[0, i]) / (Math.Sqrt(Math.Pow((x - SatPos[0, i]), 2) + Math.Pow((y - SatPos[1, i]), 2)));
                            Grad[i, 1] = (y - SatPos[1, i]) / (Math.Sqrt(Math.Pow((x - SatPos[0, i]), 2) + Math.Pow((y - SatPos[1, i]), 2)));
                            i += 1;
                        }
                        Transp(kol1,2);
                        multi(kol1,2);
                        inversionMatrix(2);
                        DeltaX[x, y] = pogreshnostX();
                        DeltaY[x, y] = pogreshnostY();
                        Z[x, y] = trace(2);
                    }
                    i = 0;
                    kol1 = beaconqunt;

                    SatPos = new double[2, beaconqunt + 1];
                    for (int h = 0; h < beaconqunt; h++)
                    {
                        SatPos[0, h] = SatClone[0, h];
                        SatPos[1, h] = SatClone[1, h];
                    }

                    rightpoint.Clear();
                    form.progressBar1.Value += 1;
                }
            }
        }

        private void RD(int kol1)//Решение для псевдо разностно-дальномерного метода
        {
            SatClone = new double[2, beaconqunt + 1];
            for (int h = 0; h < beaconqunt; h++)
            {
                SatClone[0, h] = SatPos[0, h];
                SatClone[1, h] = SatPos[1, h];
            }
            BoxClone = new double[2, uglqunt + 1];
            for (int h = 0; h < uglqunt; h++)
            {
                BoxClone[0, h] = BoxPos[0, h];
                BoxClone[1, h] = BoxPos[1, h];
            }

            Z = new double[pictureBox1.Width, pictureBox1.Height];
            DeltaX = new double[pictureBox1.Width, pictureBox1.Height];
            DeltaY = new double[pictureBox1.Width, pictureBox1.Height];
           
            int i = 0;
            for (int x = 0; x < pictureBox1.Width; x++)
            {
                for (int y = 0; y < pictureBox1.Height; y++)
                {
                    if (checkBox3.Checked == false)
                    {
                        if (walluqnt == 0)
                            vidimost(x, y);
                        else
                            vidimostwall(x, y);
                        kol1 = kolich;
                    }
                    if (textBox11.Text != "")
                    {
                        radius(kol1, x, y);
                        kol1 = quntbeaconsradius;
                    }
                    if (kol1 < 3)
                        Z[x, y] = 0;
                    else
                    {
                        Grad = new double[kol1+1, 3];
                        Tran = new double[3, kol1+1];
                        Multi = new double[3, 3];
                        while (i < kol1)
                        {
                            Grad[i, 0] = (x - SatPos[0, i]) / (Math.Sqrt(Math.Pow((x - SatPos[0, i]), 2) + Math.Pow((y - SatPos[1, i]), 2)));
                            Grad[i, 1] = (y - SatPos[1, i]) / (Math.Sqrt(Math.Pow((x - SatPos[0, i]), 2) + Math.Pow((y - SatPos[1, i]), 2)));
                            Grad[i, 2] = 1;
                            i += 1;
                        }
                        Transp(kol1,3);
                        multi(kol1,3);
                        inversionMatrix(3);
                        DeltaX[x, y] = pogreshnostX();
                        DeltaY[x, y] = pogreshnostY();
                        Z[x, y] = trace(3);
                    }
                    i = 0;
                    kol1 = beaconqunt;

                    SatPos = new double[2, beaconqunt + 1];
                    for (int h = 0; h < beaconqunt; h++)
                    {
                        SatPos[0, h] = SatClone[0, h];
                        SatPos[1, h] = SatClone[1, h];
                    }

                    rightpoint.Clear();
                    form.progressBar1.Value += 1;
                }
            }
        }
        /*  private void RD(int kol1)//Решение для разностно-дальномерного метода
          {
              SatClone = new double[2, beaconqunt + 1];
              for (int h = 0; h < beaconqunt; h++)
              {
                  SatClone[0, h] = SatPos[0, h];
                  SatClone[1, h] = SatPos[1, h];
              }
              BoxClone = new double[2, uglqunt + 1];
              for (int h = 0; h < uglqunt; h++)
              {
                  BoxClone[0, h] = BoxPos[0, h];
                  BoxClone[1, h] = BoxPos[1, h];
              }

              Z = new double[pictureBox1.Width, pictureBox1.Height];
              DeltaX = new double[pictureBox1.Width, pictureBox1.Height];
              DeltaY = new double[pictureBox1.Width, pictureBox1.Height];

              int i = 0;
              for (int x = 0; x < pictureBox1.Width; x++)
              {
                  for (int y = 0; y < pictureBox1.Height; y++)
                  {
                      if (checkBox3.Checked == false)
                      {
                          if (walluqnt == 0)
                              lish();
                          else
                              lishwall();
                          if (walluqnt == 0)
                              vidimost1(x, y);
                          else
                              vidimost1wall(x, y);
                          kol1 = kolich + 1;
                      }
                      if (textBox11.Text != "")
                      {
                          radiusRD(kol1, x, y);
                          kol1 = quntbeaconsradius;
                      }
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
                          DeltaX[x, y] = pogreshnostX();
                          DeltaY[x, y] = pogreshnostY();
                          Z[x, y] = trace(2);
                        }                   
                      i = 0;
                      kol1 = beaconqunt;
                      form.progressBar1.Value += 1;

                      SatPos = new double[2, beaconqunt + 1];
                      for (int h = 0; h < beaconqunt; h++)
                      {
                          SatPos[0, h] = SatClone[0, h];
                          SatPos[1, h] = SatClone[1, h];
                      }

                      needpoint.Clear();
                      rightpoint.Clear();
                  }
              }
          }
          */
        private void radius(int kol,int x, int y)
        {
            string radius = textBox11.Text;
            if (textBox11.Text != "")//Проверка на пустоту
            {
                double n;
                if (double.TryParse(textBox11.Text, out n))
                {
                    int beacs=0;
                    double rad = Convert.ToDouble(radius);
                    for(int i=0;i<kol;i++)
                    {
                        double way = Math.Sqrt(Math.Pow((Math.Abs((Convert.ToDouble(x - SatPos[0, i])) / Convert.ToDouble(pxX))), 2) + Math.Pow((Math.Abs(Convert.ToDouble(y - SatPos[1, i]) / Convert.ToDouble(pxY))), 2));
                        if (way < rad)
                        {
                            radiusbeacon.Add(i);
                            beacs++;
                        }          
                    }
                    quntbeaconsradius = beacs;
                    foreach (int pp in radiusbeacon)
                    {
                        radiuscoordbeacons.Add(new Point() { X = (Convert.ToInt32(SatPos[0,pp])), Y = (Convert.ToInt32(SatPos[1,pp])) });
                    }
                    SatPos = new double[2, beacs + 1];
                    beacs = 0;
                    foreach (Point p in radiuscoordbeacons)
                    {
                        SatPos[0, beacs] = p.X;
                        SatPos[1, beacs] = p.Y;
                        beacs++;
                    }
                    beacs = 0;
                    radiusbeacon.Clear();
                    radiuscoordbeacons.Clear();
                }
            }
        }

        private void radiusRD(int kol, int x, int y)
        {
            string radius = textBox11.Text;
            if (textBox11.Text != "")//Проверка на пустоту
            {
                double n;
                if (double.TryParse(textBox11.Text, out n))
                {
                    int beacs = 0;
                    double rad = Convert.ToDouble(radius);
                    double way = Math.Sqrt(Math.Pow((Math.Abs((Convert.ToDouble(x - SatPos[0, kol-1])) / Convert.ToDouble(pxX))), 2) + Math.Pow((Math.Abs(Convert.ToDouble(y - SatPos[1, kol-1]) / Convert.ToDouble(pxY))), 2));
                    if (way > rad)
                    {
                        quntbeaconsradius = 0;
                    }
                    else
                    {
                        for (int i = 0; i < kol; i++)
                        {
                            way = Math.Sqrt(Math.Pow((Math.Abs((Convert.ToDouble(x - SatPos[0, i])) / Convert.ToDouble(pxX))), 2) + Math.Pow((Math.Abs(Convert.ToDouble(y - SatPos[1, i]) / Convert.ToDouble(pxY))), 2));
                            if (way < rad)
                            {
                                radiusbeacon.Add(i);
                                beacs++;
                            }
                        }

                        quntbeaconsradius = beacs;
                        foreach (int pp in radiusbeacon)
                        {
                            radiuscoordbeacons.Add(new Point() { X = (Convert.ToInt32(SatPos[0, pp])), Y = (Convert.ToInt32(SatPos[1, pp])) });
                        }
                        SatPos = new double[2, beacs + 1];
                        beacs = 0;
                        foreach (Point p in radiuscoordbeacons)
                        {
                            SatPos[0, beacs] = p.X;
                            SatPos[1, beacs] = p.Y;
                            beacs++;
                        }
                        beacs = 0;
                        radiusbeacon.Clear();
                        radiuscoordbeacons.Clear();

                        if (quntbeaconsradius >= 3)
                        {
                            for (int i = 0; i < quntbeaconsradius - 1; i++)
                            {
                                way = Math.Sqrt(Math.Pow((Math.Abs((Convert.ToDouble(SatPos[0, i] - SatPos[0, quntbeaconsradius - 1])) / Convert.ToDouble(pxX))), 2) + Math.Pow((Math.Abs(Convert.ToDouble(SatPos[1, i] - SatPos[1, quntbeaconsradius - 1]) / Convert.ToDouble(pxY))), 2));
                                if (way < rad)
                                {
                                    radiusbeacon.Add(i);
                                    beacs++;
                                }
                            }
                            foreach (int pp in radiusbeacon)
                            {
                                radiuscoordbeacons.Add(new Point() { X = (Convert.ToInt32(SatPos[0, pp])), Y = (Convert.ToInt32(SatPos[1, pp])) });
                            }
                            radiuscoordbeacons.Add(new Point() { X = (Convert.ToInt32(SatPos[0, quntbeaconsradius - 1])), Y = (Convert.ToInt32(SatPos[1, quntbeaconsradius - 1])) });
                            quntbeaconsradius = beacs + 1;

                            SatPos = new double[2, beacs + 2];
                            beacs = 0;
                            foreach (Point p in radiuscoordbeacons)
                            {
                                SatPos[0, beacs] = p.X;
                                SatPos[1, beacs] = p.Y;
                                beacs++;
                            }
                            beacs = 0;
                            radiusbeacon.Clear();
                            radiuscoordbeacons.Clear();
                        }
                    }   
                }
            }
        }

        private void labalbeacon()//Нумерация маяков
        {
            for (int b = 0; b < beaconqunt; b++)
            {
                labelbeacon[b] = new Label();
                labelbeacon[b].Location = new Point(Convert.ToInt32(SatPos[0, b]) - 6, Convert.ToInt32(SatPos[1, b]) - 20);
                labelbeacon[b].ForeColor = Color.Black;
                labelbeacon[b].Text = (b + 1).ToString();
                labelbeacon[b].Size = new Size(30, 12);
                labelbeacon[b].BackColor = this.label1.Parent.BackColor;
                labelbeacon[b].Parent = this.pictureBox1;
                labelbeacon[b].BackColor = Color.Transparent;
            }
            pictureBox1.BringToFront();
        }
        private void labalbox()//Нумерация углов
        {
            for (int b = 0; b < uglqunt; b++)
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
            pictureBox1.BringToFront();
        }
        private void roompaint()//Отрисовка комнаты
        {
            for (int j = 0; j < uglqunt; j++)
            {
                graph.DrawRectangle(pen, Convert.ToInt32(BoxPos[0, j]) - 6, Convert.ToInt32(BoxPos[1, j]) - 6, 12, 12);
            }
            for (int i = 0; i < uglqunt - 1; i++)
            {
                graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, i]), Convert.ToInt32(BoxPos[1, i]), Convert.ToInt32(BoxPos[0, i + 1]), Convert.ToInt32(BoxPos[1, i + 1]));
            }
            if (walluqnt!=0)
            {
                for(int i=0;i<walluqnt;i++)
                {
                    graph.DrawRectangle(pen, Convert.ToInt32(WallPos[0, i] - 6), Convert.ToInt32(WallPos[1, i] - 6), 12, 12);
                }
                    for(int u=0;u<walluqnt;u+=2)
                    graph.DrawLine(pen, Convert.ToInt32(WallPos[0, u]), Convert.ToInt32(WallPos[1, u]), Convert.ToInt32(WallPos[0, u + 1]), Convert.ToInt32(WallPos[1, u + 1])); 
            }
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)//Клики
        {
            string sum = textBox1.Text;
            string ugl = textBox2.Text;
            //Маяки
            if (e.Button == MouseButtons.Left)
            {
                if (beaconnumbers < beaconqunt)
                {
                    x = e.Location.X;
                    y = e.Location.Y;
                    if (beaconnumbers > 0)
                    {
                        foreach (Point p in beaconlist)
                        {
                            if (x > p.X - 16 && x < p.X + 16 && y < p.Y + 16 && y > p.Y - 16)
                            {
                                indtakenplace = 1;
                            }
                        }
                        foreach (Point pp in boxlist)
                        {
                            if (x > pp.X - 12 && x < pp.X + 12 && y < pp.Y + 12 && y > pp.Y - 12)
                                indtakenplace = 1;
                        }
                        if (indtakenplace == 1)
                        {
                            indtakenplace = 0;
                            MessageBox.Show("Is this place taken");
                        }
                        else
                        {
                            Drawing();
                            beaconlist.Add(new Point() { X = x, Y = y });//Заполнение листа с координатами маяков
                            foreach (Point pp in beaconlist)
                            {
                                graph.DrawEllipse(pen, pp.X - 8, pp.Y - 8, 16, 16);//Прорисовка маяков
                            }
                            roompaint();
                            listBox1.Items.Clear();//Очистка листа перед новым заполненим 
                            int pole1 = 1;
                            foreach (Point l in beaconlist)
                            {
                                listBox1.Items.Add(pole1 + ")" + "X:" + Math.Round((Convert.ToDouble(l.X)/Convert.ToDouble(pxX)),2) + "," + "Y:" + Math.Round((Convert.ToDouble(pictureBox1.Height - l.Y)/Convert.ToDouble(pxY)),2));//Вывод координат маяков на экран
                                pole1 += 1;
                            }
                            beaconnumbers = beaconnumbers + 1;
                        }
                    }
                    if (beaconnumbers == 0)
                    {
                        Drawing();
                        beaconlist.Add(new Point() { X = x, Y = y });//Заполнение листа с координатами маяков
                        foreach (Point p in beaconlist)
                        {
                            graph.DrawEllipse(pen, p.X - 8, p.Y - 8, 16, 16);//Прорисовка маяков
                        }
                        roompaint();
                        listBox1.Items.Clear();//Очистка листа перед новым заполненим 
                        int pole = 1;
                        foreach (Point l in beaconlist)
                        {
                            listBox1.Items.Add(pole + ")" + "X:" + Math.Round((Convert.ToDouble(l.X) / Convert.ToDouble(pxX)),2) + "," + "Y:" + Math.Round((Convert.ToDouble(pictureBox1.Height - l.Y) / Convert.ToDouble(pxY)),2));//Вывод координат маяков на экран
                            pole += 1;
                        }
                        beaconnumbers = beaconnumbers + 1;
                    }
                    if (beaconnumbers == beaconqunt)
                    {
                        button23.Enabled = true;
                        button1.Enabled = true;
                        button3.Enabled = true;
                        button25.Enabled = true;
                        button27.Enabled = true;
                        button28.Enabled = true;
                        button14.Enabled = true;
                        int kol = 0;
                        foreach (Point lol in beaconlist)//Создание массива с координтами маяков
                        {
                            SatPos[0, kol] = lol.X;
                            SatPos[1, kol] = lol.Y;
                            kol += 1;
                        }
                        labalbeacon();
                        labalbox();
                        kol = 0;
                        button26.Enabled = true;
                    }
                }
            }
            //Маяки
            if (e.Button == MouseButtons.Right)
            {
                if (IsClicked == false && IsClicked2 == false && IsClicked3 == false)
                {
                    if (beaconnumbers >= beaconqunt)
                    {
                        x = e.Location.X;
                        y = e.Location.Y;
                        for (int j = 0; j < beaconqunt; j++)
                        {
                            if (x > (SatPos[0, j] - 8) && x < (SatPos[0, j] + 8) && y < (SatPos[1, j] + 8) && y > (SatPos[1, j] - 8))//Проверка тучка в область маяка
                            {
                                if (beaconqunt > 2)
                                {
                                    beaconflag = j;
                                    for (int k = 0; k < beaconqunt; k++)
                                    {
                                        labelbeacon[k].Dispose();
                                    }
                                    Drawing();
                                    beacontraf = new Rectangle(1, 1, 1, 1);//Убираем с карты образ маяка, создавшийся при перетаскивании
                                    graph.DrawRectangle(pen, beacontraf);
                                    beaconqunt -= 1;
                                    beaconnumbers = beaconqunt;
                                    textBox1.Text = beaconqunt.ToString();
                                    clone = new double[3, beaconqunt + 1];
                                    Grad = new double[beaconqunt + 1, 3];
                                    Tran = new double[3, beaconqunt + 1];
                                    Multi = new double[3, 3];
                                    labelbeacon = new Label[beaconqunt + 1];
                                    deletebeacons(beaconflag);
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
                if (uglnumbers < uglqunt)
                {
                    if (addugl == 0)
                    {
                        x = e.Location.X;
                        y = e.Location.Y;
                        if (uglnumbers > 0)
                        {
                            foreach (Point p in boxlist)
                            {
                                if (x > p.X - 12 && x < p.X + 12 && y < p.Y + 12 && y > p.Y - 12)
                                {
                                    indtakenplace = 1;
                                }
                            }
                            if (indtakenplace == 1)
                            {
                                indtakenplace = 0;
                                MessageBox.Show("Is this place taken");
                            }
                            else
                            {
                                if (indphoto == false)
                                {
                                    Drawing();
                                    boxlist.Add(new Point() { X = x, Y = y });//Заполнение листа с координатами комнаты
                                    foreach (Point p in boxlist)
                                    {
                                        graph.DrawRectangle(pen, p.X - 6, p.Y - 6, 12, 12);//Прорисовка углов
                                    }
                                    BoxPos[0, uglnumbers] = x;
                                    BoxPos[1, uglnumbers] = y;

                                    for (int i = 0; i < uglnumbers; i++)
                                    {
                                        graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, i]), Convert.ToInt32(BoxPos[1, i]), Convert.ToInt32(BoxPos[0, i + 1]), Convert.ToInt32(BoxPos[1, i + 1]));
                                    }
                                    if (indclearroom == 1)
                                    {
                                        foreach (Point pp in beaconlist)
                                        {
                                            graph.DrawEllipse(pen, pp.X - 8, pp.Y - 8, 16, 16);//Прорисовка маяков
                                        }
                                    }
                                    if (uglnumbers == uglqunt - 1)
                                    {
                                        if (indclearroom == 0)
                                        {
                                            button22.Enabled = true;
                                            textBox1.Enabled = true;
                                            pictureBox1.Enabled = false;
                                            button1.Enabled = true;
                                            button12.Enabled = true;
                                        }
                                        if (indclearroom == 1)
                                        {
                                            button23.Enabled = true;
                                            button25.Enabled = true;
                                            button3.Enabled = true;
                                            button14.Enabled = true;
                                            button1.Enabled = true;
                                        }
                                        button27.Enabled = true;
                                        button29.Enabled = true;
                                        button1.Enabled = true;
                                        labalbox();
                                    }
                                    listBox2.Items.Clear();//Очистка листа перед новым заполненим 
                                    int pole = 1;
                                    foreach (Point l in boxlist)
                                    {
                                        listBox2.Items.Add(pole + ")" + "X:" + Math.Round((Convert.ToDouble(l.X) / Convert.ToDouble(pxX)),2) + "," + "Y:" + Math.Round((Convert.ToDouble(pictureBox1.Height - l.Y) / Convert.ToDouble(pxY)),2));//Вывод координат комнаты на экран
                                        pole += 1;
                                    }
                                    uglnumbers += 1;
                                }
                                else
                                {
                                    graph = Graphics.FromImage(bmp);
                                    pen = new Pen(Color.Black);
                                    
                                    pictureBox1.Image = bmp;
                                    boxlist.Add(new Point() { X = x, Y = y });//Заполнение листа с координатами комнаты
                                    foreach (Point p in boxlist)
                                    {
                                        graph.DrawRectangle(pen, p.X - 6, p.Y - 6, 12, 12);//Прорисовка углов
                                    }
                                    BoxPos[0, uglnumbers] = x;
                                    BoxPos[1, uglnumbers] = y;

                                    for (int i = 0; i < uglnumbers; i++)
                                    {
                                        graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, i]), Convert.ToInt32(BoxPos[1, i]), Convert.ToInt32(BoxPos[0, i + 1]), Convert.ToInt32(BoxPos[1, i + 1]));
                                    }
                                    if (indclearroom == 1)
                                    {
                                        foreach (Point pp in beaconlist)
                                        {
                                            graph.DrawEllipse(pen, pp.X - 8, pp.Y - 8, 16, 16);//Прорисовка маяков
                                        }
                                    }
                                    if (uglnumbers == uglqunt - 1)
                                    {
                                       // pictureBox1.BackgroundImage = null;
                                        Drawing();
                                        foreach (Point p in boxlist)
                                        {
                                            graph.DrawRectangle(pen, p.X - 6, p.Y - 6, 12, 12);//Прорисовка углов
                                        }
                                        for (int i = 0; i < uglnumbers; i++)
                                        {
                                            graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, i]), Convert.ToInt32(BoxPos[1, i]), Convert.ToInt32(BoxPos[0, i + 1]), Convert.ToInt32(BoxPos[1, i + 1]));
                                        }
                                        if (indclearroom == 0)
                                        {
                                            button22.Enabled = true;
                                            textBox1.Enabled = true;
                                            pictureBox1.Enabled = false;
                                            button1.Enabled = true;
                                        }
                                        if (indclearroom == 1)
                                        {
                                            button23.Enabled = true;
                                            button25.Enabled = true;
                                            button3.Enabled = true;
                                            button14.Enabled = false;
                                            button1.Enabled = true;
                                        }
                                        button27.Enabled = true;
                                        button29.Enabled = true;
                                        button1.Enabled = true;
                                        labalbox();
                                    }
                                    listBox2.Items.Clear();//Очистка листа перед новым заполненим 
                                    int pole = 1;
                                    foreach (Point l in boxlist)
                                    {
                                        listBox2.Items.Add(pole + ")" + "X:" + Math.Round((Convert.ToDouble(l.X) / Convert.ToDouble(pxX)),2) + "," + "Y:" + Math.Round((Convert.ToDouble(pictureBox1.Height - l.Y) / Convert.ToDouble(pxY)),2));//Вывод координат комнаты на экран
                                        pole += 1;
                                    }
                                    uglnumbers += 1;
                                }
                            }
                        }
                        if (uglnumbers == 0)
                        {
                            if (indphoto == false)
                            {
                                Drawing();
                                boxlist.Add(new Point() { X = x, Y = y });//Заполнение листа с координатами комнаты
                                foreach (Point p in boxlist)
                                {
                                    graph.DrawRectangle(pen, p.X - 6, p.Y - 6, 12, 12);//Прорисовка углов
                                }
                                if (indclearroom == 1)
                                {
                                    foreach (Point pp in beaconlist)
                                    {
                                        graph.DrawEllipse(pen, pp.X - 8, pp.Y - 8, 16, 16);//Прорисовка маяков
                                    }
                                }
                                BoxPos[0, uglnumbers] = x;
                                BoxPos[1, uglnumbers] = y;
                                listBox2.Items.Clear();//Очистка листа перед новым заполненим 
                                int pole = 1;
                                foreach (Point l in boxlist)
                                {
                                    listBox2.Items.Add(pole + ")" + "X:" + Math.Round((Convert.ToDouble(l.X) / Convert.ToDouble(pxX)),2) + "," + "Y:" + Math.Round((Convert.ToDouble(pictureBox1.Height - l.Y) / Convert.ToDouble(pxY)),2));//Вывод координат комнаты на экран
                                    pole += 1;
                                }
                                uglnumbers += 1;
                            }
                            else
                            {
                                pictureBox1.BackgroundImage = photokard;
                                graph = Graphics.FromImage(bmp);
                                pen = new Pen(Color.Black);
                                pictureBox1.Image = bmp;

                                boxlist.Add(new Point() { X = x, Y = y });//Заполнение листа с координатами комнаты
                                foreach (Point p in boxlist)
                                {
                                    listBox1.Items.Add("_");
                                    graph.DrawRectangle(pen, p.X - 6, p.Y - 6, 12, 12);//Прорисовка углов
                                }
                                if (indclearroom == 1)
                                {
                                    foreach (Point pp in beaconlist)
                                    {
                                        graph.DrawEllipse(pen, pp.X - 8, pp.Y - 8, 16, 16);//Прорисовка маяков
                                    }
                                }
                                BoxPos[0, uglnumbers] = x;
                                BoxPos[1, uglnumbers] = y;
                                listBox2.Items.Clear();//Очистка листа перед новым заполненим 
                                int pole = 1;
                                foreach (Point l in boxlist)
                                {
                                    listBox2.Items.Add(pole + ")" + "X:" + Math.Round((Convert.ToDouble(l.X) / Convert.ToDouble(pxX)),2) + "," + "Y:" + Math.Round((Convert.ToDouble(pictureBox1.Height - l.Y) / Convert.ToDouble(pxY)),2));//Вывод координат комнаты на экран
                                    pole += 1;
                                }
                                uglnumbers += 1;
                            }
                        }
                    }
                }
                if (uglnumbers < uglqunt)
                {
                    if (addugl != 0)
                    {
                        x = e.Location.X;
                        y = e.Location.Y;
                        foreach (Point p in boxlist)
                        {
                            if (x > p.X - 12 && x < p.X + 12 && y < p.Y + 12 && y > p.Y - 12)
                            {
                                indtakenplace = 1;
                            }
                        }
                        foreach (Point p in beaconlist)
                        {
                            if (x > p.X - 16 && x < p.X + 16 && y < p.Y + 16 && y > p.Y - 16)
                            {
                                indtakenplace = 1;
                            }
                        }
                        if (indtakenplace == 1)
                        {
                            indtakenplace = 0;
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
                            button14.Enabled = true;
                            for (int j = 0; j < beaconqunt; j++)
                            {
                                graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                            }
                            double[,] BoxPoint = new double[2, uglqunt - 1];
                            for (int i = 0; i < uglqunt - 1; i++)
                            {
                                BoxPoint[0, i] = BoxPos[0, i];
                                BoxPoint[1, i] = BoxPos[1, i];
                            }
                            BoxPos = new double[2, uglqunt + 1];
                            if ((minnum == 0 && nextnum == uglqunt - 2) || (minnum == uglqunt - 2 && nextnum == 0))
                            {
                                if (nextnum == 0)
                                {
                                    for (int i = 0; i < uglqunt - 1; i++)
                                    {
                                        BoxPos[0, i] = BoxPoint[0, i];
                                        BoxPos[1, i] = BoxPoint[1, i];
                                    }
                                    BoxPos[0, uglqunt - 1] = x;
                                    BoxPos[1, uglqunt - 1] = y;
                                }
                                else
                                {
                                    BoxPos[0, 0] = x;
                                    BoxPos[1, 0] = y;
                                    for (int i = 1; i < uglqunt; i++)
                                    {
                                        BoxPos[0, i] = BoxPoint[0, i-1];
                                        BoxPos[1, i] = BoxPoint[1, i-1];
                                    }
                                }
                            }
                            else
                            {
                                if (minnum < nextnum)
                                {
                                    for (int i = 0; i <= minnum; i++)
                                    {
                                        BoxPos[0, i] = BoxPoint[0, i];
                                        BoxPos[1, i] = BoxPoint[1, i];
                                    }
                                    BoxPos[0, minnum + 1] = x;
                                    BoxPos[1, minnum + 1] = y;
                                    for (int i = minnum + 2; i < uglqunt; i++)
                                    {
                                        BoxPos[0, i] = BoxPoint[0, i - 1];
                                        BoxPos[1, i] = BoxPoint[1, i - 1];
                                    }
                                }
                                if (minnum > nextnum)
                                {
                                    for (int i = 0; i <= nextnum; i++)
                                    {
                                        BoxPos[0, i] = BoxPoint[0, i];
                                        BoxPos[1, i] = BoxPoint[1, i];
                                    }
                                    BoxPos[0, nextnum + 1] = x;
                                    BoxPos[1, nextnum + 1] = y;
                                    for (int i = nextnum + 2; i < uglqunt; i++)
                                    {
                                        BoxPos[0, i] = BoxPoint[0, i - 1];
                                        BoxPos[1, i] = BoxPoint[1, i - 1];
                                    }
                                }
                            }
                            boxlist.Clear();
                            for (int i = 0; i < uglqunt; i++)
                                boxlist.Add(new Point() { X = Convert.ToInt32(BoxPos[0, i]), Y = Convert.ToInt32(BoxPos[1, i]) });
                            foreach (Point p in boxlist)
                            {
                                graph.DrawRectangle(pen, p.X - 6, p.Y - 6, 12, 12);//Прорисовка углов
                            }
                            listBox2.Items.Clear();//Очистка листа перед новым заполненим 
                            for (int j = 0; j < uglqunt; j++)
                            {
                                listBox2.Items.Add((j + 1) + ")" + "X:" + Math.Round((Convert.ToDouble(BoxPos[0, j]) / Convert.ToDouble(pxX)),2) + "," + "Y:" + Math.Round((Convert.ToDouble(pictureBox1.Height - BoxPos[1, j]) / Convert.ToDouble(pxY)),2));
                            }
                            labalbeacon();
                            labalbox();
                            roompaint();
                            uglnumbers += 1;
                            addugl = 0;
                        }
                    }
                }

            }
            //Комната
            if (e.Button == MouseButtons.Right)
            {
                if (IsClicked == false && IsClicked2 == false && IsClicked3 == false)
                {
                    if (uglnumbers >= uglqunt)
                    {
                        x = e.Location.X;
                        y = e.Location.Y;
                        for (int j = 0; j < uglqunt; j++)
                        {
                            if (x > (BoxPos[0, j] - 6) && x < (BoxPos[0, j] + 6) && y < (BoxPos[1, j] + 6) && y > (BoxPos[1, j] - 6))//Проверка тучка в область маяка
                            {
                                if (uglqunt > 3)
                                {
                                    boxflag = j;
                                    Drawing();
                                    for (int k = 0; k < uglqunt; k++)
                                    {
                                        labelbox[k].Dispose();
                                    }
                                    boxtraf = new Rectangle(1, 1, 1, 1);//Убираем с карты образ маяка, создавшийся при перетаскивании
                                    graph.DrawRectangle(pen, boxtraf);
                                    uglqunt -= 1;
                                    textBox2.Text = uglqunt.ToString();
                                    clone = new double[2, uglqunt + 1];
                                    labelbox = new Label[uglqunt + 1];
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
            if(e.Button==MouseButtons.Left)//Wall
            {
                if (wallnumbers<walluqnt)
                {
                    x = e.Location.X;
                    y = e.Location.Y;
                    if (wallnumbers == walluqnt-1 || wallnumbers == walluqnt-2)
                    {
                        foreach (Point p in beaconlist)
                        {
                            if (x > p.X - 16 && x < p.X + 16 && y < p.Y + 16 && y > p.Y - 16)
                            {
                                indtakenplace = 1;
                            }
                        }
                        if (indtakenplace == 1)
                        {
                            indtakenplace = 0;
                            MessageBox.Show("Is this place taken");
                        }
                        else
                        {
                            foreach (Point p in boxlist)
                            {
                                if (x > p.X - 12 && x < p.X + 12 && y < p.Y + 12 && y > p.Y - 12)
                                {
                                    WallPos[0, wallnumbers] = p.X;
                                    WallPos[1, wallnumbers] = p.Y;

                                    bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                                    graph = Graphics.FromImage(bmp);
                                    pen = new Pen(Color.Black);
                                    pictureBox1.Image = bmp;

                                    graph.DrawRectangle(pen, Convert.ToInt32(WallPos[0, wallnumbers] - 6), Convert.ToInt32(WallPos[1, wallnumbers] - 6), 12, 12);
                                    for (int j = 0; j < beaconqunt; j++)
                                    {
                                        graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                                    }
                                    for (int j = 0; j < uglqunt; j++)
                                    {
                                        graph.DrawRectangle(pen, Convert.ToInt32(BoxPos[0, j]) - 6, Convert.ToInt32(BoxPos[1, j]) - 6, 12, 12);
                                    }
                                    for (int i = 0; i < uglqunt - 1; i++)
                                    {
                                        graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, i]), Convert.ToInt32(BoxPos[1, i]), Convert.ToInt32(BoxPos[0, i + 1]), Convert.ToInt32(BoxPos[1, i + 1]));
                                    }
                                    for(int i=0;i<walluqnt-2;i+=2)
                                    {
                                        graph.DrawLine(pen, Convert.ToInt32(WallPos[0, i]), Convert.ToInt32(WallPos[1, i]), Convert.ToInt32(WallPos[0, i + 1]), Convert.ToInt32(WallPos[1, i+1]));
                                    }
                                    for (int i = 0; i < walluqnt-1; i++)
                                    {
                                        graph.DrawRectangle(pen, Convert.ToInt32(WallPos[0, i] - 6), Convert.ToInt32(WallPos[1, i] - 6), 12, 12);
                                    }
                                    wallquntin = true;
                                }
                                for (int i = 0; i < walluqnt - 2; i++)
                                {
                                    if (x > WallPos[0, i] - 12 && x < WallPos[0, i] + 12 && y < WallPos[1, i] + 12 && y > WallPos[1, i] - 12)
                                    {
                                        wallquntin = true;
                                        WallPos[0, wallnumbers] = WallPos[0, i];
                                        WallPos[1, wallnumbers] = WallPos[1, i];
                                    }
                                }
                            }
                            if (wallquntin == true)
                            {
                                wallnumbers += 1;
                                wallquntin = false;
                            }
                            else
                            {
                                WallPos[0, wallnumbers] = x;
                                WallPos[1, wallnumbers] = y;

                                bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                                graph = Graphics.FromImage(bmp);
                                pen = new Pen(Color.Black);
                                pictureBox1.Image = bmp;

                                graph.DrawRectangle(pen, Convert.ToInt32(WallPos[0, wallnumbers] - 6), Convert.ToInt32(WallPos[1, wallnumbers] - 6), 12, 12);
                                for (int j = 0; j < beaconqunt; j++)
                                {
                                    graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                                }
                                for (int j = 0; j < uglqunt; j++)
                                {
                                    graph.DrawRectangle(pen, Convert.ToInt32(BoxPos[0, j]) - 6, Convert.ToInt32(BoxPos[1, j]) - 6, 12, 12);
                                }
                                for (int i = 0; i < uglqunt - 1; i++)
                                {
                                    graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, i]), Convert.ToInt32(BoxPos[1, i]), Convert.ToInt32(BoxPos[0, i + 1]), Convert.ToInt32(BoxPos[1, i + 1]));
                                }
                                for (int i = 0; i < walluqnt - 2; i += 2)
                                {
                                    graph.DrawLine(pen, Convert.ToInt32(WallPos[0, i]), Convert.ToInt32(WallPos[1, i]), Convert.ToInt32(WallPos[0, i + 1]), Convert.ToInt32(WallPos[1, i+1]));
                                }
                                for (int i = 0; i < walluqnt-1; i++)
                                {
                                    graph.DrawRectangle(pen, Convert.ToInt32(WallPos[0, i] - 6), Convert.ToInt32(WallPos[1, i] - 6), 12, 12);
                                }
                                wallnumbers += 1;
                            }
                        }
                    }
                    if (wallnumbers == walluqnt)
                    {
                        bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                        graph = Graphics.FromImage(bmp);
                        pen = new Pen(Color.Black);
                        pictureBox1.Image = bmp;

                        for(int i=0;i<walluqnt;i++)
                        {
                            graph.DrawRectangle(pen, Convert.ToInt32(WallPos[0, i]-6), Convert.ToInt32(WallPos[1, i]-6), 12, 12);
                        }
                        for (int j = 0; j < beaconqunt; j++)
                        {
                            graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                        }
                        roompaint();
                        button14.Enabled = true;
                        button23.Enabled = true;
                        button25.Enabled = true;
                        button3.Enabled = true;
                        button27.Enabled = true;
                        button26.Enabled = true;
                        button1.Enabled = true;
                    }
                }
            }
            //wall
            if (e.Button == MouseButtons.Right)
            {
                if (IsClicked == false && IsClicked2 == false && IsClicked3 == false)
                {
                    if (wallnumbers >= walluqnt)
                    {
                        bool east = false;
                        x = e.Location.X;
                        y = e.Location.Y;
                        for (int i = 0; i < walluqnt; i++)
                        {
                            if (x > WallPos[0, i] - 6 && x < WallPos[0, i] + 6 && y < WallPos[1, i] + 6 && y > WallPos[1, i] - 6)
                            {
                                east = true;
                                wallreplace.Add(i);
                                Drawing();
                                boxtraf = new Rectangle(1, 1, 1, 1);//Убираем с карты образ маяка, создавшийся при перетаскивании
                                graph.DrawRectangle(pen, boxtraf);
                            }
                        }
                        if (east)
                        {
                            east = false;
                            deletewall();
                        }
                    }
                }
            }
        }
        private void deletewall()
        {
            List<int> inl = new List<int>();
            foreach (int p in wallreplace)
            {
                if(p%2==0)
                {
                    inl.Add(p+1);
                }
                if(p%2!=0)
                {
                    inl.Add(p - 1);
                }
            }
            foreach(int p in inl)
            {
                wallreplace.Add(p);
            }
            schet = 0;
            for (int i = 0; i < walluqnt; i++)
            {
                inlist = false;
                foreach (int p in wallreplace)
                {
                    if (i == p)
                        inlist = true;
                }
                if (inlist == false)
                {
                    wallin.Add(i);
                    schet += 1;
                }
                else
                    inlist = false;
            }
            BlockClone = new double[2, schet];
            int z = 0;
            foreach (int p in wallin)
            {
                BlockClone[0, z] = WallPos[0, p];
                BlockClone[1, z] = WallPos[1, p];
                z += 1;
            }
            WallPos = new double[2, schet];
            for(int i=0;i<schet;i++)
            {
                WallPos[0, i] = BlockClone[0, i];
                WallPos[1, i] = BlockClone[1, i];
            }
            walluqnt = schet;
            wallnumbers = walluqnt;
            schet = 0;
            wallin.Clear();
            wallreplace.Clear();
            inl.Clear();
            for (int j = 0; j < beaconqunt; j++)
            {
                graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
            }
            roompaint();
        }
        private void deletebeacons(int numberbeacon)
        {
            Point del = new Point() { X = Convert.ToInt32(SatPos[0, numberbeacon]), Y = Convert.ToInt32(SatPos[1, numberbeacon]) };
            beaconlist.Remove(del);

            for (int i = 0; i <= beaconqunt; i++)
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
            SatPos = new double[2, beaconqunt + 1];
            for (int i = 0; i < beaconqunt; i++)
            {
                SatPos[0, i] = clone[0, i];
                SatPos[1, i] = clone[1, i];
            }
            for (int j = 0; j < beaconqunt; j++)
            {
                graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
            }
            roompaint();
            listBox1.Items.Clear();
            for (int i = 0; i < beaconqunt; i++)
            {
                listBox1.Items.Add((i + 1) + ")" + "X:" + Math.Round((Convert.ToDouble(SatPos[0, i])/Convert.ToDouble(pxX)),2) + "," + "Y:" + Math.Round((Convert.ToDouble(pictureBox1.Height - SatPos[1, i])/Convert.ToDouble(pxY)),2));
            }
            labalbeacon();
            button2.PerformClick();
        }
        private void deleteugl()
        {
            Point del = new Point() { X = Convert.ToInt32(BoxPos[0, boxflag]), Y = Convert.ToInt32(BoxPos[1, boxflag]) };
            boxlist.Remove(del);

            for (int i = 0; i <= uglqunt; i++)
            {
                if (i < boxflag)
                {
                    clone[0, i] = BoxPos[0, i];
                    clone[1, i] = BoxPos[1, i];
                }
                if (i > boxflag)
                {
                    clone[0, i - 1] = BoxPos[0, i];
                    clone[1, i - 1] = BoxPos[1, i];
                }
            }
            BoxPos = new double[2, uglqunt + 1];
            for (int i = 0; i < uglqunt; i++)
            {
                BoxPos[0, i] = clone[0, i];
                BoxPos[1, i] = clone[1, i];
            }
            for (int j = 0; j < beaconqunt; j++)
            {
                graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
            }
            roompaint();
            listBox2.Items.Clear();
            for (int i = 0; i < uglqunt; i++)
            {
                listBox2.Items.Add((i + 1) + ")" + "X:" + Math.Round((Convert.ToDouble(BoxPos[0, i])/Convert.ToDouble(pxX)),2) + "," + "Y:" + Math.Round((Convert.ToDouble(pictureBox1.Height - BoxPos[1, i])/Convert.ToDouble(pxY)),2));
            }
            labalbox();
            button2.PerformClick();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)//Вывод координат курсора на экран
        {
            textBox3.Text = (Math.Round((Convert.ToDouble(e.Location.X) / Convert.ToDouble(pxX)),2)).ToString(); 
            textBox4.Text = (Math.Round(((Convert.ToDouble(pictureBox1.Height - e.Location.Y)) / Convert.ToDouble(pxY)),2)).ToString();     

            if(textBox7.Visible == true)
            {
                bmp2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                gr = Graphics.FromImage(bmp2);
                pictureBox1.BackgroundImage = bmp;
                pen = new Pen(Color.Black);
                string delt = textBox8.Text;
                if (textBox8.Text != "")
                {
                    double n;
                    if (double.TryParse(textBox8.Text, out n))
                    {
                        pen.Width = 3;
                        if (Convert.ToSingle(Z[e.Location.X, e.Location.Y] * Convert.ToDouble(Math.Sqrt(DeltaX[e.Location.X, e.Location.Y])) * Convert.ToDouble(delt))<(pictureBox1.Width*2) && Convert.ToSingle(Z[e.Location.X, e.Location.Y] * Convert.ToDouble(Math.Sqrt(DeltaY[e.Location.X, e.Location.Y])) * Convert.ToDouble(delt))<(pictureBox1.Height*2))
                        gr.DrawEllipse(pen, Convert.ToSingle(e.Location.X - (((Z[e.Location.X, e.Location.Y] * Convert.ToDouble(Math.Sqrt(DeltaX[e.Location.X, e.Location.Y]))) * Convert.ToDouble(delt)) / 2)), Convert.ToSingle(e.Location.Y - (((Z[e.Location.X, e.Location.Y] * Convert.ToDouble(Math.Sqrt(DeltaY[e.Location.X, e.Location.Y]))) * Convert.ToDouble(delt)) / 2)), Convert.ToSingle(Z[e.Location.X, e.Location.Y] * Convert.ToDouble(Math.Sqrt(DeltaX[e.Location.X, e.Location.Y])) * Convert.ToDouble(delt)), Convert.ToSingle(Z[e.Location.X, e.Location.Y] * Convert.ToDouble(Math.Sqrt(DeltaY[e.Location.X, e.Location.Y])) * Convert.ToDouble(delt)));
                        pen.Width = 1;
                    }
                }
                pictureBox1.Image = bmp2;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (textBox7.Visible == true)
            {
                button16.Enabled = true;
                trackBar1.Enabled = true;
                textBox10.Visible = true;
                textBox8.Visible = true;
                textBox9.Visible = true;
                label23.Visible = true;
                label25.Visible = true;
                label26.Visible = true;
                label30.Visible = true;
                label28.Visible = true;
            }
            if (textBox7.Visible == false)
            {
                button16.Enabled = false;
                trackBar1.Enabled = false;
                textBox10.Visible = false;
                textBox8.Visible = false;
                textBox9.Visible = false;
                label23.Visible = false;
                label25.Visible = false;
                label26.Visible = false;
                label30.Visible = false;
                label28.Visible = false;
                pictureBox1.BackgroundImage = null;
                pictureBox1.Image = bmp;
                timer1.Stop();
            }
            if(trackBar1.Enabled==false)
            {
                trackBar1.Value = 100;
                label21.Text = 100.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)//Построение(GO)
        {
            pictureBox1.BackgroundImage = null;
            indphoto = false;
            newImage = null;
            photokard = null;

            pic1.Size = new Size(201, 50);
            pic1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y - 3 - pic1.Height);
            n = new Bitmap(pic1.Width, pic1.Height);
            pic1.Image = n;
            r = Graphics.FromImage(n);

            pic2.Size = new Size(301, 50);
            pic2.Location = new Point(pic1.Location.X+pic1.Width+1, pictureBox1.Location.Y - 3 - pic1.Height);
            nn = new Bitmap(pic2.Width, pic2.Height);
            pic2.Image = nn;
            rr = Graphics.FromImage(nn);

            Rectangle rec1 = new Rectangle();
            rec1.Size = new Size(pic1.Width, pic1.Height);
            LinearGradientBrush br = new LinearGradientBrush(rec1, Color.Black, Color.Black, 0, false);
            ColorBlend cb = new ColorBlend();
            cb.Positions = new[] { 0, 1 / 6f, 2 / 6f, 3 / 6f, 4 / 6f, 5 / 6f, 1 };
            cb.Colors = new[] { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.RoyalBlue, Color.CornflowerBlue };
            br.InterpolationColors = cb;
            r.FillRectangle(br, rec1);

            Rectangle rec2 = new Rectangle();
            rec2.Size = new Size(pic2.Width, pic2.Height);
            LinearGradientBrush brr = new LinearGradientBrush(rec2, Color.Black, Color.Black, 0, false);
            ColorBlend cbb = new ColorBlend();
            cbb.Positions = new[] { 0, 1 / 2f, 1 };
            cbb.Colors = new[] { Color.Red, Color.Brown, Color.Maroon };
            brr.InterpolationColors = cbb;
            rr.FillRectangle(brr, rec2);

            pictureBox1.BackgroundImage = null;
            timer1.Start();
            if (checkBox1.Checked == true && checkBox2.Checked == false)//D
            {
                form.Show();
                button2.PerformClick();
                form.progressBar1.Value = 0;
                D(beaconqunt);
                // Surf();
                color();
                pen = new Pen(Color.Black);
                for (int j = 0; j < beaconqunt; j++)
                {
                    Brush = new SolidBrush(Color.Black);
                    graph.FillEllipse(Brush, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                }
                for (int j = 0; j < uglqunt; j++)
                {
                    Brush = new SolidBrush(Color.Black);
                    graph.FillRectangle(Brush, Convert.ToInt32(BoxPos[0, j]) - 6, Convert.ToInt32(BoxPos[1, j]) - 6, 12, 12);
                }
                for (int j = 0; j < walluqnt; j++)
                {
                    Brush = new SolidBrush(Color.Black);
                    graph.FillRectangle(Brush, Convert.ToInt32(WallPos[0, j]) - 6, Convert.ToInt32(WallPos[1, j]) - 6, 12, 12);
                }
                pen.Width = 5;
                for (int i = 0; i < uglqunt - 1; i++)
                {
                    graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, i]), Convert.ToInt32(BoxPos[1, i]), Convert.ToInt32(BoxPos[0, i + 1]), Convert.ToInt32(BoxPos[1, i + 1]));
                }
                if (walluqnt != 0)
                {
                    for (int u = 0; u < walluqnt; u += 2)
                        graph.DrawLine(pen, Convert.ToInt32(WallPos[0, u]), Convert.ToInt32(WallPos[1, u]), Convert.ToInt32(WallPos[0, u + 1]), Convert.ToInt32(WallPos[1, u + 1]));
                }
                pen.Width = 1;
                for (int j = 0; j < uglqunt; j++)
                {
                    graph.DrawRectangle(pen, Convert.ToInt32(BoxPos[0, j]) - 6, Convert.ToInt32(BoxPos[1, j]) - 6, 12, 12);
                }
                if (walluqnt != 0)
                {
                    for (int i = 0; i < walluqnt; i++)
                    {
                        graph.DrawRectangle(pen, Convert.ToInt32(WallPos[0, i] - 6), Convert.ToInt32(WallPos[1, i] - 6), 12, 12);
                    }
                }
                pen.Width = 2;
                if (textBox11.Text!="")
                {
                    double radius = Convert.ToDouble(textBox11.Text);
                    for(int i =0;i< beaconqunt;i++)
                    {
                        graph.DrawEllipse(pen, Convert.ToSingle(SatPos[0, i] - (radius * pxX)), Convert.ToSingle(SatPos[1, i] - (radius * pxY)), Convert.ToSingle(radius * Convert.ToDouble(pxX)) * 2, Convert.ToSingle(radius * Convert.ToDouble(pxY)) * 2);
                    }
                }
                pen.Width = 1;
            }
            if (checkBox2.Checked == true && checkBox1.Checked == false)//RD
            {
                if (beaconqunt == 2)
                {
                    MessageBox.Show("Install more beacons");
                }
                else
                {
                    form.Show();
                    button2.PerformClick();
                    form.progressBar1.Value = 0;
                    RD(beaconqunt);
                    // Surf();
                    color();
                    pen = new Pen(Color.Black);
                    for (int j = 0; j < beaconqunt; j++)
                    {
                        Brush = new SolidBrush(Color.Black);
                        graph.FillEllipse(Brush, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                    }
                    for (int j = 0; j < uglqunt; j++)
                    {
                        Brush = new SolidBrush(Color.Black);
                        graph.FillRectangle(Brush, Convert.ToInt32(BoxPos[0, j]) - 6, Convert.ToInt32(BoxPos[1, j]) - 6, 12, 12);
                    }
                    for (int j = 0; j < walluqnt; j++)
                    {
                        Brush = new SolidBrush(Color.Black);
                        graph.FillRectangle(Brush, Convert.ToInt32(WallPos[0, j]) - 6, Convert.ToInt32(WallPos[1, j]) - 6, 12, 12);
                    }
                    pen.Width = 5;
                    for (int i = 0; i < uglqunt - 1; i++)
                    {
                        graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, i]), Convert.ToInt32(BoxPos[1, i]), Convert.ToInt32(BoxPos[0, i + 1]), Convert.ToInt32(BoxPos[1, i + 1]));
                    }
                    if (walluqnt != 0)
                    {
                        for (int u = 0; u < walluqnt; u += 2)
                            graph.DrawLine(pen, Convert.ToInt32(WallPos[0, u]), Convert.ToInt32(WallPos[1, u]), Convert.ToInt32(WallPos[0, u + 1]), Convert.ToInt32(WallPos[1, u + 1]));
                    }
                    pen.Width = 1;
                    for (int j = 0; j < uglqunt; j++)
                    {
                        graph.DrawRectangle(pen, Convert.ToInt32(BoxPos[0, j]) - 6, Convert.ToInt32(BoxPos[1, j]) - 6, 12, 12);
                    }
                    if (walluqnt != 0)
                    {
                        for (int i = 0; i < walluqnt; i++)
                        {
                            graph.DrawRectangle(pen, Convert.ToInt32(WallPos[0, i] - 6), Convert.ToInt32(WallPos[1, i] - 6), 12, 12);
                        }
                    }
                    pen.Width = 2;
                    if (textBox11.Text != "")
                    {
                        double radius = Convert.ToDouble(textBox11.Text);
                        for (int i = 0; i < beaconqunt; i++)
                        {
                            graph.DrawEllipse(pen, Convert.ToSingle(SatPos[0, i] - (radius * pxX)), Convert.ToSingle(SatPos[1, i] - (radius * pxY)), Convert.ToSingle(radius * Convert.ToDouble(pxX)) * 2, Convert.ToSingle(radius * Convert.ToDouble(pxY)) * 2);
                        }
                    }
                    pen.Width = 1;
                 //   int xM = Convert.ToInt32(SatPos[0, beaconqunt - 1]);
                 //   int yM = Convert.ToInt32(SatPos[1, beaconqunt - 1]);
                 //   Brush = new SolidBrush(Color.White);
                 //   graph.FillEllipse(Brush, xM - 6, yM - 6, 12, 12);
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
                if (beaconnumbers >= beaconqunt && uglnumbers >= uglqunt && wallnumbers>=walluqnt)
                {
                    x = e.Location.X;
                    y = e.Location.Y;
                    for (int j = 0; j < beaconqunt; j++)
                    {
                        if (x > (SatPos[0, j] - 8) && x < (SatPos[0, j] + 8) && y < (SatPos[1, j] + 8) && y > (SatPos[1, j] - 8))//Проверка тычка в область маяка
                        {
                            beaconflag = j;
                            beacontraf = new Rectangle((Convert.ToInt32(SatPos[0, j]) - 8), (Convert.ToInt32(SatPos[1, j]) - 8), 16, 16);
                            IsClicked = true;
                            for (int k = 0; k < beaconqunt; k++)
                            {
                                labelbeacon[k].Dispose();
                            }
                            for (int k = 0; k < uglqunt; k++)
                            {
                                labelbox[k].Dispose();
                            }
                            button5.Visible = false;
                            button6.Visible = false;
                            button7.Visible = false;
                            button8.Visible = false;
                            textBox7.Visible = false;
                            label7.Visible = false;
                            form.progressBar1.Value = 0;
                            deltax = e.X - beacontraf.X;
                            deltay = e.Y - beacontraf.Y;
                            break;
                        }
                    }
                }
            }
            //Комната
            if (e.Button == MouseButtons.Left)
            {
                if (uglnumbers >= uglqunt && beaconnumbers >= beaconqunt && wallnumbers >= walluqnt)
                {
                    x = e.Location.X;
                    y = e.Location.Y;
                    for (int j = 0; j < uglqunt; j++)
                    {
                        if (x > (BoxPos[0, j] - 6) && x < (BoxPos[0, j] + 6) && y < (BoxPos[1, j] + 6) && y > (BoxPos[1, j] - 6))//Проверка тучка в область маяка
                        {
                            boxflag = j;
                            boxtraf = new Rectangle((Convert.ToInt32(BoxPos[0, j]) - 6), (Convert.ToInt32(BoxPos[1, j]) - 6), 12, 12);
                            IsClicked2 = true;
                            for (int k = 0; k < beaconqunt; k++)
                            {
                                labelbeacon[k].Dispose();
                            }
                            for (int k = 0; k < uglqunt; k++)
                            {
                                labelbox[k].Dispose();
                            }
                            button5.Visible = false;
                            button6.Visible = false;
                            button7.Visible = false;
                            button8.Visible = false;
                            textBox7.Visible = false;
                            label7.Visible = false;
                            form.progressBar1.Value = 0;
                            deltax = e.X - boxtraf.X;
                            deltay = e.Y - boxtraf.Y;
                            break;
                        }
                    }
                }
            }
            if (e.Button == MouseButtons.Left)//Wall
            {
                if (uglnumbers >= uglqunt && beaconnumbers >= beaconqunt && wallnumbers >= walluqnt)
                {
                    x = e.Location.X;
                    y = e.Location.Y;
                    for (int i = 0; i < walluqnt; i++)
                    {
                        if (x > WallPos[0, i] - 6 && x < WallPos[0, i] + 6 && y < WallPos[1, i] + 6 && y > WallPos[1, i] - 6)
                        {
                            wallreplace.Add(i);
                            wallflag = i;
                            boxtraf = new Rectangle((Convert.ToInt32(WallPos[0, i]) - 6), (Convert.ToInt32(WallPos[1, i]) - 6), 12, 12);
                            IsClicked3 = true;
                            button5.Visible = false;
                            button6.Visible = false;
                            button7.Visible = false;
                            button8.Visible = false;
                            textBox7.Visible = false;
                            label7.Visible = false;
                            form.progressBar1.Value = 0;
                            deltax = e.X - boxtraf.X;
                            deltay = e.Y - boxtraf.Y;
                        }
                    }
                }
            }
            if (IsClicked3 == true)
            {
                for (int i = 0; i < walluqnt; i++)
                {
                    foreach(int p in wallreplace)
                    {
                        if (i == p)
                            inlist = true;
                    }
                    if (inlist == false)
                    {
                        wallin.Add(i);
                        schet += 1;
                    }
                    else
                        inlist = false;
                }
                BlockClone = new double[2, schet];
                int z = 0;
                foreach (int p in wallin)
                {
                    BlockClone[0, z] = WallPos[0, p];
                    BlockClone[1, z] = WallPos[1, p];
                    z += 1;
                }
            }

            if (e.Button == MouseButtons.Left)
            {
                if (beaconnumbers >= beaconqunt && uglnumbers >= uglqunt && IsClicked == false && IsClicked2 == false && wallnumbers >= walluqnt && IsClicked3 == false)
                {
                    IsInfo = true;
                    labelInfo = new Label();
                    labelInfo.Location = new Point(e.Location.X, e.Location.Y - 10);
                    labelInfo.ForeColor = Color.Black;
                    labelInfo.Text = (0).ToString();
                    labelInfo.Size = new Size(50, 12);
                    labelInfo.BackColor = this.label1.Parent.BackColor;
                    labelInfo.Parent = this.pictureBox1;
                    labelInfo.BackColor = Color.Transparent;
                    StartInfoX = e.Location.X;
                    StartInfoY = e.Location.Y;
                    if (textBox7.Visible == false)
                    {
                        bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                        graph = Graphics.FromImage(bmp);
                        pen = new Pen(Color.Black);
                        pictureBox1.Image = bmp;
                    }
                    else
                        pictureBox1.Image = bmp;
                }
            }         
        }
        private void pictureBox1_MouseMove_1(object sender, MouseEventArgs e)//Отслеживание движения мыши
        {
            if (textBox7.Visible == true && e.Location.X>=0 && e.Location.X< pictureBox1.Width && e.Location.Y>=0 && e.Location.Y< pictureBox1.Height)
                textBox7.Text = Z[e.Location.X, e.Location.Y].ToString();
            if (textBox7.Visible == true && e.Location.X >= 0 && e.Location.X < pictureBox1.Width && e.Location.Y >= 0 && e.Location.Y < pictureBox1.Height)
            {
                string delt = textBox8.Text;
                if (delt != "")
                {
                    double n;
                    if (double.TryParse(textBox8.Text, out n))
                    {
                            double deltainfo = Convert.ToDouble(delt);
                        textBox9.Text = (deltainfo * Math.Sqrt(DeltaX[e.Location.X, e.Location.Y])).ToString();
                        textBox10.Text = (deltainfo * Math.Sqrt(DeltaY[e.Location.X, e.Location.Y])).ToString();
                    }
                }
            }

            if (IsInfo == true) 
            {
                if (textBox7.Visible == true)
                {
                    labelInfo.Text = Math.Sqrt(Math.Pow((Math.Abs((Convert.ToDouble(e.Location.X - StartInfoX)) / Convert.ToDouble(pxX))), 2) + Math.Pow((Math.Abs(Convert.ToDouble(e.Location.Y - StartInfoY) / Convert.ToDouble(pxY))), 2)).ToString();
                }
                else
                {
                    if (e.Location.X < 0 || e.Location.Y < 0 || e.Location.X > pictureBox1.Width || e.Location.Y > pictureBox1.Height)
                    {
                        labelInfo.Dispose();
                        IsInfo = false;

                        bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                        graph = Graphics.FromImage(bmp);
                        pen = new Pen(Color.Black);
                        pictureBox1.Image = bmp;

                        Drawing();
                        roompaint();

                        for (int j = 0; j < beaconqunt; j++)
                        {
                            graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                        }
                        if (textBox11.Text != "")
                        {
                            double n;
                            if (double.TryParse(textBox11.Text, out n))
                            {
                                double radius = Convert.ToDouble(textBox11.Text);
                                for (int i = 0; i < beaconqunt; i++)
                                {
                                    graph.DrawEllipse(pen, Convert.ToSingle(SatPos[0, i] - (radius * pxX)), Convert.ToSingle(SatPos[1, i] - (radius * pxY)), Convert.ToSingle(radius * Convert.ToDouble(pxX)) * 2, Convert.ToSingle(radius * Convert.ToDouble(pxY)) * 2);
                                }
                            }
                        }

                    }
                    else
                    {
                        labelInfo.Text = Math.Sqrt(Math.Pow((Math.Abs((Convert.ToDouble(e.Location.X - StartInfoX)) / Convert.ToDouble(pxX))), 2) + Math.Pow((Math.Abs(Convert.ToDouble(e.Location.Y - StartInfoY) / Convert.ToDouble(pxY))), 2)).ToString();
                        Drawing();
                        roompaint();
                        for (int j = 0; j < beaconqunt; j++)
                        {
                            graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                        }
                        if (textBox11.Text != "")
                        {
                            double n;
                            if (double.TryParse(textBox11.Text, out n))
                            {
                                double radius = Convert.ToDouble(textBox11.Text);
                                for (int i = 0; i < beaconqunt; i++)
                                {
                                    graph.DrawEllipse(pen, Convert.ToSingle(SatPos[0, i] - (radius * pxX)), Convert.ToSingle(SatPos[1, i] - (radius * pxY)), Convert.ToSingle(radius * Convert.ToDouble(pxX)) * 2, Convert.ToSingle(radius * Convert.ToDouble(pxY)) * 2);
                                }
                            }
                        }
                        pen = new Pen(Color.Red);
                        graph.DrawLine(pen, Convert.ToSingle(StartInfoX), Convert.ToSingle(StartInfoY), e.Location.X, e.Location.Y);
                        pen = new Pen(Color.Black);
                    }
                }
            }
                //Маяки
                if (beaconnumbers >= beaconqunt)
            {
                if (IsClicked == true && IsClicked2 == false && IsClicked3 == false )
                {
                    beacontraf.X = e.X - deltax;
                    beacontraf.Y = e.Y - deltay;
                    if (beacontraf.X < 0)
                    {
                        beacontraf.X = 0;
                    }
                    if (beacontraf.X > pictureBox1.Width-16)
                    {
                        beacontraf.X = pictureBox1.Width - 16;
                    }
                    if (beacontraf.Y < 0)
                    {
                        beacontraf.Y = 0;
                    }
                    if (beacontraf.Y > pictureBox1.Height - 16)
                    {
                        beacontraf.Y = pictureBox1.Height - 16;
                    }
                    for (int i = 0; i < uglqunt; i++)
                    {
                        if (beacontraf.X > (BoxPos[0, i] - 10) && beacontraf.X < (BoxPos[0, i] + 10) && beacontraf.Y < (BoxPos[1, i] + 10) && beacontraf.Y > (BoxPos[1, i] - 10))
                        {
                            if (beacontraf.Y < (BoxPos[1, i] + 10))
                            {
                                beacontraf.Y = Convert.ToInt32(BoxPos[1, i] + 4);
                            }
                            if (beacontraf.X < (BoxPos[0, i] + 10))
                            {
                                beacontraf.X = Convert.ToInt32(BoxPos[0, i] + 4);
                            }
                        }
                        if (beacontraf.X + 16 > (BoxPos[0, i] - 10) && beacontraf.X + 16 < (BoxPos[0, i] + 10) && beacontraf.Y < (BoxPos[1, i] + 10) && beacontraf.Y > (BoxPos[1, i] - 10))
                        {
                            if (beacontraf.Y < (BoxPos[1, i] + 10))
                            {
                                beacontraf.Y = Convert.ToInt32(BoxPos[1, i] + 8);
                            }
                            if (beacontraf.X + 16 > (BoxPos[0, i] - 10))
                            {
                                beacontraf.X = Convert.ToInt32(BoxPos[0, i] - 16);
                            }
                        }
                        if (beacontraf.X > (BoxPos[0, i] - 10) && beacontraf.X < (BoxPos[0, i] + 10) && beacontraf.Y + 16 < (BoxPos[1, i] + 10) && beacontraf.Y + 16 > (BoxPos[1, i] - 10))
                        {
                            if (beacontraf.Y + 16 > (BoxPos[1, i] - 10))
                            {
                                beacontraf.Y = Convert.ToInt32(BoxPos[1, i] - 16);
                            }
                            if (beacontraf.X < (BoxPos[0, i] + 10))
                            {
                                beacontraf.X = Convert.ToInt32(BoxPos[0, i] + 8);
                            }
                        }
                        if (beacontraf.X + 16 > (BoxPos[0, i] - 10) && beacontraf.X + 16 < (BoxPos[0, i] + 10) && beacontraf.Y + 16 < (BoxPos[1, i] + 10) && beacontraf.Y + 16 > (BoxPos[1, i] - 10))
                        {
                            if (beacontraf.Y + 16 > (BoxPos[1, i] - 10))
                            {
                                beacontraf.Y = Convert.ToInt32(BoxPos[1, i] - 20);
                            }
                            if (beacontraf.X + 16 > (BoxPos[0, i] - 10))
                            {
                                beacontraf.X = Convert.ToInt32(BoxPos[0, i] - 20);
                            }
                        }
                    }
                    for (int i = 0; i < walluqnt; i++)
                    {
                        if (beacontraf.X > (WallPos[0, i] - 12) && beacontraf.X < (WallPos[0, i] + 12) && beacontraf.Y < (WallPos[1, i] + 12) && beacontraf.Y > (WallPos[1, i] - 12))
                        {
                            if (beacontraf.Y < (WallPos[1, i] + 12))
                            {
                                beacontraf.Y = Convert.ToInt32(WallPos[1, i] + 6);
                            }
                            if (beacontraf.X < (WallPos[0, i] + 12))
                            {
                                beacontraf.X = Convert.ToInt32(WallPos[0, i] + 6);
                            }
                        }
                        if (beacontraf.X + 12 > (WallPos[0, i] - 12) && beacontraf.X + 12 < (WallPos[0, i] + 12) && beacontraf.Y < (WallPos[1, i] + 12) && beacontraf.Y > (WallPos[1, i] - 12))
                        {
                            if (beacontraf.Y < (WallPos[1, i] + 12))
                            {
                                beacontraf.Y = Convert.ToInt32(WallPos[1, i] + 8);
                            }
                            if (beacontraf.X + 12 > (WallPos[0, i] - 12))
                            {
                                beacontraf.X = Convert.ToInt32(WallPos[0, i] - 16);
                            }
                        }
                        if (beacontraf.X > (WallPos[0, i] - 12) && beacontraf.X < (WallPos[0, i] + 12) && beacontraf.Y + 12 < (WallPos[1, i] + 12) && beacontraf.Y + 12 > (WallPos[1, i] - 12))
                        {
                            if (beacontraf.Y + 12 > (WallPos[1, i] - 12))
                            {
                                beacontraf.Y = Convert.ToInt32(WallPos[1, i] - 16);
                            }
                            if (beacontraf.X < (WallPos[0, i] + 12))
                            {
                                beacontraf.X = Convert.ToInt32(WallPos[0, i] + 8);
                            }
                        }
                        if (beacontraf.X + 12 > (WallPos[0, i] - 12) && beacontraf.X + 12 < (WallPos[0, i] + 12) && beacontraf.Y + 12 < (WallPos[1, i] + 12) && beacontraf.Y + 12 > (WallPos[1, i] - 12))
                        {
                            if (beacontraf.Y + 12 > (WallPos[1, i] - 12))
                            {
                                beacontraf.Y = Convert.ToInt32(WallPos[1, i] - 18);
                            }
                            if (beacontraf.X + 12 > (WallPos[0, i] - 12))
                            {
                                beacontraf.X = Convert.ToInt32(WallPos[0, i] - 18);
                            }
                        }
                    }
                    for (int i = 0; i < beaconflag; i++)
                    {
                        if (beacontraf.X > (SatPos[0, i] - 12) && beacontraf.X < (SatPos[0, i] + 12) && beacontraf.Y < (SatPos[1, i] + 12) && beacontraf.Y > (SatPos[1, i] - 12))
                        {
                            if (beacontraf.Y < (SatPos[1, i] + 12))
                            {
                                beacontraf.Y = Convert.ToInt32(SatPos[1, i] + 4);
                            }
                            if (beacontraf.X < (SatPos[0, i] + 12))
                            {
                                beacontraf.X = Convert.ToInt32(SatPos[0, i] + 4);
                            }
                        }
                        if (beacontraf.X + 12 > (SatPos[0, i] - 12) && beacontraf.X + 12 < (SatPos[0, i] + 12) && beacontraf.Y < (SatPos[1, i] + 12) && beacontraf.Y > (SatPos[1, i] - 12))
                        {
                            if (beacontraf.Y < (SatPos[1, i] + 12))
                            {
                                beacontraf.Y = Convert.ToInt32(SatPos[1, i] + 8);
                            }
                            if (beacontraf.X + 12 > (SatPos[0, i] - 12))
                            {
                                beacontraf.X = Convert.ToInt32(SatPos[0, i] - 16);
                            }
                        }
                        if (beacontraf.X > (SatPos[0, i] - 12) && beacontraf.X < (SatPos[0, i] + 12) && beacontraf.Y + 12 < (SatPos[1, i] + 12) && beacontraf.Y + 12 > (SatPos[1, i] - 12))
                        {
                            if (beacontraf.Y + 12 > (SatPos[1, i] - 12))
                            {
                                beacontraf.Y = Convert.ToInt32(SatPos[1, i] - 16);
                            }
                            if (beacontraf.X < (SatPos[0, i] + 12))
                            {
                                beacontraf.X = Convert.ToInt32(SatPos[0, i] + 8);
                            }
                        }
                        if (beacontraf.X + 12 > (SatPos[0, i] - 12) && beacontraf.X + 12 < (SatPos[0, i] + 12) && beacontraf.Y + 12 < (SatPos[1, i] + 12) && beacontraf.Y + 12 > (SatPos[1, i] - 12))
                        {
                            if (beacontraf.Y + 12 > (SatPos[1, i] - 12))
                            {
                                beacontraf.Y = Convert.ToInt32(SatPos[1, i] - 20);
                            }
                            if (beacontraf.X + 12 > (SatPos[0, i] - 12))
                            {
                                beacontraf.X = Convert.ToInt32(SatPos[0, i] - 20);
                            }
                        }
                    }
                    for (int i = beaconflag + 1; i < beaconqunt; i++)
                    {
                        if (beacontraf.X > (SatPos[0, i] - 12) && beacontraf.X < (SatPos[0, i] + 12) && beacontraf.Y < (SatPos[1, i] + 12) && beacontraf.Y > (SatPos[1, i] - 12))
                        {
                            if (beacontraf.Y < (SatPos[1, i] + 12))
                            {
                                beacontraf.Y = Convert.ToInt32(SatPos[1, i] + 4);
                            }
                            if (beacontraf.X < (SatPos[0, i] + 12))
                            {
                                beacontraf.X = Convert.ToInt32(SatPos[0, i] + 4);
                            }
                        }
                        if (beacontraf.X + 12 > (SatPos[0, i] - 12) && beacontraf.X + 12 < (SatPos[0, i] + 12) && beacontraf.Y < (SatPos[1, i] + 12) && beacontraf.Y > (SatPos[1, i] - 12))
                        {
                            if (beacontraf.Y < (SatPos[1, i] + 12))
                            {
                                beacontraf.Y = Convert.ToInt32(SatPos[1, i] + 8);
                            }
                            if (beacontraf.X + 12 > (SatPos[0, i] - 12))
                            {
                                beacontraf.X = Convert.ToInt32(SatPos[0, i] - 16);
                            }
                        }
                        if (beacontraf.X > (SatPos[0, i] - 12) && beacontraf.X < (SatPos[0, i] + 12) && beacontraf.Y + 12 < (SatPos[1, i] + 12) && beacontraf.Y + 12 > (SatPos[1, i] - 12))
                        {
                            if (beacontraf.Y + 12 > (SatPos[1, i] - 12))
                            {
                                beacontraf.Y = Convert.ToInt32(SatPos[1, i] - 16);
                            }
                            if (beacontraf.X < (SatPos[0, i] + 12))
                            {
                                beacontraf.X = Convert.ToInt32(SatPos[0, i] + 8);
                            }
                        }
                        if (beacontraf.X + 12 > (SatPos[0, i] - 12) && beacontraf.X + 12 < (SatPos[0, i] + 12) && beacontraf.Y + 12 < (SatPos[1, i] + 12) && beacontraf.Y + 12 > (SatPos[1, i] - 12))
                        {
                            if (beacontraf.Y + 12 > (SatPos[1, i] - 12))
                            {
                                beacontraf.Y = Convert.ToInt32(SatPos[1, i] - 20);
                            }
                            if (beacontraf.X + 12 > (SatPos[0, i] - 12))
                            {
                                beacontraf.X = Convert.ToInt32(SatPos[0, i] - 20);
                            }
                        }
                    }
                    pictureBox1.Invalidate();
                    SatPos[0, beaconflag] = beacontraf.X + 8;
                    SatPos[1, beaconflag] = beacontraf.Y + 8;
                    Drawing();
                    for (int j = 0; j < beaconqunt; j++)
                    {
                        graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                    }
                    roompaint();
                }
            }

            //Комната
            if (uglnumbers >= uglqunt)
            { 
                if (IsClicked2 == true && IsClicked == false && IsClicked3 == false)
                {
                    boxtraf.X = e.X - deltax;
                    boxtraf.Y = e.Y - deltay;
                    if (boxtraf.X < 0)
                    {
                        boxtraf.X = 0;
                    }
                    if (boxtraf.X > pictureBox1.Width - 12)
                    {
                        boxtraf.X = pictureBox1.Width - 12;
                    }
                    if (boxtraf.Y < 0)
                    {
                        boxtraf.Y = 0;
                    }
                    if (boxtraf.Y > pictureBox1.Height - 12)
                    {
                        boxtraf.Y = pictureBox1.Height - 12;
                    }
                    for (int i = 0; i < beaconqunt; i++)
                    {
                        if (boxtraf.X > (SatPos[0, i] - 12) && boxtraf.X < (SatPos[0, i] + 12) && boxtraf.Y < (SatPos[1, i] + 12) && boxtraf.Y > (SatPos[1, i] - 12))
                        {
                            if (boxtraf.Y < (SatPos[1, i] + 12))
                            {
                                boxtraf.Y = Convert.ToInt32(SatPos[1, i] + 6);
                            }
                            if (boxtraf.X < (SatPos[0, i] + 12))
                            {
                                boxtraf.X = Convert.ToInt32(SatPos[0, i] + 6);
                            }
                        }
                        if (boxtraf.X + 12 > (SatPos[0, i] - 12) && boxtraf.X + 12 < (SatPos[0, i] + 12) && boxtraf.Y < (SatPos[1, i] + 12) && boxtraf.Y > (SatPos[1, i] - 12))
                        {
                            if (boxtraf.Y < (SatPos[1, i] + 12))
                            {
                                boxtraf.Y = Convert.ToInt32(SatPos[1, i] + 8);
                            }
                            if (boxtraf.X + 12 > (SatPos[0, i] - 12))
                            {
                                boxtraf.X = Convert.ToInt32(SatPos[0, i] - 16);
                            }
                        }
                        if (boxtraf.X > (SatPos[0, i] - 12) && boxtraf.X < (SatPos[0, i] + 12) && boxtraf.Y + 12 < (SatPos[1, i] + 12) && boxtraf.Y + 12 > (SatPos[1, i] - 12))
                        {
                            if (boxtraf.Y + 12 > (SatPos[1, i] - 12))
                            {
                                boxtraf.Y = Convert.ToInt32(SatPos[1, i] - 16);
                            }
                            if (boxtraf.X < (SatPos[0, i] + 12))
                            {
                                boxtraf.X = Convert.ToInt32(SatPos[0, i] + 8);
                            }
                        }
                        if (boxtraf.X + 12 > (SatPos[0, i] - 12) && boxtraf.X + 12 < (SatPos[0, i] + 12) && boxtraf.Y + 12 < (SatPos[1, i] + 12) && boxtraf.Y + 12 > (SatPos[1, i] - 12))
                        {
                            if (boxtraf.Y + 12 > (SatPos[1, i] - 12))
                            {
                                boxtraf.Y = Convert.ToInt32(SatPos[1, i] - 18);
                            }
                            if (boxtraf.X + 12 > (SatPos[0, i] - 12))
                            {
                                boxtraf.X = Convert.ToInt32(SatPos[0, i] - 18);
                            }
                        }
                    }
                    for (int i = 0; i < walluqnt; i++)
                    {
                        if (boxtraf.X > (WallPos[0, i] - 12) && boxtraf.X < (WallPos[0, i] + 12) && boxtraf.Y < (WallPos[1, i] + 12) && boxtraf.Y > (WallPos[1, i] - 12))
                        {
                            if (boxtraf.Y < (WallPos[1, i] + 12))
                            {
                                boxtraf.Y = Convert.ToInt32(WallPos[1, i] + 6);
                            }
                            if (boxtraf.X < (WallPos[0, i] + 12))
                            {
                                boxtraf.X = Convert.ToInt32(WallPos[0, i] + 6);
                            }
                        }
                        if (boxtraf.X + 12 > (WallPos[0, i] - 12) && boxtraf.X + 12 < (WallPos[0, i] + 12) && boxtraf.Y < (WallPos[1, i] + 12) && boxtraf.Y > (WallPos[1, i] - 12))
                        {
                            if (boxtraf.Y < (WallPos[1, i] + 12))
                            {
                                boxtraf.Y = Convert.ToInt32(WallPos[1, i] + 8);
                            }
                            if (boxtraf.X + 12 > (WallPos[0, i] - 12))
                            {
                                boxtraf.X = Convert.ToInt32(WallPos[0, i] - 16);
                            }
                        }
                        if (boxtraf.X > (WallPos[0, i] - 12) && boxtraf.X < (WallPos[0, i] + 12) && boxtraf.Y + 12 < (WallPos[1, i] + 12) && boxtraf.Y + 12 > (WallPos[1, i] - 12))
                        {
                            if (boxtraf.Y + 12 > (WallPos[1, i] - 12))
                            {
                                boxtraf.Y = Convert.ToInt32(WallPos[1, i] - 16);
                            }
                            if (boxtraf.X < (WallPos[0, i] + 12))
                            {
                                boxtraf.X = Convert.ToInt32(WallPos[0, i] + 8);
                            }
                        }
                        if (boxtraf.X + 12 > (WallPos[0, i] - 12) && boxtraf.X + 12 < (WallPos[0, i] + 12) && boxtraf.Y + 12 < (WallPos[1, i] + 12) && boxtraf.Y + 12 > (WallPos[1, i] - 12))
                        {
                            if (boxtraf.Y + 12 > (WallPos[1, i] - 12))
                            {
                                boxtraf.Y = Convert.ToInt32(WallPos[1, i] - 18);
                            }
                            if (boxtraf.X + 12 > (WallPos[0, i] - 12))
                            {
                                boxtraf.X = Convert.ToInt32(WallPos[0, i] - 18);
                            }
                        }
                    }
                    for (int i = 0; i < boxflag; i++)
                    {
                        if (boxtraf.X > (BoxPos[0, i] - 8) && boxtraf.X < (BoxPos[0, i] + 8) && boxtraf.Y < (BoxPos[1, i] + 8) && boxtraf.Y > (BoxPos[1, i] - 8))
                        {
                            if (boxtraf.Y < (BoxPos[1, i] + 8))
                            {
                                boxtraf.Y = Convert.ToInt32(BoxPos[1, i] + 6);
                            }
                            if (boxtraf.X < (BoxPos[0, i] + 8))
                            {
                                boxtraf.X = Convert.ToInt32(BoxPos[0, i] + 6);
                            }
                        }
                        if (boxtraf.X + 12 > (BoxPos[0, i] - 8) && boxtraf.X + 12 < (BoxPos[0, i] + 8) && boxtraf.Y < (BoxPos[1, i] + 8) && boxtraf.Y > (BoxPos[1, i] - 8))
                        {
                            if (boxtraf.Y < (BoxPos[1, i] + 8))
                            {
                                boxtraf.Y = Convert.ToInt32(BoxPos[1, i] + 8);
                            }
                            if (boxtraf.X + 12 > (BoxPos[0, i] - 8))
                            {
                                boxtraf.X = Convert.ToInt32(BoxPos[0, i] - 16);
                            }
                        }
                        if (boxtraf.X > (BoxPos[0, i] - 8) && boxtraf.X < (BoxPos[0, i] + 8) && boxtraf.Y + 12 < (BoxPos[1, i] + 8) && boxtraf.Y + 12 > (BoxPos[1, i] - 8))
                        {
                            if (boxtraf.Y + 12 > (BoxPos[1, i] - 8))
                            {
                                boxtraf.Y = Convert.ToInt32(BoxPos[1, i] - 16);
                            }
                            if (boxtraf.X < (BoxPos[0, i] + 8))
                            {
                                boxtraf.X = Convert.ToInt32(BoxPos[0, i] + 8);
                            }
                        }
                        if (boxtraf.X + 12 > (BoxPos[0, i] - 8) && boxtraf.X + 12 < (BoxPos[0, i] + 8) && boxtraf.Y + 12 < (BoxPos[1, i] + 8) && boxtraf.Y + 12 > (BoxPos[1, i] - 8))
                        {
                            if (boxtraf.Y + 12 > (BoxPos[1, i] - 8))
                            {
                                boxtraf.Y = Convert.ToInt32(BoxPos[1, i] - 18);
                            }
                            if (boxtraf.X + 12 > (BoxPos[0, i] - 8))
                            {
                                boxtraf.X = Convert.ToInt32(BoxPos[0, i] - 18);
                            }
                        }
                    }
                    for (int i = boxflag + 1; i < uglqunt; i++)
                    {
                        if (boxtraf.X > (BoxPos[0, i] - 8) && boxtraf.X < (BoxPos[0, i] + 8) && boxtraf.Y < (BoxPos[1, i] + 8) && boxtraf.Y > (BoxPos[1, i] - 8))
                        {
                            if (boxtraf.Y < (BoxPos[1, i] + 8))
                            {
                                boxtraf.Y = Convert.ToInt32(BoxPos[1, i] + 6);
                            }
                            if (boxtraf.X < (BoxPos[0, i] + 8))
                            {
                                boxtraf.X = Convert.ToInt32(BoxPos[0, i] + 6);
                            }
                        }
                        if (boxtraf.X + 12 > (BoxPos[0, i] - 8) && boxtraf.X + 12 < (BoxPos[0, i] + 8) && boxtraf.Y < (BoxPos[1, i] + 8) && boxtraf.Y > (BoxPos[1, i] - 8))
                        {
                            if (boxtraf.Y < (BoxPos[1, i] + 8))
                            {
                                boxtraf.Y = Convert.ToInt32(BoxPos[1, i] + 8);
                            }
                            if (boxtraf.X + 12 > (BoxPos[0, i] - 8))
                            {
                                boxtraf.X = Convert.ToInt32(BoxPos[0, i] - 16);
                            }
                        }
                        if (boxtraf.X > (BoxPos[0, i] - 8) && boxtraf.X < (BoxPos[0, i] + 8) && boxtraf.Y + 12 < (BoxPos[1, i] + 8) && boxtraf.Y + 12 > (BoxPos[1, i] - 8))
                        {
                            if (boxtraf.Y + 12 > (BoxPos[1, i] - 8))
                            {
                                boxtraf.Y = Convert.ToInt32(BoxPos[1, i] - 16);
                            }
                            if (boxtraf.X < (BoxPos[0, i] + 8))
                            {
                                boxtraf.X = Convert.ToInt32(BoxPos[0, i] + 8);
                            }
                        }
                        if (boxtraf.X + 12 > (BoxPos[0, i] - 8) && boxtraf.X + 12 < (BoxPos[0, i] + 8) && boxtraf.Y + 12 < (BoxPos[1, i] + 8) && boxtraf.Y + 12 > (BoxPos[1, i] - 8))
                        {
                            if (boxtraf.Y + 12 > (BoxPos[1, i] - 8))
                            {
                                boxtraf.Y = Convert.ToInt32(BoxPos[1, i] - 18);
                            }
                            if (boxtraf.X + 12 > (BoxPos[0, i] - 8))
                            {
                                boxtraf.X = Convert.ToInt32(BoxPos[0, i] - 18);
                            }
                        }
                    }
                    pictureBox1.Invalidate();
                    BoxPos[0, boxflag] = boxtraf.X + 6;
                    BoxPos[1, boxflag] = boxtraf.Y + 6;
                    Drawing();
                    for (int j = 0; j < beaconqunt; j++)
                    {
                        graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                    }
                    roompaint();
                }
            }

            if (wallnumbers >= walluqnt)
            {
                if (IsClicked3 == true && IsClicked == false && IsClicked2 == false)
                {
                    boxtraf.X = e.X - deltax;
                    boxtraf.Y = e.Y - deltay;
                    if (boxtraf.X < 0)
                    {
                        boxtraf.X = 0;
                    }
                    if (boxtraf.X > pictureBox1.Width - 12)
                    {
                        boxtraf.X = pictureBox1.Width - 12;
                    }
                    if (boxtraf.Y < 0)
                    {
                        boxtraf.Y = 0;
                    }
                    if (boxtraf.Y > pictureBox1.Height - 12)
                    {
                        boxtraf.Y = pictureBox1.Height - 12;
                    }
                    for (int i = 0; i < beaconqunt; i++)
                    {
                        if (boxtraf.X > (SatPos[0, i] - 12) && boxtraf.X < (SatPos[0, i] + 12) && boxtraf.Y < (SatPos[1, i] + 12) && boxtraf.Y > (SatPos[1, i] - 12))
                        {
                            if (boxtraf.Y < (SatPos[1, i] + 12))
                            {
                                boxtraf.Y = Convert.ToInt32(SatPos[1, i] + 6);
                            }
                            if (boxtraf.X < (SatPos[0, i] + 12))
                            {
                                boxtraf.X = Convert.ToInt32(SatPos[0, i] + 6);
                            }
                        }
                        if (boxtraf.X + 12 > (SatPos[0, i] - 12) && boxtraf.X + 12 < (SatPos[0, i] + 12) && boxtraf.Y < (SatPos[1, i] + 12) && boxtraf.Y > (SatPos[1, i] - 12))
                        {
                            if (boxtraf.Y < (SatPos[1, i] + 12))
                            {
                                boxtraf.Y = Convert.ToInt32(SatPos[1, i] + 8);
                            }
                            if (boxtraf.X + 12 > (SatPos[0, i] - 12))
                            {
                                boxtraf.X = Convert.ToInt32(SatPos[0, i] - 16);
                            }
                        }
                        if (boxtraf.X > (SatPos[0, i] - 12) && boxtraf.X < (SatPos[0, i] + 12) && boxtraf.Y + 12 < (SatPos[1, i] + 12) && boxtraf.Y + 12 > (SatPos[1, i] - 12))
                        {
                            if (boxtraf.Y + 12 > (SatPos[1, i] - 12))
                            {
                                boxtraf.Y = Convert.ToInt32(SatPos[1, i] - 16);
                            }
                            if (boxtraf.X < (SatPos[0, i] + 12))
                            {
                                boxtraf.X = Convert.ToInt32(SatPos[0, i] + 8);
                            }
                        }
                        if (boxtraf.X + 12 > (SatPos[0, i] - 12) && boxtraf.X + 12 < (SatPos[0, i] + 12) && boxtraf.Y + 12 < (SatPos[1, i] + 12) && boxtraf.Y + 12 > (SatPos[1, i] - 12))
                        {
                            if (boxtraf.Y + 12 > (SatPos[1, i] - 12))
                            {
                                boxtraf.Y = Convert.ToInt32(SatPos[1, i] - 18);
                            }
                            if (boxtraf.X + 12 > (SatPos[0, i] - 12))
                            {
                                boxtraf.X = Convert.ToInt32(SatPos[0, i] - 18);
                            }
                        }
                    }
                    for (int i = 0; i < uglqunt; i++)
                    {
                        if (boxtraf.X > (BoxPos[0, i] - 10) && boxtraf.X < (BoxPos[0, i] + 10) && boxtraf.Y < (BoxPos[1, i] + 10) && boxtraf.Y > (BoxPos[1, i] - 10))
                        {
                            if (boxtraf.Y < (BoxPos[1, i] + 10))
                            {
                                boxtraf.Y = Convert.ToInt32(BoxPos[1, i] + 4);
                            }
                            if (boxtraf.X < (BoxPos[0, i] + 10))
                            {
                                boxtraf.X = Convert.ToInt32(BoxPos[0, i] + 4);
                            }
                        }
                        if (boxtraf.X + 16 > (BoxPos[0, i] - 10) && boxtraf.X + 16 < (BoxPos[0, i] + 10) && boxtraf.Y < (BoxPos[1, i] + 10) && boxtraf.Y > (BoxPos[1, i] - 10))
                        {
                            if (boxtraf.Y < (BoxPos[1, i] + 10))
                            {
                                boxtraf.Y = Convert.ToInt32(BoxPos[1, i] + 8);
                            }
                            if (boxtraf.X + 16 > (BoxPos[0, i] - 10))
                            {
                                boxtraf.X = Convert.ToInt32(BoxPos[0, i] - 16);
                            }
                        }
                        if (boxtraf.X > (BoxPos[0, i] - 10) && boxtraf.X < (BoxPos[0, i] + 10) && boxtraf.Y + 16 < (BoxPos[1, i] + 10) && boxtraf.Y + 16 > (BoxPos[1, i] - 10))
                        {
                            if (boxtraf.Y + 16 > (BoxPos[1, i] - 10))
                            {
                                boxtraf.Y = Convert.ToInt32(BoxPos[1, i] - 16);
                            }
                            if (boxtraf.X < (BoxPos[0, i] + 10))
                            {
                                boxtraf.X = Convert.ToInt32(BoxPos[0, i] + 8);
                            }
                        }
                        if (boxtraf.X + 16 > (BoxPos[0, i] - 10) && boxtraf.X + 16 < (BoxPos[0, i] + 10) && boxtraf.Y + 16 < (BoxPos[1, i] + 10) && boxtraf.Y + 16 > (BoxPos[1, i] - 10))
                        {
                            if (boxtraf.Y + 16 > (BoxPos[1, i] - 10))
                            {
                                boxtraf.Y = Convert.ToInt32(BoxPos[1, i] - 20);
                            }
                            if (boxtraf.X + 16 > (BoxPos[0, i] - 10))
                            {
                                boxtraf.X = Convert.ToInt32(BoxPos[0, i] - 20);
                            }
                        }
                    }

                    //wall
                    for (int i = 0; i < schet; i++)
                    {
                        if (boxtraf.X > (BlockClone[0, i] - 8) && boxtraf.X < (BlockClone[0, i] + 8) && boxtraf.Y < (BlockClone[1, i] + 8) && boxtraf.Y > (BlockClone[1, i] - 8))
                        {
                            if (boxtraf.Y < (BlockClone[1, i] + 8))
                            {
                                boxtraf.Y = Convert.ToInt32(BlockClone[1, i] + 6);
                            }
                            if (boxtraf.X < (BlockClone[0, i] + 8))
                            {
                                boxtraf.X = Convert.ToInt32(BlockClone[0, i] + 6);
                            }
                        }
                        if (boxtraf.X + 12 > (BlockClone[0, i] - 8) && boxtraf.X + 12 < (BlockClone[0, i] + 8) && boxtraf.Y < (BlockClone[1, i] + 8) && boxtraf.Y > (BlockClone[1, i] - 8))
                        {
                            if (boxtraf.Y < (BlockClone[1, i] + 8))
                            {
                                boxtraf.Y = Convert.ToInt32(BlockClone[1, i] + 8);
                            }
                            if (boxtraf.X + 12 > (BlockClone[0, i] - 8))
                            {
                                boxtraf.X = Convert.ToInt32(BlockClone[0, i] - 16);
                            }
                        }
                        if (boxtraf.X > (BlockClone[0, i] - 8) && boxtraf.X < (BlockClone[0, i] + 8) && boxtraf.Y + 12 < (BlockClone[1, i] + 8) && boxtraf.Y + 12 > (BlockClone[1, i] - 8))
                        {
                            if (boxtraf.Y + 12 > (BlockClone[1, i] - 8))
                            {
                                boxtraf.Y = Convert.ToInt32(BlockClone[1, i] - 16);
                            }
                            if (boxtraf.X < (BlockClone[0, i] + 8))
                            {
                                boxtraf.X = Convert.ToInt32(BlockClone[0, i] + 8);
                            }
                        }
                        if (boxtraf.X + 12 > (BlockClone[0, i] - 8) && boxtraf.X + 12 < (BlockClone[0, i] + 8) && boxtraf.Y + 12 < (BlockClone[1, i] + 8) && boxtraf.Y + 12 > (BlockClone[1, i] - 8))
                        {
                            if (boxtraf.Y + 12 > (BlockClone[1, i] - 8))
                            {
                                boxtraf.Y = Convert.ToInt32(BlockClone[1, i] - 18);
                            }
                            if (boxtraf.X + 12 > (BlockClone[0, i] - 8))
                            {
                                boxtraf.X = Convert.ToInt32(BlockClone[0, i] - 18);
                            }
                        }
                    }
                    pictureBox1.Invalidate();
                    foreach (int p in wallreplace)
                    {
                        WallPos[0, p] = boxtraf.X + 6;
                        WallPos[1, p] = boxtraf.Y + 6;
                    }
                    Drawing();
                    for (int j = 0; j < beaconqunt; j++)
                    {
                        graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                    }
                    roompaint();
                }
            }

            if (IsClicked == false && IsClicked2 == true && IsClicked3 == true)
            {
                boxtraf.X = e.X - deltax;
                boxtraf.Y = e.Y - deltay;
                if (boxtraf.X < 0)
                {
                    boxtraf.X = 0;
                }
                if (boxtraf.X > pictureBox1.Width - 12)
                {
                    boxtraf.X = pictureBox1.Width - 12;
                }
                if (boxtraf.Y < 0)
                {
                    boxtraf.Y = 0;
                }
                if (boxtraf.Y > pictureBox1.Height - 12)
                {
                    boxtraf.Y = pictureBox1.Height - 12;
                }
                for (int i = 0; i < beaconqunt; i++)
                {
                    if (boxtraf.X > (SatPos[0, i] - 12) && boxtraf.X < (SatPos[0, i] + 12) && boxtraf.Y < (SatPos[1, i] + 12) && boxtraf.Y > (SatPos[1, i] - 12))
                    {
                        if (boxtraf.Y < (SatPos[1, i] + 12))
                        {
                            boxtraf.Y = Convert.ToInt32(SatPos[1, i] + 6);
                        }
                        if (boxtraf.X < (SatPos[0, i] + 12))
                        {
                            boxtraf.X = Convert.ToInt32(SatPos[0, i] + 6);
                        }
                    }
                    if (boxtraf.X + 12 > (SatPos[0, i] - 12) && boxtraf.X + 12 < (SatPos[0, i] + 12) && boxtraf.Y < (SatPos[1, i] + 12) && boxtraf.Y > (SatPos[1, i] - 12))
                    {
                        if (boxtraf.Y < (SatPos[1, i] + 12))
                        {
                            boxtraf.Y = Convert.ToInt32(SatPos[1, i] + 8);
                        }
                        if (boxtraf.X + 12 > (SatPos[0, i] - 12))
                        {
                            boxtraf.X = Convert.ToInt32(SatPos[0, i] - 16);
                        }
                    }
                    if (boxtraf.X > (SatPos[0, i] - 12) && boxtraf.X < (SatPos[0, i] + 12) && boxtraf.Y + 12 < (SatPos[1, i] + 12) && boxtraf.Y + 12 > (SatPos[1, i] - 12))
                    {
                        if (boxtraf.Y + 12 > (SatPos[1, i] - 12))
                        {
                            boxtraf.Y = Convert.ToInt32(SatPos[1, i] - 16);
                        }
                        if (boxtraf.X < (SatPos[0, i] + 12))
                        {
                            boxtraf.X = Convert.ToInt32(SatPos[0, i] + 8);
                        }
                    }
                    if (boxtraf.X + 12 > (SatPos[0, i] - 12) && boxtraf.X + 12 < (SatPos[0, i] + 12) && boxtraf.Y + 12 < (SatPos[1, i] + 12) && boxtraf.Y + 12 > (SatPos[1, i] - 12))
                    {
                        if (boxtraf.Y + 12 > (SatPos[1, i] - 12))
                        {
                            boxtraf.Y = Convert.ToInt32(SatPos[1, i] - 18);
                        }
                        if (boxtraf.X + 12 > (SatPos[0, i] - 12))
                        {
                            boxtraf.X = Convert.ToInt32(SatPos[0, i] - 18);
                        }
                    }
                }
                for (int i = 0; i < boxflag; i++)
                {
                    if (boxtraf.X > (BoxPos[0, i] - 8) && boxtraf.X < (BoxPos[0, i] + 8) && boxtraf.Y < (BoxPos[1, i] + 8) && boxtraf.Y > (BoxPos[1, i] - 8))
                    {
                        if (boxtraf.Y < (BoxPos[1, i] + 8))
                        {
                            boxtraf.Y = Convert.ToInt32(BoxPos[1, i] + 6);
                        }
                        if (boxtraf.X < (BoxPos[0, i] + 8))
                        {
                            boxtraf.X = Convert.ToInt32(BoxPos[0, i] + 6);
                        }
                    }
                    if (boxtraf.X + 12 > (BoxPos[0, i] - 8) && boxtraf.X + 12 < (BoxPos[0, i] + 8) && boxtraf.Y < (BoxPos[1, i] + 8) && boxtraf.Y > (BoxPos[1, i] - 8))
                    {
                        if (boxtraf.Y < (BoxPos[1, i] + 8))
                        {
                            boxtraf.Y = Convert.ToInt32(BoxPos[1, i] + 8);
                        }
                        if (boxtraf.X + 12 > (BoxPos[0, i] - 8))
                        {
                            boxtraf.X = Convert.ToInt32(BoxPos[0, i] - 16);
                        }
                    }
                    if (boxtraf.X > (BoxPos[0, i] - 8) && boxtraf.X < (BoxPos[0, i] + 8) && boxtraf.Y + 12 < (BoxPos[1, i] + 8) && boxtraf.Y + 12 > (BoxPos[1, i] - 8))
                    {
                        if (boxtraf.Y + 12 > (BoxPos[1, i] - 8))
                        {
                            boxtraf.Y = Convert.ToInt32(BoxPos[1, i] - 16);
                        }
                        if (boxtraf.X < (BoxPos[0, i] + 8))
                        {
                            boxtraf.X = Convert.ToInt32(BoxPos[0, i] + 8);
                        }
                    }
                    if (boxtraf.X + 12 > (BoxPos[0, i] - 8) && boxtraf.X + 12 < (BoxPos[0, i] + 8) && boxtraf.Y + 12 < (BoxPos[1, i] + 8) && boxtraf.Y + 12 > (BoxPos[1, i] - 8))
                    {
                        if (boxtraf.Y + 12 > (BoxPos[1, i] - 8))
                        {
                            boxtraf.Y = Convert.ToInt32(BoxPos[1, i] - 18);
                        }
                        if (boxtraf.X + 12 > (BoxPos[0, i] - 8))
                        {
                            boxtraf.X = Convert.ToInt32(BoxPos[0, i] - 18);
                        }
                    }
                }
                for (int i = boxflag + 1; i < uglqunt; i++)
                {
                    if (boxtraf.X > (BoxPos[0, i] - 8) && boxtraf.X < (BoxPos[0, i] + 8) && boxtraf.Y < (BoxPos[1, i] + 8) && boxtraf.Y > (BoxPos[1, i] - 8))
                    {
                        if (boxtraf.Y < (BoxPos[1, i] + 8))
                        {
                            boxtraf.Y = Convert.ToInt32(BoxPos[1, i] + 6);
                        }
                        if (boxtraf.X < (BoxPos[0, i] + 8))
                        {
                            boxtraf.X = Convert.ToInt32(BoxPos[0, i] + 6);
                        }
                    }
                    if (boxtraf.X + 12 > (BoxPos[0, i] - 8) && boxtraf.X + 12 < (BoxPos[0, i] + 8) && boxtraf.Y < (BoxPos[1, i] + 8) && boxtraf.Y > (BoxPos[1, i] - 8))
                    {
                        if (boxtraf.Y < (BoxPos[1, i] + 8))
                        {
                            boxtraf.Y = Convert.ToInt32(BoxPos[1, i] + 8);
                        }
                        if (boxtraf.X + 12 > (BoxPos[0, i] - 8))
                        {
                            boxtraf.X = Convert.ToInt32(BoxPos[0, i] - 16);
                        }
                    }
                    if (boxtraf.X > (BoxPos[0, i] - 8) && boxtraf.X < (BoxPos[0, i] + 8) && boxtraf.Y + 12 < (BoxPos[1, i] + 8) && boxtraf.Y + 12 > (BoxPos[1, i] - 8))
                    {
                        if (boxtraf.Y + 12 > (BoxPos[1, i] - 8))
                        {
                            boxtraf.Y = Convert.ToInt32(BoxPos[1, i] - 16);
                        }
                        if (boxtraf.X < (BoxPos[0, i] + 8))
                        {
                            boxtraf.X = Convert.ToInt32(BoxPos[0, i] + 8);
                        }
                    }
                    if (boxtraf.X + 12 > (BoxPos[0, i] - 8) && boxtraf.X + 12 < (BoxPos[0, i] + 8) && boxtraf.Y + 12 < (BoxPos[1, i] + 8) && boxtraf.Y + 12 > (BoxPos[1, i] - 8))
                    {
                        if (boxtraf.Y + 12 > (BoxPos[1, i] - 8))
                        {
                            boxtraf.Y = Convert.ToInt32(BoxPos[1, i] - 18);
                        }
                        if (boxtraf.X + 12 > (BoxPos[0, i] - 8))
                        {
                            boxtraf.X = Convert.ToInt32(BoxPos[0, i] - 18);
                        }
                    }
                }
                //wall
                for (int i = 0; i < schet; i++)
                {
                    if (boxtraf.X > (BlockClone[0, i] - 8) && boxtraf.X < (BlockClone[0, i] + 8) && boxtraf.Y < (BlockClone[1, i] + 8) && boxtraf.Y > (BlockClone[1, i] - 8))
                    {
                        if (boxtraf.Y < (BlockClone[1, i] + 8))
                        {
                            boxtraf.Y = Convert.ToInt32(BlockClone[1, i] + 6);
                        }
                        if (boxtraf.X < (BlockClone[0, i] + 8))
                        {
                            boxtraf.X = Convert.ToInt32(BlockClone[0, i] + 6);
                        }
                    }
                    if (boxtraf.X + 12 > (BlockClone[0, i] - 8) && boxtraf.X + 12 < (BlockClone[0, i] + 8) && boxtraf.Y < (BlockClone[1, i] + 8) && boxtraf.Y > (BlockClone[1, i] - 8))
                    {
                        if (boxtraf.Y < (BlockClone[1, i] + 8))
                        {
                            boxtraf.Y = Convert.ToInt32(BlockClone[1, i] + 8);
                        }
                        if (boxtraf.X + 12 > (BlockClone[0, i] - 8))
                        {
                            boxtraf.X = Convert.ToInt32(BlockClone[0, i] - 16);
                        }
                    }
                    if (boxtraf.X > (BlockClone[0, i] - 8) && boxtraf.X < (BlockClone[0, i] + 8) && boxtraf.Y + 12 < (BlockClone[1, i] + 8) && boxtraf.Y + 12 > (BlockClone[1, i] - 8))
                    {
                        if (boxtraf.Y + 12 > (BlockClone[1, i] - 8))
                        {
                            boxtraf.Y = Convert.ToInt32(BlockClone[1, i] - 16);
                        }
                        if (boxtraf.X < (BlockClone[0, i] + 8))
                        {
                            boxtraf.X = Convert.ToInt32(BlockClone[0, i] + 8);
                        }
                    }
                    if (boxtraf.X + 12 > (BlockClone[0, i] - 8) && boxtraf.X + 12 < (BlockClone[0, i] + 8) && boxtraf.Y + 12 < (BlockClone[1, i] + 8) && boxtraf.Y + 12 > (BlockClone[1, i] - 8))
                    {
                        if (boxtraf.Y + 12 > (BlockClone[1, i] - 8))
                        {
                            boxtraf.Y = Convert.ToInt32(BlockClone[1, i] - 18);
                        }
                        if (boxtraf.X + 12 > (BlockClone[0, i] - 8))
                        {
                            boxtraf.X = Convert.ToInt32(BlockClone[0, i] - 18);
                        }
                    }
                }          
                pictureBox1.Invalidate();
                BoxPos[0, boxflag] = boxtraf.X + 6;
                BoxPos[1, boxflag] = boxtraf.Y + 6;
                Drawing();
                for (int j = 0; j < beaconqunt; j++)
                {
                    graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                }
                foreach (int p in wallreplace)
                {
                    WallPos[0, p] = boxtraf.X + 6;
                    WallPos[1, p] = boxtraf.Y + 6;
                }
                roompaint();
            }
            if (addugl!=0)
            {
                double[] kord = new double[uglqunt - 1];
                for(int i=0;i<uglqunt-1;i++)
                {
                    kord[i] = Math.Sqrt(Math.Pow(Math.Abs(e.Location.X - BoxPos[0, i]), 2) + Math.Pow(Math.Abs(e.Location.Y - BoxPos[1, i]), 2));
                }
                double minkord = kord[0];
                minnum = 0;
                 for(int i=0;i<uglqunt-1;i++)
                {
                    if (kord[i] < minkord)
                    {
                        minkord = kord[i];
                        minnum = i;
                    }
                }
                if (minnum != 0 && minnum != uglqunt - 2)
                {
                    if (kord[minnum + 1] > kord[minnum - 1])
                        nextnum = minnum - 1;
                    else
                        nextnum = minnum + 1;
                }
                if(minnum ==0)
                {
                    if (kord[minnum + 1] > kord[uglqunt - 2])
                        nextnum = uglqunt - 2;
                    else
                        nextnum = minnum + 1;
                }
                if(minnum ==uglqunt-2)
                {
                    if (kord[uglqunt - 3] > kord[0])
                        nextnum = 0;
                    else
                        nextnum = uglqunt - 3;
                }

                bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                graph = Graphics.FromImage(bmp);
                pen = new Pen(Color.Black);
                pictureBox1.Image = bmp;

                Drawing();
                for (int j = 0; j < uglqunt-1; j++)
                {
                    graph.DrawRectangle(pen, Convert.ToInt32(BoxPos[0, j]) - 6, Convert.ToInt32(BoxPos[1, j]) - 6, 12, 12);
                }
                for (int i = 0; i < uglqunt - 2; i++)
                {
                    graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, i]), Convert.ToInt32(BoxPos[1, i]), Convert.ToInt32(BoxPos[0, i + 1]), Convert.ToInt32(BoxPos[1, i + 1]));
                }
                for (int j = 0; j < beaconqunt; j++)
                {
                    graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                }
                pen = new Pen(Color.Red);
                minway = Convert.ToInt32(Math.Sqrt(Math.Pow(Math.Abs(e.Location.X - BoxPos[0, minnum]), 2) + Math.Pow(Math.Abs(e.Location.Y - BoxPos[1, minnum]), 2)));
                nextway = Convert.ToInt32(Math.Sqrt(Math.Pow(Math.Abs(e.Location.X - BoxPos[0, nextnum]), 2) + Math.Pow(Math.Abs(e.Location.Y - BoxPos[1, nextnum]), 2)));
                if ((minnum == 0 && nextnum == uglqunt - 2) || (minnum == uglqunt-2 && nextnum == 0))
                {
                    
                    if ( minway > nextway)
                    {
                        graph.DrawLine(pen, Convert.ToSingle(e.Location.X), Convert.ToSingle(e.Location.Y), Convert.ToSingle(BoxPos[0, nextnum]), Convert.ToSingle(BoxPos[1, nextnum]));
                    }
                    else
                    {
                        graph.DrawLine(pen, Convert.ToSingle(e.Location.X), Convert.ToSingle(e.Location.Y), Convert.ToSingle(BoxPos[0, minnum]), Convert.ToSingle(BoxPos[1, minnum]));
                    }
                }
                else
                {
                    graph.DrawLine(pen, Convert.ToSingle(e.Location.X), Convert.ToSingle(e.Location.Y), Convert.ToSingle(BoxPos[0, minnum]), Convert.ToSingle(BoxPos[1, minnum]));
                    graph.DrawLine(pen, Convert.ToSingle(e.Location.X), Convert.ToSingle(e.Location.Y), Convert.ToSingle(BoxPos[0, nextnum]), Convert.ToSingle(BoxPos[1, nextnum]));
                }
                pen = new Pen(Color.Black);
            }
        }
        private void beacon()//Прорисовка новых маяков
        {
            SatPos[0, beaconflag] = beacontraf.X + 8;
            SatPos[1, beaconflag] = beacontraf.Y + 8;
            for (int kol = 0; kol < beaconqunt; kol++)
            {
                Point test = new Point() { X = Convert.ToInt32(SatPos[0, kol]), Y = Convert.ToInt32(SatPos[1, kol]) };
                beaconlist[kol] = test;
            }
            Drawing();
            for (int j = 0; j < beaconqunt; j++)
            {
                graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
            }
            roompaint();
            listBox1.Items.Clear();
            for (int i = 0; i < beaconqunt; i++)
            {
                listBox1.Items.Add((i + 1) + ")" + "X:" + Math.Round((Convert.ToDouble(SatPos[0, i])/Convert.ToDouble(pxX)),2) + "," + "Y:" + Math.Round((Convert.ToDouble(pictureBox1.Height - SatPos[1, i])/Convert.ToDouble(pxY)),2));
            }
            labalbeacon();
            labalbox();
            boxtraf = new Rectangle(1, 1, 1, 1);//Убираем с карты образ маяка, создавшийся при перетаскивании
            graph.DrawRectangle(pen, boxtraf);
            button2.PerformClick();
        }
        private void Box()
        {
            BoxPos[0, boxflag] = boxtraf.X + 6;
            BoxPos[1, boxflag] = boxtraf.Y + 6;
            for (int kol = 0; kol < uglqunt; kol++)
            {
                Point test = new Point() { X = Convert.ToInt32(BoxPos[0, kol]), Y = Convert.ToInt32(BoxPos[1, kol]) };
                boxlist[kol] = test;
            }
            Drawing();
            for (int j = 0; j < beaconqunt; j++)
            {
                graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
            }
            roompaint();
            listBox2.Items.Clear();
            for (int i = 0; i < uglqunt; i++)
            {
                listBox2.Items.Add((i + 1) + ")" + "X:" + Math.Round((Convert.ToDouble(BoxPos[0, i])/Convert.ToDouble(pxX)),2) + "," + "Y:" + Math.Round((Convert.ToDouble(pictureBox1.Height - BoxPos[1, i])/Convert.ToDouble(pxY)),2));
            }
            labalbeacon();
            labalbox();
            button2.PerformClick();
        }
        private void WallBox()
        {
            foreach (int p in wallreplace)
            {
                WallPos[0, p] = boxtraf.X + 6;
                WallPos[1, p] = boxtraf.Y + 6;
            }
            Drawing();
            for (int j = 0; j < beaconqunt; j++)
            {
                graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
            }
            roompaint();
            button2.PerformClick();
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)//Отслеживание отпуска ЛКМ
        {
            //Маяки
            if (beaconnumbers >= beaconqunt && IsClicked == true && IsClicked2 == false && IsClicked3 == false)
            {
                IsClicked = false;
                beacon();
            }
            //Комната
            if (uglnumbers >= uglqunt && IsClicked2 == true && IsClicked == false && IsClicked3 == false)
            {
                IsClicked2 = false;
                Box();
                boxtraf = new Rectangle(1, 1, 1, 1);//Убираем с карты образ маяка, создавшийся при перетаскивании
                graph.DrawRectangle(pen, boxtraf);
            }
            if (IsInfo == true)
            {
                labelInfo.Dispose();
                IsInfo = false;
                if (textBox7.Visible == false)
                {
                    bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    graph = Graphics.FromImage(bmp);
                    pen = new Pen(Color.Black);
                    pictureBox1.Image = bmp;

                    Drawing();
                    roompaint();

                    for (int j = 0; j < beaconqunt; j++)
                    {
                        graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                    }
                }
            }
            if (wallnumbers >= walluqnt && IsClicked3 == true && IsClicked2 == false && IsClicked == false)
            {
                IsClicked3 = false;
                WallBox();
                boxtraf = new Rectangle(1, 1, 1, 1);//Убираем с карты образ маяка, создавшийся при перетаскивании
                graph.DrawRectangle(pen, boxtraf);
                wallreplace.Clear();
                wallin.Clear();
            }
            if (wallnumbers >= walluqnt && IsClicked3 == true && IsClicked2 == true && IsClicked == false && uglnumbers >= uglqunt)
            {
                IsClicked3 = false;
                WallBox();
                IsClicked2 = false;
                Box();
                boxtraf = new Rectangle(1, 1, 1, 1);//Убираем с карты образ маяка, создавшийся при перетаскивании
                graph.DrawRectangle(pen, boxtraf);
                wallreplace.Clear();
                wallin.Clear();
            }
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)//Отрисока маяка при перетаскивании
        {
            if (IsClicked == true)
            {
                pen = new Pen(Color.Black);
                e.Graphics.DrawEllipse(pen, beacontraf);
            }
            if (IsClicked2 == true)
            {
                pen = new Pen(Color.Black);
                e.Graphics.DrawRectangle(pen, boxtraf);
            }
            if (IsClicked3 == true)
            {
                pen = new Pen(Color.Black);
                e.Graphics.DrawRectangle(pen, boxtraf);
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
                    beaconqunt = Convert.ToInt32(sum);
                    if (beaconqunt == 1 || beaconqunt <= 0)
                    {
                        MessageBox.Show("Install more beacons");
                    }
                    else
                    {
                        for (int k = 0; k < uglqunt; k++)
                        {
                            labelbox[k].Dispose();
                        }
                        pictureBox1.Enabled = true;
                        textBox1.Enabled = false;
                        SatPos = new double[2, beaconqunt + 1];
                        Grad = new double[beaconqunt + 1, 2];
                        Tran = new double[2, beaconqunt + 1];
                        Multi = new double[2, 2];
                        labelbeacon = new Label[beaconqunt + 1];
                        button22.Enabled = false;
                        button27.Enabled = false;
                        button1.Enabled = false;
                        button12.Enabled = false;
                    }
                }
                else
                    MessageBox.Show("Input correct number");
            }
        }
        private void button23_Click(object sender, EventArgs e)//add1 beacon
        {
            for (int k = 0; k < beaconqunt; k++)
            {
                labelbeacon[k].Dispose();
            }
            for (int k = 0; k < uglqunt; k++)
            {
                labelbox[k].Dispose();
            }

            textBox1.Text = (beaconqunt + 1).ToString();
            beaconqunt += 1;
            beaconnumbers = beaconqunt - 1;
            button23.Enabled = false;
            SatPos = new double[2, beaconqunt + 1];
            Grad = new double[beaconqunt + 1, 2];
            Tran = new double[2, beaconqunt + 1];
            Multi = new double[2, 2];
            labelbeacon = new Label[beaconqunt + 1];
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
            textBox7.Visible = false;
            label7.Visible = false;
            button14.Enabled = false;
        }
        private void button25_Click(object sender, EventArgs e)//add1 room
        {
            for (int k = 0; k < beaconqunt; k++)
            {
                labelbeacon[k].Dispose();
            }
            for (int k = 0; k < uglqunt; k++)
            {
                labelbox[k].Dispose();
            }

            textBox2.Text = (uglqunt + 1).ToString();
            uglqunt += 1;
            uglnumbers = uglqunt - 1;
            button25.Enabled = false;
            labelbox = new Label[uglqunt + 1];
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
            textBox7.Visible = false;
            label7.Visible = false;
            button14.Enabled = false;
            addugl = 1;
        }
        private void button24_Click(object sender, EventArgs e)//build
        {
            string ugl = textBox2.Text;
            if (ugl != "")
            {
                int n;
                if (int.TryParse(textBox2.Text, out n))
                {
                    uglqunt = Convert.ToInt32(ugl);
                    if (uglqunt > 2)
                    {
                        BoxPos = new double[2, uglqunt + 1];
                        labelbox = new Label[uglqunt + 1];
                        pictureBox1.Enabled = true;
                        button24.Enabled = false;
                        textBox2.Enabled = false;
                        button11.Enabled = false;
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
            for (int j = 0; j < beaconqunt; j++)
            {
                graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
            }
            roompaint();
            double[,] Grad;
            double[,] Z;
            double[,] DeltaX;
            double[,] DeltaY;
            double[,] Tran;
            double[,] Umn;
            button2.Enabled = false;
            form.progressBar1.Value = 0;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
            textBox7.Visible = false;
            label7.Visible = false;
        }
        private void button26_Click(object sender, EventArgs e)//clear beacons
        {
            if (uglqunt == 0)
            {
                for (int k = 0; k < beaconqunt; k++)
                {
                    labelbeacon[k].Dispose();
                }
                for (int k = 0; k < uglqunt; k++)
                {
                    labelbox[k].Dispose();
                }

                graph.Clear(Color.White);
                beaconnumbers = 0;
                uglnumbers = 0;
                beaconlist.Clear();
                boxlist.Clear();
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                textBox1.Text = "";
                textBox2.Text = "";
                Drawing();

                checkBox2.Checked = false;
                checkBox1.Checked = false;

                double[,] SatPos;
                SatPos = null;
                double[,] Grad;
                double[,] Z;
                double[,] DeltaX;
                double[,] DeltaY;
                double[,] Tran;
                double[,] Umn;
                double[,] BoxPos;

                beaconflag = 0;
                boxflag = 0;
                deltax = 0;
                deltay = 0;
                beacontraf = new Rectangle();
                boxtraf = new Rectangle();
                beaconqunt = 0;
                uglqunt = 0;
                addugl = 0;
                indclearroom = 0;

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
                button1.Enabled = true;
                pictureBox1.Enabled = false;
                button11.Enabled = true;
                button12.Enabled = false;
                textBox7.Visible = false;
                label7.Visible = false;
                button14.Enabled = false;
                double[,] Block;
                walluqnt = 0;
                wallnumbers = 0;
            }
            else
            {
                for (int k = 0; k < beaconqunt; k++)
                {
                    labelbeacon[k].Dispose();
                }
                beaconnumbers = 0;
                beaconlist.Clear();
                listBox1.Items.Clear();
                textBox1.Text = "";
                Drawing();
                roompaint();

                double[,] SatPos;
                SatPos = null;
                double[,] Grad;
                double[,] Z;
                double[,] DeltaX;
                double[,] DeltaY;
                double[,] Tran;
                double[,] Umn;

                beaconflag = 0;
                beacontraf = new Rectangle();
                beaconqunt = 0;

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
                button12.Enabled = true;
                textBox7.Visible = false;
                label7.Visible = false;
                button14.Enabled = false;
            }
        }

        private void button27_Click(object sender, EventArgs e)//clear room
        {
            if (beaconqunt == 0)
            {
                for (int k = 0; k < beaconqunt; k++)
                {
                    labelbeacon[k].Dispose();
                }
                for (int k = 0; k < uglqunt; k++)
                {
                    labelbox[k].Dispose();
                }

                graph.Clear(Color.White);
                beaconnumbers = 0;
                uglnumbers = 0;
                beaconlist.Clear();
                boxlist.Clear();
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                textBox1.Text = "";
                textBox2.Text = "";
                Drawing();

                checkBox2.Checked = false;
                checkBox1.Checked = false;

                double[,] SatPos;
                SatPos = null;
                double[,] Grad;
                double[,] Z;
                double[,] DeltaX;
                double[,] DeltaY;
                double[,] Tran;
                double[,] Umn;
                double[,] BoxPos;

                beaconflag = 0;
                boxflag = 0;
                deltax = 0;
                deltay = 0;
                beacontraf = new Rectangle();
                boxtraf = new Rectangle();
                beaconqunt = 0;
                uglqunt = 0;
                addugl = 0;
                indclearroom = 0;

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
                button1.Enabled = true;
                pictureBox1.Enabled = false;
                button11.Enabled = true;
                button12.Enabled = false;
                textBox7.Visible = false;
                label7.Visible = false;
                button14.Enabled = false;
                double[,] Block;
                walluqnt = 0;
                wallnumbers = 0;
            }
            else
            {
                for (int k = 0; k < uglqunt; k++)
                {
                    labelbox[k].Dispose();
                }
                uglnumbers = 0;
                boxlist.Clear();
                listBox2.Items.Clear();
                textBox2.Text = "";
                double[,] BoxPos;
                boxflag = 0;
                uglqunt = 0;
                addugl = 0;
                indclearroom = 1;

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
                button11.Enabled = true;
                button12.Enabled = false;
                textBox7.Visible = false;
                label7.Visible = false;
                button14.Enabled = false;

                double[,] Block;
                walluqnt = 0;
                wallnumbers = 0;
                boxtraf = new Rectangle();
                Drawing();
                foreach (Point p in beaconlist)
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
                    for (int i = 0; i < beaconqunt; i++)
                        beacon.Write(Math.Round((Convert.ToDouble(SatPos[0, i])/Convert.ToDouble(pxX)),2) + " " + Math.Round((Convert.ToDouble(pictureBox1.Height - SatPos[1, i])/Convert.ToDouble(pxY)),2) + '\n');
                }
            }
        }

        private void button29_Click(object sender, EventArgs e)//save room
        {
            SaveFileDialog saveroom = new SaveFileDialog();
            saveroom.DefaultExt = ".txt";
            saveroom.Filter = "txt.|*.txt";
            if (saveroom.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveroom.FileName.Length > 0)
            {
                using (StreamWriter beacon = new StreamWriter(saveroom.FileName, true))
                {
                    for (int i = 0; i < uglqunt; i++)
                        beacon.Write(Math.Round((Convert.ToDouble(BoxPos[0, i])/Convert.ToDouble(pxX)),2) + " " + Math.Round((Convert.ToDouble(pictureBox1.Height - BoxPos[1, i])/Convert.ToDouble(pxY)),2) + '\n');
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)//Полная очитска
        {
            for (int k = 0; k < beaconqunt; k++)
            {
                labelbeacon[k].Dispose();
            }
            for (int k = 0; k < uglqunt; k++)
            {
                labelbox[k].Dispose();
            }
            graph.Clear(Color.White);
            beaconnumbers = 0;
            uglnumbers = 0;
            beaconlist.Clear();
            boxlist.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            Drawing();

            checkBox2.Checked = false;
            checkBox1.Checked = false;

            double[,] SatPos;
            SatPos = null;
            double[,] Grad;
            double[,] Z;
            double[,] DeltaX;
            double[,] DeltaY;
            double[,] Tran;
            double[,] Umn;
            double[,] BoxPos;

            beaconflag = 0;
            boxflag = 0;
            deltax = 0;
            deltay = 0;
            beacontraf = new Rectangle();
            boxtraf = new Rectangle();
            beaconqunt = 0;
            uglqunt = 0;
            addugl = 0;
            indclearroom = 0;

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
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            button10.Enabled = true;
            textBox2.Enabled = false;
            button24.Enabled = false;
            button11.Enabled = true;

            textBox5.Text = "";
            textBox6.Text = "";
            label22.Text = Convert.ToString("");
            label20.Text = Convert.ToString("");
            label18.Text = Convert.ToString("");
            label16.Text = Convert.ToString("");
            label14.Text = Convert.ToString("");
            label12.Text = Convert.ToString("");
            label10.Text = Convert.ToString("");
            label8.Text = Convert.ToString("");
            label6.Text = Convert.ToString("");
            label4.Text = Convert.ToString("");
            label46.Text = Convert.ToString("");
            label44.Text = Convert.ToString("");
            label42.Text = Convert.ToString("");
            label35.Text = Convert.ToString("");
            label33.Text = Convert.ToString("");
            label31.Text = Convert.ToString("");
            label29.Text = Convert.ToString("");
            label27.Text = Convert.ToString("");
            label36.Text = Convert.ToString("");
            label24.Text = Convert.ToString("");

            Xmax=0;
            Ymax=0;
            pxX=0;
            pxY=0;

            pictureBox1.Enabled = false;
            button12.Enabled = false;
            textBox7.Visible = false;
            label7.Visible = false;
            button14.Enabled = false;
            double[,] Block;
            walluqnt = 0;
            wallnumbers = 0;
        }

        private void button30_Click(object sender, EventArgs e)
        {
            indphoto = true;
          //  Bitmap image; //Bitmap для открываемого изображения
            OpenFileDialog open_dialog = new OpenFileDialog(); //создание диалогового окна для выбора файла
            open_dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*"; //формат загружаемого файла
            if (open_dialog.ShowDialog() == DialogResult.OK) //если в окне была нажата кнопка "ОК"
            {
                try
                {
                    image = new Bitmap(open_dialog.FileName);
                    newImage = new Bitmap(image.Width, image.Height);
                    using (Graphics gr = Graphics.FromImage(newImage))
                    {
                        gr.SmoothingMode = SmoothingMode.HighQuality;
                        gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        gr.DrawImage(image, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
                    }
                    pictureBox1.Image = newImage;
                    photokard = pictureBox1.Image;
                    pictureBox1.BackgroundImage = photokard;
                    pictureBox1.Invalidate();
                    button18.Enabled = true;
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
            indintersection = false;
            for (int i = 0; i < beaconqunt; i++)
            {
                double ax1 = x;
                double ay1 = y;
                double ax2 = SatPos[0, i];
                double ay2 = SatPos[1, i];
                indintersection = false;
                for (int j = 0; j < uglqunt-1; j++)
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
                        indintersection = true;
                    }
                }
                if(indintersection==false)
                    rightpoint.Add(new Point() { X = Convert.ToInt32(SatPos[0, i]), Y = Convert.ToInt32(SatPos[1, i]) });
            }
            kolich = 0;
            foreach (Point pp in rightpoint)
            {
                kolich += 1;
            }
            SatPos = new double[2, kolich + 1];
            kolich = 0;
            foreach (Point pp in rightpoint)
            {
                SatPos[0, kolich] = pp.X;
                SatPos[1, kolich] = pp.Y;
                kolich += 1;
            }
            indintersection = false;
        }

        private void vidimostwall(int x, int y)
        {
            indintersection = false;
            for (int i = 0; i < beaconqunt; i++)
            {
                double ax1 = x;
                double ay1 = y;
                double ax2 = SatPos[0, i];
                double ay2 = SatPos[1, i];
                indintersection = false;
                for (int j = 0; j < uglqunt-1; j++)
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
                        indintersection = true;
                    }
                }
                for(int j = 0;j<walluqnt;j+=2)
                {
                    double bx1 = WallPos[0, j];
                    double by1 = WallPos[1, j];
                    double bx2 = WallPos[0, j + 1];
                    double by2 = WallPos[1, j + 1];
                    double v1 = (bx2 - bx1) * (ay1 - by1) - (by2 - by1) * (ax1 - bx1);
                    double v2 = (bx2 - bx1) * (ay2 - by1) - (by2 - by1) * (ax2 - bx1);
                    double v3 = (ax2 - ax1) * (by1 - ay1) - (ay2 - ay1) * (bx1 - ax1);
                    double v4 = (ax2 - ax1) * (by2 - ay1) - (ay2 - ay1) * (bx2 - ax1);
                    if ((v1 * v2 < 0) && (v3 * v4 < 0))
                    {
                        indintersection = true;
                    }
                }
                if (indintersection == false)
                    rightpoint.Add(new Point() { X = Convert.ToInt32(SatPos[0, i]), Y = Convert.ToInt32(SatPos[1, i]) });
            }
            kolich = 0;
            foreach (Point pp in rightpoint)
            {
                kolich += 1;
            }
            SatPos = new double[2, kolich + 1];
            kolich = 0;
            foreach (Point pp in rightpoint)
            {
                SatPos[0, kolich] = pp.X;
                SatPos[1, kolich] = pp.Y;
                kolich += 1;
            }
            indintersection = false;
        }
        private void lish()
        {
            SatClone = new double[2, beaconqunt + 1];
            for (int h = 0; h < beaconqunt; h++)
            {
                SatClone[0, h] = SatPos[0, h];
                SatClone[1, h] = SatPos[1, h];
            }

            for (int i = 0; i < beaconqunt - 1; i++)
            {

             
                indintersection = false;
                for (int j = 0; j < uglqunt-1; j++)
                {
                    double ax1 = SatPos[0, i];
                    double ay1 = SatPos[1, i];
                    double ax2 = SatPos[0, beaconqunt - 1];
                    double ay2 = SatPos[1, beaconqunt - 1];
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
                        indintersection = true;
                    }
                }
                if (indintersection==false)
                {
                    needpoint.Add(new Point() { X = Convert.ToInt32(SatPos[0, i]), Y = Convert.ToInt32(SatPos[1, i]) });
                }

            }
            qunt = 0;
            foreach (Point pp in needpoint)
            {
                qunt += 1;
            }
            SatPos = new double[2, qunt + 1];
            qunt = 0;
            foreach (Point pp in needpoint)
            {
                SatPos[0, qunt] = pp.X;
                SatPos[1, qunt] = pp.Y;
                qunt += 1;
            }
            SatPos[0, qunt] = SatClone[0, beaconqunt - 1];
            SatPos[1, qunt] = SatClone[1, beaconqunt - 1];
        }
        private void lishwall()
        {
            SatClone = new double[2, beaconqunt + 1];
            for (int h = 0; h < beaconqunt; h++)
            {
                SatClone[0, h] = SatPos[0, h];
                SatClone[1, h] = SatPos[1, h];
            }

            for (int i = 0; i < beaconqunt - 1; i++)
            {


                indintersection = false;
                for (int j = 0; j < uglqunt-1; j++)
                {
                    double ax1 = SatPos[0, i];
                    double ay1 = SatPos[1, i];
                    double ax2 = SatPos[0, beaconqunt - 1];
                    double ay2 = SatPos[1, beaconqunt - 1];
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
                        indintersection = true;
                    }
                }
                for (int j = 0; j < walluqnt; j+=2)
                {
                    double ax1 = SatPos[0, i];
                    double ay1 = SatPos[1, i];
                    double ax2 = SatPos[0, beaconqunt - 1];
                    double ay2 = SatPos[1, beaconqunt - 1];
                    double bx1 = WallPos[0, j];
                    double by1 = WallPos[1, j];
                    double bx2 = WallPos[0, j + 1];
                    double by2 = WallPos[1, j + 1];

                    double v1 = (bx2 - bx1) * (ay1 - by1) - (by2 - by1) * (ax1 - bx1);
                    double v2 = (bx2 - bx1) * (ay2 - by1) - (by2 - by1) * (ax2 - bx1);
                    double v3 = (ax2 - ax1) * (by1 - ay1) - (ay2 - ay1) * (bx1 - ax1);
                    double v4 = (ax2 - ax1) * (by2 - ay1) - (ay2 - ay1) * (bx2 - ax1);
                    if ((v1 * v2 < 0) && (v3 * v4 < 0))
                    {
                        indintersection = true;
                    }
                }
                if (indintersection == false)
                {
                    needpoint.Add(new Point() { X = Convert.ToInt32(SatPos[0, i]), Y = Convert.ToInt32(SatPos[1, i]) });
                }

            }
            qunt = 0;
            foreach (Point pp in needpoint)
            {
                qunt += 1;
            }
            SatPos = new double[2, qunt + 1];
            qunt = 0;
            foreach (Point pp in needpoint)
            {
                SatPos[0, qunt] = pp.X;
                SatPos[1, qunt] = pp.Y;
                qunt += 1;
            }
            SatPos[0, qunt] = SatClone[0, beaconqunt - 1];
            SatPos[1, qunt] = SatClone[1, beaconqunt - 1];
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"GDOP - программа, которая помогает определить значение геометрического фактора. 
1.Изначально Вам доступна стандартная комната. Вы можете использовать ее или стереть(кнопка clear room) и нарисоват/загрузить свою.
2.Если используете стандартную комнату следущий шаг - ввести количество маяков и поставить их на местность;
3.Если используете свою комнату - ввести количество углов помещения, в котором вы собираетесь определить значение геометрического фактора;
4.После нажатия на кпоку build on the plot установить углы на местности(для удобства имеется координатная ось, а также слева отображаются координаты курсора на поле в данный момент);
5.Ввести количество маяков;
6.После нажатия на кнопку set on the plot утановить маяки на местности.
7.После этого вы уже можете расчитать значение геометричесукого фактора по дальномерному методу или разностно - дальномерному методу(главный маяк(мастер) будет последним поставленным на местности), для этого выберите метод и нажмите кнопку plot a graph;
8.Также у вас есть возможность добавлять по 1 маяку на местность и 1 углу комнаты. Для этого необходимо нажать на кнопку add 1 рядом с введенным значением маяков или углов комнаты(Красные линии подскажут будущее расположение стен команты);
9.У вас есть возможность перемещать маяки по местности или углы комнаты, при этом все расчеты будут проводится относительно новых параметров.
10.При нажатии правой кнопки мыши на маяк или угол команты данный маяк(угол) удалится с местности.
11.Вы не можете ставить несколько маяков или углов комнаты в 1 точку.
12.Также при переносе маяков или углов комнаты вы не можете перетащить их в другой маяк или угол.
13.Вы можете полностью удалить комнату с местности, нажав кнопку clear room;
14.Вы можете полностью удалить маяки с местности, нажав кнопку clear beacons;
15.Очистить все полностью - кнопка clear all.
16.При нажатии кнопки clear all у вас есть возможность указать размер местности(в саниметрах);
17.У Вас есть воможность сохранять координаты маяков и углов комнаты(кнопки SAVE);
18.У Вас есть возможность загружать координты комнаты и маяков(кнопки LOAD).Проследите за тем,чтобы координты были записаны в 2 столбика;
19.У Вас есть возможесть сохранять картинку(кнопка SAVE IMAGE);
20.При наведении мыши на область градиента, Вы можеет посмотреть значение геометрического фактора в данной точке в специальном окне слева от градинта;
21.При нажатии левой кнопки мыши на точку местности и ведя мышь в произвольном направлении, Вы можете измерить расстояние от точки нажатие до настощей точки курсора;
22.У вас есть возможность загрузить план помещения. Для этого нажмите кнопку LOAD PLAN и выберите нужный файл. Далее введите количество углов этого помещения и постройте стены,используя план в качестве трафарета;
23.После построения поверхности появятся окна с погрешностям. Сначала введите коэффицинт погрешности в доступное окно слева, затем при наведении курсора на точку метсности, в двух окнах ниже выводятся погрешности по двух осям;
24.После ввода погрешности вокруг куросра будет риссоваться эллипс,показывающий погрешности на расстоянии он курсора;
25.В графу радиус введите радиус действия маяка(расстояние, после которого маяк совсем теряет видимость).
О значениях градиента геометрического фактора:
Синий цвет - хорошая видимость;Зеленый цвет - средняя видимость;Красный цвет - плохая видимость;Белый цвет - видимости нет.");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"GDOP is a program that helps determine the value of a geometric factor.
1.Initially, a standard room is available to you.You can use it or erase(clear room button) and draw / load your own.
2.If you are using a standard room, the next step is to enter the number of lighthouses and put them on the ground;
3.If you use your room -enter the number of corners of the room in which you are going to determine the value of the geometric factor;
4.After clicking on the build on the plot button, set the angles on the terrain(for convenience, there is a coordinate axis, and the coordinates of the cursor on the field at the moment are also displayed on the left) ;
5.Enter the number of beacons;
6.After pressing the set on the plot button, set the beacons on the ground.
7.After that, you can already calculate the value of the geometric factor by the rangefinder method or the difference - rangefinder method(the main beacon(master) will be the last set on the ground), for this select the method and press the GO button;
8.Also you have the opportunity to add 1 lighthouse to the terrain and 1 corner of the room.To do this, click on the add 1 button next to the entered value of the beacons or the corners of the room(Red lines will tell you the future location of the walls of the room);
9.You have the opportunity to move the beacons in the area or the corners of the room, while all calculations will be carried out with respect to the new parameters.
10.When you right-click on a lighthouse or commando corner, this lighthouse(corner) will be removed from the terrain.
11.You cannot set several beacons or corners of a room at 1 point.
12.Also, when moving beacons or room corners, you cannot drag them to another beacon or corner.
13.You can completely remove the room from the area by pressing the clear room button;
14.You can completely remove the beacons from the area by clicking the clear beacons button;
15.Clear all completely -clear all button.
16.When you click the clear all button, you have the opportunity to indicate the size of the terrain(in centimeters);
17.You have the ability to save the coordinates of the beacons and the corners of the room(SAVE buttons);
18.You have the ability to load the coordinates of the room and beacons(LOAD buttons). Make sure that the coordinates are written in 2 columns;
19.You have the opportunity to save the picture(SAVE IMAGE button);
20.When you hover over the gradient area, you can see the value of the geometric factor at a given point in a special window to the left of the gradient;
21.When you click the left mouse button on a terrain point and moving the mouse in an arbitrary direction, you can measure the distance from the point by pressing to the current cursor point;
22.You have the opportunity to download the floor plan. To do this, press the LOAD PLAN button and select the desired file. Next, enter the number of corners of this room and build the walls using the plan as a stencil;
23.After building the surface, windows with errors will appear.First, enter the error coefficient in the available window on the left, then when you hover the cursor on the mixed point, the errors in the two axes are displayed in the two windows below;
24.After entering the error, an ellipse will be drawn around the kurosra, showing the error at the distance of the cursor;
25.In the radius column, enter the radius of the beacon(the distance after which the beacon completely loses visibility).
About the values ​​of the gradient of the geometric factor:
Blue color -good visibility; Green color -medium visibility; Red color -poor visibility; White color -no visibility.");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string xMax = textBox5.Text;
            string yMax = textBox6.Text;
            if (xMax == "" || yMax == "")
                MessageBox.Show("Input X and Y");
            else
            {
                int n;
                if (int.TryParse(textBox5.Text, out n))
                {
                    if (int.TryParse(textBox6.Text, out n))
                    {
                        Xmax = Convert.ToInt32(xMax);
                        Ymax = Convert.ToInt32(yMax);
                        label22.Text = Convert.ToString(Xmax);
                        label20.Text = Convert.ToString(Xmax * 0.9);
                        label18.Text = Convert.ToString(Xmax * 0.8);
                        label16.Text = Convert.ToString(Xmax * 0.7);
                        label14.Text = Convert.ToString(Xmax * 0.6);
                        label12.Text = Convert.ToString(Xmax * 0.5);
                        label10.Text = Convert.ToString(Xmax * 0.4);
                        label8.Text = Convert.ToString(Xmax * 0.3);
                        label6.Text = Convert.ToString(Xmax * 0.2);
                        label4.Text = Convert.ToString(Xmax * 0.1);
                        label46.Text = Convert.ToString(Ymax);
                        label44.Text = Convert.ToString(Ymax * 0.9);
                        label42.Text = Convert.ToString(Ymax * 0.8);
                        label35.Text = Convert.ToString(Ymax * 0.7);
                        label33.Text = Convert.ToString(Ymax * 0.6);
                        label31.Text = Convert.ToString(Ymax * 0.5);
                        label29.Text = Convert.ToString(Ymax * 0.4);
                        label27.Text = Convert.ToString(Ymax * 0.3);
                        label36.Text = Convert.ToString(Ymax * 0.2);
                        label24.Text = Convert.ToString(Ymax * 0.1);

                        Size resolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
                        pictureBox1.Location = new Point(groupBox4.Width + label46.Width + 10, (resolution.Height - 100 - pictureBox1.Height) / 2);

                        label46.Location = new Point(pictureBox1.Location.X - label46.Width, pictureBox1.Location.Y);
                        label44.Location = new Point(pictureBox1.Location.X - label44.Width, pictureBox1.Location.Y + pictureBox1.Height / 10);
                        label42.Location = new Point(pictureBox1.Location.X - label42.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 2);
                        label35.Location = new Point(pictureBox1.Location.X - label35.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 3);
                        label33.Location = new Point(pictureBox1.Location.X - label33.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 4);
                        label31.Location = new Point(pictureBox1.Location.X - label31.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 5);
                        label29.Location = new Point(pictureBox1.Location.X - label29.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 6);
                        label27.Location = new Point(pictureBox1.Location.X - label27.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 7);
                        label36.Location = new Point(pictureBox1.Location.X - label36.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 8);
                        label24.Location = new Point(pictureBox1.Location.X - label24.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 9);
                        label2.Location = new Point(pictureBox1.Location.X - label2.Width, pictureBox1.Location.Y + pictureBox1.Height);

                        label4.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 - label4.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
                        label6.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 2 - label6.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
                        label8.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 3 - label8.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
                        label10.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 4 - label10.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
                        label12.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 5 - label12.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
                        label14.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 6 - label14.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
                        label16.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 7 - label16.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
                        label18.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 8 - label18.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
                        label20.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 9 - label20.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
                        label22.Location = new Point(pictureBox1.Location.X + pictureBox1.Width - label22.Width, pictureBox1.Location.Y + pictureBox1.Height);

                        textBox2.Enabled = true;
                        button24.Enabled = true;
                        textBox5.Enabled = false;
                        textBox6.Enabled = false;
                        button10.Enabled = false;
                        button1.Enabled = true;
                        pxX = (double)(pictureBox1.Width / Convert.ToDouble(Xmax));
                        pxY = (double)(pictureBox1.Height / Convert.ToDouble(Ymax));
                    }
                    else
                        MessageBox.Show("Input correct number");
                }
                else
                    MessageBox.Show("Input correct number");              
            }
        }
        string name;
        private void button11_Click(object sender, EventArgs e)
        {
            OpenFileDialog loadroom = new OpenFileDialog();
            loadroom.DefaultExt = ".txt";
            loadroom.Filter = "txt.|*.txt";
            if (loadroom.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    name = loadroom.FileName;
                    BoxPos = ReadArray(name);
                    double maxXX = BoxPos[0, 0];
                    double maxYY = BoxPos[1, 0];
                    for (int i = 0; i < uglqunt; i++)
                    {
                        if (BoxPos[0, i] > maxXX)
                            maxXX = BoxPos[0, i];
                    }
                    for (int i = 0; i < uglqunt; i++)
                    {
                        if (BoxPos[1, i] > maxYY)
                            maxYY = BoxPos[1, i];
                    }
                    Xmax = Convert.ToInt32(100 + maxXX);
                    Ymax = Convert.ToInt32(100 + maxYY);
                    textBox5.Text = Xmax.ToString();
                    textBox6.Text = Ymax.ToString();
                    pxX = (double)(pictureBox1.Width / Convert.ToDouble(Xmax));
                    pxY = (double)(pictureBox1.Height / Convert.ToDouble(Ymax));
                    for (int i = 0; i < uglqunt; i++)
                    {
                        BoxPos[0, i] = Convert.ToDouble(BoxPos[0, i]) * Convert.ToDouble(pxX);
                        BoxPos[1, i] = pictureBox1.Height - (Convert.ToDouble(BoxPos[1, i]) * Convert.ToDouble(pxY));
                    }
                    boxlist.Clear();
                    for (int j = 0; j < uglqunt; j++)
                    {
                        listBox2.Items.Add((j + 1) + ")" + "X:" + Math.Round((Convert.ToDouble(BoxPos[0, j]) / Convert.ToDouble(pxX)),2) + "," + "Y:" + Math.Round(((Convert.ToDouble(pictureBox1.Height - BoxPos[1, j]) / Convert.ToDouble(pxY))),2));
                        boxlist.Add(new Point() { X = Convert.ToInt32(BoxPos[0, j]), Y = Convert.ToInt32(BoxPos[1, j]) });
                    }

                    bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    graph = Graphics.FromImage(bmp);
                    pen = new Pen(Color.Black);
                    pictureBox1.Image = bmp;

                    label22.Text = Convert.ToString(Xmax);
                    label20.Text = Convert.ToString(Xmax * 0.9);
                    label18.Text = Convert.ToString(Xmax * 0.8);
                    label16.Text = Convert.ToString(Xmax * 0.7);
                    label14.Text = Convert.ToString(Xmax * 0.6);
                    label12.Text = Convert.ToString(Xmax * 0.5);
                    label10.Text = Convert.ToString(Xmax * 0.4);
                    label8.Text = Convert.ToString(Xmax * 0.3);
                    label6.Text = Convert.ToString(Xmax * 0.2);
                    label4.Text = Convert.ToString(Xmax * 0.1);
                    label46.Text = Convert.ToString(Ymax);
                    label44.Text = Convert.ToString(Ymax * 0.9);
                    label42.Text = Convert.ToString(Ymax * 0.8);
                    label35.Text = Convert.ToString(Ymax * 0.7);
                    label33.Text = Convert.ToString(Ymax * 0.6);
                    label31.Text = Convert.ToString(Ymax * 0.5);
                    label29.Text = Convert.ToString(Ymax * 0.4);
                    label27.Text = Convert.ToString(Ymax * 0.3);
                    label36.Text = Convert.ToString(Ymax * 0.2);
                    label24.Text = Convert.ToString(Ymax * 0.1);

                    Size resolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
                    pictureBox1.Location = new Point(groupBox4.Width + label46.Width + 10, (resolution.Height - 100 - pictureBox1.Height) / 2);

                    label46.Location = new Point(pictureBox1.Location.X - label46.Width, pictureBox1.Location.Y);
                    label44.Location = new Point(pictureBox1.Location.X - label44.Width, pictureBox1.Location.Y + pictureBox1.Height / 10);
                    label42.Location = new Point(pictureBox1.Location.X - label42.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 2);
                    label35.Location = new Point(pictureBox1.Location.X - label35.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 3);
                    label33.Location = new Point(pictureBox1.Location.X - label33.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 4);
                    label31.Location = new Point(pictureBox1.Location.X - label31.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 5);
                    label29.Location = new Point(pictureBox1.Location.X - label29.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 6);
                    label27.Location = new Point(pictureBox1.Location.X - label27.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 7);
                    label36.Location = new Point(pictureBox1.Location.X - label36.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 8);
                    label24.Location = new Point(pictureBox1.Location.X - label24.Width, pictureBox1.Location.Y + pictureBox1.Height / 10 * 9);
                    label2.Location = new Point(pictureBox1.Location.X - label2.Width, pictureBox1.Location.Y + pictureBox1.Height);

                    label4.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 - label4.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
                    label6.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 2 - label6.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
                    label8.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 3 - label8.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
                    label10.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 4 - label10.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
                    label12.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 5 - label12.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
                    label14.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 6 - label14.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
                    label16.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 7 - label16.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
                    label18.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 8 - label18.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
                    label20.Location = new Point(pictureBox1.Location.X + pictureBox1.Width / 10 * 9 - label20.Width / 2, pictureBox1.Location.Y + pictureBox1.Height);
                    label22.Location = new Point(pictureBox1.Location.X + pictureBox1.Width - label22.Width, pictureBox1.Location.Y + pictureBox1.Height);

                    roompaint();
                    labelbox = new Label[uglqunt + 1];
                    labalbox();
                    textBox5.Enabled = false;
                    textBox6.Enabled = false;
                    button10.Enabled = false;
                    textBox2.Enabled = false;
                    button24.Enabled = false;
                    textBox1.Enabled = true;
                    button22.Enabled = true;
                    button27.Enabled = true;
                    button1.Enabled = true;
                    button11.Enabled = false;
                    uglnumbers = uglqunt;
                    textBox2.Text = uglqunt.ToString();
                    button12.Enabled = true;
                    button29.Enabled = true;
                    if (SatPos != null && beaconqunt!=0)
                    {
                        for (int j = 0; j < beaconqunt; j++)
                        {
                            graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                        }
                        pictureBox1.Enabled = true;
                        textBox1.Enabled = false;
                        textBox2.Enabled = false;
                        button22.Enabled = false;
                        button23.Enabled = true;
                        button25.Enabled = true;
                        button22.Enabled = false;
                        button14.Enabled = true;
                        button3.Enabled = true;
                        button12.Enabled = false;
                    }
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно открыть выбранный файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        double[,] ReadArray(string filename)
        {
            double[,] result;
            double[,] result1;

            using (var reader = new StreamReader(filename))
            {
                int count = System.IO.File.ReadAllLines(filename).Length;
                result = new double[count, 2];
                BoxPos = new double[count, 2];
                uglqunt = count;
                for (int i = 0; i < count; i++)
                {
                    var line = reader.ReadLine();
                    line = line.Trim();
                    line = string.Join(" ", line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                    var values = line.Split(' ').Select(double.Parse).ToArray();

                    for (int j = 0; j < 2; j++)
                        result[i, j] = values[j];
                }
                result1 = new double[2, count];
                for (int i = 0; i < uglqunt; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        result1[j, i] = result[i, j];
                    }
                }
            }

            return result1;
        }
        double[,] ReadArray1(string filename)
        {
            double[,] result;
            double[,] result1;

            using (var reader = new StreamReader(filename))
            {
                int count = System.IO.File.ReadAllLines(filename).Length;
                result = new double[count, 2];
                SatPos = new double[count, 2];
                beaconqunt = count;
                for (int i = 0; i < count; i++)
                {
                    var line = reader.ReadLine();
                    line = line.Trim();
                    line = string.Join(" ", line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                    var values = line.Split(' ').Select(double.Parse).ToArray();

                    for (int j = 0; j < 2; j++)
                        result[i, j] = values[j];
                }
                result1 = new double[2, count];
                for (int i = 0; i < beaconqunt; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        result1[j, i] = result[i, j];
                    }
                }
            }

            return result1;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            OpenFileDialog loadbeacons = new OpenFileDialog();
            loadbeacons.DefaultExt = ".txt";
            loadbeacons.Filter = "txt.|*.txt";
            if (loadbeacons.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    name = loadbeacons.FileName;
                    SatPos = ReadArray1(name);

                    double maxXX = SatPos[0, 0];
                    double maxYY = SatPos[1, 0];
                    for (int i = 0; i < beaconqunt; i++)
                    {
                        if (SatPos[0, i] > maxXX)
                            maxXX = SatPos[0, i];
                    }
                    for (int i = 0; i < beaconqunt; i++)
                    {
                        if (SatPos[1, i] > maxYY)
                            maxYY = SatPos[1, i];
                    }
                    if (maxXX > Xmax || maxYY > Ymax)
                    {
                        MessageBox.Show("One of the beacons goes beyond the borders");
                        SatPos = null;
                    }
                    else
                    {
                        for (int i = 0; i < beaconqunt; i++)
                        {
                            SatPos[0, i] = Convert.ToDouble(SatPos[0, i]) * Convert.ToDouble(pxX);
                            SatPos[1, i] = pictureBox1.Height - (Convert.ToDouble(SatPos[1, i]) * Convert.ToDouble(pxY));
                        }
                        beaconlist.Clear();
                        for (int j = 0; j < beaconqunt; j++)
                        {
                            listBox1.Items.Add((j + 1) + ")" + "X:" + Math.Round((Convert.ToDouble(SatPos[0, j]) / Convert.ToDouble(pxX)),2) + "," + "Y:" + Math.Round(((Convert.ToDouble(pictureBox1.Height - SatPos[1, j]) / Convert.ToDouble(pxY))),2));
                            beaconlist.Add(new Point() { X = Convert.ToInt32(SatPos[0, j]), Y = Convert.ToInt32(SatPos[1, j]) });
                        }

                        bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                        graph = Graphics.FromImage(bmp);
                        pen = new Pen(Color.Black);
                        pictureBox1.Image = bmp;

                        for (int j = 0; j < beaconqunt; j++)
                        {
                            graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                        }

                        beaconnumbers = beaconqunt;
                        textBox1.Text = beaconqunt.ToString();
                        labelbeacon = new Label[beaconqunt + 1];
                        Grad = new double[beaconqunt + 1, 2];
                        Tran = new double[2, beaconqunt + 1];
                        Multi = new double[2, 2];
                        labalbeacon();
                        roompaint();
                        button12.Enabled = false;
                        button22.Enabled = false;
                        textBox1.Enabled = false;
                        button3.Enabled = true;
                        pictureBox1.Enabled = true;
                        button26.Enabled = true;
                        button23.Enabled = true;
                        button14.Enabled = true;
                        button25.Enabled = true;
                        button28.Enabled = true;
                    }
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно открыть выбранный файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Bitmap bmpe;
            if (pictureBox1.BackgroundImage != null)
                bmpe = new Bitmap(pictureBox1.BackgroundImage);
            else
                bmpe = new Bitmap(bmp);
            if (pictureBox1.Image != null) //если в pictureBox есть изображение
            {
                //создание диалогового окна "Сохранить как..", для сохранения изображения
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Title = "Сохранить картинку как...";
                //отображать ли предупреждение, если пользователь указывает имя уже существующего файла
                savedialog.OverwritePrompt = true;
                //отображать ли предупреждение, если пользователь указывает несуществующий путь
                savedialog.CheckPathExists = true;
                //список форматов файла, отображаемый в поле "Тип файла"
                savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                //отображается ли кнопка "Справка" в диалоговом окне
                savedialog.ShowHelp = true;
                if (savedialog.ShowDialog() == DialogResult.OK) //если в диалоговом окне нажата кнопка "ОК"
                {
                    try
                    {
                        bmpe.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)//Wall
        {
            walluqnt += 2;
            double[,] BlockClone = new double[2, walluqnt - 2];
            for(int i=0;i<walluqnt-2;i++)
            {
                BlockClone[0, i] = WallPos[0, i];
                BlockClone[1, i] = WallPos[1, i];
            }
            WallPos = new double[2, walluqnt];
            for(int i=0;i<walluqnt-2;i++)
            {
                WallPos[0, i] = BlockClone[0, i];
                WallPos[1, i] = BlockClone[1, i];
            }
            button14.Enabled = false;
            button23.Enabled = false;
            button25.Enabled = false;
            button3.Enabled = false;
            button27.Enabled = false;
            button26.Enabled = false;
            button1.Enabled = false;
        }

        private void vidimost1(int x, int y)
        {
            indintersection = false;
            for (int j = 0; j < uglqunt-1; j++)
            {
                double ax1 = x;
                double ay1 = y;
                double ax2 = SatClone[0, beaconqunt-1];
                double ay2 = SatClone[1, beaconqunt-1];

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
                    indintersection = true;
                }
            }
            if (indintersection == true)
                kolich = 0;
            else
            {
                indintersection = false;
                for (int i = 0; i < qunt; i++)
                {
                    indintersection = false;
                    double ax1 = x;
                    double ay1 = y;
                    double ax2 = SatPos[0, i];
                    double ay2 = SatPos[1, i];
                    indintersection = false;
                    for (int j = 0; j < uglqunt-1; j++)
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
                            indintersection = true;
                        }
                    }
                    if (indintersection == false)
                    {
                        rightpoint.Add(new Point() { X = Convert.ToInt32(SatPos[0, i]), Y = Convert.ToInt32(SatPos[1, i]) });
                    }

                }
                kolich = 0;
                foreach (Point pp in rightpoint)
                {
                    kolich += 1;
                }
                SatPos = new double[2, kolich + 2];
                kolich = 0;
                foreach (Point pp in rightpoint)
                {
                    SatPos[0, kolich] = pp.X;
                    SatPos[1, kolich] = pp.Y;
                    kolich += 1;
                }
                SatPos[0, kolich] = SatClone[0,beaconqunt-1];
                SatPos[1, kolich] = SatClone[1,beaconqunt-1];
                indintersection = false;
            }
        }
        private void button15_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        //remove
        private void button16_Click(object sender, EventArgs e)
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graph = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;

            form.Show();
            form.progressBar1.Value = 0;

            for (int j = 0; j < pictureBox1.Width; j++)
            {
                for (int l = 0; l < pictureBox1.Height; l++)
                {
                    if (Z[j, l] == 0)
                    {
                        pen = new Pen(Color.FromArgb(255, 255, 255));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 0 && Z[j, l] <= 20)
                    {
                        int test = Convert.ToInt32(Math.Round(Z[j, l], 1) * 10);
                        var pixelColor = new Color();
                        if (201 - test > 0 && 201 - test <= nn.Width)
                            pixelColor = n.GetPixel(201 - test, 1);
                        pen.Color = pixelColor;
                        graph.DrawEllipse(pen, j, l, 1, 1);

                    }
                    if (Z[j, l] > 20 && Z[j, l] <= 50)
                    {
                        int test2 = Convert.ToInt32(Math.Round(Z[j, l], 1) * 10);
                        var pixelColor1 = new Color();
                        if (test2 - 201 > 0 && test2 - 201 <= nn.Width)
                            pixelColor1 = nn.GetPixel((test2 - 201), 1);
                        pen.Color = pixelColor1;
                        graph.DrawEllipse(pen, j, l, 1, 1);

                    }
                    if (Z[j, l] > 20 && Z[j, l] < 21)
                    {
                        pen.Color = Color.Red;
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] == 50)
                    {
                        pen.Color = Color.Maroon;
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 50 && Z[j, l] < 1000000000)
                    {
                        pen = new Pen(Color.FromArgb(139, 0, 0));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1000000000)
                    {
                        if (Z[j + 1, l] == 0 && Z[j - 1, l] == 0 && Z[j, l + 1] == 0 && Z[j, l - 1] == 0)
                        {
                            pen = new Pen(Color.FromArgb(255, 255, 255));
                            graph.DrawEllipse(pen, j, l, 1, 1);
                        }
                    }
                    form.progressBar1.Value += 1;
                }
            }

            for (int j = 0; j < pictureBox1.Width; j++)
            {
                for (int l = 0; l < pictureBox1.Height; l++)
                {
                    if (Z[j, l] > trackBar1.Value)
                    {
                        pen = new Pen(Color.FromArgb(255, 255, 255));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    form.progressBar1.Value += 1;
                }
            }
            if (form.progressBar1.Value == form.progressBar1.Maximum)
            {
                form.Hide();
            }
            if (checkBox1.Checked == true && checkBox2.Checked == false)
            {
                pen = new Pen(Color.Black);
                for (int j = 0; j < beaconqunt; j++)
                {
                    Brush = new SolidBrush(Color.Black);
                    graph.FillEllipse(Brush, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                }
                for (int j = 0; j < uglqunt; j++)
                {
                    Brush = new SolidBrush(Color.Black);
                    graph.FillRectangle(Brush, Convert.ToInt32(BoxPos[0, j]) - 6, Convert.ToInt32(BoxPos[1, j]) - 6, 12, 12);
                }
                for (int j = 0; j < walluqnt; j++)
                {
                    Brush = new SolidBrush(Color.Black);
                    graph.FillRectangle(Brush, Convert.ToInt32(WallPos[0, j]) - 6, Convert.ToInt32(WallPos[1, j]) - 6, 12, 12);
                }
                pen.Width = 5;
                for (int i = 0; i < uglqunt - 1; i++)
                {
                    graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, i]), Convert.ToInt32(BoxPos[1, i]), Convert.ToInt32(BoxPos[0, i + 1]), Convert.ToInt32(BoxPos[1, i + 1]));
                }
                if (walluqnt != 0)
                {
                    for (int u = 0; u < walluqnt; u += 2)
                        graph.DrawLine(pen, Convert.ToInt32(WallPos[0, u]), Convert.ToInt32(WallPos[1, u]), Convert.ToInt32(WallPos[0, u + 1]), Convert.ToInt32(WallPos[1, u + 1]));
                }
                pen.Width = 1;
                for (int j = 0; j < uglqunt; j++)
                {
                    graph.DrawRectangle(pen, Convert.ToInt32(BoxPos[0, j]) - 6, Convert.ToInt32(BoxPos[1, j]) - 6, 12, 12);
                }
                if (walluqnt != 0)
                {
                    for (int i = 0; i < walluqnt; i++)
                    {
                        graph.DrawRectangle(pen, Convert.ToInt32(WallPos[0, i] - 6), Convert.ToInt32(WallPos[1, i] - 6), 12, 12);
                    }
                }
                pen.Width = 2;
                if (textBox11.Text != "")
                {
                    double radius = Convert.ToDouble(textBox11.Text);
                    for (int i = 0; i < beaconqunt; i++)
                    {
                        graph.DrawEllipse(pen, Convert.ToSingle(SatPos[0, i] - (radius * pxX)), Convert.ToSingle(SatPos[1, i] - (radius * pxY)), Convert.ToSingle(radius * Convert.ToDouble(pxX)) * 2, Convert.ToSingle(radius * Convert.ToDouble(pxY)) * 2);
                    }
                }
                pen.Width = 1;
            }
            if (checkBox2.Checked == true && checkBox1.Checked == false)
            {
                pen = new Pen(Color.Black);
                for (int j = 0; j < beaconqunt; j++)
                {
                    Brush = new SolidBrush(Color.Black);
                    graph.FillEllipse(Brush, Convert.ToInt32(SatPos[0, j]) - 8, Convert.ToInt32(SatPos[1, j]) - 8, 16, 16);
                }
                for (int j = 0; j < uglqunt; j++)
                {
                    Brush = new SolidBrush(Color.Black);
                    graph.FillRectangle(Brush, Convert.ToInt32(BoxPos[0, j]) - 6, Convert.ToInt32(BoxPos[1, j]) - 6, 12, 12);
                }
                for (int j = 0; j < walluqnt; j++)
                {
                    Brush = new SolidBrush(Color.Black);
                    graph.FillRectangle(Brush, Convert.ToInt32(WallPos[0, j]) - 6, Convert.ToInt32(WallPos[1, j]) - 6, 12, 12);
                }
                pen.Width = 5;
                for (int i = 0; i < uglqunt - 1; i++)
                {
                    graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, i]), Convert.ToInt32(BoxPos[1, i]), Convert.ToInt32(BoxPos[0, i + 1]), Convert.ToInt32(BoxPos[1, i + 1]));
                }
                if (walluqnt != 0)
                {
                    for (int u = 0; u < walluqnt; u += 2)
                        graph.DrawLine(pen, Convert.ToInt32(WallPos[0, u]), Convert.ToInt32(WallPos[1, u]), Convert.ToInt32(WallPos[0, u + 1]), Convert.ToInt32(WallPos[1, u + 1]));
                }
                pen.Width = 1;
                for (int j = 0; j < uglqunt; j++)
                {
                    graph.DrawRectangle(pen, Convert.ToInt32(BoxPos[0, j]) - 6, Convert.ToInt32(BoxPos[1, j]) - 6, 12, 12);
                }
                if (walluqnt != 0)
                {
                    for (int i = 0; i < walluqnt; i++)
                    {
                        graph.DrawRectangle(pen, Convert.ToInt32(WallPos[0, i] - 6), Convert.ToInt32(WallPos[1, i] - 6), 12, 12);
                    }
                }
                pen.Width = 2;
                if (textBox11.Text != "")
                {
                    double radius = Convert.ToDouble(textBox11.Text);
                    for (int i = 0; i < beaconqunt; i++)
                    {
                        graph.DrawEllipse(pen, Convert.ToSingle(SatPos[0, i] - (radius * pxX)), Convert.ToSingle(SatPos[1, i] - (radius * pxY)), Convert.ToSingle(radius * Convert.ToDouble(pxX)) * 2, Convert.ToSingle(radius * Convert.ToDouble(pxY)) * 2);
                    }
                }
                pen.Width = 1;
                int xM = Convert.ToInt32(SatPos[0, beaconqunt - 1]);
                int yM = Convert.ToInt32(SatPos[1, beaconqunt - 1]);
                Brush = new SolidBrush(Color.White);
                graph.FillEllipse(Brush, xM - 6, yM - 6, 12, 12);
            }
            pictureBox1.BackgroundImage = bmp;
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label21.Text = trackBar1.Value.ToString();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            string text1 = textBox1.Text;
            string text2 = textBox2.Text;
            string text3 = textBox5.Text;
            string text4 = textBox6.Text;
            if (text1=="" && text2==""&& text3!=""&&text4!=""&&textBox5.Enabled==false)
            {
                button30.Enabled = true;
            }
            else
            {
                button30.Enabled = false;
            }

            string textrad = textBox11.Text;
            if(textrad!=""&& label7.Visible==false&&beaconnumbers==beaconqunt&&IsClicked==false)
            {
                bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                graph = Graphics.FromImage(bmp);
                pictureBox1.Image = bmp;
                pen = new Pen(Color.Black);
                Drawing();
                for (int i = 0; i < uglqunt - 1; i++)
                {
                    graph.DrawLine(pen, Convert.ToInt32(BoxPos[0, i]), Convert.ToInt32(BoxPos[1, i]), Convert.ToInt32(BoxPos[0, i + 1]), Convert.ToInt32(BoxPos[1, i + 1]));
                }
                if (walluqnt != 0 && wallnumbers % 2==0)
                {
                    for (int u = 0; u < walluqnt; u += 2)
                        graph.DrawLine(pen, Convert.ToInt32(WallPos[0, u]), Convert.ToInt32(WallPos[1, u]), Convert.ToInt32(WallPos[0, u + 1]), Convert.ToInt32(WallPos[1, u + 1]));
                }
                if(walluqnt > 2 && wallnumbers % 2 != 0)
                {
                    for (int u = 0; u < walluqnt-2; u += 2)
                        graph.DrawLine(pen, Convert.ToInt32(WallPos[0, u]), Convert.ToInt32(WallPos[1, u]), Convert.ToInt32(WallPos[0, u + 1]), Convert.ToInt32(WallPos[1, u + 1]));
                }
                for (int j = 0; j < uglqunt; j++)
                {
                    graph.DrawRectangle(pen, Convert.ToInt32(BoxPos[0, j]) - 6, Convert.ToInt32(BoxPos[1, j]) - 6, 12, 12);
                }
                if (walluqnt != 0)
                {
                    for (int i = 0; i < walluqnt; i++)
                    {
                        graph.DrawRectangle(pen, Convert.ToInt32(WallPos[0, i] - 6), Convert.ToInt32(WallPos[1, i] - 6), 12, 12);
                    }
                }
                for (int i = 0; i < beaconqunt; i++)
                {
                    graph.DrawEllipse(pen, Convert.ToInt32(SatPos[0, i]) - 8, Convert.ToInt32(SatPos[1, i]) - 8, 16, 16);
                }
                if (textBox11.Text != "")
                {
                    double n;
                    if (double.TryParse(textBox11.Text, out n))
                    {
                        double radius = Convert.ToDouble(textBox11.Text);
                        for (int i = 0; i < beaconqunt; i++)
                        {
                            graph.DrawEllipse(pen, Convert.ToSingle(SatPos[0, i] - (radius * pxX)), Convert.ToSingle(SatPos[1, i] - (radius * pxY)), Convert.ToSingle(radius * Convert.ToDouble(pxX)) * 2, Convert.ToSingle(radius * Convert.ToDouble(pxY)) * 2);
                        }
                    }
                }
                if(IsInfo==true)
                {
                    Point p = pictureBox1.PointToClient(System.Windows.Forms.Cursor.Position);
                    pen = new Pen(Color.Red);
                    graph.DrawLine(pen, Convert.ToSingle(StartInfoX), Convert.ToSingle(StartInfoY), p.X, p.Y);
                    pen = new Pen(Color.Black);
                }
                pictureBox1.Refresh();
            }
        }

        private void vidimost1wall(int x, int y)
        {
            indintersection = false;
            for (int j = 0; j < uglqunt-1; j++)
            {
                double ax1 = x;
                double ay1 = y;
                double ax2 = SatClone[0, beaconqunt - 1];
                double ay2 = SatClone[1, beaconqunt - 1];

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
                    indintersection = true;
                }
            }
            for (int j = 0; j < walluqnt; j+=2)
            {
                double ax1 = x;
                double ay1 = y;
                double ax2 = SatClone[0, beaconqunt - 1];
                double ay2 = SatClone[1, beaconqunt - 1];

                double bx1 = WallPos[0, j];
                double by1 = WallPos[1, j];
                double bx2 = WallPos[0, j + 1];
                double by2 = WallPos[1, j + 1];
                double v1 = (bx2 - bx1) * (ay1 - by1) - (by2 - by1) * (ax1 - bx1);
                double v2 = (bx2 - bx1) * (ay2 - by1) - (by2 - by1) * (ax2 - bx1);
                double v3 = (ax2 - ax1) * (by1 - ay1) - (ay2 - ay1) * (bx1 - ax1);
                double v4 = (ax2 - ax1) * (by2 - ay1) - (ay2 - ay1) * (bx2 - ax1);
                if ((v1 * v2 < 0) && (v3 * v4 < 0))
                {
                    indintersection = true;
                }
            }
            if (indintersection == true)
                kolich = 0;
            else
            {
                indintersection = false;
                for (int i = 0; i < qunt; i++)
                {
                    indintersection = false;
                    double ax1 = x;
                    double ay1 = y;
                    double ax2 = SatPos[0, i];
                    double ay2 = SatPos[1, i];
                    indintersection = false;
                    for (int j = 0; j < uglqunt-1; j++)
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
                            indintersection = true;
                        }
                    }
                    for (int j = 0; j < walluqnt; j+=2)
                    {
                        double bx1 = WallPos[0, j];
                        double by1 = WallPos[1, j];
                        double bx2 = WallPos[0, j + 1];
                        double by2 = WallPos[1, j + 1];
                        double v1 = (bx2 - bx1) * (ay1 - by1) - (by2 - by1) * (ax1 - bx1);
                        double v2 = (bx2 - bx1) * (ay2 - by1) - (by2 - by1) * (ax2 - bx1);
                        double v3 = (ax2 - ax1) * (by1 - ay1) - (ay2 - ay1) * (bx1 - ax1);
                        double v4 = (ax2 - ax1) * (by2 - ay1) - (ay2 - ay1) * (bx2 - ax1);
                        if ((v1 * v2 < 0) && (v3 * v4 < 0))
                        {
                            indintersection = true;
                        }
                    }
                    if (indintersection == false)
                    {
                        rightpoint.Add(new Point() { X = Convert.ToInt32(SatPos[0, i]), Y = Convert.ToInt32(SatPos[1, i]) });
                    }

                }
                kolich = 0;
                foreach (Point pp in rightpoint)
                {
                    kolich += 1;
                }
                SatPos = new double[2, kolich + 2];
                kolich = 0;
                foreach (Point pp in rightpoint)
                {
                    SatPos[0, kolich] = pp.X;
                    SatPos[1, kolich] = pp.Y;
                    kolich += 1;
                }
                SatPos[0, kolich] = SatClone[0, beaconqunt - 1];
                SatPos[1, kolich] = SatClone[1, beaconqunt - 1];
                indintersection = false;
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            button18.Enabled = false;
            pictureBox1.BackgroundImage = null;
            pictureBox1.Refresh();
            pictureBox1.Image = bmp;
            photokard = null;
            newImage = null;
            indphoto = false;
        }

        private void color()
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graph = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;

            button5.Visible = true;
            button6.Visible = true;
            button7.Visible = true;
            button8.Visible = true;

            for (int j = 0; j < pictureBox1.Width; j++)
            {
                for (int l = 0; l < pictureBox1.Height; l++)
                {
                    if (Z[j, l] == 0)
                    {
                        pen = new Pen(Color.FromArgb(255, 255, 255));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 0 && Z[j, l] <= 20)
                    {
                        int test = Convert.ToInt32(Math.Round(Z[j, l], 1) * 10);
                        var pixelColor = new Color();
                        if (201-test > 0 && 201-test <= nn.Width)
                            pixelColor = n.GetPixel(201 - test, 1);
                        pen.Color = pixelColor;
                        graph.DrawEllipse(pen, j, l, 1, 1);

                    }
                    if (Z[j, l] > 20 && Z[j, l] <= 50)
                    {
                        int test2 = Convert.ToInt32(Math.Round(Z[j, l], 1) * 10);
                        var pixelColor1 = new Color();
                        if (test2 - 201 > 0 && test2 - 201 <= nn.Width)
                            pixelColor1 = nn.GetPixel((test2 - 201), 1);
                        pen.Color = pixelColor1;
                        graph.DrawEllipse(pen, j, l, 1, 1);

                    }
                    if (Z[j, l] > 20 && Z[j, l] < 21)
                    {
                        pen.Color = Color.Red;
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] == 50)
                    {
                        pen.Color = Color.Maroon;
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 50 && Z[j, l] < 1000000000)
                    {
                        pen = new Pen(Color.FromArgb(139, 0, 0));
                        graph.DrawEllipse(pen, j, l, 1, 1);
                    }
                    if (Z[j, l] > 1000000000)
                    {
                        if (Z[j + 1, l] == 0 && Z[j - 1, l] == 0 && Z[j, l + 1] == 0 && Z[j, l - 1] == 0)
                        {
                            pen = new Pen(Color.FromArgb(255, 255, 255));
                            graph.DrawEllipse(pen, j, l, 1, 1);
                        }
                    }

                    form.progressBar1.Value += 1;
                }
            }
            button2.Enabled = true;
            if (form.progressBar1.Value == form.progressBar1.Maximum)
            {
                textBox7.Visible = true;
                label7.Visible = true;
                form.Hide();
            }

        }
    }
}
