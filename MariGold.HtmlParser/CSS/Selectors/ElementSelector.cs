namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Linq;

    internal sealed class ElementSelector : CSSelector
    {
        private Regex regex;
        private Regex spliter;
        private Regex isElementOnly;
        private Regex isElementWithProp;

        private string[] selectorSplit;

        internal override SelectorWeight Weight
        {
            get 
            {
                return SelectorWeight.Element;
            }
        }

        internal ElementSelector()
        {
            regex = new Regex(@"^([a-zA-Z]+[0-9]*([:\.#][a-zA-Z]+[0-9]*)*)+(\s*[>~\+]?\s*([a-zA-Z]+[0-9]*([:\.#][a-zA-Z]+[0-9]*)*)+)*$");
            spliter = new Regex(@"([a-zA-Z]+[0-9]*([:\.#][a-zA-Z]+[0-9]*)*)|[>~\+]|\s");

            isElementOnly = new Regex(@"^([a-zA-Z]+[0-9]*)$");
            isElementWithProp = new Regex(@"^([a-zA-Z]+[0-9]*([:\.#][a-zA-Z]+[0-9]*)*)$");
        }

        internal ElementSelector(string[] selectorSplit)
            : this()
        {
            this.selectorSplit = selectorSplit;
        }

        private IEnumerable<string> SplitSelector(string selector)
        {
            foreach(Match m in spliter.Matches(selector))
            {
                yield return m.Value;
            }
        }

        internal override CSSelector Parse(string selector)
        {
            selector = selector.Trim();

            if(regex.IsMatch(selector))
            {
                return new ElementSelector(SplitSelector(selector).ToArray());
            }
            else
            {
                return PassToSuccessor(selector);
            }
        }

        internal override void Parse(HtmlNode node)
        {
            if (selectorSplit != null && selectorSplit.Length > 0)
            {
                string selector = selectorSplit[0];

                if (isElementOnly.IsMatch(selector) && 
                    string.Compare(selector, node.Tag, true) == 0)
                {
                    //node.PotentialStyles
                }
            }
        }
    }
}
