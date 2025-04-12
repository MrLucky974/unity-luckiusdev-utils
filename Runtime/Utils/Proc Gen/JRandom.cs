using System;
using System.Collections.Generic;
using UnityEngine;

namespace LuckiusDev.Utils.ProcGen
{
    /// <summary>
    /// Utility class for random generation using both UnityEngine.Random and System.Random.
    /// Includes dice rolling, weighted selection, and collection randomizers.
    /// </summary>
    public static class JRandom
    {
        /// <summary>
        /// Represents a weighted random selector for generic elements.
        /// </summary>
        public class WeightSumGenerator<T>
        {
            private readonly Dictionary<T, int> values = new();

            /// <summary>
            /// Adds an element with a specified weight (overwrites if key exists).
            /// </summary>
            public void Add(T element, int weight)
            {
                values[element] = weight;
            }

            /// <summary>
            /// Selects a random element based on weights using a seed.
            /// </summary>
            public T Generate(int seed)
            {
                return Generate(new System.Random(seed));
            }

            /// <summary>
            /// Selects a random element based on weights using a System.Random instance.
            /// </summary>
            public T Generate(System.Random rng)
            {
                int weightedSum = 0;
                foreach (var kvp in values)
                {
                    weightedSum += kvp.Value;
                }

                if (weightedSum <= 0)
                    throw new InvalidOperationException("Total weight must be greater than 0.");

                int r = rng.Next(weightedSum);
                foreach (var (value, weight) in values)
                {
                    if (r < weight)
                        return value;

                    r -= weight;
                }

                // Fallback (should not occur if weights are valid)
                throw new InvalidOperationException("Failed to select a weighted element.");
            }
        }

        #region Weighted Selection (One-liner)

        /// <summary>
        /// Picks a random item from a weighted list of (item, weight) tuples.
        /// </summary>
        public static T PickWeighted<T>(List<(T item, int weight)> weightedItems, System.Random rng = null)
        {
            rng ??= new System.Random();
            int totalWeight = 0;

            foreach (var (_, weight) in weightedItems)
                totalWeight += weight;

            int r = rng.Next(totalWeight);

            foreach (var (item, weight) in weightedItems)
            {
                if (r < weight)
                    return item;
                r -= weight;
            }

            throw new InvalidOperationException("Failed to select a weighted item.");
        }
        
        /// <summary>
        /// Picks a weighted item using UnityEngine.Random from a list of (item, weight) tuples.
        /// </summary>
        public static T PickWeightedUnity<T>(List<(T item, int weight)> weightedItems)
        {
            int totalWeight = 0;

            foreach (var (_, weight) in weightedItems)
                totalWeight += weight;

            int r = UnityEngine.Random.Range(0, totalWeight);

            foreach (var (item, weight) in weightedItems)
            {
                if (r < weight)
                    return item;
                r -= weight;
            }

            throw new InvalidOperationException("Failed to select a weighted item.");
        }

        #endregion

        #region Shuffle

        /// <summary>
        /// Randomly shuffles a list using the Fisher-Yates algorithm.
        /// </summary>
        public static void Shuffle<T>(List<T> list, System.Random rng = null)
        {
            rng ??= new System.Random();
            int n = list.Count;

            for (int i = n - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        /// <summary>
        /// Shuffles a list using UnityEngine.Random and Fisher-Yates algorithm.
        /// </summary>
        public static void ShuffleUnity<T>(List<T> list)
        {
            int n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        #endregion

        #region Bag Random (No Repeats)

        /// <summary>
        /// Draws random values from a bag without replacement until the bag is empty.
        /// </summary>
        public class BagRandom<T>
        {
            private readonly List<T> originalItems;
            private List<T> currentBag;
            private readonly System.Random rng;

            public BagRandom(IEnumerable<T> items, int? seed = null)
            {
                originalItems = new List<T>(items);
                rng = seed.HasValue ? new System.Random(seed.Value) : new System.Random();
                ResetBag();
            }

            private void ResetBag()
            {
                currentBag = new List<T>(originalItems);
                Shuffle(currentBag, rng);
            }

            public T Draw()
            {
                if (currentBag.Count == 0)
                    ResetBag();

                int index = rng.Next(currentBag.Count);
                T value = currentBag[index];
                currentBag.RemoveAt(index);
                return value;
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
        public static float GaussianRandom(float mean = 0f, float stdDev = 1f, System.Random rng = null)
        {
            rng ??= new System.Random();

            // Box-Muller transform
            double u1 = 1.0 - rng.NextDouble(); // avoid zero
            double u2 = 1.0 - rng.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                   Math.Sin(2.0 * Math.PI * u2);

            return mean + stdDev * (float)randStdNormal;
        }

        /// <summary>
        /// Returns a float based on a Gaussian (normal) distribution using UnityEngine.Random and Box-Muller transform.
        /// </summary>
        public static float GaussianRandomUnity(float mean = 0f, float stdDev = 1f)
        {
            float u1 = 1.0f - UnityEngine.Random.value; // avoid zero
            float u2 = 1.0f - UnityEngine.Random.value;
            float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
            return mean + stdDev * randStdNormal;
        }

        #endregion

        #region Dice

        /// <summary>
        /// Rolls N dice with values ranging from L to S.
        /// </summary>
        public static int RollDice(int n, int l, int s, System.Random random)
        {
            int total = 0;
            for (int i = 0; i < n; i++)
            {
                total += random.Next(l, s + 1);
            }
            return total;
        }

        /// <summary>
        /// Rolls N dice from 0 to S (inclusive).
        /// </summary>
        public static int RollDice(int n, int s, System.Random random) =>
            RollDice(n, 0, s, random);

        public static int RollDice(int n, int s, int seed) =>
            RollDice(n, s, new System.Random(seed));

        public static int RollDice(int n, int s) =>
            RollDice(n, s, new System.Random());

        /// <summary>
        /// Rolls N dice with values ranging from L to S using UnityEngine.Random.
        /// </summary>
        public static int RollDiceUnity(int n, int l, int s)
        {
            int total = 0;
            for (int i = 0; i < n; i++)
            {
                total += UnityEngine.Random.Range(l, s + 1);
            }
            return total;
        }

        /// <summary>
        /// Rolls N dice from 0 to S using UnityEngine.Random.
        /// </summary>
        public static int RollDiceUnity(int n, int s) => RollDiceUnity(n, 0, s);

        #endregion

        #region Random Element Pickers (Unity + System)

        /// <summary>
        /// Picks a random element using UnityEngine.Random.
        /// </summary>
        public static T PickRandomUnity<T>(this T[] array) =>
            array[UnityEngine.Random.Range(0, array.Length)];

        /// <summary>
        /// Picks a random element from an array using System.Random.
        /// </summary>
        public static T PickRandom<T>(this T[] array, System.Random random) =>
            array[random.Next(array.Length)];

        public static T PickRandom<T>(this T[] array, int seed) =>
            array.PickRandom(new System.Random(seed));

        public static T PickRandom<T>(this T[] array) =>
            array.PickRandom(new System.Random());

        public static T PickRandom<T>(this List<T> list, System.Random random) =>
            list[random.Next(list.Count)];

        public static T PickRandom<T>(this List<T> list, int seed) =>
            list.PickRandom(new System.Random(seed));

        public static T PickRandom<T>(this List<T> list) =>
            list.PickRandom(new System.Random());

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
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
            float radius = UnityEngine.Random.Range(minRadius, maxRadius);
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            return origin + offset;
        }

        /// <summary>
        /// Generates a normalized random direction on the XZ plane.
        /// </summary>
        public static Vector3 GenerateRandomFlatDirection()
        {
            Vector2 random2D = UnityEngine.Random.insideUnitCircle;
            return new Vector3(random2D.x, 0f, random2D.y).normalized;
        }

        #endregion
    }
}
