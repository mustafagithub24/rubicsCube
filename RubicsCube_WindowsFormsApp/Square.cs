using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubicsCube_WindowsFormsApp
{
	internal class Square
	{
		private Point3D[] pointsExterior;
		private Point3D[] pointsInterior;
		public Color color;
		public Point3D center;
		public Constants.Plane plane;
		public Point3D cubeCenter;

		public Square(Point3D cubeCenter, Point3D center, Constants.Plane plane, Color color) 
		{
			this.cubeCenter = cubeCenter;
			this.center= center;
			this.plane = plane;
            this.color = color;
            pointsExterior = new Point3D[4];
			pointsInterior = new Point3D[4];

			switch (plane)
			{
				case Constants.Plane.XY:
					for (int i = 0; i < 4; i++)
					{
						double xAxisEx = i == 0 || i == 3 ? 0.5 : -0.5;
						double yAxisEx = i == 0 || i == 1 ? 0.5 : -0.5;
						double xAxisIn = i == 0 || i == 3 ? 0.45 : -0.45;
						double yAxisIn = i == 0 || i == 1 ? 0.45 : -0.45;
						pointsExterior[i] = new Point3D(center.X3 + xAxisEx, center.Y3 + yAxisEx, center.Z3);
						pointsInterior[i] = new Point3D(center.X3 + xAxisIn, center.Y3 + yAxisIn, center.Z3);
					}
					break;
				case Constants.Plane.XZ:
					for (int i = 0; i < 4; i++)
					{
						double xAxisEx = i == 0 || i == 3 ? 0.5 : -0.5;
						double zAxisEx = i == 0 || i == 1 ? 0.5 : -0.5;
						double xAxisIn = i == 0 || i == 3 ? 0.45 : -0.45;
						double zAxisIn = i == 0 || i == 1 ? 0.45 : -0.45;
						pointsExterior[i] = new Point3D(center.X3 + xAxisEx, center.Y3, center.Z3 + zAxisEx);
						pointsInterior[i] = new Point3D(center.X3 + xAxisIn, center.Y3, center.Z3 + zAxisIn);
                    }
					break;
				case Constants.Plane.YZ:
					for (int i = 0; i < 4; i++)
					{
						double yAxisEx = i == 0 || i == 3 ? 0.5 : -0.5;
						double zAxisEx = i == 0 || i == 1 ? 0.5 : -0.5;
						double yAxisIn = i == 0 || i == 3 ? 0.45 : -0.45;
						double zAxisIn = i == 0 || i == 1 ? 0.45 : -0.45;
						pointsExterior[i] = new Point3D(center.X3, center.Y3 + yAxisEx, center.Z3 + zAxisEx);
						pointsInterior[i] = new Point3D(center.X3, center.Y3 + yAxisIn, center.Z3 + zAxisIn);
                    }
					break;
				default:
					break;
			}
		}

		public void Draw(Graphics g, bool isLight=false)
		{
			PointF[] pointsEx = pointsExterior.Select(x => x.point2D).ToArray();
			PointF[] pointsIn = pointsInterior.Select(x => x.point2D).ToArray();
			g.FillPolygon(new Pen(Color.Black).Brush, pointsEx);
			if (color != Color.Black) g.FillPolygon(new Pen(isLight?Color.FromArgb(128,color):color).Brush, pointsIn);
		}

		public void Rotate(Constants.Axis axis, double angle)
		{
            foreach (var point in pointsExterior)
            {
				point.Rotate(axis, angle);
            }
			foreach (var point in pointsInterior)
			{
				point.Rotate(axis, angle);
			}
			center.Rotate(axis, angle);
        }

		public bool IsMouseInside(Point mousePosition)
		{
			float[] x = new float[4];
			float[] y = new float[4];
            for (int i = 0; i < 4; i++)
			{
                x[i] = pointsInterior[i].point2D.X;
                y[i] = pointsInterior[i].point2D.Y;
            }

			float MaxX = x[0], MaxY = y[0], MinX = x[0], MinY = y[0];

			for (int i = 1; i < 4; i++)
			{
				if (x[i] > MaxX) MaxX = x[i];
				if (x[i] < MinX) MinX = x[i];
				if (y[i] > MaxY) MaxY = y[i];
				if (y[i] < MinY) MinY = y[i];
            }

            if (!(MinX <= mousePosition.X && mousePosition.X <= MaxX &&
                  MinY <= mousePosition.Y  && mousePosition.Y  <= MaxY))
            {
                return false;
            }

            return true;
        }
		
	}
}
