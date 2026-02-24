using UnityEngine;

namespace LuckiusDev.Utils.Extensions
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

        #region Search & Addition

        /// <summary>
        /// Attempts to find a component of type <typeparamref name="T"/> on the current object,
        /// in its children (optionally including inactive ones), or up through its parent hierarchy.
        /// </summary>
        /// <typeparam name="T">The type of component to find.</typeparam>
        /// <param name="obj">The starting component.</param>
        /// <param name="component">Outputs the component if found.</param>
        /// <param name="includeInactive">Whether to include inactive children in the search.</param>
        /// <returns>True if the component was found; otherwise, false.</returns>
        public static bool TryGetComponentAnywhere<T>(this Component obj, out T component, bool includeInactive = false) where T : Component
        {
            // Check on the current object
            if (obj.TryGetComponent(out component))
                return true;

            // Check in children (with option to include inactive ones)
            component = obj.GetComponentInChildren<T>(includeInactive);
            if (component != null)
                return true;

            // Recurse up the parent hierarchy
            var parent = obj.transform.parent;
            while (parent != null)
            {
                if (parent.TryGetComponent(out component))
                    return true;

                parent = parent.parent;
            }

            component = default;
            return false;
        }

        /// <summary>
        /// Overload for GameObject to call TryGetComponentAnywhere in the same way.
        /// </summary>
        public static bool TryGetComponentAnywhere<T>(this GameObject gameObject, out T component,
            bool includeInactive = false) where T : Component
        {
            return gameObject.transform.TryGetComponentAnywhere(out component, includeInactive);
        }

        /// <summary>
        /// Gets a component of type T from the GameObject if it exists,
        /// otherwise adds the component and returns the newly created instance.
        /// </summary>
        /// <typeparam name="T">
        /// The type of Component to retrieve or add. Must inherit from Component.
        /// </typeparam>
        /// <param name="gameObject">
        /// The GameObject on which to get or create the component.
        /// </param>
        /// <returns>
        /// The existing component if found; otherwise, a newly added component of type T.
        /// </returns>
        /// <remarks>
        /// This method avoids calling AddComponent if the component already exists,
        /// making it safe and efficient to ensure a required component is present.
        /// </remarks>
        public static T GetOrCreate<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.TryGetComponent<T>(out var component))
            {
                return component;
            }

            return gameObject.AddComponent<T>();
        }

        #endregion
    }
}