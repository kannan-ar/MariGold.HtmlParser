namespace MariGold.HtmlParser
{
    using System;
    using System.Net;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal sealed class CSSParser
    {
        private const string rel = "stylesheet";
        private Regex mediaRegex;

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
        
        internal CSSParser()
        {
            mediaRegex = new Regex(@"^\s*@media");
        }
        
        private int ParseSelector(int position, string style, out string selectorText)
        {
            selectorText = string.Empty;

            int openBraceIndex = style.IndexOf(CSSTokenizer.openBrace, position);

            if (openBraceIndex > position)
            {
                selectorText = style.Substring(position, openBraceIndex - position).Trim();
            }

            return openBraceIndex;
        }

        private HtmlStyle CreateHtmlStyleFromRule(string styleName, string value, SelectorWeight weight)
        {
            styleName = styleName.Trim().Replace("\"", string.Empty).Replace("'", string.Empty);
            value = value.Trim();
            bool important = false;

            //Replace this with regular expression
            int importantIndex = value.IndexOf("!important");

            if (importantIndex > -1)
            {
                important = true;
                value = value.Replace("!important", string.Empty).Trim();
            }

            return new HtmlStyle(styleName, value, important, weight);
        }

        private int ParseHtmlStyles(int position, string styleText, out List<HtmlStyle> htmlStyles)
        {
            htmlStyles = new List<HtmlStyle>();

            int closeBraceIndex = styleText.IndexOf(CSSTokenizer.closeBrace, position);

            if (closeBraceIndex > position)
            {
                string styles = styleText.Substring(position + 1, closeBraceIndex - position - 1);

                if (!string.IsNullOrEmpty(styles))
                {
                    htmlStyles = ParseRules(styles, SelectorWeight.None).ToList();
                }
            }

            //+1 to advance to next location
            return closeBraceIndex + 1;
        }
        
        private bool IsMediaQuery(string selectorText, string styleText, ref int position)
        {
            if (string.IsNullOrEmpty(styleText))
            {
                return false;
            }

            Match match = mediaRegex.Match(selectorText);

            if (!match.Success)
            {
                return false;
            }

            int closeBraceIndex = 0;
            string style = CSSTokenizer.FindOpenCloseBraceArea(styleText, position + 1, out closeBraceIndex);

            if (closeBraceIndex > position)
            {
                if (!string.IsNullOrEmpty(style))
                {
                    List<KeyValuePair<string, List<HtmlStyle>>> styles = new List<KeyValuePair<string, List<HtmlStyle>>>();

                    var htmlStyles = ParseCSS(style);

                    foreach (var htmlStyle in htmlStyles)
                    {
                        string selecters = htmlStyle.Key;

                        foreach (string selector in selecters.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            styles.Add(new KeyValuePair<string, List<HtmlStyle>>(selector.Trim(), htmlStyle.Value));
                        }
                    }

                    selectorText = selectorText.Substring(match.Length + 1);

                    //styleSheet.AddMediaQuery(selectorText, styles);
                }

                position = closeBraceIndex + 1;
            }

            return true;
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
                    (string.IsNullOrEmpty(media) || media.CompareStringInvariantCultureIgnoreCase("screen")))
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
                var styles = ParseCSS(style);

                foreach (var styleNode in styles)
                {
                    styleSheet.AddRange(styleNode.Key, styleNode.Value);
                }
            }

            foreach (HtmlNode n in node.GetChildren())
            {
                TraverseHtmlNodes(n, styleSheet);
            }
        }

        internal IEnumerable<HtmlStyle> ParseRules(string styleText, SelectorWeight weight)
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
                        yield return CreateHtmlStyleFromRule(nodeSet[0], nodeSet[1], weight);
                    }
                }
            }
        }

        internal List<KeyValuePair<string, List<HtmlStyle>>> ParseCSS(string style)
        {
            //MediaQuery mediaQuery=new MediaQuery(sty)
            int eof = style.Length;
            int position = 0;
            List<KeyValuePair<string, List<HtmlStyle>>> styles = new List<KeyValuePair<string, List<HtmlStyle>>>();

            while (position < eof)
            {
                string selectorText;

                int bracePosition = ParseSelector(position, style, out selectorText);

                if (bracePosition > position && selectorText != string.Empty)
                {
                    List<HtmlStyle> htmlStyles;

                    if (!IsMediaQuery(selectorText, style, ref bracePosition))
                    {
                        //Returning close brace index
                        bracePosition = ParseHtmlStyles(bracePosition, style, out htmlStyles);
                        styles.Add(new KeyValuePair<string, List<HtmlStyle>>(selectorText, htmlStyles));
                        //action(selectorText, htmlStyles);
                    }
                }

                if (bracePosition == -1)
                {
                    break;
                }

                position = bracePosition;
            }

            return styles;
        }

        internal StyleSheet ParseStyleSheet(HtmlNode node)
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
