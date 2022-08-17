namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal sealed class AttributeAnalyzer : HtmlAnalyzer
    {
        private enum Mode
        {
            Invalid,
            KeySeek,
            AssignSeek,
            ValueSeek
        }

        private Action<int, HtmlNode> analyze;
        private int start;
        private string key;
        private Dictionary<string, string> attributes;
        private char quote;
        private Mode mode;

        public AttributeAnalyzer(IAnalyzerContext context)
            : base(context)
        {
            analyze = KeySeek;
            start = -1;
            quote = char.MinValue;
            mode = Mode.Invalid;
            attributes = new Dictionary<string, string>();
        }

        private void Clear()
        {
            start = -1;
            key = string.Empty;
            quote = char.MinValue;
            mode = Mode.Invalid;
        }

        private void KeySeek(int position, HtmlNode node)
        {
            char letter = context.Html[position];
            mode = Mode.KeySeek;

            if (start == -1 && IsValidHtmlLetter(letter))
            {
                start = position;
            }
            else if (quote == char.MinValue && (letter == HtmlTag.singleQuote || letter == HtmlTag.doubleQuote))
            {
                start = position + 1;
                quote = letter;
            }
            else if (letter == HtmlTag.equalSign && start > -1 && position > start)
            {
                key = context.Html.Substring(start, position - start);
                start = -1;
                quote = char.MinValue;
                analyze = ValueSeek;
            }
            else if (start > -1 && quote != char.MinValue && start == position && letter == quote)//A quote is already opened and a close quote found with empty content inbetween. So resetting the indexes
            {
                start = -1;
                quote = char.MinValue;
            }
            else if (start > -1 && ((!IsValidHtmlLetter(letter) && quote == char.MinValue) ||
                letter == quote || (letter == HtmlTag.closeAngle && quote == char.MinValue)) && position > start)
            {
                key = context.Html.Substring(start, position - start);
                quote = char.MinValue;
                start = -1;

                if (letter != HtmlTag.closeAngle)
                {
                    analyze = AssignSeek;
                }
                else
                {
                    if (!string.IsNullOrEmpty(key) && !attributes.ContainsKey(key))
                    {
                        attributes.Add(key, string.Empty);
                        Clear();
                    }

                    analyze = null;
                }
            }
        }

        private void AssignSeek(int position, HtmlNode node)
        {
            char letter = context.Html[position];
            bool isQuote = letter == HtmlTag.doubleQuote || letter == HtmlTag.singleQuote;
            mode = Mode.AssignSeek;

            if (letter == HtmlTag.equalSign)
            {
                start = -1;
                quote = char.MinValue;
                analyze = ValueSeek;
            }
            else if (IsValidHtmlLetter(letter) || isQuote)
            {
                if (!string.IsNullOrEmpty(key) && !attributes.ContainsKey(key))
                {
                    attributes.Add(key, string.Empty);
                    Clear();
                }

                if (isQuote)
                {
                    start = position + 1;
                    quote = letter;
                }
                else
                {
                    start = position;
                }

                analyze = KeySeek;
            }
        }

        private void ValueSeek(int position, HtmlNode node)
        {
            char letter = context.Html[position];
            mode = Mode.ValueSeek;

            if (start == -1 && IsValidHtmlLetter(letter))
            {
                start = position;
            }
            else if (quote == char.MinValue && (letter == HtmlTag.singleQuote || letter == HtmlTag.doubleQuote))
            {
                start = position + 1;
                quote = letter;
            }
            else if (start > -1 &&
                ((!IsValidHtmlLetter(letter) && quote == char.MinValue) ||
                letter == quote || (letter == HtmlTag.closeAngle && quote == char.MinValue)) && position >= start)
            {
                if (letter != HtmlTag.closeAngle)
                {
                    analyze = KeySeek;
                }
                else
                {
                    analyze = null;
                }

                if (!string.IsNullOrEmpty(key) && !attributes.ContainsKey(key))
                {
                    string value = context.Html.Substring(start, position - start);
                    attributes.Add(key, value);
                }

                Clear();
            }
        }

        protected override bool ProcessHtml(int position, ref HtmlNode node)
        {
            analyze?.Invoke(position, node);

            return false;
        }

        internal void Finalize(int position, ref HtmlNode node)
        {
            foreach (KeyValuePair<string, string> attribute in attributes)
            {
                node.Attributes.Add(attribute.Key, attribute.Value);
            }

            if (string.IsNullOrEmpty(key) && start > -1 && position > start)
            {
                key = context.Html.Substring(start, position - start);
                start = -1;
            }

            if (!string.IsNullOrEmpty(key) && !node.Attributes.ContainsKey(key))
            {
                string value = string.Empty;

                if (start > -1 && position > start)
                {
                    value = context.Html.Substring(start, position - start);
                }

                node.Attributes.Add(key, value);
                Clear();
            }

            analyze = null;
        }

        internal bool IsQuotedValueSeek()
        {
            return (quote == HtmlTag.singleQuote || quote == HtmlTag.doubleQuote) && mode == Mode.ValueSeek;
        }
    }
}
