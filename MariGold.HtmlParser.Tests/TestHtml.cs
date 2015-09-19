namespace MariGold.HtmlParser.Tests
{
    using System;
    using NUnit.Framework;
    using MariGold.HtmlParser;

    internal class TestHtml
    {
        private readonly HtmlNode node;

        internal TestHtml(HtmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            this.node = node;
        }

        internal TestHtml IsNotNull()
        {
            Assert.IsNotNull(node);

            return new TestHtml(node);
        }

        internal TestHtml AreEqual(string tag, string text, string html)
        {
            Assert.AreEqual(tag, node.Tag);
            Assert.AreEqual(text, node.InnerHtml);
            Assert.AreEqual(html, node.Html);

            return new TestHtml(node);
        }

        internal TestHtml IsNull()
        {
            Assert.IsNull(node);

            return new TestHtml(node);
        }

        internal TestHtml Parent()
        {
            return new TestHtml(node.Parent);
        }

        internal TestHtml Childeren(int index)
        {
            return new TestHtml(node.Children[index]);
        }

        internal TestHtml HasChildrenCount(int count)
        {
            Assert.AreEqual(count, node.Children.Count);

            return new TestHtml(node);
        }
    }
}
