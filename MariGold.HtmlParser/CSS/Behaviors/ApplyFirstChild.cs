namespace MariGold.HtmlParser
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	
	internal sealed class ApplyFirstChild : CSSBehavior
	{
		private readonly Regex regex;
		
		private string selectorText;
		
		internal ApplyFirstChild(ISelectorContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			this.context = context;
			
			regex = new Regex("^:first-child");
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
				foreach (HtmlNode child in node.Parent.Children)
				{
					//Find first child tag which matches the node's tag. The break statement will discard the loop after finding the first matching node.
					if (string.Compare(node.Tag, child.Tag, true) == 0)
					{
						//If the node is the first child, it will apply the styles.
						if (node == child)
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
								//ParseBehavior(this.selectorText, node, htmlStyles);
							}
						}
						
						break;
					}
				}
			}
		}
	}
}
