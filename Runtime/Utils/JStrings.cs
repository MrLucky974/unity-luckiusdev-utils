using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LuckiusDev.Utils
{
    /// <summary>
    /// Utility class for string generation, formatting, parsing, and visualization.
    /// </summary>
    public static class JStrings
    {
        private const string ALPHANUMERIC_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const string COLORED_TEXT_FORMAT = "<color={0}>{1}</color>";
        private const char FILLED_CHAR = '■';
        private const char EMPTY_CHAR = '□';

        #region Random Strings

        public static string GenerateRandomString(int size)
        {
            char[] stringChars = new char[size];
            for (int i = 0; i < size; i++)
            {
                stringChars[i] = ALPHANUMERIC_CHARACTERS[UnityEngine.Random.Range(0, ALPHANUMERIC_CHARACTERS.Length)];
            }
            return new string(stringChars);
        }

        #endregion

        #region Hashing

        public static int ComputeFNV1aHash(this string s)
        {
            uint hash = 2166136261;
            foreach (char c in s)
            {
                hash = (hash ^ c) * 16777619;
            }
            return unchecked((int)hash);
        }

        #endregion

        #region Number Formatting

        public static string FormatNumberWithSuffix(float number)
        {
            string[] suffixes = { "", "K", "M", "B", "T" };
            int suffixIndex = 0;
            while (number >= 1000f && suffixIndex < suffixes.Length - 1)
            {
                number /= 1000f;
                suffixIndex++;
            }
            return number.ToString("F0") + suffixes[suffixIndex];
        }

        public static string FormatWithCommas(float value) => string.Format("{0:N0}", value);

        public static string FormatTimeHHMMSS(float seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return time.ToString(@"hh\:mm\:ss");
        }

        public static float ParseWithSuffix(string value)
        {
            value = value.ToUpper();
            float multiplier = 1f;
            if (value.EndsWith("K")) multiplier = 1_000f;
            else if (value.EndsWith("M")) multiplier = 1_000_000f;
            else if (value.EndsWith("B")) multiplier = 1_000_000_000f;
            else if (value.EndsWith("T")) multiplier = 1_000_000_000_000f;

            float number = float.Parse(value.TrimEnd('K', 'M', 'B', 'T'));
            return number * multiplier;
        }

        #endregion

        #region Text Sliders & Bars

        public static string GenerateTextSlider(float normalizedValue, int count = 5)
        {
            normalizedValue = Mathf.Clamp01(normalizedValue);
            int filledCount = Mathf.RoundToInt(normalizedValue * count);
            return BuildSlider(filledCount, count);
        }

        public static string GenerateTextSlider(int value, int min, int max, int count = 5)
        {
            float fraction = Mathf.InverseLerp(min, max, value);
            int filledCount = Mathf.RoundToInt(fraction * count);
            return BuildSlider(filledCount, count);
        }

        private static string BuildSlider(int filledCount, int totalCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            for (int i = 0; i < totalCount; i++)
                sb.Append(i < filledCount ? FILLED_CHAR : EMPTY_CHAR);
            sb.Append(']');
            return sb.ToString();
        }

        public static string ProgressBar(float progress, int width = 10, char fill = '#', char empty = '-')
        {
            progress = Mathf.Clamp01(progress);
            int filled = Mathf.RoundToInt(progress * width);
            return "[" + new string(fill, filled) + new string(empty, width - filled) + "]";
        }

        public static string GenerateBarGraph(float[] values, int height = 5)
        {
            StringBuilder sb = new StringBuilder();
            float max = Mathf.Max(values);
            for (int row = height; row >= 1; row--)
            {
                foreach (float val in values)
                {
                    float norm = val / max;
                    sb.Append(norm >= (float)row / height ? "█" : " ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        #endregion

        #region Rich Text Formatters

        public static string Bold(string text) => $"<b>{text}</b>";
        public static string Italic(string text) => $"<i>{text}</i>";
        public static string Size(string text, int size) => $"<size={size}>{text}</size>";
        public static string ColorHex(string text, string hex) => string.Format(COLORED_TEXT_FORMAT, hex, text);

        public static string Rainbowize(string text)
        {
            string[] colors = { "red", "orange", "yellow", "green", "blue", "indigo", "violet" };
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                string color = colors[i % colors.Length];
                sb.AppendFormat("<color={0}>{1}</color>", color, text[i]);
            }
            return sb.ToString();
        }

        public static string FormatColor(string message, Color color)
        {
            return string.Format(COLORED_TEXT_FORMAT, ColorExtensions.ToHex(color), message);
        }

        #endregion

        #region String Utilities

        public static string ToCamelCase(string input)
        {
            string[] words = input.Split(new[] { ' ', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            foreach (string word in words)
                sb.Append(char.ToUpperInvariant(word[0]) + word.Substring(1).ToLowerInvariant());
            return sb.ToString();
        }

        public static string ToSnakeCase(string input)
        {
            return string.Concat(input.Select((c, i) =>
                i > 0 && char.IsUpper(c) ? "_" + char.ToLower(c) : char.ToLower(c).ToString()));
        }

        public static string ToKebabCase(string input)
        {
            return string.Concat(input.Select((c, i) =>
                i > 0 && char.IsUpper(c) ? "-" + char.ToLower(c) : char.ToLower(c).ToString()));
        }

        public static string CapitalizeFirst(string input)
        {
            return string.IsNullOrEmpty(input) ? input : char.ToUpper(input[0]) + input.Substring(1);
        }

        public static string Truncate(string input, int maxLength, string suffix = "...")
        {
            return input.Length <= maxLength ? input : input.Substring(0, maxLength) + suffix;
        }

        #endregion

        #region Debug/Log Helpers

        public static string DumpArray<T>(T[] array, string separator = ", ") => string.Join(separator, array);

        public static string DumpDictionary<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kvp in dict)
            {
                sb.AppendLine($"{kvp.Key}: {kvp.Value}");
            }
            return sb.ToString();
        }

        public static string LogFormattedTable(string[] headers, List<string[]> rows)
        {
            StringBuilder sb = new StringBuilder();
            int[] colWidths = new int[headers.Length];

            for (int i = 0; i < headers.Length; i++)
            {
                colWidths[i] = headers[i].Length;
                foreach (var row in rows)
                {
                    if (row[i].Length > colWidths[i])
                        colWidths[i] = row[i].Length;
                }
            }

            for (int i = 0; i < headers.Length; i++)
                sb.Append(headers[i].PadRight(colWidths[i] + 2));
            sb.AppendLine();

            foreach (var row in rows)
            {
                for (int i = 0; i < row.Length; i++)
                    sb.Append(row[i].PadRight(colWidths[i] + 2));
                sb.AppendLine();
            }

            return sb.ToString();
        }

        #endregion
    }
}
