namespace MariGold.HtmlParser
{
	using System;

	internal enum SelectorWeight
	{
		None = 0,
		Global = 0,
		Element = 1,
        Child = 10,
		Attribute = 10,
		//Attribute and Class have same weight
		Class = 10,
		Identity = 100,
		Inline = 1000,
	}
}
