namespace MariGold.HtmlParser
{
    using System;

    internal sealed class ClassSelector : CSSelector
    {
        public override SelectorWeight Weight
        {
            get
            {
                return SelectorWeight.Class;
            }
        }

        public override CSSelector Parse(string selector)
        {
            throw new NotImplementedException();
        }
    }
}
