namespace MariGold.HtmlParser
{
    using System;

    public static class CSSUtility
    {
        public static decimal CalculateRelativeChildFontSize(string parentFontSize,string childFontSize)
        {
            FontSizeProperty fontSize = new FontSizeProperty();

            return fontSize.CalculateChildNodeFontSize(parentFontSize, childFontSize);
        }
    }
}
