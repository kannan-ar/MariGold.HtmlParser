namespace MariGold.HtmlParser.Tests
{
    using System;
    using NUnit.Framework;
    using MariGold.HtmlParser;
    using System.Linq;
    using System.IO;

    [TestFixture]
    public partial class ComplexStyles
    {
        [Test]
        public void AttributeImmediateChildrenClassIdentity()
        {
            string html = @"<style>
								[attr] > .cls #pt
								{
									background-color:red;
								}
							</style>
							<div attr><div class='cls'><p>one1</p><p id='pt'>one2</p></div><div>two</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "div", "<div class='cls'><p>one1</p><p id='pt'>one2</p></div><div>two</div>",
                                    "<div attr><div class='cls'><p>one1</p><p id='pt'>one2</p></div><div>two</div></div>", null, false, true, 2, 1, 0);
            TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "");

            TestUtility.AnalyzeNode(node.Children.ElementAt(0), "div", "<p>one1</p><p id='pt'>one2</p>", "<div class='cls'><p>one1</p><p id='pt'>one2</p></div>", node, false, true, 2, 1, 0);
            TestUtility.CheckKeyValuePair(node.Children.ElementAt(0).Attributes.ElementAt(0), "class", "cls");

            TestUtility.AnalyzeNode(node.Children.ElementAt(0).Children.ElementAt(0), "p", "one1", "<p>one1</p>", node.Children.ElementAt(0), false, true, 1, 0, 0);

            TestUtility.AnalyzeNode(node.Children.ElementAt(0).Children.ElementAt(1), "p", "one2", "<p id='pt'>one2</p>", node.Children.ElementAt(0), false, true, 1, 1, 1);
            TestUtility.CheckKeyValuePair(node.Children.ElementAt(0).Children.ElementAt(1).Attributes.ElementAt(0), "id", "pt");
            TestUtility.CheckKeyValuePair(node.Children.ElementAt(0).Children.ElementAt(1).Styles.ElementAt(0), "background-color", "red");

            TestUtility.AnalyzeNode(node.Children.ElementAt(1), "div", "two", "<div>two</div>", node, false, true, 1, 0, 0);

        }

        [Test]
        public void AttributeImmediateChildrenClassIdentityNthChild()
        {
            string html = @"<style>
								[attr] > .cls #pt:nth-child(2)
								{
									background-color:red;
								}
							</style>
							<div attr><div class='cls'><p>one1</p><p id='pt'>one2</p></div><div>two</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "div", "<div class='cls'><p>one1</p><p id='pt'>one2</p></div><div>two</div>",
                                    "<div attr><div class='cls'><p>one1</p><p id='pt'>one2</p></div><div>two</div></div>", null, false, true, 2, 1, 0);
            TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "");

            TestUtility.AnalyzeNode(node.Children.ElementAt(0), "div", "<p>one1</p><p id='pt'>one2</p>", "<div class='cls'><p>one1</p><p id='pt'>one2</p></div>", node, false, true, 2, 1, 0);
            TestUtility.CheckKeyValuePair(node.Children.ElementAt(0).Attributes.ElementAt(0), "class", "cls");

            TestUtility.AnalyzeNode(node.Children.ElementAt(0).Children.ElementAt(0), "p", "one1", "<p>one1</p>", node.Children.ElementAt(0), false, true, 1, 0, 0);

            TestUtility.AnalyzeNode(node.Children.ElementAt(0).Children.ElementAt(1), "p", "one2", "<p id='pt'>one2</p>", node.Children.ElementAt(0), false, true, 1, 1, 1);
            TestUtility.CheckKeyValuePair(node.Children.ElementAt(0).Children.ElementAt(1).Attributes.ElementAt(0), "id", "pt");
            TestUtility.CheckKeyValuePair(node.Children.ElementAt(0).Children.ElementAt(1).Styles.ElementAt(0), "background-color", "red");

            TestUtility.AnalyzeNode(node.Children.ElementAt(1), "div", "two", "<div>two</div>", node, false, true, 1, 0, 0);

        }

        [Test]
        public void AttributeImmediateChildrenClassIdentitySpanFirstChild()
        {
            string html = @"<style>
								[attr] > .cls #pt span:first-child
								{
									background-color:red;
								}
							</style>
							<div attr><div class='cls'><p>one1</p><p id='pt'><span>1</span><span>2</span></p></div><div>two</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "div", "<div class='cls'><p>one1</p><p id='pt'><span>1</span><span>2</span></p></div><div>two</div>",
                                    "<div attr><div class='cls'><p>one1</p><p id='pt'><span>1</span><span>2</span></p></div><div>two</div></div>", null, false, true, 2, 1, 0);
            TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "");

            TestUtility.AnalyzeNode(node.Children.ElementAt(0), "div", "<p>one1</p><p id='pt'><span>1</span><span>2</span></p>", "<div class='cls'><p>one1</p><p id='pt'><span>1</span><span>2</span></p></div>",
                                    node, false, true, 2, 1, 0);
            TestUtility.CheckKeyValuePair(node.Children.ElementAt(0).Attributes.ElementAt(0), "class", "cls");

            TestUtility.AnalyzeNode(node.Children.ElementAt(0).Children.ElementAt(0), "p", "one1", "<p>one1</p>", node.Children.ElementAt(0), false, true, 1, 0, 0);

            TestUtility.AnalyzeNode(node.Children.ElementAt(0).Children.ElementAt(1), "p", "<span>1</span><span>2</span>", "<p id='pt'><span>1</span><span>2</span></p>", node.Children.ElementAt(0), false, true, 2, 1, 0);
            TestUtility.CheckKeyValuePair(node.Children.ElementAt(0).Children.ElementAt(1).Attributes.ElementAt(0), "id", "pt");

            TestUtility.AnalyzeNode(node.Children.ElementAt(0).Children.ElementAt(1).Children.ElementAt(0), "span", "1", "<span>1</span>", node.Children.ElementAt(0).Children.ElementAt(1), false, true, 1, 0, 1);
            TestUtility.CheckKeyValuePair(node.Children.ElementAt(0).Children.ElementAt(1).Children.ElementAt(0).Styles.ElementAt(0), "background-color", "red");

            TestUtility.AnalyzeNode(node.Children.ElementAt(0).Children.ElementAt(1).Children.ElementAt(1), "span", "2", "<span>2</span>", node.Children.ElementAt(0).Children.ElementAt(1), false, true, 1, 0, 0);

            TestUtility.AnalyzeNode(node.Children.ElementAt(1), "div", "two", "<div>two</div>", node, false, true, 1, 0, 0);

        }

        [Test]
        public void FirstChildIdentity()
        {
            string html = @"<style>
								:first-child #dv > p
								{
									background-color:red;
								}
							</style>
							<div><div><div id='dv'><p>1</p><span>2</span></div></div><div>3</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "div", "<div><div id='dv'><p>1</p><span>2</span></div></div><div>3</div>",
                "<div><div><div id='dv'><p>1</p><span>2</span></div></div><div>3</div></div>", null, false, true, 2, 0, 0);

            node.Children.ElementAt(0).AnalyzeNode("div", "<div id='dv'><p>1</p><span>2</span></div>", "<div><div id='dv'><p>1</p><span>2</span></div></div>",
                node, false, true, 1, 0, 0);

            node.Children.ElementAt(0).Children.ElementAt(0).AnalyzeNode("div", "<p>1</p><span>2</span>", "<div id='dv'><p>1</p><span>2</span></div>", node.Children.ElementAt(0), false, true,
                2, 1, 0);
            node.Children.ElementAt(0).Children.ElementAt(0).Attributes.CheckKeyValuePair(0, "id", "dv");

            node.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(0).AnalyzeNode("p", "1", "<p>1</p>", node.Children.ElementAt(0).Children.ElementAt(0), false, true, 1, 0, 1);
            node.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(0).Styles.CheckKeyValuePair(0, "background-color", "red");

            node.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(1).AnalyzeNode("span", "2", "<span>2</span>", node.Children.ElementAt(0).Children.ElementAt(0), false, true, 1, 0, 0);

            node.Children.ElementAt(1).AnalyzeNode("div", "3", "<div>3</div>", node, false, true, 1, 0, 0);
        }

        [Test]
        public void DivNthChildNextElement()
        {
            string html = @"<style>
								div:nth-child(1) + div
								{
									background-color:red;
								}
							</style>
							<div><div>1</div><div>2</div><div>3</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            node.AnalyzeNode("div", "<div>1</div><div>2</div><div>3</div>", "<div><div>1</div><div>2</div><div>3</div></div>", null,
                false, true, 3, 0, 0);

            node.Children.ElementAt(0).AnalyzeNode("div", "1", "<div>1</div>", node, false, true, 1, 0, 0);

            node.Children.ElementAt(1).AnalyzeNode("div", "2", "<div>2</div>", node, false, true, 1, 0, 1);
            node.Children.ElementAt(1).Styles.CheckKeyValuePair(0, "background-color", "red");

            node.Children.ElementAt(2).AnalyzeNode("div", "3", "<div>3</div>", node, false, true, 1, 0, 0);
        }

        [Test]
        public void DivNthChildAllNextElement()
        {
            string html = @"<style>
								div:nth-child(1) ~ div
								{
									background-color:red;
								}
							</style>
							<div><div>1</div><div>2</div><div>3</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            node.AnalyzeNode("div", "<div>1</div><div>2</div><div>3</div>", "<div><div>1</div><div>2</div><div>3</div></div>", null,
                false, true, 3, 0, 0);

            node.Children.ElementAt(0).AnalyzeNode("div", "1", "<div>1</div>", node, false, true, 1, 0, 0);

            node.Children.ElementAt(1).AnalyzeNode("div", "2", "<div>2</div>", node, false, true, 1, 0, 1);
            node.Children.ElementAt(1).Styles.CheckKeyValuePair(0, "background-color", "red");

            node.Children.ElementAt(2).AnalyzeNode("div", "3", "<div>3</div>", node, false, true, 1, 0, 1);
            node.Children.ElementAt(1).Styles.CheckKeyValuePair(0, "background-color", "red");
        }

        [Test]
        public void OverrideGlobalStyle()
        {
            string path = TestUtility.GetFolderPath("Html\\overrideglobalstyle.htm");
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

            IHtmlNode body = node;
            node = node.Children.ElementAt(0);

            while (node.Tag != "div")
                node = node.Next;

            TestUtility.AnalyzeNode(node, "div", "one", "<div class=\"cls\">one</div>", body, false, true, 1, 1, 1);
            node.CheckStyle(0, "color", "blue");
        }

        [Test]
        public void GlobalStyleChain()
        {
            string path = TestUtility.GetFolderPath("Html\\globalstylechain.htm");
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

            IHtmlNode body = node;
            node = node.Children.ElementAt(0);

            while (node.Tag != "div")
                node = node.Next;

            TestUtility.AnalyzeNode(node, "div", "one", "<div class=\"cls\">one</div>", body, false, true, 1, 1, 1);
            node.CheckStyle(0, "color", "gray");
        }

        [Test]
        public void IdentityOverClass()
        {
            string path = TestUtility.GetFolderPath("Html\\identityoverclass.htm");
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

            IHtmlNode body = node;
            node = node.Children.ElementAt(0);

            while (node.Tag != "div")
                node = node.Next;

            TestUtility.AnalyzeNode(node, "div", "one", "<div id=\"dv\" class=\"cls\">one</div>", body, false, true, 1, 2, 1);
            node.CheckStyle(0, "color", "red");

            node = node.Next;

            while (node.Tag != "div")
                node = node.Next;

            TestUtility.AnalyzeNode(node, "div", "two", "<div class=\"cls\">two</div>", body, false, true, 1, 1, 1);
            node.CheckStyle(0, "color", "blue");
        }

        [Test]
        public void ElementFirstChildChain()
        {
            string path = TestUtility.GetFolderPath("Html\\elementfirstchildchain.htm");
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

            IHtmlNode body = node;
            node = node.Children.ElementAt(0);

            while (node.Tag != "div")
                node = node.Next;

            TestUtility.AnalyzeNode(node, "div", "one", "<div>one</div>", body, false, true, 1, 0, 1);
            node.CheckStyle(0, "color", "red");

            node = node.Next;

            while (node.Tag != "div")
                node = node.Next;

            TestUtility.AnalyzeNode(node, "div", "<p>two</p>", "<div><p>two</p></div>", body, false, true, 1, 0, 0);

            IHtmlNode p = node.Children.ElementAt(0);
            TestUtility.AnalyzeNode(p, "p", "two", "<p>two</p>", node, false, true, 1, 0, 1);
            p.CheckStyle(0, "color", "blue");
        }

        [Test]
        public void ComplextCSSChain()
        {
            string path = TestUtility.GetFolderPath("Html\\complexcsschain.htm");
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

            while (node.Tag != "div")
                node = node.Next;

            IHtmlNode div = node;

            node = node.Children.ElementAt(0);

            while (node.Tag != "h2")
                node = node.Next;

            TestUtility.AnalyzeNode(node, "h2", "one", "<h2 class=\"heading\">one</h2>", div, false, true, 1, 1, 1);
            node.CheckStyle(0, "color", "red");
        }

        [Test]
        public void Level3Specificity()
        {
            string path = TestUtility.GetFolderPath("Html\\level3specificity.htm");
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

            int divCount = 0;

            while (divCount < 2)
            {
                node = node.Next;

                if (node.Tag == "div")
                    ++divCount;
            }

            IHtmlNode div = node;

            node = node.Children.ElementAt(0);

            while (node.Tag != "span")
                node = node.Next;

            TestUtility.AnalyzeNode(node, "span", "two", "<span class=\"text\">two</span>", div, false, true, 1, 1, 1);
            node.CheckStyle(0, "background-color", "transparent");
        }

        [Test]
        public void ALink()
        {
            string html = @"<style>a:link{color:#333;}</style><a href='http://google.com'>test</a>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Parse());
            parser.ParseStyles();

            IHtmlNode node = parser.Current.Next;

            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "a", "test", "<a href='http://google.com'>test</a>", null, false,true, 1, 1, 1);
        }

        [Test]
        public void InheritedFontSizeParse()
        {
            string path = TestUtility.GetFolderPath("Html\\inheritedfontsizeparse.htm");
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

            Assert.AreEqual(1, node.Styles.Count);
            node.CheckStyle(0, "font-size", "62.5%");

            node = node.Children.ElementAt(0);

            while (node.Tag != "article")
                node = node.Next;

            Assert.AreEqual(1, node.InheritedStyles.Count);
            node.CheckInheritedStyle(0, "font-size", "62.5%");

            node = node.Children.ElementAt(0);

            while (node.Tag != "p")
                node = node.Next;

            Assert.AreEqual(1, node.Styles.Count);
            node.CheckStyle(0, "font-size", "16px");
        }
    }
}
