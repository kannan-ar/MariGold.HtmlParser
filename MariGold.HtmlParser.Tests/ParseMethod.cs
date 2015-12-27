namespace MariGold.HtmlParser.Tests
{
    using System;
	using System.Linq;
    using NUnit.Framework;
    using MariGold.HtmlParser;

    [TestFixture]
    public class ParseMethod
    {
        [Test]
        public void EmptyHtml()
        {
            string html = " ";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#text", html, html);
            Assert.IsNull(parser.Current.Previous);
            Assert.IsNull(parser.Current.Next);
        }

        [Test]
        public void SingleNode()
        {
            string html = "<div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", html);
            Assert.IsNull(parser.Current.Previous);
            Assert.IsNull(parser.Current.Next);

            Assert.IsFalse(parser.Parse());
        }

        [Test]
        public void DivAndP()
        {
            string html = "<div>test</div><p>one</p>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "test", "<div>test</div>");

            Assert.IsNull(parser.Current.Previous);
            Assert.IsNotNull(parser.Current.Next);
            TestUtility.AreEqual(parser.Current.Next, "p", "one", "<p>one</p>");
            Assert.IsNotNull(parser.Current.Next.Previous);
            Assert.AreEqual(parser.Current, parser.Current.Next.Previous);
            Assert.IsNull(parser.Current.Next.Next);

            Assert.IsFalse(parser.Parse());
        }

        [Test]
        public void DivInsidePAndB()
        {
            string html = "<div><p>one</p></div><b></b>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<p>one</p>", "<div><p>one</p></div>");
            Assert.IsTrue(parser.Current.HasChildren);
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "one", "<p>one</p>");
            Assert.IsNull(parser.Current.Children.ElementAt(0).Previous);
            Assert.IsNull(parser.Current.Children.ElementAt(0).Next);
            Assert.IsNull(parser.Current.Previous);
            Assert.IsNotNull(parser.Current.Next);
            TestUtility.AreEqual(parser.Current.Next, "b", "", "<b></b>");

            Assert.IsNull(parser.Current.Next.Next);
            Assert.IsNotNull(parser.Current.Next.Previous);
            Assert.AreEqual(parser.Current, parser.Current.Next.Previous);

            Assert.IsFalse(parser.Parse());
        }

        [Test]
        public void DivPB()
        {
            string html = "<div></div><p></p><b></b>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div></div>");
            Assert.IsNull(parser.Current.Previous);
            Assert.IsNotNull(parser.Current.Next);

            TestUtility.AreEqual(parser.Current.Next, "p", "", "<p></p>");
            Assert.IsNotNull(parser.Current.Next.Previous);
            Assert.IsNotNull(parser.Current.Next.Next);
            Assert.AreEqual(parser.Current, parser.Current.Next.Previous);

            Assert.IsNotNull(parser.Current.Next.Next);
            TestUtility.AreEqual(parser.Current.Next.Next, "b", "", "<b></b>");
            Assert.IsNull(parser.Current.Next.Next.Next);
            Assert.IsNotNull(parser.Current.Next.Next.Previous);
            Assert.AreEqual(parser.Current.Next, parser.Current.Next.Next.Previous);

            Assert.AreEqual(parser.Current, parser.Current.Next.Next.Previous.Previous);

            Assert.IsFalse(parser.Parse());
        }
    }
}
