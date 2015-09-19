namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal abstract class CSSelector
    {
        protected CSSelector successor;
        protected List<HtmlStyle> styles;

        public abstract SelectorWeight Weight { get; }
        public abstract CSSelector Parse(string selector);

        public CSSelector()
        {
            styles = new List<HtmlStyle>();
        }

        public CSSelector SetSuccessor(CSSelector selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            successor = selector;

            return this;
        }

        public void AddStyle(HtmlStyle style)
        {
            styles.Add(style);
        }
    }
}
