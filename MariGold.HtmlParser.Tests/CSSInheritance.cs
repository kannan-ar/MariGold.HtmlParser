namespace MariGold.HtmlParser.Tests
{
    using System;
    using NUnit.Framework;
    using MariGold.HtmlParser;
    using System.Linq;
    using System.IO;

    [TestFixture]
    public class CSSInheritance
    {
        [Test]
        public void BasicInheritance()
        {
            string html = "<div style='font-family:arial'><div>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            node.AnalyzeNode("div", "<div>test</div>", html, null, false, true, 1, 1, 1);
            node.Attributes.CheckKeyValuePair(0, "style", "font-family:arial");
            node.Styles.CheckKeyValuePair(0, "font-family", "arial");

            node.Children.ElementAt(0).AnalyzeNode("div", "test", "<div>test</div>", node, false, true, 1, 0, 1);
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-family", "arial");
        }

        [Test]
        public void MultiLayerInheritance()
        {
            string html = "<div style='font-family:arial'><div><span>test</span></div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            node.AnalyzeNode("div", "<div><span>test</span></div>", html, null, false, true, 1, 1, 1);
            node.Attributes.CheckKeyValuePair(0, "style", "font-family:arial");
            node.Styles.CheckKeyValuePair(0, "font-family", "arial");

            node.Children.ElementAt(0).AnalyzeNode("div", "<span>test</span>", "<div><span>test</span></div>", node, false, true, 1, 0, 1);
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-family", "arial");

            node.Children.ElementAt(0).Children.ElementAt(0).AnalyzeNode("span", "test", "<span>test</span>", node.Children.ElementAt(0), false, true, 1, 0, 1);
            node.Children.ElementAt(0).Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-family", "arial");
        }

        [Test]
        public void MultiLayerWithNoInheritance()
        {
            string html = "<div style='font-family:arial;width:10px'><div><span>test</span></div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            node.AnalyzeNode("div", "<div><span>test</span></div>", html, null, false, true, 1, 1, 2);
            node.Attributes.CheckKeyValuePair(0, "style", "font-family:arial;width:10px");
            node.Styles.CheckKeyValuePair(0, "font-family", "arial");
            node.Styles.CheckKeyValuePair(1, "width", "10px");

            node.Children.ElementAt(0).AnalyzeNode("div", "<span>test</span>", "<div><span>test</span></div>", node, false, true, 1, 0, 1);
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-family", "arial");

            node.Children.ElementAt(0).Children.ElementAt(0).AnalyzeNode("span", "test", "<span>test</span>", node.Children.ElementAt(0), false, true, 1, 0, 1);
            node.Children.ElementAt(0).Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-family", "arial");
        }

        [Test]
        public void MultiLevelInheritance()
        {
            string html = "<div style='font-family:arial'><div>one</div><span>two</span><p>three</p></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            node.AnalyzeNode("div", "<div>one</div><span>two</span><p>three</p>", html, null, false, true, 3, 1, 1);
            node.Attributes.CheckKeyValuePair(0, "style", "font-family:arial");
            node.Styles.CheckKeyValuePair(0, "font-family", "arial");

            node.Children.ElementAt(0).AnalyzeNode("div", "one", "<div>one</div>", node, false, true, 1, 0, 1);
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-family", "arial");

            node.Children.ElementAt(1).AnalyzeNode("span", "two", "<span>two</span>", node, false, true, 1, 0, 1);
            node.Children.ElementAt(1).Styles.CheckKeyValuePair(0, "font-family", "arial");

            node.Children.ElementAt(2).AnalyzeNode("p", "three", "<p>three</p>", node, false, true, 1, 0, 1);
            node.Children.ElementAt(2).Styles.CheckKeyValuePair(0, "font-family", "arial");
        }

        [Test]
        public void MultiLevelWithNoInheritance()
        {
            string html = "<div style='font-family:arial;margin:20px'><div>one</div><span>two</span><p>three</p></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            node.AnalyzeNode("div", "<div>one</div><span>two</span><p>three</p>", html, null, false, true, 3, 1, 2);
            node.Attributes.CheckKeyValuePair(0, "style", "font-family:arial;margin:20px");
            node.Styles.CheckKeyValuePair(0, "font-family", "arial");
            node.Styles.CheckKeyValuePair(1, "margin", "20px");

            node.Children.ElementAt(0).AnalyzeNode("div", "one", "<div>one</div>", node, false, true, 1, 0, 1);
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-family", "arial");

            node.Children.ElementAt(1).AnalyzeNode("span", "two", "<span>two</span>", node, false, true, 1, 0, 1);
            node.Children.ElementAt(1).Styles.CheckKeyValuePair(0, "font-family", "arial");

            node.Children.ElementAt(2).AnalyzeNode("p", "three", "<p>three</p>", node, false, true, 1, 0, 1);
            node.Children.ElementAt(2).Styles.CheckKeyValuePair(0, "font-family", "arial");
        }

        [Test]
        public void MultiLevelAndLayerInheritance()
        {
            string html = "<div style='font-family:arial'><div style='font-family:verdana'><b>one</b></div><span>two</span><p>three</p></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            node.AnalyzeNode("div", "<div style='font-family:verdana'><b>one</b></div><span>two</span><p>three</p>", html, null, false, true, 3, 1, 1);
            node.Attributes.CheckKeyValuePair(0, "style", "font-family:arial");
            node.Styles.CheckKeyValuePair(0, "font-family", "arial");

            node.Children.ElementAt(0).AnalyzeNode("div", "<b>one</b>", "<div style='font-family:verdana'><b>one</b></div>", node, false, true, 1, 1, 1);
            node.Children.ElementAt(0).Attributes.CheckKeyValuePair(0, "style", "font-family:verdana");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-family", "verdana");

            node.Children.ElementAt(0).Children.ElementAt(0).AnalyzeNode("b", "one", "<b>one</b>", node.Children.ElementAt(0), false, true, 1, 0, 1);
            node.Children.ElementAt(0).Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-family", "verdana");

            node.Children.ElementAt(1).AnalyzeNode("span", "two", "<span>two</span>", node, false, true, 1, 0, 1);
            node.Children.ElementAt(1).Styles.CheckKeyValuePair(0, "font-family", "arial");

            node.Children.ElementAt(2).AnalyzeNode("p", "three", "<p>three</p>", node, false, true, 1, 0, 1);
            node.Children.ElementAt(2).Styles.CheckKeyValuePair(0, "font-family", "arial");
        }

        [Test]
        public void ClassChildrenInheritance()
        {
            string html = @"<style>
                                .cls
                                {
                                	color:#fff;
                                	background-color:#000;
                                }
                            </style>
                            <div class='cls'><div>one</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "div", "<div>one</div>", "<div class='cls'><div>one</div></div>", null, false, true, 1, 1, 2);
            TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
            node.Styles.CheckKeyValuePair(0, "color", "#fff");
            node.Styles.CheckKeyValuePair(1, "background-color", "#000");

            TestUtility.AnalyzeNode(node.Children.ElementAt(0), "div", "one", "<div>one</div>", node, false, true, 1, 0, 2);
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(0, "color", "#fff");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(1, "background-color", "#000");
        }

        [Test]
        public void ClassChildrenMultiLevelInheritance()
        {
            string html = @"<style>
                                .cls
                                {
                                	color:#fff;
                                	background-color:#000;
                                }
                            </style>
                            <div class='cls'><div>one</div><span>two</span></div><div>three</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "div", "<div>one</div><span>two</span>", "<div class='cls'><div>one</div><span>two</span></div>", null, false, true, 2, 1, 2);
            TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
            node.Styles.CheckKeyValuePair(0, "color", "#fff");
            node.Styles.CheckKeyValuePair(1, "background-color", "#000");

            node.Children.ElementAt(0).AnalyzeNode("div", "one", "<div>one</div>", node, false, true, 1, 0, 2);
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(0, "color", "#fff");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(1, "background-color", "#000");

            node.Children.ElementAt(1).AnalyzeNode("span", "two", "<span>two</span>", node, false, true, 1, 0, 2);
            node.Children.ElementAt(1).Styles.CheckKeyValuePair(0, "color", "#fff");
            node.Children.ElementAt(1).Styles.CheckKeyValuePair(1, "background-color", "#000");

            node = node.Next;

            node.AnalyzeNode("div", "three", "<div>three</div>", null, false, true, 1, 0, 0);
        }

        [Test]
        public void BasicNonInheritance()
        {
            string html = "<div style='width:10px'><div>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            node.AnalyzeNode("div", "<div>test</div>", html, null, false, true, 1, 1, 1);
            node.Attributes.CheckKeyValuePair(0, "style", "width:10px");
            node.Styles.CheckKeyValuePair(0, "width", "10px");

            node.Children.ElementAt(0).AnalyzeNode("div", "test", "<div>test</div>", node, false, true, 1, 0, 0);
        }

        [Test]
        public void InheritKeyWord()
        {
            string html = "<div style='width:10px'><div style='width:inherit'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            node.AnalyzeNode("div", "<div style='width:inherit'>test</div>", html, null, false, true, 1, 1, 1);
            node.Attributes.CheckKeyValuePair(0, "style", "width:10px");
            node.Styles.CheckKeyValuePair(0, "width", "10px");

            node.Children.ElementAt(0).AnalyzeNode("div", "test", "<div style='width:inherit'>test</div>", node, false, true, 1, 1, 1);
            node.Children.ElementAt(0).Attributes.CheckKeyValuePair(0, "style", "width:inherit");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(0, "width", "10px");
        }

        [Test]
        public void InheritKeyWordMultiLevel()
        {
            string html = "<div style='font-size:10px'><div style='font-size:inherit'><span>test</span><p>one</p></div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            node.AnalyzeNode("div", "<div style='font-size:inherit'><span>test</span><p>one</p></div>", html, null, false, true, 1, 1, 1);
            node.Attributes.CheckKeyValuePair(0, "style", "font-size:10px");
            node.Styles.CheckKeyValuePair(0, "font-size", "10px");

            node.Children.ElementAt(0).AnalyzeNode("div", "<span>test</span><p>one</p>", "<div style='font-size:inherit'><span>test</span><p>one</p></div>", node, false, true, 2, 1, 1);
            node.Children.ElementAt(0).Attributes.CheckKeyValuePair(0, "style", "font-size:inherit");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-size", "10px");

            node.Children.ElementAt(0).Children.ElementAt(0).AnalyzeNode("span", "test", "<span>test</span>", node.Children.ElementAt(0), false, true, 1, 0, 1);
            node.Children.ElementAt(0).Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-size", "10px");

            node.Children.ElementAt(0).Children.ElementAt(1).AnalyzeNode("p", "one", "<p>one</p>", node.Children.ElementAt(0), false, true, 1, 0, 1);
            node.Children.ElementAt(0).Children.ElementAt(1).Styles.CheckKeyValuePair(0, "font-size", "10px");
        }

        [Test]
        public void InheritKeyWordMultiLevelTwoStyle()
        {
            string html = "<div style='font-size:10px;width:20px'><div style='width:inherit'><span>test</span><p>one</p></div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            node.AnalyzeNode("div", "<div style='width:inherit'><span>test</span><p>one</p></div>", html, null, false, true, 1, 1, 2);
            node.Attributes.CheckKeyValuePair(0, "style", "font-size:10px;width:20px");
            node.Styles.CheckKeyValuePair(0, "font-size", "10px");
            node.Styles.CheckKeyValuePair(1, "width", "20px");

            node.Children.ElementAt(0).AnalyzeNode("div", "<span>test</span><p>one</p>", "<div style='width:inherit'><span>test</span><p>one</p></div>", node, false, true, 2, 1, 2);
            node.Children.ElementAt(0).Attributes.CheckKeyValuePair(0, "style", "width:inherit");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(0, "width", "20px");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(1, "font-size", "10px");

            node.Children.ElementAt(0).Children.ElementAt(0).AnalyzeNode("span", "test", "<span>test</span>", node.Children.ElementAt(0), false, true, 1, 0, 1);
            node.Children.ElementAt(0).Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-size", "10px");

            node.Children.ElementAt(0).Children.ElementAt(1).AnalyzeNode("p", "one", "<p>one</p>", node.Children.ElementAt(0), false, true, 1, 0, 1);
            node.Children.ElementAt(0).Children.ElementAt(1).Styles.CheckKeyValuePair(0, "font-size", "10px");
        }

        [Test]
        public void InheritKeyWordMultiLevelLayerTwoStyle()
        {
            string html = "<div style='font-size:10px;width:20px'><div style='width:inherit'><span>test</span><p>one</p></div><div>last</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            node.AnalyzeNode("div", "<div style='width:inherit'><span>test</span><p>one</p></div><div>last</div>", html, null, false, true, 2, 1, 2);
            node.Attributes.CheckKeyValuePair(0, "style", "font-size:10px;width:20px");
            node.Styles.CheckKeyValuePair(0, "font-size", "10px");
            node.Styles.CheckKeyValuePair(1, "width", "20px");

            node.Children.ElementAt(0).AnalyzeNode("div", "<span>test</span><p>one</p>", "<div style='width:inherit'><span>test</span><p>one</p></div>", node, false, true, 2, 1, 2);
            node.Children.ElementAt(0).Attributes.CheckKeyValuePair(0, "style", "width:inherit");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(0, "width", "20px");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(1, "font-size", "10px");

            node.Children.ElementAt(0).Children.ElementAt(0).AnalyzeNode("span", "test", "<span>test</span>", node.Children.ElementAt(0), false, true, 1, 0, 1);
            node.Children.ElementAt(0).Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-size", "10px");

            node.Children.ElementAt(0).Children.ElementAt(1).AnalyzeNode("p", "one", "<p>one</p>", node.Children.ElementAt(0), false, true, 1, 0, 1);
            node.Children.ElementAt(0).Children.ElementAt(1).Styles.CheckKeyValuePair(0, "font-size", "10px");

            node.Children.ElementAt(1).AnalyzeNode("div", "last", "<div>last</div>", node, false, true, 1, 0, 1);
            node.Children.ElementAt(1).Styles.CheckKeyValuePair(0, "font-size", "10px");
        }

        [Test]
        public void CustomAttributeOnly()
        {
            string html = @"<style>
                                [attr]
                                {
                                	font-weight:bold;
                                }
                            </style>
                            <div>one</div>
                            <div attr><p>one</p></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "div", "one", "<div>one</div>", null, false, true, 1, 0, 0);

            node = node.Next;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "div", "<p>one</p>", "<div attr><p>one</p></div>", null, false, true, 1, 1, 1);
            TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "");
            TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "font-weight", "bold");

            node.Children.ElementAt(0).AnalyzeNode("p", "one", "<p>one</p>", node, false, true, 1, 0, 1);
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-weight", "bold");

            Assert.IsNull(node.Next);
        }

        [Test]
        public void CustomAttributeOnlyImmediateP()
        {
            string html = @"<style>
                                [attr]>p
                                {
                                	font-weight:bold;
                                }
                            </style>
                            <div>one</div>
                            <div attr><p><span>one</span></p></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "div", "one", "<div>one</div>", null, false, true, 1, 0, 0);

            node = node.Next;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "div", "<p><span>one</span></p>", "<div attr><p><span>one</span></p></div>", null, false, true, 1, 1, 0);
            TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "");

            node.Children.ElementAt(0).AnalyzeNode("p", "<span>one</span>", "<p><span>one</span></p>", node, false, true, 1, 0, 1);
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-weight", "bold");

            node.Children.ElementAt(0).Children.ElementAt(0).AnalyzeNode("span", "one", "<span>one</span>", node.Children.ElementAt(0), false, true, 1, 0, 1);
            node.Children.ElementAt(0).Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-weight", "bold");

            Assert.IsNull(node.Next);
        }

        [Test]
        public void MultiLevelStyles()
        {
            string path = TestUtility.GetFolderPath("Html\\multilevelstyles.htm");
            string html = string.Empty;

            using (StreamReader sr = new StreamReader(path))
            {
                html = sr.ReadToEnd();
            }

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();
            parser.ParseCSS();

            IHtmlNode node = parser.Current;

            while (node.Tag != "html")
                node = node.Next;

            node = node.Children.ElementAt(0);

            while (node.Tag != "body")
                node = node.Next;

            IHtmlNode parent = node.Children.ElementAt(0).Children.ElementAt(0);
            node = node.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(0);
            node.AnalyzeNode("h2", "test", "<h2>test</h2>", parent,
                 false, true, 1, 0, 1);
            TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "blue");
        }

        [Test]
        public void CSSSpecificity()
        {
            string path = TestUtility.GetFolderPath("Html\\cssspecificity.htm");
            string html = string.Empty;

            using (StreamReader sr = new StreamReader(path))
            {
                html = sr.ReadToEnd();
            }

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();
            parser.ParseCSS();

            IHtmlNode node = parser.Current;

            while (node.Tag != "html")
                node = node.Next;

            node = node.Children.ElementAt(0);

            while (node.Tag != "body")
                node = node.Next;

            IHtmlNode parent = node.Children.ElementAt(0).Children.ElementAt(0);
            node = node.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(0);
            node.AnalyzeNode("h2", "test", "<h2>test</h2>", parent,
                 false, true, 1, 0, 1);
            TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "red");
        }

        [Test]
        public void CSSChain10Classes()
        {
            string path = TestUtility.GetFolderPath("Html\\csschain10classes.htm");
            string html = string.Empty;

            using (StreamReader sr = new StreamReader(path))
            {
                html = sr.ReadToEnd();
            }

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();
            parser.ParseCSS();

            IHtmlNode node = parser.Current;

            while (node.Tag != "html")
                node = node.Next;

            node = node.Children.ElementAt(0);

            while (node.Tag != "body")
                node = node.Next;

            IHtmlNode parent = node;

            while (node.Tag != "h2")
            {
                parent = node;
                node = node.Children.ElementAt(0);
            }

            node.AnalyzeNode("h2", "test", "<h2 id=\"h\">test</h2>", parent,
                 false, true, 1, 1, 1);
            TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "red");
        }

        [Test]
        public void BackgroundInheritance()
        {
            string html = "<div style='background: #fff'><div>Test</div></div>";
            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            node.AnalyzeNode("div", "<div>Test</div>", html, null, false, true, 1, 1, 1);
            IHtmlNode head = node;

            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            node.AnalyzeNode("div", "Test", "<div>Test</div>", head, false, true, 1, 0, 1);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "background", "#fff");
        }

        [Test]
        public void CSSFontAndFontFamily()
        {
            string html = "<div style='font:10px Arial'><div style='font-family:Verdana;'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);

            node.AnalyzeNode("div", "<div style='font-family:Verdana;'>test</div>", html, null, false, true, 1, 1, 2);
            IHtmlNode parent = node;

            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            node.AnalyzeNode("div", "test", "<div style='font-family:Verdana;'>test</div>", parent, false, true, 1, 1, 2);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-family", "Verdana");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-size", "10px");

        }

        [Test]
        public void OverrideBackgroundTransparent()
        {
            string html = "<div style='background-color:#000'><div style='background-color:transparent'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);

            node.AnalyzeNode("div", "<div style='background-color:transparent'>test</div>", html, null, false, true, 1, 1, 1);
            IHtmlNode parent = node;

            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            node.AnalyzeNode("div", "test", "<div style='background-color:transparent'>test</div>", parent, false, true, 1, 1, 1);
            node.Styles.CheckKeyValuePair(0, "background-color", "#000");
        }

        [Test]
        public void OverrideBackgroundColorTransparent()
        {
            string html = "<div style='background: #000 url(\"img.gif\") no-repeat fixed center;'><div style='background-color:transparent'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);

            node.AnalyzeNode("div", "<div style='background-color:transparent'>test</div>", html, null, false, true, 1, 1, 1);
            IHtmlNode parent = node;

            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            node.AnalyzeNode("div", "test", "<div style='background-color:transparent'>test</div>", parent, false, true, 1, 1, 1);
            node.Styles.CheckKeyValuePair(0, "background-color", "#000");
        }

        [Test]
        public void OverrideBackgroundColorBackgroundTransparent()
        {
            string html = "<div style='background-color: #000'><div style='background:transparent'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);

            node.AnalyzeNode("div", "<div style='background:transparent'>test</div>", html, null, false, true, 1, 1, 1);
            IHtmlNode parent = node;

            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            node.AnalyzeNode("div", "test", "<div style='background:transparent'>test</div>", parent, false, true, 1, 1, 1);
            node.Styles.CheckKeyValuePair(0, "background", "#000");
        }

        [Test]
        public void OverrideChildFontSizePercentage()
        {
            string html = "<div style='font-size:4em'><div style='font-size:100%'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            node.AnalyzeNode("div", "<div style='font-size:100%'>test</div>", html, null, false, true, 1, 1, 1);
            node.Styles.CheckKeyValuePair(0, "font-size", "4em");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            node.AnalyzeNode("div", "test", "<div style='font-size:100%'>test</div>", parent, false, true, 1, 1, 1);
            node.Styles.CheckKeyValuePair(0, "font-size", "4em");
        }

        [Test]
        public void OverrideChildFontSizeTwoHunderedPercentage()
        {
            string html = "<div style='font-size:4em'><div style='font-size:200%'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            node.AnalyzeNode("div", "<div style='font-size:200%'>test</div>", html, null, false, true, 1, 1, 1);
            node.Styles.CheckKeyValuePair(0, "font-size", "4em");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            node.AnalyzeNode("div", "test", "<div style='font-size:200%'>test</div>", parent, false, true, 1, 1, 1);
            node.Styles.CheckKeyValuePair(0, "font-size", "8em");
        }

        [Test]
        public void OverrideChildFontSizeAllPercentages()
        {
            string html = "<div style='font-size:200%'><div style='font-size:50%'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            node.AnalyzeNode("div", "<div style='font-size:50%'>test</div>", html, null, false, true, 1, 1, 1);
            node.Styles.CheckKeyValuePair(0, "font-size", "200%");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            node.AnalyzeNode("div", "test", "<div style='font-size:50%'>test</div>", parent, false, true, 1, 1, 1);
            node.Styles.CheckKeyValuePair(0, "font-size", "100%");
        }

        [Test]
        public void OverrideChildFontSizeWith120Percentage()
        {
            string html = "<div style='font-size:10px'><div style='font-size:120%'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);
            node.AnalyzeNode("div", "<div style='font-size:120%'>test</div>", html, null, false, true, 1, 1, 1);
            node.Styles.CheckKeyValuePair(0, "font-size", "10px");

            IHtmlNode parent = node;
            node = node.Children.ElementAt(0);
            node.AnalyzeNode("div", "test", "<div style='font-size:120%'>test</div>", parent, false, true, 1, 1, 1);
            node.Styles.CheckKeyValuePair(0, "font-size", "12px");
        }

        [Test]
        public void CSSFontPercentageAndFontFamily()
        {
            string html = "<div style='font:10px Arial'><div style='font-size:75%'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);

            node.AnalyzeNode("div", "<div style='font-size:75%'>test</div>", html, null, false, true, 1, 1, 2);
            IHtmlNode parent = node;

            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            node.AnalyzeNode("div", "test", "<div style='font-size:75%'>test</div>", parent, false, true, 1, 1, 2);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-size", "8px");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-family", "Arial");
        }

        [Test]
        public void CSSFontPercentageInheritance()
        {
            string html = "<div style='font:10px Arial'><div style='font:50% Verdana'>test</div></div>";

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();
            parser.ParseCSS();

            IHtmlNode node = parser.Current;
            Assert.IsNotNull(node);

            node.AnalyzeNode("div", "<div style='font:50% Verdana'>test</div>", html, null, false, true, 1, 1, 2);
            IHtmlNode parent = node;

            node = node.Children.ElementAt(0);
            Assert.IsNotNull(node);
            node.AnalyzeNode("div", "test", "<div style='font:50% Verdana'>test</div>", parent, false, true, 1, 1, 2);
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "font-size", "5px");
            TestUtility.CheckStyle(node.Styles.ElementAt(1), "font-family", "Verdana");
        }
    }
}
