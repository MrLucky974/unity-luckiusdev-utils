using UnityEngine;

namespace LuckiusDev.Utils
{
    public static class ComponentExtensions
    {
        #region Transform

        public static Vector3 GetPosition(this Component component) => component.transform.position;
        public static Vector3 GetLocalPosition(this Component component) => component.transform.localPosition;
        public static Quaternion GetRotation(this Component component) => component.transform.rotation;
        public static Quaternion GetLocalRotation(this Component component) => component.transform.localRotation;

        #endregion
    }

}