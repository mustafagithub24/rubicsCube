using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubicsCube_WindowsFormsApp
{
	internal class Point3D
	{
		public double X3;
		public double Y3;
		public double Z3;
		public PointF point2D;
		public Point3D(double x, double y, double z)
		{
			setCoordinates(new double[] { x, y, z });
		}

        public Point3D(Point3D p) : this(p.X3, p.Y3, p.Z3)
        {

        }

        private void setCoordinates(double[] coordinates)
		{
			X3 = coordinates[0];
			Y3 = coordinates[1];
			Z3 = coordinates[2];

			point2D.X = (float)(Constants.origin.X
				+ Constants.squareSize * X3 * (double)Constants.xVector.X
				+ Constants.squareSize * Y3 * (double)Constants.yVector.X
				+ Constants.squareSize * Z3 * (double)Constants.zVector.X);
			point2D.Y = (float)(Constants.origin.Y
				- Constants.squareSize * X3 * (double)Constants.xVector.Y
				- Constants.squareSize * Y3 * (double)Constants.yVector.Y
				- Constants.squareSize * Z3 * (double)Constants.zVector.Y);
		}

		public void Rotate(Constants.Axis axis, double angle)
		{
			
			angle = Math.PI * angle / 180.0;

			double[,] rX = { {1, 0, 0 }
							,{0, Math.Cos(angle),-1*Math.Sin(angle) }
							,{0, Math.Sin(angle), Math.Cos(angle) } };

			double[,] rY = { {Math.Cos(angle), 0, Math.Sin(angle) }
							,{0, 1, 0 }
							,{-1*Math.Sin(angle), 0, Math.Cos(angle) } };

			double[,] rZ = { {Math.Cos(angle), -1*Math.Sin(angle), 0 }
							,{Math.Sin(angle), Math.Cos(angle), 0 }
							,{0, 0, 1 } };

			double[] point = { X3, Y3, Z3 };
			double[] result = new double[3];
			double[,] matrix= new double[3,3];
			switch (axis)
			{
				case Constants.Axis.X:
					matrix = rX;
					break;
				case Constants.Axis.Y:
					matrix = rY;
					break;
				case Constants.Axis.Z:
					matrix = rZ;
					break;
				default:
					break;
			}

			for (int i = 0; i < 3; i++)
            {
				double sum = 0;
                for (int j = 0; j < 3; j++)
                {
					sum += matrix[i, j] * point[j];
                }
				result[i] = Math.Round(sum,3);
            }

			setCoordinates(result);
        }
	}
}
