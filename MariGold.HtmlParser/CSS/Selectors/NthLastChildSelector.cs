namespace MariGold.HtmlParser
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	
	internal sealed class NthLastChildSelector : CSSelector, IAttachedSelector
	{
		private readonly Regex regex;
		
		private string selectorText;
		private int position;
		
		internal NthLastChildSelector(ISelectorContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			context.AddAttachedSelector(this);
			this.context = context;
			
			regex = new Regex("^(:nth-last-child\\(\\d+\\))|(:nth-last-of-type\\(\\d+\\))");
		}
		
		internal override bool Prepare(string selector)
		{
			Match match = regex.Match(selector);
			
			this.selectorText = string.Empty;
			this.position = -1;
			
			if (match.Success)
			{
				this.selectorText = selector.Substring(match.Value.Length);
				
				int value;
				
				if (int.TryParse(new Regex("\\d+").Match(match.Value).Value, out value))
				{
					this.position = value;
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
			
			if (this.position == -1)
			{
				return false;
			}
			
			if (node.Parent == null)
			{
				return false;
			}
			
			HtmlNode parent = node.GetParent();
			int childrenCount = parent.GetChildren().Count;
			
			if (childrenCount < this.position)
			{
				return false;
			}
			
			
			return node.GetParent().GetChild(childrenCount - this.position) == node;
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
