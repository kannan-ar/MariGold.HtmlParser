namespace MariGold.HtmlParser.Tests;

using MariGold.HtmlParser;
using System.Linq;
using System.IO;
    using Xunit;
using System.Threading.Tasks;

public class ExternalStyles
{
	[Fact]
	public async Task BasicExternalStyleSheet()
	{
		string path = TestUtility.GetFolderPath("Html\\basicexternalstyle.htm");
		string html = string.Empty;

		using (StreamReader sr = new StreamReader(path))
		{
			html = sr.ReadToEnd();
		}

		HtmlParser parser = new HtmlTextParser(html);

		Assert.True(parser.Parse());
		 await parser.ParseStylesAsync();

		Assert.NotNull(parser.Current);

		IHtmlNode node = parser.Current.Children.ElementAt(0);

		while (node.Tag != "body")
			node = node.Next;

		IHtmlNode body = node;

		node = node.Children.ElementAt(0);

		while (node.Tag != "p")
			node = node.Next;

		node.AnalyzeNode("p", "test", "<p>test</p>", body, false, true, 1, 0, 1);
		node.Styles.CheckKeyValuePair(0, "font-size", "20px");
	}

	[Fact]
	public async Task BasicExternalStyleSheetWithImportant()
	{
		string path = TestUtility.GetFolderPath("Html\\basicstyleimportant.html");
		string html = string.Empty;

		using (StreamReader sr = new StreamReader(path))
		{
			html = sr.ReadToEnd();
		}

		HtmlParser parser = new HtmlTextParser(html);

		Assert.True(parser.Parse());
		 await parser.ParseStylesAsync();

		Assert.NotNull(parser.Current);

		IHtmlNode node = parser.Current.Children.ElementAt(0);

		while (node.Tag != "body")
			node = node.Next;

		IHtmlNode body = node;

		node = node.Children.ElementAt(0);

		while (node.Tag != "div")
			node = node.Next;

		node.AnalyzeNode("div", "test", "<div class=\"cls\">test</div>", body, false, true, 1, 1, 1);
		node.Attributes.CheckKeyValuePair(0, "class", "cls");
		node.Styles.CheckKeyValuePair(0, "font-size", "10px");
	}

	[Fact]
	public async Task BasicExternalStyleSheetWithTwoElements()
	{
		string path = TestUtility.GetFolderPath("Html\\basicstylewithtwoelements.html");
		string html = string.Empty;

		using (StreamReader sr = new StreamReader(path))
		{
			html = sr.ReadToEnd();
		}

		HtmlParser parser = new HtmlTextParser(html);

		Assert.True(parser.Parse());
		 await parser.ParseStylesAsync();

		Assert.NotNull(parser.Current);

		IHtmlNode node = parser.Current.Children.ElementAt(0);

		while (node.Tag != "body")
			node = node.Next;

		IHtmlNode body = node;

		node = node.Children.ElementAt(0);

		while (node.Tag != "div")
			node = node.Next;

		node.AnalyzeNode("div", "test", "<div class=\"cls\">test</div>", body, false, true, 1, 1, 1);
		node.Attributes.CheckKeyValuePair(0, "class", "cls");
		node.Styles.CheckKeyValuePair(0, "font-size", "10px");
	}

	[Fact]
	public async Task ATagWithStyle()
	{
		string path = TestUtility.GetFolderPath("Html\\atagwithstyle.htm");
		string html = string.Empty;

		using (StreamReader sr = new StreamReader(path))
		{
			html = sr.ReadToEnd();
		}

		HtmlParser parser = new HtmlTextParser(html);

		Assert.True(parser.Parse());
		 await parser.ParseStylesAsync();

		Assert.NotNull(parser.Current);

		IHtmlNode node = parser.Current;

		while (node.Tag != "html")
			node = node.Next;

		node = node.Children.ElementAt(0);

		while (node.Tag != "body")
			node = node.Next;

		IHtmlNode body = node;

		node = node.Children.ElementAt(0);

		while (node.Tag != "a")
			node = node.Next;

		node.AnalyzeNode("a", "google", "<a href=\"http://google.com\">google</a>", body, false, true, 1, 1, 2);
		node.Attributes.CheckKeyValuePair(0, "href", "http://google.com");
		node.Styles.CheckKeyValuePair(0, "color", "#a21f1f");
		node.Styles.CheckKeyValuePair(1, "text-decoration", "none");

	}

	[Fact]
	public async Task RemoteStyleSheet()
	{
		string path = TestUtility.GetFolderPath("Html\\remotestylesheet.htm");
		string html = string.Empty;

		using (StreamReader sr = new StreamReader(path))
		{
			html = sr.ReadToEnd();
		}

		HtmlParser parser = new HtmlTextParser(html);

		Assert.True(parser.Parse());
		 await parser.ParseStylesAsync();

		Assert.NotNull(parser.Current);

		IHtmlNode node = parser.Current.Children.ElementAt(0);

		while (node.Tag != "body")
			node = node.Next;

		IHtmlNode body = node;

		node = node.Children.ElementAt(0);

		while (node.Tag != "p")
			node = node.Next;

		node.AnalyzeNode("p", "test", "<p class=\"well\">test</p>", body, false, true, 1, 1);
		Assert.True(node.Styles.Count > 0);
	}
}
