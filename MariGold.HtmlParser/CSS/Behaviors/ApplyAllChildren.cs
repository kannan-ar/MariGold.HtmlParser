namespace MariGold.HtmlParser
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;

	internal sealed class ApplyAllChildren : CSSBehavior
	{
		private readonly Regex isValid;
		private readonly Regex parse;
        
		private string selectorText;

		internal ApplyAllChildren(ISelectorContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			this.context = context;

			//Atleast one space should occur preceeded by an occurance of an element selector
			isValid = new Regex(@"^\s+[^\s]+");
			//Parse all the space
			parse = new Regex(@"\s+");
		}

		private void ApplyStyle(CSSelector nextSelector, HtmlNode node, List<HtmlStyle> htmlStyles)
		{
			if (nextSelector.IsValidNode(node))
			{
				nextSelector.Parse(node, htmlStyles);
			}

			foreach (HtmlNode child in node.Children)
			{
				ApplyStyle(nextSelector, child, htmlStyles);
			}
		}

		internal override bool IsValidBehavior(string selectorText)
		{
			bool found = false;
			this.selectorText = string.Empty;

			if (isValid.IsMatch(selectorText))
			{
				found = true;

				Match match = parse.Match(selectorText);

				this.selectorText = selectorText.Substring(match.Length);
			}

			return found;
		}

		internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
		{
			CSSelector nextSelector;
			
			if (context.ParseSelector(this.selectorText, out nextSelector))
			{
				if (nextSelector != null)
				{
					foreach (HtmlNode child in node.Children)
					{
						ApplyStyle(nextSelector, child, htmlStyles);
					}
				}
			}
		}
	}
}
