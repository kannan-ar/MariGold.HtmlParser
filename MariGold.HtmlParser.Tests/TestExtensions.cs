namespace MariGold.HtmlParser.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using NUnit.Framework;
	
	public static class TestExtensions
	{
		public static void AnalyzeNode(
			this HtmlNode node,
			string tag,
			string text,
			string html,
			HtmlNode parent,
			bool selfClosing,
			bool hasChildren,
			int childrenCount,
			int attributeCount,
			int styleCount)
		{
			TestUtility.AnalyzeNode(node, tag, text, html, parent, selfClosing, hasChildren, childrenCount, attributeCount, styleCount);
		}
		
		public static void CheckKeyValuePair(this Dictionary<string,string> dict, int index, string key, string value)
		{
			var attribute = dict.ElementAt(index);
			
			Assert.AreEqual(key, attribute.Key);
			Assert.AreEqual(value, attribute.Value);
		}
	}
}
