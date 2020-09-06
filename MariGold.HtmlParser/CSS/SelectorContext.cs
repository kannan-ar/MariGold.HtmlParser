namespace MariGold.HtmlParser
{
    using System.Collections.Generic;

    internal sealed class SelectorContext : ISelectorContext
    {
        internal SelectorContext()
        {
            AttachedSelectors = new List<IAttachedSelector>();

            CSSBehaviors = new List<CSSBehavior>() {
				new ApplyImmediateChildren(this),
				new ApplyNextElement(this),
				new ApplyAllNextElement(this),
				new ApplyAllChildren(this)
			};

            Selectors = new List<CSSelector>() {
				new IdentitySelector(this),
				new ClassSelector(this),
				new AttributeSelector(this),
				new ElementSelector(this),
				new FirstChildSelector(this),
				new LastChildSelector(this),
				new NotSelector(this),
				new NthChildSelector(this),
				new NthLastChildSelector(this),
				new OnlyChildSelector(this),
				new GlobalSelector(this)
			};
        }

        public List<CSSBehavior> CSSBehaviors { get; }

        public List<CSSelector> Selectors { get; }

        public List<IAttachedSelector> AttachedSelectors { get; }

        public void AddAttachedSelector(IAttachedSelector selector)
        {
            if (!AttachedSelectors.Contains(selector))
            {
                AttachedSelectors.Add(selector);
            }
        }

        public bool ParseSelector(string selectorText, out CSSelector selector)
        {
            selector = null;

            foreach (CSSelector item in Selectors)
            {
                if (item.Prepare(selectorText))
                {
                    selector = item.Clone();

                    return true;
                }
            }

            return false;
        }

        public bool ParseBehavior(string selectorText, Specificity specificity, HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            bool foundBehavior = false;

            foreach (CSSBehavior behavior in CSSBehaviors)
            {
                if (behavior.IsValidBehavior(selectorText))
                {
                    foundBehavior = true;

                    behavior.Parse(node, specificity, htmlStyles);

                    break;
                }
            }

            return foundBehavior;
        }

        public bool ParseSelectorOrBehavior(string selectorText, Specificity specificity, HtmlNode node, List<HtmlStyle> htmlStyles)
        {
            bool found = false;

            foreach (IAttachedSelector selector in AttachedSelectors)
            {
                if (selector.Prepare(selectorText))
                {
                    if (selector.IsValidNode(node))
                    {
                        found = true;
                        selector.AddSpecificity(specificity);
                        selector.Parse(node, htmlStyles);
                    }

                    break;
                }
            }

            if (!found)
            {
                found = ParseBehavior(selectorText, specificity, node, htmlStyles);
            }

            return found;
        }
    }
}
