namespace MariGold.HtmlParser
{
    using System;

    /// <summary>
    /// Parses the HTML string given via constructor argument.
    /// </summary>
    public sealed class HtmlTextParser : HtmlParser
    {
        private readonly IAnalyzerContext context;
        private readonly int eof;

        private int index;
        private HtmlAnalyzer analyzer;
        private HtmlNode current;

        private bool FinilizeNodes(int position, ref HtmlNode current)
        {
            if (current != null)
            {
                current.Finilize(position);

                while (current.Parent != null)
                {
                    current = current.GetParent();
                    current.Finilize(position);
                }
            }

            return current != null;
        }

        public override IHtmlNode Current
        {
            get
            {
                return current;
            }
        }

        public HtmlTextParser(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                throw new ArgumentNullException("html");
            }

            context = new AnalyzerContext(html, this);
            index = 0;
            eof = context.EOF;
            analyzer = context.GetTextAnalyzer(index);
        }

        internal override void SetAnalyzer(HtmlAnalyzer newAnalyzer)
        {
            analyzer = newAnalyzer;
        }

        internal override void SetPosition(int position)
        {
            if (position <= context.EOF)
            {
                index = position;
            }
        }

        public override bool Traverse()
        {
            if (index >= eof)
            {
                current = null;
                return false;
            }

            bool canTraverse;

            while (index < eof)
            {
                canTraverse = analyzer.Process(index, ref current);

                ++index;

                if (canTraverse)
                {
                    break;
                }
            }

            canTraverse = FinilizeNodes(index, ref current);

            return canTraverse;
        }

        public override bool FindFirst(string tag)
        {
            index = 0;
            bool canTraverse = false;
            HtmlAnalyzer searchAnalyzer = null;
            bool invalidTag = false;
            SearchAnalyzerContext searchContext = new SearchAnalyzerContext(this.context.Html);

            searchContext.OnAnalyzerChange += (a) =>
            {
                searchAnalyzer = a;
            };

            searchContext.OnPositionChange += (p) =>
            {
                index = p;
            };

            while (index < eof)
            {
                if (searchAnalyzer == null)
                {
                    foreach (IOpenTag openTag in searchContext.OpenTags)
                    {
                        if (openTag.IsOpenTag(index, searchContext.Html))
                        {
                            searchAnalyzer = openTag.GetAnalyzer(index, null);
                            invalidTag = false;
                            searchAnalyzer.OnTagCreate += (t) =>
                            {
                                if (!string.Equals(t, tag, StringComparison.OrdinalIgnoreCase))
                                {
                                    invalidTag = true;
                                }
                            };

                            break;
                        }
                    }
                }
                else
                {
                    canTraverse = searchAnalyzer.Process(index, ref current);

                    if (invalidTag)
                    {
                        searchAnalyzer = null;
                        invalidTag = false;
                    }

                    if (canTraverse)
                    {
                        break;
                    }
                }

                ++index;
            }

            return canTraverse;
        }

        public override void ParseStyles()
        {
            if (current != null)
            {
                CSSTracker cssTracker = new CSSTracker
                {
                    UriSchema = uriSchema,
                    BaseURL = baseUrl
                };

                StyleSheet styleSheet = cssTracker.TrackCSS(current);
                styleSheet.ApplyStyles(current);

                CSSPropertyParser propertyParser = new CSSPropertyParser();
                propertyParser.ParseStyle(current);

                CSSInheritance cssInheritance = new CSSInheritance();
                cssInheritance.Apply(current);
            }
        }

        public override bool Parse()
        {
            HtmlNode temp = null;
            bool isParsed = Traverse();

            if (isParsed)
            {
                temp = current;

                while (Traverse()) ;
            }

            current = temp;

            return isParsed;
        }
    }
}
