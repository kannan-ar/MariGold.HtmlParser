namespace MariGold.HtmlParser
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	
	internal sealed class ApplyLastChild : CSSBehavior
	{
		private readonly Regex regex;
		
		private string selectorText;
		
		internal ApplyLastChild(ISelectorContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			this.context = context;
			
			regex = new Regex("^(:last-child)|(:last-of-type)");
		}
		
		internal override bool IsValidBehavior(string selectorText)
		{
			this.selectorText = string.Empty;

			Match match = regex.Match(selectorText);

			if (match.Success)
			{
				this.selectorText = selectorText.Substring(match.Value.Length);
			}

			return match.Success;
		}
		
		internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
		{
			if (node != null && node.Parent != null)
			{
				HtmlNode lastChild = null;
				
				foreach (HtmlNode child in node.Parent.Children)
				{
					if (string.Compare(node.Tag, child.Tag, true) == 0)
					{
						lastChild = child;
					}
				}
				
				if (lastChild != null && lastChild == node)
				{
					if (string.IsNullOrEmpty(this.selectorText))
					{
						if (selector == null)
						{
							throw new InvalidOperationException("Current CSS Selector is null");
						}

						selector.ApplyStyle(node, htmlStyles);
					}
					else
					{
						ParseSelectorOrBehavior(this.selectorText, node, htmlStyles);
					}
				}
			}
		}
	}
}
