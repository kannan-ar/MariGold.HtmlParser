namespace MariGold.HtmlParser;

using System;

internal sealed class HtmlStyle
{
    public string Name { get; private set; }

    public string Value { get; private set; }

    public bool Important { get; internal set; }

    internal Specificity Specificity { get; set; }

    internal HtmlStyle(string name, string value, bool important)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value));
        }

        this.Name = name;
        this.Value = value;
        this.Important = important;
    }

    internal HtmlStyle(string name, string value, bool important, Specificity specificity)
        : this(name, value, important)
    {
        this.Specificity = specificity;
    }

    internal void OverWrite(HtmlStyle htmlStyle)
    {
        CSSPropertyParser propertyParser = new();

        if (propertyParser.StyleContains(this, htmlStyle))
        {
            if (!Important && htmlStyle.Important)
            {
                Name = htmlStyle.Name;
                Value = htmlStyle.Value;
                Important = htmlStyle.Important;
                Specificity = htmlStyle.Specificity;
            }
            else if (htmlStyle.Specificity >= Specificity && (!Important || (Important && htmlStyle.Important)))
            {
                Name = htmlStyle.Name;
                Value = htmlStyle.Value;
            }
        }
    }

    internal void ModifyStyle(string value)
    {
        this.Value = value;
    }

    internal HtmlStyle Clone()
    {
        return new HtmlStyle(Name, Value, Important, Specificity);
    }

    static internal bool IsNonStyleElement(string tag)
    {
        tag = tag.ToLower();

        return tag == "style" || tag == "script" || tag == "html";
    }
}
