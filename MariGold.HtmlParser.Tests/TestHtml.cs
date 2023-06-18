namespace MariGold.HtmlParser.Tests;

    using System;
    using System.Linq;
    using MariGold.HtmlParser;
    using Xunit;

    internal class TestHtml
{
	private readonly IHtmlNode node;

	internal TestHtml(IHtmlNode node)
	{
		this.node = node ?? throw new ArgumentNullException("node");
	}

	internal TestHtml IsNotNull()
	{
		Assert.NotNull(node);

		return new TestHtml(node);
	}

	internal TestHtml AreEqual(string tag, string text, string html)
	{
		Assert.Equal(tag, node.Tag);
		Assert.Equal(text, node.InnerHtml);
		Assert.Equal(html, node.Html);

		return new TestHtml(node);
	}

	internal TestHtml IsNull()
	{
		Assert.Null(node);

		return new TestHtml(node);
	}

	internal TestHtml Parent()
	{
		return new TestHtml(node.Parent);
	}

	internal TestHtml Childeren(int index)
	{
		return new TestHtml(node.Children.ElementAt(index));
	}

	internal TestHtml HasChildrenCount(int count)
	{
		Assert.Equal(count, node.Children.Count());

		return new TestHtml(node);
	}
}
