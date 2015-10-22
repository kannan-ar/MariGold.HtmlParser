namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal sealed class SelectorContext : ISelectorContext
    {
        private CSSelector selector;
        private List<CSSBehavior> behaviors;

        public List<CSSBehavior> CSSBehaviors
        {
            get
            {
                if (behaviors == null)
                {
                    behaviors = new List<CSSBehavior>()
                    {
                        new ApplyAllChildren(this),
                        new ApplyImmediateChildren(this),
                        new ApplyNextElement(this),
                        new ApplyAllNextElement(this),
                        new ApplyAttribute(this),
                        new ApplyFirstChild(this)
                    };
                }

                return behaviors;
            }
        }

        public CSSelector Selector
        {
            get
            {
                if (selector == null)
                {
                    selector = new IdentitySelector(this).SetSuccessor(
                        new ClassSelector(this).SetSuccessor(
                        new ElementSelector(this).SetSuccessor(
                        new GlobalSelector())));
                }

                return selector;
            }
        }

        public CSSBehavior FindBehavior(string selectorText)
        {
            foreach (CSSBehavior behavior in CSSBehaviors)
            {
                if (behavior.IsValidBehavior(selectorText))
                {
                    return behavior;
                }
            }

            return null;
        }
    }
}
