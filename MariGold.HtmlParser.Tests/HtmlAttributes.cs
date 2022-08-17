namespace MariGold.HtmlParser.Tests
{
    using MariGold.HtmlParser;
    using System.Linq;
    using Xunit;

    public class HtmlAttributes
    {
        [Fact]
        public void KeyOnly()
        {
            string html = "<div id></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id></div>");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);

            Assert.NotNull(parser.Current.Attributes);
            Assert.Single(parser.Current.Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "");

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void TwoKeyOnly()
        {
            string html = "<div id name></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id name></div>");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);

            Assert.NotNull(parser.Current.Attributes);
            Assert.Equal(2, parser.Current.Attributes.Count);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "");
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(1), "name", "");

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void KeyOnlyWithEqual()
        {
            string html = "<div id=></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id=></div>");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);

            Assert.NotNull(parser.Current.Attributes);
            Assert.Single(parser.Current.Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "");

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void KeyOnlyWithValueWithoutQuote()
        {
            string html = "<div id=1></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id=1></div>");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);

            Assert.NotNull(parser.Current.Attributes);
            Assert.Single(parser.Current.Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "1");

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void KeyOnlyWithValueAndSingleQuote()
        {
            string html = "<div id='1'></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id='1'></div>");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);

            Assert.NotNull(parser.Current.Attributes);
            Assert.Single(parser.Current.Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "1");

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void KeyOnlyWithValueAndDoubleQuote()
        {
            string html = "<div id=\"1\"></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id=\"1\"></div>");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);

            Assert.NotNull(parser.Current.Attributes);
            Assert.Single(parser.Current.Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "1");

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void KeyOnlyWithValueAndDoubleQuoteAndSpace()
        {
            string html = "<div id= \"1\" ></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id= \"1\" ></div>");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);

            Assert.NotNull(parser.Current.Attributes);
            Assert.Single(parser.Current.Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "1");

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void KeyOnlyWithMultiCharValueWithoutQuote()
        {
            string html = "<div id=abc></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id=abc></div>");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);

            Assert.NotNull(parser.Current.Attributes);
            Assert.Single(parser.Current.Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "abc");

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void KeyOnlyWithMultiCharValueWithoutQuoteAndWithSpace()
        {
            string html = "<div id = abc ></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id = abc ></div>");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);

            Assert.NotNull(parser.Current.Attributes);
            Assert.Single(parser.Current.Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "abc");

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void KeyOnlyWithMultiCharValueWithQuoteAndWithSpace()
        {
            string html = "<div id =  \" abc\" ></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id =  \" abc\" ></div>");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);

            Assert.NotNull(parser.Current.Attributes);
            Assert.Single(parser.Current.Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", " abc");

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void TwoAttributes()
        {
            string html = "<div id='abc' type = \"text\" ></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id='abc' type = \"text\" ></div>");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);

            Assert.NotNull(parser.Current.Attributes);
            Assert.Equal(2, parser.Current.Attributes.Count);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "abc");
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(1), "type", "text");

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void AttributeOnInnerElement()
        {
            string html = "<div id='abc' type = \"text\" ><span id=\"sp\"></span></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<span id=\"sp\"></span>", html);
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.SelfClosing);
            Assert.Single(parser.Current.Children);
            Assert.True(parser.Current.HasChildren);

            Assert.NotNull(parser.Current.Attributes);
            Assert.Equal(2, parser.Current.Attributes.Count);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "abc");
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(1), "type", "text");

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "span", "", "<span id=\"sp\"></span>");
            Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children);
            Assert.False(parser.Current.Children.ElementAt(0).HasChildren);

            Assert.NotNull(parser.Current.Children.ElementAt(0).Attributes);
            Assert.Single(parser.Current.Children.ElementAt(0).Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(0).Attributes.ElementAt(0), "id", "sp");

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void AttributeOnInnerElementWithoutQuote()
        {
            string html = "<div id='abc' type = \"text\" ><span id=sp></span></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<span id=sp></span>", html);
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.SelfClosing);
            Assert.Single(parser.Current.Children);
            Assert.True(parser.Current.HasChildren);

            Assert.NotNull(parser.Current.Attributes);
            Assert.Equal(2, parser.Current.Attributes.Count);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "abc");
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(1), "type", "text");

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "span", "", "<span id=sp></span>");
            Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children);
            Assert.False(parser.Current.Children.ElementAt(0).HasChildren);

            Assert.NotNull(parser.Current.Children.ElementAt(0).Attributes);
            Assert.Single(parser.Current.Children.ElementAt(0).Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(0).Attributes.ElementAt(0), "id", "sp");

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void ThreeLevelInnerAttributes()
        {
            string html = "<div id='abc' type = \"text\" ><p width=\"100px\"><span style=\"color:#fff\">test<span></p></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<p width=\"100px\"><span style=\"color:#fff\">test<span></p>", html);
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.SelfClosing);
            Assert.Single(parser.Current.Children);
            Assert.True(parser.Current.HasChildren);

            Assert.NotNull(parser.Current.Attributes);
            Assert.Equal(2, parser.Current.Attributes.Count);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "abc");
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(1), "type", "text");

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "<span style=\"color:#fff\">test<span>",
                "<p width=\"100px\"><span style=\"color:#fff\">test<span></p>");
            Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.Single(parser.Current.Children.ElementAt(0).Children);
            Assert.True(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);

            Assert.NotNull(parser.Current.Children.ElementAt(0).Attributes);
            Assert.Single(parser.Current.Children.ElementAt(0).Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(0).Attributes.ElementAt(0), "width", "100px");

            Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "span", "test<span>", "<span style=\"color:#fff\">test<span>");
            Assert.False(parser.Current.Children.ElementAt(0).Children.ElementAt(0).SelfClosing);
            Assert.Equal(2, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.Count());
            Assert.True(parser.Current.Children.ElementAt(0).Children.ElementAt(0).HasChildren);
            Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);

            Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Attributes);
            Assert.Single(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Attributes.ElementAt(0), "style", "color:#fff");

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void SingleQuoteInDoubleQuote()
        {
            string html = "<div style=\"font:'verdana arial'\">test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "test", html);
            Assert.Null(parser.Current.Parent);
            Assert.True(parser.Current.HasChildren);
            Assert.Single(parser.Current.Children);
            Assert.False(parser.Current.SelfClosing);
            Assert.Single(parser.Current.Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "style", "font:'verdana arial'");

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children);

        }

        [Fact]
        public void DoubleQuoteInSingleQuote()
        {
            string html = "<div style='font:\"verdana arial\"'>test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "test", html);
            Assert.Null(parser.Current.Parent);
            Assert.True(parser.Current.HasChildren);
            Assert.Single(parser.Current.Children);
            Assert.False(parser.Current.SelfClosing);
            Assert.Single(parser.Current.Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "style", "font:\"verdana arial\"");

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "test", "test");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children);

        }

        [Fact]
        public void DuplicateAttributes()
        {
            string html = "<div id='abc' id='abc'></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", html);
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);

            Assert.NotNull(parser.Current.Attributes);
            Assert.Single(parser.Current.Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "abc");

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void DuplicateAttributesWithZeroAttributeDiv()
        {
            string html = "<div id='abc' id='abc'></div><div>test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "", "<div id='abc' id='abc'></div>");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);

            Assert.NotNull(parser.Current.Attributes);
            Assert.Single(parser.Current.Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "abc");

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AnalyzeNode(parser.Current, "div", "test", "<div>test</div>", null, false, true, 1, 0);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void BracesInAttributeQuotes()
        {
            string html = "<a t=\"a<b>\">test</a>";

            HtmlParser parser = new HtmlTextParser(html);
            parser.Parse();
            Assert.NotNull(parser.Current);
            parser.Current.AnalyzeNode("a", "test", html, null, false, true, 1, 1, 0);
            parser.Current.Attributes.CheckKeyValuePair(0, "t", "a<b>");
        }
    }
}
