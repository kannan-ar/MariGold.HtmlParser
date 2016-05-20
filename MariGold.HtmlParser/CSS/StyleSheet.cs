namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal sealed class StyleSheet
    {
        private ISelectorContext context;
        private List<CSSElement> styles;
        private List<MediaQuery> mediaQueries;

        private List<HtmlStyle> CloneStyles(List<HtmlStyle> styles)
        {
            List<HtmlStyle> clonedStyles = new List<HtmlStyle>();

            foreach (HtmlStyle style in styles)
            {
                clonedStyles.Add(style.Clone());
            }

            return clonedStyles;
        }

        private void InterpretStyles(HtmlNode htmlNode)
        {
            string style;

            if (!HtmlStyle.IsNonStyleElement(htmlNode.Tag))
            {
                if (htmlNode.Attributes.TryGetValue("style", out style))
                {
                    CSSParser cssParser = new CSSParser();
                    htmlNode.AddStyles(cssParser.ParseRules(style, SelectorWeight.Inline));
                }

                Parse(htmlNode);
            }

            foreach (HtmlNode node in htmlNode.GetChildren())
            {
                InterpretStyles(node);
            }

            //This loop only needs when the parent is null. If parent is not null, it will loop through all the 
            //child elements thus next nodes processed without this loop.
            if (htmlNode.Parent == null && htmlNode.Next != null)
            {
                InterpretStyles(htmlNode.GetNext());
            }
        }

        internal StyleSheet(ISelectorContext context)
        {
            this.context = context;
            styles = new List<CSSElement>();
            mediaQueries = new List<MediaQuery>();
        }

        internal void AddRange(List<CSSElement> elements)
        {
            styles.AddRange(elements);
        }

        internal void AddElement(CSSElement element)
        {
            styles.Add(element);
        }

        internal void AddMediaQueryRange(List<MediaQuery> mediaQueries)
        {
            mediaQueries.AddRange(mediaQueries);
        }

        internal void AddMediaQuery(MediaQuery mediaQuery)
        {
            mediaQueries.Add(mediaQuery);
        }

        internal void Add(string selector, List<HtmlStyle> htmlStyles)
        {
            styles.Add(new CSSElement(selector, htmlStyles));
        }

        internal void Parse(HtmlNode node)
        {
            foreach (var style in styles)
            {
                foreach (CSSelector selector in context.Selectors)
                {
                    if (selector.Prepare(style.Selector))
                    {
                        if (selector.IsValidNode(node))
                        {
                            selector.Parse(node, CloneStyles(style.HtmlStyles));
                        }

                        break;
                    }
                }
            }
        }

        internal void ApplyStyles(HtmlNode htmlNode)
        {
            InterpretStyles(htmlNode);
        }
    }
}