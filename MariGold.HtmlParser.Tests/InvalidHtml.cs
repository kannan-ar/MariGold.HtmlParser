namespace MariGold.HtmlParser.Tests
{
    using System;
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
            parser.ParseCSS();
            Assert.IsNotNull(parser.Current);
            parser.Current.AnalyzeNode("div", "test", html, null, false, true, 1, 1, 1);
        }
    }
}
