namespace MariGold.HtmlParser;

using System;

internal sealed class Specificity
{
    private Int16 inline;
    private Int16 id;
    private Int16 classes;
    private Int16 elements;

    internal Specificity()
    {
        inline = 0;
        id = 0;
        classes = 0;
        elements = 0;
    }

    internal Specificity(Int16 inline, Int16 id, Int16 classes, Int16 elements)
    {
        this.inline = inline;
        this.id = id;
        this.classes = classes;
        this.elements = elements;
    }

    internal void SetSpecificity(SelectorType type)
    {
        switch (type)
        {
            case SelectorType.Element:
                elements += 1;
                break;

            case SelectorType.Class:
            case SelectorType.Attribute:
            case SelectorType.PseudoClass:
                classes += 1;
                break;

            case SelectorType.Identity:
                id += 1;
                break;

            case SelectorType.Inline:
                inline += 1;
                break;
        }
    }

    internal Specificity Clone()
    {
        return new Specificity(inline, id, classes, elements);
    }

    public static implicit operator Specificity(SelectorType type)
    {
        Specificity specificity = new();
        specificity.SetSpecificity(type);

        return specificity;
    }

    public static Specificity operator +(Specificity specificity, SelectorType type)
    {
        Specificity newSpecificity = new(specificity.inline, specificity.id, specificity.classes, specificity.elements);
        newSpecificity.SetSpecificity(type);

        return newSpecificity;
    }

    public static bool operator >=(Specificity first, Specificity second)
    {
        if (first.inline == second.inline && first.id == second.id &&
            first.classes == second.classes && first.elements == second.elements)
        {
            return true;
        }

        if (first.inline > second.inline)
        {
            return true;
        }
        else if (first.inline < second.inline)
        {
            return false;
        }

        if (first.id > second.id)
        {
            return true;
        }
        else if (first.id < second.id)
        {
            return false;
        }

        if (first.classes > second.classes)
        {
            return true;
        }
        else if (first.classes < second.classes)
        {
            return false;
        }

        if (first.elements > second.elements)
        {
            return true;
        }
        else if (first.elements < second.elements)
        {
            return false;
        }

        return false;
    }

    public static bool operator <=(Specificity first, Specificity second)
    {
        throw new NotImplementedException();
    }
}
