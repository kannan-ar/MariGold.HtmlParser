namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal interface ISelectorContext
    {
        List<CSSBehavior> CSSBehaviors { get; }
        List<CSSelector> Selectors { get; }
		List<CSSelector> AttachedSelectors{ get; }
		
		void AddAttachedSelector(CSSelector selector);
		bool ParseSelector(string selectorText, out CSSelector selector);
		bool ParseBehavior(string selectorText, CSSelector currentSelector, HtmlNode node, List<HtmlStyle> htmlStyles);
		bool ParseSelectorOrBehavior(string selectorText, CSSelector currentSelector, HtmlNode node, List<HtmlStyle> htmlStyles);
    }
}
