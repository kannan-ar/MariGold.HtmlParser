namespace MariGold.HtmlParser;

using System.Collections.Generic;

internal interface IAttachedSelector
{
    bool Prepare(string selector);
    bool IsValidNode(HtmlNode node);
    void Parse(HtmlNode node, List<HtmlStyle> htmlStyles);
    void AddSpecificity(Specificity specificity);
}
