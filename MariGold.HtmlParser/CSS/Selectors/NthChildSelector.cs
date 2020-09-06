namespace MariGold.HtmlParser
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class NthChildSelector : CSSelector, IAttachedSelector
    {
        private readonly Regex regex;

        private string selectorText;
        private int position;

        private NthChildSelector(ISelectorContext context, string selectorText, int position, Specificity specificity)
        {
            this.context = context;
            regex = new Regex("^:nth-child\\(\\d+\\)");
            this.selectorText = selectorText;
            this.position = position;
            this.specificity = specificity;
        }

        internal NthChildSelector(ISelectorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.AddAttachedSelector(this);
            this.context = context;

            regex = new Regex("^:nth-child\\(\\d+\\)");
        }

        private HtmlNode GetNodeAtPosition(int position, HtmlNode parent)
        {
            HtmlNode node = null;

            foreach (HtmlNode child in parent.Children)
            {
                if (child.Tag != HtmlTag.TEXT)
                {
                    position--;
                }

                if (position == 0)
                {
                    node = child;
                    break;
                }
            }

            return node;
        }

        internal override bool Prepare(string selector)
        {
            Match match = regex.Match(selector);

            this.selectorText = string.Empty;
            this.position = -1;
            this.specificity = new Specificity();

            if (match.Success)
            {
                this.selectorText = selector.Substring(match.Value.Length);

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

            if (node.Parent == null)
            {
                return false;
            }

            var parent = node.GetParent();

            if (parent.GetChildren().Count() < this.position)
            {
                return false;
            }

            HtmlNode pNode = GetNodeAtPosition(position, parent);

            if (pNode == null)
            {
                return false;
            }

            return pNode == node;
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
            return new NthChildSelector(context, selectorText, position, specificity.Clone());
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
}
