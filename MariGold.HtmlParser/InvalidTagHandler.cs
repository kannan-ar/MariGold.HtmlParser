namespace MariGold.HtmlParser
{
    using System.Collections.Generic;

    internal class InvalidTagHandler
    {
        private static Dictionary<string, List<string>> nonNestedTags;

        static InvalidTagHandler()
        {
            nonNestedTags = new Dictionary<string, List<string>>
            {
                { "li", new List<string> { "li" } },
                { "td", new List<string> { "td" } },
                { "tr", new List<string> { "tr" } }
            };
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

            if (nonNestedTags.TryGetValue(tag.Trim().ToLower(), out List<string> parentTags))
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
