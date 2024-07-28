using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RubicsCube_WindowsFormsApp.Constants;

namespace RubicsCube_WindowsFormsApp
{
	internal class RubicsCube
	{
		private Cube[,,] cubes;
        private Layer[] layers;
		private Constants.Axis axis;
		private int rotationCount;

		public RubicsCube() 
		{
            cubes = new Cube[3, 3, 3];
			layers = new Layer[3];

			for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    for (int k = -1; k <= 1; k++)
                    {
						cubes[i + 1, j + 1, k + 1] = new Cube(new Point3D(k, j, i)); 
                    }
                }
            }
			Divide(Constants.Axis.Z);
        }

        public void Draw(PictureBox canvas)
        {
			canvas.BackgroundImage = DrawBitmap();
		}

        private Bitmap DrawBitmap()
        {
            Bitmap bitmap = new Bitmap(Constants.CanvasSize.Width, Constants.CanvasSize.Height);
            Graphics g = Graphics.FromImage(bitmap);
            Draw(g);

            return bitmap;
        }

        public void Draw(Graphics g)
        {
            for (int i = 0; i < 3; i++)
            {
                layers[i].Draw(g);
            }
        }

        private void Divide(Constants.Axis axis)
        {
			this.axis = axis;

            switch (axis)
            {
                case Constants.Axis.X:
                    for (int i = 0; i < 3; i++)
                    {
						layers[i] = new Layer { plane = Constants.Plane.YZ };
                        for (int j = 0; j < 3; j++)
						{
							for (int k = 0; k < 3; k++)
							{
                                layers[i].cubes[j, k] = cubes[j, k, i];
							}
						}
					}
                    break;
                case Constants.Axis.Y:
					for (int i = 0; i < 3; i++)
					{
                        layers[i] = new Layer { plane = Constants.Plane.XZ };
                        for (int j = 0; j < 3; j++)
						{
							for (int k = 0; k < 3; k++)
							{
                                layers[i].cubes[j, k] = cubes[j, i, k];
							}
						}
					}
					break;
                case Constants.Axis.Z:
					for (int i = 0; i < 3; i++)
					{
                        layers[i] = new Layer { plane = Constants.Plane.XY };
                        for (int j = 0; j < 3; j++)
						{
							for (int k = 0; k < 3; k++)
							{
                                layers[i].cubes[j, k] = cubes[i, j, k];
							}
						}
					}
					break;
                default:
                    break;
			}
        }
		
		private void Combine()
		{
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					for (int k = 0; k < 3; k++)
					{
						switch (axis)
						{
							case Axis.X:
								cubes[i, j, k] = layers[k].cubes[i, j];
								break;
							case Axis.Y:
								cubes[i, j, k] = layers[j].cubes[i, k];
								break;
							case Axis.Z:
								cubes[i, j, k] = layers[i].cubes[j, k];
								break;
							default:
								break;
						}
					}
				}
			}
		}
		
		public void RotateLayer(Point3D point, Constants.Axis axis, bool isClockwise, PictureBox canvas)
		{
			if (rotationCount == 0)
			{
                Divide(axis);
            }

			int layerIndex = 0;

			switch (axis)
			{
				case Axis.X:
					layerIndex = (int)point.X3 + 1;
					break;
				case Axis.Y:
					layerIndex = (int)point.Y3 + 1;
					break;
				case Axis.Z:
					layerIndex = (int)point.Z3 + 1;
					break;
				default:
					break;
			}

			layers[layerIndex].Rotate(isClockwise);
			rotationCount++;
            if (rotationCount == 18)
            {
                Combine();
            }
            rotationCount %= 18;
			Draw(canvas);
        }

		public void RotateCube(Constants.Axis axis, bool isClockwise, PictureBox canvas)
		{
			if (rotationCount == 0)
			{
				Divide(axis);
			}
			for (int i = 0; i < 3; i++)
			{
				layers[i].Rotate(isClockwise);
			}
			rotationCount++;
			if (rotationCount == 18)
			{
				Combine();
			}
			rotationCount %= 18;
			Draw(canvas);
		}

		public Square[] SquaresOnScreen()
		{
			Square[] result = new Square[27];
			int t = 0;
			
			for (int i = 0; i < 3; i++) 
			{
                for (int j = 0; j < 3; j++)
                {
					result[t] = cubes[2, i, j].zP;
					t++;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    result[t] = cubes[i, 2, j].yP;
                    t++;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
					result[t] = cubes[i, j, 2].xP;
                    t++;
                }
            }

            return result;
        }
	}
}
