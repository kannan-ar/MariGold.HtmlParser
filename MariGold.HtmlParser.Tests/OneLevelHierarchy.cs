namespace MariGold.HtmlParser.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using MariGold.HtmlParser;

    [TestFixture]
    public class OneLevelHierarchy
    {
        [Test]
        public void DivInsideOneDiv()
        {
            string html = "<div><div>test2</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                TestUtility.AreEqual(parser.Current, "div", "<div>test2</div>", "<div><div>test2</div></div>");

                Assert.IsNull(parser.Current.Parent);

                Assert.IsNotNull(parser.Current.Children);

                Assert.AreEqual(parser.Current.Children.Count(), 1);

                Assert.IsNotNull(parser.Current.Children.ElementAt(0));

                IHtmlNode node = parser.Current.Children.ElementAt(0);

                Assert.IsNotNull(node.Parent);

                TestUtility.AreEqual(node, "div", "test2", "<div>test2</div>");
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            Assert.AreEqual(parser.Traverse(), false);
        }

        [Test]
        public void DivPDivSpan()
        {
            string html = "<div><p>t1</p></div><div><span>t2</span></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(parser.Traverse(), true);

            Assert.IsNotNull(parser.Current);

            TestUtility.AreEqual(parser.Current, "div", "<p>t1</p>", "<div><p>t1</p></div>");

            Assert.IsNull(parser.Current.Parent);

            Assert.IsNotNull(parser.Current.Children);

            Assert.AreEqual(1, parser.Current.Children.Count());

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);

            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "t1", "<p>t1</p>");



            Assert.AreEqual(parser.Traverse(), true);

            Assert.IsNotNull(parser.Current);

            TestUtility.AreEqual(parser.Current, "div", "<span>t2</span>", "<div><span>t2</span></div>");

            Assert.IsNull(parser.Current.Parent);

            Assert.IsNotNull(parser.Current.Children);

            Assert.AreEqual(parser.Current.Children.Count(), 1);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);

            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "span", "t2", "<span>t2</span>");

            Assert.AreEqual(parser.Traverse(), false);

            Assert.IsNull(parser.Current);
        }

        [Test]
        public void EmptyDivPDivSpan()
        {
            string html = "<div><p></p></div><div><span></span></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(parser.Traverse(), true);

            Assert.IsNotNull(parser.Current);

            TestUtility.AreEqual(parser.Current, "div", "<p></p>", "<div><p></p></div>");

            Assert.IsNull(parser.Current.Parent);

            Assert.IsNotNull(parser.Current.Children);

            Assert.AreEqual(1, parser.Current.Children.Count());

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);

            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "", "<p></p>");



            Assert.AreEqual(parser.Traverse(), true);

            Assert.IsNotNull(parser.Current);

            TestUtility.AreEqual(parser.Current, "div", "<span></span>", "<div><span></span></div>");

            Assert.IsNull(parser.Current.Parent);

            Assert.IsNotNull(parser.Current.Children);

            Assert.AreEqual(parser.Current.Children.Count(), 1);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);

            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "span", "", "<span></span>");

            Assert.AreEqual(parser.Traverse(), false);

            Assert.IsNull(parser.Current);
        }
    }
}
