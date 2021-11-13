using System.Drawing;

namespace PlatformRacing3.Server.Utils
{
	internal class Maths
    {
        internal const double DEG_RAD = 0.0174533;

        internal static PointF RotatePoint(double x, double y, float rot)
        {
            rot = -rot;

            double pythag = Maths.Pythag(x, y);
            double angle = Maths.DEG_RAD * rot + Math.Atan2(y, x);

            x = Math.Cos(angle) * pythag;
            y = Math.Sin(angle) * pythag;

            return new PointF((float)x, (float)y);
        }

        internal static double Pythag(double x, double y)
        {
            return Math.Sqrt(x * x + y * y);
        }
    }
}
