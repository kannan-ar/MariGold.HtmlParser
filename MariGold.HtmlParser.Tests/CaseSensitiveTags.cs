namespace MariGold.HtmlParser.Tests
{
    using MariGold.HtmlParser;
    using System.Linq;
    using Xunit;

    public class CaseSensitiveTags
    {
        [Fact]
        public void DivWithUpperCaseClose()
        {
            string html = "<div></DIV>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", html);
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.HasChildren);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Attributes);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void DivWithUpperCaseOpen()
        {
            string html = "<DIV></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "DIV", "", html);
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.HasChildren);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Attributes);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void DivInsidePWithUpperCaseClose()
        {
            string html = "<div><p></P></DIV>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<p></P>", html);
            Assert.Null(parser.Current.Parent);
            Assert.True(parser.Current.HasChildren);
            Assert.Single(parser.Current.Children);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Attributes);

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "", "<p></P>");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children);
            Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.Empty(parser.Current.Attributes);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }
    }
}
