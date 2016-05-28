namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class FirstChildSelector : CSSelector, IAttachedSelector
    {
        private string selectorText;

        private readonly Regex regex;

        internal FirstChildSelector(ISelectorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.AddAttachedSelector(this);
            this.context = context;

            regex = new Regex("^:first-child");
        }

        private void ApplyToFirstChild(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            if (node.HasChildren)
            {
                HtmlNode child = node.GetChild(0);

                //Loop to skip empty text children
                while (child != null && child.Tag == HtmlTag.TEXT && child.Html.Trim() == string.Empty)
                {
                    child = child.GetNext();
                }

                if (child != null)
                {
                    ApplyStyle(child, htmlStyles);
                }
            }
        }

        internal override bool Prepare(string selector)
        {
            Match match = regex.Match(selector);

            this.selectorText = string.Empty;
            this.specificity = 0;

            if (match.Success)
            {
                this.selectorText = selector.Substring(match.Value.Length);
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
                isValid = string.Compare(node.Tag, child.Tag, StringComparison.InvariantCultureIgnoreCase) == 0 && node == child;

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
                    context.ParseBehavior(this.selectorText, CalculateSpecificity(SelectorWeight.Child), node, htmlStyles);
                }
            }
        }

        internal override void ApplyStyle(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            node.CopyHtmlStyles(htmlStyles, CalculateSpecificity(SelectorWeight.Child));
        }

        bool IAttachedSelector.Prepare(string selector)
        {
            return Prepare(selector);
        }

        bool IAttachedSelector.IsValidNode(HtmlNode node)
        {
            /*if (node == null)
            {
                return false;
            }
			
            bool isValid = false;
			
            foreach (HtmlNode  child in node.Children)
            {
                if (child.Tag != HtmlTag.TEXT)
                {
                    isValid = true;
                    break;
                }
            }
			
            return isValid;*/
            return IsValidNode(node);
        }

        void IAttachedSelector.Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            /*if (string.IsNullOrEmpty(this.selectorText))
            {
                ApplyToFirstChild(node, htmlStyles);
            }
            else
            {
                context.ParseBehavior(this.selectorText, node, htmlStyles);
            }*/
            Parse(node, htmlStyles);
        }
    }
}
