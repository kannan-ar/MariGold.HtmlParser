namespace MariGold.HtmlParser.Tests
{
    using System;
    using NUnit.Framework;
    using MariGold.HtmlParser;
    using System.Linq;

    [TestFixture]
    public class BasicStyle
    {
        [Test]
        public void OneInlineStyle()
        {
            string html = "<div style='color:#fff'>test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            if(parser.Traverse())
            {
                parser.ParseCSS();

                Assert.IsNotNull(parser.Current);
                Assert.IsNotNull(parser.Current.Styles);
                Assert.AreEqual(1, parser.Current.Styles.Count);
                TestUtility.CheckStyle(parser.Current.Styles.ElementAt(0), "color", "#fff");
            }
        }

        [Test]
        public void TwoInlineStyle()
        {
            string html = "<div style='color:#fff;font-size:24px'>test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            if (parser.Traverse())
            {
                parser.ParseCSS();

                Assert.IsNotNull(parser.Current);
                Assert.IsNotNull(parser.Current.Styles);
                Assert.AreEqual(2, parser.Current.Styles.Count);
                TestUtility.CheckStyle(parser.Current.Styles.ElementAt(0), "color", "#fff");
                TestUtility.CheckStyle(parser.Current.Styles.ElementAt(1), "font-size", "24px");
            }
        }

        [Test]
        public void BasicIdentityTagStyle()
        {
            string html = "<html><style>#dv{font:verdana,arial;color:#000}</style><div id='dv'>test</div></html>";

            HtmlParser parser = new HtmlTextParser(html);

            if (parser.Traverse())
            {
                parser.ParseCSS();

                Assert.IsNotNull(parser.Current);
                Assert.IsNotNull(parser.Current.Children);
                Assert.AreEqual(2, parser.Current.Children.Count);
                TestUtility.AnalyzeNode(parser.Current.Children[1], "div", "test", "<div id='dv'>test</div>",
                    parser.Current, false, true, 1, 1);

                Assert.IsNotNull(parser.Current.Children[1].Styles);
                Assert.AreEqual(2, parser.Current.Children[1].Styles.Count);
                TestUtility.CheckStyle(parser.Current.Children[1].Styles.ElementAt(0), "font", "verdana,arial");
                TestUtility.CheckStyle(parser.Current.Children[1].Styles.ElementAt(1), "color", "#000");
            }
        }

        [Test]
        public void BasicClassTagStyle()
        {
            string html = "<html><style>.cls{font:verdana,arial;color:#000}</style><div class='cls'>test</div></html>";

            HtmlParser parser = new HtmlTextParser(html);

            if (parser.Traverse())
            {
                parser.ParseCSS();

                Assert.IsNotNull(parser.Current);
                Assert.IsNotNull(parser.Current.Children);
                Assert.AreEqual(2, parser.Current.Children.Count);
                TestUtility.AnalyzeNode(parser.Current.Children[1], "div", "test", "<div class='cls'>test</div>",
                    parser.Current, false, true, 1, 1);

                Assert.IsNotNull(parser.Current.Children[1].Styles);
                Assert.AreEqual(2, parser.Current.Children[1].Styles.Count);
                TestUtility.CheckStyle(parser.Current.Children[1].Styles.ElementAt(0), "font", "verdana,arial");
                TestUtility.CheckStyle(parser.Current.Children[1].Styles.ElementAt(1), "color", "#000");
            }
        }
    }
}
