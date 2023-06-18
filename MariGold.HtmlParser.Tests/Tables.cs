namespace MariGold.HtmlParser.Tests;

using MariGold.HtmlParser;
using System.Linq;
using Xunit;

public class Tables
{
    [Fact]
    public void SimpleTable()
    {
        string html = "<table><tr><td>1</td></tr></table>";

        HtmlTextParser parser = new(html);

        Assert.True(parser.Traverse());
        Assert.NotNull(parser.Current);
        TestUtility.AreEqual(parser.Current, "table", "<tr><td>1</td></tr>", "<table><tr><td>1</td></tr></table>");
        Assert.Null(parser.Current.Parent);

        Assert.NotNull(parser.Current.Children);
        Assert.Single(parser.Current.Children);
        Assert.NotNull(parser.Current.Children.ElementAt(0));
        TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "tr", "<td>1</td>", "<tr><td>1</td></tr>");
        Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
        Assert.Equal(parser.Current.Children.ElementAt(0).Parent, parser.Current);

        Assert.NotNull(parser.Current.Children.ElementAt(0).Children);
        Assert.Single(parser.Current.Children.ElementAt(0).Children);
        Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
        TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "td", "1", "<td>1</td>");
        Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
        Assert.Equal(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent, parser.Current.Children.ElementAt(0));

        Assert.False(parser.Traverse());
        Assert.Null(parser.Current);

    }

    [Fact]
    public void TableWithOneRowAndTwoColumn()
    {
        string html = "<table><tr><td>1</td><td></td></tr></table>";

        HtmlTextParser parser = new(html);

        Assert.True(parser.Traverse());
        Assert.NotNull(parser.Current);
        TestUtility.AreEqual(parser.Current, "table", "<tr><td>1</td><td></td></tr>", "<table><tr><td>1</td><td></td></tr></table>");
        Assert.Null(parser.Current.Parent);

        Assert.NotNull(parser.Current.Children);
        Assert.Single(parser.Current.Children);
        Assert.NotNull(parser.Current.Children.ElementAt(0));
        TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "tr", "<td>1</td><td></td>", "<tr><td>1</td><td></td></tr>");
        Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
        Assert.Equal(parser.Current.Children.ElementAt(0).Parent, parser.Current);

        Assert.NotNull(parser.Current.Children.ElementAt(0).Children);
        Assert.Equal(2, parser.Current.Children.ElementAt(0).Children.Count());

        Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
        TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "td", "1", "<td>1</td>");
        Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
        Assert.Equal(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent, parser.Current.Children.ElementAt(0));

        Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(1));
        TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(1), "td", "", "<td></td>");
        Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(1).Parent);
        Assert.Equal(parser.Current.Children.ElementAt(0).Children.ElementAt(1).Parent, parser.Current.Children.ElementAt(0));

        Assert.False(parser.Traverse());
        Assert.Null(parser.Current);

    }

    [Fact]
    public void TwoRowWithOneColumnEach()
    {
        string html = "<table><tr><td>test1</td></tr><tr><td>test2</td></tr></table>";

        HtmlTextParser parser = new(html);

        Assert.True(parser.Traverse());

        Assert.NotNull(parser.Current);
        TestUtility.AreEqual(parser.Current, "table", "<tr><td>test1</td></tr><tr><td>test2</td></tr>", html);
        Assert.Null(parser.Current.Parent);

        Assert.NotNull(parser.Current.Children);
        Assert.Equal(2, parser.Current.Children.Count());

        Assert.NotNull(parser.Current.Children.ElementAt(0));
        TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "tr", "<td>test1</td>", "<tr><td>test1</td></tr>");
        Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);

        Assert.NotNull(parser.Current.Children.ElementAt(0).Children);
        Assert.Single(parser.Current.Children.ElementAt(0).Children);
        Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
        TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "td", "test1", "<td>test1</td>");
        Assert.Equal(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);

        Assert.NotNull(parser.Current.Children.ElementAt(1));
        TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "tr", "<td>test2</td>", "<tr><td>test2</td></tr>");
        Assert.Equal(parser.Current, parser.Current.Children.ElementAt(1).Parent);

        Assert.NotNull(parser.Current.Children.ElementAt(1).Children);
        Assert.Single(parser.Current.Children.ElementAt(1).Children);
        Assert.NotNull(parser.Current.Children.ElementAt(1).Children.ElementAt(0));
        TestUtility.AreEqual(parser.Current.Children.ElementAt(1).Children.ElementAt(0), "td", "test2", "<td>test2</td>");
        Assert.Equal(parser.Current.Children.ElementAt(1), parser.Current.Children.ElementAt(1).Children.ElementAt(0).Parent);
    }

    [Fact]
    public void OneCellWithSpanText()
    {
        string html = "<table><tr><td><span>s1</span>test</td></tr></table>";


        HtmlTextParser parser = new(html);

        Assert.True(parser.Traverse());
        Assert.NotNull(parser.Current);
        TestUtility.AreEqual(parser.Current, "table", "<tr><td><span>s1</span>test</td></tr>", html);

        Assert.NotNull(parser.Current.Children);
        Assert.Single(parser.Current.Children);
        Assert.NotNull(parser.Current.Children.ElementAt(0));
        TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "tr", "<td><span>s1</span>test</td>",
            "<tr><td><span>s1</span>test</td></tr>");
        Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);

        Assert.NotNull(parser.Current.Children.ElementAt(0).Children);
        Assert.Single(parser.Current.Children.ElementAt(0).Children);
        Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
        TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "td",
            "<span>s1</span>test",
            "<td><span>s1</span>test</td>");
        Assert.Equal(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
        Assert.Equal(2, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.Count());
    }

    [Fact]
    public void CloneTable()
    {
        string html = "<table><tr><td>1</td></tr></table>";

        HtmlTextParser parser = new HtmlTextParser(html);

        Assert.True(parser.Traverse());
        Assert.NotNull(parser.Current);
        TestUtility.AreEqual(parser.Current, "table", "<tr><td>1</td></tr>", "<table><tr><td>1</td></tr></table>");
        Assert.Null(parser.Current.Parent);

        Assert.NotNull(parser.Current.Children);
        Assert.Single(parser.Current.Children);
        Assert.NotNull(parser.Current.Children.ElementAt(0));
        TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "tr", "<td>1</td>", "<tr><td>1</td></tr>");
        Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
        Assert.Equal(parser.Current.Children.ElementAt(0).Parent, parser.Current);

        Assert.NotNull(parser.Current.Children.ElementAt(0).Children);
        Assert.Single(parser.Current.Children.ElementAt(0).Children);
        Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
        TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "td", "1", "<td>1</td>");
        var td = parser.Current.Children.ElementAt(0).Children.ElementAt(0).Clone();
        Assert.Empty(td.InheritedStyles);
        Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
        Assert.Equal(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent, parser.Current.Children.ElementAt(0));

        Assert.False(parser.Traverse());
        Assert.Null(parser.Current);

    }
}
