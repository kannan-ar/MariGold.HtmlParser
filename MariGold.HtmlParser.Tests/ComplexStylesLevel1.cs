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
        public void DivClassAllNextP()
        {
            string html = @"<style>
                                div.cls~p
                                {
                                	font-weight:bold;
                                }
                            </style>
                            <div class='cls'><p>1</p></div>
							test
							<p>one</p>
							<span>two</span>
							<p>three</p>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            bool divFound = false;
            bool spanFound = false;
            int pCount = 0;

            while (node != null)
            {
                if (node.Tag == "div")
                {
                    divFound = true;
                    TestUtility.AnalyzeNode(node, "div", "<p>1</p>", "<div class='cls'><p>1</p></div>", null, false, true, 1, 1, 0);
                    TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");

                    TestUtility.AnalyzeNode(node.Children.ElementAt(0), "p", "1", "<p>1</p>", node, false, true, 1, 0, 0);
                }

                if (node.Tag == "span")
                {
                    spanFound = true;
                    TestUtility.AnalyzeNode(node, "span", "two", "<span>two</span>", null, false, true, 1, 0, 0);
                }

                if (node.Tag == "p")
                {
                    ++pCount;

                    Assert.AreEqual(1, node.Styles.Count);
                    TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "font-weight", "bold");
                }

                node = node.Next;
            }

            if (!divFound)
            {
                throw new Exception("div tag not found");
            }

            if (!spanFound)
            {
                throw new Exception("span tag not found");
            }

            if (pCount != 2)
            {
                throw new Exception("mismatch in p count");
            }
        }

        [Test]
        public void DivClassImmediateP()
        {
            string html = @"<style>
                                div.cls > p
                                {
                                	font-weight:bold;
                                }
                            </style>
                            <div class='cls'><p>1</p></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "div", "<p>1</p>", "<div class='cls'><p>1</p></div>", null, false, true, 1, 1, 0);
            TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");

            TestUtility.AnalyzeNode(node.Children.ElementAt(0), "p", "1", "<p>1</p>", node, false, true, 1, 0, 1);
            TestUtility.CheckKeyValuePair(node.Children.ElementAt(0).Styles.ElementAt(0), "font-weight", "bold");
        }

        [Test]
        public void DivClassFirstChild()
        {
            string html = @"<style>
                                div.cls:first-child
                                {
                                	font-weight:bold;
                                }
                            </style>
                            <div><div class='cls'>1</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "div", "<div class='cls'>1</div>", "<div><div class='cls'>1</div></div>", null, false, true, 1, 0, 0);

            TestUtility.AnalyzeNode(node.Children.ElementAt(0), "div", "1", "<div class='cls'>1</div>", node, false, true, 1, 1, 1);
            TestUtility.CheckKeyValuePair(node.Children.ElementAt(0).Styles.ElementAt(0), "font-weight", "bold");
            TestUtility.CheckKeyValuePair(node.Children.ElementAt(0).Attributes.ElementAt(0), "class", "cls");
        }

        [Test]
        public void DivClassAttributeFirstChild()
        {
            string html = @"<style>
                                div[class='cls']:first-child
                                {
                                	font-weight:bold;
                                }
                            </style>
                            <div><div class='cls'>1</div><div>2</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "div", "<div class='cls'>1</div><div>2</div>", "<div><div class='cls'>1</div><div>2</div></div>", null, false, true, 2, 0, 0);

            TestUtility.AnalyzeNode(node.Children.ElementAt(0), "div", "1", "<div class='cls'>1</div>", node, false, true, 1, 1, 1);
            TestUtility.CheckKeyValuePair(node.Children.ElementAt(0).Attributes.ElementAt(0), "class", "cls");
            TestUtility.CheckKeyValuePair(node.Children.ElementAt(0).Styles.ElementAt(0), "font-weight", "bold");

            TestUtility.AnalyzeNode(node.Children.ElementAt(1), "div", "2", "<div>2</div>", node, false, true, 1, 0, 0);
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
                            <div attr></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

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

            TestUtility.AnalyzeNode(node, "div", "", "<div attr></div>", null, false, false, 0, 1, 1);
            TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "");
            TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "font-weight", "bold");

            Assert.IsNull(node.Next);
        }

        [Test]
        public void CustomAttributeWithValueOnly()
        {
            string html = @"<style>
                                [attr='val']
                                {
                                	font-weight:bold;
                                }
                            </style>
                            <div attr='val'>one</div>
                            <div attr></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "div", "one", "<div attr='val'>one</div>", null, false, true, 1, 1, 1);
            TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "val");
            TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "font-weight", "bold");

            node = node.Next;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "div", "", "<div attr></div>", null, false, false, 0, 1, 0);
            TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "");

            Assert.IsNull(node.Next);
        }

        [Test]
        public void DivGlobalStyle()
        {
            string html = @"<style>
								div *
								{
									background-color:red;
								}
							</style>
							<div><p>one</p><hr/></div>
							<p>three</p>
							<span>four</span>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "div", "<p>one</p><hr/>", "<div><p>one</p><hr/></div>", null, false,
                true, 2, 0, 0);

            TestUtility.AnalyzeNode(node.Children.ElementAt(0), "p", "one", "<p>one</p>", node, false, true, 1, 0, 1);
            TestUtility.CheckKeyValuePair(node.Children.ElementAt(0).Styles.ElementAt(0), "background-color", "red");

            TestUtility.AnalyzeNode(node.Children.ElementAt(1), "hr", "<hr/>", "<hr/>", node, true, false, 0, 0, 1);
            TestUtility.CheckKeyValuePair(node.Children.ElementAt(1).Styles.ElementAt(0), "background-color", "red");

            while (node.Tag != "p")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "p", "three", "<p>three</p>", null, false, true, 1, 0, 0);

            while (node.Tag != "span")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "span", "four", "<span>four</span>", null, false, true, 1, 0, 0);
        }

        [Test]
        public void IdentityLastChildP()
        {
            string html = @"<style>
								#dv:last-child p
								{
									background-color:red;
								}
							</style>
							<body><div id='dv'><div><p>three</p><span>two</span></div></div></body>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "body")
            {
                node = node.Next;
            }

            IHtmlNode body = node;

            node = node.Children.ElementAt(0);

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "div", "<div><p>three</p><span>two</span></div>", "<div id='dv'><div><p>three</p><span>two</span></div></div>",
                body, false, true, 1, 1, 0);
            TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "id", "dv");

            IHtmlNode parent = node;

            node = node.Children.ElementAt(0);

            TestUtility.AnalyzeNode(node, "div", "<p>three</p><span>two</span>", "<div><p>three</p><span>two</span></div>", parent, false, true, 2, 0, 0);

            TestUtility.AnalyzeNode(node.Children.ElementAt(0), "p", "three", "<p>three</p>", node, false, true, 1, 0, 1);
            TestUtility.CheckKeyValuePair(node.Children.ElementAt(0).Styles.ElementAt(0), "background-color", "red");

            TestUtility.AnalyzeNode(node.Children.ElementAt(1), "span", "two", "<span>two</span>", node, false, true, 1, 0, 0);
        }

        [Test]
        public void AttributeImmediateChildrenClass()
        {
            string html = @"<style>
								[attr] > .cls
								{
									background-color:red;
								}
							</style>
							<div attr><div class='cls'>one</div><div>two</div></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "div", "<div class='cls'>one</div><div>two</div>", "<div attr><div class='cls'>one</div><div>two</div></div>",
                null, false, true, 2, 1, 0);
            TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "");

            TestUtility.AnalyzeNode(node.Children.ElementAt(0), "div", "one", "<div class='cls'>one</div>", node, false, true, 1, 1, 1);
            TestUtility.CheckKeyValuePair(node.Children.ElementAt(0).Attributes.ElementAt(0), "class", "cls");
            TestUtility.CheckKeyValuePair(node.Children.ElementAt(0).Styles.ElementAt(0), "background-color", "red");

            TestUtility.AnalyzeNode(node.Children.ElementAt(1), "div", "two", "<div>two</div>", node, false, true, 1, 0, 0);

        }

        [Test]
        public void PDivSpan()
        {
            string html = @"<style>
								p, span
								{
									background-color:red;
								}
							</style>
							<p>1</p><div>2</div><span>3</span>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "p")
            {
                node = node.Next;
            }

            node.AnalyzeNode("p", "1", "<p>1</p>", null, false, true, 1, 0, 1);
            node.Styles.CheckKeyValuePair(0, "background-color", "red");

            node = node.Next;

            node.AnalyzeNode("div", "2", "<div>2</div>", null, false, true, 1, 0, 0);

            node = node.Next;

            node.AnalyzeNode("span", "3", "<span>3</span>", null, false, true, 1, 0, 1);
            node.Styles.CheckKeyValuePair(0, "background-color", "red");
        }

        [Test]
        public void PDivWithImmediateChildrenSpan()
        {
            string html = @"<style>
								div > span, p
								{
									background-color:red;
								}
							</style>
							<p>1</p><div><span>2</span></div><span>3</span>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "p")
            {
                node = node.Next;
            }

            node.AnalyzeNode("p", "1", "<p>1</p>", null, false, true, 1, 0, 1);
            node.Styles.CheckKeyValuePair(0, "background-color", "red");

            node = node.Next;

            node.AnalyzeNode("div", "<span>2</span>", "<div><span>2</span></div>", null, false, true, 1, 0, 0);

            node.Children.ElementAt(0).AnalyzeNode("span", "2", "<span>2</span>", node, false, true, 1, 0, 1);
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(0, "background-color", "red");

            node = node.Next;

            node.AnalyzeNode("span", "3", "<span>3</span>", null, false, true, 1, 0, 0);
        }

        [Test]
        public void StylesFromDivAndSpan()
        {
            string html = @"<style>
								span
								{
								  font-family: arial;
								}
								
								div > span
								{
								  font-size:15pt;
								}
							</style>
							<div><span>test</span></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            node.AnalyzeNode("div", "<span>test</span>", "<div><span>test</span></div>", null, false, true, 1, 0, 0);

            node.Children.ElementAt(0).AnalyzeNode("span", "test", "<span>test</span>", node, false, true, 1, 0, 2);
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-size", "15pt");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(1, "font-family", "arial");
        }

        [Test]
        public void StylesFromDivAndSpanClass()
        {
            string html = @"<style>
								span
								{
								  font-family: arial;
								}
								
								div > span
								{
								  font-size:15pt;
								}
								
								.s
								{
									font-weight:bold;
								}
							</style>
							<div><span class='s'>test</span></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            node.AnalyzeNode("div", "<span class='s'>test</span>", "<div><span class='s'>test</span></div>", null, false, true, 1, 0, 0);

            node.Children.ElementAt(0).AnalyzeNode("span", "test", "<span class='s'>test</span>", node, false, true, 1, 1, 3);
            node.Children.ElementAt(0).Attributes.CheckKeyValuePair(0, "class", "s");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-size", "15pt");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(1, "font-family", "arial");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(2, "font-weight", "bold");
        }

        [Test]
        public void StylesFromDivClassAndSpanClass()
        {
            string html = @"<style>
								span
								{
								  font-family: arial;
								}
								
								div > span
								{
								  font-size:15pt;
								}
								
								.d > span
								{
									font-weight:normal !important;
								}
								
								.s
								{
									font-weight:bold;
								}
							</style>
							<div class='d'><span class='s'>test</span></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            node.AnalyzeNode("div", "<span class='s'>test</span>", "<div class='d'><span class='s'>test</span></div>",
                             null, false, true, 1, 1, 0);
            node.Attributes.CheckKeyValuePair(0, "class", "d");

            node.Children.ElementAt(0).AnalyzeNode("span", "test", "<span class='s'>test</span>", node, false, true, 1, 1, 3);
            node.Children.ElementAt(0).Attributes.CheckKeyValuePair(0, "class", "s");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-size", "15pt");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(1, "font-weight", "normal");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(2, "font-family", "arial");

        }

        [Test]
        public void StylesFromInlineDivClassAndSpanClass()
        {
            string html = @"<style>
								span
								{
								  font-family: arial;
								}
								
								div > span
								{
								  font-size:15pt;
								}
								
								.d > span
								{
									font-weight:normal !important;
								}
								
								.s
								{
									font-weight:bold;
								}
							</style>
							<div class='d'><span style='font-family:verdana' class='s'>test</span></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            node.AnalyzeNode("div", "<span style='font-family:verdana' class='s'>test</span>", "<div class='d'><span style='font-family:verdana' class='s'>test</span></div>",
                             null, false, true, 1, 1, 0);
            node.Attributes.CheckKeyValuePair(0, "class", "d");

            node.Children.ElementAt(0).AnalyzeNode("span", "test", "<span style='font-family:verdana' class='s'>test</span>", node, false, true, 1, 2, 3);
            node.Children.ElementAt(0).Attributes.CheckKeyValuePair(0, "style", "font-family:verdana");
            node.Children.ElementAt(0).Attributes.CheckKeyValuePair(1, "class", "s");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-size", "15pt");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(1, "font-weight", "normal");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(2, "font-family", "verdana");
        }

        [Test]
        public void StylesFromInlineDivClassAndSpanInlineClass()
        {
            string html = @"<style>
								span
								{
								  font-family: arial !important;
								}
								
								div > span
								{
								  font-size:15pt;
								}
								
								.d > span
								{
									font-weight:normal !important;
								}
								
								.s
								{
									font-weight:bold;
								}
							</style>
							<div class='d'><span style='font-family:verdana' class='s'>test</span></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseStyles();

            Assert.IsNotNull(parser.Current);

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            node.AnalyzeNode("div", "<span style='font-family:verdana' class='s'>test</span>", "<div class='d'><span style='font-family:verdana' class='s'>test</span></div>",
                             null, false, true, 1, 1, 0);
            node.Attributes.CheckKeyValuePair(0, "class", "d");

            node.Children.ElementAt(0).AnalyzeNode("span", "test", "<span style='font-family:verdana' class='s'>test</span>", node, false, true, 1, 2, 3);
            node.Children.ElementAt(0).Attributes.CheckKeyValuePair(0, "style", "font-family:verdana");
            node.Children.ElementAt(0).Attributes.CheckKeyValuePair(1, "class", "s");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(0, "font-size", "15pt");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(1, "font-weight", "normal");
            node.Children.ElementAt(0).Styles.CheckKeyValuePair(2, "font-family", "arial");
        }

        [Test]
        public void GlobalAndElementSelector()
        {
            string path = TestUtility.GetFolderPath("Html\\globalselectors.htm");
            string html = string.Empty;

            using (StreamReader sr = new StreamReader(path))
            {
                html = sr.ReadToEnd();
            }

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Parse());
            parser.ParseStyles();

            IHtmlNode node = parser.Current;

            Assert.IsNotNull(node);
            Assert.AreEqual(true, node.HasChildren);

            node = node.Children.ElementAt(0);

            while (node.Tag != "body")
            {
                node = node.Next;
            }

            IHtmlNode body = node;

            node = node.Children.ElementAt(0);

            while (node.Tag != "div")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "div", "div", "<div>div</div>", body, false, true, 1, 0, 1);
            node.CheckStyle(0, "color", "red");

            while (node.Tag != "p")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "p", "p", "<p>p</p>", body, false, true, 1, 0, 1);
            node.CheckStyle(0, "color", "blue");

            while (node.Tag != "span")
            {
                node = node.Next;
            }

            TestUtility.AnalyzeNode(node, "span", "span", "<span>span</span>", body, false, true, 1, 0, 1);
            node.CheckStyle(0, "color", "red");
        }
    }
}
