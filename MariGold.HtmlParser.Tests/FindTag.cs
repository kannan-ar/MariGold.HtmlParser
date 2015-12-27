namespace MariGold.HtmlParser.Tests
{
    using System;
	using System.Linq;
    using NUnit.Framework;
    using MariGold.HtmlParser;

    [TestFixture]
    public class FindTag
    {
        [Test]
        public void PInDiv()
        {
            string html = "<div><p>test</p></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.FindFirst("p"));
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "p", "test", "<p>test</p>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);
        }

        [Test]
        public void DivInsideTable()
        {
            string html = "<table><tr><td><div>test</div></td></tr></table>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.FindFirst("div"));
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "test", "<div>test</div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);
        }

        [Test]
        public void PDiv()
        {
            string html = "<p></p><div>test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.FindFirst("div"));
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "test", "<div>test</div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);
        }

        [Test]
        public void InvalidPOpenDiv()
        {
            string html = "<p><div>test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.FindFirst("div"));
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "test", "<div>test</div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);
        }

        [Test]
        public void InvalidPCloseDiv()
        {
            string html = "</p><div>test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.FindFirst("div"));
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "test", "<div>test</div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);
        }
    }
}
