namespace MariGold.HtmlParser
{
    using System;

    internal interface IOpenTag
    {
        bool IsOpenTag(int position, string html);
        HtmlAnalyzer GetAnalyzer(int position, HtmlNode parent);
    }
}
