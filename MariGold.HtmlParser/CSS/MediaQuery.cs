namespace MariGold.HtmlParser;

using System.Collections.Generic;
using System.Text.RegularExpressions;

internal sealed class MediaQuery
{
    private readonly Regex mediaRegex;

    internal MediaQuery()
    {
        mediaRegex = new Regex(@"^\s*@media");
    }

    internal bool Process(string selectorText, string styleText, List<MediaQuery> mediaQuries, ref int position)
    {
        List<CSSElement> elements = new();
        Match match = mediaRegex.Match(selectorText);

        if (string.IsNullOrEmpty(styleText))
        {
            return false;
        }

        if (!match.Success)
        {
            return false;
        }

        string style = CSSTokenizer.FindOpenCloseBraceArea(styleText, position + 1, out int closeBraceIndex);

        if (closeBraceIndex > position)
        {
            if (!string.IsNullOrEmpty(style))
            {
                CSSParser.ParseCSS(style, elements, mediaQuries);
                mediaQuries.Add(new ());
            }

            position = closeBraceIndex + 1;
        }

        return match.Success;
    }
}
