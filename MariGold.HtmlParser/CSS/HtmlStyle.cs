namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    public sealed class HtmlStyle
    {
        private string name;
        private string value;
        private bool important;
        private SelectorWeight weight;

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

        internal SelectorWeight Weight
        {
            get
            {
                return weight;
            }
        }

        internal HtmlStyle(string name, string value, bool important, SelectorWeight weight)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }

            if (weight < 0)
            {
                throw new ArgumentOutOfRangeException("weight");
            }

            this.name = name;
            this.value = value;
            this.important = important;
            this.weight = weight;
        }

        internal void OverWriteIfCan(HtmlStyle htmlStyle)
        {
            if (string.Compare(name, htmlStyle.Name, true) == 0)
            {
                if (htmlStyle.Weight < weight &&
                    ((!important && htmlStyle.Important) ||
                    important && htmlStyle.Important))
                {
                    value = htmlStyle.Value;
                    important = htmlStyle.Important;
                }
            }
        }
    }
}
