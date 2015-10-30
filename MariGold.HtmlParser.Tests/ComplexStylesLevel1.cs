namespace MariGold.HtmlParser.Tests
{
	using System;
	using NUnit.Framework;
	using MariGold.HtmlParser;
	using System.Linq;
	
	[TestFixture]
	public partial class ComplexStyles
	{
		[Test]
		public void DivClassAllNextP()
		{
			string html = @"<style>
                                div.cls~p
                                {
                                	font-weight:bold;
                                }
                            </style>
                            <div class='cls'><p>1</p></div>
							test
							<p>one</p>
							<span>two</span>
							<p>three</p>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			bool divFound = false;
			bool spanFound = false;
			int pCount = 0;
			
			while (node != null)
			{
				if (node.Tag == "div")
				{
					divFound = true;
					TestUtility.AnalyzeNode(node, "div", "<p>1</p>", "<div class='cls'><p>1</p></div>", null, false, true, 1, 1, 0);
					TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
					
					TestUtility.AnalyzeNode(node.Children[0], "p", "1", "<p>1</p>", node, false, true, 1, 0, 0);
				}
				
				if (node.Tag == "span")
				{
					spanFound = true;
					TestUtility.AnalyzeNode(node, "span", "two", "<span>two</span>", null, false, true, 1, 0, 0);
				}
				
				if (node.Tag == "p")
				{
					++pCount;
					
					Assert.AreEqual(1, node.Styles.Count);
					TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "font-weight", "bold");
				}
				
				node = node.Next;
			}
			
			if (!divFound)
			{
				throw new Exception("div tag not found");
			}
			
			if (!spanFound)
			{
				throw new Exception("span tag not found");
			}
			
			if (pCount != 2)
			{
				throw new Exception("mismatch in p count");
			}
		}
		
		[Test]
		public void DivClassImmediateP()
		{
			string html = @"<style>
                                div.cls > p
                                {
                                	font-weight:bold;
                                }
                            </style>
                            <div class='cls'><p>1</p></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "<p>1</p>", "<div class='cls'><p>1</p></div>", null, false, true, 1, 1, 0);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
				
			TestUtility.AnalyzeNode(node.Children[0], "p", "1", "<p>1</p>", node, false, true, 1, 0, 1);
			TestUtility.CheckKeyValuePair(node.Children[0].Styles.ElementAt(0), "font-weight", "bold");
		}
		
		[Test]
		public void DivClassFirstChild()
		{
			string html = @"<style>
                                div.cls:first-child
                                {
                                	font-weight:bold;
                                }
                            </style>
                            <div class='cls'><p>1</p></div><div><p>2</p></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "<p>1</p>", "<div class='cls'><p>1</p></div>", null, false, true, 1, 1, 0);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
				
			TestUtility.AnalyzeNode(node.Children[0], "p", "1", "<p>1</p>", node, false, true, 1, 0, 1);
			TestUtility.CheckKeyValuePair(node.Children[0].Styles.ElementAt(0), "font-weight", "bold");
			
			node = node.Next;
			
			TestUtility.AnalyzeNode(node, "div", "<p>2</p>", "<div><p>2</p></div>", null, false, true, 1, 0, 0);
			TestUtility.AnalyzeNode(node.Children[0], "p", "2", "<p>2</p>", node, false, true, 1, 0, 0);
		}
	}
}
