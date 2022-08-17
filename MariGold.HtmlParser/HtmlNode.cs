namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Stores all the information of an HTML element including child elements, attributes and CSS styles
    /// </summary>
    public sealed class HtmlNode : IHtmlNode
    {
        private readonly HtmlNode parent;
        private readonly HtmlContext context;
        private readonly int htmlStart;
        private readonly int textStart;
        private int textEnd;
        private int htmlEnd;
        private List<HtmlNode> children;
        private HtmlNode previous;
        private HtmlNode next;
        private Dictionary<string, string> attributes;
        private Dictionary<string, string> styles;
        private Dictionary<string, string> inheritedStyles;
        private readonly List<HtmlStyle> htmlStyles;

        private HtmlNode(
            string tag,
            HtmlNode parent,
            HtmlContext context,
            List<HtmlStyle> htmlStyles,
            List<HtmlStyle> inheritedStyles,
            bool isText,
            int htmlStart,
            int textStart,
            int textEnd,
            int htmlEnd,
            bool selfClosing,
            List<HtmlNode> children,
            HtmlNode previous,
            HtmlNode next,
            Dictionary<string, string> attributes,
            Dictionary<string, string> styles)
        {
            this.Tag = tag;
            this.parent = parent;
            this.context = context;
            this.htmlStyles = htmlStyles;
            //this.inheritedStyles = inheritedStyles.ToDictionary(x => ;
            this.IsText = isText;
            this.htmlStart = htmlStart;
            this.textStart = textStart;
            this.textEnd = textEnd;
            this.htmlEnd = htmlEnd;
            this.SelfClosing = selfClosing;

            if (children != null)
            {
                this.children = new List<HtmlNode>();

                foreach (var child in children)
                {
                    this.children.Add(child);
                }
            }

            this.previous = previous;
            this.next = next;

            if (attributes != null)
            {
                this.attributes = new Dictionary<string, string>();

                foreach (var attribute in attributes)
                {
                    this.attributes.Add(attribute.Key, attribute.Value);
                }
            }

            if (styles != null)
            {
                this.styles = new Dictionary<string, string>();

                foreach (var style in styles)
                {
                    this.styles.Add(style.Key, style.Value);
                }
            }
        }

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

            this.Tag = tag;
            this.htmlStart = htmlStart;
            this.textStart = textStart;
            this.textEnd = -1;
            this.htmlEnd = -1;
            this.context = context;
            this.parent = parent;
            htmlStyles = new List<HtmlStyle>();
            InheritedHtmlStyles = new List<HtmlStyle>();
            IsText = tag == HtmlTag.TEXT;

            if (parent != null)
            {
                parent.AddChild(this);
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

        internal List<HtmlStyle> HtmlStyles
        {
            get
            {
                return htmlStyles;
            }
        }

        internal List<HtmlStyle> InheritedHtmlStyles { get; private set; }

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

        internal void CopyHtmlStyles(List<HtmlStyle> newStyles, Specificity specificity)
        {
            CSSPropertyParser propertyParser = new CSSPropertyParser();

            foreach (HtmlStyle newStyle in newStyles)
            {
                bool found = false;

                newStyle.Specificity = specificity;

                foreach (HtmlStyle style in htmlStyles)
                {
                    if (propertyParser.StyleContains(newStyle, style))
                    {
                        found = true;
                        style.OverWrite(newStyle);
                    }
                }

                if (!found)
                {
                    htmlStyles.Add(newStyle.Clone());
                }
            }
        }

        internal void SetPreviousNode(HtmlNode node)
        {
            previous = node;
        }

        internal void SetNextNode(HtmlNode node)
        {
            next = node;
        }

        internal void SetSelfClosing(bool isClosing)
        {
            SelfClosing = isClosing;
        }

        internal HtmlNode GetParent()
        {
            return parent;
        }

        internal HtmlNode GetNext()
        {
            return next;
        }

        internal HtmlNode GetPrevious()
        {
            return previous;
        }

        internal List<HtmlNode> GetChildren()
        {
            if (children == null)
            {
                children = new List<HtmlNode>();
            }

            return children;
        }

        internal HtmlNode GetChild(int index)
        {
            if (children == null)
            {
                children = new List<HtmlNode>();
            }

            return children[index];
        }

        internal void AddChild(HtmlNode child)
        {
            if (children == null)
            {
                children = new List<HtmlNode>();
            }

            children.Add(child);
        }

        internal string ExtractAttributeValue(string attributeName)
        {
            foreach (KeyValuePair<string, string> attribute in attributes)
            {
                if (string.Equals(attributeName, attribute.Key, StringComparison.OrdinalIgnoreCase))
                {
                    return attribute.Value;
                }
            }

            return string.Empty;
        }

        internal bool HasStyle(string styleName)
        {
            foreach (HtmlStyle style in htmlStyles)
            {
                if (string.Equals(styleName, style.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        internal bool TryGetStyle(string styleName, out HtmlStyle htmlStyle)
        {
            htmlStyle = null;

            foreach (HtmlStyle style in htmlStyles)
            {
                if (string.Equals(styleName, style.Name, StringComparison.OrdinalIgnoreCase))
                {
                    htmlStyle = style;
                    return true;
                }
            }

            return false;
        }

        internal bool TryGetInheritedStyle(string styleName, out HtmlStyle htmlStyle)
        {
            htmlStyle = null;

            if (InheritedHtmlStyles == null || InheritedHtmlStyles.Count == 0)
            {
                return false;
            }

            foreach (HtmlStyle style in InheritedHtmlStyles)
            {
                if (string.Equals(styleName, style.Name, StringComparison.OrdinalIgnoreCase))
                {
                    htmlStyle = style;
                    return true;
                }
            }

            return false;
        }

        internal void RemoveStyle(string styleName)
        {
            for (int i = 0; htmlStyles.Count > i; i++)
            {
                if (string.Equals(styleName, htmlStyles[i].Name, StringComparison.OrdinalIgnoreCase))
                {
                    htmlStyles.RemoveAt(i);
                    break;
                }
            }
        }

        internal void ImportInheritedStyles(List<HtmlStyle> styles)
        {
            InheritedHtmlStyles = new List<HtmlStyle>();

            foreach (HtmlStyle style in styles)
            {
                InheritedHtmlStyles.Add(style.Clone());
            }
        }

        internal void UpdateInheritedStyles(HtmlStyle style)
        {
            CSSPropertyParser propertyParser = new CSSPropertyParser();

            foreach (HtmlStyle inheritedStyle in InheritedHtmlStyles)
            {
                if (propertyParser.StyleContains(style, inheritedStyle))
                {
                    inheritedStyle.ModifyStyle(style.Value);
                    return;
                }
            }

            //If style found in the inheritedStyles, control will return without hit this statment.
            InheritedHtmlStyles.Add(style.Clone());
        }

        /// <summary>
        /// Html tag of the node.
        /// </summary>
        public string Tag { get; }

        /// <summary>
        /// Inner Html string of the node
        /// </summary>
        public string InnerHtml
        {
            get
            {
                return context.Html.Substring(textStart, textEnd - textStart);
            }
        }

        /// <summary>
        /// Html string of the node.
        /// </summary>
        public string Html
        {
            get
            {
                return context.Html.Substring(htmlStart, htmlEnd - htmlStart);
            }
        }

        /// <summary>
        /// Parent node.
        /// </summary>
        public IHtmlNode Parent
        {
            get
            {
                return parent;
            }
        }

        /// <summary>
        /// List of child nodes
        /// </summary>
        public IEnumerable<IHtmlNode> Children
        {
            get
            {
                if (children != null)
                {
                    foreach (var child in children)
                    {
                        yield return child;
                    }
                }
            }
        }

        /// <summary>
        /// Previous Node
        /// </summary>
        public IHtmlNode Previous
        {
            get
            {
                return previous;
            }
        }

        /// <summary>
        /// Next Node
        /// </summary>
        public IHtmlNode Next
        {
            get
            {
                return next;
            }
        }

        /// <summary>
        /// Returns true if the node has any child nodes.
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return children != null && children.Count > 0;
            }
        }

        /// <summary>
        /// Returns true if the html element is self closing
        /// </summary>
        public bool SelfClosing { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsText { get; }

        /// <summary>
        /// Dictionary of html attributes
        /// </summary>
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

        /// <summary>
        /// Dictionary of CSS styles
        /// </summary>
        public Dictionary<string, string> Styles
        {
            get
            {
                if (styles == null || (styles != null && htmlStyles.Count != styles.Count))
                {
                    styles = new Dictionary<string, string>();

                    foreach (HtmlStyle style in htmlStyles)
                    {
                        if (!styles.ContainsKey(style.Name))
                        {
                            styles.Add(style.Name, style.Value);
                        }
                    }
                }

                return styles;
            }
        }

        public Dictionary<string, string> InheritedStyles
        {
            get
            {
                if (inheritedStyles == null || (inheritedStyles != null && InheritedHtmlStyles.Count != inheritedStyles.Count))
                {
                    inheritedStyles = new Dictionary<string, string>();

                    foreach (HtmlStyle style in InheritedHtmlStyles)
                    {
                        if (!inheritedStyles.ContainsKey(style.Name))
                        {
                            inheritedStyles.Add(style.Name, style.Value);
                        }
                    }
                }

                return inheritedStyles;
            }
        }

        public IHtmlNode Clone()
        {
            var node = new HtmlNode(
                this.Tag,
                this.parent,
                this.context,
                this.htmlStyles,
                this.InheritedHtmlStyles,
                this.IsText,
                this.htmlStart,
                this.textStart,
                this.textEnd,
                this.htmlEnd,
                this.SelfClosing,
                this.children,
                previous,
                next,
                attributes,
                styles);

            node.ImportInheritedStyles(this.InheritedHtmlStyles);

            return node;
        }

        public override string ToString()
        {
            return Html;
        }
    }
}
