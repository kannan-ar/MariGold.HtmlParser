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

            if (parser.Traverse())
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

            Assert.IsTrue(parser.Traverse());

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

        [Test]
        public void BasicClassTagStyle()
        {
            string html = "<html><style>.cls{font:verdana,arial;color:#000}</style><div class='cls'>test</div></html>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Traverse());

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

        [Test]
        public void DivIdGreatherThanP()
        {
            string html = @"<style>
                                #dv > p
                                {
                                    color:#fff;
                                }
                            </style>
                            <div id='dv'>
                                <p>test</p>
                            </div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());

            parser.ParseCSS();

            HtmlNode node = parser.Current;

            while (node.Tag != "div")
                node = node.Next;

            foreach (HtmlNode child in node.Children)
            {
                if (child.Tag == "p")
                {
                    Assert.AreEqual(1, child.Styles.Count);
                    TestUtility.CheckKeyValuePair(child.Styles.ElementAt(0), "color", "#fff");
                    break;
                }
            }
        }

        [Test]
        public void DivIdSpaceP()
        {
            string html = @"<style>
                                #dv p
                                {
                                    color:#fff;
                                }
                            </style>
                            <div id='dv'>
                                <a href='#'>link</a>
                                <p>test</p>
                                <art>
                                    <p>one</p>
                                </art>
                            </div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());

            parser.ParseCSS();

            HtmlNode node = parser.Current;

            while (node.Tag != "div")
                node = node.Next;

            bool aTagFound = false;
            bool pTagFound = false;
            bool artTagFound = false;
            bool innerPTagFound = false;

            foreach (HtmlNode child in node.Children)
            {
                if (child.Tag == "a")
                {
                    aTagFound = true;
                    Assert.AreEqual(0, child.Styles.Count);
                }
                else if (child.Tag == "p")
                {
                    pTagFound = true;
                    Assert.AreEqual(1, child.Styles.Count);
                    TestUtility.CheckKeyValuePair(child.Styles.ElementAt(0), "color", "#fff");
                }
                else if (child.Tag == "art")
                {
                    artTagFound = true;

                    foreach (HtmlNode child1 in child.Children)
                    {
                        if (child1.Tag == "p")
                        {
                            innerPTagFound = true;
                            Assert.AreEqual(1, child1.Styles.Count);
                            TestUtility.CheckKeyValuePair(child1.Styles.ElementAt(0),
                                "color", "#fff");
                        }
                    }
                }
            }

            Assert.IsTrue(aTagFound);
            Assert.IsTrue(pTagFound);
            Assert.IsTrue(artTagFound);
            Assert.IsTrue(innerPTagFound);
        }

        [Test]
        public void DivImmediateP()
        {
            string html = @"<style>
                                #dv + p
                                {
                                    color:#fff;
                                }
                            </style>
                            <div id='dv'></div>
                            <p>one</p>
                            <p>two</p>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            Assert.IsNotNull(parser.Current);

            HtmlNode temp = parser.Current;
            bool divFound = false;

            while (temp != null)
            {
                if (temp.Tag == "div")
                {
                    divFound = true;

                    TestUtility.AreEqual(temp, "div", "", "<div id='dv'></div>");

                    while (temp != null && (temp.Tag == "#text" || temp.Tag == "div"))
                        temp = temp.Next;

                    Assert.IsNotNull(temp);
                    TestUtility.AreEqual(temp, "p", "one", "<p>one</p>");
                    Assert.AreEqual(1, temp.Styles.Count);
                    TestUtility.CheckKeyValuePair(temp.Styles.ElementAt(0),
                        "color", "#fff");

                    temp = temp.Next;

                    while (temp != null && temp.Tag == "#text")
                        temp = temp.Next;

                    Assert.IsNotNull(temp);
                    TestUtility.AreEqual(temp, "p", "two", "<p>two</p>");
                    Assert.AreEqual(0, temp.Styles.Count);

                    break;
                }

                temp = temp.Next;
            }

            Assert.IsTrue(divFound);
        }

        [Test]
        public void DivAllNextP()
        {
            string html = @"<style>
                                #dv ~ p
                                {
                                    color:#fff;
                                }
                            </style>
                            <div id='dv'></div>
                            <p>one</p>
                            <a href='#'>tt</a>
                            <p>two</p>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.IsTrue(parser.Parse());
            parser.ParseCSS();

            Assert.IsNotNull(parser.Current);

            HtmlNode temp = parser.Current;
            bool divFound = false;

            while (temp != null)
            {
                if (temp.Tag == "div")
                {
                    divFound = true;

                    TestUtility.AreEqual(temp, "div", "", "<div id='dv'></div>");

                    while (temp != null && (temp.Tag == "#text" || temp.Tag == "div"))
                        temp = temp.Next;

                    Assert.IsNotNull(temp);
                    TestUtility.AreEqual(temp, "p", "one", "<p>one</p>");
                    Assert.AreEqual(1, temp.Styles.Count);
                    TestUtility.CheckKeyValuePair(temp.Styles.ElementAt(0),
                        "color", "#fff");

                    temp = temp.Next;

                    while (temp != null && temp.Tag == "#text")
                        temp = temp.Next;


                    Assert.IsNotNull(temp);
                    TestUtility.AreEqual(temp, "a", "tt", "<a href='#'>tt</a>");
                    Assert.AreEqual(0, temp.Styles.Count);

                    temp = temp.Next;

                    while (temp != null && temp.Tag == "#text")
                        temp = temp.Next;

                    Assert.IsNotNull(temp);
                    TestUtility.AreEqual(temp, "p", "two", "<p>two</p>");
                    Assert.AreEqual(1, temp.Styles.Count);
                    TestUtility.CheckKeyValuePair(temp.Styles.ElementAt(0),
                        "color", "#fff");

                    break;
                }

                temp = temp.Next;
            }

            Assert.IsTrue(divFound);
        }
    }
}
