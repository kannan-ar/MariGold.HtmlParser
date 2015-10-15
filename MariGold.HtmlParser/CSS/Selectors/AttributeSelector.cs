namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class AttributeSelector : CSSelector
    {
        private readonly Regex isValid;
        private readonly Regex spliter;
        private readonly AttributeElements element;

        internal class AttributeElements
        {
            internal string SelectorText { get; set; }
            internal string AttributeName { get; set; }
            internal bool HasValue { get; set; }
            internal Action Filter { get; set; }
            internal string Value { get; set; }
        }

        internal AttributeSelector(ISelectorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.context = context;

            isValid = new Regex("^\\[([a-zA-Z]+[0-9]*)+([~|^$*]?=+[\"']?([a-zA-Z]+[0-9]*)+[\"']?)*\\]");
            spliter = new Regex(@"\w+|[~|^$*]|=");
        }

        internal AttributeSelector(AttributeElements element, ISelectorContext context)
        {
            this.element = element;
            this.context = context;
        }

        private List<string> SplitSelector(string selector)
        {
            List<string> elements = new List<string>();

            MatchCollection matches = spliter.Matches(selector);

            foreach (Match match in matches)
            {
                if (!string.IsNullOrEmpty(match.Value))
                {
                    elements.Add(match.Value);
                }
            }

            return elements;
        }

        private AttributeElements ParseSelector(string selector, Match match)
        {
            AttributeElements element = new AttributeElements();

            string trimmedSelector = selector.Substring(match.Value.Length);

            List<string> elements = SplitSelector(trimmedSelector);

            return element;
        }

        internal override bool IsValidNode(HtmlNode node)
        {
            throw new NotImplementedException();
        }

        internal override CSSelector Parse(string selector)
        {
            Match match = isValid.Match(selector);

            if (match.Success)
            {
                return new AttributeSelector(ParseSelector(selector, match), context);
            }
            else
            {
                return PassToSuccessor(selector);
            }
        }

        internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            throw new NotImplementedException();
        }
    }
}
