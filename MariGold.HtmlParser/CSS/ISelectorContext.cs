namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal interface ISelectorContext
    {
        List<CSSBehavior> CSSBehaviors { get; }
        List<CSSelector> Selectors { get; }
		List<IAttachedSelector> AttachedSelectors{ get; }
		
		void AddAttachedSelector(IAttachedSelector selector);
		bool ParseSelector(string selectorText, out CSSelector selector);
        bool ParseBehavior(string selectorText, Specificity specificity, HtmlNode node, List<HtmlStyle> htmlStyles);
        bool ParseSelectorOrBehavior(string selectorText, Specificity specificity, HtmlNode node, List<HtmlStyle> htmlStyles);
    }
}
