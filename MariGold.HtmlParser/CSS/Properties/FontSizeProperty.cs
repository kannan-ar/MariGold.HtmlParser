namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class FontSizeProperty : CSSProperty
    {
        private const bool PROCESSED = true;
        private const bool NOT_PROCESSED = false;

        private const decimal defaultFontSize = 16;
        private readonly Dictionary<string, decimal> defaultFontSizes;
        private readonly Dictionary<string, decimal> absoluteNamedFontSizes;
        private readonly Dictionary<string, decimal> relativeNamedFontSizes;

        Regex decimalValue;

        private void Init()
        {
            defaultFontSizes.Add("px", 1);
            defaultFontSizes.Add("pt", 1.33m);
            defaultFontSizes.Add("em", 16);
            defaultFontSizes.Add("in", 96);

            absoluteNamedFontSizes.Add("xx-small", 9);
            absoluteNamedFontSizes.Add("x-small", 10);
            absoluteNamedFontSizes.Add("small", 13);
            absoluteNamedFontSizes.Add("medium", 16);
            absoluteNamedFontSizes.Add("large", 18);
            absoluteNamedFontSizes.Add("x-large", 24);
            absoluteNamedFontSizes.Add("xx-large", 32);

            relativeNamedFontSizes.Add("smaller", .83m);
            relativeNamedFontSizes.Add("larger", 1.2m);
        }

        private bool ExtractFontSize(string value, out decimal fontSize)
        {
            fontSize = 0;

            foreach (var item in defaultFontSizes)
            {
                if (value.IndexOf(item.Key, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    fontSize = item.Value;
                    return true;
                }
            }

            return false;
        }

        private decimal ParseFontSize(string value)
        {
            decimal fontSize;

            Match match = decimalValue.Match(value);

            if (match.Success && decimal.TryParse(match.Value, out fontSize))
            {
                return fontSize;
            }
            else
            {
                return 0;
            }
        }

        private bool ExtractNamedFontSize(string value, out decimal fontSize)
        {
            fontSize = 0;

            foreach (var item in absoluteNamedFontSizes)
            {
                if (item.Key.CompareOrdinalIgnoreCase(value))
                {
                    fontSize = item.Value;
                    return true;
                }
            }

            foreach (var item in relativeNamedFontSizes)
            {
                if (item.Key.CompareOrdinalIgnoreCase(value))
                {
                    fontSize = item.Value;
                    return true;
                }
            }

            return false;
        }

        private decimal ConvertParentFontSize(string fontSizeValue)
        {
            if (string.IsNullOrEmpty(fontSizeValue))
            {
                return 0;
            }

            decimal fontSize = 0;

            if (fontSizeValue.Contains("%") &&
                decimal.TryParse(fontSizeValue.Replace("%", ""), out fontSize))
            {
                fontSize = defaultFontSize / 100 * fontSize;
            }
            else
            {
                if (ExtractNamedFontSize(fontSizeValue, out fontSize))
                {
                    return fontSize;
                }
                else if (ExtractFontSize(fontSizeValue, out fontSize))
                {
                    return fontSize * ParseFontSize(fontSizeValue);
                }
            }

            return fontSize;
        }

        private decimal ConvertChildFontSize(string fontSizeValue, decimal parentFontSize)
        {
            decimal fontSize = 0;

            if (fontSizeValue.Contains("%") &&
                decimal.TryParse(fontSizeValue.Replace("%", ""), out fontSize))
            {
                fontSize = parentFontSize / 100 * fontSize;
            }
            else if (fontSizeValue.Contains("em"))
            {
                Match match = decimalValue.Match(fontSizeValue);

                if (match.Success && decimal.TryParse(match.Value, out fontSize))
                {
                    fontSize = (parentFontSize / defaultFontSize) * fontSize * defaultFontSize;
                }
            }
            else
            {
                foreach (var item in relativeNamedFontSizes)
                {
                    if (item.Key.CompareOrdinalIgnoreCase(fontSizeValue))
                    {
                        fontSize = item.Value * parentFontSize;
                        break;
                    }
                }
            }

            return fontSize;
        }

        private bool IsAbsoluteFont(string fontSizeValue)
        {
            foreach (var item in relativeNamedFontSizes)
            {
                if (item.Key.CompareOrdinalIgnoreCase(fontSizeValue))
                {
                    return true;
                }
            }

            return false;
        }

        internal FontSizeProperty()
        {
            defaultFontSizes = new Dictionary<string, decimal>();
            absoluteNamedFontSizes = new Dictionary<string, decimal>();
            relativeNamedFontSizes = new Dictionary<string, decimal>();

            decimalValue = new Regex("^(\\.)?\\d+(\\.?\\d+)?");

            Init();
        }

        internal decimal CalculateChildNodeFontSize(string parentFontSizeValue, string childFontSizeValue)
        {
            if (!childFontSizeValue.Contains("%") && !childFontSizeValue.Contains("em") &&
                !IsAbsoluteFont(childFontSizeValue))
            {
                return 0;
            }

            decimal parentFontSize = ConvertParentFontSize(parentFontSizeValue);

            if (parentFontSize == 0)
            {
                return 0;
            }

            return ConvertChildFontSize(childFontSizeValue, parentFontSize);
        }

        internal override bool AppendStyle(HtmlStyle parentStyle, HtmlNode child)
        {
            if (parentStyle == null || child == null)
            {
                return NOT_PROCESSED;
            }

            if (!parentStyle.Name.CompareOrdinalIgnoreCase(fontSize))
            {
                return NOT_PROCESSED;
            }

            HtmlStyle childFontSize;

            if (!child.HasStyle(fontSize))
            {
                child.UpdateInheritedStyles(parentStyle);
                return PROCESSED;
            }
            else if (!child.TryGetStyle(fontSize, out childFontSize) &&
                !child.TryGetInheritedStyle(fontSize, out childFontSize))
            {
                return PROCESSED;
            }

            string parentFontSizeValue = parentStyle.Value;
            string childFontSizeValue = childFontSize.Value;

            if ((string.IsNullOrEmpty(childFontSizeValue) || string.IsNullOrEmpty(parentFontSizeValue)))
            {
                return PROCESSED;
            }

            decimal childFont = CalculateChildNodeFontSize(parentFontSizeValue, childFontSizeValue);

            if (childFont != 0)
            {
                childFontSize.ModifyStyle(string.Concat(childFont.ToString("G29"), "px"));
            }

            return PROCESSED;
        }

        internal override void ParseStyle(HtmlNode node)
        {

        }
    }
}
