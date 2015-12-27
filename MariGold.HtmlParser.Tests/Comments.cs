namespace MariGold.HtmlParser.Tests
{
    using System;
	using System.Linq;
    using NUnit.Framework;
    using MariGold.HtmlParser;

    [TestFixture]
    public class Comments
    {
        [Test]
        public void EmptyComment()
        {
            string html = "<!---->";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#comment", "<!---->", "<!---->");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(0, parser.Current.Children.Count());
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);
            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void CommentWithText()
        {
            string html = "<!--test-->";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#comment", "<!--test-->", "<!--test-->");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(0, parser.Current.Children.Count());
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);
            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void CommentWithDiv()
        {
            string html = "<!--<div>test</div>-->";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#comment", html, html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(0, parser.Current.Children.Count());
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);
            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }
    }
}
