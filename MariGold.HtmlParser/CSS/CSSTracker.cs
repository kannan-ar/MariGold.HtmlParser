namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal sealed class CSSTracker
    {
        private const string rel = "stylesheet";

        internal string UriSchema { get; set; }

        internal string BaseURL { get; set; }

        private void TraverseHtmlNodes(HtmlNode node, StyleSheet styleSheet)
        {
            string style = string.Empty;

            if (string.Equals(node.Tag, HtmlTag.STYLE, StringComparison.OrdinalIgnoreCase))
            {
                style = node.InnerHtml == null ? string.Empty : node.InnerHtml.Trim();
            }
            else if (string.Equals(node.Tag, HtmlTag.LINK, StringComparison.OrdinalIgnoreCase))
            {
                string relValue = node.ExtractAttributeValue("rel");
                string media = node.ExtractAttributeValue("media");

                if (string.Equals(rel, relValue, StringComparison.OrdinalIgnoreCase) &&
                    (string.IsNullOrEmpty(media) || media.CompareOrdinalIgnoreCase("screen")
                    || media.CompareOrdinalIgnoreCase("all")))
                {
                    string url = node.ExtractAttributeValue("href");

                    if (!string.IsNullOrEmpty(url))
                    {
                        WebManager web = new WebManager(UriSchema, BaseURL);
                        style = web.ExtractStylesFromLink(url);
                    }
                }
            }

            if (!string.IsNullOrEmpty(style))
            {
                List<CSSElement> styles = new List<CSSElement>();
                List<MediaQuery> mediaQueries = new List<MediaQuery>();

                CSSParser parser = new CSSParser();
                parser.ParseCSS(style, styles, mediaQueries);

                styleSheet.AddRange(styles);
                styleSheet.AddMediaQueryRange(mediaQueries);
            }

            foreach (HtmlNode n in node.GetChildren())
            {
                TraverseHtmlNodes(n, styleSheet);
            }
        }

        internal StyleSheet TrackCSS(HtmlNode node)
        {
            StyleSheet styleSheet = new StyleSheet(new SelectorContext());

            TraverseHtmlNodes(node, styleSheet);

            HtmlNode nextNode = node.GetNext();

            while (nextNode != null)
            {
                TraverseHtmlNodes(nextNode, styleSheet);
                nextNode = nextNode.GetNext();
            }

            return styleSheet;
        }
    }
}
