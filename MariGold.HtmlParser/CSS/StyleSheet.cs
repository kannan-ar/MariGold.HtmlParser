namespace MariGold.HtmlParser
{
	using System;
	using System.Collections.Generic;

	internal sealed class StyleSheet
	{
		private ISelectorContext context;
		private List<KeyValuePair<string, List<HtmlStyle>>> styles;

		internal StyleSheet(ISelectorContext context)
		{
			this.context = context;
			styles = new List<KeyValuePair<string, List<HtmlStyle>>>();
		}

		internal void Add(string selector, List<HtmlStyle> htmlStyles)
		{
			styles.Add(new KeyValuePair<string, List<HtmlStyle>>(selector, htmlStyles));
		}

		internal void Parse(HtmlNode node)
		{
			foreach (var style in styles)
			{
				foreach (CSSelector selector in context.Selectors)
				{
					if (selector.Prepare(style.Key))
					{
						if (selector.IsValidNode(node))
						{
							selector.Parse(node, style.Value);
						}
					}
				}
                
			}
		}
	}
}