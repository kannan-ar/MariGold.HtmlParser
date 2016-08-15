namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal sealed class HtmlStyle
    {
        private string name;
        private string value;
        private bool important;
        private Specificity specificity;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Value
        {
            get
            {
                return value;
            }
        }

        public bool Important
        {
            get
            {
                return important;
            }

            internal set
            {
                important = value;
            }
        }

        internal Specificity Specificity
        {
            get
            {
                return specificity;
            }

            set
            {
                specificity = value;
            }
        }

        internal HtmlStyle(string name, string value, bool important)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }

            this.name = name;
            this.value = value;
            this.important = important;
        }

        internal HtmlStyle(string name, string value, bool important, Specificity specificity)
            : this(name, value, important)
        {
            this.specificity = specificity;
        }

        internal void OverWrite(HtmlStyle htmlStyle)
        {
            CSSPropertyParser propertyParser = new CSSPropertyParser();

            //if (string.Compare(name, htmlStyle.Name, StringComparison.InvariantCultureIgnoreCase) == 0)
            if (propertyParser.StyleContains(this, htmlStyle))
            {
                if (!important && htmlStyle.Important)
                {
                    name = htmlStyle.Name;
                    value = htmlStyle.Value;
                    important = htmlStyle.Important;
                    specificity = htmlStyle.Specificity;
                }
                else if (htmlStyle.Specificity >= specificity && (!important || (important && htmlStyle.Important)))
                {
                    name = htmlStyle.Name;
                    value = htmlStyle.Value;
                }
            }
        }

        internal void ModifyStyle(string value)
        {
            this.value = value;
        }

        internal HtmlStyle Clone()
        {
            return new HtmlStyle(name, value, important, specificity);
        }

        static internal bool IsNonStyleElement(string tag)
        {
            tag = tag.ToLower();

            return tag == "style" || tag == "script" || tag == "html";
        }
    }
}
