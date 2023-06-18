namespace MariGold.HtmlParser.Tests;

using MariGold.HtmlParser;
using System.Linq;
using Xunit;

public class OneLevelHierarchy
{
    [Fact]
    public void DivInsideOneDiv()
    {
        string html = "<div><div>test2</div></div>";

        HtmlParser parser = new HtmlTextParser(html);

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            TestUtility.AreEqual(parser.Current, "div", "<div>test2</div>", "<div><div>test2</div></div>");

            Assert.Null(parser.Current.Parent);

            Assert.NotNull(parser.Current.Children);

            Assert.Single(parser.Current.Children);

            Assert.NotNull(parser.Current.Children.ElementAt(0));

            IHtmlNode node = parser.Current.Children.ElementAt(0);

            Assert.NotNull(node.Parent);

            TestUtility.AreEqual(node, "div", "test2", "<div>test2</div>");
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        Assert.False(parser.Traverse());
    }

    [Fact]
    public void DivPDivSpan()
    {
        string html = "<div><p>t1</p></div><div><span>t2</span></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Traverse());

        Assert.NotNull(parser.Current);

        TestUtility.AreEqual(parser.Current, "div", "<p>t1</p>", "<div><p>t1</p></div>");

        Assert.Null(parser.Current.Parent);

        Assert.NotNull(parser.Current.Children);

        Assert.Single(parser.Current.Children);

        Assert.NotNull(parser.Current.Children.ElementAt(0));

        Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);

        TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "t1", "<p>t1</p>");



        Assert.True(parser.Traverse());

        Assert.NotNull(parser.Current);

        TestUtility.AreEqual(parser.Current, "div", "<span>t2</span>", "<div><span>t2</span></div>");

        Assert.Null(parser.Current.Parent);

        Assert.NotNull(parser.Current.Children);

        Assert.Single(parser.Current.Children);

        Assert.NotNull(parser.Current.Children.ElementAt(0));

        Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);

        TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "span", "t2", "<span>t2</span>");

        Assert.False(parser.Traverse());

        Assert.Null(parser.Current);
    }

    [Fact]
    public void EmptyDivPDivSpan()
    {
        string html = "<div><p></p></div><div><span></span></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Traverse());

        Assert.NotNull(parser.Current);

        TestUtility.AreEqual(parser.Current, "div", "<p></p>", "<div><p></p></div>");

        Assert.Null(parser.Current.Parent);

        Assert.NotNull(parser.Current.Children);

        Assert.Single(parser.Current.Children);

        Assert.NotNull(parser.Current.Children.ElementAt(0));

        Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);

        TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "", "<p></p>");



        Assert.True(parser.Traverse());

        Assert.NotNull(parser.Current);

        TestUtility.AreEqual(parser.Current, "div", "<span></span>", "<div><span></span></div>");

        Assert.Null(parser.Current.Parent);

        Assert.NotNull(parser.Current.Children);

        Assert.Single(parser.Current.Children);

        Assert.NotNull(parser.Current.Children.ElementAt(0));

        Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);

        TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "span", "", "<span></span>");

        Assert.False(parser.Traverse());

        Assert.Null(parser.Current);
    }
}
