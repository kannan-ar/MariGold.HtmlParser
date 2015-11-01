namespace MariGold.HtmlParser.Tests
{
	using System;
	using NUnit.Framework;
	using MariGold.HtmlParser;
	using System.Linq;
	
	[TestFixture]
	public class CSSelectorTypePriority
	{
		[Test]
		public void InlineIdentity()
		{
			string html = @"<style>
                                .cls
                                {
                                	color:#fff;
                                }
                            </style>
                            <div class='cls' style='color:#000'></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "", "<div class='cls' style='color:#000'></div>", null, false, false, 0, 2, 1);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "style", "color:#000");
			TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "#000");
		}
		
		[Test]
		public void ClassAttribute()
		{
			string html = @"<style>
                                .cls
                                {
                                	color:red;
                                }
                                
                                [attr]
                                {
                                	color:blue;
                                }
                            </style>
                            <div class='cls' attr></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "", "<div class='cls' attr></div>", null, false, false, 0, 2, 1);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "attr", "");
			TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "blue");
		}
		
		[Test]
		public void AttributeClass()
		{
			string html = @"<style>
			
                                [attr]
                                {
                                	color:blue;
                                }
                                
                                .cls
                                {
                                	color:red;
                                }
                                
                            </style>
                            <div class='cls' attr></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "", "<div class='cls' attr></div>", null, false, false, 0, 2, 1);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "attr", "");
			TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "red");
		}
	}
}
