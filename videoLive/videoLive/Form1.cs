using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;

namespace videoLive
{
    public partial class Form1 : Form
    {
        MJPEGStream stream;
        string Link = "";
        int x = 0, y = 0;
        

        public Form1()
        {

            
            InitializeComponent();     
            
            //stream.NewFrame += Stream_NewFrame;
        }

        private void Stream_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bmp = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = bmp;
        }

        private void btnStartLive_Click(object sender, EventArgs e)
        {
            Link = txtUrl.Text;
            stream = new MJPEGStream(Link);
            stream.NewFrame += Stream_NewFrame;
            stream.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Link = txtUrl.Text;
            stream = new MJPEGStream(Link);
            stream.NewFrame += Stream_NewFrame;
            stream.Stop();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = 0, y = 0;
            if(label3.Text == "start")
            {
                label3.Text = "" + e.X + ";" + "" + e.Y;
            }
            else if (label3.Text != "start" && x < e.X && y < e.Y)
            {
                label3.Text = "" + e.X + ";" + "" + e.Y;
            }
        }

        private void btn_Box(object sender, EventArgs e)
        {
            label3.Text = "start";

        }

        private void pictureBox1_paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(
                new Pen(Color.Blue, 2f),
                new Point(0, 0),
                new Point(pictureBox1.Size.Width, pictureBox1.Size.Height));
            e.Graphics.DrawEllipse(
                new Pen(Color.Red, 2f),
                0, 0, pictureBox1.Size.Width, pictureBox1.Size.Height);

            e.Graphics.DrawRectangle(new Pen(Brushes.White), 30, 30, 40, 40);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Graphics g = CreateGraphics();
            Pen p = new Pen(Color.Navy);
            Pen erase = new Pen(Color.White);
            g.DrawLine(erase, 0, 0, x, y);
            x = e.X; y = e.Y;
            g.DrawLine(p, 0, 0, x, y);
            label1.Location = new Point(x - label1.Width, y);
            label2.Location = new Point(x, y - label2.Height);
            label1.Text = x.ToString(); label2.Text = y.ToString();
        }

    }
}
