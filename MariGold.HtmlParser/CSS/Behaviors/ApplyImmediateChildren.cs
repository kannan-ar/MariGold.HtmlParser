namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class ApplyImmediateChildren : CSSBehavior
    {
        private readonly Regex regex;

        private string selectorText;

        internal ApplyImmediateChildren(ISelectorContext context)
        {
            this.context = context ?? throw new ArgumentNullException("context");

            regex = new Regex(@"^\s*>\s*");
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
            if (node.HasChildren)
            {
                if (context.ParseSelector(this.selectorText, out CSSelector nextSelector))
                {
                    if (nextSelector != null)
                    {
                        foreach (HtmlNode child in node.GetChildren())
                        {
                            if (nextSelector.IsValidNode(child))
                            {
                                nextSelector.AddSpecificity(specificity);
                                nextSelector.Parse(child, htmlStyles);
                            }
                        }
                    }
                }
            }
        }
    }
}
