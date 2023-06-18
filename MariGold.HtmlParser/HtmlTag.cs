namespace MariGold.HtmlParser;

internal static class HtmlTag
{
    private static readonly string[] nonContainerTags = { "br", "img", "input", "link", "meta", "hr" };

    public const char openAngle = '<';
    public const char closeAngle = '>';
    public const char escapeChar = '/';
    public const char hypen = '-';
    public const char equalSign = '=';
    public const char singleQuote = '\'';
    public const char doubleQuote = '"';
    public const char exclamation = '!';
    public const char space = ' ';

    public const string TEXT = "#text";
    public const string COMMENT = "#comment";
    public const string STYLE = "style";
    public const string LINK = "link";

    public static bool IsSelfClosing(string tag)
    {
        if (string.IsNullOrEmpty(tag))
        {
            return false;
        }

        tag = tag.Trim();

        foreach (string tags in nonContainerTags)
        {
            if (tags == tag)
            {
                return true;
            }

        }

        return false;
    }
}
