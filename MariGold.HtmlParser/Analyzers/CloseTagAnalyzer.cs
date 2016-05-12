namespace MariGold.HtmlParser
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

        private bool CloseOpenedChilds(HtmlNode current, string closeTag, int textEnd, int htmlEnd, ref HtmlNode newNode)
        {
            bool tagFound = false;

            if (current != null)
            {
                if (string.Compare(current.Tag, closeTag, StringComparison.InvariantCultureIgnoreCase) == 0 && current.IsOpened)
                {
                    current.SetBoundary(textEnd, htmlEnd);
                    newNode = current;
                    return true;
                }

                tagFound = CloseOpenedChilds(current.GetParent(), closeTag, textEnd, htmlEnd, ref newNode);

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
                        ignoreTag = string.Compare(current.Tag, tag, StringComparison.InvariantCultureIgnoreCase) != 0;
                    }
                }
            }

            if (letter == HtmlTag.closeAngle)
            {
                HtmlNode nextNode = current;
                HtmlNode newNode = null;

                if (current != null)
                {
                    CloseOpenedChilds(current, tag, startPosition, position + 1, ref newNode);

                    if (newNode != null)
                    {
                        nextNode = newNode;
                    }
                }

                node = current;

                if (newNode != null)
                {
                    nextNode = newNode.GetParent();
                    tagCreated = newNode.Parent == null;
                }

                //The nextNode is the parent node for next element
                if (!AssignNextAnalyzer(position + 1, nextNode))
                {
                    context.SetAnalyzer(context.GetTextAnalyzer(position + 1, nextNode));
                }

                //Sometimes invalid close tags may have in the html. In that case the newNode will be null because the 
                //CloseTagAnalyzer can't find an appropriate open tag. Setting null to previous node will disconnect the
                //Node chain. 
                if (newNode != null)
                {
                    InnerTagClosed(newNode);
                }
            }

            return tagCreated;
        }
    }
}
