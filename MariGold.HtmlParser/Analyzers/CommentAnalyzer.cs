namespace MariGold.HtmlParser
{
    using System;

    internal sealed class CommentAnalyzer : HtmlAnalyzer, IOpenTag
    {
        private const string start = "<!--";
        private const string end = "-->";

        private int startPosition;
        private HtmlNode parent;
        private bool onTagExecuted;

        public CommentAnalyzer(IAnalyzerContext context)
            : base(context)
        {
            onTagExecuted = false;
        }

        protected override bool ProcessHtml(int position, ref HtmlNode node)
        {
            if (!onTagExecuted)
            {
                onTagExecuted = true;
                TagCreated(HtmlTag.COMMENT);
            }

            bool tagCreated = false;
            bool isEnd = position + 3 <= context.EOF && context.Html.Substring(position, 3) == end;

            if (isEnd)
            {
                tagCreated = CreateTag(HtmlTag.COMMENT, startPosition, startPosition, position + 3,
                    position + 3, parent, out node);

                if (!AssignNextAnalyzer(position + 3, parent))
                {
                    context.SetAnalyzer(context.GetTextAnalyzer(position + 3, parent));
                }

                context.SetPosition(position + 3);
            }

            return tagCreated;
        }
        
        public bool IsOpenTag(int position, string html)
        {
            return position + 4 <= context.EOF && html.Substring(position, 4) == start;
        }

        public HtmlAnalyzer GetAnalyzer(int position, HtmlNode parent)
        {
            if (position < 0 || position > context.EOF)
            {
                throw new ArgumentOutOfRangeException("position");
            }

            CommentAnalyzer analyzer = new CommentAnalyzer(context);

            analyzer.startPosition = position;
            analyzer.parent = parent;

            return analyzer;
        }
    }
}
