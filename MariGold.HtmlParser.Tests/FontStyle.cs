﻿namespace MariGold.HtmlParser.Tests;

using MariGold.HtmlParser;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class FontStyle
{
    [Fact]
    public async Task FontSizeOnly()
    {
        string html = "<div style='font:25px'><div style='font-family:verdana'>test</div></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "<div style='font-family:verdana'>test</div>", html, null, false, true, 1, 1, 1);
        node.CheckStyle(0, "font", "25px");

        IHtmlNode parent = node;
        node = node.Children.ElementAt(0);
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-family:verdana'>test</div>", parent, false, true, 1, 1, 1);
        node.CheckStyle(0, "font-family", "verdana");
    }

    [Fact]
    public async Task FontSizeFontFamily()
    {
        string html = "<div style='font:25px arial'><div style='font-family:verdana'>test</div></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "<div style='font-family:verdana'>test</div>", html, null, false, true, 1, 1, 2);
        node.CheckStyle(0, "font-size", "25px");
        node.CheckStyle(1, "font-family", "arial");

        IHtmlNode parent = node;
        node = node.Children.ElementAt(0);
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-family:verdana'>test</div>", parent, false, true, 1, 1, 1);
        node.CheckStyle(0, "font-family", "verdana");
        Assert.Equal(2, node.InheritedStyles.Count);
        node.CheckInheritedStyle(0, "font-size", "25px");
        node.CheckInheritedStyle(1, "font-family", "verdana");
    }

    [Fact]
    public async Task FontSizeLineHeightFontFamily()
    {
        string html = "<div style='font:25px/2pt arial'><div style='font-family:verdana'>test</div></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "<div style='font-family:verdana'>test</div>", html, null, false, true, 1, 1, 3);
        node.CheckStyle(0, "font-size", "25px");
        node.CheckStyle(1, "line-height", "2pt");
        node.CheckStyle(2, "font-family", "arial");

        IHtmlNode parent = node;
        node = node.Children.ElementAt(0);
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-family:verdana'>test</div>", parent, false, true, 1, 1, 1);
        node.CheckStyle(0, "font-family", "verdana");
        Assert.Equal(3, node.InheritedStyles.Count);
        node.CheckInheritedStyle(0, "font-size", "25px");
        node.CheckInheritedStyle(1, "line-height", "2pt");
        node.CheckInheritedStyle(2, "font-family", "verdana");
    }

    [Fact]
    public async Task ParentFont()
    {
        string html = "<div style='font:arial'><div style='font-size:10px'>test</div></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 1);
        node.CheckStyle(0, "font", "arial");

        IHtmlNode parent = node;
        node = node.Children.ElementAt(0);
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
        node.CheckStyle(0, "font-size", "10px");
    }

    [Fact]
    public async Task ChildFont()
    {
        string html = "<div style='font:8pt arial'><div style='font-size:10px'>test</div></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 2);
        node.CheckStyle(0, "font-size", "8pt");
        node.CheckStyle(1, "font-family", "arial");


        IHtmlNode parent = node;
        node = node.Children.ElementAt(0);
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
        node.CheckStyle(0, "font-size", "10px");
        Assert.Equal(2, node.InheritedStyles.Count);
        node.CheckInheritedStyle(0, "font-family", "arial");
        node.CheckInheritedStyle(1, "font-size", "10px");
    }

    [Fact]
    public async Task FontWeightNormal()
    {
        string html = "<div style='font:normal 8pt arial'><div style='font-size:10px'>test</div></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 3);
        node.CheckStyle(0, "font-weight", "normal");
        node.CheckStyle(1, "font-size", "8pt");
        node.CheckStyle(2, "font-family", "arial");

        IHtmlNode parent = node;
        node = node.Children.ElementAt(0);
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
        node.CheckStyle(0, "font-size", "10px");
        Assert.Equal(3, node.InheritedStyles.Count);
        node.CheckInheritedStyle(0, "font-weight", "normal");
        node.CheckInheritedStyle(1, "font-family", "arial");
        node.CheckInheritedStyle(2, "font-size", "10px");
    }

    [Fact]
    public async Task FontVariantFontWeightNormal()
    {
        string html = "<div style='font:normal inherit 1em arial'><div style='font-size:10px'>test</div></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 4);
        node.CheckStyle(0, "font-weight", "inherit");
        node.CheckStyle(1, "font-variant", "normal");
        node.CheckStyle(2, "font-size", "1em");
        node.CheckStyle(3, "font-family", "arial");

        IHtmlNode parent = node;
        node = node.Children.ElementAt(0);
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
        node.CheckStyle(0, "font-size", "10px");
        Assert.Equal(4, node.InheritedStyles.Count);
        node.CheckInheritedStyle(0, "font-weight", "inherit");
        node.CheckInheritedStyle(1, "font-variant", "normal");
        node.CheckInheritedStyle(2, "font-family", "arial");
        node.CheckInheritedStyle(3, "font-size", "10px");
    }

    [Fact]
    public async Task FontStyleFontVariantFontWeightNormal()
    {
        string html = "<div style='font:initial normal inherit 1em arial'><div style='font-size:10px'>test</div></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 5);
        node.CheckStyle(0, "font-weight", "inherit");
        node.CheckStyle(1, "font-variant", "normal");
        node.CheckStyle(2, "font-style", "initial");
        node.CheckStyle(3, "font-size", "1em");
        node.CheckStyle(4, "font-family", "arial");

        IHtmlNode parent = node;
        node = node.Children.ElementAt(0);
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
        node.CheckStyle(0, "font-size", "10px");
        Assert.Equal(5, node.InheritedStyles.Count);
        node.CheckInheritedStyle(0, "font-weight", "inherit");
        node.CheckInheritedStyle(1, "font-variant", "normal");
        node.CheckInheritedStyle(2, "font-style", "initial");
        node.CheckInheritedStyle(3, "font-family", "arial");
        node.CheckInheritedStyle(4, "font-size", "10px");
    }

    [Fact]
    public async Task FontStyleFontVariantFontWeightNormalFontSizeLineHeight()
    {
        string html = "<div style='font:initial normal inherit 1em/2px arial'><div style='font-size:10px'>test</div></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 6);
        node.CheckStyle(0, "font-weight", "inherit");
        node.CheckStyle(1, "font-variant", "normal");
        node.CheckStyle(2, "font-style", "initial");
        node.CheckStyle(3, "font-size", "1em");
        node.CheckStyle(4, "line-height", "2px");
        node.CheckStyle(5, "font-family", "arial");

        IHtmlNode parent = node;
        node = node.Children.ElementAt(0);
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
        node.CheckStyle(0, "font-size", "10px");
        Assert.Equal(6, node.InheritedStyles.Count);

        node.CheckInheritedStyle(0, "font-weight", "inherit");
        node.CheckInheritedStyle(1, "font-variant", "normal");
        node.CheckInheritedStyle(2, "font-style", "initial");
        node.CheckInheritedStyle(3, "line-height", "2px");
        node.CheckInheritedStyle(4, "font-family", "arial");
        node.CheckInheritedStyle(5, "font-size", "10px");
    }

    [Fact]
    public async Task FontStyleFontVariantFontWeightNormalFontSizeLineHeightCaption()
    {
        string html = "<div style='font:initial normal inherit 1em/2px arial  caption'><div style='font-size:10px'>test</div></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 6);
        node.CheckStyle(0, "font-weight", "inherit");
        node.CheckStyle(1, "font-variant", "normal");
        node.CheckStyle(2, "font-style", "initial");
        node.CheckStyle(3, "font-size", "1em");
        node.CheckStyle(4, "line-height", "2px");
        node.CheckStyle(5, "font-family", "arial");

        IHtmlNode parent = node;
        node = node.Children.ElementAt(0);
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
        node.CheckStyle(0, "font-size", "10px");
        Assert.Equal(6, node.InheritedStyles.Count);

        node.CheckInheritedStyle(0, "font-weight", "inherit");
        node.CheckInheritedStyle(1, "font-variant", "normal");
        node.CheckInheritedStyle(2, "font-style", "initial");
        node.CheckInheritedStyle(3, "line-height", "2px");
        node.CheckInheritedStyle(4, "font-family", "arial");
        node.CheckInheritedStyle(5, "font-size", "10px");
    }

    [Fact]
    public async Task FontSizeLineHeightFontFamilyCaption()
    {
        string html = "<div style='font:8pt/2px verdana caption'><div style='font-size:10px'>test</div></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 3);
        node.CheckStyle(0, "font-size", "8pt");
        node.CheckStyle(1, "line-height", "2px");
        node.CheckStyle(2, "font-family", "verdana");

        IHtmlNode parent = node;
        node = node.Children.ElementAt(0);
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
        node.CheckStyle(0, "font-size", "10px");
        Assert.Equal(3, node.InheritedStyles.Count);
        node.CheckInheritedStyle(0, "line-height", "2px");
        node.CheckInheritedStyle(1, "font-family", "verdana");
        node.CheckInheritedStyle(2, "font-size", "10px");

    }

    [Fact]
    public async Task FontWeightFontSizeFontFamilyMessageBox()
    {
        string html = "<div style='font:bold 8pt verdana message-box'><div style='font-size:10px'>test</div></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 3);
        node.CheckStyle(0, "font-weight", "bold");
        node.CheckStyle(1, "font-size", "8pt");
        node.CheckStyle(2, "font-family", "verdana");

        IHtmlNode parent = node;
        node = node.Children.ElementAt(0);
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
        node.CheckStyle(0, "font-size", "10px");
        Assert.Equal(3, node.InheritedStyles.Count);
        node.CheckInheritedStyle(0, "font-weight", "bold");
        node.CheckInheritedStyle(1, "font-family", "verdana");
        node.CheckInheritedStyle(2, "font-size", "10px");
    }

    [Fact]
    public async Task FontVariantFontWeightFontSizeFontFamilyMessageBox()
    {
        string html = "<div style='font:small-caps  bold 8pt  verdana  message-box'><div style='font-size:10px'>test</div></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 4);
        node.CheckStyle(0, "font-variant", "small-caps");
        node.CheckStyle(1, "font-weight", "bold");
        node.CheckStyle(2, "font-size", "8pt");
        node.CheckStyle(3, "font-family", "verdana");

        IHtmlNode parent = node;
        node = node.Children.ElementAt(0);
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
        node.CheckStyle(0, "font-size", "10px");
        Assert.Equal(4, node.InheritedStyles.Count);
        node.CheckInheritedStyle(0, "font-variant", "small-caps");
        node.CheckInheritedStyle(1, "font-weight", "bold");
        node.CheckInheritedStyle(2, "font-family", "verdana");
        node.CheckInheritedStyle(3, "font-size", "10px");
    }

    [Fact]
    public async Task FontStyleFontVariantFontWeightFontSizeFontFamilyMessageBox()
    {
        string html = "<div style='font: oblique small-caps  bold 8pt  verdana  message-box'><div style='font-size:10px'>test</div></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "<div style='font-size:10px'>test</div>", html, null, false, true, 1, 1, 5);
        node.CheckStyle(0, "font-style", "oblique");
        node.CheckStyle(1, "font-variant", "small-caps");
        node.CheckStyle(2, "font-weight", "bold");
        node.CheckStyle(3, "font-size", "8pt");
        node.CheckStyle(4, "font-family", "verdana");

        IHtmlNode parent = node;
        node = node.Children.ElementAt(0);
        Assert.NotNull(node);
        TestUtility.AnalyzeNode(node, "div", "test", "<div style='font-size:10px'>test</div>", parent, false, true, 1, 1, 1);
        node.CheckStyle(0, "font-size", "10px");
        Assert.Equal(5, node.InheritedStyles.Count);
        node.CheckInheritedStyle(0, "font-style", "oblique");
        node.CheckInheritedStyle(1, "font-variant", "small-caps");
        node.CheckInheritedStyle(2, "font-weight", "bold");
        node.CheckInheritedStyle(3, "font-family", "verdana");
        node.CheckInheritedStyle(4, "font-size", "10px");
    }

    [Fact]
    public async Task Parent10pxChild2Em()
    {
        string html = "<div style=\"font-size:10px\"><div style=\"font-size:2em\">test</div></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;
        Assert.NotNull(node);

        node.AnalyzeNode("div", "<div style=\"font-size:2em\">test</div>", html, null, false, true, 1, 1, 1);
        node.CheckStyle(0, "font-size", "10px");
        IHtmlNode parent = node;

        node = node.Children.ElementAt(0);
        Assert.NotNull(node);
        node.AnalyzeNode("div", "test", "<div style=\"font-size:2em\">test</div>", parent, false, true, 1, 1, 1);
        node.CheckStyle(0, "font-size", "20px");
    }

    [Fact]
    public async Task InnerSpanRelativeFontSize()
    {
        string path = TestUtility.GetFolderPath("Html\\innerspanrelativefontsize.htm");
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

        node.CheckStyle(0, "font-size", "50px");

        node = node.Children.ElementAt(0);

        while (node.Tag != "span")
            node = node.Next;

        node.CheckStyle(0, "font-size", "25px");
    }

    [Fact]
    public async Task FiftyPercentageEM()
    {
        string html = "<div style=\"font-size:10px\"><div style=\"font-size:0.50em\">test</div></div>";

        HtmlParser parser = new HtmlTextParser(html);

        Assert.True(parser.Parse());
        await parser.ParseStylesAsync();

        IHtmlNode node = parser.Current;
        Assert.NotNull(node);

        node.AnalyzeNode("div", "<div style=\"font-size:0.50em\">test</div>", html, null, false, true, 1, 1, 1);
        node.CheckStyle(0, "font-size", "10px");
        IHtmlNode parent = node;

        node = node.Children.ElementAt(0);
        Assert.NotNull(node);
        node.AnalyzeNode("div", "test", "<div style=\"font-size:0.50em\">test</div>", parent, false, true, 1, 1, 1);
        node.CheckStyle(0, "font-size", "5px");
    }
}
