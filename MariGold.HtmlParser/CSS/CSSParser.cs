namespace MariGold.HtmlParser.CSS
{
    using System;

    internal sealed class CSSParser
    {
        private const char openBrace = '{';
        private const char closeBrace = '}';

        private readonly CSSelector selector;

        private int ParseSelector(int position, string style, out CSSelector csSelector)
        {
            int openBraceIndex = style.IndexOf(openBrace);
            csSelector = null;

            if (openBraceIndex > position)
            {
                string selectorText = style.Substring(position, openBraceIndex - position).Trim();

                csSelector = selector.Parse(selectorText);
            }

            return openBraceIndex;
        }

        private void FormatStyle(string styleName, string value, CSSelector csSelector)
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

            csSelector.AddStyle(new HtmlStyle(styleName, value, important, csSelector.Weight));
        }

        private void ParseRules(string styleText,CSSelector csSelector)
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
                        FormatStyle(nodeSet[0], nodeSet[1], csSelector);
                    }
                }
            }
        }

        private void ParseStyles(int position, string styleText, CSSelector csSelector)
        {
            int closeBraceIndex = styleText.IndexOf(closeBrace);

            if (closeBraceIndex > position)
            {
                string styles = styleText.Substring(position, closeBraceIndex - position);

                if (!string.IsNullOrEmpty(styleText))
                {
                    ParseRules(styleText, csSelector);
                }
            }
        }

        private string ExtractStylesFromLink(string url)
        {
            throw new NotImplementedException();
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
                    ParseStyles(bracePosition, style, csSelector);

                    styleSheet.Add(csSelector);
                }
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
    }
}
