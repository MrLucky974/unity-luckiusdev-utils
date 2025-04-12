using UnityEngine;

public static class GameObjectExtensions {
    
    /// <summary>
    /// Returns the object itself if it exists, null otherwise.
    /// </summary>
    /// <remarks>
    /// This method helps differentiate between a null reference and a destroyed Unity object. Unity's "== null" check
    /// can incorrectly return true for destroyed objects, leading to misleading behaviour. The OrNull method use
    /// Unity's "null check", and if the object has been marked for destruction, it ensures an actual null reference is returned,
    /// aiding in correctly chaining operations and preventing NullReferenceExceptions.
    /// </remarks>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="obj">The object being checked.</param>
    /// <returns>The object itself if it exists and not destroyed, null otherwise.</returns>
    public static T OrNull<T> (this T obj) where T : Object => obj ? obj : null;

    /// <summary>
    /// Attempts to find a component of type <typeparamref name="T"/> on the current object,
    /// in its children (optionally including inactive ones), or up through its parent hierarchy.
    /// </summary>
    /// <typeparam name="T">The type of component to find.</typeparam>
    /// <param name="obj">The starting component.</param>
    /// <param name="component">Outputs the component if found.</param>
    /// <param name="includeInactive">Whether to include inactive children in the search.</param>
    /// <returns>True if the component was found; otherwise, false.</returns>
    public static bool TryGetComponentAnywhere<T>(this Component obj, out T component, bool includeInactive = false)
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
    public static bool TryGetComponentAnywhere<T>(this GameObject gameObject, out T component, bool includeInactive = false)
    {
        return gameObject.transform.TryGetComponentAnywhere(out component, includeInactive);
    }
}
