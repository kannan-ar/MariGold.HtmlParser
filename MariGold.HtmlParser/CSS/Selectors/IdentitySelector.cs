namespace MariGold.HtmlParser
{
    using System;

    internal sealed class IdentitySelector : CSSelector
    {
        public override SelectorWeight Weight
        {
            get
            {
                return SelectorWeight.Identity;
            }
        }

        public override CSSelector Parse(string selector)
        {
            throw new NotImplementedException();
        }
    }
}
