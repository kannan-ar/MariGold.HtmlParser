namespace MariGold.HtmlParser.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using MariGold.HtmlParser;

    [TestFixture]
    public class Tables
    {
        [Test]
        public void SimpleTable()
        {
            string html = "<table><tr><td>1</td></tr></table>";

            HtmlTextParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "table", "<tr><td>1</td></tr>", "<table><tr><td>1</td></tr></table>");
            Assert.IsNull(parser.Current.Parent);

            Assert.IsNotNull(parser.Current.Children);
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "tr", "<td>1</td>", "<tr><td>1</td></tr>");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current.Children.ElementAt(0).Parent, parser.Current);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children);
            Assert.AreEqual(1, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "td", "1", "<td>1</td>");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent, parser.Current.Children.ElementAt(0));

            Assert.AreEqual(parser.Traverse(), false);
            Assert.IsNull(parser.Current);

        }

        [Test]
        public void TableWithOneRowAndTwoColumn()
        {
            string html = "<table><tr><td>1</td><td></td></tr></table>";

            HtmlTextParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "table", "<tr><td>1</td><td></td></tr>", "<table><tr><td>1</td><td></td></tr></table>");
            Assert.IsNull(parser.Current.Parent);

            Assert.IsNotNull(parser.Current.Children);
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "tr", "<td>1</td><td></td>", "<tr><td>1</td><td></td></tr>");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current.Children.ElementAt(0).Parent, parser.Current);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children);
            Assert.AreEqual(2, parser.Current.Children.ElementAt(0).Children.Count());

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "td", "1", "<td>1</td>");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent, parser.Current.Children.ElementAt(0));

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(1), "td", "", "<td></td>");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(1).Parent);
            Assert.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(1).Parent, parser.Current.Children.ElementAt(0));

            Assert.AreEqual(parser.Traverse(), false);
            Assert.IsNull(parser.Current);

        }

        [Test]
        public void TwoRowWithOneColumnEach()
        {
            string html = "<table><tr><td>test1</td></tr><tr><td>test2</td></tr></table>";

            HtmlTextParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());

            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "table", "<tr><td>test1</td></tr><tr><td>test2</td></tr>", html);
            Assert.IsNull(parser.Current.Parent);

            Assert.IsNotNull(parser.Current.Children);
            Assert.AreEqual(2, parser.Current.Children.Count());

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "tr", "<td>test1</td>", "<tr><td>test1</td></tr>");
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children);
            Assert.AreEqual(1, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "td", "test1", "<td>test1</td>");
            Assert.AreEqual(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);

            Assert.IsNotNull(parser.Current.Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "tr", "<td>test2</td>", "<tr><td>test2</td></tr>");
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(1).Parent);

            Assert.IsNotNull(parser.Current.Children.ElementAt(1).Children);
            Assert.AreEqual(1, parser.Current.Children.ElementAt(1).Children.Count());
            Assert.IsNotNull(parser.Current.Children.ElementAt(1).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1).Children.ElementAt(0), "td", "test2", "<td>test2</td>");
            Assert.AreEqual(parser.Current.Children.ElementAt(1), parser.Current.Children.ElementAt(1).Children.ElementAt(0).Parent);
        }

        [Test]
        public void OneCellWithSpanText()
        {
            string html = "<table><tr><td><span>s1</span>test</td></tr></table>";


            HtmlTextParser parser = new HtmlTextParser(html);

            Assert.AreEqual(parser.Traverse(), true);
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "table", "<tr><td><span>s1</span>test</td></tr>", html);

            Assert.IsNotNull(parser.Current.Children);
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "tr", "<td><span>s1</span>test</td>",
                "<tr><td><span>s1</span>test</td></tr>");
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children);
            Assert.AreEqual(1, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "td",
                "<span>s1</span>test",
                "<td><span>s1</span>test</td>");
            Assert.AreEqual(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(2, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.Count());
        }
    }
}
