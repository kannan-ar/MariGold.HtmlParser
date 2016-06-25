namespace MariGold.HtmlParser
{
    using System;
    using System.Globalization;

    internal static class HtmlStringComparer
    {
        internal static bool CompareStringInvariantCultureIgnoreCase(this string source, string value)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(value))
            {
                return false;
            }

            return string.Compare(source, value, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        internal static bool Contains(string[] array, string value)
        {
            StringComparer stringComparer = StringComparer.Create(CultureInfo.InvariantCulture, true);

            foreach (string item in array)
            {
                if (stringComparer.Compare(item, value) == 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
