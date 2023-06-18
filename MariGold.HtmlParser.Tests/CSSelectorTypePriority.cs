namespace MariGold.HtmlParser.Tests;

using MariGold.HtmlParser;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class CSSelectorTypePriority
{
    [Fact]
    public async Task InlineIdentity()
    {
        string html = @"<style>
                                .cls
                                {
                                	color:#fff;
                                }
                            </style>
                            <div class='cls' style='color:#000'></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        Assert.NotNull(parser.Current);

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        TestUtility.AnalyzeNode(node, "div", "", "<div class='cls' style='color:#000'></div>", null, false, false, 0, 2, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "style", "color:#000");
        TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "#000");
    }

    [Fact]
    public async Task ClassAttribute()
    {
        string html = @"<style>
                                .cls
                                {
                                	color:red;
                                }
                                
                                [attr]
                                {
                                	color:blue;
                                }
                            </style>
                            <div class='cls' attr></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        Assert.NotNull(parser.Current);

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        TestUtility.AnalyzeNode(node, "div", "", "<div class='cls' attr></div>", null, false, false, 0, 2, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "attr", "");
        TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "blue");
    }

    [Fact]
    public async Task AttributeClass()
    {
        string html = @"<style>
			
                                [attr]
                                {
                                	color:blue;
                                }
                                
                                .cls
                                {
                                	color:red;
                                }
                                
                            </style>
                            <div class='cls' attr></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        Assert.NotNull(parser.Current);

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        TestUtility.AnalyzeNode(node, "div", "", "<div class='cls' attr></div>", null, false, false, 0, 2, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "attr", "");
        TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "red");
    }

    [Fact]
    public async Task ElementClass()
    {
        string html = @"<style>
			
								div
                                {
                                	color:red;
                                }
                                
                                .cls
                                {
                                	color:blue;
                                }
                                
                            </style>
                            <div class='cls'></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        Assert.NotNull(parser.Current);

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        TestUtility.AnalyzeNode(node, "div", "", "<div class='cls'></div>", null, false, false, 0, 1, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
        TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "blue");
    }

    [Fact]
    public async Task ClassElement()
    {
        string html = @"<style>
			
                                .cls
                                {
                                	color:blue;
                                }
                                
                                div
                                {
                                	color:red;
                                }
                                
                            </style>
                            <div class='cls'></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        Assert.NotNull(parser.Current);

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        TestUtility.AnalyzeNode(node, "div", "", "<div class='cls'></div>", null, false, false, 0, 1, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
        TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "blue");
    }

    [Fact]
    public async Task ClassIdentity()
    {
        string html = @"<style>
			
                                .cls
                                {
                                	color:blue;
                                }
                                
                                #dv
                                {
                                	color:red;
                                }
                                
                            </style>
                            <div id='dv' class='cls'></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        Assert.NotNull(parser.Current);

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        TestUtility.AnalyzeNode(node, "div", "", "<div id='dv' class='cls'></div>", null, false, false, 0, 2, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "id", "dv");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "class", "cls");
        TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "red");
    }

    [Fact]
    public async Task IdentityClass()
    {
        string html = @"<style>
			
                                #dv
                                {
                                	color:red;
                                }
                                
                                .cls
                                {
                                	color:blue;
                                }
                                
                            </style>
                            <div id='dv' class='cls'></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        Assert.NotNull(parser.Current);

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        TestUtility.AnalyzeNode(node, "div", "", "<div id='dv' class='cls'></div>", null, false, false, 0, 2, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "id", "dv");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "class", "cls");
        TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "red");
    }

    [Fact]
    public async Task GlobalClass()
    {
        string html = @"<style>
			
                                *
                                {
                                	color:red;
                                }
                                
                                .cls
                                {
                                	color:blue;
                                }
                                
                            </style>
                            <div class='cls'></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        Assert.NotNull(parser.Current);

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        TestUtility.AnalyzeNode(node, "div", "", "<div class='cls'></div>", null, false, false, 0, 1, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
        TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "blue");
    }

    [Fact]
    public async Task ClassImportantInline()
    {
        string html = @"<style>
			
                                .cls
                                {
                                	color:blue !important;
                                }
                                
                            </style>
                            <div class='cls' style='color:red'></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        Assert.NotNull(parser.Current);

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        TestUtility.AnalyzeNode(node, "div", "", "<div class='cls' style='color:red'></div>", null, false, false, 0, 2, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "style", "color:red");
        TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "blue");
    }

    [Fact]
    public async Task ClassImportantInlineImportant()
    {
        string html = @"<style>
			
                                .cls
                                {
                                	color:blue !important;
                                }
                                
                            </style>
                            <div class='cls' style='color:red !important'></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        Assert.NotNull(parser.Current);

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        TestUtility.AnalyzeNode(node, "div", "", "<div class='cls' style='color:red !important'></div>", null, false, false, 0, 2, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "style", "color:red !important");
        TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "red");
    }

    [Fact]
    public async Task ElementImportantClassInline()
    {
        string html = @"<style>
			
								div
								{
									color:black !important;
								}
								
                                .cls
                                {
                                	color:blue;
                                }
                                
                            </style>
                            <div class='cls' style='color:red'></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        Assert.NotNull(parser.Current);

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        TestUtility.AnalyzeNode(node, "div", "", "<div class='cls' style='color:red'></div>", null, false, false, 0, 2, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "style", "color:red");
        TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "black");
    }

    [Fact]
    public async Task ElementImportantClassInlineImportant()
    {
        string html = @"<style>
			
								div
								{
									color:black !important;
								}
								
                                .cls
                                {
                                	color:blue;
                                }
                                
                            </style>
                            <div class='cls' style='color:red !important'></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        Assert.NotNull(parser.Current);

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        TestUtility.AnalyzeNode(node, "div", "", "<div class='cls' style='color:red !important'></div>", null, false, false, 0, 2, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "style", "color:red !important");
        TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "red");
    }

    [Fact]
    public async Task ElementImportantClassImportantInline()
    {
        string html = @"<style>
			
								div
								{
									color:black !important;
								}
								
                                .cls
                                {
                                	color:blue !important;
                                }
                                
                            </style>
                            <div class='cls' style='color:red'></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        Assert.NotNull(parser.Current);

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        TestUtility.AnalyzeNode(node, "div", "", "<div class='cls' style='color:red'></div>", null, false, false, 0, 2, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "style", "color:red");
        TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "blue");
    }

    [Fact]
    public async Task ClassElementImportantInline()
    {
        string html = @"<style>
			
                                .cls
                                {
                                	color:blue;
                                }
                                
                                div
								{
									color:black !important;
								}
								
                            </style>
                            <div class='cls' style='color:red'></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        Assert.NotNull(parser.Current);

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        TestUtility.AnalyzeNode(node, "div", "", "<div class='cls' style='color:red'></div>", null, false, false, 0, 2, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "style", "color:red");
        TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "black");
    }

    [Fact]
    public async Task ClassImportantElementInline()
    {
        string html = @"<style>
			
                                .cls
                                {
                                	color:blue !important;
                                }
                                
                                div
								{
									color:black;
								}
								
                            </style>
                            <div class='cls' style='color:red'></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        Assert.NotNull(parser.Current);

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        TestUtility.AnalyzeNode(node, "div", "", "<div class='cls' style='color:red'></div>", null, false, false, 0, 2, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "style", "color:red");
        TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "blue");
    }

    [Fact]
    public async Task ClassImportantAttribute()
    {
        string html = @"<style>
			
                                .cls
                                {
                                	color:blue !important;
                                }
                                
                                [attr]
								{
									color:black;
								}
								
                            </style>
                            <div attr class='cls'></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        Assert.NotNull(parser.Current);

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        TestUtility.AnalyzeNode(node, "div", "", "<div attr class='cls'></div>", null, false, false, 0, 2, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "class", "cls");
        TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "blue");
    }

    [Fact]
    public async Task ClassImportantAttributeImportant()
    {
        string html = @"<style>
			
                                .cls
                                {
                                	color:blue !important;
                                }
                                
                                [attr]
								{
									color:black !important;
								}
								
                            </style>
                            <div attr class='cls'></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        Assert.NotNull(parser.Current);

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        TestUtility.AnalyzeNode(node, "div", "", "<div attr class='cls'></div>", null, false, false, 0, 2, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "class", "cls");
        TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "black");
    }

    [Fact]
    public async Task ClassAttributeInline()
    {
        string html = @"<style>
			
                                .cls
                                {
                                	color:blue;
                                }
                                
                                [attr]
								{
									color:black;
								}
								
                            </style>
                            <div attr class='cls' style='color:red'></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        Assert.NotNull(parser.Current);

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        TestUtility.AnalyzeNode(node, "div", "", "<div attr class='cls' style='color:red'></div>", null, false, false, 0, 3, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "class", "cls");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(2), "style", "color:red");
        TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "red");
    }

    [Fact]
    public async Task ClassImportantAttributeImportantInlineImportant()
    {
        string html = @"<style>
			
                                .cls
                                {
                                	color:blue !important;
                                }
                                
                                [attr]
								{
									color:black !important;
								}
								
                            </style>
                            <div attr class='cls' style='color:red !important'></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        Assert.NotNull(parser.Current);

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        TestUtility.AnalyzeNode(node, "div", "", "<div attr class='cls' style='color:red !important'></div>", null, false, false, 0, 3, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "class", "cls");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(2), "style", "color:red !important");
        TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "red");
    }

    [Fact]
    public async Task GlobalImportantClassAttributeInline()
    {
        string html = @"<style>
								
								*
								{
									color:white !important;
								}
								
                                .cls
                                {
                                	color:blue;
                                }
                                
                                [attr]
								{
									color:black;
								}
								
                            </style>
                            <div attr class='cls' style='color:red'></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
         await parser.ParseStylesAsync();

        Assert.NotNull(parser.Current);

        IHtmlNode node = parser.Current;

        while (node.Tag != "div")
        {
            node = node.Next;
        }

        TestUtility.AnalyzeNode(node, "div", "", "<div attr class='cls' style='color:red'></div>", null, false, false, 0, 3, 1);
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "class", "cls");
        TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(2), "style", "color:red");
        TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "white");
    }

    [Fact]
    public async Task NthChildPriority()
    {
        string path = TestUtility.GetFolderPath("Html\\nthchildpriority.htm");
        string html = string.Empty;

        using (StreamReader sr = new StreamReader(path))
        {
            html = sr.ReadToEnd();
        }

        HtmlParser parser = new HtmlTextParser(html);
        parser.Parse();
         await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;

        while (node.Tag != "html")
            node = node.Next;

        node = node.Children.ElementAt(0);

        while (node.Tag != "body")
            node = node.Next;

        IHtmlNode div = node.Children.ElementAt(0);
        IHtmlNode p = div.Children.ElementAt(0);

        TestUtility.AnalyzeNode(p, "p", "ano", "<p>ano</p>", div, false, true, 1, 0, 1);
        TestUtility.CheckKeyValuePair(p.Styles.ElementAt(0), "color", "blue");

        div = div.Next;
        IHtmlNode span = div.Children.ElementAt(0);
        TestUtility.AnalyzeNode(span, "span", "test", "<span>test</span>", div, false, true, 1, 0, 1);
        TestUtility.CheckKeyValuePair(span.Styles.ElementAt(0), "color", "red");
    }
}
