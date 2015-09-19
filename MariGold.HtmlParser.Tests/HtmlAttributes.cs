namespace MariGold.HtmlParser.Tests
{
    using System;
    using NUnit.Framework;
    using MariGold.HtmlParser;
    using System.Linq;

    [TestFixture]
    public class HtmlAttributes
    {
        [Test]
        public void KeyOnly()
        {
            string html = "<div id></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id></div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.HasChildren);

            Assert.IsNotNull(parser.Current.Attributes);
            Assert.AreEqual(1, parser.Current.Attributes.Count);
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(0), "id", "");

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void TwoKeyOnly()
        {
            string html = "<div id name></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id name></div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.HasChildren);

            Assert.IsNotNull(parser.Current.Attributes);
            Assert.AreEqual(2, parser.Current.Attributes.Count);
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(0), "id", "");
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(1), "name", "");

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void KeyOnlyWithEqual()
        {
            string html = "<div id=></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id=></div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.HasChildren);

            Assert.IsNotNull(parser.Current.Attributes);
            Assert.AreEqual(1, parser.Current.Attributes.Count);
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(0), "id", "");

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void KeyOnlyWithValueWithoutQuote()
        {
            string html = "<div id=1></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id=1></div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.HasChildren);

            Assert.IsNotNull(parser.Current.Attributes);
            Assert.AreEqual(1, parser.Current.Attributes.Count);
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(0), "id", "1");

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void KeyOnlyWithValueAndSingleQuote()
        {
            string html = "<div id='1'></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id='1'></div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.HasChildren);

            Assert.IsNotNull(parser.Current.Attributes);
            Assert.AreEqual(1, parser.Current.Attributes.Count);
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(0), "id", "1");

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void KeyOnlyWithValueAndDoubleQuote()
        {
            string html = "<div id=\"1\"></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id=\"1\"></div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.HasChildren);

            Assert.IsNotNull(parser.Current.Attributes);
            Assert.AreEqual(1, parser.Current.Attributes.Count);
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(0), "id", "1");

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void KeyOnlyWithValueAndDoubleQuoteAndSpace()
        {
            string html = "<div id= \"1\" ></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id= \"1\" ></div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.HasChildren);

            Assert.IsNotNull(parser.Current.Attributes);
            Assert.AreEqual(1, parser.Current.Attributes.Count);
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(0), "id", "1");

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void KeyOnlyWithMultiCharValueWithoutQuote()
        {
            string html = "<div id=abc></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id=abc></div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.HasChildren);

            Assert.IsNotNull(parser.Current.Attributes);
            Assert.AreEqual(1, parser.Current.Attributes.Count);
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(0), "id", "abc");

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void KeyOnlyWithMultiCharValueWithoutQuoteAndWithSpace()
        {
            string html = "<div id = abc ></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id = abc ></div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.HasChildren);

            Assert.IsNotNull(parser.Current.Attributes);
            Assert.AreEqual(1, parser.Current.Attributes.Count);
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(0), "id", "abc");

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void KeyOnlyWithMultiCharValueWithQuoteAndWithSpace()
        {
            string html = "<div id =  \" abc\" ></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id =  \" abc\" ></div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.HasChildren);

            Assert.IsNotNull(parser.Current.Attributes);
            Assert.AreEqual(1, parser.Current.Attributes.Count);
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(0), "id", " abc");

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void TwoAttributes()
        {
            string html = "<div id='abc' type = \"text\" ></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id='abc' type = \"text\" ></div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.HasChildren);

            Assert.IsNotNull(parser.Current.Attributes);
            Assert.AreEqual(2, parser.Current.Attributes.Count);
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(0), "id", "abc");
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(1), "type", "text");

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void AttributeOnInnerElement()
        {
            string html = "<div id='abc' type = \"text\" ><span id=\"sp\"></span></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<span id=\"sp\"></span>", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);

            Assert.IsNotNull(parser.Current.Attributes);
            Assert.AreEqual(2, parser.Current.Attributes.Count);
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(0), "id", "abc");
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(1), "type", "text");

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "span", "", "<span id=\"sp\"></span>");
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);

            Assert.IsNotNull(parser.Current.Children[0].Attributes);
            Assert.AreEqual(1, parser.Current.Children[0].Attributes.Count);
            TestUtility.CheckAttribute(parser.Current.Children[0].Attributes.ElementAt(0), "id", "sp");

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void AttributeOnInnerElementWithoutQuote()
        {
            string html = "<div id='abc' type = \"text\" ><span id=sp></span></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<span id=sp></span>", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);

            Assert.IsNotNull(parser.Current.Attributes);
            Assert.AreEqual(2, parser.Current.Attributes.Count);
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(0), "id", "abc");
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(1), "type", "text");

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "span", "", "<span id=sp></span>");
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);

            Assert.IsNotNull(parser.Current.Children[0].Attributes);
            Assert.AreEqual(1, parser.Current.Children[0].Attributes.Count);
            TestUtility.CheckAttribute(parser.Current.Children[0].Attributes.ElementAt(0), "id", "sp");

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void ThreeLevelInnerAttributes()
        {
            string html = "<div id='abc' type = \"text\" ><p width=\"100px\"><span style=\"color:#fff\">test<span></p></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<p width=\"100px\"><span style=\"color:#fff\">test<span></p>", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);

            Assert.IsNotNull(parser.Current.Attributes);
            Assert.AreEqual(2, parser.Current.Attributes.Count);
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(0), "id", "abc");
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(1), "type", "text");

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "p", "<span style=\"color:#fff\">test<span>",
                "<p width=\"100px\"><span style=\"color:#fff\">test<span></p>");
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
            Assert.AreEqual(1, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(true, parser.Current.Children[0].HasChildren);
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);

            Assert.IsNotNull(parser.Current.Children[0].Attributes);
            Assert.AreEqual(1, parser.Current.Children[0].Attributes.Count);
            TestUtility.CheckAttribute(parser.Current.Children[0].Attributes.ElementAt(0), "width", "100px");

            Assert.IsNotNull(parser.Current.Children[0].Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0].Children[0], "span", "test<span>", "<span style=\"color:#fff\">test<span>");
            Assert.AreEqual(false, parser.Current.Children[0].Children[0].SelfClosing);
            Assert.AreEqual(2, parser.Current.Children[0].Children[0].Children.Count);
            Assert.AreEqual(true, parser.Current.Children[0].Children[0].HasChildren);
            Assert.IsNotNull(parser.Current.Children[0].Children[0].Parent);
            Assert.AreEqual(parser.Current.Children[0], parser.Current.Children[0].Children[0].Parent);

            Assert.IsNotNull(parser.Current.Children[0].Children[0].Attributes);
            Assert.AreEqual(1, parser.Current.Children[0].Children[0].Attributes.Count);
            TestUtility.CheckAttribute(parser.Current.Children[0].Children[0].Attributes.ElementAt(0), "style", "color:#fff");

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void SingleQuoteInDoubleQuote()
        {
            string html = "<div style=\"font:'verdana arial'\">test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "test", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(1, parser.Current.Attributes.Count);
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(0), "style", "font:'verdana arial'");

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);

        }

        [Test]
        public void DoubleQuoteInSingleQuote()
        {
            string html = "<div style='font:\"verdana arial\"'>test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "test", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(1, parser.Current.Attributes.Count);
            TestUtility.CheckAttribute(parser.Current.Attributes.ElementAt(0), "style", "font:\"verdana arial\"");

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "#text", "test", "test");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);

        }
    }
}
