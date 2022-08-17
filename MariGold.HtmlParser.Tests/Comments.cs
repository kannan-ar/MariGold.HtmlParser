namespace MariGold.HtmlParser.Tests
{
    using MariGold.HtmlParser;
    using System.Linq;
    using Xunit;

    public class Comments
    {
        [Fact]
        public void EmptyComment()
        {
            string html = "<!---->";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#comment", "<!---->", "<!---->");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.HasChildren);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Attributes);
            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void CommentWithText()
        {
            string html = "<!--test-->";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#comment", "<!--test-->", "<!--test-->");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.HasChildren);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Attributes);
            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void CommentWithDiv()
        {
            string html = "<!--<div>test</div>-->";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#comment", html, html);
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.HasChildren);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Attributes);
            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void CommentNextDiv()
        {
            string html = "<!--test--><div>test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#comment", "<!--test-->", "<!--test-->");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.HasChildren);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Attributes);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AnalyzeNode(parser.Current, "div", "test", "<div>test</div>", null, false, true, 1, 0);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }
    }
}
