﻿namespace MariGold.HtmlParser
{
    using System;

    internal sealed class CloseTagAnalyzer : HtmlAnalyzer, ICloseTag
    {
        private int startPosition;
        private HtmlNode current;
        private bool ignoreTag;
        private int tagStart;
        private string tag;

        public CloseTagAnalyzer(IAnalyzerContext context)
            : base(context)
        {
            tagStart = -1;
        }

        private bool CloseOpenedChilds(HtmlNode current, string closeTag, int textEnd, int htmlEnd)
        {
            bool tagFound = false;

            if (current != null)
            {
                if (string.Compare(current.Tag, closeTag, true) == 0 && current.IsOpened)
                {
                    current.SetBoundary(textEnd, htmlEnd);
                    return true;
                }

                tagFound = CloseOpenedChilds(current.Parent, closeTag, textEnd, htmlEnd);

                if (tagFound)
                {
                    current.Finilize(textEnd);
                }
            }

            return tagFound;
        }

        public bool IsCloseTag(int position, string html)
        {
            return position + 1 < context.EOF && html[position] == HtmlTag.openAngle &&
                html[position + 1] == HtmlTag.escapeChar;
        }

        public void Init(int position, HtmlNode current)
        {
            if (startPosition < 0)
            {
                throw new ArgumentOutOfRangeException("position");
            }

            this.startPosition = position;
            this.current = current;
            this.tagStart = -1;
            this.tag = string.Empty;
            ignoreTag = current == null;

            if (current != null)
            {
                ignoreTag = string.IsNullOrEmpty(current.Tag);
            }
        }

        public HtmlAnalyzer GetAnalyzer()
        {
            return this;
        }

        protected override void Finalize(int position, ref HtmlNode node)
        {
        }

        protected override bool ProcessHtml(int position, ref HtmlNode node)
        {
            bool tagCreated = false;
            char letter = context.Html[position];

            if (!ignoreTag)
            {
                if (tagStart == -1 && IsValidHtmlLetter(letter))
                {
                    tagStart = position;
                }

                if (string.IsNullOrEmpty(tag) && tagStart > -1 && tagStart <= position && position + 1 < context.EOF &&
                    !IsValidHtmlLetter(context.Html[position + 1]))
                {
                    tag = context.Html.Substring(tagStart, position - tagStart + 1);

                    if (current != null)
                    {
                        ignoreTag = string.Compare(current.Tag, tag, true) != 0;
                    }
                }
            }

            if (letter == HtmlTag.closeAngle)
            {
                HtmlNode nextNode = current;

                if (current != null)
                {
                    CloseOpenedChilds(current, tag, startPosition, position + 1);
                }

                node = current;

                if (!ignoreTag && current != null)
                {
                    nextNode = current.Parent;
                    tagCreated = current.Parent == null;
                }

                if (!AssignNextAnalyzer(position + 1, nextNode))
                {
                    context.SetAnalyzer(context.GetTextAnalyzer(position + 1, nextNode));
                }

                InnerTagClosed(current);
            }

            return tagCreated;
        }
    }
}
