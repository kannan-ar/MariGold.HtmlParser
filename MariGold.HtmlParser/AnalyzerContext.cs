namespace MariGold.HtmlParser
{
    using System.Collections.Generic;

    internal sealed class AnalyzerContext : IAnalyzerContext
    {
        private readonly HtmlParser parser;

        public string Html { get; }

        public int EOF { get; }

        public IList<IOpenTag> OpenTags { get; }

        public IList<ICloseTag> CloseTags { get; }

        public HtmlContext HtmlContext { get; }

        public HtmlNode PreviousNode { get; set; }

        public AnalyzerContext(string html, HtmlParser parser)
        {
            this.Html = html;
            this.parser = parser;
            this.EOF = html.Length;

            this.OpenTags = CreateOpenTags();
            this.CloseTags = CreateCloseTags();

            //If parent is not null, we can use that html context
            HtmlContext = new HtmlContext(html);
        }

        private IList<IOpenTag> CreateOpenTags()
        {
            return new List<IOpenTag>()
            {
                new OpenTagAnalyzer(this),
                new CommentAnalyzer(this),
                new MetaTagAnalyzer(this)
            };
        }

        private IList<ICloseTag> CreateCloseTags()
        {
            return new List<ICloseTag>()
            {
                new CloseTagAnalyzer(this)
            };
        }

        public void SetAnalyzer(HtmlAnalyzer analyzer)
        {
            parser.SetAnalyzer(analyzer);
        }

        public void SetPosition(int position)
        {
            parser.SetPosition(position);
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
