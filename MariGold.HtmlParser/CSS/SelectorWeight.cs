namespace MariGold.HtmlParser
{
	using System;

	internal enum SelectorWeight
	{
		None = 0,
		Global = 1,
		Child = 2,
		Element = 3,
		Attribute = 4,
		//Attribute and Class have same weight
		Class = 4,
		Identity = 5,
		Inline = 6,
	}
}
