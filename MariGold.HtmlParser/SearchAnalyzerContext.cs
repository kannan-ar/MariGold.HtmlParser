namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal sealed class SearchAnalyzerContext : IAnalyzerContext
    {
        public event Action<HtmlAnalyzer> OnAnalyzerChange;
        public event Action<int> OnPositionChange;

        public string Html { get; }

        public int EOF { get; }

        public IList<IOpenTag> OpenTags { get; }

        public IList<ICloseTag> CloseTags { get; }

        public HtmlContext HtmlContext { get; }

        public HtmlNode PreviousNode { get; set; }

        public SearchAnalyzerContext(string html)
        {
            this.Html = html;
            this.EOF = html.Length;

            this.OpenTags = new List<IOpenTag>()
            {
                new OpenTagAnalyzer(this),
                new MetaTagAnalyzer(this),
                new CommentAnalyzer(this)
            };

            this.CloseTags = new List<ICloseTag>()
            {
                new CloseTagAnalyzer(this)
            };

            HtmlContext = new HtmlContext(html);
        }

        public void SetAnalyzer(HtmlAnalyzer analyzer)
        {
            OnAnalyzerChange?.Invoke(analyzer);
        }

        public void SetPosition(int position)
        {
            OnPositionChange?.Invoke(position);
        }

        public HtmlAnalyzer GetTextAnalyzer(int position)
        {
            return new TextAnalyzer(this, position);
        }

        public HtmlAnalyzer GetTextAnalyzer(int position, HtmlNode parent)
        {
            return new TextAnalyzer(this, position, parent);
        }
    }
}
