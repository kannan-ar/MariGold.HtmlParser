namespace MariGold.HtmlParser
{
    using System;

    internal enum SelectorWeight
    {
        Inline = 0,
        Identity = 1,
        Class = 2,
        Element = 3,
        Global = 4
    }
}
