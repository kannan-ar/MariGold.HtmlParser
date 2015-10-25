namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal abstract class CSSelector
    {
        protected ISelectorContext context;

        internal abstract bool Prepare(string selector);
        internal abstract bool IsValidNode(HtmlNode node);
        internal abstract void Parse(HtmlNode node, List<HtmlStyle> htmlStyles);
		internal abstract void ApplyStyle(HtmlNode node, List<HtmlStyle> htmlStyles);
    }
}
