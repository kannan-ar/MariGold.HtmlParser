namespace MariGold.HtmlParser
{
    using System;

    public static class CSSUtility
    {
        public static decimal CalculateRelativeChildFontSize(string parentFontSize,string childFontSize)
        {
            return new FontSizeProperty().CalculateChildNodeFontSize(parentFontSize, childFontSize);
        }
    }
}
