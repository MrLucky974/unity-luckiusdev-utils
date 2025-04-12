using UnityEngine;

namespace LuckiusDev.Utils
{
    /// <summary>
    /// A collection of static math utility functions commonly used in game development,
    /// including vector math, grid calculations, noise patterns, and numeric approximation.
    /// </summary>
    public static class JMath
    {
        #region Angle & Direction

        /// <summary>
        /// Converts an angle in degrees to a 2D direction vector.
        /// </summary>
        public static Vector3 GetVectorFromAngle(float angle)
        {
            float radians = angle * Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(radians), Mathf.Sin(radians));
        }

        /// <summary>
        /// Returns the angle (0–360°) from a 2D direction vector as a float.
        /// </summary>
        public static float GetAngleFromVectorFloat(Vector3 dir)
        {
            dir = dir.normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            return angle < 0 ? angle + 360f : angle;
        }

        /// <summary>
        /// Returns the angle (0–360°) from a 2D direction vector as a rounded int.
        /// </summary>
        public static int GetAngleFromVector(Vector3 dir)
        {
            return Mathf.RoundToInt(GetAngleFromVectorFloat(dir));
        }

        #endregion

        #region Grid & Rects

        /// <summary>
        /// Computes a roughly rectangular shape (w × h) for a given area.
        /// </summary>
        public static void ApproximativeRectangle(float area, out int w, out int h)
        {
            h = Mathf.RoundToInt(Mathf.Sqrt(area));
            w = Mathf.CeilToInt(area / h);
        }

        /// <summary>
        /// Calculates optimal square side count and total size given element count and size.
        /// </summary>
        public static (int sideCount, int squareSize) OptimalSquare(int elementCount, int elementSize)
        {
            int sideCount = Mathf.CeilToInt(Mathf.Sqrt(elementCount));
            return (sideCount, sideCount * elementSize);
        }

        /// <summary>
        /// Converts a 2D grid coordinate (x, z) into a 1D array index.
        /// </summary>
        public static int CoordinateToIndex((int x, int z) position, int size)
        {
            return position.x + size * position.z;
        }

        /// <summary>
        /// Converts a 1D array index to a 2D grid coordinate (x, z).
        /// </summary>
        public static (int x, int z) IndexToCoordinate(int i, int size)
        {
            int x = i % size;
            int z = i / size;
            return (x, z);
        }

        #endregion

        #region Headbob Effect

        /// <summary>
        /// Simulates a headbob motion pattern using sinusoidal waves.
        /// </summary>
        public static Vector2 Headbob(float time, float frequency, float amplitude)
        {
            return new Vector2(
                Mathf.Cos(time * frequency * 0.5f) * amplitude,
                Mathf.Sin(time * frequency) * amplitude
            );
        }

        #endregion

        #region Wrapping & Range

        /// <summary>
        /// Wraps a value between two bounds, including both ends.
        /// </summary>
        public static int WrapValue(int a, int b, int x)
        {
            int range = b - a + 1;
            int result = (x - a) % range;
            return result < 0 ? result + range + a : result + a;
        }

        #endregion

        #region Approximation Checks

        /// <summary>
        /// Returns true if two ints are approximately equal within a given margin.
        /// </summary>
        public static bool IsApproxEqual(int a, int b, float margin) =>
            Mathf.Abs(a - b) <= margin;

        public static bool IsApproxEqual(int a, int b) =>
            IsApproxEqual(a, b, Mathf.Epsilon);

        /// <summary>
        /// Returns true if two floats are approximately equal within a given margin.
        /// </summary>
        public static bool IsApproxEqual(float a, float b, float margin) =>
            Mathf.Abs(a - b) <= margin;

        public static bool IsApproxEqual(float a, float b) =>
            IsApproxEqual(a, b, Mathf.Epsilon);

        /// <summary>
        /// Returns true if two doubles are approximately equal within a given margin.
        /// </summary>
        public static bool IsApproxEqual(double a, double b, double margin) =>
            Mathf.Abs((float)(a - b)) <= margin;

        public static bool IsApproxEqual(double a, double b) =>
            IsApproxEqual(a, b, double.Epsilon);

        #endregion

        /// <summary>
        /// Remaps a value from one range to another.
        /// </summary>
        public static float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
        }

        /// <summary>
        /// Linearly interpolates between two values and clamps the result.
        /// </summary>
        public static float LerpClamped(float a, float b, float t)
        {
            return Mathf.Lerp(a, b, Mathf.Clamp01(t));
        }

        /// <summary>
        /// Returns true if the value is approximately zero.
        /// </summary>
        public static bool IsZero(float value, float epsilon = 0.0001f)
        {
            return Mathf.Abs(value) < epsilon;
        }

        /// <summary>
        /// Rounds a float value to the nearest step.
        /// </summary>
        public static float RoundToStep(float value, float step)
        {
            return Mathf.Round(value / step) * step;
        }
    }
}
