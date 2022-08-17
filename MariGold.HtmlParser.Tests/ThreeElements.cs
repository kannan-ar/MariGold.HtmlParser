namespace MariGold.HtmlParser.Tests
{
    using MariGold.HtmlParser;
    using Xunit;

    public class ThreeElements
    {
        [Fact]
        public void OneDivTextNext()
        {
            string html = "<div>this is a div</div>test<span>this is a span</span>";

            HtmlParser parser = new HtmlTextParser(html);

            if (parser.Traverse())
            {
                Assert.NotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.Equal("div", parser.Current.Tag);
                    Assert.Equal("<div>this is a div</div>", parser.Current.Html);
                    Assert.Equal("this is a div", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            if (parser.Traverse())
            {
                Assert.NotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.Equal("#text", parser.Current.Tag);
                    Assert.Equal("test", parser.Current.Html);
                    Assert.Equal("test", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            if (parser.Traverse())
            {
                Assert.NotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.Equal("span", parser.Current.Tag);
                    Assert.Equal("<span>this is a span</span>", parser.Current.Html);
                    Assert.Equal("this is a span", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            Assert.False(parser.Traverse());

        }

        [Fact]
        public void TextDivSpan()
        {
            string html = "test<div>this is a div</div><span>this is a span</span>";

            HtmlParser parser = new HtmlTextParser(html);

            if (parser.Traverse())
            {
                Assert.NotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.Equal("#text", parser.Current.Tag);
                    Assert.Equal("test", parser.Current.Html);
                    Assert.Equal("test", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            if (parser.Traverse())
            {
                Assert.NotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.Equal("div", parser.Current.Tag);
                    Assert.Equal("<div>this is a div</div>", parser.Current.Html);
                    Assert.Equal("this is a div", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }


            if (parser.Traverse())
            {
                Assert.NotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.Equal("span", parser.Current.Tag);
                    Assert.Equal("<span>this is a span</span>", parser.Current.Html);
                    Assert.Equal("this is a span", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            Assert.False(parser.Traverse());

        }

        [Fact]
        public void PTextDivSpan()
        {
            string html = "<p>ptag</p>test<div>this is a div</div><span>this is a span</span>";

            HtmlParser parser = new HtmlTextParser(html);

            if (parser.Traverse())
            {
                Assert.NotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.Equal("p", parser.Current.Tag);
                    Assert.Equal("<p>ptag</p>", parser.Current.Html);
                    Assert.Equal("ptag", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            if (parser.Traverse())
            {
                Assert.NotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.Equal("#text", parser.Current.Tag);
                    Assert.Equal("test", parser.Current.Html);
                    Assert.Equal("test", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            if (parser.Traverse())
            {
                Assert.NotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.Equal("div", parser.Current.Tag);
                    Assert.Equal("<div>this is a div</div>", parser.Current.Html);
                    Assert.Equal("this is a div", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }


            if (parser.Traverse())
            {
                Assert.NotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.Equal("span", parser.Current.Tag);
                    Assert.Equal("<span>this is a span</span>", parser.Current.Html);
                    Assert.Equal("this is a span", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            Assert.False(parser.Traverse());

        }
    }
}
