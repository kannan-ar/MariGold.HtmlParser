namespace MariGold.HtmlParser;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

internal sealed class CSSTracker
{
    private const string rel = "stylesheet";

    internal string UriSchema { get; set; }

    internal string BaseURL { get; set; }

    private async Task TraverseHtmlNodesAsync(HtmlNode node, StyleSheet styleSheet)
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
                    WebManager web = new(UriSchema, BaseURL);
                    style = await web.ExtractStylesFromLinkAsync(url).ConfigureAwait(false);
                }
            }
        }

        if (!string.IsNullOrEmpty(style))
        {
            List<CSSElement> styles = new();
            List<MediaQuery> mediaQueries = new();

            CSSParser.ParseCSS(style, styles, mediaQueries);

            styleSheet.AddRange(styles);
            StyleSheet.AddMediaQueryRange(mediaQueries);
        }

        foreach (HtmlNode n in node.GetChildren())
        {
            await TraverseHtmlNodesAsync(n, styleSheet).ConfigureAwait(false);
        }
    }

    internal async Task<StyleSheet> TrackCSSAsync(HtmlNode node)
    {
        StyleSheet styleSheet = new(new SelectorContext());

        await TraverseHtmlNodesAsync(node, styleSheet).ConfigureAwait(false);

        HtmlNode nextNode = node.GetNext();

        while (nextNode != null)
        {
            await TraverseHtmlNodesAsync(nextNode, styleSheet).ConfigureAwait(false);
            nextNode = nextNode.GetNext();
        }

        return styleSheet;
    }
}
