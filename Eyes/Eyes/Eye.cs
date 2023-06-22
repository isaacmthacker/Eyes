using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eyes
{
    internal class Eye
    {
        float x, y;
        float width, height;
        //Radii for w and h respectively 
        float rw, rh;
        float pupil_x, pupil_y;
        float pupil_size;

        float maxPupilX = 0;
        float minPupilX = 0;

        float maxPupilY = 0;
        float minPupilY = 0;

        double ex, ey;
        float lastMouseX, lastMouseY = 0;


        double ellipseDrawangle = 0;
        double focalPointAtZero;


        public Eye(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;

            rw = this.width / 2.0f;
            rh = this.height / 2.0f;

            pupil_x = x;
            pupil_y = y;
            lastMouseX = x;
            lastMouseY = y;
            pupil_size = this.height / 2.0f;

            focalPointAtZero = Math.Sqrt(rw * rw - rh * rh);

        }

        public double RadToDegrees(double angle)
        {
            return 180 * (angle / Math.PI);
        }

        public void Draw(Graphics g)
        {

            //Ellipse
            g.FillEllipse(Brushes.Blue, new RectangleF(x - rw, y - rh, width, height));


            //Pupil
            g.DrawEllipse(new Pen(Brushes.Black), new RectangleF(pupil_x - pupil_size / 2.0f, pupil_y - pupil_size / 2.0f, pupil_size, pupil_size));
            //Center of pupil
            g.DrawEllipse(new Pen(Brushes.Black), new RectangleF(pupil_x - 5, pupil_y - 5, 10, 10));

            //Center
            g.FillEllipse(Brushes.Black, new RectangleF(new PointF(x - 5, y - 5), new SizeF(10, 10)));
            //Line to pupil center
            g.DrawLine(new Pen(Brushes.Black), new PointF(x, y), new PointF(pupil_x, pupil_y));


            //Point on ellipse
            g.FillEllipse(Brushes.Red, new RectangleF(new PointF((float)ex - 5, (float)ey - 5), new SizeF(10, 10)));

            //Getting point on the ellipse
            EllipsePoint(ellipseDrawangle, g);

            //Line to ellipse point
            g.DrawLine(new Pen(Brushes.Red), x, y, (float)ex, (float)ey);

            //Axis
            g.DrawLine(new Pen(Brushes.Red), x-rw, y, x+rw,y);
            g.DrawLine(new Pen(Brushes.Red), x, y-rh, x, y+rh);


            //Focal points
            g.DrawEllipse(new Pen(Brushes.Red), new RectangleF((float)focalPointAtZero-5 + x, y-5, 10, 10));
            g.DrawEllipse(new Pen(Brushes.Red), new RectangleF(-(float)focalPointAtZero - 5 + x, y - 5, 10, 10));

            //todo: use arcs again at some point


        }

        public bool Intersect(int mousex, int mousey)
        {
            //check if pupil intersects with inner ellipse
            double posFoc = focalPointAtZero + x;
            double negFoc = x - focalPointAtZero;

            double distPos = Math.Sqrt((mousex - posFoc) * (mousex - posFoc) + (mousey - y) * (mousey - y));
            double distNeg = Math.Sqrt((mousex - negFoc) * (mousex - negFoc) + (mousey - y) * (mousey - y));

            double sum = distPos + distNeg;


            return sum <= (2 * rw);

        }

        public void EllipsePoint(double angle, Graphics g)
        {
        }

        public void Move(int mousex, int mousey)
        {

            double angle;

            bool left = mousex < x;
            bool top = mousey < y;

            if (left)
            {
                if (top)
                {
                    angle = -(Math.PI - Math.Abs(Math.Atan((y - mousey) / (x - mousex))));
                    Console.WriteLine(RadToDegrees(angle));
                }
                else
                {
                    angle = -(Math.PI - Math.Atan((mousey - y) / (mousex - x)));
                    Console.WriteLine(RadToDegrees(angle));
                }
            }
            else
            {
                if (top)
                {
                    angle = -Math.Atan((mousey - y) / (x - mousex));
                    Console.WriteLine(RadToDegrees(angle));
                }
                else
                {
                    angle = Math.Atan((y - mousey) / (x - mousex));
                    Console.WriteLine(RadToDegrees(angle));
                }
            }

            ellipseDrawangle = angle;

            double r = Math.Sqrt((rw*rw*rh*rh)/(rh*rh*Math.Cos(angle)*Math.Cos(angle) + rw*rw*Math.Sin(angle)*Math.Sin(angle)));

            ex = r * Math.Cos(angle) + x;
            ey = r*Math.Sin(angle) + y;


            if(Intersect(mousex, mousey))
            {
                Console.WriteLine("!!!!!!!!!!!!!!!!!!Mouse on ellipse");
                pupil_x = mousex;
                pupil_y = mousey;
            } else
            {
                Console.WriteLine("Mouse NOT on ellipse");
                pupil_x = (float)ex;
                pupil_y = (float)ey;
            }



        }

    }
}
