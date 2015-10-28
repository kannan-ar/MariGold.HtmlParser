namespace MariGold.HtmlParser
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	
	internal sealed class NotSelector : CSSelector, IAttachedSelector
	{
		private readonly Regex regex;
		private readonly Regex elementName;
		
		private string selectorText;
		private string currentSelector;
		
		internal NotSelector(ISelectorContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			context.AddAttachedSelector(this);
			this.context = context;
			
			regex = new Regex("^:not\\([a-zA-Z]+[0-9]*\\)");
			elementName = new Regex("\\([a-zA-Z]+[0-9]*\\)");
		}
		
		private void ApplyToChildren(HtmlNode node, List<HtmlStyle> htmlStyles)
		{
			if (IsValidNode(node))
			{
				ApplyStyle(node, htmlStyles);
				
				foreach (HtmlNode child in node.Children)
				{
					ApplyToChildren(child, htmlStyles);
				}
			}
		}
		
		internal override bool Prepare(string selector)
		{
			Match match = regex.Match(selector);
			
			this.selectorText = string.Empty;
			this.currentSelector = string.Empty;
			
			if (match.Success)
			{
				this.selectorText = selector.Substring(match.Value.Length);
				
				Match elementMatch = elementName.Match(match.Value);
				
				if (elementMatch.Success)
				{
					this.currentSelector = elementMatch.Value.Replace("(", string.Empty).Replace(")", string.Empty);
				}
			}
			
			return match.Success;
		}
		
		internal override bool IsValidNode(HtmlNode node)
		{
			if (node == null)
			{
				return false;
			}
			
			if (string.IsNullOrEmpty(currentSelector))
			{
				return false;
			}
			
			return string.Compare(node.Tag, currentSelector, true) != 0;
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
		
		void IAttachedSelector.Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
		{
			if (string.IsNullOrEmpty(this.selectorText))
			{
				foreach (HtmlNode child in node.Children)
				{
					ApplyToChildren(child, htmlStyles);
				}
			}
			else
			{
				context.ParseBehavior(this.selectorText, node, htmlStyles);
			}
		}
	}
}
