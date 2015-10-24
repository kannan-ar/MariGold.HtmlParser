namespace MariGold.HtmlParser
{
	using System;
	
	internal interface IAttachedSelector
	{
		void Parse(string selectorText, CSSelector selector);
	}
}
