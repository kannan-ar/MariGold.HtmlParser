namespace MariGold.HtmlParser;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

internal sealed class NthLastChildSelector : CSSelector, IAttachedSelector
{
    private readonly Regex regex;

    private string selectorText;
    private int position;

    private NthLastChildSelector(ISelectorContext context, string selectorText, int position, Specificity specificity)
    {
        this.context = context;
        regex = new Regex("^(:nth-last-child\\(\\d+\\))|(:nth-last-of-type\\(\\d+\\))");
        this.selectorText = selectorText;
        this.position = position;
        this.specificity = specificity;
    }

    internal NthLastChildSelector(ISelectorContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        context.AddAttachedSelector(this);
        this.context = context;

        regex = new Regex("^(:nth-last-child\\(\\d+\\))|(:nth-last-of-type\\(\\d+\\))");
    }

    internal override bool Prepare(string selector)
    {
        Match match = regex.Match(selector);

        this.selectorText = string.Empty;
        this.position = -1;
        this.specificity = new ();

        if (match.Success)
        {
            this.selectorText = selector[match.Value.Length..];

            if (int.TryParse(new Regex("\\d+").Match(match.Value).Value, out int value))
            {
                this.position = value;
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

        if (this.position == -1)
        {
            return false;
        }

        HtmlNode parent = node.GetParent();

        if (parent == null)
        {
            return false;
        }

        int childrenCount = 0;
        int nodeIndex = 0;

        foreach (HtmlNode child in parent.GetChildren())
        {
            if (child.Tag == HtmlTag.TEXT)
            {
                continue;
            }

            ++childrenCount;

            if (child == node)
            {
                nodeIndex = childrenCount;
            }
        }

        if (childrenCount < this.position)
        {
            return false;
        }

        return (childrenCount - position) + 1 == nodeIndex;
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
        return new NthLastChildSelector(context, selectorText, position, specificity.Clone());
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
