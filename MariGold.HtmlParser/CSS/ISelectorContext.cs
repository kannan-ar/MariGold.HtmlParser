namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal interface ISelectorContext
    {
        List<CSSBehavior> CSSBehaviors { get; }
        CSSelector Selector { get; }

        CSSBehavior FindBehavior(string selectorText);
    }
}
