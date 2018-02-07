using CS3D.CustomDataTypes;
using System;
using System.Runtime.CompilerServices;

namespace CS3D
{
    static class MathTools
    {
        public static float DegreesToRadians(float degrees)
        {
            return degrees * (float)Math.PI / 180.0f;
        }

        public static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0f;
        }

        public static float Min3(float val1, float val2, float val3)
        {
            return Math.Min(val1, Math.Min(val2, val3));
        }

        public static float Max3(float val1, float val2, float val3)
        {
            return Math.Max(val1, Math.Max(val2, val3));
        }

        public static float Distance2D(Vector2 point1, Vector2 point2)
        {
            return (float)Math.Sqrt(Math.Pow(point1.u - point2.u, 2.0) + Math.Pow(point1.v - point2.v, 2.0));
        }

        public static float Distance2D(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(Math.Pow(x1 - x2, 2.0) + Math.Pow(y1 - y2, 2.0));
        }

        public static float Distance3D(Vector3 point1, Vector3 point2)
        {
            return (float)Math.Sqrt(Math.Pow(point1.x - point2.x, 2.0) + Math.Pow(point1.y - point2.y, 2.0) + Math.Pow(point1.z - point2.z, 2.0));
        }
    }
}
