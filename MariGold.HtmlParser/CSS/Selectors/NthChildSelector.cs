namespace MariGold.HtmlParser
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	
	internal sealed class NthChildSelector : CSSelector, IAttachedSelector
	{
		private readonly Regex regex;
		
		private string selectorText;
		private int position;
		
		internal NthChildSelector(ISelectorContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			context.AddAttachedSelector(this);
			this.context = context;
			
			regex = new Regex("^:nth-child\\(\\d+\\)");
		}
		
		private void ApplyToChild(HtmlNode node, List<HtmlStyle> htmlStyles)
		{
			if (node == null)
			{
				return;
			}
			
			if (!node.HasChildren)
			{
				return;
			}
			
			if (node.GetChildren().Count < this.position)
			{
				return;
			}
			
			ApplyStyle(node.GetChild(this.position - 1), htmlStyles);
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
			
			if (node.Parent.Children.Count() < this.position)
			{
				return false;
			}
			
			return node.Parent.Children.ElementAt(this.position - 1) == node;
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
			/*if (string.IsNullOrEmpty(this.selectorText))
			{
				ApplyToChild(node, htmlStyles);
			}
			else
			{
				context.ParseBehavior(this.selectorText, node, htmlStyles);
			}*/
			
			Parse(node, htmlStyles);
		}
	}
}
