namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class ApplyAllNextElement : CSSBehavior
    {
        private readonly Regex regex;
        
        private string selectorText;

        internal ApplyAllNextElement(ISelectorContext context)
        {
            this.context = context ?? throw new ArgumentNullException("context");

            regex = new Regex(@"^\s*~\s*");
        }

        private void ApplyStyle(CSSelector nextSelector, Specificity specificity, HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            if (node.Next != null)
            {
            	if (nextSelector.IsValidNode(node.GetNext()))
                {
                    nextSelector.AddSpecificity(specificity);
            		nextSelector.Parse(node.GetNext(), htmlStyles);
                }

                ApplyStyle(nextSelector, specificity, node.GetNext(), htmlStyles);
            }
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
            if (context.ParseSelector(this.selectorText, out CSSelector nextSelector))
            {
                if (nextSelector != null)
                {
                    ApplyStyle(nextSelector, specificity, node, htmlStyles);
                }
            }
        }
    }
}
