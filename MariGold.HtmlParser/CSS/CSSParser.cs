namespace MariGold.HtmlParser.CSS
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    internal sealed class CSSParser
    {
        private const char openBrace = '{';
        private const char closeBrace = '}';

        private readonly CSSelector selector;

        private int ParseSelector(int position, string style, out CSSelector csSelector)
        {
            int openBraceIndex = style.IndexOf(openBrace, position);
            csSelector = null;

            if (openBraceIndex > position)
            {
                string selectorText = style.Substring(position, openBraceIndex - position).Trim();

                csSelector = selector.Parse(selectorText);
            }

            return openBraceIndex;
        }

        private HtmlStyle CreateHtmlStyleFromRule(string styleName, string value, SelectorWeight selectWeight)
        {
            styleName = styleName.Trim().Replace("\"", string.Empty).Replace("'", string.Empty);
            value = value.Trim();
            bool important = false;

            //Replace this with regular expression
            int importantIndex = value.IndexOf("!important");

            if (importantIndex > -1)
            {
                important = true;
                value = value.Replace("!important", string.Empty);
            }

            return new HtmlStyle(styleName, value, important, selectWeight);
        }

        private int ParseStyles(int position, string styleText, CSSelector csSelector)
        {
            int closeBraceIndex = styleText.IndexOf(closeBrace, position);

            if (closeBraceIndex > position)
            {
                string styles = styleText.Substring(position + 1, closeBraceIndex - position - 1);

                if (!string.IsNullOrEmpty(styles))
                {
                    csSelector.AddRange(ParseRules(styles, csSelector.Weight));
                }
            }

            return closeBraceIndex;
        }

        private string ExtractStylesFromLink(string url)
        {
            string styles = string.Empty;

            using (WebClient client = new WebClient())
            {
                styles = client.DownloadString(url);
            }

            return styles;
        }

        private void ParseCSS(string style, StyleSheet styleSheet)
        {
            int eof = style.Length;
            int position = 0;

            while (position < eof)
            {
                CSSelector csSelector;

                int bracePosition = ParseSelector(position, style, out csSelector);

                if (bracePosition > position && csSelector != null)
                {
                    //Returning close brace index
                    bracePosition = ParseStyles(bracePosition, style, csSelector);

                    styleSheet.Add(csSelector);
                }

                if (bracePosition == -1)
                {
                    break;
                }

                position = bracePosition;
            }
        }

        private void ParseHtmlNode(HtmlNode node, StyleSheet styleSheet)
        {
            string style = string.Empty;

            if (string.Compare(node.Tag, HtmlTag.STYLE, true) == 0)
            {
                style = node.InnerHtml == null ? string.Empty : node.InnerHtml.Trim();
            }
            else if (string.Compare(node.Tag, HtmlTag.LINK, true) == 0)
            {
                string url;

                if (node.Attributes.TryGetValue("href", out url))
                {
                    style = ExtractStylesFromLink(url);
                }
            }

            if (!string.IsNullOrEmpty(style))
            {
                ParseCSS(style, styleSheet);
            }

            foreach (HtmlNode n in node.Children)
            {
                ParseHtmlNode(n, styleSheet);
            }
        }

        internal CSSParser()
        {
            selector = new ClassSelector().SetSuccessor(
                new IdentitySelector());
        }

        internal StyleSheet Parse(HtmlNode node)
        {
            StyleSheet styleSheet = new StyleSheet();

            ParseHtmlNode(node, styleSheet);

            return styleSheet;
        }

        internal IEnumerable<HtmlStyle> ParseRules(string styleText, SelectorWeight selectorWeight)
        {
            string[] styleSet = styleText.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string style in styleSet)
            {
                string styleNode = style.Trim();

                if (!string.IsNullOrEmpty(styleNode))
                {
                    string[] nodeSet = styleNode.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                    if (nodeSet != null && nodeSet.Length > 1)
                    {
                        yield return CreateHtmlStyleFromRule(nodeSet[0], nodeSet[1], selectorWeight);
                    }
                }
            }
        }

        internal void InterpretStyles(StyleSheet styleSheet, HtmlNode htmlNode)
        {
            string style;

            if (htmlNode.Attributes.TryGetValue("style", out style))
            {
                htmlNode.AddStyles(ParseRules(style, SelectorWeight.Inline));
            }

            styleSheet.Parse(htmlNode);

            foreach (HtmlNode node in htmlNode.Children)
            {
                InterpretStyles(styleSheet, node);
            }
        }
    }
}
