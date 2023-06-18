namespace MariGold.HtmlParser.Tests;

using MariGold.HtmlParser;
using Xunit;

public class TwoElements
{
    [Fact]
    public void OneDivStartWithSpace()
    {
        string html = " <div>test</div>";

        HtmlParser parser = new HtmlTextParser(html);

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            if (parser.Current != null)
            {
                Assert.Equal("#text", parser.Current.Tag);
                Assert.Equal(" ", parser.Current.Html);
                Assert.Equal(" ", parser.Current.InnerHtml);
            }
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            if (parser.Current != null)
            {
                Assert.Equal("div", parser.Current.Tag);
                Assert.Equal("<div>test</div>", parser.Current.Html);
                Assert.Equal("test", parser.Current.InnerHtml);
            }
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        Assert.False(parser.Traverse());
    }

    [Fact]
    public void OneDivSpace()
    {
        string html = "<div>test</div> ";

        HtmlParser parser = new HtmlTextParser(html);

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            if (parser.Current != null)
            {
                Assert.Equal("div", parser.Current.Tag);
                Assert.Equal("<div>test</div>", parser.Current.Html);
                Assert.Equal("test", parser.Current.InnerHtml);
            }
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            if (parser.Current != null)
            {
                Assert.Equal("#text", parser.Current.Tag);
                Assert.Equal(" ", parser.Current.Html);
                Assert.Equal(" ", parser.Current.InnerHtml);
            }
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        Assert.False(parser.Traverse());
    }

    [Fact]
    public void OneDivTextNext()
    {
        string html = "<div>this is a test</div>test";

        HtmlParser parser = new HtmlTextParser(html);

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            if (parser.Current != null)
            {
                Assert.Equal("div", parser.Current.Tag);
                Assert.Equal("<div>this is a test</div>", parser.Current.Html);
                Assert.Equal("this is a test", parser.Current.InnerHtml);
            }
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            if (parser.Current != null)
            {
                Assert.Equal("#text", parser.Current.Tag);
                Assert.Equal("test", parser.Current.Html);
                Assert.Equal("test", parser.Current.InnerHtml);
            }
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        Assert.False(parser.Traverse());

    }

    [Fact]
    public void OneTextNextDiv()
    {
        string html = "test<div>this is a test</div>";

        HtmlParser parser = new HtmlTextParser(html);

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            if (parser.Current != null)
            {
                Assert.Equal("#text", parser.Current.Tag);
                Assert.Equal("test", parser.Current.Html);
                Assert.Equal("test", parser.Current.InnerHtml);
            }
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            if (parser.Current != null)
            {
                Assert.Equal("div", parser.Current.Tag);
                Assert.Equal("<div>this is a test</div>", parser.Current.Html);
                Assert.Equal("this is a test", parser.Current.InnerHtml);
            }
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        Assert.False(parser.Traverse());

    }

    [Fact]
    public void OneTextNextDivWithSpaceOnOpenTag()
    {
        string html = "test< div>this is a test</div>";

        HtmlParser parser = new HtmlTextParser(html);

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            if (parser.Current != null)
            {
                Assert.Equal("#text", parser.Current.Tag);
                Assert.Equal("test< div>this is a test", parser.Current.Html);
                Assert.Equal("test< div>this is a test", parser.Current.InnerHtml);
            }
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        Assert.False(parser.Traverse());
    }

    [Fact]
    public void OneTextNextDivWithSpaceAtTheEndOfOpenTag()
    {
        string html = "test<div >this is a test</div>";

        HtmlParser parser = new HtmlTextParser(html);

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            if (parser.Current != null)
            {
                Assert.Equal("#text", parser.Current.Tag);
                Assert.Equal("test", parser.Current.Html);
                Assert.Equal("test", parser.Current.InnerHtml);
            }
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            if (parser.Current != null)
            {
                Assert.Equal("div", parser.Current.Tag);
                Assert.Equal("<div >this is a test</div>", parser.Current.Html);
                Assert.Equal("this is a test", parser.Current.InnerHtml);
            }
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        Assert.False(parser.Traverse());
    }

    [Fact]
    public void OneTextNextDivWithSpaceAtTheStartOfEndTag()
    {
        string html = "test<div>this is a test< /div>";

        HtmlParser parser = new HtmlTextParser(html);

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            if (parser.Current != null)
            {
                Assert.Equal("#text", parser.Current.Tag);
                Assert.Equal("test", parser.Current.Html);
                Assert.Equal("test", parser.Current.InnerHtml);
            }
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            if (parser.Current != null)
            {
                Assert.Equal("div", parser.Current.Tag);
                Assert.Equal("<div>this is a test< /div>", parser.Current.Html);
                Assert.Equal("this is a test< /div>", parser.Current.InnerHtml);
            }
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        Assert.False(parser.Traverse());
    }

    [Fact]
    public void OneTextNextDivWithSpaceAtTheEndOfEndTag()
    {
        string html = "test<div>this is a test</div >";

        HtmlParser parser = new HtmlTextParser(html);

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            if (parser.Current != null)
            {
                Assert.Equal("#text", parser.Current.Tag);
                Assert.Equal("test", parser.Current.Html);
                Assert.Equal("test", parser.Current.InnerHtml);
            }
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        if (parser.Traverse())
        {
            Assert.NotNull(parser.Current);

            if (parser.Current != null)
            {
                Assert.Equal("div", parser.Current.Tag);
                Assert.Equal("<div>this is a test</div >", parser.Current.Html);
                Assert.Equal("this is a test", parser.Current.InnerHtml);
            }
        }
        else
        {
            Assert.Fail("Fail to traverse");
        }

        Assert.False(parser.Traverse());
    }
}
