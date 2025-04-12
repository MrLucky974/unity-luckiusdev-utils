using UnityEngine;

namespace LuckiusDev.Utils
{
    /// <summary>
    /// A collection of helpful extension methods for Unity's Component class.
    /// Provides convenient accessors for Transform properties, along with utility methods
    /// to manipulate position, rotation, scale, hierarchy, and GameObject state.
    /// </summary>
    public static class ComponentExtensions
    {
        #region Transform - Position

        /// <summary>
        /// Gets the world position of the component's Transform.
        /// </summary>
        public static Vector3 GetPosition(this Component component) =>
            component != null ? component.transform.position : Vector3.zero;

        /// <summary>
        /// Gets the local position relative to the parent Transform.
        /// </summary>
        public static Vector3 GetLocalPosition(this Component component) =>
            component != null ? component.transform.localPosition : Vector3.zero;

        /// <summary>
        /// Sets the world position of the component's Transform.
        /// </summary>
        public static void SetPosition(this Component component, Vector3 position)
        {
            if (component != null) component.transform.position = position;
        }

        /// <summary>
        /// Sets the local position relative to the parent Transform.
        /// </summary>
        public static void SetLocalPosition(this Component component, Vector3 localPosition)
        {
            if (component != null) component.transform.localPosition = localPosition;
        }

        #endregion

        #region Transform - Rotation

        /// <summary>
        /// Gets the world rotation (Quaternion) of the component's Transform.
        /// </summary>
        public static Quaternion GetRotation(this Component component) =>
            component != null ? component.transform.rotation : Quaternion.identity;

        /// <summary>
        /// Gets the local rotation (Quaternion) relative to the parent Transform.
        /// </summary>
        public static Quaternion GetLocalRotation(this Component component) =>
            component != null ? component.transform.localRotation : Quaternion.identity;

        /// <summary>
        /// Sets the world rotation of the component's Transform.
        /// </summary>
        public static void SetRotation(this Component component, Quaternion rotation)
        {
            if (component != null) component.transform.rotation = rotation;
        }

        /// <summary>
        /// Sets the local rotation of the component's Transform.
        /// </summary>
        public static void SetLocalRotation(this Component component, Quaternion localRotation)
        {
            if (component != null) component.transform.localRotation = localRotation;
        }

        #endregion

        #region Transform - Scale

        /// <summary>
        /// Gets the local scale of the component's Transform.
        /// </summary>
        public static Vector3 GetLocalScale(this Component component) =>
            component != null ? component.transform.localScale : Vector3.one;

        /// <summary>
        /// Sets the local scale of the component's Transform.
        /// </summary>
        public static void SetLocalScale(this Component component, Vector3 localScale)
        {
            if (component != null) component.transform.localScale = localScale;
        }

        #endregion

        #region Hierarchy / GameObject Utilities

        /// <summary>
        /// Sets the parent of the component's Transform.
        /// </summary>
        public static void SetParent(this Component component, Transform newParent, bool worldPositionStays = true)
        {
            if (component != null) component.transform.SetParent(newParent, worldPositionStays);
        }

        /// <summary>
        /// Destroys the GameObject the component is attached to.
        /// </summary>
        public static void DestroySelf(this Component component)
        {
            if (component != null) Object.Destroy(component.gameObject);
        }

        /// <summary>
        /// Enables or disables the component's GameObject.
        /// </summary>
        public static void SetActive(this Component component, bool state)
        {
            if (component != null) component.gameObject.SetActive(state);
        }

        /// <summary>
        /// Checks if the component's GameObject is currently active in the hierarchy.
        /// </summary>
        public static bool IsActive(this Component component) =>
            component != null && component.gameObject.activeInHierarchy;

        #endregion
    }
}