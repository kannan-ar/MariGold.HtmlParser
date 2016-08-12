namespace MariGold.HtmlParser.Tests
{
    using System;
    using NUnit.Framework;
    using MariGold.HtmlParser;
    using System.Linq;
    using System.IO;

    [TestFixture]
    public class CSSComments
    {
        [Test]
        public void CommentWithMediaQuery()
        {
            string path = TestUtility.GetFolderPath("Html\\commentwithmediaquery.htm");
            string html = string.Empty;

            using (StreamReader sr = new StreamReader(path))
            {
                html = sr.ReadToEnd();
            }

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Parse());
            parser.ParseCSS();

            IHtmlNode node = parser.Current;

            while (node.Tag != "html")
                node = node.Next;

            node = node.Children.ElementAt(0);

            while (node.Tag != "body")
                node = node.Next;

            IHtmlNode body=node;

            node = node.Children.ElementAt(0);

            while (node.Tag != "p")
                node = node.Next;

            TestUtility.AnalyzeNode(node, "p", "test", "<p>test</p>", body, false, true, 1, 0, 0);
        }

        [Test]
        public void CommentWithoutSpace()
        {
            string html = @"<style>/*styles*/.cls{color:red;}</style>
                            <div class='cls'>test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Parse());
            parser.ParseCSS();

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
                node = node.Next;

            TestUtility.AnalyzeNode(node, "div", "test", "<div class='cls'>test</div>", null, false, true, 1, 1, 1);
            node.CheckStyle(0, "color", "red");
        }

        [Test]
        public void CommentInStyles()
        {
            string html = @"<style>
                                .cls
                                {
                                    /*color:red;*/
                                    color:white;
                                }
                            </style>
                            <div class='cls'>test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Parse());
            parser.ParseCSS();

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
                node = node.Next;

            TestUtility.AnalyzeNode(node, "div", "test", "<div class='cls'>test</div>", null, false, true, 1, 1, 1);
            node.CheckStyle(0, "color", "white");
        }

        [Test]
        public void MultipleCommentsInStyles()
        {
            string html = @"<style>
                                .cls
                                {
                                    /*color:red;*/
                                    color:white;
                                    /*color:blue*/
                                }
                            </style>
                            <div class='cls'>test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Parse());
            parser.ParseCSS();

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
                node = node.Next;

            TestUtility.AnalyzeNode(node, "div", "test", "<div class='cls'>test</div>", null, false, true, 1, 1, 1);
            node.CheckStyle(0, "color", "white");
        }

        [Test]
        public void MultipleCommentsInStylesWithoutSpace()
        {
            string html = @"<style>.cls{/*color:red;*/color:white;/*color:blue*/}</style><div class='cls'>test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Parse());
            parser.ParseCSS();

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
                node = node.Next;

            TestUtility.AnalyzeNode(node, "div", "test", "<div class='cls'>test</div>", null, false, true, 1, 1, 1);
            node.CheckStyle(0, "color", "white");
        }

        [Test]
        public void MultipleCommentsInStyleSheet()
        {
            string html = @"<style>
                                /* .cls beginning */

                                .cls
                                {
                                    color:white;
                                }
                                
                                /* .cls end */

                                /* fnt beginning */

                                .fnt
                                {
                                    font-weight:bold;
                                }

                                /* fnt end */
                            </style>
                            <div class='cls fnt'>test</div>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Parse());
            parser.ParseCSS();

            IHtmlNode node = parser.Current;

            while (node.Tag != "div")
                node = node.Next;

            TestUtility.AnalyzeNode(node, "div", "test", "<div class='cls fnt'>test</div>", null, false, true, 1, 1, 2);
            node.CheckStyle(0, "color", "white");
            node.CheckStyle(1, "font-weight", "bold");
        }
    }
}
