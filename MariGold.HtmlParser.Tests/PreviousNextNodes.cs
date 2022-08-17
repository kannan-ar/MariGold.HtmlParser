namespace MariGold.HtmlParser.Tests
{
    using MariGold.HtmlParser;
    using System.Linq;
    using Xunit;

    public class PreviousNextNodes
    {
        [Fact]
        public void SingleNode()
        {
            string html = "<div>test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "test", html);
            Assert.Null(parser.Current.Previous);
            Assert.Null(parser.Current.Next);
            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void DivA()
        {
            string html = "<div>test</div><a>ano</a>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "test", "<div>test</div>");
            Assert.Null(parser.Current.Previous);
            Assert.Null(parser.Current.Next);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "a", "ano", "<a>ano</a>");
            Assert.NotNull(parser.Current.Previous);
            Assert.Null(parser.Current.Next);
            TestUtility.AreEqual(parser.Current.Previous, "div", "test", "<div>test</div>");
            Assert.NotNull(parser.Current.Previous.Next);
            TestUtility.AreEqual(parser.Current.Previous.Next, "a", "ano", "<a>ano</a>");
        }

        [Fact]
        public void DivInsideA()
        {
            string html = "<div><a>ano</a></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<a>ano</a>", "<div><a>ano</a></div>");
            Assert.Null(parser.Current.Previous);
            Assert.Null(parser.Current.Next);

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "a", "ano", "<a>ano</a>");
            Assert.Null(parser.Current.Previous);
            Assert.Null(parser.Current.Next);
        }

        [Fact]
        public void DivInsideTextA()
        {
            string html = "<div>test<a>ano</a></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "test<a>ano</a>", "<div>test<a>ano</a></div>");
            Assert.Null(parser.Current.Previous);
            Assert.Null(parser.Current.Next);
            Assert.Equal(2, parser.Current.Children.Count());

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
            Assert.Null(parser.Current.Children.ElementAt(0).Previous);
            Assert.NotNull(parser.Current.Children.ElementAt(0).Next);
            Assert.Equal(parser.Current.Children.ElementAt(0).Next, parser.Current.Children.ElementAt(1));

            Assert.NotNull(parser.Current.Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "a", "ano", "<a>ano</a>");
            Assert.Null(parser.Current.Children.ElementAt(1).Next);
            Assert.NotNull(parser.Current.Children.ElementAt(1).Previous);
            Assert.Equal(parser.Current.Children.ElementAt(1).Previous, parser.Current.Children.ElementAt(0));
        }

        [Fact]
        public void DivInsideTextAP()
        {
            string html = "<div>test<a>ano</a></div><p>tt</p>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "test<a>ano</a>", "<div>test<a>ano</a></div>");
            Assert.Null(parser.Current.Previous);
            Assert.Null(parser.Current.Next);
            Assert.Equal(2, parser.Current.Children.Count());

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
            Assert.Null(parser.Current.Children.ElementAt(0).Previous);
            Assert.NotNull(parser.Current.Children.ElementAt(0).Next);
            Assert.Equal(parser.Current.Children.ElementAt(0).Next, parser.Current.Children.ElementAt(1));

            Assert.NotNull(parser.Current.Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "a", "ano", "<a>ano</a>");
            Assert.Null(parser.Current.Children.ElementAt(1).Next);
            Assert.NotNull(parser.Current.Children.ElementAt(1).Previous);
            Assert.Equal(parser.Current.Children.ElementAt(1).Previous, parser.Current.Children.ElementAt(0));

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "p", "tt", "<p>tt</p>");
            Assert.Single(parser.Current.Children);
            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "tt", "tt");
            Assert.Null(parser.Current.Children.ElementAt(0).Previous);
            Assert.Null(parser.Current.Children.ElementAt(0).Next);

            Assert.NotNull(parser.Current.Previous);
            Assert.Null(parser.Current.Next);
            TestUtility.AreEqual(parser.Current.Previous, "div", "test<a>ano</a>", "<div>test<a>ano</a></div>");
            Assert.NotNull(parser.Current.Previous.Next);
            TestUtility.AreEqual(parser.Current.Previous.Next, "p", "tt", "<p>tt</p>");
        }

        [Fact]
        public void TextDiv()
        {
            string html = "test<div>ano</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#text", "test", "test");
            Assert.Null(parser.Current.Previous);
            Assert.Null(parser.Current.Next);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "ano", "<div>ano</div>");
            Assert.NotNull(parser.Current.Previous);
            Assert.Null(parser.Current.Next);

            TestUtility.AreEqual(parser.Current.Previous, "#text", "test", "test");
            Assert.NotNull(parser.Current.Previous.Next);
            TestUtility.AreEqual(parser.Current.Previous.Next, "div", "ano", "<div>ano</div>");
            Assert.Null(parser.Current.Previous.Previous);
        }

        [Fact]
        public void DivText()
        {
            string html = "<div>ano</div>test";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "ano", "<div>ano</div>");
            Assert.Null(parser.Current.Previous);
            Assert.Null(parser.Current.Next);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#text", "test", "test");
            Assert.NotNull(parser.Current.Previous);
            Assert.Null(parser.Current.Next);

            TestUtility.AreEqual(parser.Current.Previous, "div", "ano", "<div>ano</div>");
            Assert.NotNull(parser.Current.Previous.Next);
            TestUtility.AreEqual(parser.Current.Previous.Next, "#text", "test", "test");
            Assert.Null(parser.Current.Previous.Previous);

            Assert.Equal(parser.Current, parser.Current.Previous.Next);
            Assert.Equal(parser.Current.Previous, parser.Current.Previous.Next.Previous);
        }

        [Fact]
        public void BrText()
        {
            string html = "<br />test";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br />", "<br />");
            Assert.True(parser.Current.SelfClosing);
            Assert.Null(parser.Current.Previous);
            Assert.Null(parser.Current.Next);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#text", "test", "test");
            Assert.NotNull(parser.Current.Previous);
            Assert.Null(parser.Current.Next);

            TestUtility.AreEqual(parser.Current.Previous, "br", "<br />", "<br />");
            TestUtility.AreEqual(parser.Current.Previous.Next, "#text", "test", "test");
            Assert.Equal(parser.Current, parser.Current.Previous.Next);
            Assert.Equal(parser.Current.Previous, parser.Current.Previous.Next.Previous);
        }

        [Fact]
        public void SimpleTable()
        {
            string html = "<table><tr><td>one</td><td>two</td></tr></table>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "table", "<tr><td>one</td><td>two</td></tr>", html);
            Assert.Null(parser.Current.Next);
            Assert.Null(parser.Current.Previous);

            Assert.Single(parser.Current.Children);
            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "tr", "<td>one</td><td>two</td>",
                "<tr><td>one</td><td>two</td></tr>");
            Assert.Null(parser.Current.Children.ElementAt(0).Next);
            Assert.Null(parser.Current.Children.ElementAt(0).Previous);

            Assert.Equal(2, parser.Current.Children.ElementAt(0).Children.Count());

            Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "td", "one", "<td>one</td>");
            Assert.Null(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Previous);
            Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Next);

            Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(1), "td", "two", "<td>two</td>");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(1).Previous);
            Assert.Null(parser.Current.Children.ElementAt(0).Children.ElementAt(1).Next);

            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Next, "td", "two", "<td>two</td>");
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(1).Previous, "td", "one", "<td>one</td>");
            Assert.Equal(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Next,
                parser.Current.Children.ElementAt(0).Children.ElementAt(1));
            Assert.Equal(parser.Current.Children.ElementAt(0).Children.ElementAt(1).Previous,
                parser.Current.Children.ElementAt(0).Children.ElementAt(0));
        }

        [Fact]
        public void DivInsidePA()
        {
            string html = "<div><p>1</p><a>2</a></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<p>1</p><a>2</a>", html);
            Assert.Null(parser.Current.Previous);
            Assert.Null(parser.Current.Next);
            Assert.Equal(2, parser.Current.Children.Count());

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "1", "<p>1</p>");
            Assert.Null(parser.Current.Children.ElementAt(0).Previous);
            Assert.NotNull(parser.Current.Children.ElementAt(0).Next);

            Assert.NotNull(parser.Current.Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "a", "2", "<a>2</a>");
            Assert.NotNull(parser.Current.Children.ElementAt(1).Previous);
            Assert.Null(parser.Current.Children.ElementAt(1).Next);

            TestUtility.AreEqual(parser.Current.Children.ElementAt(1).Previous, "p", "1", "<p>1</p>");
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Next, "a", "2", "<a>2</a>");
            Assert.Equal(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(1).Previous);
            Assert.Equal(parser.Current.Children.ElementAt(1), parser.Current.Children.ElementAt(0).Next);
        }

        [Fact]
        public void DivInsidePBr()
        {
            string html = "<div><p>1</p><br /></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<p>1</p><br />", html);
            Assert.Null(parser.Current.Previous);
            Assert.Null(parser.Current.Next);
            Assert.Equal(2, parser.Current.Children.Count());

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "1", "<p>1</p>");
            Assert.Null(parser.Current.Children.ElementAt(0).Previous);
            Assert.NotNull(parser.Current.Children.ElementAt(0).Next);

            Assert.NotNull(parser.Current.Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "br", "<br />", "<br />");
            Assert.True(parser.Current.Children.ElementAt(1).SelfClosing);
            Assert.NotNull(parser.Current.Children.ElementAt(1).Previous);
            Assert.Null(parser.Current.Children.ElementAt(1).Next);

            TestUtility.AreEqual(parser.Current.Children.ElementAt(1).Previous, "p", "1", "<p>1</p>");
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Next, "br", "<br />", "<br />");
            Assert.Equal(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(1).Previous);
            Assert.Equal(parser.Current.Children.ElementAt(1), parser.Current.Children.ElementAt(0).Next);
        }

        [Fact]
        public void DivInsideBrP()
        {
            string html = "<div><br /><p>1</p></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<br /><p>1</p>", html);
            Assert.Null(parser.Current.Previous);
            Assert.Null(parser.Current.Next);
            Assert.Equal(2, parser.Current.Children.Count());

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "br", "<br />", "<br />");
            Assert.True(parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.Null(parser.Current.Children.ElementAt(0).Previous);
            Assert.NotNull(parser.Current.Children.ElementAt(0).Next);

            Assert.NotNull(parser.Current.Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "p", "1", "<p>1</p>");
            Assert.NotNull(parser.Current.Children.ElementAt(1).Previous);
            Assert.Null(parser.Current.Children.ElementAt(1).Next);

            TestUtility.AreEqual(parser.Current.Children.ElementAt(1).Previous, "br", "<br />", "<br />");
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Next, "p", "1", "<p>1</p>");
            Assert.Equal(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(1).Previous);
            Assert.Equal(parser.Current.Children.ElementAt(1), parser.Current.Children.ElementAt(0).Next);
        }

        [Fact]
        public void HtmlHeadBody()
        {
            string html = "<html><head></head><body></body></html>";

            HtmlParser parser = new HtmlTextParser(html);

            parser.Parse();

            IHtmlNode node = parser.Current;

            node.AnalyzeNode("html", "<head></head><body></body>", "<html><head></head><body></body></html>", null, false, true, 2, 0, 0);
            Assert.Null(node.Previous);
            Assert.Null(node.Next);

            IHtmlNode hnode = node;

            node = node.Children.ElementAt(0);

            node.AnalyzeNode("head", "", "<head></head>", hnode, false, false, 0, 0, 0);
            Assert.Null(node.Previous);
            Assert.NotNull(node.Next);

            node = node.Next;
            node.AnalyzeNode("body", "", "<body></body>", hnode, false, false, 0, 0, 0);
            Assert.NotNull(node.Previous);
            Assert.Null(node.Next);

        }
    }
}
