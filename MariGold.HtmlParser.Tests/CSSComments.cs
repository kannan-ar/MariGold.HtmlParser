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
            TestUtility.CheckStyle(node.Styles.ElementAt(0), "color", "red");
        }
    }
}
