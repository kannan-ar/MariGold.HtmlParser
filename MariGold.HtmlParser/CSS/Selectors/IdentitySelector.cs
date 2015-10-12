namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class IdentitySelector : CSSelector
    {
        private const string key = "id";
        
        private Regex regex;
        private string currentSelector;
        private string selectorText;

        internal IdentitySelector(ISelectorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.context = context;
            regex = new Regex("^#[-_]*([a-zA-Z]+[0-9_-]*)+");
        }

        internal IdentitySelector(string currentSelector, string selectorText, ISelectorContext context)
        {
            this.currentSelector = currentSelector;
            this.selectorText = selectorText;
            this.context = context;
        }

        private void ApplyIfMatch(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            string id;

            if (node.Attributes.TryGetValue(key, out id))
            {
                if (string.Compare(currentSelector, id, true) == 0)
                {
                    node.CopyHtmlStyles(htmlStyles, SelectorWeight.Identity);
                }
            }
        }

        internal override CSSelector Parse(string selector)
        {
            Match match = regex.Match(selector);

            if (match.Success)
            {
                string trimmedSelector = selector.Substring(match.Value.Length);

                return new IdentitySelector(
                    match.Value.Replace("#", string.Empty), trimmedSelector, context);
            }
            else
            {
                return PassToSuccessor(selector);
            }
        }

        internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            if(string.IsNullOrEmpty(selectorText))
            {
                ApplyIfMatch(node, htmlStyles);
            }
            else
            {
                ParseBehaviour(selectorText, node, htmlStyles);
            }
        }
    }
}
