namespace MariGold.HtmlParser.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using MariGold.HtmlParser;

    [TestFixture]
    public class InvalidHtml
    {
        [Test]
        public void InvalidInputAttribute()
        {
            string html = "<input =\"\" name=\"fld_quicksign\">";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Parse());
            Assert.IsNotNull(parser.Current);
            Assert.AreEqual(1, parser.Current.Attributes.Count);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "name", "fld_quicksign");
        }

        [Test]
        public void InvalidQuote()
        {
            string html = "<div style=\"width:100%\" \">test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Parse());
            parser.ParseStyles();
            Assert.IsNotNull(parser.Current);
            parser.Current.AnalyzeNode("div", "test", html, null, false, true, 1, 1, 1);
        }

        [Test]
        public void InputWithEmptyValue()
        {
            string html = "<div><input value=\"\" /><div style=\"width:100%\">1</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Parse());
            parser.ParseStyles();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            node.AnalyzeNode("div", "<input value=\"\" /><div style=\"width:100%\">1</div>", html, null, false, true, 2, 0, 0);
            IHtmlNode parent = node;

            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            node.AnalyzeNode("input", "<input value=\"\" />", "<input value=\"\" />", parent, true, false, 0, 1, 0);

            node = parent.Children.ElementAt(1);
            Assert.IsNotNull(node);
            node.AnalyzeNode("div", "1", "<div style=\"width:100%\">1</div>", parent, false, true, 1, 1, 1);
        }

        [Test]
        public void InputWithEmptyQuote()
        {
            string html = "<input \"\" />";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Parse());
            Assert.IsNotNull(parser.Current);
            parser.Current.AnalyzeNode("input", html, html, null, true, false, 0, 0, 0);
        }
    }
}
