namespace MariGold.HtmlParser.Tests
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using MariGold.HtmlParser;
    using System.Linq;

    [TestFixture]
    public class FontStyle
    {
        [Test]
        public void FontSizeOnly()
        {
            string html = "<div style='font:25px'><div style='font-family:verdana'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-family:verdana'>test</div>", html, null, false, true, 1, 1, 1);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font", "25px");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-family:verdana'>test</div>", parent, false, true, 1, 1, 1);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-family", "verdana");
        }

        [Test]
        public void FontSizeFontFamily()
        {
            string html = "<div style='font:25px arial'><div style='font-family:verdana'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-family:verdana'>test</div>", html, null, false, true, 1, 1, 2);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-size", "25px");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-family", "arial");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-family:verdana'>test</div>", parent, false, true, 1, 1, 2);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-family", "verdana");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-size", "25px");
        }

        [Test]
        public void FontSizeLineHeightFontFamily()
        {
            string html = "<div style='font:25px/2pt arial'><div style='font-family:verdana'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-family:verdana'>test</div>", html, null, false, true, 1, 1, 3);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-size", "25px");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "line-height", "2pt");
            TestUtility.CheckStyle(node.Styles.ElementAt(2), "font-family", "arial");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-family:verdana'>test</div>", parent, false, true, 1, 1, 3);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-family", "verdana");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-size", "25px");
            TestUtility.CheckStyle(node.Styles.ElementAt(2), "line-height", "2pt");
        }

        [Test]
        public void ParentFont()
        {
            string html = "<div style='font:arial'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 1);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font", "arial");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-size", "10px");
        }

        [Test]
        public void ChildFont()
        {
            string html = "<div style='font:8pt arial'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 2);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-size", "8pt");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-family", "arial");


            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 2);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-size", "10px");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-family", "arial");
        }

        [Test]
        public void FontWeightNormal()
        {
            string html = "<div style='font:normal 8pt arial'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 3);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-weight", "normal");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-size", "8pt");
            TestUtility.CheckStyle(node.Styles.ElementAt(2), "font-family", "arial");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 3);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-size", "10px");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-weight", "normal");
            TestUtility.CheckStyle(node.Styles.ElementAt(2), "font-family", "arial");
        }

        [Test]
        public void FontVariantFontWeightNormal()
        {
            string html = "<div style='font:normal inherit 1em arial'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 4);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-weight", "inherit");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-variant", "normal");
            TestUtility.CheckStyle(node.Styles.ElementAt(2), "font-size", "1em");
            TestUtility.CheckStyle(node.Styles.ElementAt(3), "font-family", "arial");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 4);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-size", "10px");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-weight", "inherit");
            TestUtility.CheckStyle(node.Styles.ElementAt(2), "font-variant", "normal");
            TestUtility.CheckStyle(node.Styles.ElementAt(3), "font-family", "arial");
        }

        [Test]
        public void FontStyleFontVariantFontWeightNormal()
        {
            string html = "<div style='font:initial normal inherit 1em arial'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 5);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-weight", "inherit");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-variant", "normal");
            TestUtility.CheckStyle(node.Styles.ElementAt(2), "font-style", "initial");
            TestUtility.CheckStyle(node.Styles.ElementAt(3), "font-size", "1em");
            TestUtility.CheckStyle(node.Styles.ElementAt(4), "font-family", "arial");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 5);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-size", "10px");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-weight", "inherit");
            TestUtility.CheckStyle(node.Styles.ElementAt(2), "font-variant", "normal");
            TestUtility.CheckStyle(node.Styles.ElementAt(3), "font-style", "initial");
            TestUtility.CheckStyle(node.Styles.ElementAt(4), "font-family", "arial");
        }

        [Test]
        public void FontStyleFontVariantFontWeightNormalFontSizeLineHeight()
        {
            string html = "<div style='font:initial normal inherit 1em/2px arial'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 6);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-weight", "inherit");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-variant", "normal");
            TestUtility.CheckStyle(node.Styles.ElementAt(2), "font-style", "initial");
            TestUtility.CheckStyle(node.Styles.ElementAt(3), "font-size", "1em");
            TestUtility.CheckStyle(node.Styles.ElementAt(4), "line-height", "2px");
            TestUtility.CheckStyle(node.Styles.ElementAt(5), "font-family", "arial");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 6);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-size", "10px");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-weight", "inherit");
            TestUtility.CheckStyle(node.Styles.ElementAt(2), "font-variant", "normal");
            TestUtility.CheckStyle(node.Styles.ElementAt(3), "font-style", "initial");
            TestUtility.CheckStyle(node.Styles.ElementAt(4), "line-height", "2px");
            TestUtility.CheckStyle(node.Styles.ElementAt(5), "font-family", "arial");
        }

        [Test]
        public void FontStyleFontVariantFontWeightNormalFontSizeLineHeightCaption()
        {
            string html = "<div style='font:initial normal inherit 1em/2px arial  caption'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 6);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-weight", "inherit");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-variant", "normal");
            TestUtility.CheckStyle(node.Styles.ElementAt(2), "font-style", "initial");
            TestUtility.CheckStyle(node.Styles.ElementAt(3), "font-size", "1em");
            TestUtility.CheckStyle(node.Styles.ElementAt(4), "line-height", "2px");
            TestUtility.CheckStyle(node.Styles.ElementAt(5), "font-family", "arial");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 6);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-size", "10px");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-weight", "inherit");
            TestUtility.CheckStyle(node.Styles.ElementAt(2), "font-variant", "normal");
            TestUtility.CheckStyle(node.Styles.ElementAt(3), "font-style", "initial");
            TestUtility.CheckStyle(node.Styles.ElementAt(4), "line-height", "2px");
            TestUtility.CheckStyle(node.Styles.ElementAt(5), "font-family", "arial");
        }

        [Test]
        public void FontSizeLineHeightFontFamilyCaption()
        {
            string html = "<div style='font:8pt/2px verdana caption'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 3);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-size", "8pt");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "line-height", "2px");
            TestUtility.CheckStyle(node.Styles.ElementAt(2), "font-family", "verdana");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 3);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-size", "10px");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "line-height", "2px");
            TestUtility.CheckStyle(node.Styles.ElementAt(2), "font-family", "verdana");
        }

        [Test]
        public void FontWeightFontSizeFontFamilyMessageBox()
        {
            string html = "<div style='font:bold 8pt verdana message-box'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 3);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-weight", "bold");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-size", "8pt");
            TestUtility.CheckStyle(node.Styles.ElementAt(2), "font-family", "verdana");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 3);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-size", "10px");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-weight", "bold");
            TestUtility.CheckStyle(node.Styles.ElementAt(2), "font-family", "verdana");
        }

        [Test]
        public void FontVariantFontWeightFontSizeFontFamilyMessageBox()
        {
            string html = "<div style='font:small-caps  bold 8pt  verdana  message-box'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 4);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-variant", "small-caps");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-weight", "bold");
            TestUtility.CheckStyle(node.Styles.ElementAt(2), "font-size", "8pt");
            TestUtility.CheckStyle(node.Styles.ElementAt(3), "font-family", "verdana");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 4);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-size", "10px");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-variant", "small-caps");
            TestUtility.CheckStyle(node.Styles.ElementAt(2), "font-weight", "bold");
            TestUtility.CheckStyle(node.Styles.ElementAt(3), "font-family", "verdana");
        }

        [Test]
        public void FontStyleFontVariantFontWeightFontSizeFontFamilyMessageBox()
        {
            string html = "<div style='font: oblique small-caps  bold 8pt  verdana  message-box'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 5);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-style", "oblique");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-variant", "small-caps");
            TestUtility.CheckStyle(node.Styles.ElementAt(2), "font-weight", "bold");
            TestUtility.CheckStyle(node.Styles.ElementAt(3), "font-size", "8pt");
            TestUtility.CheckStyle(node.Styles.ElementAt(4), "font-family", "verdana");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 5);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-size", "10px");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-style", "oblique");
            TestUtility.CheckStyle(node.Styles.ElementAt(2), "font-variant", "small-caps");
            TestUtility.CheckStyle(node.Styles.ElementAt(3), "font-weight", "bold");
            TestUtility.CheckStyle(node.Styles.ElementAt(4), "font-family", "verdana");
        }
    }
}
