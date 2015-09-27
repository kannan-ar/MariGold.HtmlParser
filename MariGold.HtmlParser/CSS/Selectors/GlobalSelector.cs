namespace MariGold.HtmlParser
{
    using System;

    internal sealed class GlobalSelector : CSSelector
    {
        private const string globalSelector = "*";

        internal override SelectorWeight Weight
        {
            get
            {
                return SelectorWeight.Global;
            }
        }

        internal override CSSelector Parse(string selector)
        {
            if (selector == globalSelector)
            {
                return new GlobalSelector();
            }
            else
            {
                return PassToSuccessor(selector);
            }
        }

        internal override void Parse(HtmlNode node)
        {
            node.CopyHtmlStyles(styles);
        }
    }
}
