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
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.context = context;

            regex = new Regex(@"^\s*>\s*");
        }

        internal override bool IsValidBehavior(string selectorText)
        {
            this.selectorText = string.Empty;

            Match match = regex.Match(selectorText);

            if(match.Success)
            {
                this.selectorText = selectorText.Substring(match.Value.Length);
            }

            return match.Success;
        }

        internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            if (node.HasChildren)
            {
                CSSelector nextSelector;
			
				if (context.ParseSelector(this.selectorText, out nextSelector))
				{
					if (nextSelector != null)
					{
						foreach (HtmlNode child in node.Children)
						{
							if (nextSelector.IsValidNode(child))
							{
								nextSelector.Parse(child, htmlStyles);
							}
						}
					}
				}
            }
        }
    }
}
