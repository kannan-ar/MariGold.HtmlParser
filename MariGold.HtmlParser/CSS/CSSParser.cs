namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class CSSParser
    {
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

        private HtmlStyle CreateHtmlStyleFromRule(string styleName, string value, SelectorType type)
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

            if (string.IsNullOrEmpty(styleName) || string.IsNullOrEmpty(value))
            {
                return null;
            }

            return new HtmlStyle(styleName, value, important, type);
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
                    htmlStyles = ParseRules(styles, SelectorType.Global).ToList();
                }
            }

            //+1 to advance to next location
            return closeBraceIndex + 1;
        }

        private int FindComment(int position, string style, string commentToken)
        {
            char firstLetter = commentToken[0];
            int eof = style.Length;
            int commentPosition = -1;

            for (int i = position; eof > i; i++)
            {
                if (style[i] == firstLetter && i + 1 <= eof && style[i + 1] == commentToken[1])
                {
                    commentPosition = i;
                    break;
                }
            }

            return commentPosition;
        }

        private bool HasComment(int position, int bracePosition, int eof, string style, out int commentPosition)
        {
            commentPosition = FindComment(position, style, CSSTokenizer.openComment);

            if (commentPosition != -1 && commentPosition < bracePosition)
            {
                commentPosition = FindComment(commentPosition + 2, style, CSSTokenizer.closeComment);

                if (commentPosition == -1)
                {
                    //Open comment found and respective close comment tag not found. Thus skip to the very end.
                    commentPosition = eof;
                }
                else
                {
                    //+2 to skip the comment tag
                    commentPosition += 2;
                }

                return true;
            }

            return false;
        }

        internal IEnumerable<HtmlStyle> ParseRules(string styleText, SelectorType type)
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
                        HtmlStyle htmlStyle = CreateHtmlStyleFromRule(nodeSet[0], nodeSet[1], type);

                        if (htmlStyle != null)
                        {
                            yield return htmlStyle;
                        }
                    }
                }
            }
        }

        internal void ParseCSS(string style, List<CSSElement> elements, List<MediaQuery> mediaQuries)
        {
            MediaQuery mediaQuery = new MediaQuery();
            int eof = style.Length;
            int position = 0;

            while (position < eof)
            {
                string selectorText;

                int bracePosition = ParseSelector(position, style, out selectorText);
                int commentPosition;

                if (HasComment(position, bracePosition, eof, style, out commentPosition))
                {
                    position = commentPosition;
                    continue;
                }

                if (bracePosition > position && selectorText != string.Empty)
                {
                    List<HtmlStyle> htmlStyles;

                    if (!mediaQuery.Process(selectorText, style, mediaQuries, ref bracePosition))
                    {
                        //Returning close brace index
                        bracePosition = ParseHtmlStyles(bracePosition, style, out htmlStyles);

                        foreach (string selector in selectorText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            elements.Add(new CSSElement(selector.Trim(), htmlStyles));
                        }
                    }
                }

                if (bracePosition == -1)
                {
                    break;
                }

                position = bracePosition;
            }
        }
    }
}
