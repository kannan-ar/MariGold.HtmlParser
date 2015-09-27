namespace MariGold.HtmlParser
{
    using System;

    internal sealed class ElementSelector : CSSelector
    {
        internal override SelectorWeight Weight
        {
            get 
            {
                return SelectorWeight.Element;
            }
        }

        internal override CSSelector Parse(string selector)
        {
            throw new NotImplementedException();
        }

        internal override void Parse(HtmlNode node)
        {
            throw new NotImplementedException();
        }
    }
}
