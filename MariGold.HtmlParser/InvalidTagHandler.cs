namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal class InvalidTagHandler
    {
        private static Dictionary<string, List<string>> nonNestedTags;

        static InvalidTagHandler()
        {
            nonNestedTags = new Dictionary<string, List<string>>();

            nonNestedTags.Add("li", new List<string> { "li" });
            nonNestedTags.Add("td", new List<string> { "td" });
            nonNestedTags.Add("tr", new List<string> { "tr" });
        }

        internal void CloseNonNestedParents(int htmlStart, string tag, IAnalyzerContext context, ref HtmlNode parent)
        {
            if (parent == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(tag) || string.IsNullOrEmpty(parent.Tag))
            {
                return;
            }

            List<string> parentTags;

            if (nonNestedTags.TryGetValue(tag.Trim().ToLower(), out parentTags))
            {
                if (parentTags.Contains(parent.Tag.Trim().ToLower()))
                {
                    parent.SetBoundary(htmlStart, htmlStart);
                    context.PreviousNode = parent;
                    parent = parent.GetParent();
                }
            }
        }
    }
}
