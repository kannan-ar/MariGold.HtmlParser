namespace MariGold.HtmlParser.Tests;

using System;
using System.Linq;
using MariGold.HtmlParser;
using System.Collections.Generic;
using Xunit;

public static class TestUtility
{
    public static void AreEqual(IHtmlNode node, string tag, string text, string html)
    {
        Assert.Equal(tag, node.Tag);
        Assert.Equal(text, node.InnerHtml);
        Assert.Equal(html, node.Html);
    }

    public static void CheckKeyValuePair(KeyValuePair<string, string> attribute, string key, string value)
    {
        Assert.Equal(key, attribute.Key);
        Assert.Equal(value, attribute.Value);
    }

    public static string GetFolderPath(string folderName)
    {
        string path = Environment.CurrentDirectory;
        int index = path.IndexOf("bin");

        path = path.Remove(index);

        return path + folderName;
    }

    public static void AnalyzeNode(
        IHtmlNode node,
        string tag,
        string text,
        string html,
        IHtmlNode parent,
        bool selfClosing,
        bool hasChildren,
        int childrenCount,
        int attributeCount)
    {
        Assert.NotNull(node);
        AreEqual(node, tag, text, html);
        Assert.Equal(parent, node.Parent);
        Assert.Equal(selfClosing, node.SelfClosing);
        Assert.Equal(hasChildren, node.HasChildren);
        Assert.Equal(childrenCount, node.Children.Count());
        Assert.Equal(attributeCount, node.Attributes.Count);
    }

    public static void AnalyzeNode(
        IHtmlNode node,
        string tag,
        string text,
        string html,
        IHtmlNode parent,
        bool selfClosing,
        bool hasChildren,
        int childrenCount,
        int attributeCount,
        int styleCount)
    {
        Assert.NotNull(node);
        AreEqual(node, tag, text, html);
        Assert.Equal(parent, node.Parent);
        Assert.Equal(selfClosing, node.SelfClosing);
        Assert.Equal(hasChildren, node.HasChildren);
        Assert.True(childrenCount == node.Children.Count(), "Children count is " + node.Children.Count().ToString());
        Assert.True(attributeCount == node.Attributes.Count, "Attribute count is " + node.Attributes.Count.ToString());
        Assert.True(styleCount == node.Styles.Count, "Style count is " + node.Styles.Count.ToString());
    }

    public static void CheckStyle(this IHtmlNode node, int index, string styleName, string styleValue)
    {
        Assert.True(index < node.Styles.Count, "No style at position " + index.ToString());
        KeyValuePair<string, string> style = node.Styles.ElementAt(index);
        Assert.Equal(styleName, style.Key);
        Assert.Equal(styleValue, style.Value);
    }

    public static void CheckInheritedStyle(this IHtmlNode node, int index, string styleName, string styleValue)
    {
        Assert.True(index < node.InheritedStyles.Count, "No style at position " + index.ToString());
        KeyValuePair<string, string> style = node.InheritedStyles.ElementAt(index);
        Assert.Equal(styleName, style.Key);
        Assert.Equal(styleValue, style.Value);
    }
}
