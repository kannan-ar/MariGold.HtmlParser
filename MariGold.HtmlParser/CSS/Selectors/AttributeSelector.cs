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
            internal Func<string, string, bool> Filter { get; set; }
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

        #region Filters

        private bool EqualTo(string selectorValue, string attributeValue)
        {
            return string.Compare(selectorValue, attributeValue, true) == 0;
        }

        private bool StartsWith(string selectorValue, string attributeValue)
        {
            if (string.IsNullOrEmpty(attributeValue))
            {
                return false;
            }

            return attributeValue.StartsWith(selectorValue);
        }

        private bool EndsWith(string selectorValue, string attributeValue)
        {
            if (string.IsNullOrEmpty(attributeValue))
            {
                return false;
            }

            return attributeValue.EndsWith(selectorValue);
        }

        private bool Contains(string selectorValue, string attributeValue)
        {
            if (string.IsNullOrEmpty(attributeValue))
            {
                return false;
            }

            return attributeValue.Contains(selectorValue);
        }

        #endregion Filters

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

        private Func<string, string, bool> ChooseFilter(string filter)
        {
            Func<string, string, bool> act = EqualTo;

            switch (filter)
            {
                case "|":
                case "^":
                    act = StartsWith;
                    break;

                case "$":
                    act = EndsWith;
                    break;

                case "*":
                    act = Contains;
                    break;
            }
            return act;
        }

        private void FillAttributeElements(List<string> elements, AttributeElements element)
        {
            if (elements.Count > 0)
            {
                element.AttributeName = elements[0];
            }

            if (elements.Count > 1)
            {
                element.HasValue = true;
                element.Filter = ChooseFilter(elements[1]);
            }

            if (elements.Count > 2)
            {
                element.Value = elements[elements.Count - 1];
            }
        }

        private AttributeElements ParseSelector(string selector, Match match)
        {
            AttributeElements element = new AttributeElements();

            element.SelectorText = selector.Substring(match.Value.Length);

            FillAttributeElements(SplitSelector(match.Value), element);

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
