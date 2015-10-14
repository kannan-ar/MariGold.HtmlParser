namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class ElementSelector : CSSelector
    {
        private readonly Regex regex;
        private readonly string currentSelector;
        private readonly string selectorText;

        internal ElementSelector(ISelectorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.context = context;
            regex = new Regex(@"^([a-zA-Z]+[0-9]*)+(:[a-zA-Z]+[0-9]*)*");
        }

        private ElementSelector(string currentSelector, string selectorText, ISelectorContext context)
        {
            this.currentSelector = currentSelector;
            this.selectorText = selectorText;
            this.context = context;
        }

        private void ApplyIfMatch(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            if (IsValidNode(node))
            {
                node.CopyHtmlStyles(htmlStyles, SelectorWeight.Element);
            }
        }

        internal override CSSelector Parse(string selector)
        {
            Match match = regex.Match(selector);

            if (match.Success)
            {
                string trimmedSelector = selector.Substring(match.Value.Length);
                return new ElementSelector(match.Value, trimmedSelector, context);
            }
            else
            {
                return PassToSuccessor(selector);
            }
        }

        internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            if (string.IsNullOrEmpty(selectorText))
            {
                ApplyIfMatch(node, htmlStyles);
            }
            else
            {
                ParseBehaviour(selectorText, node, htmlStyles);
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

            if (string.Compare(node.Tag, currentSelector, true) == 0)
            {
                isValid = true;
            }

            return isValid;
        }
    }
}
