namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal abstract class CSSelector
    {
        protected ISelectorContext context;
        protected Specificity specificity;

        protected Specificity CalculateSpecificity(SelectorType type)
        {
            return this.specificity += type;
        }

        internal abstract bool Prepare(string selector);
        internal abstract bool IsValidNode(HtmlNode node);
        internal abstract void Parse(HtmlNode node, List<HtmlStyle> htmlStyles);
		internal abstract void ApplyStyle(HtmlNode node, List<HtmlStyle> htmlStyles);
        internal abstract CSSelector Clone();

        //This is method is public because it can serve as the IAttachedSelector.AddSpecificity
        public void AddSpecificity(Specificity specificity)
        {
            this.specificity = specificity;
        }
    }
}
