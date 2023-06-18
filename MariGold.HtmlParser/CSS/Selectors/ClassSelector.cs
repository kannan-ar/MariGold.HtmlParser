namespace MariGold.HtmlParser;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

internal sealed class ClassSelector : CSSelector, IAttachedSelector
{
    private const string key = "class";

    private readonly Regex regex;

    private string currentSelector;
    private string selectorText;

    private ClassSelector(ISelectorContext context, string currentSelector, string selectorText, Specificity specificity)
    {
        this.context = context;
        regex = new Regex(@"^\.[-_]*([a-zA-Z]+[0-9_-]*)+");
        this.currentSelector = currentSelector;
        this.selectorText = selectorText;
        this.specificity = specificity;
    }

    internal ClassSelector(ISelectorContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        context.AddAttachedSelector(this);

        this.context = context;
        regex = new Regex(@"^\.[-_]*([a-zA-Z]+[0-9_-]*)+");
    }

    internal override bool Prepare(string selector)
    {
        Match match = regex.Match(selector);

        this.currentSelector = string.Empty;
        this.selectorText = string.Empty;
        this.specificity = new Specificity();

        if (match.Success)
        {
            this.currentSelector = match.Value.Replace(".", string.Empty);
            this.selectorText = selector[match.Value.Length..];
        }

        return match.Success;
    }

    internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
    {
        if (string.IsNullOrEmpty(selectorText) && IsValidNode(node))
        {
            ApplyStyle(node, htmlStyles);
        }
        else
        {
            context.ParseSelectorOrBehavior(this.selectorText, CalculateSpecificity(SelectorType.Class), node, htmlStyles);
        }
    }

    internal override bool IsValidNode(HtmlNode node)
    {
        if (node == null)
        {
            return false;
        }

        if (string.IsNullOrEmpty(currentSelector))
        {
            return false;
        }

        bool isValid = false;

        if (node.Attributes.TryGetValue(key, out string className))
        {
            string[] names = className.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string name in names)
            {
                if (string.Equals(currentSelector, name, StringComparison.OrdinalIgnoreCase))
                {
                    isValid = true;
                }
            }
        }

        return isValid;
    }

    internal override void ApplyStyle(HtmlNode node, List<HtmlStyle> htmlStyles)
    {
        node.CopyHtmlStyles(htmlStyles, CalculateSpecificity(SelectorType.Class));
    }

    internal override CSSelector Clone()
    {
        return new ClassSelector(context, currentSelector, selectorText, this.specificity.Clone());
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
