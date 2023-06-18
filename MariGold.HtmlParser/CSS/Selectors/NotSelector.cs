namespace MariGold.HtmlParser;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

internal sealed class NotSelector : CSSelector, IAttachedSelector
{
    private readonly Regex regex;
    private readonly Regex elementName;

    private string selectorText;
    private string currentSelector;

    private NotSelector(ISelectorContext context, string selectorText, string currentSelector, Specificity specificity)
    {
        this.context = context;
        regex = new Regex("^:not\\([a-zA-Z]+[0-9]*\\)");
        elementName = new Regex("\\([a-zA-Z]+[0-9]*\\)");
        this.selectorText = selectorText;
        this.currentSelector = currentSelector;
        this.specificity = specificity;
    }

    internal NotSelector(ISelectorContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        context.AddAttachedSelector(this);
        this.context = context;

        regex = new Regex("^:not\\([a-zA-Z]+[0-9]*\\)");
        elementName = new Regex("\\([a-zA-Z]+[0-9]*\\)");
    }

    private void ApplyToChildren(HtmlNode node, List<HtmlStyle> htmlStyles)
    {
        if (IsValidNode(node))
        {
            ApplyStyle(node, htmlStyles);

            foreach (HtmlNode child in node.GetChildren())
            {
                ApplyToChildren(child, htmlStyles);
            }
        }
    }

    internal override bool Prepare(string selector)
    {
        Match match = regex.Match(selector);

        this.selectorText = string.Empty;
        this.currentSelector = string.Empty;
        this.specificity = new ();

        if (match.Success)
        {
            this.selectorText = selector[match.Value.Length..];

            Match elementMatch = elementName.Match(match.Value);

            if (elementMatch.Success)
            {
                this.currentSelector = elementMatch.Value.Replace("(", string.Empty).Replace(")", string.Empty);
            }
        }

        return match.Success;
    }

    internal override bool IsValidNode(HtmlNode node)
    {
        if (node == null)
        {
            return false;
        }

        if (node.Tag == HtmlTag.TEXT)
        {
            return false;
        }

        if (string.IsNullOrEmpty(currentSelector))
        {
            return false;
        }

        return !string.Equals(node.Tag, currentSelector, StringComparison.OrdinalIgnoreCase);
    }

    internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
    {
        if (IsValidNode(node))
        {
            if (string.IsNullOrEmpty(this.selectorText))
            {
                ApplyStyle(node, htmlStyles);
            }
            else
            {
                context.ParseBehavior(this.selectorText, CalculateSpecificity(SelectorType.PseudoClass), node, htmlStyles);
            }
        }
    }

    internal override void ApplyStyle(HtmlNode node, List<HtmlStyle> htmlStyles)
    {
        node.CopyHtmlStyles(htmlStyles, CalculateSpecificity(SelectorType.PseudoClass));
    }

    internal override CSSelector Clone()
    {
        return new NotSelector(context, selectorText, currentSelector, specificity.Clone());
    }

    bool IAttachedSelector.Prepare(string selector)
    {
        return Prepare(selector);
    }

    bool IAttachedSelector.IsValidNode(HtmlNode node)
    {
        if (node == null)
        {
            return false;
        }

        return node.GetChildren().Count > 0;
    }

    void IAttachedSelector.Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
    {
        if (string.IsNullOrEmpty(this.selectorText))
        {
            foreach (HtmlNode child in node.GetChildren())
            {
                ApplyToChildren(child, htmlStyles);
            }
        }
        else
        {
            context.ParseBehavior(this.selectorText, CalculateSpecificity(SelectorType.PseudoClass), node, htmlStyles);
        }
    }
}
