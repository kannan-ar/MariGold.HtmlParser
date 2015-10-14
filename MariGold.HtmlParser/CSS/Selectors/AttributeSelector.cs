namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class AttributeSelector : CSSelector
    {
        private readonly Regex isValid;
        private readonly Regex spliter;
        private readonly string currentSelector;
        private readonly string selectorText;

        internal AttributeSelector(ISelectorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.context = context;

            isValid = new Regex("^\\[([a-zA-Z]+[0-9]*)+([~|\\^\\$\\*]?=+[\"']?([a-zA-Z]+[0-9]*)+[\"']?)*\\]$");
        }

        internal AttributeSelector(string currentSelector, string selectorText, ISelectorContext context)
        {
            this.currentSelector = currentSelector;
            this.selectorText = selectorText;
            this.context = context;

            spliter = new Regex("([a-zA-Z]+[0-9]*)+|[~|\\^\\$\\*]?|=|");
        }

        internal override bool IsValidNode(HtmlNode node)
        {
            throw new NotImplementedException();
        }

        internal override CSSelector Parse(string selector)
        {
            throw new NotImplementedException();
        }

        internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            throw new NotImplementedException();
        }
    }
}
