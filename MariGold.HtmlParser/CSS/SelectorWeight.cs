namespace MariGold.HtmlParser
{
    using System;

    internal enum SelectorWeight
    {
        None = 0,
        Global = 1,
        Element = 2,
        Attribute = 3, //Attribute and Class have same weight
        Class = 3,
        Identity = 4,
        Inline = 5,
    }
}
