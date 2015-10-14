namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class ApplyImmediateChildren : ICSSBehavior
    {
        private readonly ISelectorContext context;

        private Regex regex;
        private string selectorText;

        internal ApplyImmediateChildren(ISelectorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.context = context;

            regex = new Regex(@"^\s*>\s*");
        }

        public bool IsValidBehavior(string selectorText)
        {
            this.selectorText = string.Empty;

            Match match = regex.Match(selectorText);

            if(match.Success)
            {
                this.selectorText = selectorText.Substring(match.Value.Length);
            }

            return match.Success;
        }

        public void Do(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            if (node.HasChildren)
            {
                CSSelector selector = context.Selector.Parse(selectorText);

                if (selector != null)
                {
                    foreach (HtmlNode child in node.Children)
                    {
                        if (selector.IsValidNode(child))
                        {
                            selector.Parse(child, htmlStyles);
                        }
                    }
                }
            }
        }
    }
}
