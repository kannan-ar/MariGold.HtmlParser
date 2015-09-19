namespace MariGold.HtmlParser
{
    using System;

    internal interface ICloseTag
    {
        bool IsCloseTag(int position, string html);
        void Init(int position, HtmlNode current);
        HtmlAnalyzer GetAnalyzer();
    }
}
