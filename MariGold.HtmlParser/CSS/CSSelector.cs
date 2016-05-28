namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal abstract class CSSelector
    {
        protected ISelectorContext context;
        protected int specificity;

        protected int CalculateSpecificity(SelectorWeight weight)
        {
            return this.specificity + (int)weight;
        }

        internal abstract bool Prepare(string selector);
        internal abstract bool IsValidNode(HtmlNode node);
        internal abstract void Parse(HtmlNode node, List<HtmlStyle> htmlStyles);
		internal abstract void ApplyStyle(HtmlNode node, List<HtmlStyle> htmlStyles);

        //This is method is public because it can serve as the IAttachedSelector.AddSpecificity
        public void AddSpecificity(int specificity)
        {
            this.specificity += specificity;
        }
    }
}
