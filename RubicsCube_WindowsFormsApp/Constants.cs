using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RubicsCube_WindowsFormsApp
{
	internal class Constants
	{
		public static double squareSize = 100;
		public static Size CanvasSize = new Size(800, 800);
		public static PointF origin = new PointF(400, 400);
		public static PointF xVector = new PointF((float)Math.Cos(-1 * Math.PI / 6), (float)Math.Sin(-1 * Math.PI / 6));
		public static PointF yVector = new PointF((float)Math.Cos(7 * Math.PI / 6), (float)Math.Sin(7 * Math.PI / 6));
		public static PointF zVector = new PointF((float)Math.Cos(3 * Math.PI / 6), (float)Math.Sin(3 * Math.PI / 6));
		public enum Plane { XY,XZ,YZ };
		public enum Axis { X, Y, Z };
		public static Color[,] colors = { {Color.Orange, Color.Black, Color.Red }
										, {Color.White, Color.Black, Color.Yellow }
										, {Color.Green, Color.Black, Color.Blue } };
		public static Color backgroundColor = Color.Azure;
		
        public static Tuple<bool,Axis> RotationAndAxis(Point start, Point end, Square square)
		{
			float X = end.X - start.X;
            float Y = -1 * (end.Y - start.Y);
			float norm = Norm(new PointF(X, Y));
            if (norm < 10)
            {
                return null;
            }
			X = X / norm;
			Y = Y / norm;
			PointF positionVector = new PointF(X, Y);

			float[] angles= new float[4];
			Axis axis = Axis.X;
			bool isClockwise = false;
			switch (square.plane)
			{
				case Plane.XY:
					angles[0] = AngleBetweenVectors(new PointF(-1 * xVector.X, -1 * xVector.Y), positionVector);
					angles[1] = AngleBetweenVectors(new PointF(xVector.X, xVector.Y), positionVector);
                    angles[2] = AngleBetweenVectors(new PointF(-1 * yVector.X, -1 * yVector.Y), positionVector);
                    angles[3] = AngleBetweenVectors(new PointF(yVector.X, yVector.Y), positionVector);
					switch (minIndex(angles))
					{
						case 0:
							axis = Axis.Y;
							isClockwise = false;
							break;
						case 1:
                            axis = Axis.Y;
                            isClockwise = true;
                            break; 
						case 2:
                            axis = Axis.X;
                            isClockwise = true;
                            break;
						case 3:
                            axis = Axis.X;
                            isClockwise = false;
                            break;
						default:
							break;
					}
					break;
				case Plane.XZ:
                    angles[0] = AngleBetweenVectors(new PointF(-1 * xVector.X, -1 * xVector.Y), positionVector);
                    angles[1] = AngleBetweenVectors(new PointF(xVector.X, xVector.Y), positionVector);
                    angles[2] = AngleBetweenVectors(new PointF(-1 * zVector.X, -1 * zVector.Y), positionVector);
                    angles[3] = AngleBetweenVectors(new PointF(zVector.X, zVector.Y), positionVector);
                    switch (minIndex(angles))
                    {
                        case 0:
                            axis = Axis.Z;
                            isClockwise = true;
                            break;
                        case 1:
                            axis = Axis.Z;
                            isClockwise = false;
                            break;
                        case 2:
                            axis = Axis.X;
                            isClockwise = false;
                            break;
                        case 3:
                            axis = Axis.X;
                            isClockwise = true;
                            break;
                        default:
                            break;
                    }
                    break;
				case Plane.YZ:
                    angles[0] = AngleBetweenVectors(new PointF(-1 * yVector.X, -1 * yVector.Y), positionVector);
                    angles[1] = AngleBetweenVectors(new PointF(yVector.X, yVector.Y), positionVector);
                    angles[2] = AngleBetweenVectors(new PointF(-1 * zVector.X, -1 * zVector.Y), positionVector);
                    angles[3] = AngleBetweenVectors(new PointF(zVector.X, zVector.Y), positionVector);
                    switch (minIndex(angles))
                    {
                        case 0:
                            axis = Axis.Z;
                            isClockwise = false;
                            break;
                        case 1:
                            axis = Axis.Z;
                            isClockwise = true;
                            break;
                        case 2:
                            axis = Axis.Y;
                            isClockwise = true;
                            break;
                        case 3:
                            axis = Axis.Y;
                            isClockwise = false;
                            break;
                        default:
                            break;
                    }
                    break;
				default:
					break;
			}

			return new Tuple<bool, Axis>(isClockwise, axis);
		}
		
		private static float DotProduct(PointF A, PointF B)
		{
			return A.X * B.X + A.Y * B.Y;
		}
        private static float Norm(PointF A)
        {
			return (float)Math.Sqrt(A.X * A.X + A.Y * A.Y);
        }
        private static float AngleBetweenVectors(PointF A, PointF B)
        {
			float angle = (float)Math.Acos(DotProduct(A, B) / (Norm(A) * Norm(B)));
			angle = (float)Math.Abs(angle % (2 * Math.PI));
			angle = (float)(angle * (180.0 / Math.PI));
            return angle;
        }
		private static int minIndex(float[] items)
		{
			int index = 0;
            for (int i = 1; i < items.Length; i++)
            {
				if (Math.Abs(items[i]) < Math.Abs(items[index]))
				{
					index = i;
				}
            }
			return index;
        }
    }
}
