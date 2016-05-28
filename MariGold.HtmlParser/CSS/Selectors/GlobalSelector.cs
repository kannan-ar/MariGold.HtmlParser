namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal sealed class GlobalSelector : CSSelector
    {
        private const string globalSelector = "*";

        internal override bool Prepare(string selector)
        {
            this.specificity = 0;

            if (selector == globalSelector)
            {
                return true;
            }

            return false;
        }

        internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            ApplyStyle(node, htmlStyles);
        }

        internal override bool IsValidNode(HtmlNode node)
        {
            if (node == null)
            {
                return false;
            }

            if (node.Tag == HtmlTag.TEXT)
            {
                return false;
            }

            return !HtmlStyle.IsNonStyleElement(node.Tag);
        }

        internal override void ApplyStyle(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            if (IsValidNode(node))
            {
                node.CopyHtmlStyles(htmlStyles, CalculateSpecificity(SelectorWeight.Global));
            }
        }
    }
}
