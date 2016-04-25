namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal sealed class AttributeAnalyzer : HtmlAnalyzer
    {
        private Action<int, HtmlNode> analyze;
        private int start;
        private string key;
        private char quote;
        private Dictionary<string, string> attributes;

        public AttributeAnalyzer(IAnalyzerContext context)
            : base(context)
        {
            analyze = KeySeek;
            start = -1;
            quote = char.MinValue;
            attributes = new Dictionary<string, string>();
        }

        private void Clear()
        {
            start = -1;
            key = string.Empty;
            quote = char.MinValue;
        }

        private void KeySeek(int position, HtmlNode node)
        {
            char letter = context.Html[position];

            if (start == -1 && IsValidHtmlLetter(letter))
            {
                start = position;
            }
            else if (quote == char.MinValue && (letter == HtmlTag.singleQuote || letter == HtmlTag.doubleQuote))
            {
                start = position + 1;
                quote = letter;
            }
            else if (letter == HtmlTag.equalSign && position > start)
            {
                key = context.Html.Substring(start, position - start);
                start = -1;
                quote = char.MinValue;
                analyze = ValueSeek;
            }
            else if (start > -1 && ((!IsValidHtmlLetter(letter) && quote == char.MinValue) ||
                letter == quote || letter == HtmlTag.closeAngle) && position > start)
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
                
                if(isQuote)
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
                letter == quote || letter == HtmlTag.closeAngle) && position > start)
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
                    Clear();
                }
            }
        }

        protected override void Finalize(int position, ref HtmlNode node)
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

        protected override bool ProcessHtml(int position, ref HtmlNode node)
        {
            if (analyze != null)
            {
                analyze(position, node);
            }

            return false;
        }
    }
}
