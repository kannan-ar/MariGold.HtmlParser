namespace MariGold.HtmlParser.Tests
{
    using System;
    using NUnit.Framework;
    using MariGold.HtmlParser;

    [TestFixture]
    class InvalidOpenClose
    {
        [Test]
        public void DivP()
        {
            string html = "<div><p></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<p>", "<div><p></div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "p", "", "<p>");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void DivPSpanTextInvalidSpanOpen()
        {
            string html = "<div><p><span>test<span></p></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<p><span>test<span></p>", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "p", "<span>test<span>", "<p><span>test<span></p>");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(1, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(true, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0].Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0].Children[0], "span", "test<span>", "<span>test<span>");
            Assert.IsNotNull(parser.Current.Children[0].Children[0].Parent);
            Assert.AreEqual(parser.Current.Children[0], parser.Current.Children[0].Children[0].Parent);
            Assert.AreEqual(2, parser.Current.Children[0].Children[0].Children.Count);
            Assert.AreEqual(true, parser.Current.Children[0].Children[0].HasChildren);
            Assert.AreEqual(false, parser.Current.Children[0].Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Children[0].Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0].Children[0].Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0].Children[0].Children[0], "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children[0].Children[0].Children[0].Parent);
            Assert.AreEqual(parser.Current.Children[0].Children[0],
                parser.Current.Children[0].Children[0].Children[0].Parent);
            Assert.AreEqual(0, parser.Current.Children[0].Children[0].Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].Children[0].Children[0].HasChildren);
            Assert.AreEqual(false, parser.Current.Children[0].Children[0].Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Children[0].Children[0].Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0].Children[0].Children[1]);
            TestUtility.AreEqual(parser.Current.Children[0].Children[0].Children[1], "span", "", "<span>");
            Assert.IsNotNull(parser.Current.Children[0].Children[0].Children[1].Parent);
            Assert.AreEqual(parser.Current.Children[0].Children[0],
                parser.Current.Children[0].Children[0].Children[1].Parent);
            Assert.AreEqual(0, parser.Current.Children[0].Children[0].Children[1].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].Children[0].Children[1].HasChildren);
            Assert.AreEqual(false, parser.Current.Children[0].Children[0].Children[1].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Children[0].Children[1].Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }
        
        [Test]
        public void DivInvalidSpanClose()
        {
            string html = "<div>test</span></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "test</span>", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void DivInvalidTwoSpanClose()
        {
            string html = "<div>test</span></span></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "test</span></span>", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void DivInvalidTwoSpanCloseInBetweenText()
        {
            string html = "<div>test</span>ano</span></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "test</span>ano</span>", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(2, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[1]);
            TestUtility.AreEqual(parser.Current.Children[1], "#text", "ano", "ano");
            Assert.IsNotNull(parser.Current.Children[1].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[1].Parent);
            Assert.AreEqual(0, parser.Current.Children[1].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[1].HasChildren);
            Assert.AreEqual(false, parser.Current.Children[1].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[1].Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void DivPInvalidTwoSpanCloseInBetweenText()
        {
            string html = "<div><p>test</span>ano</span></p></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<p>test</span>ano</span></p>", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "p", "test</span>ano</span>", "<p>test</span>ano</span></p>");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(2, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(true, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0].Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0].Children[0], "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children[0].Children[0].Parent);
            Assert.AreEqual(parser.Current.Children[0], parser.Current.Children[0].Children[0].Parent);
            Assert.AreEqual(0, parser.Current.Children[0].Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].Children[0].HasChildren);
            Assert.AreEqual(false, parser.Current.Children[0].Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Children[0].Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0].Children[1]);
            TestUtility.AreEqual(parser.Current.Children[0].Children[1], "#text", "ano", "ano");
            Assert.IsNotNull(parser.Current.Children[0].Children[1].Parent);
            Assert.AreEqual(parser.Current.Children[0], parser.Current.Children[0].Children[1].Parent);
            Assert.AreEqual(0, parser.Current.Children[0].Children[1].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].Children[1].HasChildren);
            Assert.AreEqual(false, parser.Current.Children[0].Children[1].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Children[1].Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void DivInvalidPSpanTextInvalidSpanClose()
        {
            string html = "<div><p><span>test</span></span></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());

            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<p><span>test</span></span>", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "p", "<span>test</span></span>", 
                "<p><span>test</span></span>");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(1, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(true, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0].Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0].Children[0], "span", "test", "<span>test</span>");
            Assert.IsNotNull(parser.Current.Children[0].Children[0].Parent);
            Assert.AreEqual(parser.Current.Children[0], parser.Current.Children[0].Children[0].Parent);
            Assert.AreEqual(1, parser.Current.Children[0].Children[0].Children.Count);
            Assert.AreEqual(true, parser.Current.Children[0].Children[0].HasChildren);
            Assert.AreEqual(false, parser.Current.Children[0].Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Children[0].Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0].Children[0].Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0].Children[0].Children[0], "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children[0].Children[0].Children[0].Parent);
            Assert.AreEqual(parser.Current.Children[0].Children[0], 
                parser.Current.Children[0].Children[0].Children[0].Parent);
            Assert.AreEqual(0, parser.Current.Children[0].Children[0].Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].Children[0].Children[0].HasChildren);
            Assert.AreEqual(false, parser.Current.Children[0].Children[0].Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Children[0].Children[0].Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void OrphanDivOpenAndPInsideText()
        {
            string html = "<div><p>d</p>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<p>d</p>", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "p", "d", "<p>d</p>");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(1, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(true, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0].Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0].Children[0], "#text", "d", "d");
            Assert.IsNotNull(parser.Current.Children[0].Children[0].Parent);
            Assert.AreEqual(parser.Current.Children[0], parser.Current.Children[0].Children[0].Parent);
            Assert.AreEqual(0, parser.Current.Children[0].Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].Children[0].HasChildren);
            Assert.AreEqual(false, parser.Current.Children[0].Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Children[0].Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void PInvalidDivClose()
        {
            string html = "<p>d</p></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());

            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "p", "d", "<p>d</p>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "#text", "d", "d");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void PAndTextAndInvalidDivClose()
        {
            string html = "<p>d</p>test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());

            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "p", "d", "<p>d</p>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "#text", "d", "d");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Attributes.Count);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#text", "test", "test");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void InvalidOpenDivPThenText()
        {
            string html = "<div><p>test";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<p>test", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "p", "test", "<p>test");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(true, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(1, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0].Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0].Children[0], "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children[0].Children[0].Parent);
            Assert.AreEqual(parser.Current.Children[0], parser.Current.Children[0].Children[0].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].Children[0].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[0].Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Children[0].Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void InvalidOpenDivPThenTextAndInvalidAClose()
        {
            string html = "<div><p>test</a>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());

            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<p>test</a>", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "p", "test</a>", "<p>test</a>");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(true, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(1, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0].Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0].Children[0], "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children[0].Children[0].Parent);
            Assert.AreEqual(parser.Current.Children[0], parser.Current.Children[0].Children[0].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].Children[0].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[0].Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Children[0].Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void InvalidOpenDivPThenTextAndInvalidACloseAndText()
        {
            string html = "<div><p>test</a>ad";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());

            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<p>test</a>ad", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "p", "test</a>ad", "<p>test</a>ad");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(true, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(2, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0].Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0].Children[0], "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children[0].Children[0].Parent);
            Assert.AreEqual(parser.Current.Children[0], parser.Current.Children[0].Children[0].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].Children[0].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[0].Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Children[0].Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0].Children[1]);
            TestUtility.AreEqual(parser.Current.Children[0].Children[1], "#text", "ad", "ad");
            Assert.IsNotNull(parser.Current.Children[0].Children[1].Parent);
            Assert.AreEqual(parser.Current.Children[0], parser.Current.Children[0].Children[1].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].Children[1].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[0].Children[1].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].Children[1].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Children[1].Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void TextInvalidPCloseAndTwoSpanOpenText()
        {
            string html = "a</p><span>test<span>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#text", "a", "a");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "span", "test<span>", "<span>test<span>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(2, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[1]);
            TestUtility.AreEqual(parser.Current.Children[1], "span", "", "<span>");
            Assert.IsNotNull(parser.Current.Children[1].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[1].Parent);
            Assert.AreEqual(false, parser.Current.Children[1].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[1].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[1].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[1].Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void TextInvalidPCloseAndSpanWithText()
        {
            string html = "a</p><span>test</span>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#text", "a", "a");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "span", "test", "<span>test</span>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }
    }
}
