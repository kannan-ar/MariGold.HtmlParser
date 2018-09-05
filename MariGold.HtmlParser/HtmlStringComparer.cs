namespace MariGold.HtmlParser
{
    using System;
    using System.Globalization;

    internal static class HtmlStringComparer
    {
        internal static bool CompareOrdinalIgnoreCase(this string source, string value)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(value))
            {
                return false;
            }

            return string.Equals(source, value, StringComparison.OrdinalIgnoreCase);
        }

        internal static bool Contains(string[] array, string value)
        {
            foreach (string item in array)
            {
                if (string.Equals(item, value, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
