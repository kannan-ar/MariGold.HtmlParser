namespace MariGold.HtmlParser
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	
	internal sealed class OnlyChildSelector : CSSelector, IAttachedSelector
	{
		private readonly Regex regex;
		
		private string selectorText;
		
		internal OnlyChildSelector(ISelectorContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			context.AddAttachedSelector(this);
			this.context = context;
			
			regex = new Regex("^:only-child");
		}
		
		internal override bool Prepare(string selector)
		{
			Match match = regex.Match(selector);
			
			this.selectorText = string.Empty;
			
			if (match.Success)
			{
				this.selectorText = selector.Substring(match.Value.Length);
			}
			
			return match.Success;
		}
		
		internal override bool IsValidNode(HtmlNode node)
		{
			if (node == null)
			{
				return false;
			}
			
			if (node.Parent == null)
			{
				return false;
			}
			
			bool isValid = true;
			
			foreach (HtmlNode child in node.Parent.Children)
			{
				//There is a non text node other than current node. So the selector is not valid
				if (child != node && child.Tag != HtmlTag.TEXT)
				{
					isValid = false;
					break;
				}
			}
			
			return isValid;
		}
		
		internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
		{
			if (IsValidNode(node))
			{
				if (string.IsNullOrEmpty(this.selectorText))
				{
					ApplyStyle(node, htmlStyles);
				}
				else
				{
					context.ParseBehavior(this.selectorText, node, htmlStyles);
				}
			}
		}
		
		internal override void ApplyStyle(HtmlNode node, List<HtmlStyle> htmlStyles)
		{
			node.CopyHtmlStyles(htmlStyles, SelectorWeight.Child);
		}
		
		bool IAttachedSelector.Prepare(string selector)
		{
			return Prepare(selector);
		}
		
		bool IAttachedSelector.IsValidNode(HtmlNode node)
		{
			return IsValidNode(node);
		}
		
		void IAttachedSelector.Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
		{
			Parse(node, htmlStyles);
		}
	}
}
