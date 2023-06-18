namespace MariGold.HtmlParser.Tests;

using MariGold.HtmlParser;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class InvalidHtml
{
    [Fact]
    public void InvalidInputAttribute()
    {
        string html = "<input =\"\" name=\"fld_quicksign\">";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        Assert.NotNull(parser.Current);
        Assert.Single(parser.Current.Attributes);
        TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "name", "fld_quicksign");
    }

    [Fact]
    public async Task InvalidQuote()
    {
        string html = "<div style=\"width:100%\" \">test</div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        await parser.ParseStylesAsync();
        Assert.NotNull(parser.Current);
        parser.Current.AnalyzeNode("div", "test", html, null, false, true, 1, 1, 1);
    }

    [Fact]
    public async Task InputWithEmptyValue()
    {
        string html = "<div><input value=\"\" /><div style=\"width:100%\">1</div></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;
        Assert.NotNull(node);
        node.AnalyzeNode("div", "<input value=\"\" /><div style=\"width:100%\">1</div>", html, null, false, true, 2, 0, 0);
        IHtmlNode parent = node;

        node = node.Children.ElementAt(0);
        Assert.NotNull(node);
        node.AnalyzeNode("input", "<input value=\"\" />", "<input value=\"\" />", parent, true, false, 0, 1, 0);

        node = parent.Children.ElementAt(1);
        Assert.NotNull(node);
        node.AnalyzeNode("div", "1", "<div style=\"width:100%\">1</div>", parent, false, true, 1, 1, 1);
    }

    [Fact]
    public void InputWithEmptyQuote()
    {
        string html = "<input \"\" />";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        Assert.NotNull(parser.Current);
        parser.Current.AnalyzeNode("input", html, html, null, true, false, 0, 0, 0);
    }
}
