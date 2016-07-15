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

        private void AppendStyles(HtmlNode node, List<HtmlStyle> styles)
        {
            foreach (HtmlStyle parentStyle in styles)
            {
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
                    node.HtmlStyles.Add(parentStyle.Clone());
                }
            }
        }

        private void CopyStyles(HtmlNode node, List<HtmlStyle> styles)
        {
            foreach (HtmlStyle style in node.HtmlStyles)
            {
                if (CanInherit(style.Name))
                {
                    bool found = false;

                    for (int i = 0; styles.Count > i; i++)
                    {
                        if (string.Compare(style.Name, styles[i].Name, StringComparison.InvariantCultureIgnoreCase) == 0)
                        {
                            found = true;
                            styles[i].ModifyStyle(style.Value);
                            break;
                        }
                    }

                    if (!found)
                    {
                        styles.Add(style.Clone());
                    }
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

        private void InheritFromParent(HtmlNode node)
        {
            foreach (HtmlStyle style in node.HtmlStyles)
            {
                if (string.Compare(style.Value, "inherit", StringComparison.InvariantCultureIgnoreCase) == 0 && node.Parent != null)
                {
                    string value = FindParentStyle(node.GetParent(), style.Name);

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

        private void ApplyToChildren(HtmlNode node, List<HtmlStyle> styles)
        {
            if (node == null)
            {
                return;
            }

            AppendStyles(node, styles);

            InheritFromParent(node);

            List<HtmlStyle> newStyles = CloneStyles(styles);

            CopyStyles(node, newStyles);

            foreach (HtmlNode child in node.GetChildren())
            {
                ApplyToChildren(child, newStyles);
            }

            if (node.Parent == null && node.Next != null)
            {
                ApplyToChildren(node.GetNext(), styles);
            }
        }

        internal void Apply(HtmlNode node)
        {
            List<HtmlStyle> styles = new List<HtmlStyle>();

            ApplyToChildren(node, styles);
        }
    }
}
