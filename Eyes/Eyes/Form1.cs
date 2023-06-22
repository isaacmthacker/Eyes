using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eyes
{
    public partial class Form1 : Form
    {
        Eye eye;
        public Form1()
        {
            DoubleBuffered = true;
            InitializeComponent();
            float w = 450;
            float h = w * 0.55f;
            eye = new Eye(ClientSize.Width / 2.0f, ClientSize.Height / 2.0f, w,h);
        }

        public void EyeWatching_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, ClientSize.Width, ClientSize.Height));
            eye.Draw(g);
        }
        public void EyeWatching_MouseMove(object sender, MouseEventArgs e)
        {
            eye.Move(e.X, e.Y);
            Invalidate();
        }
    }
}
