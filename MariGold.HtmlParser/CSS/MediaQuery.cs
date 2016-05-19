namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class MediaQuery
    {
        private readonly Regex mediaRegex;
        private readonly StyleSheet styleSheet;

        internal MediaQuery(StyleSheet styleSheet)
        {
            mediaRegex = new Regex(@"^\s*@media");
            this.styleSheet = styleSheet;
        }

        private bool Process(string selectorText, string styleText, ref int position)
        {
            CSSParser cssParser = new CSSParser();

            Match match = mediaRegex.Match(selectorText);


            if (string.IsNullOrEmpty(styleText))
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

                    var htmlStyles = cssParser.ParseCSS(style);

                    foreach (var htmlStyle in htmlStyles)
                    {
                        string selecters = htmlStyle.Key;

                        foreach (string selector in selecters.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            styles.Add(new KeyValuePair<string, List<HtmlStyle>>(selector.Trim(), htmlStyle.Value));
                        }
                    }

                    selectorText = selectorText.Substring(match.Length + 1);

                    styleSheet.AddMediaQuery(selectorText, styles);
                }

                position = closeBraceIndex + 1;
            }

            return match.Success;
        }
    }
}
