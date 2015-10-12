namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal sealed class GlobalSelector : CSSelector
    {
        private const string globalSelector = "*";

        internal override CSSelector Parse(string selector)
        {
            if (selector == globalSelector)
            {
                return this;
            }
            else
            {
                return PassToSuccessor(selector);
            }
        }

        internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            if (HtmlStyle.IsNonStyleElement(node.Tag))
            {
                node.CopyHtmlStyles(htmlStyles, SelectorWeight.Global);
            }
        }
    }
}
