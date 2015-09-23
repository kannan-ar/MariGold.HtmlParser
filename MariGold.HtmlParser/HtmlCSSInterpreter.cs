namespace MariGold.HtmlParser
{
    using System;
    using MariGold.HtmlParser.CSS;

    internal sealed class HtmlCSSInterpreter
    {
        private CSSParser cssParser;

        internal HtmlCSSInterpreter()
        {
            cssParser = new CSSParser();
        }

        internal void Parse(StyleSheet styleSheet, HtmlNode htmlNode)
        {
            string style;

            if (htmlNode.Attributes.TryGetValue("style", out style))
            {
                htmlNode.AddStyles(cssParser.ParseRules(style, SelectorWeight.Inline));
            }

            styleSheet.Parse(htmlNode);

            foreach(HtmlNode node in htmlNode.Children)
            {
                Parse(styleSheet, node);
            }
        }
    }
}
