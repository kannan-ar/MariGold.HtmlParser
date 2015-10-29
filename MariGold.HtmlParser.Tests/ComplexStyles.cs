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
		public void ClassChildrenIdentity()
		{
			string html = @"<style>
                                .cls #dv
                                {
                                	color:#fff;
                                	background-color:#000;
                                }
                            </style>
                            <div class='cls'><div id='dv'>one</div></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node != null)
			{
				if (node.Tag == "div")
				{
					TestUtility.AnalyzeNode(node, "div", "<div id='dv'>one</div>", "<div class='cls'><div id='dv'>one</div></div>", null, false, true, 1, 1, 0);
					TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
					
					TestUtility.AnalyzeNode(node.Children[0], "div", "one", "<div id='dv'>one</div>", node, false, true, 1, 1, 2);
					TestUtility.CheckKeyValuePair(node.Children[0].Attributes.ElementAt(0), "id", "dv");
					TestUtility.CheckKeyValuePair(node.Children[0].Styles.ElementAt(0), "color", "#fff");
					TestUtility.CheckKeyValuePair(node.Children[0].Styles.ElementAt(1), "background-color", "#000");
				}
				
				node = node.Next;
			}
		}
		
		[Test]
		public void IdentityChildrenClass()
		{
			string html = @"<style>
                                #dv .cls
                                {
                                	color:#fff;
                                	background-color:#000;
                                }
                            </style>
                            <div  id='dv'><div class='cls'>one</div></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node != null)
			{
				if (node.Tag == "div")
				{
					TestUtility.AnalyzeNode(node, "div", "<div class='cls'>one</div>", "<div  id='dv'><div class='cls'>one</div></div>", null, false, true, 1, 1, 0);
					TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "id", "dv");
					
					TestUtility.AnalyzeNode(node.Children[0], "div", "one", "<div class='cls'>one</div>", node, false, true, 1, 1, 2);
					TestUtility.CheckKeyValuePair(node.Children[0].Attributes.ElementAt(0), "class", "cls");
					TestUtility.CheckKeyValuePair(node.Children[0].Styles.ElementAt(0), "color", "#fff");
					TestUtility.CheckKeyValuePair(node.Children[0].Styles.ElementAt(1), "background-color", "#000");
				}
				
				node = node.Next;
			}
		}
		
		[Test]
		public void IdentityChildrenNoClass()
		{
			string html = @"<style>
                                #dv .cls
                                {
                                	color:#fff;
                                	background-color:#000;
                                }
                            </style>
                            <div  id='dv'><div>one</div></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node != null)
			{
				if (node.Tag == "div")
				{
					TestUtility.AnalyzeNode(node, "div", "<div>one</div>", "<div  id='dv'><div>one</div></div>", null, false, true, 1, 1, 0);
					TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "id", "dv");
					
					TestUtility.AnalyzeNode(node.Children[0], "div", "one", "<div>one</div>", node, false, true, 1, 0, 0);

				}
				
				node = node.Next;
			}
		}
		
		[Test]
		public void IdentityChildrenWithInvalidClass()
		{
			string html = @"<style>
                                #dv .cls
                                {
                                	color:#fff;
                                	background-color:#000;
                                }
                            </style>
                            <div  id='dv'><div class='cts'>one</div></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node != null)
			{
				if (node.Tag == "div")
				{
					TestUtility.AnalyzeNode(node, "div", "<div class='cts'>one</div>", "<div  id='dv'><div class='cts'>one</div></div>", null, false, true, 1, 1, 0);
					TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "id", "dv");
					
					TestUtility.AnalyzeNode(node.Children[0], "div", "one", "<div class='cts'>one</div>", node, false, true, 1, 1, 0);
					TestUtility.CheckKeyValuePair(node.Children[0].Attributes.ElementAt(0), "class", "cts");
				}
				
				node = node.Next;
			}
		}
		
		[Test]
		public void IdentityToAllP()
		{
			string html = @"<style>
                                #dv p
                                {
                                	font-size:20px;
                                }
                            </style>
                            <div  id='dv'><div><p>one</p></div><span><p>two</p></span></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "<div><p>one</p></div><span><p>two</p></span>", "<div  id='dv'><div><p>one</p></div><span><p>two</p></span></div>", null,
				false, true, 2, 1, 0);
			
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "id", "dv");
			
			TestUtility.AnalyzeNode(node.Children[0], "div", "<p>one</p>", "<div><p>one</p></div>", node, false, true, 1, 0, 0);
			
			TestUtility.AnalyzeNode(node.Children[0].Children[0], "p", "one", "<p>one</p>", node.Children[0], false, true, 1, 0, 1);
			TestUtility.CheckKeyValuePair(node.Children[0].Children[0].Styles.ElementAt(0), "font-size", "20px");
			
			TestUtility.AnalyzeNode(node.Children[1], "span", "<p>two</p>", "<span><p>two</p></span>", node, false, true, 1, 0, 0);
			
			TestUtility.AnalyzeNode(node.Children[1].Children[0], "p", "two", "<p>two</p>", node.Children[1], false, true, 1, 0, 1);
			TestUtility.CheckKeyValuePair(node.Children[1].Children[0].Styles.ElementAt(0), "font-size", "20px");
		}
		
		[Test]
		public void ClassDivImmediateP()
		{
			string html = @"<style>
                                .cls > p
                                {
                                	font-weight:bold;
                                }
                            </style>
                            <div class='cls'><div><p>one</p></div><p>two</p></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "<div><p>one</p></div><p>two</p>", "<div class='cls'><div><p>one</p></div><p>two</p></div>",
				null, false, true, 2, 1, 0);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
			
			TestUtility.AnalyzeNode(node.Children[0], "div", "<p>one</p>", "<div><p>one</p></div>", node, false, true, 1, 0, 0);
			TestUtility.AnalyzeNode(node.Children[0].Children[0], "p", "one", "<p>one</p>", node.Children[0], false, true, 1, 0, 0);
			TestUtility.AnalyzeNode(node.Children[0].Children[0].Children[0], "#text", "one", "one",
				node.Children[0].Children[0], false, false, 0, 0, 0);
			
			TestUtility.AnalyzeNode(node.Children[1], "p", "two", "<p>two</p>", node, false, true, 1, 0, 1);
			TestUtility.CheckKeyValuePair(node.Children[1].Styles.ElementAt(0), "font-weight", "bold");
		}
		
		[Test]
		public void ClassDivImmediatePs()
		{
			string html = @"<style>
                                .cls > p
                                {
                                	font-weight:bold;
                                }
                            </style>
                            <div class='cls'><p>new</p><div><p>one</p></div><p>two</p></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "<p>new</p><div><p>one</p></div><p>two</p>", "<div class='cls'><p>new</p><div><p>one</p></div><p>two</p></div>",
				null, false, true, 3, 1, 0);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
			
			TestUtility.AnalyzeNode(node.Children[0], "p", "new", "<p>new</p>", node, false, true, 1, 0, 1);
			TestUtility.CheckKeyValuePair(node.Children[0].Styles.ElementAt(0), "font-weight", "bold");
			
			TestUtility.AnalyzeNode(node.Children[1], "div", "<p>one</p>", "<div><p>one</p></div>", node, false, true, 1, 0, 0);
			TestUtility.AnalyzeNode(node.Children[1].Children[0], "p", "one", "<p>one</p>", node.Children[1], false, true, 1, 0, 0);
			TestUtility.AnalyzeNode(node.Children[1].Children[0].Children[0], "#text", "one", "one",
				node.Children[1].Children[0], false, false, 0, 0, 0);
			
			TestUtility.AnalyzeNode(node.Children[2], "p", "two", "<p>two</p>", node, false, true, 1, 0, 1);
			TestUtility.CheckKeyValuePair(node.Children[2].Styles.ElementAt(0), "font-weight", "bold");
		}
		
		[Test]
		public void PNextSpan()
		{
			string html = @"<style>
                                p + span
                                {
                                	font-weight:bold;
                                }
                            </style>
                            <p>one</p>
							<span>two</span>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			bool pFound = false;
			bool spanFound = false;
			
			while (node != null)
			{
				if (node.Tag == "p")
				{
					pFound = true;
					TestUtility.AnalyzeNode(node, "p", "one", "<p>one</p>", null, false, true, 1, 0, 0);
				}
				
				if (node.Tag == "span")
				{
					spanFound = true;
					TestUtility.AnalyzeNode(node, "span", "two", "<span>two</span>", null, false, true, 1, 0, 1);
					TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "font-weight", "bold");
				}
				
				node = node.Next;
			}
			
			if (!pFound)
			{
				throw new Exception("p tag not found");
			}
			
			if (!spanFound)
			{
				throw new Exception("span tag not found");
			}
		}
		
		[Test]
		public void PNextTextSpan()
		{
			string html = @"<style>
                                p + span
                                {
                                	font-weight:bold;
                                }
                            </style>
                            <p>one</p>
                            test
							<span>two</span>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			bool pFound = false;
			bool spanFound = false;
			
			while (node != null)
			{
				if (node.Tag == "p")
				{
					pFound = true;
					TestUtility.AnalyzeNode(node, "p", "one", "<p>one</p>", null, false, true, 1, 0, 0);
				}
				
				if (node.Tag == "span")
				{
					spanFound = true;
					TestUtility.AnalyzeNode(node, "span", "two", "<span>two</span>", null, false, true, 1, 0, 1);
					TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "font-weight", "bold");
				}
				
				node = node.Next;
			}
			
			if (!pFound)
			{
				throw new Exception("p tag not found");
			}
			
			if (!spanFound)
			{
				throw new Exception("span tag not found");
			}
		}
		
		[Test]
		public void PNextDivSpan()
		{
			string html = @"<style>
                                p + span
                                {
                                	font-weight:bold;
                                }
                            </style>
                            <p>one</p>
                            <div>test</div>
							<span>two</span>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			bool pFound = false;
			bool divFound = false;
			bool spanFound = false;
			
			while (node != null)
			{
				if (node.Tag == "p")
				{
					pFound = true;
					TestUtility.AnalyzeNode(node, "p", "one", "<p>one</p>", null, false, true, 1, 0, 0);
				}
				
				if (node.Tag == "div")
				{
					divFound = true;
					TestUtility.AnalyzeNode(node, "div", "test", "<div>test</div>", null, false, true, 1, 0, 0);
				}
				
				if (node.Tag == "span")
				{
					spanFound = true;
					TestUtility.AnalyzeNode(node, "span", "two", "<span>two</span>", null, false, true, 1, 0, 0);
				}
				
				node = node.Next;
			}
			
			if (!pFound)
			{
				throw new Exception("p tag not found");
			}
			
			if (!divFound)
			{
				throw new Exception("div tag not found");
			}
			
			if (!spanFound)
			{
				throw new Exception("span tag not found");
			}
		}
		
		[Test]
		public void DivAllNextP()
		{
			string html = @"<style>
                                div~p
                                {
                                	font-weight:bold;
                                }
                            </style>
                            <div><p>1</p></div>
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
					TestUtility.AnalyzeNode(node, "div", "<p>1</p>", "<div><p>1</p></div>", null, false, true, 1, 0, 0);
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
	}
}
