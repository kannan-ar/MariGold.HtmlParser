namespace MariGold.HtmlParser
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	
	internal sealed class ApplyLastChild : CSSBehavior
	{
		private readonly Regex regex;
		
		private string selectorText;
		
		internal ApplyLastChild(ISelectorContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			this.context = context;
			
			regex = new Regex("^:last-child");
		}
		
		internal override bool IsValidBehavior(string selectorText)
		{
			throw new NotImplementedException();
		}
		
		internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
		{
			throw new NotImplementedException();
		}
	}
}
