namespace MariGold.HtmlParser.Tests
{
    using System;
    using NUnit.Framework;
    using MariGold.HtmlParser;
    using System.IO;
    using System.Linq;

    [TestFixture]
    public class FromFile
    {
        [Test]
        public void Basic()
        {
            string path = TestUtility.GetFolderPath("Html\\basic.htm");
            string html = string.Empty;

            using (StreamReader sr = new StreamReader(path))
            {
                html = sr.ReadToEnd();
            }

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            HtmlNode node = parser.Current;
            TestUtility.AnalyzeNode(node, "DOCTYPE", "<!DOCTYPE html>", "<!DOCTYPE html>", null, true, false, 0, 1);
            TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "html", "");

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            node = parser.Current;
            TestUtility.AnalyzeNode(node, "#text", "\r\n\r\n", "\r\n\r\n", null, false, false, 0, 0);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            node = parser.Current;
            html = "<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <meta charset=\"utf-8\" />\r\n    <title></title>\r\n</head>\r\n<body>\r\n\r\n</body>\r\n</html>";
            string text = "\r\n<head>\r\n    <meta charset=\"utf-8\" />\r\n    <title></title>\r\n</head>\r\n<body>\r\n\r\n</body>\r\n";
            TestUtility.AnalyzeNode(node, "html", text, html, null, false, true, 5, 2);
            TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "lang", "en");
            TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "xmlns", "http://www.w3.org/1999/xhtml");

            Assert.IsNotNull(node.Children[0]);
            TestUtility.AnalyzeNode(node.Children[0], "#text", "\r\n", "\r\n", node, false, false, 0, 0);

            Assert.IsNotNull(node.Children[1]);
            TestUtility.AnalyzeNode(node.Children[1], "head",
                "\r\n    <meta charset=\"utf-8\" />\r\n    <title></title>\r\n",
                "<head>\r\n    <meta charset=\"utf-8\" />\r\n    <title></title>\r\n</head>",
                node, false, true, 5, 0);

            Assert.IsNotNull(node.Children[1].Children[0]);
            TestUtility.AnalyzeNode(node.Children[1].Children[0], "#text", "\r\n    ", "\r\n    ",
                node.Children[1], false, false, 0, 0);

            Assert.IsNotNull(node.Children[1].Children[1]);
            TestUtility.AnalyzeNode(node.Children[1].Children[1], "meta", "<meta charset=\"utf-8\" />", "<meta charset=\"utf-8\" />",
                node.Children[1], true, false, 0, 1);
            TestUtility.CheckKeyValuePair(node.Children[1].Children[1].Attributes.ElementAt(0), "charset", "utf-8");

            Assert.IsNotNull(node.Children[1].Children[2]);
            TestUtility.AnalyzeNode(node.Children[1].Children[2], "#text", "\r\n    ", "\r\n    ",
                node.Children[1], false, false, 0, 0);

            Assert.IsNotNull(node.Children[1].Children[3]);
            TestUtility.AnalyzeNode(node.Children[1].Children[3], "title", "", "<title></title>",
                node.Children[1], false, false, 0, 0);

            Assert.IsNotNull(node.Children[1].Children[4]);
            TestUtility.AnalyzeNode(node.Children[1].Children[4], "#text", "\r\n", "\r\n",
                node.Children[1], false, false, 0, 0);

            Assert.IsNotNull(node.Children[2]);
            TestUtility.AnalyzeNode(node.Children[2], "#text", "\r\n", "\r\n", node, false, false, 0, 0);

            Assert.IsNotNull(node.Children[3]);
            TestUtility.AnalyzeNode(node.Children[3], "body", "\r\n\r\n", "<body>\r\n\r\n</body>",
                node, false, true, 1, 0);

            Assert.IsNotNull(node.Children[3].Children[0]);
            TestUtility.AnalyzeNode(node.Children[3].Children[0], "#text", "\r\n\r\n", "\r\n\r\n", node.Children[3],
                false, false, 0, 0);

            Assert.IsNotNull(node.Children[4]);
            TestUtility.AnalyzeNode(node.Children[4], "#text", "\r\n", "\r\n", node, false, false, 0, 0);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void BasicWithAttributes()
        {
            string path = TestUtility.GetFolderPath("Html\\basicwithattributes.htm");
            string html = string.Empty;

            using (StreamReader sr = new StreamReader(path))
            {
                html = sr.ReadToEnd();
            }

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            HtmlNode node = parser.Current;
            TestUtility.AnalyzeNode(node, "DOCTYPE", "<!DOCTYPE html>", "<!DOCTYPE html>", null, true, false, 0, 1);
            TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "html", "");

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            node = parser.Current;
            TestUtility.AnalyzeNode(node, "#text", "\r\n\r\n", "\r\n\r\n", null, false, false, 0, 0);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            node = parser.Current;
            html = "<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head id=\"hd\" style=\"margin:2px;\">\r\n    <meta charset=\"utf-8\" name=\"mt\" />\r\n    <title id=\"tt\">this is test</title>\r\n</head>\r\n<body id=\"bd\" role=\"application\" ng-app>\r\n\r\n</body>\r\n</html>";
            string text = "\r\n<head id=\"hd\" style=\"margin:2px;\">\r\n    <meta charset=\"utf-8\" name=\"mt\" />\r\n    <title id=\"tt\">this is test</title>\r\n</head>\r\n<body id=\"bd\" role=\"application\" ng-app>\r\n\r\n</body>\r\n";
            TestUtility.AnalyzeNode(node, "html", text, html, null, false, true, 5, 2);
            TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "lang", "en");
            TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "xmlns", "http://www.w3.org/1999/xhtml");

            Assert.IsNotNull(node.Children[0]);
            TestUtility.AnalyzeNode(node.Children[0], "#text", "\r\n", "\r\n", node, false, false, 0, 0);

            Assert.IsNotNull(node.Children[1]);
            TestUtility.AnalyzeNode(node.Children[1], "head",
                "\r\n    <meta charset=\"utf-8\" name=\"mt\" />\r\n    <title id=\"tt\">this is test</title>\r\n",
                "<head id=\"hd\" style=\"margin:2px;\">\r\n    <meta charset=\"utf-8\" name=\"mt\" />\r\n    <title id=\"tt\">this is test</title>\r\n</head>",
                node, false, true, 5, 2);
            TestUtility.CheckKeyValuePair(node.Children[1].Attributes.ElementAt(0), "id", "hd");
            TestUtility.CheckKeyValuePair(node.Children[1].Attributes.ElementAt(1), "style", "margin:2px;");

            Assert.IsNotNull(node.Children[1].Children[0]);
            TestUtility.AnalyzeNode(node.Children[1].Children[0], "#text", "\r\n    ", "\r\n    ",
                node.Children[1], false, false, 0, 0);

            Assert.IsNotNull(node.Children[1].Children[1]);
            TestUtility.AnalyzeNode(node.Children[1].Children[1], "meta", "<meta charset=\"utf-8\" name=\"mt\" />", "<meta charset=\"utf-8\" name=\"mt\" />",
                node.Children[1], true, false, 0, 2);
            TestUtility.CheckKeyValuePair(node.Children[1].Children[1].Attributes.ElementAt(0), "charset", "utf-8");
            TestUtility.CheckKeyValuePair(node.Children[1].Children[1].Attributes.ElementAt(1), "name", "mt");

            Assert.IsNotNull(node.Children[1].Children[2]);
            TestUtility.AnalyzeNode(node.Children[1].Children[2], "#text", "\r\n    ", "\r\n    ",
                node.Children[1], false, false, 0, 0);

            Assert.IsNotNull(node.Children[1].Children[3]);
            TestUtility.AnalyzeNode(node.Children[1].Children[3], "title", "this is test", "<title id=\"tt\">this is test</title>",
                node.Children[1], false, true, 1, 1);
            TestUtility.CheckKeyValuePair(node.Children[1].Children[3].Attributes.ElementAt(0), "id", "tt");

            Assert.IsNotNull(node.Children[1].Children[3].Children[0]);
            TestUtility.AnalyzeNode(node.Children[1].Children[3].Children[0], "#text", "this is test", "this is test",
                node.Children[1].Children[3], false, false, 0, 0);

            Assert.IsNotNull(node.Children[1].Children[4]);
            TestUtility.AnalyzeNode(node.Children[1].Children[4], "#text", "\r\n", "\r\n",
                node.Children[1], false, false, 0, 0);

            Assert.IsNotNull(node.Children[2]);
            TestUtility.AnalyzeNode(node.Children[2], "#text", "\r\n", "\r\n", node, false, false, 0, 0);

            Assert.IsNotNull(node.Children[3]);
            TestUtility.AnalyzeNode(node.Children[3], "body", "\r\n\r\n", "<body id=\"bd\" role=\"application\" ng-app>\r\n\r\n</body>",
                node, false, true, 1, 3);
            TestUtility.CheckKeyValuePair(node.Children[3].Attributes.ElementAt(0), "id", "bd");
            TestUtility.CheckKeyValuePair(node.Children[3].Attributes.ElementAt(1), "role", "application");
            TestUtility.CheckKeyValuePair(node.Children[3].Attributes.ElementAt(2), "ng-app", "");


            Assert.IsNotNull(node.Children[3].Children[0]);
            TestUtility.AnalyzeNode(node.Children[3].Children[0], "#text", "\r\n\r\n", "\r\n\r\n", node.Children[3],
                false, false, 0, 0);

            Assert.IsNotNull(node.Children[4]);
            TestUtility.AnalyzeNode(node.Children[4], "#text", "\r\n", "\r\n", node, false, false, 0, 0);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }
    }
}
