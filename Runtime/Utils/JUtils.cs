using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LuckiusDev.Utils
{
    /// <summary>
    /// General utility functions including value remapping and debug printing for collections.
    /// </summary>
    public static class JUtils
    {
        #region Debug Print

        /// <summary>
        /// Prints the contents of an array to the Unity Console.
        /// </summary>
        public static void Print<T>(this T[] array)
        {
            if (array == null)
            {
                Debug.Log("[null array]");
                return;
            }

            StringBuilder sb = new StringBuilder("[");
            for (int i = 0; i < array.Length; i++)
            {
                sb.Append(array[i]?.ToString() ?? "null");
                if (i < array.Length - 1) sb.Append(", ");
            }
            sb.Append("]");
            Debug.Log(sb.ToString());
        }

        /// <summary>
        /// Prints the contents of a list to the Unity Console.
        /// </summary>
        public static void Print<T>(this List<T> list)
        {
            if (list == null)
            {
                Debug.Log("[null list]");
                return;
            }

            StringBuilder sb = new StringBuilder("[");
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append(list[i]?.ToString() ?? "null");
                if (i < list.Count - 1) sb.Append(", ");
            }
            sb.Append("]");
            Debug.Log(sb.ToString());
        }

        #endregion
    }
}
