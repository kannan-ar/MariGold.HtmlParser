namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Globalization;

    internal sealed class ElementSelector : CSSelector
    {
        private readonly Regex regex;

        private string currentSelector;
        private string selectorText;
        private Dictionary<string, Func<string>> specialTags;

        private ElementSelector(ISelectorContext context, string currentSelector, string selectorText, Specificity specificity)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.context = context;
            regex = new Regex(@"^([a-zA-Z]+[0-9]*)+");
            this.currentSelector = currentSelector;
            this.selectorText = selectorText;
            this.specificity = specificity;

            FillSpecialTags();
        }

        internal ElementSelector(ISelectorContext context)
            : this(context, string.Empty, string.Empty, new Specificity())
        {
        }

        private void FillSpecialTags()
        {
            specialTags = new Dictionary<string, Func<string>>();
            specialTags.Add("a:link", () => { return "a"; });
        }

        private bool IsSpecialTag(string selector)
        {
            foreach (var item in specialTags)
            {
                if (selector.StartsWith(item.Key, StringComparison.OrdinalIgnoreCase))
                {
                    this.currentSelector = item.Value();
                    this.selectorText = selector.Substring(item.Key.Length);
                    return true;
                }
            }

            return false;
        }

        internal override bool Prepare(string selector)
        {
            this.currentSelector = string.Empty;
            this.selectorText = string.Empty;
            this.specificity = new Specificity();
            bool success = IsSpecialTag(selector);

            if (!success)
            {
                Match match = regex.Match(selector);

                if (match.Success)
                {
                    this.currentSelector = match.Value;
                    this.selectorText = selector.Substring(match.Value.Length);
                    success = true;
                }
            }

            return success;
        }

        internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            if (string.IsNullOrEmpty(selectorText) && IsValidNode(node))
            {
                ApplyStyle(node, htmlStyles);
            }
            else
            {
                context.ParseSelectorOrBehavior(this.selectorText, CalculateSpecificity(SelectorType.Element), node, htmlStyles);
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

            if (string.Equals(node.Tag, currentSelector, StringComparison.OrdinalIgnoreCase))
            {
                isValid = true;
            }

            return isValid;
        }

        internal override void ApplyStyle(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            node.CopyHtmlStyles(htmlStyles, CalculateSpecificity(SelectorType.Element));
        }

        internal override CSSelector Clone()
        {
            return new ElementSelector(context, currentSelector, selectorText, specificity.Clone());
        }
    }
}
