namespace MariGold.HtmlParser;

internal interface IOpenTag
{
    bool IsOpenTag(int position, string html);
    HtmlAnalyzer GetAnalyzer(int position, HtmlNode parent);
}
