namespace MariGold.HtmlParser
{
    using System;
    using System.Collections.Generic;

    internal abstract class HtmlAnalyzer
    {
        protected readonly IAnalyzerContext context;

        internal event Action<string> OnTagCreate;
        
        protected bool IsOpenTag(int position, out IOpenTag openTag)
        {
            openTag = null;

            foreach (IOpenTag tag in context.OpenTags)
            {
                if (tag.IsOpenTag(position, context.Html))
                {
                    openTag = tag;
                    return true;
                }
            }

            return false;
        }

        protected bool IsCloseTag(int position, out ICloseTag closeTag)
        {
            closeTag = null;

            foreach (ICloseTag tag in context.CloseTags)
            {
                if (tag.IsCloseTag(position, context.Html))
                {
                    closeTag = tag;
                    return true;
                }
            }

            return false;
        }

        protected bool IsValidHtmlLetter(char letter)
        {
            return char.IsLetterOrDigit(letter) || letter == HtmlTag.hypen;
        }

        protected bool CreateTag(string tag, int htmlStart, int textStart, int textEnd, int htmlEnd,
            HtmlNode parent, out HtmlNode node)
        {
            node = null;

            if (htmlEnd != -1 && htmlEnd <= htmlStart)
            {
                return false;
            }

            if (textEnd != -1 && textEnd < textStart)
            {
                return false;
            }

            node = new HtmlNode(tag, htmlStart, textStart, textEnd, htmlEnd, context.HtmlContext, parent);

            if (context.PreviousNode != null)
            {
                node.SetPreviousNode(context.PreviousNode);
                context.PreviousNode.SetNextNode(node);
            }

            context.PreviousNode = node;

            return parent == null;
        }

        protected bool AssignNextAnalyzer(int position, HtmlNode node)
        {
            IOpenTag openTag;
            ICloseTag closeTag;
            bool assigned = false;

            if (IsOpenTag(position, out openTag))
            {
                context.SetAnalyzer(openTag.GetAnalyzer(position, node));
                assigned = true;
            }
            else if (IsCloseTag(position, out closeTag))
            {
                closeTag.Init(position, node);
                context.SetAnalyzer(closeTag.GetAnalyzer());
                assigned = true;
            }

            return assigned;
        }

        protected HtmlAnalyzer(IAnalyzerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.context = context;
        }

        protected abstract bool ProcessHtml(int position, ref HtmlNode node);
       
        protected void TagCreated(string tag)
        {
            if (OnTagCreate != null)
            {
                OnTagCreate(tag);
            }
        }

        protected void InnerTagOpened(HtmlNode parentNode)
        {
            //New inner rows opened. So clearing the previous node.
            context.PreviousNode = null;
        }

        protected void InnerTagClosed(HtmlNode currentNode)
        {
            //Closed a row of nodes. So current node assigned as previous node.
            context.PreviousNode = currentNode;
        }
       
        public bool Process(int position, ref HtmlNode node)
        {
            bool tagCreated = ProcessHtml(position, ref node);
            
            return tagCreated;
        }
    }
}
