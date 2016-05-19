namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal sealed class CSSElement
    {
        private readonly string selector;
        private readonly List<HtmlStyle> htmlStyles;

        internal string Selector
        {
            get
            {
                return selector;
            }
        }

        internal List<HtmlStyle> HtmlStyles
        {
            get
            {
                return htmlStyles;
            }
        }

        internal CSSElement(string selector, List<HtmlStyle> htmlStyles)
        {
            this.selector = selector;
            this.htmlStyles = htmlStyles;
        }
    }
}
