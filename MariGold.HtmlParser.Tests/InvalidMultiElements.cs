namespace MariGold.HtmlParser.Tests;

using MariGold.HtmlParser;
using System.Linq;
using Xunit;

public class InvalidMultiElements
{
    [Fact]
    public void DivAndInvalidDiv()
    {
        string html = "<div>test1</div><div>test2";

        HtmlParser parser = new HtmlTextParser(html);

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            TestUtility.AreEqual(parser.Current, "div", "test1", "<div>test1</div>");
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            TestUtility.AreEqual(parser.Current, "div", "test2", "<div>test2");
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        Assert.False(parser.Traverse());
    }

    [Fact]
    public void InvalidDivAndDiv()
    {
        string html = "<div>test2<div>test1</div>";

        HtmlParser parser = new HtmlTextParser(html);

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            if (parser.Current != null)
            {
                TestUtility.AreEqual(parser.Current, "div", "test2<div>test1</div>", "<div>test2<div>test1</div>");

                Assert.Null(parser.Current.Parent);

                Assert.NotNull(parser.Current.Children);

                Assert.Equal(2, parser.Current.Children.Count());

                Assert.NotNull(parser.Current.Children.ElementAt(0));

                Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);

                TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test2", "test2");


            }
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        Assert.False(parser.Traverse());
    }

    [Fact]
    public void OpenOnlyDivAndValidDiv()
    {
        string html = "<div><div>test</div>";

        HtmlParser parser = new HtmlTextParser(html);

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            TestUtility.AreEqual(parser.Current, "div", "<div>test</div>", "<div><div>test</div>");

            Assert.Null(parser.Current.Parent);

            Assert.NotNull(parser.Current.Children);

            Assert.Single(parser.Current.Children);

            Assert.NotNull(parser.Current.Children.ElementAt(0));

            IHtmlNode node = parser.Current.Children.ElementAt(0);

            Assert.NotNull(node.Parent);

            TestUtility.AreEqual(node, "div", "test", "<div>test</div>");
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        Assert.False(parser.Traverse());
    }

    [Fact]
    public void ValidDivAndOpenOnlyDivAndP()
    {
        string html = "<div>test</div><div><p>t1</p>";

        HtmlParser parser = new HtmlTextParser(html);

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            TestUtility.AreEqual(parser.Current, "div", "test", "<div>test</div>");
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            TestUtility.AreEqual(parser.Current, "div", "<p>t1</p>", "<div><p>t1</p>");

            Assert.Null(parser.Current.Parent);

            Assert.NotNull(parser.Current.Children);

            Assert.Single(parser.Current.Children);

            Assert.NotNull(parser.Current.Children.ElementAt(0));

            IHtmlNode node = parser.Current.Children.ElementAt(0);

            Assert.NotNull(node.Parent);

            TestUtility.AreEqual(node, "p", "t1", "<p>t1</p>");
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        Assert.False(parser.Traverse());
    }
}
