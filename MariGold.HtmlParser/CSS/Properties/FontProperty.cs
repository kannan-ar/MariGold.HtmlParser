namespace MariGold.HtmlParser
{
    using System;

    internal sealed class FontProperty : CSSProperty
    {
        private void ProcessFont(HtmlStyle parentStyle, HtmlNode child)
        {
            string[] values = parentStyle.Value.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string value in values)
            {

            }
        }

        private void ProcessFontFamily(HtmlStyle parentStyle, HtmlNode child)
        {
            if (!child.Styles.ContainsKey(fontFamily) && !child.Styles.ContainsKey(font))
            {
                child.HtmlStyles.Add(parentStyle.Clone());
            }
        }

        internal override bool AppendStyle(HtmlStyle parentStyle, HtmlNode child)
        {
            if (parentStyle == null || child == null)
            {
                return false;
            }

            if (parentStyle.Name.CompareInvariantCultureIgnoreCase(fontFamily))
            {
                ProcessFontFamily(parentStyle, child);
                return true;
            }
            else if (parentStyle.Name.CompareInvariantCultureIgnoreCase(font))
            {
                ProcessFont(parentStyle, child);
                return true;
            }

            return false;
        }
    }
}
