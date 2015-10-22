namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal sealed class SelectorContext : ISelectorContext
    {
        private CSSelector selector;
        private List<ICSSBehavior> behaviors;

        public List<ICSSBehavior> CSSBehaviors
        {
            get
            {
                if (behaviors == null)
                {
                    behaviors = new List<ICSSBehavior>()
                    {
                        new ApplyAllChildren(this),
                        new ApplyImmediateChildren(this),
                        new ApplyNextElement(this),
                        new ApplyAllNextElement(this),
                        new ApplyToAttribute(this)
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

        public ICSSBehavior FindBehavior(string selectorText)
        {
            foreach (ICSSBehavior behavior in CSSBehaviors)
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
