namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    public sealed class HtmlNode
    {
        private readonly string tag;
        private readonly HtmlNode parent;
        private readonly HtmlContext context;

        private int htmlStart;
        private int textStart;
        private int textEnd;
        private int htmlEnd;
        private bool selfClosing;
        private List<HtmlNode> children;
        private HtmlNode previous;
        private HtmlNode next;
        private Dictionary<string, string> attributes;
        private Dictionary<string, string> styles;
        private List<HtmlStyle> htmlStyles;

        internal HtmlNode(string tag, int htmlStart, int textStart, HtmlContext context, HtmlNode parent)
        {
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException("tag");
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.tag = tag;
            this.htmlStart = htmlStart;
            this.textStart = textStart;
            this.textEnd = -1;
            this.htmlEnd = -1;
            this.context = context;
            this.parent = parent;
            htmlStyles = new List<HtmlStyle>();

            if (parent != null)
            {
                parent.Children.Add(this);
            }
        }

        internal HtmlNode(string tag, int htmlStart, int textStart, int textEnd, int htmlEnd,
            HtmlContext context, HtmlNode parent)
            : this(tag, htmlStart, textStart, context, parent)
        {
            SetBoundary(textEnd, htmlEnd);
        }

        internal bool IsOpened
        {
            get
            {
                return textEnd == -1 && htmlEnd == -1;
            }
        }

        internal void SetBoundary(int textEnd, int htmlEnd)
        {
            if (textEnd != -1 && textEnd < textStart)
            {
                throw new ArgumentOutOfRangeException("textEnd");
            }

            if (htmlEnd != -1 && htmlEnd <= htmlStart)
            {
                throw new ArgumentOutOfRangeException("htmlEnd");
            }

            this.textEnd = textEnd;
            this.htmlEnd = htmlEnd;
        }

        internal void Finilize(int position)
        {
            if (textEnd == -1)
            {
                textEnd = position;
            }

            if (htmlEnd == -1)
            {
                htmlEnd = position;
            }
        }

        internal void AddStyles(IEnumerable<HtmlStyle> styles)
        {
            htmlStyles.AddRange(styles);
        }

        internal void CopyHtmlStyles(List<HtmlStyle> newStyles, SelectorWeight weight)
        {
            foreach (HtmlStyle newStyle in newStyles)
            {
                bool found = false;

                newStyle.Weight = weight;

                foreach (HtmlStyle style in htmlStyles)
                {
                    if (string.Compare(newStyle.Name, style.Name, true) == 0)
                    {
                        found = true;
                        style.OverWrite(newStyle);
                    }
                }

                if (!found)
                {
                    htmlStyles.Add(newStyle);
                }
            }
        }

        public string Tag
        {
            get
            {
                return tag;
            }
        }

        public string InnerHtml
        {
            get
            {
                return context.Html.Substring(textStart, textEnd - textStart);
            }
        }

        public string Html
        {
            get
            {
                return context.Html.Substring(htmlStart, htmlEnd - htmlStart);
            }
        }

        public HtmlNode Parent
        {
            get
            {
                return parent;
            }
        }

        public List<HtmlNode> Children
        {
            get
            {
                if (children == null)
                {
                    children = new List<HtmlNode>();
                }

                return children;
            }
        }

        public HtmlNode Previous
        {
            get
            {
                return previous;
            }

            internal set
            {
                previous = value;
            }
        }

        public HtmlNode Next
        {
            get
            {
                return next;
            }

            internal set
            {
                next = value;
            }
        }

        public bool HasChildren
        {
            get
            {
                return children != null && children.Count > 0;
            }
        }

        public bool SelfClosing
        {
            get
            {
                return selfClosing;
            }

            internal set
            {
                selfClosing = value;
            }
        }

        public Dictionary<string, string> Attributes
        {
            get
            {
                if (attributes == null)
                {
                    attributes = new Dictionary<string, string>();
                }

                return attributes;
            }
        }

        public Dictionary<string, string> Styles
        {
            get
            {
                if (styles == null)
                {
                    styles = new Dictionary<string, string>();

                    foreach (HtmlStyle style in htmlStyles)
                    {
                        styles.Add(style.Name, style.Value);
                    }
                }

                return styles;
            }
        }
    }
}
