using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace gui_client_00
{
    public partial class Form1 : Form
    {
        string var;
        VideoCapture videoPlay;

        Image<Bgr, byte> imgInput;

        private bool _canDraw, checkBox;
        private int _startX, _startY;
        private Rectangle _rect;


        public int x, y, width, height;

        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                while (true)
                {
                    Mat m = new Mat();
                    videoPlay.Read(m);

                    if (!m.IsEmpty)
                    {
                        pictureBox1.Image = m.Bitmap;
                        double fps = videoPlay.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
                        await Task.Delay(1000 / Convert.ToInt32(fps));
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            var = textBox1.Text;
            //imgInput = new Image<Bgr, byte>(var);
            videoPlay = new VideoCapture(var);
            Mat m = new Mat();

            videoPlay.Read(m);
            pictureBox1.Image = m.Bitmap;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkBox = true;
        }

        private void pictureBox1_paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Blue, 5))
            {
                e.Graphics.DrawRectangle(pen, _rect);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (checkBox == true)
                _canDraw = true;
                _startX = e.X;
                _startY = e.Y;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_canDraw) return;
            x = Math.Min(_startX, e.X);
            y = Math.Min(_startY, e.Y);

            width = Math.Max(_startX, e.X) - Math.Min(_startX, e.X);
            height = Math.Max(_startY, e.Y) - Math.Min(_startY, e.Y);

            _rect = new Rectangle(x, y, width, height);
            Refresh();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _canDraw = false;
        }

    }
}
