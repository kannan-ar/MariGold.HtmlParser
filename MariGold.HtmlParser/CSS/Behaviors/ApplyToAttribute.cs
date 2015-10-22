namespace MariGold.HtmlParser
{
	using System;
	using System.Collections.Generic;
	
	internal sealed class ApplyToAttribute : ICSSBehavior
	{
		private readonly ISelectorContext context;
		
		internal ApplyToAttribute(ISelectorContext context)
		{
			if (context == null) 
			{
				throw new ArgumentNullException("context");
			}

			this.context = context;
		}
		
		public bool IsValidBehavior(string selectorText)
		{
			AttributeSelector selector = new AttributeSelector(context);
			
			return selector.IsValidNode(selectorText);
		}
		
		public void Do(HtmlNode node, List<HtmlStyle> htmlStyles)
		{
			
		}
	}
}
