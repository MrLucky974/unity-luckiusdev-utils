using UnityEngine;

namespace LuckiusDev.Utils
{
    /// <summary>
    /// Provides swizzle extension methods for converting between Vector2 and Vector3.
    /// </summary>
    public static class VectorExtensions
    {
        #region Vector3 to Vector2

        /// <summary>
        /// Returns a Vector2 containing the X and Y components of the Vector3.
        /// </summary>
        /// <param name="vec3">The source Vector3.</param>
        /// <returns>A Vector2 (x, y).</returns>
        public static Vector2 XY(this Vector3 vec3)
        {
            return new Vector2(vec3.x, vec3.y);
        }

        /// <summary>
        /// Returns a Vector2 containing the X and Z components of the Vector3.
        /// </summary>
        /// <param name="vec3">The source Vector3.</param>
        /// <returns>A Vector2 (x, z).</returns>
        public static Vector2 XZ(this Vector3 vec3)
        {
            return new Vector2(vec3.x, vec3.z);
        }

        /// <summary>
        /// Returns a Vector2 containing the Y and X components of the Vector3.
        /// </summary>
        /// <param name="vec3">The source Vector3.</param>
        /// <returns>A Vector2 (y, x).</returns>
        public static Vector2 YX(this Vector3 vec3)
        {
            return new Vector2(vec3.y, vec3.x);
        }

        /// <summary>
        /// Returns a Vector2 containing the Z and X components of the Vector3.
        /// </summary>
        /// <param name="vec3">The source Vector3.</param>
        /// <returns>A Vector2 (z, x).</returns>
        public static Vector2 ZX(this Vector3 vec3)
        {
            return new Vector2(vec3.z, vec3.x);
        }

        /// <summary>
        /// Returns a Vector2 containing the Y and Z components of the Vector3.
        /// </summary>
        /// <param name="vec3">The source Vector3.</param>
        /// <returns>A Vector2 (y, z).</returns>
        public static Vector2 YZ(this Vector3 vec3)
        {
            return new Vector2(vec3.y, vec3.z);
        }

        /// <summary>
        /// Returns a Vector2 containing the Z and Y components of the Vector3.
        /// </summary>
        /// <param name="vec3">The source Vector3.</param>
        /// <returns>A Vector2 (z, y).</returns>
        public static Vector2 ZY(this Vector3 vec3)
        {
            return new Vector2(vec3.z, vec3.y);
        }

        #endregion

        #region Vector2 to Vector3

        /// <summary>
        /// Creates a Vector3 using the Vector2 X and Y components, with a custom Z value.
        /// </summary>
        /// <param name="vec2">The source Vector2.</param>
        /// <param name="z">The Z component. Defaults to 0.</param>
        /// <returns>A Vector3 (x, y, z).</returns>
        public static Vector3 XY(this Vector2 vec2, float z = 0f)
        {
            return new Vector3(vec2.x, vec2.y, z);
        }

        /// <summary>
        /// Creates a Vector3 using the Vector2 X and Y components mapped to X and Z, with a custom Y value.
        /// </summary>
        /// <param name="vec2">The source Vector2.</param>
        /// <param name="y">The Y component. Defaults to 0.</param>
        /// <returns>A Vector3 (x, y, vec2.y).</returns>
        public static Vector3 XZ(this Vector2 vec2, float y = 0f)
        {
            return new Vector3(vec2.x, y, vec2.y);
        }

        /// <summary>
        /// Creates a Vector3 using the Vector2 Y and X components, with a custom Z value.
        /// </summary>
        /// <param name="vec2">The source Vector2.</param>
        /// <param name="z">The Z component. Defaults to 0.</param>
        /// <returns>A Vector3 (y, x, z).</returns>
        public static Vector3 YX(this Vector2 vec2, float z = 0f)
        {
            return new Vector3(vec2.y, vec2.x, z);
        }

        /// <summary>
        /// Creates a Vector3 using the Vector2 Y and X components mapped to Z and X, with a custom Y value.
        /// </summary>
        /// <param name="vec2">The source Vector2.</param>
        /// <param name="y">The Y component. Defaults to 0.</param>
        /// <returns>A Vector3 (vec2.y, y, vec2.x).</returns>
        public static Vector3 ZX(this Vector2 vec2, float y = 0f)
        {
            return new Vector3(vec2.y, y, vec2.x);
        }

        /// <summary>
        /// Creates a Vector3 using the Vector2 X and Y components mapped to Y and Z, with a custom X value.
        /// </summary>
        /// <param name="vec2">The source Vector2.</param>
        /// <param name="x">The X component. Defaults to 0.</param>
        /// <returns>A Vector3 (x, vec2.x, vec2.y).</returns>
        public static Vector3 YZ(this Vector2 vec2, float x = 0f)
        {
            return new Vector3(x, vec2.x, vec2.y);
        }

        /// <summary>
        /// Creates a Vector3 using the Vector2 Y and X components mapped to Y and Z positions, with a custom X value.
        /// </summary>
        /// <param name="vec2">The source Vector2.</param>
        /// <param name="x">The X component. Defaults to 0.</param>
        /// <returns>A Vector3 (x, vec2.y, vec2.x).</returns>
        public static Vector3 ZY(this Vector2 vec2, float x = 0f)
        {
            return new Vector3(x, vec2.y, vec2.x);
        }

        #endregion
    }
}