namespace MariGold.HtmlParser.Tests;

using MariGold.HtmlParser;
using System.Linq;
using Xunit;

public class FindTag
{
    [Fact]
    public void PInDiv()
    {
        string html = "<div><p>test</p></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.FindFirst("p"));
        Assert.NotNull(parser.Current);
        TestUtility.AreEqual(parser.Current, "p", "test", "<p>test</p>");
        Assert.Null(parser.Current.Parent);
        Assert.True(parser.Current.HasChildren);
        Assert.Single(parser.Current.Children);
        Assert.False(parser.Current.SelfClosing);
        Assert.Empty(parser.Current.Attributes);

        Assert.NotNull(parser.Current.Children.ElementAt(0));
        TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
        Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
        Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
        Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
        Assert.Empty(parser.Current.Children.ElementAt(0).Children);
        Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
        Assert.Empty(parser.Current.Children.ElementAt(0).Attributes);
    }

    [Fact]
    public void DivInsideTable()
    {
        string html = "<table><tr><td><div>test</div></td></tr></table>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.FindFirst("div"));
        Assert.NotNull(parser.Current);
        TestUtility.AreEqual(parser.Current, "div", "test", "<div>test</div>");
        Assert.Null(parser.Current.Parent);
        Assert.True(parser.Current.HasChildren);
        Assert.Single(parser.Current.Children);
        Assert.False(parser.Current.SelfClosing);
        Assert.Empty(parser.Current.Attributes);

        Assert.NotNull(parser.Current.Children.ElementAt(0));
        TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
        Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
        Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
        Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
        Assert.Empty(parser.Current.Children.ElementAt(0).Children);
        Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
        Assert.Empty(parser.Current.Children.ElementAt(0).Attributes);
    }

    [Fact]
    public void PDiv()
    {
        string html = "<p></p><div>test</div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.FindFirst("div"));
        Assert.NotNull(parser.Current);
        TestUtility.AreEqual(parser.Current, "div", "test", "<div>test</div>");
        Assert.Null(parser.Current.Parent);
        Assert.True(parser.Current.HasChildren);
        Assert.Single(parser.Current.Children);
        Assert.False(parser.Current.SelfClosing);
        Assert.Empty(parser.Current.Attributes);

        Assert.NotNull(parser.Current.Children.ElementAt(0));
        TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
        Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
        Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
        Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
        Assert.Empty(parser.Current.Children.ElementAt(0).Children);
        Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
        Assert.Empty(parser.Current.Children.ElementAt(0).Attributes);
    }

    [Fact]
    public void InvalidPOpenDiv()
    {
        string html = "<p><div>test</div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.FindFirst("div"));
        Assert.NotNull(parser.Current);
        TestUtility.AreEqual(parser.Current, "div", "test", "<div>test</div>");
        Assert.Null(parser.Current.Parent);
        Assert.True(parser.Current.HasChildren);
        Assert.Single(parser.Current.Children);
        Assert.False(parser.Current.SelfClosing);
        Assert.Empty(parser.Current.Attributes);

        Assert.NotNull(parser.Current.Children.ElementAt(0));
        TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
        Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
        Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
        Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
        Assert.Empty(parser.Current.Children.ElementAt(0).Children);
        Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
        Assert.Empty(parser.Current.Children.ElementAt(0).Attributes);
    }

    [Fact]
    public void InvalidPCloseDiv()
    {
        string html = "</p><div>test</div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.FindFirst("div"));
        Assert.NotNull(parser.Current);
        TestUtility.AreEqual(parser.Current, "div", "test", "<div>test</div>");
        Assert.Null(parser.Current.Parent);
        Assert.True(parser.Current.HasChildren);
        Assert.Single(parser.Current.Children);
        Assert.False(parser.Current.SelfClosing);
        Assert.Empty(parser.Current.Attributes);

        Assert.NotNull(parser.Current.Children.ElementAt(0));
        TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
        Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
        Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
        Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
        Assert.Empty(parser.Current.Children.ElementAt(0).Children);
        Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
        Assert.Empty(parser.Current.Children.ElementAt(0).Attributes);
    }
}
