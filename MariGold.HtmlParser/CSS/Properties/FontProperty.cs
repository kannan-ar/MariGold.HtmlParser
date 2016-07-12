namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    internal sealed class FontProperty : CSSProperty
    {
        const char space = ' ';
        const char slash = '/';
        static string[] styles = { "normal", "initial", "inherit" };

        private bool Contains(string[] array, string value)
        {
            StringComparer stringComparer = StringComparer.Create(CultureInfo.InvariantCulture, true);

            foreach (string item in array)
            {
                if (stringComparer.Compare(item, value) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        private bool ContainsText(string[] array, string value)
        {
            foreach (string item in array)
            {
                if (value.Contains(item))
                {
                    return true;
                }
            }

            return false;
        }

        private bool ExtractProperty(string font, char[] letters, int startIndex, out int index, out string property)
        {
            property = string.Empty;
            bool found = false;
            char foundLetter = ' ';

            for (index = startIndex; index < font.Length; index++)
            {
                if (!found)
                {
                    foreach (char letter in letters)
                    {
                        if (font[index] == letter && index > startIndex)
                        {
                            found = true;
                            foundLetter = letter;
                            --index;
                            break;
                        }
                    }
                }
                else
                {
                    while (index + 1 < font.Length && font[index + 1] == foundLetter)
                    {
                        ++index;
                    }

                    break;
                }
            }

            if (index > startIndex)
            {
                property = font.Substring(startIndex, index - startIndex).Trim();
            }

            if (index < font.Length)
            {
                ++index;
            }

            return index >= startIndex;
        }

        private void ProcessNormalValues(
            Dictionary<string, string> styleList,
            KeyValuePair<Stack<string>, Stack<string>> styleStack)
        {
            while (styleStack.Key.Count > 0 && styleStack.Value.Count > 0)
            {
                styleList.Add(styleStack.Key.Pop(), styleStack.Value.Pop());
            }

            styleStack.Key.Clear();
            styleStack.Value.Clear();
        }

        private bool ProcessFontStyle(
            string font,
            Dictionary<string, string> styleList,
            KeyValuePair<Stack<string>, Stack<string>> styleStack)
        {
            string[] fontStyles = { "italic", "oblique" };

            int index;
            string fontStyle;

            if (!ExtractProperty(font, new char[] { space }, 0, out index, out fontStyle))
            {
                return false;
            }

            styleStack.Key.Push("font-style");

            if (Contains(fontStyles, fontStyle))
            {
                styleList.Add("font-style", fontStyle);
            }
            else if (Contains(styles, fontStyle))
            {
                styleStack.Value.Push(fontStyle);
            }
            else
            {
                index = 0;
            }

            return ProcessFontVariant(index, font, styleList, styleStack);
        }

        private bool ProcessFontVariant(
            int startIndex,
            string font,
            Dictionary<string, string> styleList,
            KeyValuePair<Stack<string>, Stack<string>> styleStack)
        {
            string[] fontStyles = { "small-caps" };
            int index;
            string fontVariant;

            if (!ExtractProperty(font, new char[] { space }, startIndex, out index, out fontVariant))
            {
                return false;
            }

            styleStack.Key.Push("font-variant");

            if (Contains(fontStyles, fontVariant))
            {
                ProcessNormalValues(styleList, styleStack);
                styleList.Add("font-variant", fontVariant);
            }
            else if (Contains(styles, fontVariant))
            {
                styleStack.Value.Push(fontVariant);
            }
            else
            {
                index = startIndex;
            }

            return ProcessFontWeight(index, font, styleList, styleStack);
        }

        private bool ProcessFontWeight(
            int startIndex,
            string font,
            Dictionary<string, string> styleList,
            KeyValuePair<Stack<string>, Stack<string>> styleStack)
        {
            string[] fontStyles = { "bold", "bolder", "lighter" };
            int index;
            string fontWeight;

            if (!ExtractProperty(font, new char[] { space }, startIndex, out index, out fontWeight))
            {
                return false;
            }

            int weight;
            styleStack.Key.Push("font-weight");

            if (Contains(fontStyles, fontWeight) || Int32.TryParse(fontWeight, out weight))
            {
                ProcessNormalValues(styleList, styleStack);
                styleList.Add("font-weight", fontWeight);
            }
            else if (Contains(styles, fontWeight))
            {
                styleStack.Value.Push(fontWeight);
            }
            else
            {
                index = startIndex;
            }

            return ProcessFontSize(index, font, styleList, styleStack);
        }

        private bool ProcessFontSize(
            int startIndex,
            string font,
            Dictionary<string, string> styleList,
            KeyValuePair<Stack<string>, Stack<string>> styleStack)
        {
            string[] fontStyles = { "medium", "xx-small", "x-small", "small", "large", "x-large", "xx-large", "smaller", "larger" };
            string[] lengthTypes = { "px", "pt", "em", "cm", "in" };
            int index;
            string fontSize;

            if (!ExtractProperty(font, new char[] { slash, space }, startIndex, out index, out fontSize))
            {
                return false;
            }

            if (Contains(fontStyles, fontSize))
            {
                ProcessNormalValues(styleList, styleStack);
                styleList.Add("font-size", fontSize);
            }
            else if (ContainsText(lengthTypes, fontSize))
            {
                ProcessNormalValues(styleList, styleStack);
                styleList.Add("font-size", fontSize);
            }
            else if (fontSize.Contains("%"))
            {
                ProcessNormalValues(styleList, styleStack);
                styleList.Add("font-size", fontSize);
            }
            else
            {
                styleList.Clear();
                return false;
            }


            return ProcessLineHeight(index, font, styleList);
        }

        private bool ProcessLineHeight(
            int startIndex,
            string font,
            Dictionary<string, string> styleList)
        {
            string[] fontStyles = { "medium", "xx-small", "x-small", "small", "large", "x-large", "xx-large", "smaller", "larger" };
            string[] lengthTypes = { "px", "pt", "em", "cm", "in" };
            string lineHeight;
            int index;

            if (ExtractProperty(font, new char[] { space }, startIndex, out index, out lineHeight))
            {
                if (Contains(fontStyles, lineHeight))
                {
                    styleList.Add("line-height", lineHeight);
                }
                else if (ContainsText(lengthTypes, lineHeight))
                {
                    styleList.Add("line-height", lineHeight);
                }
                else if (lineHeight.Contains("%"))
                {
                    styleList.Add("line-height", lineHeight);
                }
                else
                {
                    index = startIndex;
                }
            }

            return ProcessFontFamily(index, font, styleList);

        }

        private bool ProcessFontFamily(
            int startIndex,
            string font,
            Dictionary<string, string> styleList)
        {
            string[] fontProperties = { "caption", "icon", "menu", "message-box", "small-caption", "status-bar" };
            string fontFamily = font.Substring(startIndex).Trim();

            if (string.IsNullOrEmpty(fontFamily))
            {
                return false;
            }

            int index = -1;

            foreach (string fontProperty in fontProperties)
            {
                index = fontFamily.IndexOf(fontProperty);

                if (index != -1)
                {
                    fontFamily = fontFamily.Remove(index).Trim();
                    break;
                }
            }

            if (!string.IsNullOrEmpty(fontFamily))
            {
                styleList.Add("font-family", fontFamily);
                return true;
            }

            return false;
        }

        private void ProcessFont(HtmlStyle parentStyle, HtmlNode child)
        {
            Dictionary<string, string> parentStyles = new Dictionary<string, string>();
            KeyValuePair<Stack<string>, Stack<string>> styleStack =
                new KeyValuePair<Stack<string>, Stack<string>>(new Stack<string>(), new Stack<string>());

            if (ProcessFontStyle(parentStyle.Value, parentStyles, styleStack) && parentStyles.Count > 0)
            {
                Dictionary<string, string> childStyles = new Dictionary<string, string>();

                if (child.Styles.ContainsKey(font))
                {
                    styleStack = new KeyValuePair<Stack<string>, Stack<string>>(new Stack<string>(), new Stack<string>());

                    ProcessFontStyle(child.Styles[font], childStyles, styleStack);
                }

                foreach (var style in parentStyles)
                {
                    if(!child.Styles.ContainsKey(style.Key) && !childStyles.ContainsKey(style.Key))
                    {
                        child.HtmlStyles.Add(new HtmlStyle(style.Key, style.Value, false));
                    }
                }
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
