namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal sealed class StyleSheet
    {
        private List<CSSelector> selectors;

        internal StyleSheet()
        {
            selectors = new List<CSSelector>();
        }

        internal CSSelector this[int index]
        {
            get
            {
                if (index < 0 || index > selectors.Count)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                return selectors[index];
            }
        }

        internal void Add(CSSelector selector)
        {
            selectors.Add(selector);
        }

        internal void Parse(HtmlNode node)
        {
            foreach(CSSelector selector in selectors)
            {
                selector.Parse(node);
            }
        }
    }
}
