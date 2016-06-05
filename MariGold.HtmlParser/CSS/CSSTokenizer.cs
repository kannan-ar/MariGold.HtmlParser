namespace MariGold.HtmlParser
{
    using System;

    internal static class CSSTokenizer
    {
        internal const char openBrace = '{';
        internal const char closeBrace = '}';
        internal const string openComment = "/*";
        internal const string closeComment = "*/";

        internal static string FindOpenCloseBraceArea(
           string styleText,
           int position,
           out int closeBraceIndex)
        {
            int eof = styleText.Length;
            closeBraceIndex = -1;

            //One open brace is already found before calling this function.
            for (int openCount = 1, i = position; openCount > 0 && position < eof; ++i)
            {
                if (styleText[i] == closeBrace)
                {
                    closeBraceIndex = i;
                    --openCount;
                }

                if (styleText[i] == openBrace)
                {
                    ++openCount;
                }
            }

            if (closeBraceIndex > position)
            {
                return styleText.Substring(position, closeBraceIndex - position);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
