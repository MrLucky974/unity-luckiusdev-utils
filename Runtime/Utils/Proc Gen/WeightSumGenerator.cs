using System;
using System.Collections.Generic;

namespace LuckiusDev.Utils
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
}
