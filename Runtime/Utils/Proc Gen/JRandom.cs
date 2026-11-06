using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LuckiusDev.Utils
{
    /// <summary>
    /// Utility class for random generation using both UnityEngine.Random and System.Random.
    /// Includes dice rolling, weighted selection, and collection randomizers.
    /// </summary>
    public static class JRandom
    {
        #region Shuffle

        /// <summary>
        /// Shuffles a list using Fisher-Yates algorithm.
        /// </summary>
        public static void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        #endregion

        #region Perlin Noise

        /// <summary>
        /// Returns Perlin noise at a 2D coordinate.
        /// </summary>
        public static float Perlin2D(float x, float y, float scale = 1f)
        {
            return Mathf.PerlinNoise(x * scale, y * scale);
        }

        /// <summary>
        /// Approximates 3D Perlin noise by averaging multiple 2D noise samples.
        /// </summary>
        public static float Perlin3D(float x, float y, float z, float scale = 1f)
        {
            float xy = Mathf.PerlinNoise(x * scale, y * scale);
            float yz = Mathf.PerlinNoise(y * scale, z * scale);
            float xz = Mathf.PerlinNoise(x * scale, z * scale);
            float yx = Mathf.PerlinNoise(y * scale, x * scale);
            float zy = Mathf.PerlinNoise(z * scale, y * scale);
            float zx = Mathf.PerlinNoise(z * scale, x * scale);
            return (xy + yz + xz + yx + zy + zx) / 6f;
        }

        #endregion

        #region Gaussian Distribution

        /// <summary>
        /// Returns a float based on a Gaussian (normal) distribution using Box-Muller transform.
        /// </summary>
        public static float GaussianRandom(float mean = 0f, float stdDev = 1f)
        {
            float u1 = 1.0f - Random.value; // avoid zero
            float u2 = 1.0f - Random.value;
            float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
            return mean + stdDev * randStdNormal;
        }

        #endregion

        #region Dice

        /// <summary>
        /// Rolls N dice with values ranging from L to S.
        /// </summary>
        public static int RollDice(int n, int l, int s)
        {
            int total = 0;
            for (int i = 0; i < n; i++)
            {
                total += Random.Range(l, s + 1);
            }
            return total;
        }

        /// <summary>
        /// Rolls N dice from 0 to S.
        /// </summary>
        public static int RollDice(int n, int s) => RollDice(n, 0, s);

        #endregion

        #region Random Element Pickers

        /// <summary>
        /// Picks a random element in this array.
        /// </summary>
        public static T PickRandom<T>(this T[] array) =>
            array[Random.Range(0, array.Length)];

        public static T PickRandom<T>(this List<T> list) =>
            list[Random.Range(0, list.Count)];

        #endregion

        #region System.Random Extensions

        /// <summary>
        /// Returns a random double between 0 and max.
        /// </summary>
        public static double NextDouble(this System.Random random, double max) =>
            random.NextDouble() * max;

        /// <summary>
        /// Returns a random double in a specified range.
        /// </summary>
        public static double NextDouble(this System.Random random, double min, double max) =>
            min + (max - min) * random.NextDouble();

        /// <summary>
        /// Returns a random float between 0 and 1.
        /// </summary>
        public static float NextFloat(this System.Random random) =>
            (float)random.NextDouble();

        /// <summary>
        /// Returns a random float between 0 and max.
        /// </summary>
        public static float NextFloat(this System.Random random, float max) =>
            (float)(random.NextDouble() * max);

        /// <summary>
        /// Returns a random float between min and max.
        /// </summary>
        public static float NextFloat(this System.Random random, float min, float max) =>
            min + (float)(random.NextDouble() * (max - min));

        #endregion

        #region Geometry

        /// <summary>
        /// Returns a random point within an annulus (ring) between min and max radius around an origin.
        /// </summary>
        public static Vector2 RandomPointInAnnulus(Vector2 origin, float minRadius, float maxRadius)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float radius = Random.Range(minRadius, maxRadius);
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            return origin + offset;
        }

        #endregion
    }
}