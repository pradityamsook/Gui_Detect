using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace video
{
    public partial class Form1 : Form
    {
        private bool _canDraw;
        private int _startX, _startY;
        private Rectangle _rect;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.URL = "" + textBox1.Text;
        }

        private void Form1_MouseDown(object sender, AxWMPLib._WMPOCXEvents_MouseDownEvent e)
        {
            _canDraw = true;
            _startX = e.fX;
            _startY = e.fY;
        }

        private void Form1_MouseMove(object sender, AxWMPLib._WMPOCXEvents_MouseMoveEvent e)
        {
            if (!_canDraw) return;

            int x = Math.Min(_startX, e.fX);
            int y = Math.Min(_startY, e.fY);

            int width = Math.Min(_startX, e.fX) - Math.Min(_startX, e.fX);
            int height = Math.Min(_startY, e.fY) - Math.Min(_startY, e.fY);
            _rect = new Rectangle(x, y, width, height);

            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Red, 2))
            {
                e.Graphics.DrawRectangle(pen, _rect);
            }
        }

        private void Form1_MouseUp(object sender, AxWMPLib._WMPOCXEvents_MouseUpEvent e)
        {
            _canDraw = false;
        }
    }
}
