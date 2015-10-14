namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal abstract class CSSelector
    {
        protected CSSelector successor;
        protected ISelectorContext context;

        internal abstract CSSelector Parse(string selector);
        internal abstract void Parse(HtmlNode node, List<HtmlStyle> htmlStyles);
        internal abstract bool IsValidNode(HtmlNode node);

        protected CSSelector PassToSuccessor(string selector)
        {
            if (successor == null)
            {
                return null;
            }

            return successor.Parse(selector);
        }

        protected void ParseBehaviour(string selectorText, HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            ICSSBehavior behavior = context.FindBehavior(selectorText);

            if (behavior != null)
            {
                behavior.Do(node, htmlStyles);
            }
        }

        internal CSSelector SetSuccessor(CSSelector selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            successor = selector;

            return this;
        }
    }
}
