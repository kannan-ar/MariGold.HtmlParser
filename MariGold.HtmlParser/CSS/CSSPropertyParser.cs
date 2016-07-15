namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal class CSSPropertyParser
    {
        private List<CSSProperty> properties;

        private void Init()
        {
            properties = new List<CSSProperty>(){
                new FontProperty()
            };
        }

        internal CSSPropertyParser()
        {
            Init();
        }

        internal bool InheritStyle(HtmlStyle parentStyle, HtmlNode child)
        {
            foreach (CSSProperty property in properties)
            {
                if (property.AppendStyle(parentStyle, child))
                {
                    return true;
                }
            }

            return false;
        }

        internal void ParseStyle(HtmlNode node)
        {
            foreach (CSSProperty property in properties)
            {
                property.ParseStyle(node);
            }

            foreach (HtmlNode child in node.GetChildren())
            {
                ParseStyle(child);
            }

            if (node.Parent == null && node.Next != null)
            {
                ParseStyle(node.GetNext());
            }
        }
    }
}
