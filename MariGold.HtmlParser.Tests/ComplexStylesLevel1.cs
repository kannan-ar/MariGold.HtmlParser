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
		
		[Test]
		public void DivClassAttributeFirstChild()
		{
			string html = @"<style>
                                div[class='cls']:first-child
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
		
		[Test]
		public void CustomAttributeOnly()
		{
			string html = @"<style>
                                [attr]
                                {
                                	font-weight:bold;
                                }
                            </style>
                            <div>one</div>
                            <div attr></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "one", "<div>one</div>", null, false, true, 1, 0, 0);
			
			node = node.Next;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "", "<div attr></div>", null, false, false, 0, 1, 1);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "");
			TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "font-weight", "bold");
			
			Assert.IsNull(node.Next);
		}
		
		[Test]
		public void CustomAttributeWithValueOnly()
		{
			string html = @"<style>
                                [attr='val']
                                {
                                	font-weight:bold;
                                }
                            </style>
                            <div attr='val'>one</div>
                            <div attr></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "one", "<div attr='val'>one</div>", null, false, true, 1, 1, 1);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "val");
			TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "font-weight", "bold");
			
			node = node.Next;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "", "<div attr></div>", null, false, false, 0, 1, 0);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "");
			
			Assert.IsNull(node.Next);
		}
		
		[Test]
		public void DivGlobalStyle()
		{
			string html = @"<style>
								div *
								{
									background-color:red;
								}
							</style>
							<div><p>one</p><hr/></div>
							<p>three</p>
							<span>four</span>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "<p>one</p><hr/>", "<div><p>one</p><hr/></div>", null, false,
				true, 2, 0, 0);
			
			TestUtility.AnalyzeNode(node.Children[0], "p", "one", "<p>one</p>", node, false, true, 1, 0, 1);
			TestUtility.CheckKeyValuePair(node.Children[0].Styles.ElementAt(0), "background-color", "red");
			
			TestUtility.AnalyzeNode(node.Children[1], "hr", "<hr/>", "<hr/>", node, true, false, 0, 0, 1);
			TestUtility.CheckKeyValuePair(node.Children[1].Styles.ElementAt(0), "background-color", "red");
			
			while (node.Tag != "p")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "p", "three", "<p>three</p>", null, false, true, 1, 0, 0);
			
			while (node.Tag != "span")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "span", "four", "<span>four</span>", null, false, true, 1, 0, 0);
		}
		
		[Test]
		public void IdentityLastChildP()
		{
			string html = @"<style>
								#dv:last-child p
								{
									background-color:red;
								}
							</style>
							<div id='dv'><div><p>three</p><span>two</span></div></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "<div><p>three</p><span>two</span></div>", "<div id='dv'><div><p>three</p><span>two</span></div></div>",
				null, false, true, 1, 1, 0);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "id", "dv");
			
			HtmlNode parent = node;
			
			node = node.Children[0];
			
			TestUtility.AnalyzeNode(node, "div", "<p>three</p><span>two</span>", "<div><p>three</p><span>two</span></div>", parent, false, true, 2, 0, 0);
			
			TestUtility.AnalyzeNode(node.Children[0], "p", "three", "<p>three</p>", node, false, true, 1, 0, 1);
			TestUtility.CheckKeyValuePair(node.Children[0].Styles.ElementAt(0), "background-color", "red");
			
			TestUtility.AnalyzeNode(node.Children[1], "span", "two", "<span>two</span>", node, false, true, 1, 0, 0);
		}
		
		[Test]
		public void AttributeImmediateChildrenClass()
		{
			string html = @"<style>
								[attr] > .cls
								{
									background-color:red;
								}
							</style>
							<div attr><div class='cls'>one</div><div>two</div></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "<div class='cls'>one</div><div>two</div>", "<div attr><div class='cls'>one</div><div>two</div></div>",
				null, false, true, 2, 1, 0);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "");
			
			TestUtility.AnalyzeNode(node.Children[0], "div", "one", "<div class='cls'>one</div>", node, false, true, 1, 1, 1);
			TestUtility.CheckKeyValuePair(node.Children[0].Attributes.ElementAt(0), "class", "cls");
			TestUtility.CheckKeyValuePair(node.Children[0].Styles.ElementAt(0), "background-color", "red");
			
			TestUtility.AnalyzeNode(node.Children[1], "div", "two", "<div>two</div>", node, false, true, 1, 0, 0);
			
		}
	}
}
