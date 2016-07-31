namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class FontSizeProperty : CSSProperty
    {
        private readonly Dictionary<string, string> defaultFontSizes;

        private void Init()
        {
            defaultFontSizes.Add("xx-small", "9px");
            defaultFontSizes.Add("x-small", "10px");
            defaultFontSizes.Add("smaller", "10.83px");
            defaultFontSizes.Add("small", "13px");
            defaultFontSizes.Add("medium", "16px");
            defaultFontSizes.Add("large", "18px");
            defaultFontSizes.Add("larger", "19.2px");
            defaultFontSizes.Add("x-large", "24px");
            defaultFontSizes.Add("xx-large", "32px");
        }

        private bool ExtractNamedFontSize(string value, out string fontSize)
        {
            fontSize = string.Empty;

            foreach (var item in defaultFontSizes)
            {
                if (item.Key.CompareOrdinalIgnoreCase(value))
                {
                    fontSize = item.Value;
                    return true;
                }
            }

            return false;
        }

        internal FontSizeProperty()
        {
            defaultFontSizes = new Dictionary<string, string>();

            Init();
        }

        internal override bool AppendStyle(HtmlStyle parentStyle, HtmlNode child)
        {
            if (parentStyle == null || child == null)
            {
                return false;
            }

            if (!parentStyle.Name.CompareInvariantCultureIgnoreCase(fontSize))
            {
                return false;
            }

            HtmlStyle childFontSize;

            if (!child.HasStyle(fontSize))
            {
                child.HtmlStyles.Add(new HtmlStyle(parentStyle.Name, parentStyle.Value, false));
                return true;
            }
            else if (!child.TryGetStyle(fontSize, out childFontSize))
            {
                return true;
            }

            string parentFontSizeValue = parentStyle.Value;
            string childFontSizeValue = childFontSize.Value;

            if ((string.IsNullOrEmpty(childFontSizeValue) || string.IsNullOrEmpty(parentFontSizeValue))
                && !childFontSizeValue.Contains("%"))
            {
                return true;
            }

            childFontSizeValue = childFontSizeValue.Replace("%", string.Empty).Trim();
            decimal decChildFontSize;

            if (!decimal.TryParse(childFontSizeValue, out decChildFontSize))
            {
                return true;
            }

            string namedFontSize;

            if (parentFontSizeValue.Contains("%"))
            {
                parentFontSizeValue = parentFontSizeValue.Replace("%", string.Empty);
                decimal decParentFontSize;

                if (decimal.TryParse(parentFontSizeValue, out decParentFontSize))
                {
                    childFontSize.ModifyStyle(
                        string.Concat(decimal.Round((decParentFontSize / 100) * decChildFontSize).ToString(), "%"));
                }

                return true;
            }
            else if (ExtractNamedFontSize(parentFontSizeValue, out namedFontSize))
            {
                parentFontSizeValue = namedFontSize;
            }

            Match match = Regex.Match(parentFontSizeValue, "^(\\.)?\\d+(\\.?\\d+)?");
            decimal decParentFontStyle;

            if (match.Success && decimal.TryParse(match.Value, out decParentFontStyle))
            {
                decParentFontStyle = decimal.Round(decParentFontStyle * (decChildFontSize / 100));

                childFontSize.ModifyStyle(string.Concat(decParentFontStyle,
                    parentFontSizeValue.Length > match.Length ? parentFontSizeValue.Substring(match.Length) : string.Empty));
            }

            return true;
        }

        internal override void ParseStyle(HtmlNode node)
        {

        }
    }
}
