namespace MariGold.HtmlParser;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

internal sealed class AttributeSelector : CSSelector, IAttachedSelector
{
    private readonly Regex isValid;
    private readonly Regex spliter;

    private AttributeElements element;

    internal class AttributeElements
    {
        internal string SelectorText { get; set; }
        internal string AttributeName { get; set; }
        internal bool HasValue { get; set; }
        internal Func<string, string, bool> Filter { get; set; }
        internal string Value { get; set; }
    }

    private AttributeSelector(ISelectorContext context, AttributeElements element, Specificity specificity)
    {
        this.context = context;
        this.element = element;
        this.specificity = specificity;
        isValid = new Regex("^\\[([a-zA-Z]+[0-9]*)+([~|^$*]?=+[\"']?([a-zA-Z]+[0-9]*)+[\"']?)*\\]");
        spliter = new Regex(@"\w+|[~|^$*]|=");
    }

    internal AttributeSelector(ISelectorContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        //Attribute Selector can be next a selector or an independent selector.
        context.AddAttachedSelector(this);

        this.context = context;

        isValid = new Regex("^\\[([a-zA-Z]+[0-9]*)+([~|^$*]?=+[\"']?([a-zA-Z]+[0-9]*)+[\"']?)*\\]");
        spliter = new Regex(@"\w+|[~|^$*]|=");
    }

    #region Filters

    private bool EqualTo(string selectorValue, string attributeValue)
    {
        return string.Equals(selectorValue, attributeValue, StringComparison.OrdinalIgnoreCase);
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

    #region Private Functions

    private List<string> SplitSelector(string selector)
    {
        List<string> elements = new();

        MatchCollection matches = spliter.Matches(selector);

        foreach (Match match in matches.Cast<Match>())
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
            element.Value = elements[^1];
        }
    }

    private AttributeElements PrepareElement(string selector, Match match)
    {
        AttributeElements elm = new()
        {
            SelectorText = selector[match.Value.Length..]
        };

        FillAttributeElements(SplitSelector(match.Value), elm);

        return elm;
    }

    #endregion Private Functions

    internal bool IsValidNode(string selector)
    {
        return isValid.IsMatch(selector);
    }

    internal override bool IsValidNode(HtmlNode node)
    {
        if (node == null)
        {
            return false;
        }

        if (element == null)
        {
            return false;
        }

        bool valid = false;

        if (node.Attributes.ContainsKey(element.AttributeName))
        {
            //Here we assume the element has the attribute and thus it is valid. But it will further test the attribute value also.
            valid = true;

            if (element.HasValue)
            {
                valid = node.Attributes.TryGetValue(element.AttributeName, out string value);

                if (valid)
                {
                    valid = element.Filter(value, element.Value);
                }
            }
        }

        return valid;
    }

    internal override bool Prepare(string selector)
    {
        Match match = isValid.Match(selector);
        element = null;
        this.specificity = new Specificity();

        if (match.Success)
        {
            element = PrepareElement(selector, match);
        }

        return match.Success;
    }

    internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
    {
        if (string.IsNullOrEmpty(element.SelectorText))
        {
            ApplyStyle(node, htmlStyles);
        }
        else
        {
            context.ParseSelectorOrBehavior(element.SelectorText, CalculateSpecificity(SelectorType.Attribute), node, htmlStyles);
        }
    }

    internal override void ApplyStyle(HtmlNode node, List<HtmlStyle> htmlStyles)
    {
        if (IsValidNode(node))
        {
            node.CopyHtmlStyles(htmlStyles, CalculateSpecificity(SelectorType.Attribute));
        }
    }

    internal override CSSelector Clone()
    {
        return new AttributeSelector(context, element, specificity.Clone());
    }

    bool IAttachedSelector.Prepare(string selector)
    {
        return Prepare(selector);
    }

    bool IAttachedSelector.IsValidNode(HtmlNode node)
    {
        return IsValidNode(node);
    }

    void IAttachedSelector.Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
    {
        Parse(node, htmlStyles);
    }
}
