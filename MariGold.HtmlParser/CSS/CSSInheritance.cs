namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal sealed class CSSInheritance
    {
        private string[][] tags;

        internal CSSInheritance()
        {
            Init();
        }

        private void Init()
        {
            tags = new string[][] {
                new string[]{CSSProperty.font},
				new string[]{CSSProperty.fontFamily},
				new string[]{CSSProperty.fontSize},
				new string[]{CSSProperty.color},
				new string[]{CSSProperty.fontWeight},
				new string[]{CSSProperty.textDecoration},
				new string[]{CSSProperty.fontStyle},
                new string[]{CSSProperty.lineHeight},
                new string[]{CSSProperty.fontVariant},
                new string[]{CSSProperty.textAlign},
                new string[]{CSSProperty.backgroundColor, CSSProperty.background}
			};
        }

        private bool CanInherit(string tag)
        {
            foreach (string[] array in tags)
            {
                if (HtmlStringComparer.Contains(array, tag))
                {
                    return true;
                }
            }

            return false;
        }

        private bool StyleContains(HtmlStyle parentStyle, HtmlStyle childStyle)
        {
            foreach (string[] array in tags)
            {
                if (HtmlStringComparer.Contains(array, parentStyle.Name) && HtmlStringComparer.Contains(array, childStyle.Name))
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasPropertyProcessed(HtmlStyle parentStyle, HtmlNode child)
        {
            CSSPropertyParser propertyParser = new CSSPropertyParser();
            return propertyParser.InheritStyle(parentStyle, child);
        }

        private void AppendStyles(HtmlNode node, HtmlNode parent)
        {
            foreach (HtmlStyle parentStyle in parent.HtmlStyles)
            {
                if(!CanInherit(parentStyle.Name))
                {
                    continue;
                }

                bool found = false;

                if (HasPropertyProcessed(parentStyle, node))
                {
                    continue;
                }

                foreach (HtmlStyle childStyle in node.HtmlStyles)
                {
                    if (StyleContains(parentStyle, childStyle))
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
        }
       
        private string FindParentStyle(HtmlNode node, string styleName)
        {
            string value = string.Empty;
            bool found = false;

            foreach (HtmlStyle style in node.HtmlStyles)
            {
                if (string.Compare(styleName, style.Name, StringComparison.InvariantCultureIgnoreCase) == 0)
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
                if (string.Compare(style.Value, "inherit", StringComparison.InvariantCultureIgnoreCase) == 0 && node.Parent != null)
                {
                    string value = FindParentStyle(parent, style.Name);

                    if (value != string.Empty)
                    {
                        style.ModifyStyle(value);
                    }
                }
            }
        }

        private List<HtmlStyle> CloneStyles(List<HtmlStyle> styles)
        {
            List<HtmlStyle> newStyles = new List<HtmlStyle>();

            foreach (HtmlStyle style in styles)
            {
                newStyles.Add(style.Clone());
            }

            return newStyles;
        }

        private void UpdateCurrentNodeInheritedStyles(HtmlNode node)
        {
            foreach(HtmlStyle style in node.HtmlStyles)
            {
                if(!CanInherit(style.Name))
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
