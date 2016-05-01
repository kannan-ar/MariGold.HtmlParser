namespace MariGold.HtmlParser.Tests
{
    using System;
	using System.Linq;
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
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "", "<p>");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);

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
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "<span>test<span>", "<p><span>test<span></p>");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(1, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(true, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "span", "test<span>", "<span>test<span>");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(2, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.Count());
            Assert.AreEqual(true, parser.Current.Children.ElementAt(0).Children.ElementAt(0).HasChildren);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(0), "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0),
                parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(0).HasChildren);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(0).Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(1), "span", "", "<span>");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(1).Parent);
            Assert.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0),
                parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(1).Parent);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(1).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(1).HasChildren);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(1).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(1).Attributes.Count);

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
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);

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
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);

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
            Assert.AreEqual(2, parser.Current.Children.Count());
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "#text", "ano", "ano");
            Assert.IsNotNull(parser.Current.Children.ElementAt(1).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(1).Parent);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(1).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(1).HasChildren);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(1).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(1).Attributes.Count);

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
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "test</span>ano</span>", "<p>test</span>ano</span></p>");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(2, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(true, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).HasChildren);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(1), "#text", "ano", "ano");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(1).Parent);
            Assert.AreEqual(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(1).Parent);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(1).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(1).HasChildren);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(1).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(1).Attributes.Count);

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
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "<span>test</span></span>", 
                "<p><span>test</span></span>");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(1, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(true, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "span", "test", "<span>test</span>");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(1, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.Count());
            Assert.AreEqual(true, parser.Current.Children.ElementAt(0).Children.ElementAt(0).HasChildren);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(0), "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), 
                parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(0).HasChildren);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.ElementAt(0).Attributes.Count);

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
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "d", "<p>d</p>");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(1, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(true, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "#text", "d", "d");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).HasChildren);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Attributes.Count);

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
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "d", "d");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);

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
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "d", "d");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#text", "test", "test");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(0, parser.Current.Children.Count());
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
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "test", "<p>test");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(true, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(1, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).HasChildren);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Attributes.Count);

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
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "test</a>", "<p>test</a>");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(true, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(1, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).HasChildren);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Attributes.Count);

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
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "test</a>ad", "<p>test</a>ad");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(true, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(2, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).HasChildren);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(1), "#text", "ad", "ad");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(1).Parent);
            Assert.AreEqual(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(1).Parent);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(1).HasChildren);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(1).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(1).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(1).Attributes.Count);

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
            Assert.AreEqual(0, parser.Current.Children.Count());
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "span", "test<span>", "<span>test<span>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(2, parser.Current.Children.Count());
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "span", "", "<span>");
            Assert.IsNotNull(parser.Current.Children.ElementAt(1).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(1).Parent);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(1).HasChildren);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(1).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(1).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(1).Attributes.Count);

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
            Assert.AreEqual(0, parser.Current.Children.Count());
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "span", "test", "<span>test</span>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void InvalidOpenCloseWithNextTag()
        {
            string html = "<div><span></div></span><div>test</div>";

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();

            Assert.IsNotNull(parser.Current);
            TestUtility.AnalyzeNode(parser.Current, "div", "<span>", "<div><span></div>", null, false, true, 1, 0);
            Assert.IsNotNull(parser.Current.Next);
            TestUtility.AnalyzeNode(parser.Current.Next, "div", "test", "<div>test</div>", null, false, true, 1, 0);
            Assert.IsNotNull(parser.Current.Next.Previous);
            Assert.AreEqual(parser.Current, parser.Current.Next.Previous);
        }

        [Test]
        public void InvalidOpenWithNextTag()
        {
            string html = "<a><b></a><c>c1</c>";

            HtmlParser parser = new HtmlTextParser(html);
            
            parser.Parse();

            Assert.IsNotNull(parser.Current);
            TestUtility.AnalyzeNode(parser.Current, "a", "<b>", "<a><b></a>", null, false, true, 1, 0);

            Assert.IsNotNull(parser.Current.Next);
            TestUtility.AnalyzeNode(parser.Current.Next, "c", "c1", "<c>c1</c>", null, false, true, 1, 0);
            Assert.IsNotNull(parser.Current.Next.Previous);
            Assert.AreEqual(parser.Current, parser.Current.Next.Previous);
        }

        [Test]
        public void InvalidOpenWithNextTagAndText()
        {
            string html = "<a><b>test</a><c>c1</c>";

            HtmlParser parser = new HtmlTextParser(html);

            parser.Parse();

            Assert.IsNotNull(parser.Current);
            TestUtility.AnalyzeNode(parser.Current, "a", "<b>test", "<a><b>test</a>", null, false, true, 1, 0);
            TestUtility.AnalyzeNode(parser.Current.Children.ElementAt(0), "b", "test", "<b>test", parser.Current, false, true, 1, 0);

            Assert.IsNotNull(parser.Current.Next);
            TestUtility.AnalyzeNode(parser.Current.Next, "c", "c1", "<c>c1</c>", null, false, true, 1, 0);
            Assert.IsNotNull(parser.Current.Next.Previous);
            Assert.AreEqual(parser.Current, parser.Current.Next.Previous);
        }

        [Test]
        public void InvalidCloseWithNextTag()
        {
            string html = "<a></a></b><c></c>";

            HtmlParser parser = new HtmlTextParser(html);

            parser.Parse();

            Assert.IsNotNull(parser.Current);
            TestUtility.AnalyzeNode(parser.Current, "a", "", "<a></a>", null, false, false, 0, 0);

            Assert.IsNotNull(parser.Current.Next);
            TestUtility.AnalyzeNode(parser.Current.Next, "c", "", "<c></c>", null, false, false, 0, 0);
            Assert.IsNotNull(parser.Current.Next.Previous);
            Assert.AreEqual(parser.Current, parser.Current.Next.Previous);
        }

        [Test]
        public void InvalidCloseWithNextTagInvalidOpen()
        {
            string html = "<a></a></b><c><d></c>";

            HtmlParser parser = new HtmlTextParser(html);

            parser.Parse();

            Assert.IsNotNull(parser.Current);
            TestUtility.AnalyzeNode(parser.Current, "a", "", "<a></a>", null, false, false, 0, 0);

            Assert.IsNotNull(parser.Current.Next);
            TestUtility.AnalyzeNode(parser.Current.Next, "c", "<d>", "<c><d></c>", null, false, true, 1, 0);
            Assert.IsNotNull(parser.Current.Next.Previous);
            Assert.AreEqual(parser.Current, parser.Current.Next.Previous);
        }

        [Test]
        public void InvalidCloseWithNextTagInvalidClose()
        {
            string html = "<a></a></b><c></d></c>";

            HtmlParser parser = new HtmlTextParser(html);

            parser.Parse();

            Assert.IsNotNull(parser.Current);
            TestUtility.AnalyzeNode(parser.Current, "a", "", "<a></a>", null, false, false, 0, 0);

            Assert.IsNotNull(parser.Current.Next);
            TestUtility.AnalyzeNode(parser.Current.Next, "c", "</d>", "<c></d></c>", null, false, false, 0, 0);
            Assert.IsNotNull(parser.Current.Next.Previous);
            Assert.AreEqual(parser.Current, parser.Current.Next.Previous);
        }

        [Test]
        public void InvalidOpenCloseWithNextTagHavingParent()
        {
            string html = "<p><div><span></div></span><div>test</div></p>";

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();

            Assert.IsNotNull(parser.Current);
            TestUtility.AnalyzeNode(parser.Current, "p", "<div><span></div></span><div>test</div>", html, null, false, true, 2, 0);

            IHtmlNode node = parser.Current.Children.ElementAt(0);

            Assert.IsNotNull(node);
            TestUtility.AnalyzeNode(node, "div", "<span>", "<div><span></div>", parser.Current, false, true, 1, 0);
            Assert.IsNotNull(node.Next);
            TestUtility.AnalyzeNode(node.Next, "div", "test", "<div>test</div>", parser.Current, false, true, 1, 0);
            Assert.IsNotNull(node.Next.Previous);
            Assert.AreEqual(node, node.Next.Previous);
        }
    }
}
