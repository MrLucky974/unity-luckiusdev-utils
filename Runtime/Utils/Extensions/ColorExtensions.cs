using UnityEngine;

namespace LuckiusDev.Utils.Extensions
{
    /// <summary>
    /// Extension methods for UnityEngine.Color.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Converts a Unity Color to its hexadecimal string representation (including alpha).
        /// Format: #RRGGBBAA (e.g. #FF00FF80 for semi-transparent magenta).
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>A string representing the color in hexadecimal format.</returns>
        public static string ToHex(this Color color)
        {
            // Convert color components (0-1 range) to 0-255 and then to two-digit hex
            int r = Mathf.RoundToInt(color.r * 255);
            int g = Mathf.RoundToInt(color.g * 255);
            int b = Mathf.RoundToInt(color.b * 255);
            int a = Mathf.RoundToInt(color.a * 255);

            // Format as hexadecimal string with leading '#' and uppercase hex values
            return $"#{r:X2}{g:X2}{b:X2}{a:X2}";
        }

        /// <summary>
        /// Returns a copy of the color with a new alpha value.
        /// </summary>
        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        /// <summary>
        /// Returns the grayscale version of the color (average of RGB).
        /// </summary>
        public static Color Grayscale(this Color color)
        {
            float gray = (color.r + color.g + color.b) / 3f;
            return new Color(gray, gray, gray, color.a);
        }

        /// <summary>
        /// Returns the inverted color (RGB components flipped).
        /// </summary>
        public static Color Invert(this Color color)
        {
            return new Color(1f - color.r, 1f - color.g, 1f - color.b, color.a);
        }

        /// <summary>
        /// Converts the color to a hex string in RGB format (without alpha).
        /// </summary>
        public static string ToHexRGB(this Color color)
        {
            int r = Mathf.RoundToInt(color.r * 255);
            int g = Mathf.RoundToInt(color.g * 255);
            int b = Mathf.RoundToInt(color.b * 255);
            return $"#{r:X2}{g:X2}{b:X2}";
        }

        /// <summary>
        /// Linearly interpolates from this color to the target color.
        /// </summary>
        public static Color LerpTo(this Color from, Color to, float t)
        {
            return Color.Lerp(from, to, t);
        }

        /// <summary>
        /// Returns true if two colors are approximately equal within a given tolerance.
        /// </summary>
        public static bool IsApproximately(this Color a, Color b, float tolerance = 0.01f)
        {
            return Mathf.Abs(a.r - b.r) < tolerance &&
                   Mathf.Abs(a.g - b.g) < tolerance &&
                   Mathf.Abs(a.b - b.b) < tolerance &&
                   Mathf.Abs(a.a - b.a) < tolerance;
        }
    }
}
