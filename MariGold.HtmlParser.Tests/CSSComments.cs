namespace MariGold.HtmlParser.Tests;

using MariGold.HtmlParser;
using System.Linq;
using System.IO;
using Xunit;
using System.Threading.Tasks;

public class CSSComments
{
    [Fact]
    public async Task CommentWithMediaQuery()
    {
        string path = TestUtility.GetFolderPath("Html\\commentwithmediaquery.htm");
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

        IHtmlNode body = node;

        node = node.Children.ElementAt(0);

        while (node.Tag != "p")
            node = node.Next;

        TestUtility.AnalyzeNode(node, "p", "test", "<p>test</p>", body, false, true, 1, 0, 0);
    }

    [Fact]
    public async Task CommentWithoutSpace()
    {
        string html = @"<style>/*styles*/.cls{color:red;}</style>
                            <div class='cls'>test</div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
            node = node.Next;

        TestUtility.AnalyzeNode(node, "div", "test", "<div class='cls'>test</div>", null, false, true, 1, 1, 1);
        node.CheckStyle(0, "color", "red");
    }

    [Fact]
    public async Task CommentInStyles()
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

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
            node = node.Next;

        TestUtility.AnalyzeNode(node, "div", "test", "<div class='cls'>test</div>", null, false, true, 1, 1, 1);
        node.CheckStyle(0, "color", "white");
    }

    [Fact]
    public async Task MultipleCommentsInStyles()
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

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
            node = node.Next;

        TestUtility.AnalyzeNode(node, "div", "test", "<div class='cls'>test</div>", null, false, true, 1, 1, 1);
        node.CheckStyle(0, "color", "white");
    }

    [Fact]
    public async Task MultipleCommentsInStylesWithoutSpace()
    {
        string html = @"<style>.cls{/*color:red;*/color:white;/*color:blue*/}</style><div class='cls'>test</div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
            node = node.Next;

        TestUtility.AnalyzeNode(node, "div", "test", "<div class='cls'>test</div>", null, false, true, 1, 1, 1);
        node.CheckStyle(0, "color", "white");
    }

    [Fact]
    public async Task MultipleCommentsInStyleSheet()
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

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
            node = node.Next;

        TestUtility.AnalyzeNode(node, "div", "test", "<div class='cls fnt'>test</div>", null, false, true, 1, 1, 2);
        node.CheckStyle(0, "color", "white");
        node.CheckStyle(1, "font-weight", "bold");
    }
}
