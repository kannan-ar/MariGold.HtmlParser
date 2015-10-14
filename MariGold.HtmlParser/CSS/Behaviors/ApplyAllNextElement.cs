namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class ApplyAllNextElement : ICSSBehavior
    {
        private readonly ISelectorContext context;

        private Regex regex;
        private string selectorText;

        internal ApplyAllNextElement(ISelectorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.context = context;

            regex = new Regex(@"^\s*~\s*");
        }

        private void ApplyStyle(CSSelector selector, HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            if (node.Next != null)
            {
                if (selector.IsValidNode(node.Next))
                {
                    selector.Parse(node.Next, htmlStyles);
                }

                ApplyStyle(selector, node.Next, htmlStyles);
            }
        }

        public bool IsValidBehavior(string selectorText)
        {
            this.selectorText = string.Empty;

            Match match = regex.Match(selectorText);

            if (match.Success)
            {
                this.selectorText = selectorText.Substring(match.Value.Length);
            }

            return match.Success;
        }

        public void Do(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            CSSelector selector = context.Selector.Parse(selectorText);

            if (selector != null)
            {
                ApplyStyle(selector, node, htmlStyles);
            }
        }
    }
}
