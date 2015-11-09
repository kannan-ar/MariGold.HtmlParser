namespace MariGold.HtmlParser.Tests
{
	using System;
	using NUnit.Framework;
	using MariGold.HtmlParser;
	using System.Collections.Generic;

	public static class TestUtility
	{
		public static void AreEqual(HtmlNode node, string tag, string text, string html)
		{
			Assert.AreEqual(tag, node.Tag);
			Assert.AreEqual(text, node.InnerHtml);
			Assert.AreEqual(html, node.Html);
		}

		public static void CheckKeyValuePair(KeyValuePair<string, string> attribute, string key, string value)
		{
			Assert.AreEqual(key, attribute.Key);
			Assert.AreEqual(value, attribute.Value);
		}

		public static string GetFolderPath(string folderName)
		{
			string path = Environment.CurrentDirectory;
			int index = path.IndexOf("bin");

			path = path.Remove(index);

			return path + folderName;
		}

		public static void AnalyzeNode(
			HtmlNode node,
			string tag,
			string text,
			string html,
			HtmlNode parent,
			bool selfClosing,
			bool hasChildren,
			int childrenCount,
			int attributeCount)
		{
			Assert.IsNotNull(node);
			AreEqual(node, tag, text, html);
			Assert.AreEqual(parent, node.Parent);
			Assert.AreEqual(selfClosing, node.SelfClosing);
			Assert.AreEqual(hasChildren, node.HasChildren);
			Assert.AreEqual(childrenCount, node.Children.Count);
			Assert.AreEqual(attributeCount, node.Attributes.Count);
		}

		public static void AnalyzeNode(
			HtmlNode node,
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
			Assert.IsNotNull(node);
			AreEqual(node, tag, text, html);
			Assert.AreEqual(parent, node.Parent);
			Assert.AreEqual(selfClosing, node.SelfClosing);
			Assert.AreEqual(hasChildren, node.HasChildren);
			Assert.AreEqual(childrenCount, node.Children.Count,"Children count is " + node.Children.Count.ToString());
			Assert.AreEqual(attributeCount, node.Attributes.Count, "Attribute count is " + node.Attributes.Count.ToString());
			Assert.AreEqual(styleCount, node.Styles.Count, "Style count is " + node.Styles.Count.ToString());
		}
		
		public static void CheckStyle(KeyValuePair<string,string> style, string styleName,string styleValue)
		{
			Assert.AreEqual(styleName, style.Key);
			Assert.AreEqual(styleValue, style.Value);
		}
	}
}
