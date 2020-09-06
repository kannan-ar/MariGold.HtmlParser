namespace MariGold.HtmlParser
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class MediaQuery
    {
        private readonly Regex mediaRegex;
        private readonly string selector;
        private readonly List<CSSElement> elements;

        internal MediaQuery()
        {
            mediaRegex = new Regex(@"^\s*@media");
            elements = new List<CSSElement>();
        }

        internal MediaQuery(string selector, List<CSSElement> elements)
            : base()
        {
            this.selector = selector;
            this.elements = elements;
        }

        internal bool Process(string selectorText, string styleText, List<MediaQuery> mediaQuries, ref int position)
        {
            CSSParser cssParser = new CSSParser();
            List<CSSElement> elements = new List<CSSElement>();
            Match match = mediaRegex.Match(selectorText);

            if (string.IsNullOrEmpty(styleText))
            {
                return false;
            }

            if(!match.Success)
            {
                return false;
            }

            string style = CSSTokenizer.FindOpenCloseBraceArea(styleText, position + 1, out int closeBraceIndex);

            if (closeBraceIndex > position)
            {
                if (!string.IsNullOrEmpty(style))
                {
                    cssParser.ParseCSS(style, elements, mediaQuries);
                   
                    selectorText = selectorText.Substring(match.Length + 1);
                    mediaQuries.Add(new MediaQuery(selectorText, elements));
                }

                position = closeBraceIndex + 1;
            }

            return match.Success;
        }
    }
}
