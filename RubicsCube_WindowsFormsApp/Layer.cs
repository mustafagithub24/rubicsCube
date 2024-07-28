using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RubicsCube_WindowsFormsApp.Constants;

namespace RubicsCube_WindowsFormsApp
{
	internal class Layer
	{
		public Cube[,] cubes;
		private double currentAngle;
		public Constants.Plane plane;
		private Point3D[,] centers;

		public Layer(Point3D center, Constants.Plane plane)
		{
			this.plane = plane;
			cubes = new Cube[3, 3];
			currentAngle = 0.0;

			switch (plane)
			{
				case Constants.Plane.XY:
					for (int i = -1; i <= 1; i++)
					{
						for (int j = -1; j <= 1; j++)
						{
							cubes[i + 1, j + 1] = new Cube(new Point3D(center.X3 + j, center.Y3 + i, center.Z3));
						}
					}
					break;
				case Constants.Plane.XZ:
					for (int i = -1; i <= 1; i++)
					{
						for (int j = -1; j <= 1; j++)
						{
							cubes[i + 1, j + 1] = new Cube(new Point3D(center.X3 + j, center.Y3, center.Z3 + i));
						}
					}
					break;
				case Constants.Plane.YZ:
					for (int i = -1; i <= 1; i++)
					{
						for (int j = -1; j <= 1; j++)
						{
							cubes[i + 1, j + 1] = new Cube(new Point3D(center.X3, center.Y3 + j, center.Z3 + i));
						}
					}
					break;
				default:
					break;
			}
		}
		public Layer()
		{
			cubes = new Cube[3, 3];
			plane = Constants.Plane.XY;
			currentAngle = 0.0;
		}

        public void Draw(Graphics g)
		{
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					cubes[i, j].Draw(g);
				}
			}

		}

		public void Rotate(bool isClockwise)
		{
			double angle = isClockwise ? 5 : -5;
			Constants.Axis axis = new Constants.Axis();
			switch (plane)
			{
				case Plane.XY:
					axis = Constants.Axis.Z;
					break;
				case Plane.XZ:
					axis = Constants.Axis.Y;
					break;
				case Plane.YZ:
					axis = Constants.Axis.X;
					break;
				default:
					break;
			}

            for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					cubes[i, j].Rotate(axis, isClockwise);
				}
			}
            if (currentAngle == 0)
            {
				SaveCenters();
            }

            currentAngle += angle;

            if (currentAngle == 45 || currentAngle == -45)
            {
				ReArrange(isClockwise);
            }

            if (currentAngle == 90 || currentAngle == -90)
            {
                ReArrangeCenters();
            }
        }

		private void ReArrange(bool isClockwise)
		{
			if (plane == Constants.Plane.XZ) isClockwise = !isClockwise;

			Cube[,] result = new Cube[3, 3];
            
            for (int i = 0; i < 3; i++)
            {
				for (int j = 0; j < 3; j++)
				{
					if (isClockwise)
					{
						result[i, j] = cubes[2 - j, i];
                    }
					else
					{
						result[i, j] = cubes[j, 2 - i];
                    }
                    cubes[i, j].SetCenter(cubes[i, j].center);
                }
            }

			cubes = result;
		}
        private void ReArrangeCenters()
        {

            for (int i = 0; i < 3; i ++)
            {
                for (int j = 0; j < 3; j ++)
                {
					cubes[i, j].SetCenter(centers[i, j]);
                }
            }
        }
		private void SaveCenters()
		{
			centers=new Point3D[3, 3];

            for (int i = 0; i < 3; i ++)
            {
                for (int j = 0; j < 3; j ++)
                {
					centers[i, j] = new Point3D(cubes[i, j].center);
                }
            }
        }
    }
}
