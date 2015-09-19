namespace MariGold.HtmlParser.Tests
{
    using System;
    using NUnit.Framework;
    using MariGold.HtmlParser;

    [TestFixture]
    public class TwoElements
    {
        [Test]
        public void OneDivStartWithSpace()
        {
            string html = " <div>test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.AreEqual("#text", parser.Current.Tag);
                    Assert.AreEqual(" ", parser.Current.Html);
                    Assert.AreEqual(" ", parser.Current.InnerHtml);
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
                    Assert.AreEqual("<div>test</div>", parser.Current.Html);
                    Assert.AreEqual("test", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            Assert.AreEqual(parser.Traverse(), false);
        }

        [Test]
        public void OneDivSpace()
        {
            string html = "<div>test</div> ";

            HtmlParser parser = new HtmlTextParser(html);

            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.AreEqual("div", parser.Current.Tag);
                    Assert.AreEqual("<div>test</div>", parser.Current.Html);
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
                    Assert.AreEqual("#text", parser.Current.Tag);
                    Assert.AreEqual(" ", parser.Current.Html);
                    Assert.AreEqual(" ", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            Assert.AreEqual(parser.Traverse(), false);
        }

        [Test]
        public void OneDivTextNext()
        {
            string html = "<div>this is a test</div>test";

            HtmlParser parser = new HtmlTextParser(html);

            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.AreEqual("div", parser.Current.Tag);
                    Assert.AreEqual("<div>this is a test</div>", parser.Current.Html);
                    Assert.AreEqual("this is a test", parser.Current.InnerHtml);
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

            Assert.AreEqual(parser.Traverse(), false);

        }

        [Test]
        public void OneTextNextDiv()
        {
            string html = "test<div>this is a test</div>";

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
                    Assert.AreEqual("<div>this is a test</div>", parser.Current.Html);
                    Assert.AreEqual("this is a test", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            Assert.AreEqual(parser.Traverse(), false);

        }

        [Test]
        public void OneTextNextDivWithSpaceOnOpenTag()
        {
            string html = "test< div>this is a test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            if (parser.Traverse())
            {
                Assert.IsNotNull(parser.Current);

                if (parser.Current != null)
                {
                    Assert.AreEqual("#text", parser.Current.Tag);
                    Assert.AreEqual("test< div>this is a test", parser.Current.Html);
                    Assert.AreEqual("test< div>this is a test", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            Assert.AreEqual(parser.Traverse(), false);
        }

        [Test]
        public void OneTextNextDivWithSpaceAtTheEndOfOpenTag()
        {
            string html = "test<div >this is a test</div>";

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
                    Assert.AreEqual("<div >this is a test</div>", parser.Current.Html);
                    Assert.AreEqual("this is a test", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            Assert.AreEqual(parser.Traverse(), false);
        }

        [Test]
        public void OneTextNextDivWithSpaceAtTheStartOfEndTag()
        {
            string html = "test<div>this is a test< /div>";

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
                    Assert.AreEqual("<div>this is a test< /div>", parser.Current.Html);
                    Assert.AreEqual("this is a test< /div>", parser.Current.InnerHtml);
                }
            }
            else
            {
                Assert.Fail("Fail to traverse");
            }

            Assert.AreEqual(parser.Traverse(), false);
        }

        [Test]
        public void OneTextNextDivWithSpaceAtTheEndOfEndTag()
        {
            string html = "test<div>this is a test</div >";

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
                    Assert.AreEqual("<div>this is a test</div >", parser.Current.Html);
                    Assert.AreEqual("this is a test", parser.Current.InnerHtml);
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
