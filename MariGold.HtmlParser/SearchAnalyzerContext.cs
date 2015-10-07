namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal sealed class SearchAnalyzerContext : IAnalyzerContext
    {
        private readonly string html;
        private readonly int eof;
        private readonly IList<IOpenTag> openTags;
        private readonly IList<ICloseTag> closeTags;
        private readonly HtmlContext htmlContext;

        private HtmlNode previousNode;

        public event Action<HtmlAnalyzer> OnAnalyzerChange;
        public event Action<int> OnPositionChange;

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

        public HtmlNode PreviousNode
        {
            get
            {
                return previousNode;
            }

            set
            {
                previousNode = value;
            }
        }

        public SearchAnalyzerContext(string html)
        {
            this.html = html;
            this.eof = html.Length;

            this.openTags = new List<IOpenTag>()
            {
                new OpenTagAnalyzer(this),
                new MetaTagAnalyzer(this),
                new CommentAnalyzer(this)
            };

            this.closeTags = new List<ICloseTag>()
            {
                new CloseTagAnalyzer(this)
            };

            htmlContext = new HtmlContext(html);
        }

        public void SetAnalyzer(HtmlAnalyzer analyzer)
        {
            if (OnAnalyzerChange != null)
            {
                OnAnalyzerChange(analyzer);
            }
        }

        public void SetPosition(int position)
        {
            if (OnPositionChange != null)
            {
                OnPositionChange(position);
            }
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
