namespace MariGold.HtmlParser.Tests
{
	using System;
	using System.Linq;
	using NUnit.Framework;
	using MariGold.HtmlParser;

	[TestFixture]
	public class BasicHtml
	{

		[Test]
		public void ValidateHtmlIsNotEmpty()
		{
			string html = string.Empty;

			Assert.Throws<ArgumentNullException>(() => {
				new HtmlTextParser(html);
			}, "html");
		}

		[Test]
		public void OnlySpace()
		{
			string html = " ";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "#text", " ", " ");
			Assert.AreEqual(true, parser.Current.IsText);
			Assert.AreEqual(false, parser.Current.SelfClosing);
			Assert.AreEqual(false, parser.Current.HasChildren);
			Assert.AreEqual(0, parser.Current.Children.Count());
			Assert.AreEqual(0, parser.Current.Attributes.Count);
			Assert.IsNull(parser.Current.Parent);

			Assert.AreEqual(parser.Traverse(), false);
			Assert.IsNull(parser.Current);
		}

		[Test]
		public void OnlyAngular()
		{
			string html = "<";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "#text", "<", "<");
			Assert.AreEqual(true, parser.Current.IsText);
			Assert.AreEqual(false, parser.Current.SelfClosing);
			Assert.AreEqual(false, parser.Current.HasChildren);
			Assert.AreEqual(0, parser.Current.Children.Count());
			Assert.AreEqual(0, parser.Current.Attributes.Count);
			Assert.IsNull(parser.Current.Parent);

			Assert.AreEqual(parser.Traverse(), false);
		}

		[Test]
		public void TestTextOnly()
		{
			string html = "this is a test";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "#text", "this is a test", "this is a test");
			Assert.AreEqual(true, parser.Current.IsText);
			Assert.AreEqual(false, parser.Current.SelfClosing);
			Assert.AreEqual(false, parser.Current.HasChildren);
			Assert.AreEqual(0, parser.Current.Children.Count());
			Assert.AreEqual(0, parser.Current.Attributes.Count);
			Assert.IsNull(parser.Current.Parent);

			Assert.AreEqual(parser.Traverse(), false);
		}

		[Test]
		public void OneDiv()
		{
			string html = "<div>this is a test</div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "div", "this is a test", "<div>this is a test</div>");
			Assert.AreEqual(false, parser.Current.IsText);
			Assert.IsNull(parser.Current.Parent);
			Assert.AreEqual(false, parser.Current.SelfClosing);
			Assert.AreEqual(true, parser.Current.HasChildren);
			Assert.AreEqual(1, parser.Current.Children.Count());
            
			Assert.IsNotNull(parser.Current.Children.ElementAt(0));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "this is a test", "this is a test");
			Assert.AreEqual(true, parser.Current.Children.ElementAt(0).IsText);
			Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
			Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
			Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
			Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
			Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);
			Assert.AreEqual(parser.Traverse(), false);
		}

		[Test]
		public void OneDivWithNoCloseTag()
		{
			string html = "<div>this is a test";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "div", "this is a test", "<div>this is a test");
			Assert.AreEqual(false, parser.Current.IsText);
			Assert.IsNull(parser.Current.Parent);
			Assert.AreEqual(false, parser.Current.SelfClosing);
			Assert.AreEqual(true, parser.Current.HasChildren);
			Assert.AreEqual(1, parser.Current.Children.Count());

			Assert.IsNotNull(parser.Current.Children.ElementAt(0));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "this is a test", "this is a test");
			Assert.AreEqual(true, parser.Current.Children.ElementAt(0).IsText);
			Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
			Assert.AreEqual(false, parser.Current.Children.ElementAt(0).HasChildren);
			Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
			Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
			Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);

			Assert.AreEqual(false, parser.Traverse());
		}

		[Test]
		public void OneDivWithNoOpenTag()
		{
			string html = "this is a test</div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "#text", "this is a test", "this is a test");
			Assert.AreEqual(true, parser.Current.IsText);
			Assert.IsNull(parser.Current.Parent);
			Assert.AreEqual(false, parser.Current.SelfClosing);
			Assert.AreEqual(false, parser.Current.HasChildren);
			Assert.AreEqual(0, parser.Current.Children.Count());
			Assert.AreEqual(false, parser.Traverse());
		}

		[Test]
		public void OneDivWithSpaceInOpenTag()
		{
			string html = "< div>this is a test</div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "#text", "< div>this is a test", "< div>this is a test");
			Assert.AreEqual(true, parser.Current.IsText);
			Assert.IsNull(parser.Current.Parent);
			Assert.AreEqual(false, parser.Current.SelfClosing);
			Assert.AreEqual(false, parser.Current.HasChildren);
			Assert.AreEqual(0, parser.Current.Children.Count());
			Assert.AreEqual(false, parser.Traverse());
		}

		[Test]
		public void OneDivWithSpaceAtTheEndOpenTag()
		{
			string html = "<div >this is a test</div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "div", "this is a test", "<div >this is a test</div>");
			Assert.AreEqual(false, parser.Current.SelfClosing);
			Assert.AreEqual(true, parser.Current.HasChildren);
			Assert.AreEqual(1, parser.Current.Children.Count());
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "this is a test", "this is a test");
			Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
			Assert.AreEqual(false, parser.Current.Children.ElementAt(0).HasChildren);
			Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
			Assert.AreEqual(false, parser.Traverse());
		}

		[Test]
		public void OneDivWithSpaceAtTheCloseTag()
		{
			string html = "<div>this is a test< /div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "div", "this is a test< /div>", "<div>this is a test< /div>");
			Assert.AreEqual(false, parser.Current.IsText);
			Assert.AreEqual(false, parser.Current.SelfClosing);
			Assert.AreEqual(true, parser.Current.HasChildren);
			Assert.AreEqual(1, parser.Current.Children.Count());
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "this is a test< /div>", "this is a test< /div>");
			Assert.AreEqual(true, parser.Current.Children.ElementAt(0).IsText);
			Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
			Assert.AreEqual(false, parser.Current.Children.ElementAt(0).HasChildren);
			Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
			Assert.AreEqual(false, parser.Traverse());

		}

		[Test]
		public void OneDivWithSpaceAtTheEndOfCloseTag()
		{
			string html = "<div>this is a test</div >";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "div", "this is a test", "<div>this is a test</div >");
			Assert.AreEqual(false, parser.Current.IsText);
			Assert.AreEqual(false, parser.Current.SelfClosing);
			Assert.AreEqual(true, parser.Current.HasChildren);
			Assert.AreEqual(1, parser.Current.Children.Count());
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "this is a test", "this is a test");
			Assert.AreEqual(true, parser.Current.Children.ElementAt(0).IsText);
			Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
			Assert.AreEqual(false, parser.Current.Children.ElementAt(0).HasChildren);
			Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
			Assert.AreEqual(false, parser.Traverse());
		}

		[Test]
		public void OneDivWithSpaceAfterTheEscapeCharOfCloseTag()
		{
			string html = "<div>this is a test</ div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "div", "this is a test", "<div>this is a test</ div>");
			Assert.AreEqual(false, parser.Current.IsText);
			Assert.AreEqual(false, parser.Current.SelfClosing);
			Assert.AreEqual(true, parser.Current.HasChildren);
			Assert.AreEqual(1, parser.Current.Children.Count());
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "this is a test", "this is a test");
			Assert.AreEqual(true, parser.Current.Children.ElementAt(0).IsText);
			Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
			Assert.AreEqual(false, parser.Current.Children.ElementAt(0).HasChildren);
			Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
			Assert.AreEqual(false, parser.Traverse());
		}

		[Test]
		public void OneDivWithSpaceBetweenTheCloseTag()
		{
			string html = "<div>this is a test</d iv>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			Assert.AreEqual("div", parser.Current.Tag);
			Assert.AreEqual("<div>this is a test</d iv>", parser.Current.Html);
			Assert.AreEqual("this is a test</d iv>", parser.Current.InnerHtml);
			Assert.AreEqual(false, parser.Current.SelfClosing);
			Assert.AreEqual(true, parser.Current.HasChildren);
			Assert.AreEqual(1, parser.Current.Children.Count());
			Assert.AreEqual(false, parser.Current.IsText);
			
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "this is a test", "this is a test");
			Assert.AreEqual(true, parser.Current.Children.ElementAt(0).IsText);
			Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
			Assert.AreEqual(false, parser.Current.Children.ElementAt(0).HasChildren);
			Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
			Assert.AreEqual(false, parser.Traverse());
		}
		
		[Test]
		public void CloneNode()
		{
			string html = "<div id='dv' style='color:#fff'>this is a test</div>";

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Traverse());
			parser.ParseCSS();
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "div", "this is a test", html);
			Assert.AreEqual(false, parser.Current.IsText);
			Assert.IsNull(parser.Current.Parent);
			Assert.AreEqual(false, parser.Current.SelfClosing);
			Assert.AreEqual(true, parser.Current.HasChildren);
			Assert.AreEqual(1, parser.Current.Children.Count());
			Assert.AreEqual(1, parser.Current.Styles.Count);
			Assert.AreEqual(2, parser.Current.Attributes.Count);
			
			Assert.IsNotNull(parser.Current.Children.ElementAt(0));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "this is a test", "this is a test");
			Assert.AreEqual(true, parser.Current.Children.ElementAt(0).IsText);
			Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
			Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
			Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
			Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
			Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);
			
			IHtmlNode clone = parser.Current.Clone();
			Assert.IsNotNull(clone);
			Assert.AreEqual(parser.Current.Tag, clone.Tag);
			Assert.AreEqual(parser.Current.InnerHtml, clone.InnerHtml);
			Assert.AreEqual(parser.Current.Html, clone.Html);
			Assert.AreEqual(parser.Current.Parent, clone.Parent);
			Assert.AreEqual(parser.Current.Children.Count(), clone.Children.Count());
			
			for (int i = 0; parser.Current.Children.Count() > i; i++)
			{
				Assert.AreEqual(parser.Current.Children.ElementAt(i), clone.Children.ElementAt(i));
			}
			
			Assert.AreEqual(parser.Current.Previous, clone.Previous);
			Assert.AreEqual(parser.Current.Next, clone.Next);
			Assert.AreEqual(parser.Current.HasChildren, clone.HasChildren);
			Assert.AreEqual(parser.Current.SelfClosing, clone.SelfClosing);
			Assert.AreEqual(parser.Current.IsText, clone.IsText);
			
			Assert.AreEqual(parser.Current.Attributes.Count, clone.Attributes.Count);
			
			for (int i = 0; parser.Current.Attributes.Count() > i; i++)
			{
				Assert.AreEqual(parser.Current.Attributes.ElementAt(i).Key, clone.Attributes.ElementAt(i).Key);
				Assert.AreEqual(parser.Current.Attributes.ElementAt(i).Value, clone.Attributes.ElementAt(i).Value);
			}
			
			Assert.AreEqual(parser.Current.Styles.Count, clone.Styles.Count);
			
			for (int i = 0; parser.Current.Styles.Count() > i; i++)
			{
				Assert.AreEqual(parser.Current.Styles.ElementAt(i).Key, clone.Styles.ElementAt(i).Key);
				Assert.AreEqual(parser.Current.Styles.ElementAt(i).Value, clone.Styles.ElementAt(i).Value);
			}
			
			Assert.AreEqual(parser.Traverse(), false);
		}

        [Test]
        public void TwoLi()
        {
            string html = "<ul><li>1</li><li>2</li></ul>";

            HtmlParser parser = new HtmlTextParser(html);
            Assert.IsTrue(parser.Traverse());

            Assert.IsNotNull(parser.Current);
            parser.Current.AnalyzeNode("ul", "<li>1</li><li>2</li>", html, null, false, true, 2, 0, 0);

            IHtmlNode node = parser.Current.Children.ElementAt(0);
            Assert.IsNotNull(node);
            node.AnalyzeNode("li", "1", "<li>1</li>", parser.Current, false, true, 1, 0, 0);

            node = parser.Current.Children.ElementAt(1);
            Assert.IsNotNull(node);
            node.AnalyzeNode("li", "2", "<li>2</li>", parser.Current, false, true, 1, 0, 0);
        }
	}
}
