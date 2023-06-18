namespace MariGold.HtmlParser;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

internal sealed class FirstChildSelector : CSSelector, IAttachedSelector
{
    private string selectorText;

    private readonly Regex regex;

    private FirstChildSelector(ISelectorContext context, string selectorText, Specificity specificity)
    {
        this.context = context;
        regex = new Regex("^:first-child");
        this.selectorText = selectorText;
        this.specificity = specificity;
    }

    internal FirstChildSelector(ISelectorContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        context.AddAttachedSelector(this);
        this.context = context;

        regex = new Regex("^:first-child");
    }

    internal override bool Prepare(string selector)
    {
        Match match = regex.Match(selector);

        this.selectorText = string.Empty;
        this.specificity = new();

        if (match.Success)
        {
            this.selectorText = selector[match.Value.Length..];
        }

        return match.Success;
    }

    internal override bool IsValidNode(HtmlNode node)
    {
        if (node == null)
        {
            return false;
        }

        if (node.Parent == null)
        {
            return false;
        }

        bool isValid = false;

        foreach (HtmlNode child in node.GetParent().GetChildren())
        {
            if (child.Tag == HtmlTag.TEXT && child.Html.Trim() == string.Empty)
            {
                continue;
            }

            //Find first child tag which matches the node's tag. The break statement will discard the loop after finding the first matching node.
            //If the node is the first child, it will apply the styles.
            isValid = string.Equals(node.Tag, child.Tag, StringComparison.OrdinalIgnoreCase) && node == child;

            //The loop only needs to check the first child element except the empty text element. So we can skip here.
            break;

        }

        return isValid;
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
        return new FirstChildSelector(context, selectorText, specificity.Clone());
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
