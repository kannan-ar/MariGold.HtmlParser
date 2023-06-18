namespace MariGold.HtmlParser.Tests;

using MariGold.HtmlParser;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class BasicHtml
{

	[Fact]
	public void ValidateHtmlIsNotEmpty()
	{
		string html = string.Empty;

		Assert.Throws<ArgumentNullException>("html", () =>
		{
			new HtmlTextParser(html);
		});
	}

	[Fact]
	public void OnlySpace()
	{
		string html = " ";

		HtmlParser parser = new HtmlTextParser(html);

		Assert.True(parser.Traverse());
		Assert.NotNull(parser.Current);
		TestUtility.AreEqual(parser.Current, "#text", " ", " ");
		Assert.True(parser.Current.IsText);
		Assert.False(parser.Current.SelfClosing);
		Assert.False(parser.Current.HasChildren);
		Assert.Empty(parser.Current.Children);
		Assert.Empty(parser.Current.Attributes);
		Assert.Null(parser.Current.Parent);

		Assert.False(parser.Traverse());
		Assert.Null(parser.Current);
	}

	[Fact]
	public void OnlyAngular()
	{
		string html = "<";

		HtmlParser parser = new HtmlTextParser(html);

		Assert.True(parser.Traverse());
		Assert.NotNull(parser.Current);
		TestUtility.AreEqual(parser.Current, "#text", "<", "<");
		Assert.True(parser.Current.IsText);
		Assert.False(parser.Current.SelfClosing);
		Assert.False(parser.Current.HasChildren);
		Assert.Empty(parser.Current.Children);
		Assert.Empty(parser.Current.Attributes);
		Assert.Null(parser.Current.Parent);

		Assert.False(parser.Traverse());
	}

	[Fact]
	public void TestTextOnly()
	{
		string html = "this is a test";

		HtmlParser parser = new HtmlTextParser(html);

		Assert.True(parser.Traverse());
		Assert.NotNull(parser.Current);
		TestUtility.AreEqual(parser.Current, "#text", "this is a test", "this is a test");
		Assert.True(parser.Current.IsText);
		Assert.False(parser.Current.SelfClosing);
		Assert.False(parser.Current.HasChildren);
		Assert.Empty(parser.Current.Children);
		Assert.Empty(parser.Current.Attributes);
		Assert.Null(parser.Current.Parent);

		Assert.False(parser.Traverse());
	}

	[Fact]
	public void OneDiv()
	{
		string html = "<div>this is a test</div>";

		HtmlParser parser = new HtmlTextParser(html);

		Assert.True(parser.Traverse());
		Assert.NotNull(parser.Current);
		TestUtility.AreEqual(parser.Current, "div", "this is a test", "<div>this is a test</div>");
		Assert.False(parser.Current.IsText);
		Assert.Null(parser.Current.Parent);
		Assert.False(parser.Current.SelfClosing);
		Assert.True(parser.Current.HasChildren);
		Assert.Single(parser.Current.Children);

		Assert.NotNull(parser.Current.Children.ElementAt(0));
		TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "this is a test", "this is a test");
		Assert.True(parser.Current.Children.ElementAt(0).IsText);
		Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
		Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
		Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
		Assert.Empty(parser.Current.Children.ElementAt(0).Children);
		Assert.Empty(parser.Current.Children.ElementAt(0).Attributes);
		Assert.False(parser.Traverse());
	}

	[Fact]
	public void OneDivWithNoCloseTag()
	{
		string html = "<div>this is a test";

		HtmlParser parser = new HtmlTextParser(html);

		Assert.True(parser.Traverse());
		Assert.NotNull(parser.Current);
		TestUtility.AreEqual(parser.Current, "div", "this is a test", "<div>this is a test");
		Assert.False(parser.Current.IsText);
		Assert.Null(parser.Current.Parent);
		Assert.False(parser.Current.SelfClosing);
		Assert.True(parser.Current.HasChildren);
		Assert.Single(parser.Current.Children);

		Assert.NotNull(parser.Current.Children.ElementAt(0));
		TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "this is a test", "this is a test");
		Assert.True(parser.Current.Children.ElementAt(0).IsText);
		Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
		Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
		Assert.Empty(parser.Current.Children.ElementAt(0).Children);
		Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
		Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);

		Assert.False(parser.Traverse());
	}

	[Fact]
	public void OneDivWithNoOpenTag()
	{
		string html = "this is a test</div>";

		HtmlParser parser = new HtmlTextParser(html);

		Assert.True(parser.Traverse());
		Assert.NotNull(parser.Current);
		TestUtility.AreEqual(parser.Current, "#text", "this is a test", "this is a test");
		Assert.True(parser.Current.IsText);
		Assert.Null(parser.Current.Parent);
		Assert.False(parser.Current.SelfClosing);
		Assert.False(parser.Current.HasChildren);
		Assert.Empty(parser.Current.Children);
		Assert.False(parser.Traverse());
	}

	[Fact]
	public void OneDivWithSpaceInOpenTag()
	{
		string html = "< div>this is a test</div>";

		HtmlParser parser = new HtmlTextParser(html);

		Assert.True(parser.Traverse());
		Assert.NotNull(parser.Current);
		TestUtility.AreEqual(parser.Current, "#text", "< div>this is a test", "< div>this is a test");
		Assert.True(parser.Current.IsText);
		Assert.Null(parser.Current.Parent);
		Assert.False(parser.Current.SelfClosing);
		Assert.False(parser.Current.HasChildren);
		Assert.Empty(parser.Current.Children);
		Assert.False(parser.Traverse());
	}

	[Fact]
	public void OneDivWithSpaceAtTheEndOpenTag()
	{
		string html = "<div >this is a test</div>";

		HtmlParser parser = new HtmlTextParser(html);

		Assert.True(parser.Traverse());
		Assert.NotNull(parser.Current);
		TestUtility.AreEqual(parser.Current, "div", "this is a test", "<div >this is a test</div>");
		Assert.False(parser.Current.SelfClosing);
		Assert.True(parser.Current.HasChildren);
		Assert.Single(parser.Current.Children);
		TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "this is a test", "this is a test");
		Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
		Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
		Assert.Empty(parser.Current.Children.ElementAt(0).Children);
		Assert.False(parser.Traverse());
	}

	[Fact]
	public void OneDivWithSpaceAtTheCloseTag()
	{
		string html = "<div>this is a test< /div>";

		HtmlParser parser = new HtmlTextParser(html);

		Assert.True(parser.Traverse());
		Assert.NotNull(parser.Current);
		TestUtility.AreEqual(parser.Current, "div", "this is a test< /div>", "<div>this is a test< /div>");
		Assert.False(parser.Current.IsText);
		Assert.False(parser.Current.SelfClosing);
		Assert.True(parser.Current.HasChildren);
		Assert.Single(parser.Current.Children);
		TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "this is a test< /div>", "this is a test< /div>");
		Assert.True(parser.Current.Children.ElementAt(0).IsText);
		Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
		Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
		Assert.Empty(parser.Current.Children.ElementAt(0).Children);
		Assert.False(parser.Traverse());

	}

	[Fact]
	public void OneDivWithSpaceAtTheEndOfCloseTag()
	{
		string html = "<div>this is a test</div >";

		HtmlParser parser = new HtmlTextParser(html);

		Assert.True(parser.Traverse());
		Assert.NotNull(parser.Current);
		TestUtility.AreEqual(parser.Current, "div", "this is a test", "<div>this is a test</div >");
		Assert.False(parser.Current.IsText);
		Assert.False(parser.Current.SelfClosing);
		Assert.True(parser.Current.HasChildren);
		Assert.Single(parser.Current.Children);
		TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "this is a test", "this is a test");
		Assert.True(parser.Current.Children.ElementAt(0).IsText);
		Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
		Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
		Assert.Empty(parser.Current.Children.ElementAt(0).Children);
		Assert.False(parser.Traverse());
	}

	[Fact]
	public void OneDivWithSpaceAfterTheEscapeCharOfCloseTag()
	{
		string html = "<div>this is a test</ div>";

		HtmlParser parser = new HtmlTextParser(html);

		Assert.True(parser.Traverse());
		Assert.NotNull(parser.Current);
		TestUtility.AreEqual(parser.Current, "div", "this is a test", "<div>this is a test</ div>");
		Assert.False(parser.Current.IsText);
		Assert.False(parser.Current.SelfClosing);
		Assert.True(parser.Current.HasChildren);
		Assert.Single(parser.Current.Children);
		TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "this is a test", "this is a test");
		Assert.True(parser.Current.Children.ElementAt(0).IsText);
		Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
		Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
		Assert.Empty(parser.Current.Children.ElementAt(0).Children);
		Assert.False(parser.Traverse());
	}

	[Fact]
	public void OneDivWithSpaceBetweenTheCloseTag()
	{
		string html = "<div>this is a test</d iv>";

		HtmlParser parser = new HtmlTextParser(html);

		Assert.True(parser.Traverse());
		Assert.NotNull(parser.Current);
		Assert.Equal("div", parser.Current.Tag);
		Assert.Equal("<div>this is a test</d iv>", parser.Current.Html);
		Assert.Equal("this is a test</d iv>", parser.Current.InnerHtml);
		Assert.False(parser.Current.SelfClosing);
		Assert.True(parser.Current.HasChildren);
		Assert.Single(parser.Current.Children);
		Assert.False(parser.Current.IsText);

		TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "this is a test", "this is a test");
		Assert.True(parser.Current.Children.ElementAt(0).IsText);
		Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
		Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
		Assert.Empty(parser.Current.Children.ElementAt(0).Children);
		Assert.False(parser.Traverse());
	}

	[Fact]
	public async Task CloneNode()
	{
		string html = "<div id='dv' style='color:#fff'>this is a test</div>";

		HtmlParser parser = new HtmlTextParser(html);

		Assert.True(parser.Traverse());
		await parser.ParseStylesAsync();
		Assert.NotNull(parser.Current);
		TestUtility.AreEqual(parser.Current, "div", "this is a test", html);
		Assert.False(parser.Current.IsText);
		Assert.Null(parser.Current.Parent);
		Assert.False(parser.Current.SelfClosing);
		Assert.True(parser.Current.HasChildren);
		Assert.Single(parser.Current.Children);
		Assert.Single(parser.Current.Styles);
		Assert.Equal(2, parser.Current.Attributes.Count);

		Assert.NotNull(parser.Current.Children.ElementAt(0));
		TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "this is a test", "this is a test");
		Assert.True(parser.Current.Children.ElementAt(0).IsText);
		Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
		Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
		Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
		Assert.Empty(parser.Current.Children.ElementAt(0).Children);
		Assert.Empty(parser.Current.Children.ElementAt(0).Attributes);

		IHtmlNode clone = parser.Current.Clone();
		Assert.NotNull(clone);
		Assert.Equal(parser.Current.Tag, clone.Tag);
		Assert.Equal(parser.Current.InnerHtml, clone.InnerHtml);
		Assert.Equal(parser.Current.Html, clone.Html);
		Assert.Equal(parser.Current.Parent, clone.Parent);
		Assert.Equal(parser.Current.Children.Count(), clone.Children.Count());

		for (int i = 0; parser.Current.Children.Count() > i; i++)
		{
			Assert.Equal(parser.Current.Children.ElementAt(i), clone.Children.ElementAt(i));
		}

		Assert.Equal(parser.Current.Previous, clone.Previous);
		Assert.Equal(parser.Current.Next, clone.Next);
		Assert.Equal(parser.Current.HasChildren, clone.HasChildren);
		Assert.Equal(parser.Current.SelfClosing, clone.SelfClosing);
		Assert.Equal(parser.Current.IsText, clone.IsText);

		Assert.Equal(parser.Current.Attributes.Count, clone.Attributes.Count);

		for (int i = 0; parser.Current.Attributes.Count() > i; i++)
		{
			Assert.Equal(parser.Current.Attributes.ElementAt(i).Key, clone.Attributes.ElementAt(i).Key);
			Assert.Equal(parser.Current.Attributes.ElementAt(i).Value, clone.Attributes.ElementAt(i).Value);
		}

		Assert.Equal(parser.Current.Styles.Count, clone.Styles.Count);

		for (int i = 0; parser.Current.Styles.Count() > i; i++)
		{
			Assert.Equal(parser.Current.Styles.ElementAt(i).Key, clone.Styles.ElementAt(i).Key);
			Assert.Equal(parser.Current.Styles.ElementAt(i).Value, clone.Styles.ElementAt(i).Value);
		}

		Assert.False(parser.Traverse());
	}

	[Fact]
	public void TwoLi()
	{
		string html = "<ul><li>1</li><li>2</li></ul>";

		HtmlParser parser = new HtmlTextParser(html);
		Assert.True(parser.Traverse());

		Assert.NotNull(parser.Current);
		parser.Current.AnalyzeNode("ul", "<li>1</li><li>2</li>", html, null, false, true, 2, 0, 0);

		IHtmlNode node = parser.Current.Children.ElementAt(0);
		Assert.NotNull(node);
		node.AnalyzeNode("li", "1", "<li>1</li>", parser.Current, false, true, 1, 0, 0);

		node = parser.Current.Children.ElementAt(1);
		Assert.NotNull(node);
		node.AnalyzeNode("li", "2", "<li>2</li>", parser.Current, false, true, 1, 0, 0);
	}

	[Fact]
	public async Task DuplicateStyle()
	{
		string html = "<div style='color:#fff;color:#000;'>test</div>";

		HtmlParser parser = new HtmlTextParser(html);
		Assert.True(parser.Parse());
		await parser.ParseStylesAsync();

		Assert.NotNull(parser.Current);
		TestUtility.AnalyzeNode(parser.Current, "div", "test", "<div style='color:#fff;color:#000;'>test</div>",
			null, false, true, 1, 1, 1);
		parser.Current.CheckStyle(0, "color", "#fff");
	}

	[Fact]
	public void InputOnly()
	{
		string html = "<input />";

		HtmlParser parser = new HtmlTextParser(html);
		Assert.True(parser.Parse());
		Assert.NotNull(parser.Current);
		parser.Current.AnalyzeNode("input", "<input />", html, null, true, false, 0, 0, 0);
	}
}
