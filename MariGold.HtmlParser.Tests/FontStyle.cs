namespace MariGold.HtmlParser.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using MariGold.HtmlParser;
    using System.IO;

    [TestFixture]
    public class FontStyle
    {
        [Test]
        public void FontSizeOnly()
        {
            string html = "<div style='font:25px'><div style='font-family:verdana'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-family:verdana'>test</div>", html, null, false, true, 1, 1, 1);
            node.CheckStyle(0, "font", "25px");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-family:verdana'>test</div>", parent, false, true, 1, 1, 1);
            node.CheckStyle(0, "font-family", "verdana");
        }

        [Test]
        public void FontSizeFontFamily()
        {
            string html = "<div style='font:25px arial'><div style='font-family:verdana'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-family:verdana'>test</div>", html, null, false, true, 1, 1, 2);
            node.CheckStyle(0, "font-size", "25px");
            node.CheckStyle(1, "font-family", "arial");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-family:verdana'>test</div>", parent, false, true, 1, 1, 1);
            node.CheckStyle(0, "font-family", "verdana");
            Assert.AreEqual(2, node.InheritedStyles.Count);
            node.CheckInheritedStyle(0, "font-size", "25px");
            node.CheckInheritedStyle(1, "font-family", "verdana");
        }

        [Test]
        public void FontSizeLineHeightFontFamily()
        {
            string html = "<div style='font:25px/2pt arial'><div style='font-family:verdana'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-family:verdana'>test</div>", html, null, false, true, 1, 1, 3);
            node.CheckStyle(0, "font-size", "25px");
            node.CheckStyle(1, "line-height", "2pt");
            node.CheckStyle(2, "font-family", "arial");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-family:verdana'>test</div>", parent, false, true, 1, 1, 1);
            node.CheckStyle(0, "font-family", "verdana");
            Assert.AreEqual(3, node.InheritedStyles.Count);
            node.CheckInheritedStyle(0, "font-size", "25px");
            node.CheckInheritedStyle(1, "line-height", "2pt");
            node.CheckInheritedStyle(2, "font-family", "verdana");
        }

        [Test]
        public void ParentFont()
        {
            string html = "<div style='font:arial'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 1);
            node.CheckStyle(0, "font", "arial");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
            node.CheckStyle(0, "font-size", "10px");
        }

        [Test]
        public void ChildFont()
        {
            string html = "<div style='font:8pt arial'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 2);
            node.CheckStyle(0, "font-size", "8pt");
            node.CheckStyle(1, "font-family", "arial");


            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
            node.CheckStyle(0, "font-size", "10px");
            Assert.AreEqual(2, node.InheritedStyles.Count);
            node.CheckInheritedStyle(0, "font-family", "arial");
            node.CheckInheritedStyle(1, "font-size", "10px");
        }

        [Test]
        public void FontWeightNormal()
        {
            string html = "<div style='font:normal 8pt arial'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 3);
            node.CheckStyle(0, "font-weight", "normal");
            node.CheckStyle(1, "font-size", "8pt");
            node.CheckStyle(2, "font-family", "arial");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
            node.CheckStyle(0, "font-size", "10px");
            Assert.AreEqual(3, node.InheritedStyles.Count);
            node.CheckInheritedStyle(0, "font-weight", "normal");
            node.CheckInheritedStyle(1, "font-family", "arial");
            node.CheckInheritedStyle(2, "font-size", "10px");
        }

        [Test]
        public void FontVariantFontWeightNormal()
        {
            string html = "<div style='font:normal inherit 1em arial'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 4);
            node.CheckStyle(0, "font-weight", "inherit");
            node.CheckStyle(1, "font-variant", "normal");
            node.CheckStyle(2, "font-size", "1em");
            node.CheckStyle(3, "font-family", "arial");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
            node.CheckStyle(0, "font-size", "10px");
            Assert.AreEqual(4, node.InheritedStyles.Count);
            node.CheckInheritedStyle(0, "font-weight", "inherit");
            node.CheckInheritedStyle(1, "font-variant", "normal");
            node.CheckInheritedStyle(2, "font-family", "arial");
            node.CheckInheritedStyle(3, "font-size", "10px");
        }

        [Test]
        public void FontStyleFontVariantFontWeightNormal()
        {
            string html = "<div style='font:initial normal inherit 1em arial'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 5);
            node.CheckStyle(0, "font-weight", "inherit");
            node.CheckStyle(1, "font-variant", "normal");
            node.CheckStyle(2, "font-style", "initial");
            node.CheckStyle(3, "font-size", "1em");
            node.CheckStyle(4, "font-family", "arial");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
            node.CheckStyle(0, "font-size", "10px");
            Assert.AreEqual(5, node.InheritedStyles.Count);
            node.CheckInheritedStyle(0, "font-weight", "inherit");
            node.CheckInheritedStyle(1, "font-variant", "normal");
            node.CheckInheritedStyle(2, "font-style", "initial");
            node.CheckInheritedStyle(3, "font-family", "arial");
            node.CheckInheritedStyle(4, "font-size", "10px");
        }

        [Test]
        public void FontStyleFontVariantFontWeightNormalFontSizeLineHeight()
        {
            string html = "<div style='font:initial normal inherit 1em/2px arial'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 6);
            node.CheckStyle(0, "font-weight", "inherit");
            node.CheckStyle(1, "font-variant", "normal");
            node.CheckStyle(2, "font-style", "initial");
            node.CheckStyle(3, "font-size", "1em");
            node.CheckStyle(4, "line-height", "2px");
            node.CheckStyle(5, "font-family", "arial");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
            node.CheckStyle(0, "font-size", "10px");
            Assert.AreEqual(6, node.InheritedStyles.Count);

            node.CheckInheritedStyle(0, "font-weight", "inherit");
            node.CheckInheritedStyle(1, "font-variant", "normal");
            node.CheckInheritedStyle(2, "font-style", "initial");
            node.CheckInheritedStyle(3, "line-height", "2px");
            node.CheckInheritedStyle(4, "font-family", "arial");
            node.CheckInheritedStyle(5, "font-size", "10px");
        }

        [Test]
        public void FontStyleFontVariantFontWeightNormalFontSizeLineHeightCaption()
        {
            string html = "<div style='font:initial normal inherit 1em/2px arial  caption'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 6);
            node.CheckStyle(0, "font-weight", "inherit");
            node.CheckStyle(1, "font-variant", "normal");
            node.CheckStyle(2, "font-style", "initial");
            node.CheckStyle(3, "font-size", "1em");
            node.CheckStyle(4, "line-height", "2px");
            node.CheckStyle(5, "font-family", "arial");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
            node.CheckStyle(0, "font-size", "10px");
            Assert.AreEqual(6, node.InheritedStyles.Count);

            node.CheckInheritedStyle(0, "font-weight", "inherit");
            node.CheckInheritedStyle(1, "font-variant", "normal");
            node.CheckInheritedStyle(2, "font-style", "initial");
            node.CheckInheritedStyle(3, "line-height", "2px");
            node.CheckInheritedStyle(4, "font-family", "arial");
            node.CheckInheritedStyle(5, "font-size", "10px");
        }

        [Test]
        public void FontSizeLineHeightFontFamilyCaption()
        {
            string html = "<div style='font:8pt/2px verdana caption'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 3);
            node.CheckStyle(0, "font-size", "8pt");
            node.CheckStyle(1, "line-height", "2px");
            node.CheckStyle(2, "font-family", "verdana");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
            node.CheckStyle(0, "font-size", "10px");
            Assert.AreEqual(3, node.InheritedStyles.Count);
            node.CheckInheritedStyle(0, "line-height", "2px");
            node.CheckInheritedStyle(1, "font-family", "verdana");
            node.CheckInheritedStyle(2, "font-size", "10px");

        }

        [Test]
        public void FontWeightFontSizeFontFamilyMessageBox()
        {
            string html = "<div style='font:bold 8pt verdana message-box'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 3);
            node.CheckStyle(0, "font-weight", "bold");
            node.CheckStyle(1, "font-size", "8pt");
            node.CheckStyle(2, "font-family", "verdana");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
            node.CheckStyle(0, "font-size", "10px");
            Assert.AreEqual(3, node.InheritedStyles.Count);
            node.CheckInheritedStyle(0, "font-weight", "bold");
            node.CheckInheritedStyle(1, "font-family", "verdana");
            node.CheckInheritedStyle(2, "font-size", "10px");
        }

        [Test]
        public void FontVariantFontWeightFontSizeFontFamilyMessageBox()
        {
            string html = "<div style='font:small-caps  bold 8pt  verdana  message-box'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 4);
            node.CheckStyle(0, "font-variant", "small-caps");
            node.CheckStyle(1, "font-weight", "bold");
            node.CheckStyle(2, "font-size", "8pt");
            node.CheckStyle(3, "font-family", "verdana");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
            node.CheckStyle(0, "font-size", "10px");
            Assert.AreEqual(4, node.InheritedStyles.Count);
            node.CheckInheritedStyle(0, "font-variant", "small-caps");
            node.CheckInheritedStyle(1, "font-weight", "bold");
            node.CheckInheritedStyle(2, "font-family", "verdana");
            node.CheckInheritedStyle(3, "font-size", "10px");
        }

        [Test]
        public void FontStyleFontVariantFontWeightFontSizeFontFamilyMessageBox()
        {
            string html = "<div style='font: oblique small-caps  bold 8pt  verdana  message-box'><div style='font-size:10px'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 5);
            node.CheckStyle(0, "font-style", "oblique");
            node.CheckStyle(1, "font-variant", "small-caps");
            node.CheckStyle(2, "font-weight", "bold");
            node.CheckStyle(3, "font-size", "8pt");
            node.CheckStyle(4, "font-family", "verdana");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
            node.CheckStyle(0, "font-size", "10px");
            Assert.AreEqual(5, node.InheritedStyles.Count);
            node.CheckInheritedStyle(0, "font-style", "oblique");
            node.CheckInheritedStyle(1, "font-variant", "small-caps");
            node.CheckInheritedStyle(2, "font-weight", "bold");
            node.CheckInheritedStyle(3, "font-family", "verdana");
            node.CheckInheritedStyle(4, "font-size", "10px");
        }

        [Test]
        public void Parent10pxChild2Em()
        {
            string html = "<div style=\"font-size:10px\"><div style=\"font-size:2em\">test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);

            node.AnalyzeNode("div", "<div style=\"font-size:2em\">test</div>", html, null, false, true, 1, 1, 1);
            node.CheckStyle(0, "font-size", "10px");
            IHtmlNode parent = node;

            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            node.AnalyzeNode("div", "test", "<div style=\"font-size:2em\">test</div>", parent, false, true, 1, 1, 1);
            node.CheckStyle(0, "font-size", "20px");
        }

        [Test]
        public void InnerSpanRelativeFontSize()
        {
            string path = TestUtility.GetFolderPath("Html\\innerspanrelativefontsize.htm");
            string html = string.Empty;

            using (StreamReader sr = new StreamReader(path))
            {
                html = sr.ReadToEnd();
            }

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Parse());
            parser.ParseStyles();

            IHtmlNode node = parser.Current;

            while (node.Tag != "html")
                node = node.Next;

            node = node.Children.ElementAt(0);

            while (node.Tag != "body")
                node = node.Next;

            node = node.Children.ElementAt(0);

            while (node.Tag != "div")
                node = node.Next;

            node = node.Children.ElementAt(0);

            node.CheckStyle(0, "font-size", "50px");

            node = node.Children.ElementAt(0);

            while (node.Tag != "span")
                node = node.Next;

            node.CheckStyle(0, "font-size", "25px");
        }

        [Test]
        public void FiftyPercentageEM()
        {
            string html = "<div style=\"font-size:10px\"><div style=\"font-size:0.50em\">test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);

            node.AnalyzeNode("div", "<div style=\"font-size:0.50em\">test</div>", html, null, false, true, 1, 1, 1);
            node.CheckStyle(0, "font-size", "10px");
            IHtmlNode parent = node;

            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            node.AnalyzeNode("div", "test", "<div style=\"font-size:0.50em\">test</div>", parent, false, true, 1, 1, 1);
            node.CheckStyle(0, "font-size", "5px");
        }
    }
}
