namespace MariGold.HtmlParser.Tests
{
	using System;
	using NUnit.Framework;
	using MariGold.HtmlParser;
	using System.Linq;
	using System.IO;
	
	[TestFixture]
	public class BasicStyle
	{
		[Test]
		public void OneInlineStyle()
		{
			string html = "<div style='color:#fff'>test</div>";

			HtmlParser parser = new HtmlTextParser(html);

			if (parser.Traverse())
			{
				parser.ParseCSS();

				Assert.IsNotNull(parser.Current);
				Assert.IsNotNull(parser.Current.Styles);
				Assert.AreEqual(1, parser.Current.Styles.Count);
				TestUtility.CheckStyle(parser.Current.Styles.ElementAt(0), "color", "#fff");
			}
		}

		[Test]
		public void TwoInlineStyle()
		{
			string html = "<div style='color:#fff;font-size:24px'>test</div>";

			HtmlParser parser = new HtmlTextParser(html);

			if (parser.Traverse())
			{
				parser.ParseCSS();

				Assert.IsNotNull(parser.Current);
				Assert.IsNotNull(parser.Current.Styles);
				Assert.AreEqual(2, parser.Current.Styles.Count);
				TestUtility.CheckStyle(parser.Current.Styles.ElementAt(0), "color", "#fff");
				TestUtility.CheckStyle(parser.Current.Styles.ElementAt(1), "font-size", "24px");
			}
		}

		[Test]
		public void BasicIdentityTagStyle()
		{
			string html = "<html><style>#dv{font:10px verdana,arial;color:#000}</style><div id='dv'>test</div></html>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Traverse());

			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			Assert.IsNotNull(parser.Current.Children);
			Assert.AreEqual(2, parser.Current.Children.Count());
			TestUtility.AnalyzeNode(parser.Current.Children.ElementAt(1), "div", "test", "<div id='dv'>test</div>",
				parser.Current, false, true, 1, 1);

			Assert.IsNotNull(parser.Current.Children.ElementAt(1).Styles);
			Assert.AreEqual(3, parser.Current.Children.ElementAt(1).Styles.Count());
            TestUtility.CheckStyle(parser.Current.Children.ElementAt(1).Styles.ElementAt(0), "color", "#000");
            TestUtility.CheckStyle(parser.Current.Children.ElementAt(1).Styles.ElementAt(1), "font-size", "10px");
            TestUtility.CheckStyle(parser.Current.Children.ElementAt(1).Styles.ElementAt(2), "font-family", "verdana,arial");
		}

		[Test]
		public void BasicClassTagStyle()
		{
			string html = "<html><style>.cls{font:12px verdana,arial;color:#000}</style><div class='cls'>test</div></html>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Traverse());

			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			Assert.IsNotNull(parser.Current.Children);
			Assert.AreEqual(2, parser.Current.Children.Count());
			TestUtility.AnalyzeNode(parser.Current.Children.ElementAt(1), "div", "test", "<div class='cls'>test</div>",
				parser.Current, false, true, 1, 1);

			Assert.IsNotNull(parser.Current.Children.ElementAt(1).Styles);
			Assert.AreEqual(3, parser.Current.Children.ElementAt(1).Styles.Count);
            TestUtility.CheckStyle(parser.Current.Children.ElementAt(1).Styles.ElementAt(0), "color", "#000");
			TestUtility.CheckStyle(parser.Current.Children.ElementAt(1).Styles.ElementAt(1), "font-size", "12px");
            TestUtility.CheckStyle(parser.Current.Children.ElementAt(1).Styles.ElementAt(2), "font-family", "verdana,arial");
		}

		[Test]
		public void DivIdGreatherThanP()
		{
			string html = @"<style>
                                #dv > p
                                {
                                    color:#fff;
                                }
                            </style>
                            <div id='dv'>
                                <p>test</p>
                            </div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());

			parser.ParseCSS();

			IHtmlNode node = parser.Current;

			while (node.Tag != "div")
				node = node.Next;

			foreach (IHtmlNode child in node.Children)
			{
				if (child.Tag == "p")
				{
					Assert.AreEqual(1, child.Styles.Count);
					TestUtility.CheckKeyValuePair(child.Styles.ElementAt(0), "color", "#fff");
					break;
				}
			}
		}

		[Test]
		public void DivIdSpaceP()
		{
			string html = @"<style>
                                #dv p
                                {
                                    color:#fff;
                                }
                            </style>
                            <div id='dv'>
                                <a href='#'>link</a>
                                <p>test</p>
                                <art>
                                    <p>one</p>
                                </art>
                            </div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());

			parser.ParseCSS();

			IHtmlNode node = parser.Current;

			while (node.Tag != "div")
				node = node.Next;

			bool aTagFound = false;
			bool pTagFound = false;
			bool artTagFound = false;
			bool innerPTagFound = false;

			foreach (HtmlNode child in node.Children)
			{
				if (child.Tag == "a")
				{
					aTagFound = true;
					Assert.AreEqual(0, child.Styles.Count);
				}
				else
				if (child.Tag == "p")
				{
					pTagFound = true;
					Assert.AreEqual(1, child.Styles.Count);
					TestUtility.CheckKeyValuePair(child.Styles.ElementAt(0), "color", "#fff");
				}
				else
				if (child.Tag == "art")
				{
					artTagFound = true;

					foreach (HtmlNode child1 in child.Children)
					{
						if (child1.Tag == "p")
						{
							innerPTagFound = true;
							Assert.AreEqual(1, child1.Styles.Count);
							TestUtility.CheckKeyValuePair(child1.Styles.ElementAt(0),
								"color", "#fff");
						}
					}
				}
			}

			Assert.IsTrue(aTagFound);
			Assert.IsTrue(pTagFound);
			Assert.IsTrue(artTagFound);
			Assert.IsTrue(innerPTagFound);
		}

		[Test]
		public void DivImmediateP()
		{
			string html = @"<style>
                                #dv + p
                                {
                                    color:#fff;
                                }
                            </style>
                            <div id='dv'></div>
                            <p>one</p>
                            <p>two</p>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);

			IHtmlNode temp = parser.Current;
			bool divFound = false;

			while (temp != null)
			{
				if (temp.Tag == "div")
				{
					divFound = true;

					TestUtility.AreEqual(temp, "div", "", "<div id='dv'></div>");

					while (temp != null && (temp.Tag == "#text" || temp.Tag == "div"))
						temp = temp.Next;

					Assert.IsNotNull(temp);
					TestUtility.AreEqual(temp, "p", "one", "<p>one</p>");
					Assert.AreEqual(1, temp.Styles.Count);
					TestUtility.CheckKeyValuePair(temp.Styles.ElementAt(0),
						"color", "#fff");

					temp = temp.Next;

					while (temp != null && temp.Tag == "#text")
						temp = temp.Next;

					Assert.IsNotNull(temp);
					TestUtility.AreEqual(temp, "p", "two", "<p>two</p>");
					Assert.AreEqual(0, temp.Styles.Count);

					break;
				}

				temp = temp.Next;
			}

			Assert.IsTrue(divFound);
		}

		[Test]
		public void DivAllNextP()
		{
			string html = @"<style>
                                #dv ~ p
                                {
                                    color:#fff;
                                }
                            </style>
                            <div id='dv'></div>
                            <p>one</p>
                            <a href='#'>tt</a>
                            <p>two</p>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);

			IHtmlNode temp = parser.Current;
			bool divFound = false;

			while (temp != null)
			{
				if (temp.Tag == "div")
				{
					divFound = true;

					TestUtility.AreEqual(temp, "div", "", "<div id='dv'></div>");

					while (temp != null && (temp.Tag == "#text" || temp.Tag == "div"))
						temp = temp.Next;

					Assert.IsNotNull(temp);
					TestUtility.AreEqual(temp, "p", "one", "<p>one</p>");
					Assert.AreEqual(1, temp.Styles.Count);
					TestUtility.CheckKeyValuePair(temp.Styles.ElementAt(0),
						"color", "#fff");

					temp = temp.Next;

					while (temp != null && temp.Tag == "#text")
						temp = temp.Next;


					Assert.IsNotNull(temp);
					TestUtility.AreEqual(temp, "a", "tt", "<a href='#'>tt</a>");
					Assert.AreEqual(0, temp.Styles.Count);

					temp = temp.Next;

					while (temp != null && temp.Tag == "#text")
						temp = temp.Next;

					Assert.IsNotNull(temp);
					TestUtility.AreEqual(temp, "p", "two", "<p>two</p>");
					Assert.AreEqual(1, temp.Styles.Count);
					TestUtility.CheckKeyValuePair(temp.Styles.ElementAt(0),
						"color", "#fff");

					break;
				}

				temp = temp.Next;
			}

			Assert.IsTrue(divFound);
		}
		
		[Test]
		public void AttributeNameOnly()
		{
			string html = @"<style>
                                [id]
                                {
                                	color:#fff;
                                }
                            </style>
                            <div id>one</div>
                            <div>two</div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode temp = parser.Current;
			
			while (temp != null && temp.Tag == "#text")
				temp = temp.Next;
			
			Assert.AreEqual("style", temp.Tag);
			
			temp = temp.Next;
			
			while (temp != null && temp.Tag == "#text")
				temp = temp.Next;
			
			Assert.IsNotNull(temp);
			TestUtility.AreEqual(temp, "div", "one", "<div id>one</div>");
			Assert.AreEqual(1, temp.Styles.Count);
			TestUtility.CheckStyle(temp.Styles.ElementAt(0), "color", "#fff");
			
			temp = temp.Next;
			
			while (temp != null && temp.Tag == "#text")
				temp = temp.Next;
			
			Assert.IsNotNull(temp);
			TestUtility.AreEqual(temp, "div", "two", "<div>two</div>");
			Assert.AreEqual(0, temp.Styles.Count);
		}
		
		[Test]
		public void AttributeOnly()
		{
			string html = @"<style>
                                [id]
                                {
                                	color:#fff;
                                }
                            </style>
                            <div id='div'>one</div>
                            <div>two</div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode temp = parser.Current;
			
			while (temp != null && temp.Tag == "#text")
				temp = temp.Next;
			
			Assert.AreEqual("style", temp.Tag);
			
			temp = temp.Next;
			
			while (temp != null && temp.Tag == "#text")
				temp = temp.Next;
			
			Assert.IsNotNull(temp);
			TestUtility.AreEqual(temp, "div", "one", "<div id='div'>one</div>");
			Assert.AreEqual(1, temp.Styles.Count);
			TestUtility.CheckStyle(temp.Styles.ElementAt(0), "color", "#fff");
			
			temp = temp.Next;
			
			while (temp != null && temp.Tag == "#text")
				temp = temp.Next;
			
			Assert.IsNotNull(temp);
			TestUtility.AreEqual(temp, "div", "two", "<div>two</div>");
			Assert.AreEqual(0, temp.Styles.Count);
		}
		
		[Test]
		public void AttributeWithValue()
		{
			string html = @"<style>
                                [id='dv']
                                {
                                	color:#fff;
                                }
                            </style>
                            <div id='dv'>one</div>
                            <div id>two</div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode temp = parser.Current;
			
			while (temp != null && temp.Tag == "#text")
				temp = temp.Next;
			
			Assert.AreEqual("style", temp.Tag);
			
			temp = temp.Next;
			
			while (temp != null && temp.Tag == "#text")
				temp = temp.Next;
			
			Assert.IsNotNull(temp);
			TestUtility.AreEqual(temp, "div", "one", "<div id='dv'>one</div>");
			Assert.AreEqual(1, temp.Styles.Count);
			TestUtility.CheckStyle(temp.Styles.ElementAt(0), "color", "#fff");
			
			temp = temp.Next;
			
			while (temp != null && temp.Tag == "#text")
				temp = temp.Next;
			
			Assert.IsNotNull(temp);
			TestUtility.AreEqual(temp, "div", "two", "<div id>two</div>");
			Assert.AreEqual(0, temp.Styles.Count);
		}
		
		[Test]
		public void BasicFirstChild()
		{
			string html = @"<style>
                                :first-child
                                {
                                	color:#fff;
                                }
                            </style>
                            <div id='dv'><p id='p1'>one</p><p id='p2'>two</p></div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode temp = parser.Current;
			
			while (temp.Tag != "div")
				temp = temp.Next;
			
			TestUtility.AreEqual(temp, "div", "<p id='p1'>one</p><p id='p2'>two</p>",
				"<div id='dv'><p id='p1'>one</p><p id='p2'>two</p></div>");
			
			Assert.AreEqual(2, temp.Children.Count());
			Assert.AreEqual(1, temp.Attributes.Count);
			TestUtility.CheckKeyValuePair(temp.Attributes.ElementAt(0), "id", "dv");
			
			temp = temp.Children.ElementAt(0);
			
			Assert.IsNotNull(temp);
			TestUtility.AreEqual(temp, "p", "one", "<p id='p1'>one</p>");
			Assert.AreEqual(1, temp.Attributes.Count);
			TestUtility.CheckKeyValuePair(temp.Attributes.ElementAt(0), "id", "p1");
			
			Assert.AreEqual(1, temp.Styles.Count);
			TestUtility.CheckKeyValuePair(temp.Styles.ElementAt(0), "color", "#fff");
			
			temp = temp.Next;
			
			Assert.IsNotNull(temp);
			TestUtility.AreEqual(temp, "p", "two", "<p id='p2'>two</p>");
			Assert.AreEqual(1, temp.Attributes.Count);
			TestUtility.CheckKeyValuePair(temp.Attributes.ElementAt(0), "id", "p2");
			
			Assert.AreEqual(0, temp.Styles.Count);
			
		}
		
		[Test]
		public void BasicLastChild()
		{
			string html = @"<style>
                                :last-child
                                {
                                	color:#fff;
                                }
                            </style>
                            <div id='dv'><p id='p1'>one</p><p id='p2'>two</p></div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode temp = parser.Current;
			
			while (temp.Tag != "div")
				temp = temp.Next;
			
			TestUtility.AreEqual(temp, "div", "<p id='p1'>one</p><p id='p2'>two</p>",
				"<div id='dv'><p id='p1'>one</p><p id='p2'>two</p></div>");
			
			Assert.AreEqual(2, temp.Children.Count());
			Assert.AreEqual(1, temp.Attributes.Count);
			TestUtility.CheckKeyValuePair(temp.Attributes.ElementAt(0), "id", "dv");
			
			temp = temp.Children.ElementAt(0);
			
			Assert.IsNotNull(temp);
			TestUtility.AreEqual(temp, "p", "one", "<p id='p1'>one</p>");
			Assert.AreEqual(1, temp.Attributes.Count);
			TestUtility.CheckKeyValuePair(temp.Attributes.ElementAt(0), "id", "p1");
			
			Assert.AreEqual(0, temp.Styles.Count);
			
			temp = temp.Next;
			
			Assert.IsNotNull(temp);
			TestUtility.AreEqual(temp, "p", "two", "<p id='p2'>two</p>");
			Assert.AreEqual(1, temp.Attributes.Count);
			TestUtility.CheckKeyValuePair(temp.Attributes.ElementAt(0), "id", "p2");
			
			Assert.AreEqual(1, temp.Styles.Count);
			TestUtility.CheckKeyValuePair(temp.Styles.ElementAt(0), "color", "#fff");
			
		}
		
		[Test]
		public void BasicLastOfType()
		{
			string html = @"<style>
                                :last-of-type
                                {
                                	color:#fff;
                                }
                            </style>
                            <div id='dv'><p id='p1'>one</p><p id='p2'>two</p></div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode temp = parser.Current;
			
			while (temp.Tag != "div")
				temp = temp.Next;
			
			TestUtility.AreEqual(temp, "div", "<p id='p1'>one</p><p id='p2'>two</p>",
				"<div id='dv'><p id='p1'>one</p><p id='p2'>two</p></div>");
			
			Assert.AreEqual(2, temp.Children.Count());
			Assert.AreEqual(1, temp.Attributes.Count);
			TestUtility.CheckKeyValuePair(temp.Attributes.ElementAt(0), "id", "dv");
			
			temp = temp.Children.ElementAt(0);
			
			Assert.IsNotNull(temp);
			TestUtility.AreEqual(temp, "p", "one", "<p id='p1'>one</p>");
			Assert.AreEqual(1, temp.Attributes.Count);
			TestUtility.CheckKeyValuePair(temp.Attributes.ElementAt(0), "id", "p1");
			
			Assert.AreEqual(0, temp.Styles.Count);
			
			temp = temp.Next;
			
			Assert.IsNotNull(temp);
			TestUtility.AreEqual(temp, "p", "two", "<p id='p2'>two</p>");
			Assert.AreEqual(1, temp.Attributes.Count());
			TestUtility.CheckKeyValuePair(temp.Attributes.ElementAt(0), "id", "p2");
			
			Assert.AreEqual(1, temp.Styles.Count);
			TestUtility.CheckKeyValuePair(temp.Styles.ElementAt(0), "color", "#fff");
			
		}
		
		[Test]
		public void PFirstChild()
		{
			string html = @"<style>
                                span:first-child
                                {
                                	color:#fff;
                                }
                            </style>
                            <div><span>dspan1</span><span>dspan2</span></div>
							<p><span>pspan1</span><span>pspan2</span></p>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode div = parser.Current;
			
			while (div.Tag != "div")
				div = div.Next;
			
			TestUtility.AnalyzeNode(div, "div", "<span>dspan1</span><span>dspan2</span>", "<div><span>dspan1</span><span>dspan2</span></div>",
				null, false, true, 2, 0);
			Assert.AreEqual(0, div.Styles.Count);
			
			TestUtility.AnalyzeNode(div.Children.ElementAt(0), "span", "dspan1", "<span>dspan1</span>", div, false, true, 1, 0, 1);
			
			TestUtility.AnalyzeNode(div.Children.ElementAt(1), "span", "dspan2", "<span>dspan2</span>", div, false, true, 1, 0);
			Assert.AreEqual(0, div.Children.ElementAt(1).Styles.Count);
			
			IHtmlNode p = div.Next;
			
			while (p.Tag == "#text")
				p = p.Next;
			
			TestUtility.AnalyzeNode(p, "p", "<span>pspan1</span><span>pspan2</span>", "<p><span>pspan1</span><span>pspan2</span></p>",
				null, false, true, 2, 0);
			Assert.AreEqual(0, p.Styles.Count);
			
			TestUtility.AnalyzeNode(p.Children.ElementAt(0), "span", "pspan1", "<span>pspan1</span>", p, false, true, 1, 0, 1);
			TestUtility.CheckStyle(p.Children.ElementAt(0).Styles.ElementAt(0), "color", "#fff");
			
			TestUtility.AnalyzeNode(p.Children.ElementAt(1), "span", "pspan2", "<span>pspan2</span>", p, false, true, 1, 0);
			Assert.AreEqual(0, p.Children.ElementAt(1).Styles.Count);
		}
		
		[Test]
		public void PLastChild()
		{
			string html = @"<style>
                                p:last-child
                                {
                                	color:#fff;
                                }
                            </style>
                            <div>
                            <div><span>dspan1</span><span>dspan2</span></div>
							<p><span>pspan1</span><span>pspan2</span></p>
                            </div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode div = parser.Current;
			
			while (div.Tag != "div")
				div = div.Next;

            IHtmlNode parent = div;

            div = div.Children.ElementAt(0);

            while (div.Tag != "div")
                div = div.Next;
			
			TestUtility.AnalyzeNode(div, "div", "<span>dspan1</span><span>dspan2</span>", "<div><span>dspan1</span><span>dspan2</span></div>",
                parent, false, true, 2, 0);
			Assert.AreEqual(0, div.Styles.Count);
			
			TestUtility.AnalyzeNode(div.Children.ElementAt(0), "span", "dspan1", "<span>dspan1</span>", div, false, true, 1, 0);
			Assert.AreEqual(0, div.Children.ElementAt(0).Styles.Count);
			
			TestUtility.AnalyzeNode(div.Children.ElementAt(1), "span", "dspan2", "<span>dspan2</span>", div, false, true, 1, 0);
			Assert.AreEqual(0, div.Children.ElementAt(1).Styles.Count);
			
			IHtmlNode p = div.Next;
			
			while (p.Tag == "#text")
				p = p.Next;
			
			TestUtility.AnalyzeNode(p, "p", "<span>pspan1</span><span>pspan2</span>", "<p><span>pspan1</span><span>pspan2</span></p>",
                parent, false, true, 2, 0);
			Assert.AreEqual(1, p.Styles.Count);
            TestUtility.CheckStyle(p.Styles.ElementAt(0), "color", "#fff");

			TestUtility.AnalyzeNode(p.Children.ElementAt(0), "span", "pspan1", "<span>pspan1</span>", p, false, true, 1, 0);
			Assert.AreEqual(1, p.Children.ElementAt(0).Styles.Count);
            TestUtility.CheckStyle(p.Children.ElementAt(0).Styles.ElementAt(0), "color", "#fff");

			TestUtility.AnalyzeNode(p.Children.ElementAt(1), "span", "pspan2", "<span>pspan2</span>", p, false, true, 1, 0);
			Assert.AreEqual(1, p.Children.ElementAt(1).Styles.Count);
            TestUtility.CheckStyle(p.Children.ElementAt(1).Styles.ElementAt(0), "color", "#fff");
		}
		
		[Test]
		public void NthChild()
		{
			string html = @"<style>
                                :nth-child(1)
                                {
                                	color:#fff;
                                }
                            </style>
                            <div><span>dspan1</span><span>dspan2</span></div>
							<p><span>pspan1</span><span>pspan2</span></p>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode div = parser.Current;
			
			while (div.Tag != "div")
				div = div.Next;
			
			TestUtility.AnalyzeNode(div, "div", "<span>dspan1</span><span>dspan2</span>", "<div><span>dspan1</span><span>dspan2</span></div>",
				null, false, true, 2, 0);
			Assert.AreEqual(0, div.Styles.Count);
			
			TestUtility.AnalyzeNode(div.Children.ElementAt(0), "span", "dspan1", "<span>dspan1</span>", div, false, true, 1, 0);
			Assert.AreEqual(1, div.Children.ElementAt(0).Styles.Count);
			TestUtility.CheckStyle(div.Children.ElementAt(0).Styles.ElementAt(0), "color", "#fff");
			
			TestUtility.AnalyzeNode(div.Children.ElementAt(1), "span", "dspan2", "<span>dspan2</span>", div, false, true, 1, 0);
			Assert.AreEqual(0, div.Children.ElementAt(1).Styles.Count);
			
			IHtmlNode p = div.Next;
			
			while (p.Tag == "#text")
				p = p.Next;
			
			TestUtility.AnalyzeNode(p, "p", "<span>pspan1</span><span>pspan2</span>", "<p><span>pspan1</span><span>pspan2</span></p>",
				null, false, true, 2, 0);
			Assert.AreEqual(0, p.Styles.Count);
			
			TestUtility.AnalyzeNode(p.Children.ElementAt(0), "span", "pspan1", "<span>pspan1</span>", p, false, true, 1, 0);
			Assert.AreEqual(1, p.Children.ElementAt(0).Styles.Count);
			TestUtility.CheckStyle(p.Children.ElementAt(0).Styles.ElementAt(0), "color", "#fff");
			
			TestUtility.AnalyzeNode(p.Children.ElementAt(1), "span", "pspan2", "<span>pspan2</span>", p, false, true, 1, 0);
			Assert.AreEqual(0, p.Children.ElementAt(1).Styles.Count);
			
		}
		
		[Test]
		public void BodyNthChild()
		{
			string html = @"<style>
                                :nth-child(2)
                                {
                                	color:#fff;
                                }
                            </style>
                            <body><p>p tag</p><div>div tag</div></body>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode body = parser.Current;
			
			while (body.Tag != "body")
			{
				body = body.Next;
			}
			
			TestUtility.AnalyzeNode(body, "body", "<p>p tag</p><div>div tag</div>", "<body><p>p tag</p><div>div tag</div></body>",
				null, false, true, 2, 0);
			
			foreach (HtmlNode child in body.Children)
			{
				if (child.Tag == "p")
				{
					TestUtility.AnalyzeNode(child, "p", "p tag", "<p>p tag</p>", body, false, true, 1, 0);
					Assert.AreEqual(0, child.Styles.Count);
				}
				else
				if (child.Tag == "div")
				{
					TestUtility.AnalyzeNode(child, "div", "div tag", "<div>div tag</div>", body, false, true, 1, 0);
					Assert.AreEqual(1, child.Styles.Count);
					TestUtility.CheckStyle(child.Styles.ElementAt(0), "color", "#fff");
				}
				else
				{
					throw new Exception("Invalid child tag found in body node");
				}
			}
		}
		
		[Test]
		public void PNotValidNthChild()
		{
			string html = @"<style>
                                p:nth-child(2)
                                {
                                	color:#fff;
                                }
                            </style>
                            <body><p>p tag</p><div>div tag</div></body>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode body = parser.Current;
			
			while (body.Tag != "body")
			{
				body = body.Next;
			}
			
			TestUtility.AnalyzeNode(body, "body", "<p>p tag</p><div>div tag</div>", "<body><p>p tag</p><div>div tag</div></body>",
				null, false, true, 2, 0);
			
			foreach (HtmlNode child in body.Children)
			{
				if (child.Tag == "p")
				{
					TestUtility.AnalyzeNode(child, "p", "p tag", "<p>p tag</p>", body, false, true, 1, 0);
					Assert.AreEqual(0, child.Styles.Count);
				}
				else
				if (child.Tag == "div")
				{
					TestUtility.AnalyzeNode(child, "div", "div tag", "<div>div tag</div>", body, false, true, 1, 0);
					Assert.AreEqual(0, child.Styles.Count);
				}
				else
				{
					throw new Exception("Invalid child tag found in body node");
				}
			}
		}
		
		[Test]
		public void PValidNthChild()
		{
			string html = @"<style>
                                p:nth-child(1)
                                {
                                	color:#fff;
                                }
                            </style>
                            <body><p>p tag</p><div>div tag</div></body>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode body = parser.Current;
			
			while (body.Tag != "body")
			{
				body = body.Next;
			}
			
			TestUtility.AnalyzeNode(body, "body", "<p>p tag</p><div>div tag</div>", "<body><p>p tag</p><div>div tag</div></body>",
				null, false, true, 2, 0);
			
			foreach (IHtmlNode child in body.Children)
			{
				if (child.Tag == "p")
				{
					TestUtility.AnalyzeNode(child, "p", "p tag", "<p>p tag</p>", body, false, true, 1, 0);
					Assert.AreEqual(1, child.Styles.Count);
					TestUtility.CheckStyle(child.Styles.ElementAt(0), "color", "#fff");
				}
				else
				if (child.Tag == "div")
				{
					TestUtility.AnalyzeNode(child, "div", "div tag", "<div>div tag</div>", body, false, true, 1, 0);
					Assert.AreEqual(0, child.Styles.Count);
				}
				else
				{
					throw new Exception("Invalid child tag found in body node");
				}
			}
		}
		
		[Test]
		public void NotP()
		{
			string html = @"<style>
                                :not(p)
                                {
                                	color:#fff;
                                }
                            </style>
                            <p>p tag</p>
							<div>div tag</div>
							<span>span tag</span>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode node = parser.Current;
			
			bool pTagFound = false;
			bool divTagFound = false;
			bool spanTagFound = false;
			
			while (node != null)
			{
				if (node.Tag == "p")
				{
					TestUtility.AnalyzeNode(node, "p", "p tag", "<p>p tag</p>", null, false, true, 1, 0);
					Assert.AreEqual(0, node.Styles.Count);
                    TestUtility.AnalyzeNode(node.Children.ElementAt(0), "#text", "p tag", "p tag", node, false, false, 0, 0, 0);
					pTagFound = true;
				}
				else
				if (node.Tag == "div")
				{
					TestUtility.AnalyzeNode(node, "div", "div tag", "<div>div tag</div>", null, false, true, 1, 0);
					Assert.AreEqual(1, node.Styles.Count);
					TestUtility.CheckStyle(node.Styles.ElementAt(0), "color", "#fff");
					divTagFound = true;
				}
				else
				if (node.Tag == "span")
				{
					TestUtility.AnalyzeNode(node, "span", "span tag", "<span>span tag</span>", null, false, true, 1, 0);
					Assert.AreEqual(1, node.Styles.Count);
					TestUtility.CheckStyle(node.Styles.ElementAt(0), "color", "#fff");
					spanTagFound = true;
				}
				
				node = node.Next;
			}
			
			if (!pTagFound)
			{
				throw new Exception("p tag not found");
			}
			
			if (!divTagFound)
			{
				throw new Exception("div tag not found");
			}
			
			if (!spanTagFound)
			{
				throw new Exception("span not found");
			}
		}
		
		[Test]
		public void NotDiv()
		{
			string html = @"<style>
                                :not(div)
                                {
                                	color:#fff;
                                }
                            </style>
                            <p>p tag</p>
							<div>div tag</div>
							<span>span tag</span>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode node = parser.Current;
			
			bool pTagFound = false;
			bool divTagFound = false;
			bool spanTagFound = false;
			
			while (node != null)
			{
				if (node.Tag == "p")
				{
					TestUtility.AnalyzeNode(node, "p", "p tag", "<p>p tag</p>", null, false, true, 1, 0);
					Assert.AreEqual(1, node.Styles.Count);
					TestUtility.CheckStyle(node.Styles.ElementAt(0), "color", "#fff");
					pTagFound = true;
				}
				else
				if (node.Tag == "div")
				{
					TestUtility.AnalyzeNode(node, "div", "div tag", "<div>div tag</div>", null, false, true, 1, 0);
					Assert.AreEqual(0, node.Styles.Count);
					divTagFound = true;
				}
				else
				if (node.Tag == "span")
				{
					TestUtility.AnalyzeNode(node, "span", "span tag", "<span>span tag</span>", null, false, true, 1, 0);
					Assert.AreEqual(1, node.Styles.Count);
					TestUtility.CheckStyle(node.Styles.ElementAt(0), "color", "#fff");
					spanTagFound = true;
				}
				
				node = node.Next;
			}
			
			if (!pTagFound)
			{
				throw new Exception("p tag not found");
			}
			
			if (!divTagFound)
			{
				throw new Exception("div tag not found");
			}
			
			if (!spanTagFound)
			{
				throw new Exception("span not found");
			}
		}
		
		[Test]
		public void StandAloneOnlyChild()
		{
			string html = @"<style>
                                :only-child
                                {
                                	color:#fff;
                                }
                            </style>
                            <p>p tag</p>
							<div>div tag</div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode node = parser.Current;
			
			bool pTagFound = false;
			bool divTagFound = false;
			
			while (node != null)
			{
				if (node.Tag == "p")
				{
					TestUtility.AnalyzeNode(node, "p", "p tag", "<p>p tag</p>", null, false, true, 1, 0);
					Assert.AreEqual(0, node.Styles.Count);
                    TestUtility.AnalyzeNode(node.Children.ElementAt(0), "#text", "p tag", "p tag", node, false, false, 0, 0, 0);
					pTagFound = true;
				}
				else
				if (node.Tag == "div")
				{
					TestUtility.AnalyzeNode(node, "div", "div tag", "<div>div tag</div>", null, false, true, 1, 0);
					Assert.AreEqual(0, node.Styles.Count);
					divTagFound = true;
				}
				
				node = node.Next;
			}
			
			if (!pTagFound)
			{
				throw new Exception("p tag not found");
			}
			
			if (!divTagFound)
			{
				throw new Exception("div tag not found");
			}
		}
		
		[Test]
		public void POnlyChild()
		{
			string html = @"<style>
                                p:only-child
                                {
                                	color:#fff;
                                }
                            </style>
                            <p>p tag</p>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode node = parser.Current;
			bool pTagFound = false;
			
			while (node != null)
			{
				if (node.Tag == "p")
				{
					pTagFound = true;
					
					TestUtility.AnalyzeNode(node, "p", "p tag", "<p>p tag</p>", null, false, true, 1, 0);
					Assert.AreEqual(0, node.Styles.Count);
				}
				
				node = node.Next;
			}
			
			if (!pTagFound)
			{
				throw new Exception("p tag not found");
			}
		}
		
		[Test]
		public void PSpanOnlyChild()
		{
			string html = @"<style>
                                span:only-child
                                {
                                	color:#fff;
                                }
                            </style>
                            <p><span>one</span></p>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode node = parser.Current;
			bool pTagFound = false;
			
			while (node != null)
			{
				if (node.Tag == "p")
				{
					pTagFound = true;
					
					TestUtility.AnalyzeNode(node, "p", "<span>one</span>", "<p><span>one</span></p>", null, false, true, 1, 0);
					Assert.AreEqual(0, node.Styles.Count);
					
					TestUtility.AnalyzeNode(node.Children.ElementAt(0), "span", "one", "<span>one</span>", node, false, true, 1, 0, 1);
					
					TestUtility.CheckStyle(node.Children.ElementAt(0).Styles.ElementAt(0), "color", "#fff");
				}
				
				node = node.Next;
			}
			
			if (!pTagFound)
			{
				throw new Exception("p tag not found");
			}
		}
		
		[Test]
		public void PSpanTextOnlyChild()
		{
			string html = @"<style>
                                span:only-child
                                {
                                	color:#fff;
                                }
                            </style>
                            <p><span>one</span>this is a test</p>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode node = parser.Current;
			bool pTagFound = false;
			
			while (node != null)
			{
				if (node.Tag == "p")
				{
					pTagFound = true;
					
					TestUtility.AnalyzeNode(node, "p", "<span>one</span>this is a test",
						"<p><span>one</span>this is a test</p>", null, false, true, 2, 0);
					Assert.AreEqual(0, node.Styles.Count);
					
					TestUtility.AnalyzeNode(node.Children.ElementAt(0), "span", "one", "<span>one</span>", node, false, true, 1, 0, 1);
					TestUtility.CheckStyle(node.Children.ElementAt(0).Styles.ElementAt(0), "color", "#fff");
					
					TestUtility.AnalyzeNode(node.Children.ElementAt(1), "#text", "this is a test", "this is a test", node, false, false, 0, 0, 0);
				}
				
				node = node.Next;
			}
			
			if (!pTagFound)
			{
				throw new Exception("p tag not found");
			}
		}

		[Test]
		public void TwoGlobalStyle()
		{
			string html = @"<style>
                                *
                                {
                                	color:#fff;
                                	margin: 0 10px 0 10px;
                                }
                            </style>
                            test text
							<div>one</div>
							<b></b>
							<hr />";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode node = parser.Current;
			
			bool textFound = false;
			bool divFound = false;
			bool bFound = false;
			bool hrFound = false;
			
			while (node != null)
			{
				
				if (node.Tag == "#text")
				{
					textFound = true;
					
					Assert.AreEqual(0, node.Styles.Count);
					/*TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "#fff");
					TestUtility.CheckKeyValuePair(node.Styles.ElementAt(1), "margin", "0 10px 0 10px");*/
				}
				
				if (node.Tag == "div")
				{
					divFound = true;
					
					TestUtility.AnalyzeNode(node, "div", "one", "<div>one</div>", null, false, true, 1, 0, 2);
					TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "#fff");
					TestUtility.CheckKeyValuePair(node.Styles.ElementAt(1), "margin", "0 10px 0 10px");
				}
				
				if (node.Tag == "b")
				{
					bFound = true;
					
					TestUtility.AnalyzeNode(node, "b", "", "<b></b>", null, false, false, 0, 0, 2);
					TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "#fff");
					TestUtility.CheckKeyValuePair(node.Styles.ElementAt(1), "margin", "0 10px 0 10px");
				}
				
				if (node.Tag == "hr")
				{
					hrFound = true;
					
					TestUtility.AnalyzeNode(node, "hr", "<hr />", "<hr />", null, true, false, 0, 0, 2);
					TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "#fff");
					TestUtility.CheckKeyValuePair(node.Styles.ElementAt(1), "margin", "0 10px 0 10px");
				}
				
				node = node.Next;
			}
			
			if (!textFound)
			{
				throw new Exception("Text node not found");
			}
			
			if (!divFound)
			{
				throw new Exception("div not found");
			}
			
			if (!bFound)
			{
				throw new Exception("b not found");
			}
			
			if (!hrFound)
			{
				throw new Exception("hr not found");
			}
		}
		
		[Test]
		public void NthLastChild()
		{
			string html = @"<style>
                                :nth-last-child(2)
                                {
                                	color:#fff;
                                }
                            </style>
                            <div><p>one</p><span>two</span></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "<p>one</p><span>two</span>", "<div><p>one</p><span>two</span></div>", null, false, true, 2, 0, 0);
			
			TestUtility.AnalyzeNode(node.Children.ElementAt(0), "p", "one", "<p>one</p>", node, false, true, 1, 0, 1);
			TestUtility.CheckKeyValuePair(node.Children.ElementAt(0).Styles.ElementAt(0), "color", "#fff");
			
			TestUtility.AnalyzeNode(node.Children.ElementAt(1), "span", "two", "<span>two</span>", node, false, true, 1, 0, 0);
		}
		
		[Test]
		public void PNthLastChild()
		{
			string html = @"<style>
                                p:nth-last-child(2)
                                {
                                	color:#fff;
                                }
                            </style>
                            <div><p>one</p><span>two</span></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "<p>one</p><span>two</span>", "<div><p>one</p><span>two</span></div>", null, false, true, 2, 0, 0);
			
			TestUtility.AnalyzeNode(node.Children.ElementAt(0), "p", "one", "<p>one</p>", node, false, true, 1, 0, 1);
			TestUtility.CheckKeyValuePair(node.Children.ElementAt(0).Styles.ElementAt(0), "color", "#fff");
			
			TestUtility.AnalyzeNode(node.Children.ElementAt(1), "span", "two", "<span>two</span>", node, false, true, 1, 0, 0);
		}
		
		[Test]
		public void SpanNthLastChild()
		{
			string html = @"<style>
                                span:nth-last-child(2)
                                {
                                	color:#fff;
                                }
                            </style>
                            <div><p>one</p><span>two</span></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "<p>one</p><span>two</span>", "<div><p>one</p><span>two</span></div>", null, false, true, 2, 0, 0);
			
			TestUtility.AnalyzeNode(node.Children.ElementAt(0), "p", "one", "<p>one</p>", node, false, true, 1, 0, 0);
			
			TestUtility.AnalyzeNode(node.Children.ElementAt(1), "span", "two", "<span>two</span>", node, false, true, 1, 0, 0);
		}
		
		[Test]
		public void SpanWithStyleNthLastChild()
		{
			string html = @"<style>
                                span:nth-last-child(1)
                                {
                                	color:#fff;
                                }
                            </style>
                            <div><p>one</p><span>two</span></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			IHtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "<p>one</p><span>two</span>", "<div><p>one</p><span>two</span></div>", null, false, true, 2, 0, 0);
			
			TestUtility.AnalyzeNode(node.Children.ElementAt(0), "p", "one", "<p>one</p>", node, false, true, 1, 0, 0);
			
			TestUtility.AnalyzeNode(node.Children.ElementAt(1), "span", "two", "<span>two</span>", node, false, true, 1, 0, 1);
			TestUtility.CheckKeyValuePair(node.Children.ElementAt(1).Styles.ElementAt(0), "color", "#fff");
		}
		
		[Test]
		public void TrNthChild()
		{
			string path = TestUtility.GetFolderPath("Html\\trnthchild.htm");
			string html = string.Empty;

			using (StreamReader sr = new StreamReader(path))
			{
				html = sr.ReadToEnd();
			}

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Parse());
			parser.ParseCSS();
			
            
			IHtmlNode node = parser.Current;
			
			Assert.IsNotNull(node);
			Assert.AreEqual(true, node.HasChildren);
			
			node = node.Children.ElementAt(0);
			
			while (node.Tag != "body")
			{
				node = node.Next;
			}
			
			node = node.Children.ElementAt(0);
			
			while (node.Tag != "table")
			{
				node = node.Next;
			}
			
			Assert.IsNotNull(node);
			Assert.AreEqual("table", node.Tag);
			
			foreach (IHtmlNode  tr in node.Children)
			{
				if (tr.Tag == "tr")
				{
                    Assert.AreEqual(1, tr.Styles.Count);
                    tr.Styles.CheckKeyValuePair(0, "color", "red");
					break;
				}
			}
		}

        [Test]
        public void ExcludePrintMediaCSS()
        {
            string path = TestUtility.GetFolderPath("Html\\mediaprintstyle.htm");
            string html = string.Empty;

            using (StreamReader sr = new StreamReader(path))
            {
                html = sr.ReadToEnd();
            }

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();
            parser.ParseCSS();

            IHtmlNode node = parser.Current.Children.ElementAt(0);

            while (node.Tag != "body")
                node = node.Next;

            node = node.Children.ElementAt(0);

            Assert.AreEqual("#f90", node.Styles["color"]);
        }

        [Test]
        public void ExcludePrintMediaCSSWithNoSpace()
        {
            string path = TestUtility.GetFolderPath("Html\\mediaprintwithnospace.htm");
            string html = string.Empty;

            using (StreamReader sr = new StreamReader(path))
            {
                html = sr.ReadToEnd();
            }

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();
            parser.ParseCSS();

            IHtmlNode node = parser.Current.Children.ElementAt(0);

            while (node.Tag != "body")
                node = node.Next;

            node = node.Children.ElementAt(0);

            Assert.AreEqual("#f90", node.Styles["color"]);
        }

        [Test]
        public void ExcludePrintMediaCSSTopOrder()
        {
            string path = TestUtility.GetFolderPath("Html\\mediaqueryatfirst.htm");
            string html = string.Empty;

            using (StreamReader sr = new StreamReader(path))
            {
                html = sr.ReadToEnd();
            }

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();
            parser.ParseCSS();

            IHtmlNode node = parser.Current.Children.ElementAt(0);

            while (node.Tag != "body")
                node = node.Next;

            node = node.Children.ElementAt(0);

            Assert.AreEqual("#f90", node.Styles["color"]);
        }

        [Test]
        public void EmptyMedia()
        {
            string path = TestUtility.GetFolderPath("Html\\emptymediaprint.htm");
            string html = string.Empty;

            using (StreamReader sr = new StreamReader(path))
            {
                html = sr.ReadToEnd();
            }

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();
            parser.ParseCSS();

            IHtmlNode node = parser.Current.Children.ElementAt(0);

            while (node.Tag != "body")
                node = node.Next;

            node = node.Children.ElementAt(0);

            Assert.AreEqual("#f90", node.Styles["color"]);
        }

        [Test]
        public void EmptyStyle()
        {
            string html = "<div style='color:!important;'>test</div>";

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();
            parser.ParseCSS();

            Assert.NotNull(parser.Current);
            TestUtility.AnalyzeNode(parser.Current, "div", "test", html, null, false, true, 1, 1, 0);
        }
	}
}
