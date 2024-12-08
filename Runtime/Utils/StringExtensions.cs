using System.Text;
using UnityEngine;

namespace LuckiusDev.Utils
{
    public static class StringExtensions
    {
        private const string ALPHANUMERIC_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public static string GenerateRandomString(int size)
        {
            char[] stringChars = new char[size];
            for (int i = 0; i < size; i++)
            {
                stringChars[i] = ALPHANUMERIC_CHARACTERS[Random.Range(0, ALPHANUMERIC_CHARACTERS.Length)];
            }

            return new string(stringChars);
        }

        public static int ComputeFNV1aHash(this string s)
        {
            uint hash = 2166136261;
            foreach (char c in s)
            {
                hash = (hash ^ c) * 16777619;
            }
            return unchecked((int)hash);
        }

        public static string FormatNumberWithSuffix(float number)
        {
            string[] suffixes = { "", "K", "M", "B", "T" }; // Add more suffixes as needed

            int suffixIndex = 0;
            while (number >= 1000f && suffixIndex < suffixes.Length - 1)
            {
                number /= 1000f;
                suffixIndex++;
            }

            string formattedNumber = number.ToString("F0") + suffixes[suffixIndex];
            return formattedNumber;
        }

        public static string GenerateTextSlider(float normalizedValue, int count = 5)
        {
            normalizedValue = Mathf.Clamp01(normalizedValue);
            int filledCount = Mathf.RoundToInt(normalizedValue * count);

            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            for (int i = 0; i < count; i++)
            {
                if (i < filledCount)
                {
                    sb.Append('■');
                }
                else
                {
                    sb.Append('□');
                }
            }
            sb.Append(']');
            return sb.ToString();
        }

        public static string GenerateTextSlider(int value, int min, int max, int count = 5)
        {
            count = Mathf.Max(count, 0);

            StringBuilder sb = new StringBuilder();

            // Calculate the fraction of the range covered by the current value
            float fraction = Mathf.InverseLerp(min, max, value);
            int filledCount = Mathf.RoundToInt(fraction * count);

            sb.Append('[');
            for (int i = 0; i < count; i++)
            {
                if (i < filledCount)
                {
                    sb.Append('■');
                }
                else
                {
                    sb.Append('□');
                }
            }
            sb.Append(']');

            return sb.ToString();
        }

        private const string COLORED_TEXT_FORMAT = "<color={0}>{1}</color>";
        public static string FormatColor(string message, Color color)
        {
            return string.Format(COLORED_TEXT_FORMAT, "#" + ColorUtility.ToHtmlStringRGBA(color), message);
        }
    }
}