namespace MariGold.HtmlParser.Tests
{
    using MariGold.HtmlParser;
    using System.Linq;
    using Xunit;

    public class SelfClosing
    {
        [Fact]
        public void BrOnly()
        {
            string html = @"<br />";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br />", "<br />");
            Assert.Null(parser.Current.Parent);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);
            Assert.True(parser.Current.SelfClosing);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void BrOnlyNoSpace()
        {
            string html = @"<br/>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br/>", "<br/>");
            Assert.Null(parser.Current.Parent);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);
            Assert.True(parser.Current.SelfClosing);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void BrWithNoSpaceAndSpan()
        {
            string html = @"<br/><span>1</span>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br/>", "<br/>");
            Assert.Null(parser.Current.Parent);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);
            Assert.True(parser.Current.SelfClosing);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "span", "1", "<span>1</span>");
            Assert.Null(parser.Current.Parent);
            Assert.Single(parser.Current.Children);
            Assert.True(parser.Current.HasChildren);
            Assert.False(parser.Current.SelfClosing);

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "1", "1");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children);
            Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void TextAndBr()
        {
            string html = @"test<br />";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#text", "test", "test");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.HasChildren);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.SelfClosing);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br />", "<br />");
            Assert.Null(parser.Current.Parent);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);
            Assert.True(parser.Current.SelfClosing);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void TextWithRightPaddingAndBr()
        {
            string html = @"test <br />";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#text", "test ", "test ");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.HasChildren);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.SelfClosing);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br />", "<br />");
            Assert.Null(parser.Current.Parent);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);
            Assert.True(parser.Current.SelfClosing);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void BrAndText()
        {
            string html = @"<br />test";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br />", "<br />");
            Assert.Null(parser.Current.Parent);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);
            Assert.True(parser.Current.SelfClosing);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#text", "test", "test");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.HasChildren);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.SelfClosing);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void BrAndDiv()
        {
            string html = @"<br /><div>tt</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br />", "<br />");
            Assert.Null(parser.Current.Parent);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);
            Assert.True(parser.Current.SelfClosing);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "tt", "<div>tt</div>");
            Assert.Null(parser.Current.Parent);
            Assert.Single(parser.Current.Children);
            Assert.True(parser.Current.HasChildren);
            Assert.False(parser.Current.SelfClosing);

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "tt", "tt");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children);
            Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void DivAndBr()
        {
            string html = @"<div>tt</div><br />";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "tt", "<div>tt</div>");
            Assert.Null(parser.Current.Parent);
            Assert.Single(parser.Current.Children);
            Assert.True(parser.Current.HasChildren);
            Assert.False(parser.Current.SelfClosing);

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "tt", "tt");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children);
            Assert.False(parser.Current.SelfClosing);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br />", "<br />");
            Assert.Null(parser.Current.Parent);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.HasChildren);
            Assert.True(parser.Current.SelfClosing);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void BrInsideDiv()
        {
            string html = @"<div><br /></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<br />", "<div><br /></div>");
            Assert.Null(parser.Current.Parent);
            Assert.Single(parser.Current.Children);
            Assert.True(parser.Current.HasChildren);
            Assert.False(parser.Current.SelfClosing);

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "br", "<br />", "<br />");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children);
            Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.True(parser.Current.Children.ElementAt(0).SelfClosing);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void BrInsideDivAndText()
        {
            string html = @"<div>t1<br /></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "t1<br />", "<div>t1<br /></div>");
            Assert.Null(parser.Current.Parent);
            Assert.Equal(2, parser.Current.Children.Count());
            Assert.True(parser.Current.HasChildren);
            Assert.False(parser.Current.SelfClosing);

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "t1", "t1");
            Assert.Empty(parser.Current.Children.ElementAt(0).Children);
            Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);

            Assert.NotNull(parser.Current.Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "br", "<br />", "<br />");
            Assert.NotNull(parser.Current.Children.ElementAt(1).Parent);
            Assert.Empty(parser.Current.Children.ElementAt(1).Children);
            Assert.False(parser.Current.Children.ElementAt(1).HasChildren);
            Assert.True(parser.Current.Children.ElementAt(1).SelfClosing);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void TextInsideDivAndBr()
        {
            string html = @"<div><br />t1</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<br />t1", "<div><br />t1</div>");
            Assert.Null(parser.Current.Parent);
            Assert.Equal(2, parser.Current.Children.Count());
            Assert.True(parser.Current.HasChildren);
            Assert.False(parser.Current.SelfClosing);

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "br", "<br />", "<br />");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children);
            Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.True(parser.Current.Children.ElementAt(0).SelfClosing);

            Assert.NotNull(parser.Current.Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "#text", "t1", "t1");
            Assert.Empty(parser.Current.Children.ElementAt(1).Children);
            Assert.False(parser.Current.Children.ElementAt(1).HasChildren);
            Assert.NotNull(parser.Current.Children.ElementAt(1).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(1).Parent);
            Assert.False(parser.Current.Children.ElementAt(1).SelfClosing);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void BrInsideDivAndOneChar()
        {
            string html = @"<div>t<br /></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "t<br />", "<div>t<br /></div>");
            Assert.Null(parser.Current.Parent);
            Assert.Equal(2, parser.Current.Children.Count());
            Assert.True(parser.Current.HasChildren);
            Assert.False(parser.Current.SelfClosing);

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "t", "t");
            Assert.Empty(parser.Current.Children.ElementAt(0).Children);
            Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);

            Assert.NotNull(parser.Current.Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "br", "<br />", "<br />");
            Assert.NotNull(parser.Current.Children.ElementAt(1).Parent);
            Assert.Empty(parser.Current.Children.ElementAt(1).Children);
            Assert.False(parser.Current.Children.ElementAt(1).HasChildren);
            Assert.True(parser.Current.Children.ElementAt(1).SelfClosing);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void OneCharInsideDivAndBr()
        {
            string html = @"<div><br />t</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<br />t", "<div><br />t</div>");
            Assert.Null(parser.Current.Parent);
            Assert.Equal(2, parser.Current.Children.Count());
            Assert.True(parser.Current.HasChildren);
            Assert.False(parser.Current.SelfClosing);

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "br", "<br />", "<br />");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children);
            Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.True(parser.Current.Children.ElementAt(0).SelfClosing);

            Assert.NotNull(parser.Current.Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "#text", "t", "t");
            Assert.Empty(parser.Current.Children.ElementAt(1).Children);
            Assert.False(parser.Current.Children.ElementAt(1).HasChildren);
            Assert.NotNull(parser.Current.Children.ElementAt(1).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(1).Parent);
            Assert.False(parser.Current.Children.ElementAt(1).SelfClosing);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void BrInsideDivAndOneCharAndSpace()
        {
            string html = @"<div>t <br /></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "t <br />", "<div>t <br /></div>");
            Assert.Null(parser.Current.Parent);
            Assert.Equal(2, parser.Current.Children.Count());
            Assert.True(parser.Current.HasChildren);
            Assert.False(parser.Current.SelfClosing);

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", "t ", "t ");
            Assert.Empty(parser.Current.Children.ElementAt(0).Children);
            Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);

            Assert.NotNull(parser.Current.Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "br", "<br />", "<br />");
            Assert.NotNull(parser.Current.Children.ElementAt(1).Parent);
            Assert.Empty(parser.Current.Children.ElementAt(1).Children);
            Assert.False(parser.Current.Children.ElementAt(1).HasChildren);
            Assert.True(parser.Current.Children.ElementAt(1).SelfClosing);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void OneCharAndSpaceInsideDivAndBr()
        {
            string html = @"<div><br /> t</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<br /> t", "<div><br /> t</div>");
            Assert.Null(parser.Current.Parent);
            Assert.Equal(2, parser.Current.Children.Count());
            Assert.True(parser.Current.HasChildren);
            Assert.False(parser.Current.SelfClosing);

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "br", "<br />", "<br />");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children);
            Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.True(parser.Current.Children.ElementAt(0).SelfClosing);

            Assert.NotNull(parser.Current.Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "#text", " t", " t");
            Assert.Empty(parser.Current.Children.ElementAt(1).Children);
            Assert.False(parser.Current.Children.ElementAt(1).HasChildren);
            Assert.NotNull(parser.Current.Children.ElementAt(1).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(1).Parent);
            Assert.False(parser.Current.Children.ElementAt(1).SelfClosing);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void DivPBr()
        {
            string html = @"<div><p><br /></p></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<p><br /></p>", "<div><p><br /></p></div>");
            Assert.Null(parser.Current.Parent);
            Assert.Single(parser.Current.Children);
            Assert.True(parser.Current.HasChildren);
            Assert.False(parser.Current.SelfClosing);

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "p", "<br />", "<p><br /></p>");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.Single(parser.Current.Children.ElementAt(0).Children);
            Assert.True(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);

            Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "br", "<br />", "<br />");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children);
            Assert.False(parser.Current.Children.ElementAt(0).Children.ElementAt(0).HasChildren);
            Assert.True(parser.Current.Children.ElementAt(0).Children.ElementAt(0).SelfClosing);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void SpaceBr()
        {
            string html = " <br>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#text", " ", " ");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.HasChildren);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Attributes);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br>", "<br>");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.HasChildren);
            Assert.Empty(parser.Current.Children);
            Assert.True(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Attributes);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void BrSpace()
        {
            string html = "<br> ";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br>", "<br>");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.HasChildren);
            Assert.Empty(parser.Current.Children);
            Assert.True(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Attributes);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#text", " ", " ");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.HasChildren);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Attributes);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void ImgAndBrInsideDiv()
        {
            string html = "<div><img><br></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<img><br>", html);
            Assert.Null(parser.Current.Parent);
            Assert.True(parser.Current.HasChildren);
            Assert.Equal(2, parser.Current.Children.Count());
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Attributes);

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "img", "<img>", "<img>");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children);
            Assert.True(parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.Empty(parser.Current.Children.ElementAt(0).Attributes);

            Assert.NotNull(parser.Current.Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "br", "<br>", "<br>");
            Assert.NotNull(parser.Current.Children.ElementAt(1).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(1).Parent);
            Assert.False(parser.Current.Children.ElementAt(1).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(1).Children);
            Assert.True(parser.Current.Children.ElementAt(1).SelfClosing);
            Assert.Empty(parser.Current.Children.ElementAt(1).Attributes);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void ImgWithId()
        {
            string html = "<img id=\"img\">";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "img", "<img id=\"img\">", "<img id=\"img\">");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.HasChildren);
            Assert.Empty(parser.Current.Children);
            Assert.True(parser.Current.SelfClosing);
            Assert.Single(parser.Current.Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "img");

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void ImgWithIdAndSrc()
        {
            string html = "<img id=\"img\" src=\"http://google.com/1.jpg\">";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "img", html, html);
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.HasChildren);
            Assert.Empty(parser.Current.Children);
            Assert.True(parser.Current.SelfClosing);
            Assert.Equal(2, parser.Current.Attributes.Count);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "img");
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(1), "src", "http://google.com/1.jpg");

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void ImgAndBrWithIdInsideDiv()
        {
            string html = "<div id='dv'><img id='im'><br id='b'></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<img id='im'><br id='b'>", html);
            Assert.Null(parser.Current.Parent);
            Assert.True(parser.Current.HasChildren);
            Assert.Equal(2, parser.Current.Children.Count());
            Assert.False(parser.Current.SelfClosing);
            Assert.Single(parser.Current.Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "dv");

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "img", "<img id='im'>", "<img id='im'>");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children);
            Assert.True(parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.Single(parser.Current.Children.ElementAt(0).Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(0).Attributes.ElementAt(0), "id", "im");

            Assert.NotNull(parser.Current.Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "br", "<br id='b'>", "<br id='b'>");
            Assert.NotNull(parser.Current.Children.ElementAt(1).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(1).Parent);
            Assert.False(parser.Current.Children.ElementAt(1).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(1).Children);
            Assert.True(parser.Current.Children.ElementAt(1).SelfClosing);
            Assert.Single(parser.Current.Children.ElementAt(1).Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(1).Attributes.ElementAt(0), "id", "b");

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void ImgAndBrWithSelfCloseInsideDiv()
        {
            string html = "<div><img /><br /></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<img /><br />", html);
            Assert.Null(parser.Current.Parent);
            Assert.True(parser.Current.HasChildren);
            Assert.Equal(2, parser.Current.Children.Count());
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Attributes);

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "img", "<img />", "<img />");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children);
            Assert.True(parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.Empty(parser.Current.Children.ElementAt(0).Attributes);

            Assert.NotNull(parser.Current.Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "br", "<br />", "<br />");
            Assert.NotNull(parser.Current.Children.ElementAt(1).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(1).Parent);
            Assert.False(parser.Current.Children.ElementAt(1).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(1).Children);
            Assert.True(parser.Current.Children.ElementAt(1).SelfClosing);
            Assert.Empty(parser.Current.Children.ElementAt(1).Attributes);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void ImgAndBrWithSelfCloseInsideDivWithSpaceInBetween()
        {
            string html = "<div> <img /> <br /> </div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", " <img /> <br /> ", html);
            Assert.Null(parser.Current.Parent);
            Assert.True(parser.Current.HasChildren);
            Assert.Equal(5, parser.Current.Children.Count());
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Attributes);

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "#text", " ", " ");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children);
            Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.Empty(parser.Current.Children.ElementAt(0).Attributes);

            Assert.NotNull(parser.Current.Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "img", "<img />", "<img />");
            Assert.NotNull(parser.Current.Children.ElementAt(1).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(1).Parent);
            Assert.False(parser.Current.Children.ElementAt(1).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(1).Children);
            Assert.True(parser.Current.Children.ElementAt(1).SelfClosing);
            Assert.Empty(parser.Current.Children.ElementAt(1).Attributes);

            Assert.NotNull(parser.Current.Children.ElementAt(2));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(2), "#text", " ", " ");
            Assert.NotNull(parser.Current.Children.ElementAt(2).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(2).Parent);
            Assert.False(parser.Current.Children.ElementAt(2).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(2).Children);
            Assert.False(parser.Current.Children.ElementAt(2).SelfClosing);
            Assert.Empty(parser.Current.Children.ElementAt(2).Attributes);

            Assert.NotNull(parser.Current.Children.ElementAt(3));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(3), "br", "<br />", "<br />");
            Assert.NotNull(parser.Current.Children.ElementAt(3).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(3).Parent);
            Assert.False(parser.Current.Children.ElementAt(3).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(3).Children);
            Assert.True(parser.Current.Children.ElementAt(3).SelfClosing);
            Assert.Empty(parser.Current.Children.ElementAt(3).Attributes);

            Assert.NotNull(parser.Current.Children.ElementAt(4));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(4), "#text", " ", " ");
            Assert.NotNull(parser.Current.Children.ElementAt(4).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(4).Parent);
            Assert.False(parser.Current.Children.ElementAt(4).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(4).Children);
            Assert.False(parser.Current.Children.ElementAt(4).SelfClosing);
            Assert.Empty(parser.Current.Children.ElementAt(4).Attributes);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

    }
}
