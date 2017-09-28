// Alexander Lao
// 11481444
// 4/21/2017
// CptS 321 - HW13 - Orbiting Planets Animation

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace HW13_Alexander_Lao
{
    public partial class Form1 : Form
    {
        private List<Rectangle> staticCircles = new List<Rectangle>();
        private List<Rectangle> dynamicCircles = new List<Rectangle>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // set the background color of the picture box
            pictureBox1.BackColor = Color.LightGray;

            // check the gravity radio button as the default
            gravityRadioButton.Checked = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Size.Width - 2,
                                       pictureBox1.Size.Height - 2,
                                       System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // save a copy of the old image and free it
            // set the new image
            Image old = pictureBox1.Image;
            pictureBox1.Image = bitmap;

            if (old != null)
            {
                old.Dispose();
            }

            Render();

            // get the x and y position of the click
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;
            int x = coordinates.X;
            int y = coordinates.Y;

            // check if we're creating a gravity circle or a planet circle
            if (gravityRadioButton.Checked == true)
            {
                // instantiate a new rectangle to draw the static circle
                int radius = (int)radiusUpDown.Value;
                Rectangle newStatic = new Rectangle(x - radius, y - radius, radius * 2, radius * 2);
                DrawStatic(newStatic);

                // add the rectangle of the static circle to the list of static objects
                this.staticCircles.Add(newStatic);
            }
            else if (planetRadioButton.Checked == true)
            {
                // instantiate a new rectangle to draw the dynamic circle
                // give these circles a fixed radius of 5
                int radius = 5;
                Rectangle newDynamic = new Rectangle(x - radius, y - radius, radius * 2, radius * 2);
                DrawDynamic(newDynamic);

                // add the coordinates to the list of dynamic objects
                this.dynamicCircles.Add(newDynamic);
            }

            // notify the picture box that its image has been updated
            pictureBox1.Invalidate();
        }

        // draws a static object
        private void DrawStatic(Rectangle newRectangle)
        {
            using (Graphics g = Graphics.FromImage(pictureBox1.Image))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;

                // draw the outer circle
                using (Brush b = new SolidBrush(Color.Gray))
                {
                    g.FillEllipse(b, newRectangle);
                }

                // draw the inner circle
                using (Brush b = new SolidBrush(Color.Black))
                {
                    float newRadius = 5;
                    int oldRadius = newRectangle.Width / 2;
                    int oldX = newRectangle.X + oldRadius;
                    int oldY = newRectangle.Y + oldRadius;
                    g.FillEllipse(b, oldX - newRadius, oldY - newRadius, newRadius * 2, newRadius * 2);
                }
            }
        }

        // draws a dynamic object
        private void DrawDynamic(Rectangle newRectangle)
        {
            using (Graphics g = Graphics.FromImage(pictureBox1.Image))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;

                using (Brush b = new SolidBrush(Color.Red))
                {
                    g.FillEllipse(b, newRectangle);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Render();
        }

        private void Render()
        {
            // re-draw each static and dynamic object
            foreach (Rectangle staticCircle in staticCircles)
            {
                DrawStatic(staticCircle);
            }

            foreach (Rectangle dynamicCircle in dynamicCircles)
            {
                DrawDynamic(dynamicCircle);
            }

            // find the closest static circle with respect to each dynamic circle
            for (var i = 0; i < dynamicCircles.Count; i++)
            {
                foreach (Rectangle staticCircle in staticCircles)
                {
                    if (dynamicCircles[i].IntersectsWith(staticCircle))
                    {
                        // calculate the rotation
                        // algorithm from http://stackoverflow.com/questions/786472

                        // store the planet we want to rotate in a local variable
                        Rectangle current = dynamicCircles[i];

                        // retrieve x and y of the center of the static circle
                        // subtract 5 for the radius of the inner circle of the static circle
                        int staticX = staticCircle.X + (staticCircle.Width / 2) - 5;
                        int staticY = staticCircle.Y + (staticCircle.Height / 2) - 5;

                        // define the rotation angle and convert it to radians
                        double rotation = 5;
                        double rotationRadians = rotation * (Math.PI / 180);

                        double newX = Math.Cos(rotationRadians) * (current.X - staticX) -
                                      Math.Sin(rotationRadians) * (current.Y - staticY) + staticX;
                        double newY = Math.Sin(rotationRadians) * (current.X - staticX) +
                                      Math.Cos(rotationRadians) * (current.Y - staticY) + staticY;

                        // instantiate a new rectangle object with the new x and y coordinates
                        Rectangle updatedRectangle = new Rectangle((int)newX, (int)newY, current.Width, current.Height);

                        // update the rectangle in the list
                        dynamicCircles[i] = updatedRectangle;

                        break;
                    }
                }

                pictureBox1.Invalidate();
            }
        }
    }
}