using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LuckiusDev.Utils
{
    public static class JUtils
    {
        public static void Print<T>(this T[] array)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < array.Length; i++)
            {
                sb.Append(array[i].ToString());
                sb.Append(i < array.Length - 1 ? ", " : "");
            }
            sb.Append("]");
            Debug.Log(sb.ToString());
        }

        public static void Print<T>(this List<T> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append(list[i].ToString());
                sb.Append(i < list.Count - 1 ? ", " : "");
            }
            sb.Append("]");
            Debug.Log(sb.ToString());
        }
    }
}
