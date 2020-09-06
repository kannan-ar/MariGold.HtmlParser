namespace MariGold.HtmlParser.Tests
{
	using System.Linq;
	using NUnit.Framework;
	using MariGold.HtmlParser;

	[TestFixture]
	public class PreviousNextNodes
	{
		[Test]
		public void SingleNode()
		{
			string html = "<div>test</div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "div", "test", html);
			Assert.IsNull(parser.Current.Previous);
			Assert.IsNull(parser.Current.Next);
			Assert.AreEqual(false, parser.Traverse());
			Assert.IsNull(parser.Current);
		}

		[Test]
		public void DivA()
		{
			string html = "<div>test</div><a>ano</a>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "div", "test", "<div>test</div>");
			Assert.IsNull(parser.Current.Previous);
			Assert.IsNull(parser.Current.Next);

			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "a", "ano", "<a>ano</a>");
			Assert.IsNotNull(parser.Current.Previous);
			Assert.IsNull(parser.Current.Next);
			TestUtility.AreEqual(parser.Current.Previous, "div", "test", "<div>test</div>");
			Assert.IsNotNull(parser.Current.Previous.Next);
			TestUtility.AreEqual(parser.Current.Previous.Next, "a", "ano", "<a>ano</a>");
		}

		[Test]
		public void DivInsideA()
		{
			string html = "<div><a>ano</a></div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "div", "<a>ano</a>", "<div><a>ano</a></div>");
			Assert.IsNull(parser.Current.Previous);
			Assert.IsNull(parser.Current.Next);

			Assert.IsNotNull(parser.Current.Children.ElementAt(0));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "a", "ano", "<a>ano</a>");
			Assert.IsNull(parser.Current.Previous);
			Assert.IsNull(parser.Current.Next);
		}

		[Test]
		public void DivInsideTextA()
		{
			string html = "<div>test<a>ano</a></div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "div", "test<a>ano</a>", "<div>test<a>ano</a></div>");
			Assert.IsNull(parser.Current.Previous);
			Assert.IsNull(parser.Current.Next);
			Assert.AreEqual(2, parser.Current.Children.Count());

			Assert.IsNotNull(parser.Current.Children.ElementAt(0));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
			Assert.IsNull(parser.Current.Children.ElementAt(0).Previous);
			Assert.IsNotNull(parser.Current.Children.ElementAt(0).Next);
			Assert.AreEqual(parser.Current.Children.ElementAt(0).Next, parser.Current.Children.ElementAt(1));

			Assert.IsNotNull(parser.Current.Children.ElementAt(1));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "a", "ano", "<a>ano</a>");
			Assert.IsNull(parser.Current.Children.ElementAt(1).Next);
			Assert.IsNotNull(parser.Current.Children.ElementAt(1).Previous);
			Assert.AreEqual(parser.Current.Children.ElementAt(1).Previous, parser.Current.Children.ElementAt(0));
		}

		[Test]
		public void DivInsideTextAP()
		{
			string html = "<div>test<a>ano</a></div><p>tt</p>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "div", "test<a>ano</a>", "<div>test<a>ano</a></div>");
			Assert.IsNull(parser.Current.Previous);
			Assert.IsNull(parser.Current.Next);
			Assert.AreEqual(2, parser.Current.Children.Count());

			Assert.IsNotNull(parser.Current.Children.ElementAt(0));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
			Assert.IsNull(parser.Current.Children.ElementAt(0).Previous);
			Assert.IsNotNull(parser.Current.Children.ElementAt(0).Next);
			Assert.AreEqual(parser.Current.Children.ElementAt(0).Next, parser.Current.Children.ElementAt(1));

			Assert.IsNotNull(parser.Current.Children.ElementAt(1));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "a", "ano", "<a>ano</a>");
			Assert.IsNull(parser.Current.Children.ElementAt(1).Next);
			Assert.IsNotNull(parser.Current.Children.ElementAt(1).Previous);
			Assert.AreEqual(parser.Current.Children.ElementAt(1).Previous, parser.Current.Children.ElementAt(0));

			Assert.IsTrue(parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "p", "tt", "<p>tt</p>");
			Assert.AreEqual(1, parser.Current.Children.Count());
			Assert.IsNotNull(parser.Current.Children.ElementAt(0));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "tt", "tt");
			Assert.IsNull(parser.Current.Children.ElementAt(0).Previous);
			Assert.IsNull(parser.Current.Children.ElementAt(0).Next);

			Assert.IsNotNull(parser.Current.Previous);
			Assert.IsNull(parser.Current.Next);
			TestUtility.AreEqual(parser.Current.Previous, "div", "test<a>ano</a>", "<div>test<a>ano</a></div>");
			Assert.IsNotNull(parser.Current.Previous.Next);
			TestUtility.AreEqual(parser.Current.Previous.Next, "p", "tt", "<p>tt</p>");
		}

		[Test]
		public void TextDiv()
		{
			string html = "test<div>ano</div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "#text", "test", "test");
			Assert.IsNull(parser.Current.Previous);
			Assert.IsNull(parser.Current.Next);

			Assert.IsTrue(parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "div", "ano", "<div>ano</div>");
			Assert.IsNotNull(parser.Current.Previous);
			Assert.IsNull(parser.Current.Next);

			TestUtility.AreEqual(parser.Current.Previous, "#text", "test", "test");
			Assert.IsNotNull(parser.Current.Previous.Next);
			TestUtility.AreEqual(parser.Current.Previous.Next, "div", "ano", "<div>ano</div>");
			Assert.IsNull(parser.Current.Previous.Previous);
		}

		[Test]
		public void DivText()
		{
			string html = "<div>ano</div>test";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "div", "ano", "<div>ano</div>");
			Assert.IsNull(parser.Current.Previous);
			Assert.IsNull(parser.Current.Next);

			Assert.IsTrue(parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "#text", "test", "test");
			Assert.IsNotNull(parser.Current.Previous);
			Assert.IsNull(parser.Current.Next);

			TestUtility.AreEqual(parser.Current.Previous, "div", "ano", "<div>ano</div>");
			Assert.IsNotNull(parser.Current.Previous.Next);
			TestUtility.AreEqual(parser.Current.Previous.Next, "#text", "test", "test");
			Assert.IsNull(parser.Current.Previous.Previous);

			Assert.AreEqual(parser.Current, parser.Current.Previous.Next);
			Assert.AreEqual(parser.Current.Previous, parser.Current.Previous.Next.Previous);
		}

		[Test]
		public void BrText()
		{
			string html = "<br />test";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "br", "<br />", "<br />");
			Assert.IsTrue(parser.Current.SelfClosing);
			Assert.IsNull(parser.Current.Previous);
			Assert.IsNull(parser.Current.Next);

			Assert.IsTrue(parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "#text", "test", "test");
			Assert.IsNotNull(parser.Current.Previous);
			Assert.IsNull(parser.Current.Next);

			TestUtility.AreEqual(parser.Current.Previous, "br", "<br />", "<br />");
			TestUtility.AreEqual(parser.Current.Previous.Next, "#text", "test", "test");
			Assert.AreEqual(parser.Current, parser.Current.Previous.Next);
			Assert.AreEqual(parser.Current.Previous, parser.Current.Previous.Next.Previous);
		}

		[Test]
		public void SimpleTable()
		{
			string html = "<table><tr><td>one</td><td>two</td></tr></table>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "table", "<tr><td>one</td><td>two</td></tr>", html);
			Assert.IsNull(parser.Current.Next);
			Assert.IsNull(parser.Current.Previous);

			Assert.AreEqual(1, parser.Current.Children.Count());
			Assert.IsNotNull(parser.Current.Children.ElementAt(0));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "tr", "<td>one</td><td>two</td>",
				"<tr><td>one</td><td>two</td></tr>");
			Assert.IsNull(parser.Current.Children.ElementAt(0).Next);
			Assert.IsNull(parser.Current.Children.ElementAt(0).Previous);

			Assert.AreEqual(2, parser.Current.Children.ElementAt(0).Children.Count());

			Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "td", "one", "<td>one</td>");
			Assert.IsNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Previous);
			Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Next);

			Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(1));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(1), "td", "two", "<td>two</td>");
			Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(1).Previous);
			Assert.IsNull(parser.Current.Children.ElementAt(0).Children.ElementAt(1).Next);

			TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Next, "td", "two", "<td>two</td>");
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(1).Previous, "td", "one", "<td>one</td>");
			Assert.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Next,
				parser.Current.Children.ElementAt(0).Children.ElementAt(1));
			Assert.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(1).Previous,
				parser.Current.Children.ElementAt(0).Children.ElementAt(0));
		}

		[Test]
		public void DivInsidePA()
		{
			string html = "<div><p>1</p><a>2</a></div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "div", "<p>1</p><a>2</a>", html);
			Assert.IsNull(parser.Current.Previous);
			Assert.IsNull(parser.Current.Next);
			Assert.AreEqual(2, parser.Current.Children.Count());

			Assert.IsNotNull(parser.Current.Children.ElementAt(0));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "1", "<p>1</p>");
			Assert.IsNull(parser.Current.Children.ElementAt(0).Previous);
			Assert.IsNotNull(parser.Current.Children.ElementAt(0).Next);

			Assert.IsNotNull(parser.Current.Children.ElementAt(1));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "a", "2", "<a>2</a>");
			Assert.IsNotNull(parser.Current.Children.ElementAt(1).Previous);
			Assert.IsNull(parser.Current.Children.ElementAt(1).Next);

			TestUtility.AreEqual(parser.Current.Children.ElementAt(1).Previous, "p", "1", "<p>1</p>");
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Next, "a", "2", "<a>2</a>");
			Assert.AreEqual(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(1).Previous);
			Assert.AreEqual(parser.Current.Children.ElementAt(1), parser.Current.Children.ElementAt(0).Next);
		}

		[Test]
		public void DivInsidePBr()
		{
			string html = "<div><p>1</p><br /></div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "div", "<p>1</p><br />", html);
			Assert.IsNull(parser.Current.Previous);
			Assert.IsNull(parser.Current.Next);
			Assert.AreEqual(2, parser.Current.Children.Count());

			Assert.IsNotNull(parser.Current.Children.ElementAt(0));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "1", "<p>1</p>");
			Assert.IsNull(parser.Current.Children.ElementAt(0).Previous);
			Assert.IsNotNull(parser.Current.Children.ElementAt(0).Next);

			Assert.IsNotNull(parser.Current.Children.ElementAt(1));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "br", "<br />", "<br />");
			Assert.IsTrue(parser.Current.Children.ElementAt(1).SelfClosing);
			Assert.IsNotNull(parser.Current.Children.ElementAt(1).Previous);
			Assert.IsNull(parser.Current.Children.ElementAt(1).Next);

			TestUtility.AreEqual(parser.Current.Children.ElementAt(1).Previous, "p", "1", "<p>1</p>");
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Next, "br", "<br />", "<br />");
			Assert.AreEqual(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(1).Previous);
			Assert.AreEqual(parser.Current.Children.ElementAt(1), parser.Current.Children.ElementAt(0).Next);
		}

		[Test]
		public void DivInsideBrP()
		{
			string html = "<div><br /><p>1</p></div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "div", "<br /><p>1</p>", html);
			Assert.IsNull(parser.Current.Previous);
			Assert.IsNull(parser.Current.Next);
			Assert.AreEqual(2, parser.Current.Children.Count());

			Assert.IsNotNull(parser.Current.Children.ElementAt(0));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "br", "<br />", "<br />");
			Assert.IsTrue(parser.Current.Children.ElementAt(0).SelfClosing);
			Assert.IsNull(parser.Current.Children.ElementAt(0).Previous);
			Assert.IsNotNull(parser.Current.Children.ElementAt(0).Next);

			Assert.IsNotNull(parser.Current.Children.ElementAt(1));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "p", "1", "<p>1</p>");
			Assert.IsNotNull(parser.Current.Children.ElementAt(1).Previous);
			Assert.IsNull(parser.Current.Children.ElementAt(1).Next);

			TestUtility.AreEqual(parser.Current.Children.ElementAt(1).Previous, "br", "<br />", "<br />");
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Next, "p", "1", "<p>1</p>");
			Assert.AreEqual(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(1).Previous);
			Assert.AreEqual(parser.Current.Children.ElementAt(1), parser.Current.Children.ElementAt(0).Next);
		}

		[Test]
		public void HtmlHeadBody()
		{
			string html = "<html><head></head><body></body></html>";

			HtmlParser parser = new HtmlTextParser(html);

			parser.Parse();

			IHtmlNode node = parser.Current;

			node.AnalyzeNode("html", "<head></head><body></body>", "<html><head></head><body></body></html>", null, false, true, 2, 0, 0);
			Assert.IsNull(node.Previous);
			Assert.IsNull(node.Next);

			IHtmlNode hnode = node;

			node = node.Children.ElementAt(0);

			node.AnalyzeNode("head", "", "<head></head>", hnode, false, false, 0, 0, 0);
			Assert.IsNull(node.Previous);
			Assert.IsNotNull(node.Next);

			node = node.Next;
			node.AnalyzeNode("body", "", "<body></body>", hnode, false, false, 0, 0, 0);
			Assert.IsNotNull(node.Previous);
			Assert.IsNull(node.Next);

		}
	}
}
