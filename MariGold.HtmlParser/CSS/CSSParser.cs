namespace MariGold.HtmlParser.CSS
{
	using System;
	using System.Net;
	using System.Collections.Generic;
	using System.Linq;

	internal sealed class CSSParser
	{
		private const char openBrace = '{';
		private const char closeBrace = '}';

		private int ParseSelector(int position, string style, out string selectorText)
		{
			selectorText = string.Empty;

			int openBraceIndex = style.IndexOf(openBrace, position);

			if (openBraceIndex > position)
			{
				selectorText = style.Substring(position, openBraceIndex - position).Trim();
			}

			return openBraceIndex;
		}

		private HtmlStyle CreateHtmlStyleFromRule(string styleName, string value, SelectorWeight weight)
		{
			styleName = styleName.Trim().Replace("\"", string.Empty).Replace("'", string.Empty);
			value = value.Trim();
			bool important = false;

			//Replace this with regular expression
			int importantIndex = value.IndexOf("!important");

			if (importantIndex > -1)
			{
				important = true;
				value = value.Replace("!important", string.Empty).Trim();
			}

			return new HtmlStyle(styleName, value, important, weight);
		}

		private int ParseHtmlStyles(int position, string styleText, out List<HtmlStyle> htmlStyles)
		{
			htmlStyles = new List<HtmlStyle>();

			int closeBraceIndex = styleText.IndexOf(closeBrace, position);

			if (closeBraceIndex > position)
			{
				string styles = styleText.Substring(position + 1, closeBraceIndex - position - 1);

				if (!string.IsNullOrEmpty(styles))
				{
					htmlStyles = ParseRules(styles, SelectorWeight.None).ToList();
				}
			}

			//+1 to advance to next location
			return closeBraceIndex + 1;
		}

		private string ExtractStylesFromLink(string url)
		{
			string styles = string.Empty;

			using (WebClient client = new WebClient())
			{
				styles = client.DownloadString(url);
			}

			return styles;
		}

		private void ParseCSS(string style, StyleSheet styleSheet)
		{
			int eof = style.Length;
			int position = 0;

			while (position < eof)
			{
				string selectorText;

				int bracePosition = ParseSelector(position, style, out selectorText);

				if (bracePosition > position && selectorText != string.Empty)
				{
					List<HtmlStyle> htmlStyles;
					//Returning close brace index
					bracePosition = ParseHtmlStyles(bracePosition, style, out htmlStyles);

					styleSheet.Add(selectorText, htmlStyles);
				}

				if (bracePosition == -1)
				{
					break;
				}

				position = bracePosition;
			}
		}

		private IEnumerable<HtmlStyle> ParseRules(string styleText, SelectorWeight weight)
		{
			string[] styleSet = styleText.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

			foreach (string style in styleSet)
			{
				string styleNode = style.Trim();

				if (!string.IsNullOrEmpty(styleNode))
				{
					string[] nodeSet = styleNode.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

					if (nodeSet != null && nodeSet.Length > 1)
					{
						yield return CreateHtmlStyleFromRule(nodeSet[0], nodeSet[1], weight);
					}
				}
			}
		}

		private void TravelParseHtmlNodes(HtmlNode node, StyleSheet styleSheet)
		{
			string style = string.Empty;

			if (string.Compare(node.Tag, HtmlTag.STYLE, true) == 0)
			{
				style = node.InnerHtml == null ? string.Empty : node.InnerHtml.Trim();
			}
			else if (string.Compare(node.Tag, HtmlTag.LINK, true) == 0)
			{
				string url;

				if (node.Attributes.TryGetValue("href", out url))
				{
					style = ExtractStylesFromLink(url);
				}
			}

			if (!string.IsNullOrEmpty(style))
			{
				ParseCSS(style, styleSheet);
			}

			foreach (HtmlNode n in node.Children)
			{
				TravelParseHtmlNodes(n, styleSheet);
			}
		}

		internal StyleSheet ParseStyleSheet(HtmlNode node)
		{
			StyleSheet styleSheet = new StyleSheet(new SelectorContext());

			TravelParseHtmlNodes(node, styleSheet);

			return styleSheet;
		}

		internal void InterpretStyles(StyleSheet styleSheet, HtmlNode htmlNode)
		{
			string style;

			if (!HtmlStyle.IsNonStyleElement(htmlNode.Tag))
			{
				if (htmlNode.Attributes.TryGetValue("style", out style))
				{
					htmlNode.AddStyles(ParseRules(style, SelectorWeight.Inline));
				}

				styleSheet.Parse(htmlNode);
			}

			foreach (HtmlNode node in htmlNode.Children)
			{
				InterpretStyles(styleSheet, node);
			}
			
			if (htmlNode.Next != null)
			{
				InterpretStyles(styleSheet, htmlNode.Next);
			}
		}
	}
}
