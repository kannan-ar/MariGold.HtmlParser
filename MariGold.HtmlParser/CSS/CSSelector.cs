namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal abstract class CSSelector
    {
        protected CSSelector successor;
        protected List<HtmlStyle> styles;

        internal abstract SelectorWeight Weight { get; }
        internal abstract CSSelector Parse(string selector);
        internal abstract void Parse(HtmlNode node);

        internal CSSelector()
        {
            styles = new List<HtmlStyle>();
        }

        protected internal CSSelector PassToSuccessor(string selector)
        {
            if (successor == null)
            {
                return null;
            }

            return successor.Parse(selector);
        }

        internal CSSelector SetSuccessor(CSSelector selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            successor = selector;

            return this;
        }

        internal void AddRange(IEnumerable<HtmlStyle> styles)
        {
            this.styles.AddRange(styles);
        }
    }
}
