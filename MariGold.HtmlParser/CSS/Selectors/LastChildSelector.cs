namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class LastChildSelector : CSSelector, IAttachedSelector
    {
        private readonly Regex regex;

        private string selectorText;

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
        /*
        private void ApplyToLastChild(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            if (node.HasChildren)
            {
                //Finds the last child
                HtmlNode child = node.GetChild(node.GetChildren().Count - 1);

                //Loop to skip empty text children
                while (child != null && child.Tag == HtmlTag.TEXT && child.Html.Trim() == string.Empty)
                {
                    child = child.GetPrevious();
                }

                if (child != null)
                {
                    ApplyStyle(child, htmlStyles);
                }
            }
        }
        */
        internal override bool Prepare(string selector)
        {
            Match match = regex.Match(selector);

            this.selectorText = string.Empty;

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
                    if (string.Compare(node.Tag, child.Tag, StringComparison.InvariantCultureIgnoreCase) == 0)
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
                    context.ParseBehavior(this.selectorText, node, htmlStyles);
                }
            }
        }

        internal override void ApplyStyle(HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            node.CopyHtmlStyles(htmlStyles, SelectorWeight.Child);
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
                context.ParseBehavior(this.selectorText, node, htmlStyles);
            }
        }
    }
}
