using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            pentool = new Pen(Color.Black,1);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        public int PenWith { get; set; }
        public Color PenColor { get; set; }
        private Pen pentool;
        Graphics gra;
        public Pen PenTool
        {
            get
            {
                pentool.Color = PenColor;
                pentool.Width = PenWith;
                return pentool;
            }
            
        }
        public Point StartPos = Point.Empty;
        public Point EndPos = Point.Empty;
        public Rectangle rectangle = new Rectangle();
        private Rectangle rect;

        public Rectangle Rectangle
        {
            get
            {
                rect.Location = new Point(StartPos.X,StartPos.Y);
                rect.Width = Math.Abs(StartPos.X - EndPos.X);
                rect.Height = Math.Abs(StartPos.Y - EndPos.Y);
                return rect;
            }
            
        }

        public bool IsStarted { get; set; }
        public bool IsFinished { get; set; }
        public int WhichLine { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            //if (bCatch_HideCurrent.Checked)
            {
                this.Hide();//隐藏当前窗体
                Thread.Sleep(50);//让线程睡眠一段时间，窗体消失需要一点时间
                Form1 CatchForm = new Form1();
                Bitmap CatchBmp = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);//新建一个和屏幕大小相同的图片         
                Graphics g = Graphics.FromImage(CatchBmp);
                g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height));//保存全屏图片
                CatchForm.BackgroundImage = CatchBmp;//将Catch窗体的背景设为全屏时的图片
                if (CatchForm.ShowDialog()== DialogResult.OK)
                {//如果Catch窗体结束,就将剪贴板中的图片放到信息发送框中
                    IDataObject iData = Clipboard.GetDataObject();
                    DataFormats.Format myFormat = DataFormats.GetFormat(DataFormats.Bitmap);
                    if (iData.GetDataPresent(DataFormats.Bitmap))
                    {
                        pictureBox1.Image = Clipboard.GetImage();
                        Clipboard.Clear();//清除剪贴板中的对象
                    }
                    this.Show();//重新显示窗体
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            MouseDown = false;
            IsStarted = false;
            IsFinished = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    PenColor = Color.Black;
                    break;
                case 1:
                    PenColor = Color.Blue;
                    break;
                case 2:
                    PenColor = Color.Green;
                    break;
                case 3:
                    PenColor = Color.Red;
                    break;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            PenWith = comboBox2.SelectedIndex * 3 + 1;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            IsStarted = true;
            WhichLine = 0;
            IsFinished = false;
            MouseDown = false;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            gra = pictureBox1.CreateGraphics();
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            
        }
        public bool MouseDown { get; set; }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (IsStarted)
            {
                StartPos = e.Location;
                MouseDown = true;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsStarted)
            {
                EndPos = e.Location;
                IsStarted = false;
                IsFinished = true;
            }
            if (IsFinished)
            {
                try
                {
                    switch (WhichLine)
                    {
                        case 0:
                            gra.DrawLine(PenTool, StartPos, EndPos);
                            break;
                        case 1:
                            gra.DrawRectangle(PenTool, Rectangle);
                            break;
                        case 2:
                            gra.DrawEllipse(PenTool, Rectangle);
                            break;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            IsStarted = true;
            IsFinished = false;
            WhichLine = 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IsStarted = true;
            IsFinished = false;
            WhichLine = 2;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

            if (IsStarted && WhichLine == 0 && MouseDown)
            {
                gra.DrawLine(PenTool, StartPos, e.Location);
                StartPos = e.Location;
            }
        }
    }
}
