namespace MariGold.HtmlParser
{
    using System.Collections.Generic;

    public interface IHtmlNode
    {
        string Tag { get; }
        string InnerHtml { get; }
        string Html { get; }
        IHtmlNode Parent { get; }
        IEnumerable<IHtmlNode> Children { get; }
        IHtmlNode Previous { get; }
        IHtmlNode Next { get; }
        bool HasChildren { get; }
        bool SelfClosing { get; }
        bool IsText { get; }
        Dictionary<string, string> Attributes { get; }
        Dictionary<string, string> Styles { get; }
        Dictionary<string, string> InheritedStyles { get; }
        IHtmlNode Clone();
    }
}
