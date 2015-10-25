namespace MariGold.HtmlParser
{
	using System;
	
	internal sealed class FirstChildSelector : CSSelector
	{
		private string currentSelector;
		private string selectorText;
		
		internal FirstChildSelector(ISelectorContext context)
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
		
		internal override void Parse(HtmlNode node, System.Collections.Generic.List<HtmlStyle> htmlStyles)
		{
			throw new NotImplementedException();
		}
		
		internal override void ApplyStyle(HtmlNode node, System.Collections.Generic.List<HtmlStyle> htmlStyles)
		{
			throw new NotImplementedException();
		}
	}
}
