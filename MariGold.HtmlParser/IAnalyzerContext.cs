namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal interface IAnalyzerContext
    {
        string Html { get; }
        int EOF { get; }
        IList<IOpenTag> OpenTags { get; }
        IList<ICloseTag> CloseTags { get; }
        HtmlContext HtmlContext { get; }
        HtmlNode PreviousNode { get; set; }

        void SetAnalyzer(HtmlAnalyzer analyzer);
        void SetPosition(int position);
        HtmlAnalyzer GetTextAnalyzer(int position);
        HtmlAnalyzer GetTextAnalyzer(int position, HtmlNode parent);
    }
}
