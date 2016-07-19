namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal sealed class TextAnalyzer : HtmlAnalyzer
    {
        private readonly int startPosition;
        private readonly HtmlNode parent;

        public TextAnalyzer(IAnalyzerContext context, int position)
            : base(context)
        {
            if (position < 0)
            {
                throw new ArgumentOutOfRangeException("position");
            }

            this.startPosition = position;
        }

        public TextAnalyzer(IAnalyzerContext context, int position, HtmlNode parent)
            : this(context, position)
        {
            this.parent = parent;
        }

        protected override void Finalize(int position, ref HtmlNode node)
        {
        }

        protected override bool ProcessHtml(int position, ref HtmlNode node)
        {
            bool tagCreated = false;
            char letter = context.Html[position];
            IOpenTag openTag;
            ICloseTag closeTag;

            ProcessQuote(letter);

            if (!QuoteOpened && IsOpenTag(position, out openTag))
            {
                //+1 is required because the Html is zero based array so the position is always -1 of total length.
                tagCreated = CreateTag(HtmlTag.TEXT, startPosition, startPosition, position,
                    position, parent, out node);

                context.SetAnalyzer(openTag.GetAnalyzer(position, parent));
            }
            else if (!QuoteOpened && IsCloseTag(position, out closeTag))
            {
                tagCreated = CreateTag(HtmlTag.TEXT, startPosition, startPosition, position,
                    position, parent, out node);

                closeTag.Init(position, parent);
                context.SetAnalyzer(closeTag.GetAnalyzer());
            }
            else if (position + 1 == context.EOF)//Reached EOF and still there are no tag created. 
            {                                    //So lets process if there is any pending text.
                tagCreated = CreateTag(HtmlTag.TEXT, startPosition, startPosition, position + 1,
                    position + 1, parent, out node);
            }

            return tagCreated;
        }
    }
}
