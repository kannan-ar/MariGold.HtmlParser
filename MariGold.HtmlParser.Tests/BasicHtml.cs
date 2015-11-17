namespace MariGold.HtmlParser.Tests
{
	using System;
	using NUnit.Framework;
	using MariGold.HtmlParser;

	[TestFixture]
	public class BasicHtml
	{

		[Test]
		public void ValidateHtmlIsNotEmpty()
		{
			string html = string.Empty;

			Assert.Throws<ArgumentNullException>(() =>
			{
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
			Assert.AreEqual(0, parser.Current.Children.Count);
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
			Assert.AreEqual(0, parser.Current.Children.Count);
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
			Assert.AreEqual(0, parser.Current.Children.Count);
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
			Assert.AreEqual(1, parser.Current.Children.Count);
            
			Assert.IsNotNull(parser.Current.Children[0]);
			TestUtility.AreEqual(parser.Current.Children[0], "#text", "this is a test", "this is a test");
			Assert.AreEqual(true, parser.Current.Children[0].IsText);
			Assert.IsNotNull(parser.Current.Children[0].Parent);
			Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
			Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
			Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
			Assert.AreEqual(0, parser.Current.Children[0].Attributes.Count);
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
			Assert.AreEqual(1, parser.Current.Children.Count);

			Assert.IsNotNull(parser.Current.Children[0]);
			TestUtility.AreEqual(parser.Current.Children[0], "#text", "this is a test", "this is a test");
			Assert.AreEqual(true, parser.Current.Children[0].IsText);
			Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
			Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
			Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
			Assert.IsNotNull(parser.Current.Children[0].Parent);
			Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);

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
			Assert.AreEqual(0, parser.Current.Children.Count);
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
			Assert.AreEqual(0, parser.Current.Children.Count);
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
			Assert.AreEqual(1, parser.Current.Children.Count);
			TestUtility.AreEqual(parser.Current.Children[0], "#text", "this is a test", "this is a test");
			Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
			Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
			Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
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
			Assert.AreEqual(1, parser.Current.Children.Count);
			TestUtility.AreEqual(parser.Current.Children[0], "#text", "this is a test< /div>", "this is a test< /div>");
			Assert.AreEqual(true, parser.Current.Children[0].IsText);
			Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
			Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
			Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
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
			Assert.AreEqual(1, parser.Current.Children.Count);
			TestUtility.AreEqual(parser.Current.Children[0], "#text", "this is a test", "this is a test");
			Assert.AreEqual(true, parser.Current.Children[0].IsText);
			Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
			Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
			Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
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
			Assert.AreEqual(1, parser.Current.Children.Count);
			TestUtility.AreEqual(parser.Current.Children[0], "#text", "this is a test", "this is a test");
			Assert.AreEqual(true, parser.Current.Children[0].IsText);
			Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
			Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
			Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
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
			Assert.AreEqual(1, parser.Current.Children.Count);
			Assert.AreEqual(false, parser.Current.IsText);
			
			TestUtility.AreEqual(parser.Current.Children[0], "#text", "this is a test", "this is a test");
			Assert.AreEqual(true, parser.Current.Children[0].IsText);
			Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
			Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
			Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
			Assert.AreEqual(false, parser.Traverse());
		}
	}
}
