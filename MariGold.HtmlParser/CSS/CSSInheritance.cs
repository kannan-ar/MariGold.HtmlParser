namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal sealed class CSSInheritance
    {
        private void AppendStyles(HtmlNode node, HtmlNode parent)
        {
            CSSPropertyParser propertyParser = new CSSPropertyParser();

            foreach (HtmlStyle parentStyle in parent.HtmlStyles)
            {
                if (!propertyParser.CanInherit(parentStyle.Name))
                {
                    continue;
                }

                bool found = false;

                if (propertyParser.InheritStyle(parentStyle, node))
                {
                    continue;
                }

                foreach (HtmlStyle childStyle in node.HtmlStyles)
                {
                    if (propertyParser.StyleContains(parentStyle, childStyle))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    node.UpdateInheritedStyles(parentStyle);
                }
            }

            foreach (HtmlStyle parentStyle in parent.InheritedHtmlStyles)
            {
                if (!propertyParser.CanInherit(parentStyle.Name))
                {
                    continue;
                }

                if(parent.HasStyle(parentStyle.Name))
                {
                    continue;
                }

                propertyParser.InheritStyle(parentStyle, node);
            }
        }
       
        private string FindParentStyle(HtmlNode node, string styleName)
        {
            string value = string.Empty;
            bool found = false;

            foreach (HtmlStyle style in node.HtmlStyles)
            {
                if (string.Equals(styleName, style.Name, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    value = style.Value;
                }
            }

            if (!found && node.Parent != null)
            {
                value = FindParentStyle(node.GetParent(), styleName);
            }

            return value;
        }

        private void InheritFromParent(HtmlNode node, HtmlNode parent)
        {
            foreach (HtmlStyle style in node.HtmlStyles)
            {
                if (string.Equals(style.Value, "inherit", StringComparison.OrdinalIgnoreCase) && node.Parent != null)
                {
                    string value = FindParentStyle(parent, style.Name);

                    if (value != string.Empty)
                    {
                        style.ModifyStyle(value);
                    }
                }
            }
        }

        private void UpdateCurrentNodeInheritedStyles(HtmlNode node)
        {
            CSSPropertyParser propertyParser = new CSSPropertyParser();

            foreach(HtmlStyle style in node.HtmlStyles)
            {
                if (!propertyParser.CanInherit(style.Name))
                {
                    continue;
                }

                node.UpdateInheritedStyles(style);
            }
        }

        private void ApplyToChildren(HtmlNode node, HtmlNode parent)
        {
            if (node == null)
            {
                return;
            }

            if (parent != null)
            {
                node.ImportInheritedStyles(parent.InheritedHtmlStyles);

                AppendStyles(node, parent);

                UpdateCurrentNodeInheritedStyles(node);

                InheritFromParent(node, parent);
            }

            foreach (HtmlNode child in node.GetChildren())
            {
                ApplyToChildren(child, node);
            }

            if (node.Parent == null && node.Next != null)
            {
                ApplyToChildren(node.GetNext(), parent);
            }
        }

        internal void Apply(HtmlNode node)
        {
            ApplyToChildren(node, null);
        }
    }
}
