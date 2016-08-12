namespace MariGold.HtmlParser
{
    using System;

    internal sealed class BackgroundProperty : CSSProperty
    {
        internal override bool AppendStyle(HtmlStyle parentStyle, HtmlNode child)
        {
            if (parentStyle == null || child == null)
            {
                return false;
            }

            if (parentStyle.Name.CompareInvariantCultureIgnoreCase(backgroundColor) ||
                parentStyle.Name.CompareInvariantCultureIgnoreCase(background))
            {
                HtmlStyle style;

                if (!child.HasStyle(backgroundColor) && !child.HasStyle(background))
                {
                    //child.HtmlStyles.Add(new HtmlStyle(parentStyle.Name, parentStyle.Value, false));
                    child.UpdateInheritedStyles(parentStyle);
                }
                else if ((child.TryGetStyle(background, out style) || child.TryGetStyle(backgroundColor, out style)) &&
                    style.Value.CompareInvariantCultureIgnoreCase(transparent))
                {
                    string styleValue = parentStyle.Value;
                    
                    if (parentStyle.Name.CompareInvariantCultureIgnoreCase(background) &&
                        style.Name.CompareInvariantCultureIgnoreCase(backgroundColor))
                    {
                        int index = styleValue.IndexOf(' ');

                        if (index != -1)
                        {
                            styleValue = styleValue.Remove(index);
                        }
                    }
                    
                    style.ModifyStyle(styleValue);
                }

                return true;
            }

            return false;
        }

        internal override void ParseStyle(HtmlNode node)
        {
        }
    }
}
