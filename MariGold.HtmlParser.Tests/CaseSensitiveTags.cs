namespace MariGold.HtmlParser.Tests
{
    using System;
    using NUnit.Framework;
    using MariGold.HtmlParser;

    [TestFixture]
    class CaseSensitiveTags
    {
        [Test]
        public void DivWithUpperCaseClose()
        {
            string html = "<div></DIV>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.AreEqual(parser.Traverse(), false);
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void DivWithUpperCaseOpen()
        {
            string html = "<DIV></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "DIV", "", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.AreEqual(parser.Traverse(), false);
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void DivInsidePWithUpperCaseClose()
        {
            string html = "<div><p></P></DIV>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<p></P>", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "p", "", "<p></P>");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.AreEqual(parser.Traverse(), false);
            Assert.IsNull(parser.Current);
        }
    }
}
