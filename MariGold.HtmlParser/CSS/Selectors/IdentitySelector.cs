namespace MariGold.HtmlParser
{
    using System;
    using System.Text.RegularExpressions;

    internal sealed class IdentitySelector : CSSelector
    {
        private Regex regex;
        private string selector;
        private const string key = "id";

        internal override SelectorWeight Weight
        {
            get
            {
                return SelectorWeight.Identity;
            }
        }

        internal IdentitySelector()
        {
            regex = new Regex("^#[-_]*([a-zA-Z]+[0-9_-]*)+$");
        }

        internal IdentitySelector(string selector)
            : this()
        {
            this.selector = selector;
        }

        internal override CSSelector Parse(string selector)
        {
            selector = selector.Trim();

            if (regex.IsMatch(selector))
            {
                return new IdentitySelector(selector.Replace("#", string.Empty));
            }
            else
            {
                return PassToSuccessor(selector);
            }
        }

        internal override void Parse(HtmlNode node)
        {
            string id;

            if (node.Attributes.TryGetValue(key, out id))
            {
                if (string.Compare(selector, id, true) == 0)
                {
                    node.CopyHtmlStyles(styles);
                }
            }
        }
    }
}
