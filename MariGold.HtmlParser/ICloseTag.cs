namespace MariGold.HtmlParser
{
    internal interface ICloseTag
    {
        bool IsCloseTag(int position, string html);
        void Init(int position, HtmlNode current);
        HtmlAnalyzer GetAnalyzer();
    }
}
