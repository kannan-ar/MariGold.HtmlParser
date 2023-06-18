namespace MariGold.HtmlParser;

using System.Collections.Generic;

internal sealed class CSSPropertyParser
{
    private string[][] tags;

    private List<CSSProperty> properties;

    private void Init()
    {
        properties = new List<CSSProperty>(){
            new FontProperty(),
            new BackgroundProperty(),
            new FontSizeProperty()
        };

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

    internal CSSPropertyParser()
    {
        Init();
    }

    internal bool InheritStyle(HtmlStyle parentStyle, HtmlNode child)
    {
        foreach (CSSProperty property in properties)
        {
            if (property.AppendStyle(parentStyle, child))
            {
                return true;
            }
        }

        return false;
    }

    internal void ParseStyle(HtmlNode node)
    {
        foreach (CSSProperty property in properties)
        {
            property.ParseStyle(node);
        }

        foreach (HtmlNode child in node.GetChildren())
        {
            ParseStyle(child);
        }

        if (node.Parent == null && node.Next != null)
        {
            ParseStyle(node.GetNext());
        }
    }

    internal bool CanInherit(string tag)
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

    internal bool StyleContains(HtmlStyle parentStyle, HtmlStyle childStyle)
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
}
