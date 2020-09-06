namespace MariGold.HtmlParser.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using MariGold.HtmlParser;

    [TestFixture]
    public class InvalidMultiElements
    {
        [Test]
        public void DivAndInvalidDiv()
        {
            string html = "<div>test1</div><div>test2";

            HtmlParser parser = new HtmlTextParser(html);

            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                TestUtility.AreEqual(parser.Current, "div", "test1", "<div>test1</div>");
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                TestUtility.AreEqual(parser.Current, "div", "test2", "<div>test2");
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            Assert.AreEqual(parser.Traverse(), false);
        }

        [Test]
        public void InvalidDivAndDiv()
        {
            string html = "<div>test2<div>test1</div>";

            HtmlParser parser = new HtmlTextParser(html);

            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                if (parser.Current != null)
                {
                    TestUtility.AreEqual(parser.Current, "div", "test2<div>test1</div>", "<div>test2<div>test1</div>");

                    Assert.IsNull(parser.Current.Parent);

                    Assert.IsNotNull(parser.Current.Children);

                    Assert.AreEqual(2, parser.Current.Children.Count());

                    Assert.IsNotNull(parser.Current.Children.ElementAt(0));

                    Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);

                    TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test2", "test2");


                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            Assert.AreEqual(parser.Traverse(), false);
        }

        [Test]
        public void OpenOnlyDivAndValidDiv()
        {
            string html = "<div><div>test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                TestUtility.AreEqual(parser.Current, "div", "<div>test</div>", "<div><div>test</div>");

                Assert.IsNull(parser.Current.Parent);

                Assert.IsNotNull(parser.Current.Children);

                Assert.AreEqual(parser.Current.Children.Count(), 1);

                Assert.IsNotNull(parser.Current.Children.ElementAt(0));

                IHtmlNode node = parser.Current.Children.ElementAt(0);

                Assert.IsNotNull(node.Parent);

                TestUtility.AreEqual(node, "div", "test", "<div>test</div>");
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            Assert.AreEqual(parser.Traverse(), false);
        }

        [Test]
        public void ValidDivAndOpenOnlyDivAndP()
        {
            string html = "<div>test</div><div><p>t1</p>";

            HtmlParser parser = new HtmlTextParser(html);

            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                TestUtility.AreEqual(parser.Current, "div", "test", "<div>test</div>");
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                TestUtility.AreEqual(parser.Current, "div", "<p>t1</p>", "<div><p>t1</p>");

                Assert.IsNull(parser.Current.Parent);

                Assert.IsNotNull(parser.Current.Children);

                Assert.AreEqual(parser.Current.Children.Count(), 1);

                Assert.IsNotNull(parser.Current.Children.ElementAt(0));

                IHtmlNode node = parser.Current.Children.ElementAt(0);

                Assert.IsNotNull(node.Parent);

                TestUtility.AreEqual(node, "p", "t1", "<p>t1</p>");
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            Assert.AreEqual(parser.Traverse(), false);
        }
    }
}
