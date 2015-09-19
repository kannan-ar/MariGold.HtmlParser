namespace MariGold.HtmlParser.Tests
{
    using System;
    using NUnit.Framework;
    using MariGold.HtmlParser;

    [TestFixture]
    public class ThreeElements
    {
        [Test]
        public void OneDivTextNext()
        {
            string html = "<div>this is a div</div>test<span>this is a span</span>";

            HtmlParser parser = new HtmlTextParser(html);

            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.AreEqual("div", parser.Current.Tag);
                    Assert.AreEqual("<div>this is a div</div>", parser.Current.Html);
                    Assert.AreEqual("this is a div", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.AreEqual("#text", parser.Current.Tag);
                    Assert.AreEqual("test", parser.Current.Html);
                    Assert.AreEqual("test", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.AreEqual("span", parser.Current.Tag);
                    Assert.AreEqual("<span>this is a span</span>", parser.Current.Html);
                    Assert.AreEqual("this is a span", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            Assert.AreEqual(parser.Traverse(), false);

        }

        [Test]
        public void TextDivSpan()
        {
            string html = "test<div>this is a div</div><span>this is a span</span>";

            HtmlParser parser = new HtmlTextParser(html);

            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.AreEqual("#text", parser.Current.Tag);
                    Assert.AreEqual("test", parser.Current.Html);
                    Assert.AreEqual("test", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.AreEqual("div", parser.Current.Tag);
                    Assert.AreEqual("<div>this is a div</div>", parser.Current.Html);
                    Assert.AreEqual("this is a div", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            
            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.AreEqual("span", parser.Current.Tag);
                    Assert.AreEqual("<span>this is a span</span>", parser.Current.Html);
                    Assert.AreEqual("this is a span", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            Assert.AreEqual(parser.Traverse(), false);

        }

        [Test]
        public void PTextDivSpan()
        {
            string html = "<p>ptag</p>test<div>this is a div</div><span>this is a span</span>";

            HtmlParser parser = new HtmlTextParser(html);

            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.AreEqual("p", parser.Current.Tag);
                    Assert.AreEqual("<p>ptag</p>", parser.Current.Html);
                    Assert.AreEqual("ptag", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.AreEqual("#text", parser.Current.Tag);
                    Assert.AreEqual("test", parser.Current.Html);
                    Assert.AreEqual("test", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.AreEqual("div", parser.Current.Tag);
                    Assert.AreEqual("<div>this is a div</div>", parser.Current.Html);
                    Assert.AreEqual("this is a div", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }


            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.AreEqual("span", parser.Current.Tag);
                    Assert.AreEqual("<span>this is a span</span>", parser.Current.Html);
                    Assert.AreEqual("this is a span", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            Assert.AreEqual(parser.Traverse(), false);

        }
    }
}
