using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LuckiusDev.Utils
{
    /// <summary>
    /// General utility functions.
    /// </summary>
    public static class JUtils
    {
        #region User Interface

        private static PointerEventData s_eventData;
        private static List<RaycastResult> s_results;

        public static bool OverUI()
        {
            s_eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            s_results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(s_eventData, s_results);
            return s_results.Count > 0;
        }

        public static void Show(this CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        public static void Hide(this CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        public static Vector2 GetCanvasElementWorldPosition(RectTransform element)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera.main,
                    out var result);
            return result;
        }

        /// <summary>
        /// Apply linear interpolation to a slider.
        /// </summary>
        /// <param name="slider">The slider component.</param>
        /// <param name="targetValue">The target value.</param>
        /// <param name="delta">The interpolation value.</param>
        public static void Lerp(this Slider slider, float targetValue, float delta)
        {
            float currentValue = slider.value;
            slider.value = Mathf.Lerp(currentValue, targetValue, delta);
        }

        #endregion

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
                sb.Append(ValueOrNull(array[i]));
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
                sb.Append(ValueOrNull(list[i]));
                if (i < list.Count - 1) sb.Append(", ");
            }
            sb.Append("]");
            Debug.Log(sb.ToString());
        }

        /// <summary>
        /// Prints the contents of a dictionary to the Unity Console.
        /// </summary>
        public static void Print<K, V>(this Dictionary<K, V> dict)
        {
            if (dict == null)
            {
                Debug.Log("[null dict]");
                return;
            }

            int size = dict.Count;

            StringBuilder sb = new StringBuilder("[");

            int index = 0;
            foreach (var key in dict.Keys)
            {
                var value = dict[key];
                sb.Append($"({ValueOrNull(key)}, {ValueOrNull(value)})");
                if (index < size - 1) sb.Append(", ");
                index++;
            }
            sb.Append("]");
            Debug.Log(sb.ToString());
        }

        public static string ValueOrNull(this object value)
        {
            return value?.ToString() ?? "null";
        }

        #endregion
    }
}
