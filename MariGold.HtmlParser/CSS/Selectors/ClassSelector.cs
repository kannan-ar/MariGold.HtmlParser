namespace MariGold.HtmlParser
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;

	internal sealed class ClassSelector : CSSelector
	{
		private const string key = "class";

		private readonly Regex regex;
		private readonly string currentSelector;
		private readonly string selectorText;

		internal ClassSelector(ISelectorContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			this.context = context;
			regex = new Regex(@"^\.[-_]*([a-zA-Z]+[0-9_-]*)+$");
		}

		internal ClassSelector(string currentSelector, string selectorText, ISelectorContext context)
		{
			this.currentSelector = currentSelector;
			this.selectorText = selectorText;
			this.context = context;
		}

		private void ApplyIfMatch(HtmlNode node, List<HtmlStyle> htmlStyles)
		{
			if (IsValidNode(node))
			{
				ApplyStyle(node, htmlStyles);
			}
		}

		internal override CSSelector Parse(string selector)
		{
			Match match = regex.Match(selector);

			if (match.Success)
			{
				string trimmedSelector = selector.Substring(match.Value.Length);

				return new ClassSelector(
					match.Value.Replace(".", string.Empty), trimmedSelector, context);
			}
			else
			{
				return PassToSuccessor(selector);
			}
		}

		internal override void Parse(HtmlNode node, List<HtmlStyle> htmlStyles)
		{
			if (string.IsNullOrEmpty(selectorText))
			{
				ApplyIfMatch(node, htmlStyles);
			}
			else
			{
				ParseBehaviour(selectorText, node, htmlStyles);
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
	}
}
