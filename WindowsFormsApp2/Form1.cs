using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private Point MouseDownPoint = Point.Empty;
        private bool CatchFinished = false;
        private bool CatchStart = false;
        private Bitmap originBmp;
        private Rectangle CatchRect;
        private void Form1_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer| ControlStyles.AllPaintingInWmPaint|ControlStyles.UserPaint,true);
            this.UpdateStyles();
            originBmp = new Bitmap(this.BackgroundImage);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!CatchStart)
                {
                    CatchStart = true;
                    MouseDownPoint = new Point(e.X,e.Y);
                }
            }
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (CatchStart)
            {
                Bitmap destmap = (Bitmap)originBmp.Clone();
                Point newpoint = new Point(MouseDownPoint.X,MouseDownPoint.Y);
                Graphics g = Graphics.FromImage(destmap);
                Pen p = new Pen(Color.Green,2);
                int width = Math.Abs(e.X - MouseDownPoint.X);
                int height = Math.Abs(e.Y - MouseDownPoint.Y);
                if (e.X < MouseDownPoint.X)
                {
                    newpoint.X = e.X;

                }
                if (e.Y < MouseDownPoint.Y)
                {
                    newpoint.Y = e.Y;
                }
                CatchRect = new Rectangle(newpoint, new Size(width,height));
                g.DrawRectangle(p,CatchRect);
                g.Dispose();
                p.Dispose();
                Graphics g1 = this.CreateGraphics();
                g1 = this.CreateGraphics();
                g1.DrawImage(destmap, new PointF(0,0));
                g1.Dispose();
                destmap.Dispose();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (CatchStart)
                {
                    CatchStart = false;
                    CatchFinished = true;
                }
            }
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && CatchFinished)
            {
                if (CatchRect.Contains(new Point(e.X, e.Y)))
                {
                    Bitmap CatchedBmp = new Bitmap(CatchRect.Width, CatchRect.Height);//新建一个于矩形等大的空白图片
                    Graphics g = Graphics.FromImage(CatchedBmp);
                    g.DrawImage(originBmp, new Rectangle(0, 0, CatchRect.Width, CatchRect.Height), CatchRect, GraphicsUnit.Pixel);
                    //把orginBmp中的指定部分按照指定大小画在画板上
                    Clipboard.SetImage(CatchedBmp);//将图片保存到剪贴板
                    g.Dispose();
                    CatchFinished = false;
                    this.BackgroundImage = originBmp;
                    CatchedBmp.Dispose();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }
    }
}
