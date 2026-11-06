using UnityEngine;

namespace LuckiusDev.Utils
{
    /// <summary>
    /// A utility class providing access to custom time values
    /// that account for both Unity's global Time.timeScale and a local time scale modifier.
    /// Useful for implementing slow motion, time dilation, or isolated time logic per system.
    /// </summary>
    public static class GameTime
    {
        /// <summary>
        /// Local time scale multiplier applied on top of Unity's global timeScale.
        /// </summary>
        public static float LocalTimeScale { get; set; } = 1f;

        /// <summary>
        /// Delta time affected by both global and local time scales.
        /// </summary>
        public static float ScaledDeltaTime => Time.deltaTime * LocalTimeScale;

        /// <summary>
        /// Fixed delta time affected by both global and local time scales.
        /// </summary>
        public static float ScaledFixedDeltaTime => Time.fixedDeltaTime * LocalTimeScale;

        /// <summary>
        /// Composite effective time scale (Time.timeScale * LocalTimeScale).
        /// </summary>
        public static float EffectiveTimeScale => Time.timeScale * LocalTimeScale;

        // Static constructor registers cleanup on quit
        static GameTime()
        {
            Application.quitting += OnApplicationQuit;
        }

        /// <summary>
        /// Resets local time scale on application quit to avoid persisting values in play mode/editor.
        /// </summary>
        private static void OnApplicationQuit()
        {
            LocalTimeScale = 1f;
        }
    }
}
