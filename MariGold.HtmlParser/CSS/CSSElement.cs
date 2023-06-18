namespace MariGold.HtmlParser;

using System.Collections.Generic;

internal sealed class CSSElement
{
    internal string Selector { get; }

    internal List<HtmlStyle> HtmlStyles { get; }

    internal CSSElement(string selector, List<HtmlStyle> htmlStyles)
    {
        this.Selector = selector;
        this.HtmlStyles = htmlStyles;
    }
}
