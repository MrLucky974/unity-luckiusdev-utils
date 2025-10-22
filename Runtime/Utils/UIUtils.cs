using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LuckiusDev.Utils
{
    public static class UIUtils
    {
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

        public static void Lerp(this Slider slider, float targetValue, float delta)
        {
            float currentValue = slider.value;
            slider.value = Mathf.Lerp(currentValue, targetValue, delta);
        }
    }
}