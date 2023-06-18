namespace MariGold.HtmlParser;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

internal sealed class OnlyChildSelector : CSSelector, IAttachedSelector
{
    private readonly Regex regex;

    private string selectorText;

    private OnlyChildSelector(ISelectorContext context, string selectorText, Specificity specificity)
    {
        this.context = context;
        regex = new Regex("^:only-child");
        this.selectorText = selectorText;
        this.specificity = specificity;
    }

    internal OnlyChildSelector(ISelectorContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        context.AddAttachedSelector(this);
        this.context = context;

        regex = new Regex("^:only-child");
    }

    internal override bool Prepare(string selector)
    {
        Match match = regex.Match(selector);

        this.selectorText = string.Empty;
        this.specificity = new Specificity();

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

        if (node.Tag == HtmlTag.TEXT)
        {
            return false;
        }

        if (node.Parent == null)
        {
            return false;
        }

        bool isValid = true;

        foreach (HtmlNode child in node.GetParent().GetChildren())
        {
            //There is a non text node other than current node. So the selector is not valid
            if (child != node && child.Tag != HtmlTag.TEXT)
            {
                isValid = false;
                break;
            }
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
        return new OnlyChildSelector(context, selectorText, specificity.Clone());
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
