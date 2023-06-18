namespace MariGold.HtmlParser.Tests;

using MariGold.HtmlParser;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class FromFile
{
    [Fact]
    public void Basic()
    {
        string path = TestUtility.GetFolderPath("Html\\basic.htm");
        string html = string.Empty;

        using (StreamReader sr = new StreamReader(path))
        {
            html = sr.ReadToEnd();
        }

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Traverse());
        Assert.NotNull(parser.Current);
        IHtmlNode node = parser.Current;
        TestUtility.AnalyzeNode(node, "DOCTYPE", "<!DOCTYPE html>", "<!DOCTYPE html>", null, true, false, 0, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "html", "");

        Assert.True(parser.Traverse());
        Assert.NotNull(parser.Current);
        node = parser.Current;
        TestUtility.AnalyzeNode(node, "#text", "\r\n\r\n", "\r\n\r\n", null, false, false, 0, 0);

        Assert.True(parser.Traverse());
        Assert.NotNull(parser.Current);
        node = parser.Current;
        html = "<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <meta charset=\"utf-8\" />\r\n    <title></title>\r\n</head>\r\n<body>\r\n\r\n</body>\r\n</html>";
        string text = "\r\n<head>\r\n    <meta charset=\"utf-8\" />\r\n    <title></title>\r\n</head>\r\n<body>\r\n\r\n</body>\r\n";
        TestUtility.AnalyzeNode(node, "html", text, html, null, false, true, 5, 2);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "lang", "en");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "xmlns", "http://www.w3.org/1999/xhtml");

        Assert.NotNull(node.Children.ElementAt(0));
        TestUtility.AnalyzeNode(node.Children.ElementAt(0), "#text", "\r\n", "\r\n", node, false, false, 0, 0);

        Assert.NotNull(node.Children.ElementAt(1));
        TestUtility.AnalyzeNode(node.Children.ElementAt(1), "head",
            "\r\n    <meta charset=\"utf-8\" />\r\n    <title></title>\r\n",
            "<head>\r\n    <meta charset=\"utf-8\" />\r\n    <title></title>\r\n</head>",
            node, false, true, 5, 0);

        Assert.NotNull(node.Children.ElementAt(1).Children.ElementAt(0));
        TestUtility.AnalyzeNode(node.Children.ElementAt(1).Children.ElementAt(0), "#text", "\r\n    ", "\r\n    ",
            node.Children.ElementAt(1), false, false, 0, 0);

        Assert.NotNull(node.Children.ElementAt(1).Children.ElementAt(1));
        TestUtility.AnalyzeNode(node.Children.ElementAt(1).Children.ElementAt(1), "meta", "<meta charset=\"utf-8\" />", "<meta charset=\"utf-8\" />",
            node.Children.ElementAt(1), true, false, 0, 1);
        TestUtility.CheckKeyValuePair(node.Children.ElementAt(1).Children.ElementAt(1).Attributes.ElementAt(0), "charset", "utf-8");

        Assert.NotNull(node.Children.ElementAt(1).Children.ElementAt(2));
        TestUtility.AnalyzeNode(node.Children.ElementAt(1).Children.ElementAt(2), "#text", "\r\n    ", "\r\n    ",
            node.Children.ElementAt(1), false, false, 0, 0);

        Assert.NotNull(node.Children.ElementAt(1).Children.ElementAt(3));
        TestUtility.AnalyzeNode(node.Children.ElementAt(1).Children.ElementAt(3), "title", "", "<title></title>",
            node.Children.ElementAt(1), false, false, 0, 0);

        Assert.NotNull(node.Children.ElementAt(1).Children.ElementAt(4));
        TestUtility.AnalyzeNode(node.Children.ElementAt(1).Children.ElementAt(4), "#text", "\r\n", "\r\n",
            node.Children.ElementAt(1), false, false, 0, 0);

        Assert.NotNull(node.Children.ElementAt(2));
        TestUtility.AnalyzeNode(node.Children.ElementAt(2), "#text", "\r\n", "\r\n", node, false, false, 0, 0);

        Assert.NotNull(node.Children.ElementAt(3));
        TestUtility.AnalyzeNode(node.Children.ElementAt(3), "body", "\r\n\r\n", "<body>\r\n\r\n</body>",
            node, false, true, 1, 0);

        Assert.NotNull(node.Children.ElementAt(3).Children.ElementAt(0));
        TestUtility.AnalyzeNode(node.Children.ElementAt(3).Children.ElementAt(0), "#text", "\r\n\r\n", "\r\n\r\n", node.Children.ElementAt(3),
            false, false, 0, 0);

        Assert.NotNull(node.Children.ElementAt(4));
        TestUtility.AnalyzeNode(node.Children.ElementAt(4), "#text", "\r\n", "\r\n", node, false, false, 0, 0);

        Assert.False(parser.Traverse());
        Assert.Null(parser.Current);
    }

    [Fact]
    public void BasicWithAttributes()
    {
        string path = TestUtility.GetFolderPath("Html\\basicwithattributes.htm");
        string html = string.Empty;

        using (StreamReader sr = new StreamReader(path))
        {
            html = sr.ReadToEnd();
        }

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Traverse());
        Assert.NotNull(parser.Current);
        IHtmlNode node = parser.Current;
        TestUtility.AnalyzeNode(node, "DOCTYPE", "<!DOCTYPE html>", "<!DOCTYPE html>", null, true, false, 0, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "html", "");

        Assert.True(parser.Traverse());
        Assert.NotNull(parser.Current);
        node = parser.Current;
        TestUtility.AnalyzeNode(node, "#text", "\r\n\r\n", "\r\n\r\n", null, false, false, 0, 0);

        Assert.True(parser.Traverse());
        Assert.NotNull(parser.Current);
        node = parser.Current;
        html = "<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head id=\"hd\" style=\"margin:2px;\">\r\n    <meta charset=\"utf-8\" name=\"mt\" />\r\n    <title id=\"tt\">this is test</title>\r\n</head>\r\n<body id=\"bd\" role=\"application\" ng-app>\r\n\r\n</body>\r\n</html>";
        string text = "\r\n<head id=\"hd\" style=\"margin:2px;\">\r\n    <meta charset=\"utf-8\" name=\"mt\" />\r\n    <title id=\"tt\">this is test</title>\r\n</head>\r\n<body id=\"bd\" role=\"application\" ng-app>\r\n\r\n</body>\r\n";
        TestUtility.AnalyzeNode(node, "html", text, html, null, false, true, 5, 2);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "lang", "en");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "xmlns", "http://www.w3.org/1999/xhtml");

        Assert.NotNull(node.Children.ElementAt(0));
        TestUtility.AnalyzeNode(node.Children.ElementAt(0), "#text", "\r\n", "\r\n", node, false, false, 0, 0);

        Assert.NotNull(node.Children.ElementAt(1));
        TestUtility.AnalyzeNode(node.Children.ElementAt(1), "head",
            "\r\n    <meta charset=\"utf-8\" name=\"mt\" />\r\n    <title id=\"tt\">this is test</title>\r\n",
            "<head id=\"hd\" style=\"margin:2px;\">\r\n    <meta charset=\"utf-8\" name=\"mt\" />\r\n    <title id=\"tt\">this is test</title>\r\n</head>",
            node, false, true, 5, 2);
        TestUtility.CheckKeyValuePair(node.Children.ElementAt(1).Attributes.ElementAt(0), "id", "hd");
        TestUtility.CheckKeyValuePair(node.Children.ElementAt(1).Attributes.ElementAt(1), "style", "margin:2px;");

        Assert.NotNull(node.Children.ElementAt(1).Children.ElementAt(0));
        TestUtility.AnalyzeNode(node.Children.ElementAt(1).Children.ElementAt(0), "#text", "\r\n    ", "\r\n    ",
            node.Children.ElementAt(1), false, false, 0, 0);

        Assert.NotNull(node.Children.ElementAt(1).Children.ElementAt(1));
        TestUtility.AnalyzeNode(node.Children.ElementAt(1).Children.ElementAt(1), "meta", "<meta charset=\"utf-8\" name=\"mt\" />", "<meta charset=\"utf-8\" name=\"mt\" />",
            node.Children.ElementAt(1), true, false, 0, 2);
        TestUtility.CheckKeyValuePair(node.Children.ElementAt(1).Children.ElementAt(1).Attributes.ElementAt(0), "charset", "utf-8");
        TestUtility.CheckKeyValuePair(node.Children.ElementAt(1).Children.ElementAt(1).Attributes.ElementAt(1), "name", "mt");

        Assert.NotNull(node.Children.ElementAt(1).Children.ElementAt(2));
        TestUtility.AnalyzeNode(node.Children.ElementAt(1).Children.ElementAt(2), "#text", "\r\n    ", "\r\n    ",
            node.Children.ElementAt(1), false, false, 0, 0);

        Assert.NotNull(node.Children.ElementAt(1).Children.ElementAt(3));
        TestUtility.AnalyzeNode(node.Children.ElementAt(1).Children.ElementAt(3), "title", "this is test", "<title id=\"tt\">this is test</title>",
            node.Children.ElementAt(1), false, true, 1, 1);
        TestUtility.CheckKeyValuePair(node.Children.ElementAt(1).Children.ElementAt(3).Attributes.ElementAt(0), "id", "tt");

        Assert.NotNull(node.Children.ElementAt(1).Children.ElementAt(3).Children.ElementAt(0));
        TestUtility.AnalyzeNode(node.Children.ElementAt(1).Children.ElementAt(3).Children.ElementAt(0), "#text", "this is test", "this is test",
            node.Children.ElementAt(1).Children.ElementAt(3), false, false, 0, 0);

        Assert.NotNull(node.Children.ElementAt(1).Children.ElementAt(4));
        TestUtility.AnalyzeNode(node.Children.ElementAt(1).Children.ElementAt(4), "#text", "\r\n", "\r\n",
            node.Children.ElementAt(1), false, false, 0, 0);

        Assert.NotNull(node.Children.ElementAt(2));
        TestUtility.AnalyzeNode(node.Children.ElementAt(2), "#text", "\r\n", "\r\n", node, false, false, 0, 0);

        Assert.NotNull(node.Children.ElementAt(3));
        TestUtility.AnalyzeNode(node.Children.ElementAt(3), "body", "\r\n\r\n", "<body id=\"bd\" role=\"application\" ng-app>\r\n\r\n</body>",
            node, false, true, 1, 3);
        TestUtility.CheckKeyValuePair(node.Children.ElementAt(3).Attributes.ElementAt(0), "id", "bd");
        TestUtility.CheckKeyValuePair(node.Children.ElementAt(3).Attributes.ElementAt(1), "role", "application");
        TestUtility.CheckKeyValuePair(node.Children.ElementAt(3).Attributes.ElementAt(2), "ng-app", "");


        Assert.NotNull(node.Children.ElementAt(3).Children.ElementAt(0));
        TestUtility.AnalyzeNode(node.Children.ElementAt(3).Children.ElementAt(0), "#text", "\r\n\r\n", "\r\n\r\n", node.Children.ElementAt(3),
            false, false, 0, 0);

        Assert.NotNull(node.Children.ElementAt(4));
        TestUtility.AnalyzeNode(node.Children.ElementAt(4), "#text", "\r\n", "\r\n", node, false, false, 0, 0);

        Assert.False(parser.Traverse());
        Assert.Null(parser.Current);
    }

    [Fact]
    public async Task ElementIdentityStyles()
    {
        string path = TestUtility.GetFolderPath("Html\\elementidentity.htm");
        string html = string.Empty;

        using (StreamReader sr = new StreamReader(path))
        {
            html = sr.ReadToEnd();
        }

        HtmlParser parser = new HtmlTextParser(html);
        parser.Parse();
         await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current.Children.ElementAt(0);

        while (node.Tag != "body")
        {
            node = node.Next;
        }

        node = node.Children.ElementAt(0);

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        node.AnalyzeNode("div", "test", "<div id=\"gray\">test</div>", node.Parent, false, true, 1, 1, 1);
        node.Styles.CheckKeyValuePair(0, "background-color", "#eee");
    }

    [Fact]
    public async Task BackgroundInheritance()
    {
        string path = TestUtility.GetFolderPath("Html\\backgroundinheritance.htm");
        string html = string.Empty;

        using (StreamReader sr = new StreamReader(path))
        {
            html = sr.ReadToEnd();
        }

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;

        while (node.Tag != "html")
            node = node.Next;

        node = node.Children.ElementAt(0);

        while (node.Tag != "body")
            node = node.Next;

        node = node.Children.ElementAt(0);

        while (node.Tag != "div")
            node = node.Next;

        node = node.Children.ElementAt(0);

        while (node.Tag != "a")
            node = node.Next;

        Assert.Single(node.Styles);
        node.CheckStyle(0, "background", "blue");

        Assert.Single(node.InheritedStyles);
        node.CheckInheritedStyle(0, "background", "blue");
    }

    [Fact]
    public async Task BackgroundColorSpecifity()
    {
        string path = TestUtility.GetFolderPath("Html\\backgroundcolorspecificity.htm");
        string html = string.Empty;

        using (StreamReader sr = new StreamReader(path))
        {
            html = sr.ReadToEnd();
        }

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;

        while (node.Tag != "html")
            node = node.Next;

        node = node.Children.ElementAt(0);

        while (node.Tag != "body")
            node = node.Next;

        node = node.Children.ElementAt(0);

        while (node.Tag != "ul")
            node = node.Next;

        node = node.Children.ElementAt(0);

        while (node.Tag != "li")
            node = node.Next;

        node = node.Children.ElementAt(0);

        while (node.Tag != "a")
            node = node.Next;

        Assert.Single(node.Styles);
        node.CheckStyle(0, "background-color", "red");

        Assert.Single(node.InheritedStyles);
        node.CheckInheritedStyle(0, "background-color", "red");
    }
}
