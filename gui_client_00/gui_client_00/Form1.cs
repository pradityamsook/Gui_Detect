using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        private FileConfig ini = new FileConfig(@"C:\Users\Leon\Documents\_config\config_position.ini");

        private bool _canDraw, Box, Line1, Line2, Line3; 
        public bool _DrawLine1, _DrawLine2, _DrawLine3;
        private int _startX, _startY, _endX, _endY, _line1Y, _line2Y, _line3Y;
        private Rectangle _rect;
       

        public int x, y, width, height, widthOfLine;

        

        public int x1, y1;

        

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

        /* This function is update of video play location of video and will send this update to button1_Click about event*/
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            var = textBox1.Text;
            //imgInput = new Image<Bgr, byte>(var);
            videoPlay = new VideoCapture(var);
            Mat m = new Mat();

            videoPlay.Read(m);
            pictureBox1.Image = m.Bitmap;
        }
        /*****************************************************************************************************************/

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /////////////// This function is Box //////////////////
        private void button2_Click(object sender, EventArgs e)
        {
            Box = true;
        }
        ///////////////////////////////////////////////////////

        ////////////// This is function is Line1 //////////////
        private void button3_Click(object sender, EventArgs e)
        {
            Line1 = true;
            Box = false;
            Line2 = false;
        }
        ///////////////////////////////////////////////////////

        //////////////// This function is Line2 ///////////////
        private void button4_Click(object sender, EventArgs e)
        {
            Line2 = true;
            Box = false;
            Line1 = false;
        }
        ///////////////////////////////////////////////////////

        ///////////// This function is Line3 //////////////////
        private void button5_Click(object sender, EventArgs e)
        {
            Line3 = true;
            Box = false;
            Line1 = false;
            Line2 = false;
        }
        ///////////////////////////////////////////////////////

        private void pictureBox1_paint(object sender, PaintEventArgs e)
        {
            ///////////////////////////////////////////////////////////////////////
            using (Pen pen = new Pen(Color.Blue, 5))
            {
                e.Graphics.DrawRectangle(pen, _rect);
            }
            //////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////
            using (Pen pen = new Pen(Color.Red, 5))
            {
                e.Graphics.DrawLine(pen, _startX, _line1Y, widthOfLine, _line1Y);
            }
            //////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////
            using (Pen pen = new Pen(Color.Green, 5))
            {
                e.Graphics.DrawLine(pen, _startX, _line2Y, widthOfLine, _line2Y);
            }
            //////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////
            using (Pen pen = new Pen(Color.Azure, 5))
            {
                e.Graphics.DrawLine(pen, _startX, _line3Y, widthOfLine, _line3Y);
            }
            //////////////////////////////////////////////////////////////////////
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (Box)
            {
                _canDraw = true;
                _startX = e.X;
                _startY = e.Y;

                ini.Write("Position rectangle", "X1", Convert.ToString(Math.Min(_startX, e.X)));
                ini.Write("Position rectangle", "Y1", Convert.ToString(Math.Min(_startY, e.Y)));
            }
            else if (Line1)
            {
                _DrawLine1 = true;
                _line1Y = e.Y;

            }
            else if (Line2)
            {
                _DrawLine2 = true;
                _line2Y = e.Y;
            }
            else if (Line3)
            {
                _DrawLine3 = true;
                _line3Y = e.Y;
            }

            label1.Text = "" + Math.Min(_startX, e.X) + ", " + Math.Min(_startY, e.Y);
            Refresh();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Box)
            {
                if (!_canDraw) return;
                x = Math.Min(_startX, e.X);
                y = Math.Min(_startY, e.Y);

                width = Math.Max(_startX, e.X) - Math.Min(_startX, e.X);
                widthOfLine = Math.Max(_startX, e.X);
                height = Math.Max(_startY, e.Y) - Math.Min(_startY, e.Y);

                _rect = new Rectangle(x, y, width, height);
                Refresh();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (Box)
            {
                _canDraw = false;
                if (_canDraw == false)
                {
                    ini.Write("Position rectangle", "X2", Convert.ToString(Math.Max(_endX, e.X)));
                    ini.Write("Position rectangle", "Y2", Convert.ToString(Math.Max(_endY, e.Y)));
                }
                _DrawLine2 = false;

                _endX = e.X;
                _endY = e.Y;


                label1.Text = "" + Math.Max(_endX, e.X) + "," + Math.Max(_endY, e.Y);
            }
        }
      
    }
}
