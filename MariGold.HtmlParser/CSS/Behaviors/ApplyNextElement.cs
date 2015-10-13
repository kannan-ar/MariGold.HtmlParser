namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class ApplyNextElement : ICSSBehavior
    {
        private readonly ISelectorContext context;

        private Regex regex;
        private string selectorText;

        internal ApplyNextElement(ISelectorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.context = context;
            regex = new Regex(@"^\s*\+\s*");
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
            if (node.Next != null)
            {
                HtmlNode temp = node.Next;

                //Empty text nodes can be avoid. This loop will skip those.
                while (temp != null && temp.Tag == HtmlTag.TEXT &&
                    temp.Html.Trim() == string.Empty)
                {
                    temp = temp.Next;
                }

                if (temp != null)
                {
                    CSSelector selector = context.Selector.Parse(selectorText);

                    if (selector != null)
                    {
                        selector.Parse(node.Next, htmlStyles);
                    }
                }
            }
        }
    }
}
