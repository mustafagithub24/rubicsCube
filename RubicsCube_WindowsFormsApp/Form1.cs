using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RubicsCube_WindowsFormsApp
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			r = new RubicsCube();
			g = pictureBox1.CreateGraphics();
            rotateLayerOrCube = false;
            r.Draw(pictureBox1);
            currentSquare = new Square(new Point3D(1, 1, 1), new Point3D(1.5, 1, 1), Constants.Plane.YZ, Color.Red);            
        }
		Graphics g;
		RubicsCube r;
		int counter;
		Constants.Axis axis;
		bool isClockwise;
		Square currentSquare;
        bool rotateLayerOrCube; // true for Layer rotation, false for Cube rotation
        Point mouseStartPoint;
        #region Buttons
        // Layer
        // X+
        private void button1_Click(object sender, EventArgs e)
        {
            rotateLayerOrCube = true;
            axis = Constants.Axis.X;
            isClockwise = true;
            timer1.Enabled = true;
        }
        // X-
        private void button2_Click(object sender, EventArgs e)
        {
            rotateLayerOrCube = true;
            axis = Constants.Axis.X;
            isClockwise = false;
            timer1.Enabled = true;
        }
        // Y+
        private void button4_Click(object sender, EventArgs e)
        {
            rotateLayerOrCube = true;
            axis = Constants.Axis.Y;
            isClockwise = true;
            timer1.Enabled = true;
        }
        // Y-
        private void button3_Click(object sender, EventArgs e)
        {
            rotateLayerOrCube = true;
            axis = Constants.Axis.Y;
            isClockwise = false;
            timer1.Enabled = true;
        }
        // Z+
        private void button6_Click(object sender, EventArgs e)
        {
            rotateLayerOrCube = true;
            axis = Constants.Axis.Z;
            isClockwise = true;
            timer1.Enabled = true;
        }
        // Z-
        private void button5_Click(object sender, EventArgs e)
        {
            rotateLayerOrCube = true;
            axis = Constants.Axis.Z;
            isClockwise = false;
            timer1.Enabled = true;
        }
        // Cube
        // X+
        private void button12_Click(object sender, EventArgs e)
        {
            rotateLayerOrCube = false;
            axis = Constants.Axis.X;
            isClockwise = true;
            timer1.Enabled = true;
        }
        // X-
        private void button11_Click(object sender, EventArgs e)
        {
            rotateLayerOrCube = false;
            axis = Constants.Axis.X;
            isClockwise = false;
            timer1.Enabled = true;
        }
        // Y+
        private void button10_Click(object sender, EventArgs e)
        {
            rotateLayerOrCube = false;
            axis = Constants.Axis.Y;
            isClockwise = true;
            timer1.Enabled = true;
        }
        // Y-
        private void button9_Click(object sender, EventArgs e)
        {
            rotateLayerOrCube = false;
            axis = Constants.Axis.Y;
            isClockwise = false;
            timer1.Enabled = true;
        }
        // Z+
        private void button8_Click(object sender, EventArgs e)
        {
            rotateLayerOrCube = false;
            axis = Constants.Axis.Z;
            isClockwise = true;
            timer1.Enabled = true;
        }
        // Z-
        private void button7_Click(object sender, EventArgs e)
        {
            rotateLayerOrCube = false;
            axis = Constants.Axis.Z;
            isClockwise = false;
            timer1.Enabled = true;
        }
        #endregion



        private void timer1_Tick(object sender, EventArgs e)
		{
            if (rotateLayerOrCube)
                r.RotateLayer(currentSquare.cubeCenter, axis, isClockwise, pictureBox1);
            else
                r.RotateCube(axis, isClockwise, pictureBox1);

            counter++;

			if (counter == 18)
			{
				counter = 0;
				timer1.Enabled = false;
            }
		}

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Tuple<bool, Constants.Axis> rotationAndAxis = Constants.RotationAndAxis(mouseStartPoint, new Point(e.X, e.Y), currentSquare);
            if (rotationAndAxis != null)
            {
                rotateLayerOrCube = true;
                isClockwise = rotationAndAxis.Item1;
                axis = rotationAndAxis.Item2;
                timer1.Enabled = true;
            }            
        }        

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseStartPoint = new Point(e.X, e.Y);            

            Square[] squares = r.SquaresOnScreen();

            for (int i = 0; i < squares.Length; i++)
            {
                if (squares[i].IsMouseInside(new Point(e.X, e.Y)))
                {
                    if (currentSquare != null && squares.Contains(currentSquare))
                    {
                        currentSquare.Draw(g);
                    }
                    currentSquare = squares[i];
                    currentSquare.Draw(g, true);
                    return;
                }
            }
            if (currentSquare != null)
            {
                currentSquare.Draw(g);
            }
            currentSquare = null;
        }
    }
}
