namespace MariGold.HtmlParser
{
    using System;

    internal sealed class OpenTagAnalyzer : HtmlAnalyzer, IOpenTag
    {
        private int startPosition;
        private HtmlNode parent;
        private int tagStart;
        private string tag;
        private AttributeAnalyzer attributeAnalyzer;

        public OpenTagAnalyzer(IAnalyzerContext context)
            : base(context)
        {
            tagStart = -1;
        }

        private bool IsValidSelfClosing(int position)
        {
            return position + 1 < context.EOF && context.Html[position] == HtmlTag.escapeChar &&
                context.Html[position + 1] == HtmlTag.closeAngle;
        }

        private void ExtractTag(int position)
        {
            if (tagStart > -1 && tagStart <= position)
            {
                tag = context.Html.Substring(tagStart, position - tagStart + 1);

                TagCreated(tag);
            }
        }

        private bool OnSelfClose(int position, ref HtmlNode node)
        {
            bool tagCreated = false;

            tagCreated = CreateTag(tag, startPosition, startPosition, position + 2, position + 2,
                    parent, out node);

            node.SetSelfClosing(true);
            //+ 2 is to find next position of />
            if (!AssignNextAnalyzer(position + 2, parent))
            {
                context.SetAnalyzer(context.GetTextAnalyzer(position + 2, parent));
            }

            context.SetPosition(position + 1);

            if (attributeAnalyzer != null)
            {
                attributeAnalyzer.Finalize(position, ref node);
            }

            return tagCreated;
        }

        private bool IsQuotedValueSeek()
        {
            if (attributeAnalyzer == null)
            {
                return false;
            }

            return attributeAnalyzer.IsQuotedValueSeek();
        }

        public bool IsOpenTag(int position, string html)
        {
            return position + 1 < context.EOF && html[position] == HtmlTag.openAngle &&
                char.IsLetter(html[position + 1]);
        }

        public HtmlAnalyzer GetAnalyzer(int position, HtmlNode parent)
        {
            if (position < 0)
            {
                throw new ArgumentOutOfRangeException("position");
            }

            OpenTagAnalyzer analyzer = new OpenTagAnalyzer(context);

            analyzer.startPosition = position;
            analyzer.parent = parent;
            analyzer.tagStart = -1;
            analyzer.tag = string.Empty;

            return analyzer;
        }
      
        protected override bool ProcessHtml(int position, ref HtmlNode node)
        {
            IOpenTag openTag;
            bool tagCreated = false;
            char letter = context.Html[position];

            if (tagStart == -1 && IsValidHtmlLetter(letter))
            {
                tagStart = position;
            }

            if (string.IsNullOrEmpty(tag) && tagStart > -1 && !IsValidHtmlLetter(letter))
            {
                ExtractTag(position - 1);

                InvalidTagHandler invalidTag = new InvalidTagHandler();
                invalidTag.CloseNonNestedParents(startPosition, tag, context, ref parent);

                attributeAnalyzer = new AttributeAnalyzer(context);
            }

            if (!IsQuotedValueSeek() && IsOpenTag(position, out openTag))
            {
                context.SetAnalyzer(openTag.GetAnalyzer(position, parent));
            }
            else if (!IsQuotedValueSeek() && IsValidSelfClosing(position))
            {
                tagCreated = OnSelfClose(position, ref node);
            }
            else if (!IsQuotedValueSeek() && letter == HtmlTag.closeAngle)
            {
                if (HtmlTag.IsSelfClosing(tag))
                {
                    tagCreated = CreateTag(tag, startPosition, startPosition, position + 1,
                        position + 1, parent, out node);

                    node.SetSelfClosing(true);

                    if (!AssignNextAnalyzer(position + 1, parent))
                    {
                        context.SetAnalyzer(context.GetTextAnalyzer(position + 1, parent));
                    }
                }
                else
                {
                    CreateTag(tag, startPosition, position + 1, -1, -1, parent, out node);

                    if (!AssignNextAnalyzer(position + 1, node))
                    {
                        context.SetAnalyzer(context.GetTextAnalyzer(position + 1, node));
                    }

                    InnerTagOpened(node);
                }

                if (attributeAnalyzer != null)
                {
                    attributeAnalyzer.Finalize(position, ref node);
                }
            }

            if (attributeAnalyzer != null)
            {
                attributeAnalyzer.Process(position, ref node);
            }

            return tagCreated;
        }
    }
}
