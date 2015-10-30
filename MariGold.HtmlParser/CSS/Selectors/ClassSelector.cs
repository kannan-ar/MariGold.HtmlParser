namespace MariGold.HtmlParser
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;

	internal sealed class ClassSelector : CSSelector, IAttachedSelector
	{
		private const string key = "class";

		private readonly Regex regex;
		
		private string currentSelector;
		private string selectorText;

		internal ClassSelector(ISelectorContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			context.AddAttachedSelector(this);
			
			this.context = context;
			regex = new Regex(@"^\.[-_]*([a-zA-Z]+[0-9_-]*)+");
		}

		internal override bool Prepare(string selector)
		{
			Match match = regex.Match(selector);
			
			this.currentSelector = string.Empty;
			this.selectorText = string.Empty;
			
			if (match.Success)
			{
				this.currentSelector = match.Value.Replace(".", string.Empty);
				this.selectorText = selector.Substring(match.Value.Length);
			}
			
			return match.Success;
		}

		internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
		{
			if (string.IsNullOrEmpty(selectorText) && IsValidNode(node))
			{
				ApplyStyle(node, htmlStyles);
			}
			else
			{
				context.ParseSelectorOrBehavior(this.selectorText, node, htmlStyles);
			}
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

			bool isValid = false;

			string className;

			if (node.Attributes.TryGetValue(key, out className))
			{
				if (string.Compare(currentSelector, className, true) == 0)
				{
					isValid = true;
				}
			}

			return isValid;
		}
		
		internal override void ApplyStyle(HtmlNode node, List<HtmlStyle> htmlStyles)
		{
			node.CopyHtmlStyles(htmlStyles, SelectorWeight.Class);
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
