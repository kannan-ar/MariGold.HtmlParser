namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class ElementSelector : CSSelector
    {
        private readonly Regex regex;

        private string currentSelector;
        private string selectorText;

        internal ElementSelector(ISelectorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.context = context;
            regex = new Regex(@"^([a-zA-Z]+[0-9]*)+");
        }

        internal override bool Prepare(string selector)
        {
            Match match = regex.Match(selector);

            this.currentSelector = string.Empty;
            this.selectorText = string.Empty;
            this.specificity = 0;

            if (match.Success)
            {
                this.currentSelector = match.Value;
                this.selectorText = selector.Substring(match.Value.Length);
            }

            return match.Success;
        }

        internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            if (string.IsNullOrEmpty(selectorText) && IsValidNode(node))
            {
                ApplyStyle(node, htmlStyles);
            }
            else
            {
                context.ParseSelectorOrBehavior(this.selectorText, CalculateSpecificity(SelectorWeight.Element), node, htmlStyles);
            }
        }

        internal override bool IsValidNode(HtmlNode node)
        {
            if (node == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(currentSelector))
            {
                return false;
            }

            bool isValid = false;

            if (string.Compare(node.Tag, currentSelector, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                isValid = true;
            }

            return isValid;
        }

        internal override void ApplyStyle(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            node.CopyHtmlStyles(htmlStyles, CalculateSpecificity(SelectorWeight.Element));
        }
    }
}
