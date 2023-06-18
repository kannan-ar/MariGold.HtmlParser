namespace MariGold.HtmlParser;

using System;

internal sealed class HtmlContext
{
    internal HtmlContext(string html)
    {
        if (string.IsNullOrEmpty(html))
        {
            throw new ArgumentNullException(nameof(html));
        }

        this.Html = html;
    }

    internal string Html { get; }
}
