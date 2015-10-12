namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal interface ISelectorContext
    {
        List<ICSSBehavior> CSSBehaviors { get; }
        CSSelector Selector { get; }

        ICSSBehavior FindBehavior(string selectorText);
    }
}
