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
        public string var;
        public string varUrlDB;

        public Color BigPixel;

        private VideoCapture videoPlay;
        private FileConfig ini = new FileConfig("C:\\config_position.ini");

        public bool _canDraw, Box, Line1, Line2, Line3;
        public bool _DrawLine1, _DrawLine2, _DrawLine3;
        public int _startX, _startY, _endX, _endY, _line1Y, _line2Y, _line3Y;
        private Rectangle _rect;

        private Mat _frame = new Mat();
        private bool _captureProcess;

        public int x, y, width, height, widthOfLine;
        public float widthOfVideo, heightOfVideo;

        public Bitmap bitmap;
        private OpenFileDialog openFileDialog01 = new OpenFileDialog();

        private void chooseImageButton_Click(object sender, EventArgs e)
        {
            int size = -1;
            DialogResult result = openFileDialog01.ShowDialog();
            if (result == DialogResult.OK)
            {

                string file = openFileDialog01.FileName;
                Bitmap imgBinary = new Bitmap(file);
                int width = imgBinary.Width;
                int height = imgBinary.Height;
                Color p;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        p = imgBinary.GetPixel(x, y);
                        int a = p.A;
                        int r = p.R;
                        int g = p.G;
                        int b = p.B;
                        int avg = (int)(r + g + b) / 3;
                        avg = avg < 128 ? 0 : 255;
                        imgBinary.SetPixel(x, y, Color.FromArgb(a, r, avg, avg));
                    }
                }
                pictureBox1.Image = imgBinary;
                //pictureBox1.Size = new System.Drawing.Size(1200, 1200);
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;

            }
            Console.WriteLine(size);
            Console.WriteLine(result);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        public int x1, y1;

        public Form1()
        {
            InitializeComponent();
            CvInvoke.UseOpenCL = false;

        }

        private void Process(object sender, EventArgs e)
        {
            if (videoPlay != null && videoPlay.Ptr != IntPtr.Zero)
            {
                videoPlay.Retrieve(_frame, 0);
                pictureBox1.Image = _frame.Bitmap;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                videoPlay.ImageGrabbed += Process;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (videoPlay != null)
            {
                if (_captureProcess)
                {
                    button1.Text = "Start Capture";
                    videoPlay.Pause();
                }
                else
                {
                    button1.Text = "Stop Capture";
                    videoPlay.Start();
                }
                _captureProcess = !_captureProcess;
            }

        }

        /* This function is update of video play location of video 
         * and function is will be send this update to button1_Click about event. */
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var = textBox1.Text;
            videoPlay = new VideoCapture(var);
            if (videoPlay != null && videoPlay.Ptr != IntPtr.Zero)
            {
                videoPlay.Retrieve(_frame, 0);
                pictureBox1.Image = _frame.Bitmap;
                widthOfVideo = _frame.Size.Width;
                heightOfVideo = _frame.Size.Height;
            }
        }
        /*************************************************************/

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
        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileIni = new SaveFileDialog();
            saveFileIni.Filter = "|*.ini";
            saveFileIni.Title = "Save an .ini file";
            saveFileIni.ShowDialog();
            ini.FilePath = saveFileIni.FileName;
            varUrlDB = textBox2.Text;

            if (saveFileIni.FileName != "")
            {
                ini.Write("Position rectangle", "X1", Convert.ToString(Math.Min(_startX - _startX, _startX - _startX)));
                ini.Write("Position rectangle", "Y1", Convert.ToString(Math.Min(_startY - _startY, _startY - _startY)));

                ini.Write("Position rectangle", "X2", Convert.ToString(Math.Max(_endX - _startX, _endX - _startX)));
                ini.Write("Position rectangle", "Y2", Convert.ToString(Math.Max(_endY - _startY, _endY - _startY)));

                ini.Write("Position Line1", "X1", Convert.ToString(_startX - _startX));
                ini.Write("Position Line1", "Y1", Convert.ToString(_line1Y - _startY));
                ini.Write("Position Line1", "X2", Convert.ToString(widthOfLine - _startX));
                ini.Write("Position Line1", "Y2", Convert.ToString(_line1Y - _startY));

                ini.Write("Position Line2", "X1", Convert.ToString(_startX - _startX));
                ini.Write("Position Line2", "Y1", Convert.ToString(_line2Y - _startY));
                ini.Write("Position Line2", "X2", Convert.ToString(widthOfLine - _startX));
                ini.Write("Position Line2", "Y2", Convert.ToString(_line2Y - _startY));

                ini.Write("Position Line3", "X1", Convert.ToString(_startX - _startX));
                ini.Write("Position Line3", "Y1", Convert.ToString(_line3Y - _startY));
                ini.Write("Position Line3", "X2", Convert.ToString(widthOfLine - _startX));
                ini.Write("Position Line3", "Y2", Convert.ToString(_line3Y - _startY));

                if (varUrlDB != "")
                {
                    ini.Write("URL Databse", "URL", varUrlDB);
                }
                else
                {
                    ini.Write("URL Databse", "URL", "No URL Database");
                }

            }
        }
        private void pictureBox1_paint(object sender, PaintEventArgs e)
        {
            ///////Update Box paint////////////////////////////////////////////////
            using (Pen pen = new Pen(Color.Blue, 5))
            {
                e.Graphics.DrawRectangle(pen, _rect);
            }
            //////////////////////////////////////////////////////////////////////

            ///////Update Line1 paint////////////////////////////////////////////
            using (Pen pen = new Pen(Color.Red, 5))
            {
                e.Graphics.DrawLine(pen, _startX, _line1Y, widthOfLine, _line1Y);
                ini.Write("Position Line1", "X1", Convert.ToString(_startX));
                ini.Write("Position Line1", "Y1", Convert.ToString(_line1Y));
                ini.Write("Position Line1", "X2", Convert.ToString(widthOfLine));
                ini.Write("Position Line1", "Y2", Convert.ToString(_line1Y));
            }
            //////////////////////////////////////////////////////////////////////

            ///////Update Line2 paint/////////////////////////////////////////////
            using (Pen pen = new Pen(Color.Green, 5))
            {
                e.Graphics.DrawLine(pen, _startX, _line2Y, widthOfLine, _line2Y);
                ini.Write("Position Line2", "X1", Convert.ToString(_startX));
                ini.Write("Position Line2", "Y1", Convert.ToString(_line2Y));
                ini.Write("Position Line2", "X2", Convert.ToString(widthOfLine));
                ini.Write("Position Line2", "Y2", Convert.ToString(_line2Y));
            }
            //////////////////////////////////////////////////////////////////////

            ///////Update Line3 paint/////////////////////////////////////////////
            using (Pen pen = new Pen(Color.Azure, 5))
            {
                e.Graphics.DrawLine(pen, _startX, _line3Y, widthOfLine, _line3Y);
                ini.Write("Position Line3", "X1", Convert.ToString(_startX));
                ini.Write("Position Line3", "Y1", Convert.ToString(_line3Y));
                ini.Write("Position Line3", "X2", Convert.ToString(widthOfLine));
                ini.Write("Position Line3", "Y2", Convert.ToString(_line3Y));
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


            Refresh();
            label1.Text = "" + Math.Min(_startX, e.X) + ", " + Math.Min(_startY, e.Y);
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

        private void mousePointColor(object sender, MouseEventArgs e)
        {
            double windowX = e.X;
            double windowY = e.Y;
            double controlWidth = Width;
            double controlHeight = Height;
            double imageWidth = bitmap.Width;
            double imageHeight = bitmap.Height;

            bitmap = new Bitmap(var);

            Color pixel = bitmap.GetPixel(
                   (int)(windowX * bitmap.Width / controlWidth),
                   (int)(windowY * bitmap.Height / controlHeight));
            System.Diagnostics.Debug.WriteLine(pixel);

            Console.WriteLine(pixel);
        }
    }
}
