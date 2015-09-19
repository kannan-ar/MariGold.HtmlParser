namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal sealed class StyleSheet
    {
        private List<CSSelector> styles;

        internal StyleSheet()
        {
            styles = new List<CSSelector>();
        }

        internal CSSelector this[int index]
        {
            get
            {
                if (index < 0 || index > styles.Count)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                return styles[index];
            }
        }

        internal void Add(CSSelector selector)
        {
            styles.Add(selector);
        }
    }
}
