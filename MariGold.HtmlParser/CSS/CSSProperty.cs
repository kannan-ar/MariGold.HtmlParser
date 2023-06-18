namespace MariGold.HtmlParser;

internal abstract class CSSProperty
{
    internal const string fontFamily = "font-family";
    internal const string font = "font";
    internal const string fontSize = "font-size";
    internal const string fontWeight = "font-weight";
    internal const string fontStyle = "font-style";
    internal const string fontVariant = "font-variant";
    internal const string lineHeight = "line-height";
    internal const string textAlign = "text-align";

    internal const string color = "color";
    internal const string textDecoration = "text-decoration";
    internal const string backgroundColor = "background-color";
    internal const string background = "background";
    internal const string transparent = "transparent";

    internal abstract bool AppendStyle(HtmlStyle parentStyle, HtmlNode child);
    internal abstract void ParseStyle(HtmlNode node);
}
