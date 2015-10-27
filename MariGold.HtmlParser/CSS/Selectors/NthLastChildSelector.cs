namespace MariGold.HtmlParser
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	
	internal sealed class NthLastChildSelector : CSSelector
	{
		internal NthLastChildSelector(ISelectorContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			this.context = context;
		}
		
		internal override bool Prepare(string selector)
		{
			throw new NotImplementedException();
		}
		
		internal override bool IsValidNode(HtmlNode node)
		{
			throw new NotImplementedException();
		}
		
		internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
		{
			throw new NotImplementedException();
		}
		
		internal override void ApplyStyle(HtmlNode node, List<HtmlStyle> htmlStyles)
		{
			throw new NotImplementedException();
		}
	}
}
