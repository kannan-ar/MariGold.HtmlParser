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
			this.selectorText = string.Empty;

			Match match = regex.Match(selectorText);

			if (match.Success)
			{
				this.selectorText = selectorText.Substring(match.Value.Length);
			}

			return match.Success;
		}
		
		internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
		{
			throw new NotImplementedException();
		}
	}
}
