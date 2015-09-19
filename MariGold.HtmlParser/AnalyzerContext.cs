namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal sealed class AnalyzerContext : IAnalyzerContext
    {
        private readonly string html;
        private readonly HtmlParser parser;
        private readonly int eof;
        private readonly IList<IOpenTag> openTags;
        private readonly IList<ICloseTag> closeTags;
        private readonly HtmlContext htmlContext;

        public string Html
        {
            get
            {
                return html;
            }
        }

        public int EOF
        {
            get
            {
                return eof;
            }
        }

        public IList<IOpenTag> OpenTags
        {
            get
            {
                return openTags;
            }
        }

        public IList<ICloseTag> CloseTags
        {
            get
            {
                return closeTags;
            }
        }

        public HtmlContext HtmlContext
        {
            get
            {
                return htmlContext;
            }
        }

        public AnalyzerContext(string html, HtmlParser parser)
        {
            this.html = html;
            this.parser = parser;
            this.eof = html.Length;

            this.openTags = CreateOpenTags();
            this.closeTags = CreateCloseTags();

            //If parent is not null, we can use that html context
            htmlContext = new HtmlContext(html);
        }

        private IList<IOpenTag> CreateOpenTags()
        {
            return new List<IOpenTag>()
            {
                new OpenTagAnalyzer(this),
                new MetaTagAnalyzer(this),
                new CommentAnalyzer(this)
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
