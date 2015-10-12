namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal interface ICSSBehavior
    {
        bool IsValidBehavior(string selectorText);
        void Do(HtmlNode node, List<HtmlStyle> htmlStyles);
    }
}
