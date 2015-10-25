namespace MariGold.HtmlParser
{
	using System;
	using System.Collections.Generic;

	internal abstract class CSSBehavior
	{
		protected ISelectorContext context;
    	
		internal abstract bool IsValidBehavior(string selectorText);
		internal abstract void Parse(HtmlNode node, List<HtmlStyle> htmlStyles);
	}
}
