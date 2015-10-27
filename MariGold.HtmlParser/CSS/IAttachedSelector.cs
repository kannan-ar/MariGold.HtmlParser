namespace MariGold.HtmlParser
{
	using System;
	using System.Collections.Generic;
	
	internal interface IAttachedSelector
	{
		bool Prepare(string selector);
		void Parse(HtmlNode node, List<HtmlStyle> htmlStyles);
	}
}
