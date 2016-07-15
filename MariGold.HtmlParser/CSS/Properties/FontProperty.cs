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
            string value;

            if (!ExtractProperty(font, new char[] { space }, 0, out index, out value))
            {
                return false;
            }

            styleStack.Key.Push(fontStyle);

            if (Contains(fontStyles, value))
            {
                styleList.Add(fontStyle, value);
            }
            else if (Contains(styles, value))
            {
                styleStack.Value.Push(value);
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
            string value;

            if (!ExtractProperty(font, new char[] { space }, startIndex, out index, out value))
            {
                return false;
            }

            styleStack.Key.Push(fontVariant);

            if (Contains(fontStyles, value))
            {
                ProcessNormalValues(styleList, styleStack);
                styleList.Add(fontVariant, value);
            }
            else if (Contains(styles, value))
            {
                styleStack.Value.Push(value);
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
            string value;

            if (!ExtractProperty(font, new char[] { space }, startIndex, out index, out value))
            {
                return false;
            }

            int weight;
            styleStack.Key.Push(fontWeight);

            if (Contains(fontStyles, value) || Int32.TryParse(value, out weight))
            {
                ProcessNormalValues(styleList, styleStack);
                styleList.Add(fontWeight, value);
            }
            else if (Contains(styles, value))
            {
                styleStack.Value.Push(value);
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
            string value;

            if (!ExtractProperty(font, new char[] { slash, space }, startIndex, out index, out value))
            {
                return false;
            }

            if (Contains(fontStyles, value))
            {
                ProcessNormalValues(styleList, styleStack);
                styleList.Add(fontSize, value);
            }
            else if (ContainsText(lengthTypes, value))
            {
                ProcessNormalValues(styleList, styleStack);
                styleList.Add(fontSize, value);
            }
            else if (value.Contains("%"))
            {
                ProcessNormalValues(styleList, styleStack);
                styleList.Add(fontSize, value);
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
            string value;
            int index;

            if (ExtractProperty(font, new char[] { space }, startIndex, out index, out value))
            {
                if (Contains(fontStyles, value))
                {
                    styleList.Add(lineHeight, value);
                }
                else if (ContainsText(lengthTypes, value))
                {
                    styleList.Add(lineHeight, value);
                }
                else if (value.Contains("%"))
                {
                    styleList.Add(lineHeight, value);
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
            string value = font.Substring(startIndex).Trim();

            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            int index = -1;

            foreach (string fontProperty in fontProperties)
            {
                index = value.IndexOf(fontProperty);

                if (index != -1)
                {
                    value = value.Remove(index).Trim();
                    break;
                }
            }

            if (!string.IsNullOrEmpty(value))
            {
                styleList.Add(fontFamily, value);
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
                foreach (var style in parentStyles)
                {
                    if (!child.HasStyle(style.Key))
                    {
                        child.HtmlStyles.Add(new HtmlStyle(style.Key, style.Value, false));
                    }
                }
            }
        }

        private void ProcessFontFamily(HtmlStyle parentStyle, HtmlNode child)
        {
            if (!child.HasStyle(fontFamily) && !child.HasStyle(font))
            {
                child.HtmlStyles.Add(parentStyle.Clone());
            }
        }

        internal override bool AppendStyle(HtmlStyle style, HtmlNode node)
        {
            if (style == null || node == null)
            {
                return false;
            }

            if (style.Name.CompareInvariantCultureIgnoreCase(fontFamily))
            {
                ProcessFontFamily(style, node);
                return true;
            }
            else if (style.Name.CompareInvariantCultureIgnoreCase(font))
            {
                ProcessFont(style, node);
                return true;
            }

            return false;
        }

        internal override void ParseStyle(HtmlNode node)
        {
            HtmlStyle value;

            Dictionary<string, string> styles = new Dictionary<string, string>();
            KeyValuePair<Stack<string>, Stack<string>> styleStack =
                new KeyValuePair<Stack<string>, Stack<string>>(new Stack<string>(), new Stack<string>());

            if (node.TryGetStyle(font, out value))
            {
                if (ProcessFontStyle(value.Value, styles, styleStack))
                {
                    foreach (var style in styles)
                    {
                        if (!node.HasStyle(style.Key))
                        {
                            node.HtmlStyles.Add(new HtmlStyle(style.Key, style.Value, false));
                        }
                    }

                    node.RemoveStyle(font);
                }
            }
        }
    }
}
