﻿namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal sealed class ApplyNextElement : ICSSBehavior
    {
        public bool IsValidBehavior(string selectorText)
        {
            throw new NotImplementedException();
        }

        public void Do(HtmlNode node, List<HtmlStyle> htmlStyles)
        {

        }
    }
}