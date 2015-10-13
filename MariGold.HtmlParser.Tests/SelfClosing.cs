namespace MariGold.HtmlParser.Tests
{
    using System;
    using NUnit.Framework;
    using MariGold.HtmlParser;
    using System.Linq;

    [TestFixture]
    public class SelfClosing
    {
        [Test]
        public void BrOnly()
        {
            string html = @"<br />";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br />", "<br />");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(true, parser.Current.SelfClosing);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void BrOnlyNoSpace()
        {
            string html = @"<br/>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br/>", "<br/>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(true, parser.Current.SelfClosing);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void BrWithNoSpaceAndSpan()
        {
            string html = @"<br/><span>1</span>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br/>", "<br/>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(true, parser.Current.SelfClosing);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "span", "1", "<span>1</span>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "#text", "1", "1");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void TextAndBr()
        {
            string html = @"test<br />";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#text", "test", "test");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br />", "<br />");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(true, parser.Current.SelfClosing);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void TextWithRightPaddingAndBr()
        {
            string html = @"test <br />";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#text", "test ", "test ");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br />", "<br />");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(true, parser.Current.SelfClosing);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void BrAndText()
        {
            string html = @"<br />test";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br />", "<br />");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(true, parser.Current.SelfClosing);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#text", "test", "test");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
           
            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void BrAndDiv()
        {
            string html = @"<br /><div>tt</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br />", "<br />");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(true, parser.Current.SelfClosing);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "tt", "<div>tt</div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "#text", "tt", "tt");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void DivAndBr()
        {
            string html = @"<div>tt</div><br />";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "tt", "<div>tt</div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "#text", "tt", "tt");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br />", "<br />");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(true, parser.Current.SelfClosing);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void BrInsideDiv()
        {
            string html = @"<div><br /></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<br />", "<div><br /></div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "br", "<br />", "<br />");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(true, parser.Current.Children[0].SelfClosing);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void BrInsideDivAndText()
        {
            string html = @"<div>t1<br /></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "t1<br />", "<div>t1<br /></div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(2, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "#text", "t1", "t1");
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);

            Assert.IsNotNull(parser.Current.Children[1]);
            TestUtility.AreEqual(parser.Current.Children[1], "br", "<br />", "<br />");
            Assert.IsNotNull(parser.Current.Children[1].Parent);
            Assert.AreEqual(0, parser.Current.Children[1].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[1].HasChildren);
            Assert.AreEqual(true, parser.Current.Children[1].SelfClosing);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void TextInsideDivAndBr()
        {
            string html = @"<div><br />t1</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<br />t1", "<div><br />t1</div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(2, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "br", "<br />", "<br />");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(true, parser.Current.Children[0].SelfClosing);

            Assert.IsNotNull(parser.Current.Children[1]);
            TestUtility.AreEqual(parser.Current.Children[1], "#text", "t1", "t1");
            Assert.AreEqual(0, parser.Current.Children[1].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[1].HasChildren);
            Assert.IsNotNull(parser.Current.Children[1].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[1].Parent);
            Assert.AreEqual(false, parser.Current.Children[1].SelfClosing);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void BrInsideDivAndOneChar()
        {
            string html = @"<div>t<br /></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "t<br />", "<div>t<br /></div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(2, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "#text", "t", "t");
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);

            Assert.IsNotNull(parser.Current.Children[1]);
            TestUtility.AreEqual(parser.Current.Children[1], "br", "<br />", "<br />");
            Assert.IsNotNull(parser.Current.Children[1].Parent);
            Assert.AreEqual(0, parser.Current.Children[1].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[1].HasChildren);
            Assert.AreEqual(true, parser.Current.Children[1].SelfClosing);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void OneCharInsideDivAndBr()
        {
            string html = @"<div><br />t</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<br />t", "<div><br />t</div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(2, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "br", "<br />", "<br />");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(true, parser.Current.Children[0].SelfClosing);

            Assert.IsNotNull(parser.Current.Children[1]);
            TestUtility.AreEqual(parser.Current.Children[1], "#text", "t", "t");
            Assert.AreEqual(0, parser.Current.Children[1].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[1].HasChildren);
            Assert.IsNotNull(parser.Current.Children[1].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[1].Parent);
            Assert.AreEqual(false, parser.Current.Children[1].SelfClosing);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void BrInsideDivAndOneCharAndSpace()
        {
            string html = @"<div>t <br /></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "t <br />", "<div>t <br /></div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(2, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "#text", "t ", "t ");
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);

            Assert.IsNotNull(parser.Current.Children[1]);
            TestUtility.AreEqual(parser.Current.Children[1], "br", "<br />", "<br />");
            Assert.IsNotNull(parser.Current.Children[1].Parent);
            Assert.AreEqual(0, parser.Current.Children[1].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[1].HasChildren);
            Assert.AreEqual(true, parser.Current.Children[1].SelfClosing);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void OneCharAndSpaceInsideDivAndBr()
        {
            string html = @"<div><br /> t</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<br /> t", "<div><br /> t</div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(2, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "br", "<br />", "<br />");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(true, parser.Current.Children[0].SelfClosing);

            Assert.IsNotNull(parser.Current.Children[1]);
            TestUtility.AreEqual(parser.Current.Children[1], "#text", " t", " t");
            Assert.AreEqual(0, parser.Current.Children[1].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[1].HasChildren);
            Assert.IsNotNull(parser.Current.Children[1].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[1].Parent);
            Assert.AreEqual(false, parser.Current.Children[1].SelfClosing);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void DivPBr()
        {
            string html = @"<div><p><br /></p></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<p><br /></p>", "<div><p><br /></p></div>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(1, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(false, parser.Current.SelfClosing);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "p", "<br />", "<p><br /></p>");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(1, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(true, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);

            Assert.IsNotNull(parser.Current.Children[0].Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0].Children[0], "br", "<br />", "<br />");
            Assert.IsNotNull(parser.Current.Children[0].Children[0].Parent);
            Assert.AreEqual(parser.Current.Children[0], parser.Current.Children[0].Children[0].Parent);
            Assert.AreEqual(0, parser.Current.Children[0].Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].Children[0].HasChildren);
            Assert.AreEqual(true, parser.Current.Children[0].Children[0].SelfClosing);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void SpaceBr()
        {
            string html = " <br>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#text", " ", " ");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br>", "<br>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void BrSpace()
        {
            string html = "<br> ";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "br", "<br>", "<br>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "#text", " ", " ");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void ImgAndBrInsideDiv()
        {
            string html = "<div><img><br></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<img><br>", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(2, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "img", "<img>", "<img>");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(true, parser.Current.Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[1]);
            TestUtility.AreEqual(parser.Current.Children[1], "br", "<br>", "<br>");
            Assert.IsNotNull(parser.Current.Children[1].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[1].Parent);
            Assert.AreEqual(false, parser.Current.Children[1].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[1].Children.Count);
            Assert.AreEqual(true, parser.Current.Children[1].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[1].Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void ImgWithId()
        {
            string html = "<img id=\"img\">";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "img", "<img id=\"img\">", "<img id=\"img\">");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.SelfClosing);
            Assert.AreEqual(1, parser.Current.Attributes.Count);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "img");

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void ImgWithIdAndSrc()
        {
            string html = "<img id=\"img\" src=\"http://google.com/1.jpg\">";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "img", html, html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(0, parser.Current.Children.Count);
            Assert.AreEqual(true, parser.Current.SelfClosing);
            Assert.AreEqual(2, parser.Current.Attributes.Count);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "img");
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(1), "src", "http://google.com/1.jpg");

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void ImgAndBrWithIdInsideDiv()
        {
            string html = "<div id='dv'><img id='im'><br id='b'></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<img id='im'><br id='b'>", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(2, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(1, parser.Current.Attributes.Count);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "id", "dv");

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "img", "<img id='im'>", "<img id='im'>");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(true, parser.Current.Children[0].SelfClosing);
            Assert.AreEqual(1, parser.Current.Children[0].Attributes.Count);
            TestUtility.CheckKeyValuePair(parser.Current.Children[0].Attributes.ElementAt(0), "id", "im");

            Assert.IsNotNull(parser.Current.Children[1]);
            TestUtility.AreEqual(parser.Current.Children[1], "br", "<br id='b'>", "<br id='b'>");
            Assert.IsNotNull(parser.Current.Children[1].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[1].Parent);
            Assert.AreEqual(false, parser.Current.Children[1].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[1].Children.Count);
            Assert.AreEqual(true, parser.Current.Children[1].SelfClosing);
            Assert.AreEqual(1, parser.Current.Children[1].Attributes.Count);
            TestUtility.CheckKeyValuePair(parser.Current.Children[1].Attributes.ElementAt(0), "id", "b");

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void ImgAndBrWithSelfCloseInsideDiv()
        {
            string html = "<div><img /><br /></div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", "<img /><br />", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(2, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "img", "<img />", "<img />");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(true, parser.Current.Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[1]);
            TestUtility.AreEqual(parser.Current.Children[1], "br", "<br />", "<br />");
            Assert.IsNotNull(parser.Current.Children[1].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[1].Parent);
            Assert.AreEqual(false, parser.Current.Children[1].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[1].Children.Count);
            Assert.AreEqual(true, parser.Current.Children[1].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[1].Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void ImgAndBrWithSelfCloseInsideDivWithSpaceInBetween()
        {
            string html = "<div> <img /> <br /> </div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "div", " <img /> <br /> ", html);
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(5, parser.Current.Children.Count);
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[0]);
            TestUtility.AreEqual(parser.Current.Children[0], "#text", " ", " ");
            Assert.IsNotNull(parser.Current.Children[0].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[0].Parent);
            Assert.AreEqual(false, parser.Current.Children[0].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[0].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[0].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[0].Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[1]);
            TestUtility.AreEqual(parser.Current.Children[1], "img", "<img />", "<img />");
            Assert.IsNotNull(parser.Current.Children[1].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[1].Parent);
            Assert.AreEqual(false, parser.Current.Children[1].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[1].Children.Count);
            Assert.AreEqual(true, parser.Current.Children[1].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[1].Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[2]);
            TestUtility.AreEqual(parser.Current.Children[2], "#text", " ", " ");
            Assert.IsNotNull(parser.Current.Children[2].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[2].Parent);
            Assert.AreEqual(false, parser.Current.Children[2].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[2].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[2].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[2].Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[3]);
            TestUtility.AreEqual(parser.Current.Children[3], "br", "<br />", "<br />");
            Assert.IsNotNull(parser.Current.Children[3].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[3].Parent);
            Assert.AreEqual(false, parser.Current.Children[3].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[3].Children.Count);
            Assert.AreEqual(true, parser.Current.Children[3].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[3].Attributes.Count);

            Assert.IsNotNull(parser.Current.Children[4]);
            TestUtility.AreEqual(parser.Current.Children[4], "#text", " ", " ");
            Assert.IsNotNull(parser.Current.Children[4].Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children[4].Parent);
            Assert.AreEqual(false, parser.Current.Children[4].HasChildren);
            Assert.AreEqual(0, parser.Current.Children[4].Children.Count);
            Assert.AreEqual(false, parser.Current.Children[4].SelfClosing);
            Assert.AreEqual(0, parser.Current.Children[4].Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

    }
}
