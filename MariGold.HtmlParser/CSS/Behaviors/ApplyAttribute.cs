namespace MariGold.HtmlParser
{
	using System;
	using System.Collections.Generic;
	
	internal sealed class ApplyAttribute : CSSBehavior
	{
		private string selectorText;
		
		internal ApplyAttribute(ISelectorContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			this.context = context;
		}
		
		internal override bool IsValidBehavior(string selectorText)
		{
			this.selectorText = string.Empty;
			
			AttributeSelector selector = new AttributeSelector(context);
			
			if (selector.IsValidNode(selectorText))
			{
				this.selectorText = selectorText;
				return true;
			}
			
			return false;
		}
		
		internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
		{
			CSSelector selector = new AttributeSelector(context).Parse(this.selectorText);
			
			if (selector != null)
			{
				selector.Parse(node, htmlStyles);
			}
		}
	}
}
