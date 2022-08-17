namespace MariGold.HtmlParser.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public static class TestExtensions
    {
        public static void AnalyzeNode(
            this IHtmlNode node,
            string tag,
            string text,
            string html,
            IHtmlNode parent,
            bool selfClosing,
            bool hasChildren,
            int childrenCount,
            int attributeCount)
        {
            TestUtility.AnalyzeNode(node, tag, text, html, parent, selfClosing, hasChildren, childrenCount, attributeCount);
        }

        public static void AnalyzeNode(
            this IHtmlNode node,
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
            TestUtility.AnalyzeNode(node, tag, text, html, parent, selfClosing, hasChildren, childrenCount, attributeCount, styleCount);
        }

        public static void CheckKeyValuePair(this Dictionary<string, string> dict, int index, string key, string value)
        {
            var attribute = dict.ElementAt(index);

            Assert.Equal(key, attribute.Key);
            Assert.Equal(value, attribute.Value);
        }
    }
}
