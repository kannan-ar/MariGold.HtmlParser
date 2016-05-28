﻿namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class IdentitySelector : CSSelector
    {
        private const string key = "id";

        private readonly Regex regex;

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

        internal override bool Prepare(string selector)
        {
            Match match = regex.Match(selector);

            this.currentSelector = string.Empty;
            this.selectorText = string.Empty;
            this.specificity = 0;

            if (match.Success)
            {
                this.currentSelector = match.Value.Replace("#", string.Empty);
                this.selectorText = selector.Substring(match.Value.Length);
            }

            return match.Success;
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
            string id;

            if (node.Attributes.TryGetValue(key, out id))
            {
                if (string.Compare(currentSelector, id, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            if (string.IsNullOrEmpty(selectorText))
            {
                ApplyStyle(node, htmlStyles);
            }
            else
            {
                context.ParseSelectorOrBehavior(this.selectorText, CalculateSpecificity(SelectorWeight.Identity), node, htmlStyles);
            }
        }

        internal override void ApplyStyle(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            string id;

            if (node.Attributes.TryGetValue(key, out id))
            {
                if (string.Compare(currentSelector, id, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    node.CopyHtmlStyles(htmlStyles, CalculateSpecificity(SelectorWeight.Identity));
                }
            }

        }
    }
}
