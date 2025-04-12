using System.Collections.Generic;
using UnityEngine;

namespace LuckiusDev.Utils
{
    /// <summary>
    /// Utility class for smoothing a path using the Chaikin's corner-cutting algorithm.
    /// Useful for generating smooth curves from a set of discrete waypoints.
    /// </summary>
    public static class ChaikinPathSmoothing
    {
        /// <summary>
        /// Applies Chaikin's smoothing algorithm to a list of points.
        /// </summary>
        /// <param name="points">The original list of points to smooth.</param>
        /// <param name="iterations">How many smoothing iterations to apply. More = smoother curve.</param>
        /// <param name="percent">
        /// The fraction between each point pair to generate new points.
        /// Lower values produce tighter curves. Usually between 0.25 and 0.5.
        /// </param>
        /// <param name="closed">Whether the path should be closed into a loop.</param>
        /// <returns>A new list of smoothed points.</returns>
        public static List<Vector3> GetSmoothPath(List<Vector3> points, int iterations = 4, float percent = 0.25f, bool closed = false)
        {
            if (points == null || points.Count < 2)
            {
                Debug.LogWarning("Chaikin smoothing requires at least 2 points.");
                return points;
            }

            for (int i = 0; i < iterations; i++)
            {
                points = GetPath(points, percent, closed);
            }

            return points;
        }

        /// <summary>
        /// Executes a single iteration of the Chaikin corner-cutting algorithm.
        /// </summary>
        /// <param name="points">The input list of points to smooth.</param>
        /// <param name="percent">The percentage offset used to generate intermediate points.</param>
        /// <param name="closed">Whether to treat the path as closed (looped).</param>
        /// <returns>A new list of points generated from the smoothing process.</returns>
        private static List<Vector3> GetPath(List<Vector3> points, float percent, bool closed)
        {
            List<Vector3> path = new List<Vector3>();

            Vector3 start = points[0];
            Vector3 end = points[^1];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector3 pointA = points[i];
                Vector3 pointB = points[i + 1];

                // First cut: closer to A
                Vector3 cut1 = Vector3.Lerp(pointA, pointB, percent);
                // Second cut: closer to B
                Vector3 cut2 = Vector3.Lerp(pointA, pointB, 1 - percent);

                path.Add(cut1);
                path.Add(cut2);
            }

            if (!closed)
            {
                // If path is open, preserve the last point
                path.Add(end);
            }
            else
            {
                // Close the loop by connecting last to first
                Vector3 cut1 = Vector3.Lerp(end, start, percent);
                Vector3 cut2 = Vector3.Lerp(end, start, 1 - percent);

                path.Add(cut1);
                path.Add(cut2);
            }

            return path;
        }
    }
}