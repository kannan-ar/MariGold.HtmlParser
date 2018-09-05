namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class LastChildSelector : CSSelector, IAttachedSelector
    {
        private readonly Regex regex;

        private string selectorText;

        private LastChildSelector(ISelectorContext context, string selectorText, Specificity specificity)
        {
            this.context = context;
            regex = new Regex("^(:last-child)|(:last-of-type)");
            this.selectorText = selectorText;
            this.specificity = specificity;
        }

        internal LastChildSelector(ISelectorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.AddAttachedSelector(this);
            this.context = context;
            regex = new Regex("^(:last-child)|(:last-of-type)");
        }
        
        internal override bool Prepare(string selector)
        {
            Match match = regex.Match(selector);

            this.selectorText = string.Empty;
            this.specificity = new Specificity();

            if (match.Success)
            {
                this.selectorText = selector.Substring(match.Value.Length);
            }

            return match.Success;
        }

        internal override bool IsValidNode(HtmlNode node)
        {
            bool isValid = false;

            if (node != null && node.Parent != null)
            {
                HtmlNode lastChild = null;

                foreach (HtmlNode child in node.GetParent().GetChildren())
                {
                    if (string.Equals(node.Tag, child.Tag, StringComparison.OrdinalIgnoreCase))
                    {
                        lastChild = child;
                    }
                }

                isValid = lastChild != null && lastChild == node;
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
            return new LastChildSelector(context, selectorText, specificity.Clone());
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

            bool isValid = false;

            HtmlNode parent = node.GetParent();

            if (parent == null)
            {
                return false;
            }

            HtmlNode lastChild = null;

            foreach (HtmlNode child in parent.GetChildren())
            {
                if (child.Tag != HtmlTag.TEXT)
                {
                    lastChild = child;
                }
            }

            if (lastChild != null && lastChild == node)
            {
                isValid = true;
            }
            return isValid;
        }

        void IAttachedSelector.Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
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
}
