namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class ApplyNextElement : CSSBehavior
    {
        private readonly Regex regex;

        private string selectorText;

        internal ApplyNextElement(ISelectorContext context)
        {
            this.context = context ?? throw new ArgumentNullException("context");

            regex = new Regex(@"^\s*\+\s*");
        }

        internal override bool IsValidBehavior(string selectorText)
        {
            this.selectorText = string.Empty;

            Match match = regex.Match(selectorText);

            if (match.Success)
            {
                this.selectorText = selectorText.Substring(match.Value.Length);
            }

            return match.Success;
        }

        internal override void Parse(HtmlNode node, Specificity specificity, List<HtmlStyle> htmlStyles)
        {
            if (node.Next != null)
            {
                HtmlNode temp = node.GetNext();

                //Empty text nodes can be avoid. This loop will skip those.
                while (temp != null && temp.Tag == HtmlTag.TEXT)
                {
                    temp = temp.GetNext();
                }

                if (temp != null)
                {
                    if (context.ParseSelector(this.selectorText, out CSSelector nextSelector))
                    {
                        if (nextSelector != null && nextSelector.IsValidNode(temp))
                        {
                            nextSelector.AddSpecificity(specificity);
                            nextSelector.Parse(temp, htmlStyles);
                        }
                    }
                }
            }
        }
    }
}
