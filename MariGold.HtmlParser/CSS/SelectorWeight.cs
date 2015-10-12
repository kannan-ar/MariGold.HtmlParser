namespace MariGold.HtmlParser
{
    using System;

    internal enum SelectorWeight
    {
        None = 0,
        Global = 1,
        Element = 2,
        Class = 3,
        Identity = 4,
        Inline = 5,
    }
}
