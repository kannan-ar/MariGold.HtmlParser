namespace MariGold.HtmlParser
{
    using System;

    internal static class StringUtility
    {
        internal static bool CompareStringInvariantCultureIgnoreCase(this string source, string value)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(value))
            {
                return false;
            }

            return string.Compare(source, value, StringComparison.InvariantCultureIgnoreCase) == 0;
        }
    }
}
