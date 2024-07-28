using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RubicsCube_WindowsFormsApp
{
	internal class Cube
	{
		public Square xP,xN,yP,yN,zP,zN;
		private double currentAngle;
		public Point3D center;

		public Cube(Point3D center) 
		{
			this.center = center;
			xP = new Square(center, new Point3D(center.X3 + 0.5, center.Y3, center.Z3), Constants.Plane.YZ, Constants.colors[0, center.X3 == 1 ? 2 : 1]);
            xN = new Square(center, new Point3D(center.X3 - 0.5, center.Y3, center.Z3), Constants.Plane.YZ, Constants.colors[0, center.X3 == -1 ? 0 : 1]);
            yP = new Square(center, new Point3D(center.X3, center.Y3 + 0.5, center.Z3), Constants.Plane.XZ, Constants.colors[1, center.Y3 == 1 ? 2 : 1]);
            yN = new Square(center, new Point3D(center.X3, center.Y3 - 0.5, center.Z3), Constants.Plane.XZ, Constants.colors[1, center.Y3 == -1 ? 0 : 1]);
			zP = new Square(center, new Point3D(center.X3, center.Y3, center.Z3 + 0.5), Constants.Plane.XY, Constants.colors[2, center.Z3 == 1 ? 2 : 1]);
			zN = new Square(center, new Point3D(center.X3, center.Y3, center.Z3 - 0.5), Constants.Plane.XY, Constants.colors[2, center.Z3 == -1 ? 0 : 1]);
		}

		public void SetCenter(Point3D center)
		{
			this.center = center;
            xP.cubeCenter = center;
            xN.cubeCenter = center;
            yP.cubeCenter = center;
            yN.cubeCenter = center;
            zP.cubeCenter = center;
            zN.cubeCenter = center;

        }

        public void Draw(Graphics g)
		{
			xN.Draw(g);
			yN.Draw(g);
			zN.Draw(g);
			xP.Draw(g);
			yP.Draw(g);
			zP.Draw(g);
		}
		public void Rotate(Constants.Axis axis, bool isClockwise)
		{
			currentAngle %= 90;

			Square[] squares = { xP, xN, yP, yN, zP, zN };
			double angle = isClockwise ? 5 : -5;

			foreach (Square square in squares)
			{
				square.Rotate(axis, angle);
			}			

			currentAngle += angle;
			

			if (currentAngle == 45 || currentAngle == -45)
			{
				switch (axis)
				{
					case Constants.Axis.X:
						if (isClockwise)
						{
							Square temp = zP;
							zP = yP;
							yP = zN;
							zN = yN;
							yN = temp;
						}
						else
						{
							Square temp = yP;
							yP = zP;
							zP = yN;
							yN = zN;
							zN = temp;
						}
						break;
					case Constants.Axis.Y:
						if (isClockwise)
						{
							Square temp = xP;
							xP = zP;
							zP = xN;
							xN = zN;
							zN = temp;
						}
						else
						{
							Square temp = zP;
							zP = xP;
							xP = zN;
							zN = xN;
							xN = temp;
						}
						break;
					case Constants.Axis.Z:
						if (isClockwise)
						{
							Square temp = yP;
							yP = xP;
							xP = yN;
							yN = xN;
							xN = temp;
						}
						else
						{
							Square temp = xP;
							xP = yP;
							yP = xN;
							xN = yN;
							yN = temp;
						}
						break;
					default:
						break;
				}
			}
		}
	}
}
