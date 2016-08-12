namespace MariGold.HtmlParser
{
    using System;
    using System.Net;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal sealed class CSSTracker
    {
        private const string rel = "stylesheet";

        private string uriSchema;
        private string baseUrl;

        internal string UriSchema
        {
            get
            {
                return uriSchema;
            }

            set
            {
                uriSchema = value;
            }
        }

        internal string BaseURL
        {
            get
            {
                return baseUrl;
            }

            set
            {
                baseUrl = value;
            }
        }

        private void TraverseHtmlNodes(HtmlNode node, StyleSheet styleSheet)
        {
            string style = string.Empty;

            if (string.Compare(node.Tag, HtmlTag.STYLE, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                style = node.InnerHtml == null ? string.Empty : node.InnerHtml.Trim();
            }
            else if (string.Compare(node.Tag, HtmlTag.LINK, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                string relValue = node.ExtractAttributeValue("rel");
                string media = node.ExtractAttributeValue("media");

                if (string.Compare(rel, relValue, StringComparison.InvariantCultureIgnoreCase) == 0 &&
                    (string.IsNullOrEmpty(media) || media.CompareInvariantCultureIgnoreCase("screen")
                    || media.CompareInvariantCultureIgnoreCase("all")))
                {
                    string url = node.ExtractAttributeValue("href");

                    if (!string.IsNullOrEmpty(url))
                    {
                        WebManager web = new WebManager(uriSchema, baseUrl);
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
