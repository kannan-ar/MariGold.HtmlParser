namespace MariGold.HtmlParser
{
	using System;
	using System.Collections.Generic;

	internal abstract class CSSBehavior
	{
		protected ISelectorContext context;
		protected CSSelector selector;
    	
		internal abstract bool IsValidBehavior(string selectorText);
		internal abstract void Parse(HtmlNode node, List<HtmlStyle> htmlStyles);
        
		protected void ParseSelectorOrBehavior(string selectorText, HtmlNode node, List<HtmlStyle> htmlStyles)
		{
			CSSelector csSelector = context.Selector.Parse(selectorText);
								
			if (csSelector != null)
			{
				csSelector.Parse(node, htmlStyles);
			}
			else
			{
				CSSBehavior behavior = context.FindBehavior(selectorText);
									
				if (behavior != null)
				{
					behavior.Parse(node, htmlStyles);
				}
			}
		}
        
		internal void AddCurrentSelector(CSSelector selector)
		{
			this.selector = selector;
		}
	}
}
