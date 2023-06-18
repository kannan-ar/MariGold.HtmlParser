namespace MariGold.HtmlParser;

using System;
using System.Collections.Generic;

internal sealed class GlobalSelector : CSSelector
{
    private const string globalSelector = "*";
    private string selectorText;

    private GlobalSelector(ISelectorContext context, string selectorText, Specificity specificity)
    {
        this.context = context;
        this.selectorText = selectorText;
        this.specificity = specificity;
    }

    internal GlobalSelector(ISelectorContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    internal override bool Prepare(string selector)
    {
        this.specificity = new ();

        if (selector == globalSelector)
        {
            return true;
        }

        if (selector.StartsWith(globalSelector))
        {
            selectorText = selector.Remove(0, 1);
            return true;
        }

        return false;
    }

    internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
    {
        if (string.IsNullOrEmpty(selectorText))
        {
            ApplyStyle(node, htmlStyles);
        }
        else
        {
            context.ParseSelectorOrBehavior(this.selectorText, CalculateSpecificity(SelectorType.Global), node, htmlStyles);
        }
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

        return !HtmlStyle.IsNonStyleElement(node.Tag);
    }

    internal override void ApplyStyle(HtmlNode node, List<HtmlStyle> htmlStyles)
    {
        if (IsValidNode(node))
        {
            node.CopyHtmlStyles(htmlStyles, CalculateSpecificity(SelectorType.Global));
        }
    }

    internal override CSSelector Clone()
    {
        return new GlobalSelector(context, selectorText, specificity.Clone());
    }
}
