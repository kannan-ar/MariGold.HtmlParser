namespace MariGold.HtmlParser
{
    public static class CSSUtility
    {
        public static decimal CalculateRelativeChildFontSize(string parentFontSize,string childFontSize)
        {
            return new FontSizeProperty().CalculateChildNodeFontSize(parentFontSize, childFontSize);
        }
    }
}
