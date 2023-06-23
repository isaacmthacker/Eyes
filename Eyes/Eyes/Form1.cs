using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eyes
{
    public partial class Form1 : Form
    {
        List<Eye> eyes = new List<Eye>();
        public Form1()
        {
            DoubleBuffered = true;
            InitializeComponent();

            int numRows = 20;
            float w = ClientSize.Width / (float)numRows;
            float h = w * 0.55f;
            int numCols = ClientSize.Height / numRows;

            //Add even buffer to eyes in window
            float adj = (ClientSize.Height % h) / 2.0f;
            
            for(int i = 0; i < numRows; i++)
            {
                float yPos = i*h + (h / 2.0f) + adj;
                for (int j = 0; j < numCols; j++) {
                    float xPos = j * w + (w / 2.0f);
                    eyes.Add(new Eye(xPos, yPos, w, h));
                }
            }
        }

        public void EyeWatching_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, ClientSize.Width, ClientSize.Height));
            foreach(Eye eye in eyes)
            {
                eye.Draw(g);
            }
        }
        public void EyeWatching_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (Eye eye in eyes)
            {
                eye.Move(e.X, e.Y);
            }
            Invalidate();
        }
    }
}
