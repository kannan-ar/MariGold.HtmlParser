namespace MariGold.HtmlParser.Tests;

using MariGold.HtmlParser;
using System.Linq;
using Xunit;

public class ParseMethod
{
    [Fact]
    public void EmptyHtml()
    {
        string html = " ";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        Assert.NotNull(parser.Current);
        TestUtility.AreEqual(parser.Current, "#text", html, html);
        Assert.Null(parser.Current.Previous);
        Assert.Null(parser.Current.Next);
    }

    [Fact]
    public void SingleNode()
    {
        string html = "<div></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        Assert.NotNull(parser.Current);
        TestUtility.AreEqual(parser.Current, "div", "", html);
        Assert.Null(parser.Current.Previous);
        Assert.Null(parser.Current.Next);

        Assert.False(parser.Parse());
    }

    [Fact]
    public void DivAndP()
    {
        string html = "<div>test</div><p>one</p>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        Assert.NotNull(parser.Current);
        TestUtility.AreEqual(parser.Current, "div", "test", "<div>test</div>");

        Assert.Null(parser.Current.Previous);
        Assert.NotNull(parser.Current.Next);
        TestUtility.AreEqual(parser.Current.Next, "p", "one", "<p>one</p>");
        Assert.NotNull(parser.Current.Next.Previous);
        Assert.Equal(parser.Current, parser.Current.Next.Previous);
        Assert.Null(parser.Current.Next.Next);

        Assert.False(parser.Parse());
    }

    [Fact]
    public void DivInsidePAndB()
    {
        string html = "<div><p>one</p></div><b></b>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        Assert.NotNull(parser.Current);
        TestUtility.AreEqual(parser.Current, "div", "<p>one</p>", "<div><p>one</p></div>");
        Assert.True(parser.Current.HasChildren);
        Assert.Single(parser.Current.Children);
        Assert.NotNull(parser.Current.Children.ElementAt(0));
        TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "one", "<p>one</p>");
        Assert.Null(parser.Current.Children.ElementAt(0).Previous);
        Assert.Null(parser.Current.Children.ElementAt(0).Next);
        Assert.Null(parser.Current.Previous);
        Assert.NotNull(parser.Current.Next);
        TestUtility.AreEqual(parser.Current.Next, "b", "", "<b></b>");

        Assert.Null(parser.Current.Next.Next);
        Assert.NotNull(parser.Current.Next.Previous);
        Assert.Equal(parser.Current, parser.Current.Next.Previous);

        Assert.False(parser.Parse());
    }

    [Fact]
    public void DivPB()
    {
        string html = "<div></div><p></p><b></b>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        Assert.NotNull(parser.Current);
        TestUtility.AreEqual(parser.Current, "div", "", "<div></div>");
        Assert.Null(parser.Current.Previous);
        Assert.NotNull(parser.Current.Next);

        TestUtility.AreEqual(parser.Current.Next, "p", "", "<p></p>");
        Assert.NotNull(parser.Current.Next.Previous);
        Assert.NotNull(parser.Current.Next.Next);
        Assert.Equal(parser.Current, parser.Current.Next.Previous);

        Assert.NotNull(parser.Current.Next.Next);
        TestUtility.AreEqual(parser.Current.Next.Next, "b", "", "<b></b>");
        Assert.Null(parser.Current.Next.Next.Next);
        Assert.NotNull(parser.Current.Next.Next.Previous);
        Assert.Equal(parser.Current.Next, parser.Current.Next.Next.Previous);

        Assert.Equal(parser.Current, parser.Current.Next.Next.Previous.Previous);

        Assert.False(parser.Parse());
    }
}
