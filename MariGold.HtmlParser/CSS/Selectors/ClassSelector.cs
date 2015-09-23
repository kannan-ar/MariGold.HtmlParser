namespace MariGold.HtmlParser
{
    using System;
    using System.Text.RegularExpressions;

    internal sealed class ClassSelector : CSSelector
    {
        private Regex regex;
        private string selector;
        private const string key = "class";

        internal override SelectorWeight Weight
        {
            get
            {
                return SelectorWeight.Class;
            }
        }

        internal ClassSelector()
        {
            regex = new Regex(@"\.[A-Za-z0-9]");
        }

        internal ClassSelector(string selector)
            : this()
        {
            this.selector = selector;
        }

        internal override CSSelector Parse(string selector)
        {
            selector = selector.Trim();

            if(regex.IsMatch(selector))
            {
                return new ClassSelector(selector.Replace(".", string.Empty));
            }
            else
            {
                return PassToSuccessor(selector);
            }
        }

        internal override void Parse(HtmlNode node)
        {
            string className;

            if (node.Attributes.TryGetValue(key, out className))
            {
                if (string.Compare(selector, className, true) == 0)
                {
                    node.CopyHtmlStyles(styles);
                }
            }
        }
    }
}
